import { DatabaseSync } from "node:sqlite";
import { randomUUID } from "node:crypto";
import { z } from "zod";
import type { Catalog } from "./catalog";
import { cashInRequestSchema, extractionRequestSchema, cashInFingerprint, extractionFingerprint,
  raidOperationKey, type CashInRequest, type ExtractionRequest, type RaidOperation } from "./raid-protocol";
import type { Receipt } from "./outbox";

export type RaidRequest = CashInRequest | ExtractionRequest;
export type RaidMoney = {
  key: string; appUserId: string; amountCents: number; operation: RaidOperation;
};
export interface RaidMoneyApi {
  byKey(key: string): Promise<Receipt | null>;
  moveRaidMoney(operation: RaidMoney): Promise<Receipt>;
}
export type SettlementProof = {
  operationKey: string; operation: RaidOperation; requestFingerprint: string;
  amountCents: number; transactionId: string | null; appUserId: string;
};
export interface RaidNativeBridge {
  // MUST execute validation + freeze atomically on the game thread. Authenticate
  // current Steam/session/map/raid/phase, exact cash/inventory/window and item
  // ownership. Freeze blocks spend/drop/death/value changes. Retrying this SAME
  // key and snapshot returns true without applying twice; changed snapshots fail.
  // After a process/map restart with no original raid, MUST return false.
  validateAndFreeze(key: string, operation: RaidOperation, request: RaidRequest): Promise<boolean>;
  // MUST verify proof over trusted backend transport, exact key/fingerprint and
  // frozen original session. Idempotent for identical key+proof. Cash-in adds once;
  // extract consumes frozen cash/items and marks extracted once. No receipt text.
  // False/throw leaves the durable financial receipt pending, NEVER auto-refund.
  finalize(proof: SettlementProof, request: RaidRequest): Promise<boolean>;
}
type RecordRow = {
  key: string; fingerprint: string; appUserId: string; grantId: string;
  operation: RaidOperation; request: RaidRequest; amountCents: number;
  state: "prepared" | "frozen" | "paid" | "settled" | "manual";
  receipt?: Receipt; zeroReceiptId?: string;
};
const uuid = z.string().uuid();
export class RaidLedger {
  private db: DatabaseSync;
  constructor(path: string, private serverId: string, private catalog: Catalog) {
    this.db = new DatabaseSync(path);
    this.db.exec(`PRAGMA journal_mode=WAL; PRAGMA synchronous=FULL; PRAGMA busy_timeout=5000;
      CREATE TABLE IF NOT EXISTS bindings (steam TEXT PRIMARY KEY, app_user TEXT NOT NULL UNIQUE, grant_id TEXT NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS raid_operations (key TEXT PRIMARY KEY, data TEXT NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS raid_item_claims (instance TEXT PRIMARY KEY, operation_key TEXT NOT NULL) STRICT;`);
    raidOperationKey(serverId, "cashin", "00000000000000000000000000000000", "76561198000000001");
  }
  close() { this.db.close(); }
  private get(key: string): RecordRow {
    const row = this.db.prepare("SELECT data FROM raid_operations WHERE key=?").get(key);
    if (!row) throw new Error("Unknown operation");
    return JSON.parse(String(row.data)) as RecordRow;
  }
  private transition(row: RecordRow, next: RecordRow) {
    return this.db.prepare("UPDATE raid_operations SET data=? WHERE key=? AND data=?")
      .run(JSON.stringify(next), row.key, JSON.stringify(row)).changes === 1;
  }
  private amount(request: ExtractionRequest) {
    let amount = request.cashCents;
    for (const item of request.inventory) {
      const catalog = this.catalog.get(item.catalogId);
      if (!catalog || (item.kind === "gun" && !["weapon", "pistol", "rifle", "smg", "shotgun", "sniper", "heavy", "machinegun", "lmg"].includes(catalog.kind)) ||
        (item.kind === "unused_utility" && !["grenade", "utility"].includes(catalog.kind)))
        throw new Error("Non-refundable catalog item");
      amount += catalog.priceCents; // SPEC12 full catalog price, never caller supplied.
      if (!Number.isSafeInteger(amount)) throw new Error("Settlement precision exceeded");
    }
    return amount;
  }
  private prepare(operation: RaidOperation, input: unknown) {
    const request = operation === "cashin" ? cashInRequestSchema.parse(input) : extractionRequestSchema.parse(input);
    const fingerprint = operation === "cashin" ? cashInFingerprint(request) : extractionFingerprint(request);
    const key = raidOperationKey(this.serverId, operation, request.raidId, request.session.steamId64);
    this.db.exec("BEGIN IMMEDIATE");
    try {
      const old = this.db.prepare("SELECT data FROM raid_operations WHERE key=?").get(key);
      if (old) {
        const row = JSON.parse(String(old.data)) as RecordRow;
        if (row.fingerprint !== fingerprint) throw new Error("Idempotency conflict");
        this.db.exec("COMMIT"); return row;
      }
      const binding = this.db.prepare("SELECT app_user,grant_id FROM bindings WHERE steam=?").get(request.session.steamId64);
      if (!binding) throw new Error("Link account first");
      const appUserId = uuid.parse(binding.app_user), grantId = uuid.parse(binding.grant_id);
      const amountCents = operation === "cashin" ? (request as CashInRequest).amountCents : this.amount(request as ExtractionRequest);
      const row: RecordRow = { key, fingerprint, appUserId, grantId, operation, request, amountCents, state: "prepared" };
      // Globally unique inventory instances cannot fund another extraction or raid.
      if (operation === "extract") for (const item of (request as ExtractionRequest).inventory)
        this.db.prepare("INSERT INTO raid_item_claims VALUES (?,?)").run(item.instanceId, key);
      this.db.prepare("INSERT INTO raid_operations VALUES (?,?)").run(key, JSON.stringify(row));
      this.db.exec("COMMIT"); return row;
    } catch (error) { this.db.exec("ROLLBACK"); throw error; }
  }
  private verify(row: RecordRow, receipt: Receipt) {
    uuid.parse(receipt.transaction_id);
    if (receipt.app_user_id !== row.appUserId || receipt.grant_id !== row.grantId || receipt.status !== "committed" ||
      receipt.amount_cents !== row.amountCents || receipt.external_reference !== row.key ||
      receipt.direction !== (row.operation === "cashin" ? "debit" : "credit") ||
      receipt.original_transaction_id !== undefined ||
      (row.operation === "extract" ? receipt.credit_reason !== "mint" : receipt.credit_reason !== undefined))
      throw new Error("Canonical receipt mismatch");
  }
  proof(input: unknown): SettlementProof & { state: string } {
    const value = z.object({ operationKey: z.string().min(8).max(128), requestFingerprint: z.string().regex(/^[a-f0-9]{64}$/) }).strict().parse(input);
    const row = this.get(value.operationKey);
    if (row.fingerprint !== value.requestFingerprint || !["paid", "settled"].includes(row.state))
      throw new Error("No matching durable settlement");
    if (row.receipt) this.verify(row, row.receipt);
    else if (row.amountCents !== 0 || !row.zeroReceiptId) throw new Error("Missing canonical receipt");
    return { operationKey: row.key, operation: row.operation, requestFingerprint: row.fingerprint,
      amountCents: row.amountCents, transactionId: row.receipt?.transaction_id ?? null,
      appUserId: row.appUserId, state: row.state };
  }
  async execute(operation: RaidOperation, input: unknown, api: RaidMoneyApi, native: RaidNativeBridge): Promise<SettlementProof & { state: string }> {
    let row = this.prepare(operation, input);
    if (row.state === "manual") throw new Error("Manual reconciliation required");
    if (row.state === "prepared") {
      if (!await native.validateAndFreeze(row.key, row.operation, row.request)) throw new Error("Native session or snapshot rejected");
      this.transition(row, { ...row, state: "frozen" }); row = this.get(row.key);
    }
    if (row.state === "frozen") {
      // First reconcile, even if original session has gone. NEVER change key or
      // silently retry a new identity after response loss or revoked consent.
      let receipt = row.amountCents === 0 ? null : await api.byKey(row.key);
      if (!receipt && row.amountCents > 0) {
        if (!await native.validateAndFreeze(row.key, row.operation, row.request)) throw new Error("Native session no longer valid");
        receipt = await api.moveRaidMoney({ key: row.key, appUserId: row.appUserId, amountCents: row.amountCents, operation: row.operation });
      }
      try { if (receipt) this.verify(row, receipt); }
      catch (error) { this.transition(row, { ...row, state: "manual" }); throw error; }
      this.transition(row, { ...row, state: "paid", ...(receipt ? { receipt } : { zeroReceiptId: randomUUID() }) });
      row = this.get(row.key);
    }
    const proof: SettlementProof = {
      operationKey: row.key, operation: row.operation, requestFingerprint: row.fingerprint,
      amountCents: row.amountCents, transactionId: row.receipt?.transaction_id ?? null, appUserId: row.appUserId,
    };
    if (row.state === "paid") {
      // Durable receipt precedes native completion. Retry requires a real
      // idempotent native adapter, never re-give on arbitrary response strings.
      if (!await native.finalize(proof, row.request)) throw new Error("Native completion unresolved");
      this.transition(row, { ...row, state: "settled" }); row = this.get(row.key);
    }
    return { ...proof, state: row.state };
  }
}
