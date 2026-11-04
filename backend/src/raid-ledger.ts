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
  private running = new Map<string, Promise<unknown>>();
  constructor(path: string, private serverId: string, private catalog: Catalog) {
    this.db = new DatabaseSync(path);
    this.db.exec(`PRAGMA journal_mode=WAL; PRAGMA synchronous=FULL; PRAGMA busy_timeout=5000;
      CREATE TABLE IF NOT EXISTS bindings (steam TEXT PRIMARY KEY, app_user TEXT NOT NULL UNIQUE, grant_id TEXT NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS raid_operations (key TEXT PRIMARY KEY, data TEXT NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS raid_native_journal (operation_key TEXT PRIMARY KEY, data TEXT NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS raid_reconciliation_audit (id INTEGER PRIMARY KEY, operation_key TEXT NOT NULL, data TEXT NOT NULL) STRICT;
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
  private async serialized<T>(key: string, action: () => Promise<T>): Promise<T> {
    const prior = this.running.get(key) ?? Promise.resolve();
    const current = prior.catch(() => {}).then(action);
    this.running.set(key, current);
    try { return await current; }
    finally { if (this.running.get(key) === current) this.running.delete(key); }
  }
  private journal(key: string): NativeJournal | undefined {
    const raw = this.db.prepare("SELECT data FROM raid_native_journal WHERE operation_key=?").get(key);
    return raw ? JSON.parse(String(raw.data)) : undefined;
  }
  private bound(input: unknown) {
    const identity = journalIdentity.parse(input), row = this.get(identity.operationKey);
    if (identity.requestFingerprint !== row.fingerprint) throw new Error("Journal identity conflict");
    return row;
  }
  journalRead(input: unknown) {
    const row = this.bound(input);
    let proof: (SettlementProof & {state: string}) | undefined;
    if (["paid", "settled"].includes(row.state)) proof = this.proof(input);
    return { operationKey: row.key, requestFingerprint: row.fingerprint, operation: row.operation,
      request: row.request, financialState: row.state, proof: proof ?? null,
      journal: this.journal(row.key) ?? null,
      disposition: this.journal(row.key)?.terminal ?? "unresolved",
      audit: this.db.prepare("SELECT data FROM raid_reconciliation_audit WHERE operation_key=? ORDER BY id")
        .all(row.key).map(entry => JSON.parse(String(entry.data))) };
  }
  journalPhase(input: unknown) {
    const value = phaseInput.parse(input), row = this.bound({operationKey: value.operationKey, requestFingerprint: value.requestFingerprint});
    this.db.exec("BEGIN IMMEDIATE");
    try {
      const old = this.journal(row.key);
      if (old?.terminal) throw new Error("Terminal quarantine");
      if (old && old.nativeSnapshot !== value.nativeSnapshot) throw new Error("Immutable snapshot conflict");
      const index = phases.indexOf(value.phase), oldIndex = old ? phases.indexOf(old.phase!) : -1;
      if (index > 0) {
        const canonical = this.proof({operationKey: value.operationKey, requestFingerprint: value.requestFingerprint});
        const {state: _state, ...expected} = canonical;
        if (JSON.stringify(proofSchema.parse(value.proof)) !== JSON.stringify(proofSchema.parse(expected)))
          throw new Error("Original proof required");
      } else if (value.proof !== undefined) throw new Error("Unexpected snapshot proof");
      if (index <= oldIndex) {
        this.db.exec("COMMIT"); return old!;
      }
      if (index !== oldIndex + 1) throw new Error("Journal phase out of order");
      const next: NativeJournal = { nativeSnapshot: value.nativeSnapshot, phase: value.phase,
        ...(value.proof ? {proof: value.proof} : {}), updatedAt: new Date().toISOString() };
      this.db.prepare("INSERT INTO raid_native_journal VALUES (?,?) ON CONFLICT(operation_key) DO UPDATE SET data=excluded.data")
        .run(row.key, JSON.stringify(next));
      this.audit(row.key, {kind: "native_phase", phase: value.phase, evidence: value.evidence, at: next.updatedAt});
      this.db.exec("COMMIT"); return next;
    } catch(error) { this.db.exec("ROLLBACK"); throw error; }
  }
  private audit(key: string, data: unknown) {
    this.db.prepare("INSERT INTO raid_reconciliation_audit(operation_key,data) VALUES (?,?)").run(key, JSON.stringify(data));
  }
  quarantineAck(input: unknown) {
    const value = ackInput.parse(input);
    const identity = {operationKey: value.operationKey, requestFingerprint: value.requestFingerprint};
    const row = this.bound(identity), journal = this.journal(row.key);
    if (!journal || journal.revocationEpoch !== value.revocationEpoch ||
      JSON.stringify(value.session) !== JSON.stringify(row.request.session)) throw new Error("Revocation identity conflict");
    if (journal.terminal === "quarantined") {
      if (JSON.stringify(journal.ack) !== JSON.stringify(value)) throw new Error("Conflicting quarantine acknowledgement");
      return this.journalRead(identity);
    }
    if (journal.terminal !== "quarantine_requested") throw new Error("No requested quarantine");
    this.db.exec("BEGIN IMMEDIATE");
    try {
      const next = {...journal, terminal: "quarantined", ack: value, updatedAt: new Date().toISOString()};
      this.db.prepare("UPDATE raid_native_journal SET data=? WHERE operation_key=?").run(JSON.stringify(next), row.key);
      this.audit(row.key, {kind: "native_quarantine_ack", ...value, at: next.updatedAt});
      this.db.exec("COMMIT");
    } catch(error) {this.db.exec("ROLLBACK"); throw error;}
    return this.journalRead(identity);
  }
  async quarantine(input: unknown, api: RaidMoneyApi) {
    const value = quarantineInput.parse(input);
    const identity = {operationKey: value.operationKey, requestFingerprint: value.requestFingerprint};
    let row = this.bound(identity);
    // Revocation MUST precede awaits: a delayed finalize is not protected by a process queue.
    this.db.exec("BEGIN IMMEDIATE");
    try {
      const old = this.journal(row.key);
      if (!old?.terminal) {
        const next = {...old, terminal: "quarantine_requested", revocationEpoch: randomUUID(),
          updatedAt: new Date().toISOString()};
        this.db.prepare("INSERT INTO raid_native_journal VALUES (?,?) ON CONFLICT(operation_key) DO UPDATE SET data=excluded.data")
          .run(row.key, JSON.stringify(next));
        this.audit(row.key, {kind: "quarantine_requested", ...value, revocationEpoch: next.revocationEpoch, at: next.updatedAt});
      }
      this.db.exec("COMMIT");
    } catch(error) {this.db.exec("ROLLBACK"); throw error;}
    // No financial mutation. A denied/mismatched lookup leaves durable revocation requested.
    const receipt = await api.byKey(row.key);
    row = this.get(row.key);
    if (receipt) this.verify(row, receipt);
    if (row.receipt && receipt && row.receipt.transaction_id !== receipt.transaction_id)
      throw new Error("Canonical transaction conflict");
    this.db.exec("BEGIN IMMEDIATE");
    try {
      if (receipt && !row.receipt && !this.transition(row, {...row, receipt, state: "paid"}))
        throw new Error("Concurrent financial transition");
      this.audit(row.key, {kind: "quarantine_original_key_lookup", ...value, at: new Date().toISOString(),
        originalKeyLookup: receipt ? {status: "committed", receipt} : {status: "absent_not_authority"},
        retainedProof: row.receipt ?? null});
      this.db.exec("COMMIT");
    } catch(error) {this.db.exec("ROLLBACK"); throw error;}
    return this.journalRead(identity);
  }
  private requireNotRevoked(key: string) {
    if (this.journal(key)?.terminal) throw new Error("Terminal quarantine or revocation requested");
  }
  async execute(operation: RaidOperation, input: unknown, api: RaidMoneyApi, native: RaidNativeBridge): Promise<SettlementProof & { state: string }> {
    const request = operation === "cashin" ? cashInRequestSchema.parse(input) : extractionRequestSchema.parse(input);
    const key = raidOperationKey(this.serverId, operation, request.raidId, request.session.steamId64);
    return this.serialized(key, () => this.executeOriginal(operation, input, api, native));
  }
  private async executeOriginal(operation: RaidOperation, input: unknown, api: RaidMoneyApi, native: RaidNativeBridge): Promise<SettlementProof & { state: string }> {
    let row = this.prepare(operation, input);
    this.requireNotRevoked(row.key);
    if (row.state === "manual") throw new Error("Manual reconciliation required");
    if (row.state === "prepared") {
      if (!await native.validateAndFreeze(row.key, row.operation, row.request)) throw new Error("Native session or snapshot rejected");
      this.requireNotRevoked(row.key);
      this.transition(row, { ...row, state: "frozen" }); row = this.get(row.key);
    }
    if (row.state === "frozen") {
      // First reconcile, even if original session has gone. NEVER change key or
      // silently retry a new identity after response loss or revoked consent.
      let receipt = row.amountCents === 0 ? null : await api.byKey(row.key);
      this.requireNotRevoked(row.key);
      if (!receipt && row.amountCents > 0) {
        if (!await native.validateAndFreeze(row.key, row.operation, row.request)) throw new Error("Native session no longer valid");
        this.requireNotRevoked(row.key);
        receipt = await api.moveRaidMoney({ key: row.key, appUserId: row.appUserId, amountCents: row.amountCents, operation: row.operation });
      }
      try { if (receipt) this.verify(row, receipt); }
      catch (error) { this.transition(row, { ...row, state: "manual" }); throw error; }
      this.transition(row, { ...row, state: "paid", ...(receipt ? { receipt } : { zeroReceiptId: randomUUID() }) });
      row = this.get(row.key);
      this.requireNotRevoked(row.key);
    }
    const proof: SettlementProof = {
      operationKey: row.key, operation: row.operation, requestFingerprint: row.fingerprint,
      amountCents: row.amountCents, transactionId: row.receipt?.transaction_id ?? null, appUserId: row.appUserId,
    };
    if (row.state === "paid") {
      // Durable receipt precedes native completion. Retry requires a real
      // idempotent native adapter, never re-give on arbitrary response strings.
      this.requireNotRevoked(row.key);
      if (!await native.finalize(proof, row.request)) throw new Error("Native completion unresolved");
      this.requireNotRevoked(row.key);
      this.transition(row, { ...row, state: "settled" }); row = this.get(row.key);
    }
    return { ...proof, state: row.state };
  }
}

const journalIdentity = z.object({operationKey: z.string().min(8).max(128),
  requestFingerprint: z.string().regex(/^[a-f0-9]{64}$/)}).strict();
const proofSchema = z.object({operationKey: z.string(), operation: z.enum(["cashin", "extract"]),
  requestFingerprint: z.string(), amountCents: z.number().int().nonnegative().safe(),
  transactionId: z.string().uuid().nullable(), appUserId: z.string().uuid()}).strict();
const phases = ["snapshot", "consumption_intent", "consumed", "cleanup_complete"] as const;
const phaseInput = journalIdentity.extend({nativeSnapshot: z.string().min(1).max(131072),
  phase: z.enum(phases), proof: proofSchema.optional(), evidence: z.string().min(1).max(4096)}).strict();
const quarantineInput = journalIdentity.extend({operator: z.string().min(1).max(128),
  reason: z.string().min(1).max(1024), evidence: z.string().min(1).max(4096),
  isolationEvidence: z.string().min(1).max(4096)}).strict();
type NativeJournal = {nativeSnapshot?: string; phase?: typeof phases[number]; proof?: SettlementProof;
  terminal?: string; revocationEpoch?: string; ack?: z.infer<typeof ackInput>; updatedAt: string};

const ackInput = journalIdentity.extend({revocationEpoch: z.string().uuid(),
  session: cashInRequestSchema.shape.session,
  nativePhase: z.enum([...phases, "quarantined"]), isolationEvidence: z.string().min(1).max(4096)}).strict();
