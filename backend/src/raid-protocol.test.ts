import test from "node:test";
import assert from "node:assert/strict";
import {
  cashInRequestSchema, extractionRequestSchema, raidOperationKey,
  cashInFingerprint, extractionFingerprint, MAX_RAID_CASH_IN_CENTS,
} from "./raid-protocol";

const steamId64 = "76561198000000001";
const raidId = "70000000-0000-4000-8000-000000000001";
const session = { steamId64, connectionId: "connection-1", pawnGeneration: "pawn-1", mapGeneration: "map-1" };
const cashin = { raidId, session, amountCents: MAX_RAID_CASH_IN_CENTS };
const gun = { instanceId: "70000000-0000-4000-8000-000000000002", catalogId: "ak47", kind: "gun" };
const utility = { instanceId: "70000000-0000-4000-8000-000000000003", catalogId: "hegrenade", kind: "unused_utility" };
const extraction = { raidId, session, extractionIntentId: "intent-1", extractionWindowId: "window-1", cashCents: 0, inventory: [gun, utility] };

test("cash-in wire limit is exactly $15k in integer cents; no policy approval implied", () => {
  assert.equal(cashInRequestSchema.parse(cashin).amountCents, 1_500_000);
  for (const amountCents of [0, -1, 1.5, 1_500_001, Infinity, NaN, Number.MAX_SAFE_INTEGER]) {
    assert.throws(() => cashInRequestSchema.parse({ ...cashin, amountCents }));
  }
});
test("wire bodies reject caller prices, app-user IDs, paid flags and untrusted fields", () => {
  for (const extra of [{ appUserId: raidId }, { paid: true }, { amountDollars: 15000 }, { policyApproved: true }])
    assert.throws(() => cashInRequestSchema.parse({ ...cashin, ...extra }));
  assert.throws(() => extractionRequestSchema.parse({ ...extraction, amountCents: 1 }));
  assert.throws(() => extractionRequestSchema.parse({ ...extraction, inventory: [{ ...gun, priceCents: 1 }] }));
});
test("identity requires raid UUID and a complete authenticated native session", () => {
  assert.throws(() => cashInRequestSchema.parse({ ...cashin, raidId: "round-1" }));
  for (const key of ["steamId64", "connectionId", "pawnGeneration", "mapGeneration"])
    assert.throws(() => cashInRequestSchema.parse({ ...cashin, session: { ...session, [key]: "" } }));
  assert.throws(() => cashInRequestSchema.parse({ ...cashin, session: { ...session, clientId: steamId64 } }));
});
test("extraction snapshots reject armor, helmet, spent utility and duplicate instances", () => {
  assert.equal(extractionRequestSchema.parse(extraction).inventory.length, 2);
  for (const kind of ["armor", "helmet", "spent_utility", "utility"])
    assert.throws(() => extractionRequestSchema.parse({ ...extraction, inventory: [{ ...gun, kind }] }));
  assert.throws(() => extractionRequestSchema.parse({ ...extraction, inventory: [gun, gun] }));
  assert.throws(() => extractionRequestSchema.parse({ ...extraction, inventory: Array(129).fill(gun) }));
});
test("extraction cash is safe integer cents, not HUD-clamped and can be zero", () => {
  for (const cashCents of [0, 6_553_600])
    assert.equal(extractionRequestSchema.parse({ ...extraction, cashCents }).cashCents, cashCents);
  for (const cashCents of [-1, 0.01, Infinity, Number.MAX_SAFE_INTEGER + 1])
    assert.throws(() => extractionRequestSchema.parse({ ...extraction, cashCents }));
});
test("stable operation key is retry identity, distinct across server, raid, player and kind", () => {
  const key = raidOperationKey("raid", "cashin", raidId, steamId64);
  assert.match(key, /^[A-Za-z0-9._:-]{8,128}$/);
  assert.equal(key, raidOperationKey("raid", "cashin", raidId, steamId64));
  assert.equal(key, raidOperationKey("raid", "cashin", raidId.replace(/-/g, "").toUpperCase(), steamId64));
  for (const other of [
    raidOperationKey("raid2", "cashin", raidId, steamId64),
    raidOperationKey("raid", "extract", raidId, steamId64),
    raidOperationKey("raid", "cashin", "70000000-0000-4000-8000-000000000004", steamId64),
    raidOperationKey("raid", "cashin", raidId, "76561198000000002"),
  ]) assert.notEqual(key, other);
  assert.throws(() => raidOperationKey("../raid", "cashin", raidId, steamId64));
});
test("changing cash-in amount/session changes fingerprint, never operation key", () => {
  const fingerprint = cashInFingerprint(cashin);
  assert.equal(fingerprint, cashInFingerprint(JSON.parse(JSON.stringify(cashin))));
  assert.notEqual(fingerprint, cashInFingerprint({ ...cashin, amountCents: 1 }));
  assert.notEqual(fingerprint, cashInFingerprint({ ...cashin, session: { ...session, pawnGeneration: "pawn-2" } }));
});
test("inventory ordering is irrelevant; content, window, intent, cash and session are immutable", () => {
  const fingerprint = extractionFingerprint(extraction);
  assert.equal(fingerprint, extractionFingerprint({ ...extraction, inventory: [utility, gun] }));
  for (const changed of [
    { ...extraction, cashCents: 1 },
    { ...extraction, extractionWindowId: "window-2" },
    { ...extraction, extractionIntentId: "intent-2" },
    { ...extraction, session: { ...session, connectionId: "connection-2" } },
    { ...extraction, inventory: [gun] },
    { ...extraction, inventory: [{ ...gun, catalogId: "deagle" }, utility] },
  ]) assert.notEqual(fingerprint, extractionFingerprint(changed));
});
