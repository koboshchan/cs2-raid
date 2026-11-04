import Fastify from "fastify";
import { RaidLedger, type RaidMoneyApi, type RaidNativeBridge } from "./raid-ledger";
import { NativeHttpBridge } from "./native-http";
import { timingSafeEqual } from "node:crypto";
import { mkdirSync } from "node:fs";
import { join } from "node:path";
import { SsgClient, ApiError } from "./client";
import { loadCatalog, type Catalog } from "./catalog";
import { LinkStore, steamSchema } from "./links";
export function createServer(token: string, catalog: Catalog, links: Pick<LinkStore, "create" | "poll">,
  raid?: { ledger: RaidLedger; api: RaidMoneyApi; native: RaidNativeBridge },
  recovery?: {ledger: RaidLedger; api: RaidMoneyApi; adminToken?: string}) {
  if (token.length < 32) throw new Error("BRIDGE_TOKEN must contain at least 32 random characters");
  const recoveryStore = recovery ?? raid;
  const adminToken = recovery?.adminToken;
  if (adminToken !== undefined && (adminToken.length < 32 || adminToken === token))
    throw new Error("Recovery admin credential must be separate and at least 32 characters");
  const app = Fastify({ logger: false, bodyLimit: 196608 });
  app.addHook("onRequest", async (req, reply) => {
    reply.header("Cache-Control", "no-store");
    const admin = req.routeOptions.url === "/admin/reconciliation/quarantine";
    const expected = Buffer.from("Bearer " + (admin ? (adminToken ?? "") : token)), actual = Buffer.from(req.headers.authorization ?? "");
    if ((admin && !adminToken) || req.headers.origin !== undefined || actual.length !== expected.length || !timingSafeEqual(actual, expected))
      return reply.code(401).send({ error: "unauthorized" });
  });
  app.setErrorHandler((error, _req, reply) => {
    if (error instanceof ApiError) {
      if (error.retryAfterSeconds) reply.header("Retry-After", String(error.retryAfterSeconds));
      return reply.code(error.status).send({ error: error.code });
    }
    // Do not expose upstream bodies, credentials, device tokens, SQL or stack traces.
    return reply.code(400).send({ error: "invalid_request_or_link_state" });
  });
  app.get("/health", async () => ({ status: "ok", purchases: "disabled", nativeSessionBridge: raid ? "injected" : "not_ready", payouts: raid ? "configured" : "disabled" }));
  app.get("/catalog", async () => ({ items: [...catalog.values()].map(item => ({ ...item, defaultLimitBlocked: item.priceCents > 100000 })) }));
  const identity = (body: unknown) => steamSchema.parse(
    typeof body === "object" && body !== null && "steamId64" in body ? body.steamId64 : undefined);
  // Caller MUST be trusted server plugin using an engine-authenticated SteamID, never a client supplied ID.
  app.post("/link", async req => links.create(identity(req.body)));
  app.post("/link/poll", async req => links.poll(identity(req.body)));
  if (raid?.native instanceof NativeHttpBridge) {
    const native = raid.native;
    app.post("/native/next", async req => native.next(req.body));
    app.post("/native/reply", async req => native.reply(req.body));
    app.addHook("onClose", async () => native.close());
  }
  for (const route of ["/native/recovery", "/native/journal/phase"]) app.post(route, async (req, reply) => {
    if (!recoveryStore) return reply.code(503).send({error: "recovery_store_unavailable"});
    return recoveryStore.ledger.journalPhase(req.body);
  });
  app.post("/native/quarantine/ack", async (req, reply) => {
    if (!recoveryStore) return reply.code(503).send({error: "recovery_store_unavailable"});
    return recoveryStore.ledger.quarantineAck(req.body);
  });
  app.post("/native/journal/read", async (req, reply) => {
    if (!recoveryStore) return reply.code(503).send({error: "recovery_store_unavailable"});
    return recoveryStore.ledger.journalRead(req.body);
  });
  app.post("/admin/reconciliation/quarantine", async (req, reply) => {
    if (!recoveryStore) return reply.code(503).send({error: "recovery_store_unavailable"});
    return recoveryStore.ledger.quarantine(req.body, recoveryStore.api);
  });
  // Read-only persistent proof lookup. Never accepts a receipt or triggers native completion.
  app.post("/settlement/proof", async (req, reply) => {
    if (!recoveryStore) return reply.code(503).send({ error: "recovery_store_unavailable" });
    return recoveryStore.ledger.proof(req.body);
  });
  for (const route of ["/cashin", "/cashout", "/extract", "/payout", "/settlement"]) {
    app.post(route, async (req, reply) => {
      if (!raid) return reply.code(503).send({ error: "native_session_bridge_not_ready", moneyMovement: "disabled" });
      return raid.ledger.execute(route === "/cashin" || route === "/cashout" ? "cashin" : "extract", req.body, raid.api, raid.native);
    });
  }
  for (const route of ["/purchase", "/buy", "/debit", "/sell", "/sellback"])
    app.post(route, async (_req, reply) => reply.code(503).send({ error: "native_session_bridge_not_ready", moneyMovement: "disabled" }));
  return app;
}
async function main() {
  const dir = process.env.DATA_DIR ?? "/data";
  mkdirSync(dir, { recursive: true, mode: 0o700 });
  const catalog = loadCatalog(process.env.CATALOG_PATH ?? "/app/config/prices.json");
  const api = SsgClient.fromEnv(), serverId = process.env.SERVER_ID ?? "raid";
  const path = join(dir, "raid.sqlite");
  const links = new LinkStore(path, serverId, api);
  const ledger = new RaidLedger(path, serverId, catalog);
  const raid = process.env.RAID_FINANCIAL_ENABLED === "1"
    ? { ledger, api, native: new NativeHttpBridge() } : undefined;
  const server = createServer(process.env.BRIDGE_TOKEN ?? "", catalog, links, raid, {ledger, api, adminToken: process.env.RAID_RECONCILIATION_ADMIN_TOKEN});
  server.addHook("onClose", async () => ledger.close());
  server.addHook("onClose", async () => links.close());
  for (const signal of ["SIGTERM", "SIGINT"] as const) process.once(signal, () => { void server.close(); });
  await server.listen({ host: "0.0.0.0", port: Number(process.env.PORT ?? 3000) });
}
if (require.main === module) main().catch(() => { console.error("Backend startup failed; check protected configuration"); process.exitCode = 1; });
