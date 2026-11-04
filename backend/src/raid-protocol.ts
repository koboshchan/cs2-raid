import { createHash } from "node:crypto";
import { z } from "zod";
import { steamSchema } from "./links";

// Strict trusted-native wire contract. Schema validity is not authorization.
// RaidLedger additionally requires a real authenticated native freeze adapter.
export const MAX_RAID_CASH_IN_CENTS = 1_500_000;
const generation = z.string().min(1).max(128);
const session = z.object({
  steamId64: steamSchema,
  connectionId: generation,
  pawnGeneration: generation,
  mapGeneration: generation,
}).strict();
// Native MatchId uses Guid.ToString("N"); accept dashed UUIDs too, one identity.
const raidIdSchema = z.union([z.string().uuid(), z.string().regex(/^[a-fA-F0-9]{32}$/)])
  .transform(value => value.replace(/-/g, "").toLowerCase());
const raidIdentity = {
  raidId: raidIdSchema,
  session,
};
export const cashInRequestSchema = z.object({
  ...raidIdentity,
  // Integer SSG cents, not native HUD dollars. One immutable operation per raid/player.
  amountCents: z.number().int().positive().safe().max(MAX_RAID_CASH_IN_CENTS),
}).strict();
const inventoryItem = z.object({
  instanceId: z.string().uuid(),
  catalogId: z.string().regex(/^[a-z0-9_-]{1,48}$/),
  // Armor/helmet are not refundable. Utility must be unused at extraction.
  kind: z.enum(["gun", "unused_utility"]),
}).strict();
export const extractionRequestSchema = z.object({
  ...raidIdentity,
  extractionIntentId: generation,
  extractionWindowId: generation,
  cashCents: z.number().int().nonnegative().safe(),
  inventory: z.array(inventoryItem).max(128).refine(
    items => new Set(items.map(item => item.instanceId)).size === items.length,
    "Duplicate inventory instance",
  ),
}).strict();
export type CashInRequest = z.infer<typeof cashInRequestSchema>;
export type ExtractionRequest = z.infer<typeof extractionRequestSchema>;
export type RaidOperation = "cashin" | "extract";

function digest(value: unknown) {
  return createHash("sha256").update(JSON.stringify(value)).digest("hex");
}
// A retry cannot choose a new key by changing amount, session, window or inventory.
// A future durable journal MUST reject changed fingerprints under this same key.
export function raidOperationKey(serverId: string, operation: RaidOperation, raidId: string, steamId64: string) {
  z.string().regex(/^[a-zA-Z0-9_-]{1,32}$/).parse(serverId);
  z.enum(["cashin", "extract"]).parse(operation);
  const normalizedRaidId = raidIdSchema.parse(raidId);
  steamSchema.parse(steamId64);
  return "cs2.raid.v1." + operation + "." + digest([serverId, normalizedRaidId, steamId64]);
}
export function cashInFingerprint(input: unknown) {
  const r = cashInRequestSchema.parse(input);
  return digest(["cs2.raid.cashin.v1", r.raidId, r.session, r.amountCents]);
}
export function extractionFingerprint(input: unknown) {
  const r = extractionRequestSchema.parse(input);
  const items = [...r.inventory].sort((a, b) => a.instanceId < b.instanceId ? -1 : a.instanceId > b.instanceId ? 1 : 0);
  return digest(["cs2.raid.extract.v1", r.raidId, r.session, r.extractionIntentId, r.extractionWindowId, r.cashCents, items]);
}
