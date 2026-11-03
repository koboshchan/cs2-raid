import Fastify from "fastify";
import { timingSafeEqual } from "node:crypto";
import { mkdirSync } from "node:fs";
import { join } from "node:path";
import { SsgClient, ApiError } from "./client";
import { loadCatalog, type Catalog } from "./catalog";
import { LinkStore, steamSchema } from "./links";
export function createServer(token: string, catalog: Catalog, links: Pick<LinkStore, "create" | "poll">) {
  if (token.length < 32) throw new Error("BRIDGE_TOKEN must contain at least 32 random characters");
  const app = Fastify({ logger: false, bodyLimit: 4096 });
  app.addHook("onRequest", async (req, reply) => {
    reply.header("Cache-Control", "no-store");
    const expected = Buffer.from("Bearer " + token), actual = Buffer.from(req.headers.authorization ?? "");
    if (req.headers.origin || actual.length !== expected.length || !timingSafeEqual(actual, expected))
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
  app.get("/health", async () => ({ status: "ok", purchases: "disabled", nativeSessionBridge: "not_ready", payouts: "disabled" }));
  app.get("/catalog", async () => ({ items: [...catalog.values()].map(item => ({ ...item, defaultLimitBlocked: item.priceCents > 100000 })) }));
  const identity = (body: unknown) => steamSchema.parse(
    typeof body === "object" && body !== null && "steamId64" in body ? body.steamId64 : undefined);
  // Caller MUST be trusted server plugin using an engine-authenticated SteamID, never a client supplied ID.
  app.post("/link", async req => links.create(identity(req.body)));
  app.post("/link/poll", async req => links.poll(identity(req.body)));
  for (const route of ["/purchase", "/buy", "/debit", "/sell", "/sellback", "/extract", "/payout"])
    app.post(route, async (_req, reply) => reply.code(503).send({ error: "native_session_bridge_not_ready", moneyMovement: "disabled" }));
  return app;
}
async function main() {
  const dir = process.env.DATA_DIR ?? "/data";
  mkdirSync(dir, { recursive: true, mode: 0o700 });
  const catalog = loadCatalog(process.env.CATALOG_PATH ?? "/app/config/prices.json");
  const links = new LinkStore(join(dir, "raid.sqlite"), process.env.SERVER_ID ?? "raid", SsgClient.fromEnv());
  const server = createServer(process.env.BRIDGE_TOKEN ?? "", catalog, links);
  server.addHook("onClose", async () => links.close());
  for (const signal of ["SIGTERM", "SIGINT"] as const) process.once(signal, () => { void server.close(); });
  await server.listen({ host: "0.0.0.0", port: Number(process.env.PORT ?? 3000) });
}
if (require.main === module) main().catch(() => { console.error("Backend startup failed; check protected configuration"); process.exitCode = 1; });
