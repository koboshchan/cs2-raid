# CS2 Raid TODO

Authoritative requirements remain in [SPEC.md](SPEC.md). Implementation status below is offline only; no server, main merge or deployment.

## Flow v2 implemented offline

- [x] Pure phase model: lobby, six-character selection, buy 30s, countdown 5s, spawn freeze 10s, active 30min, close.
- [x] Pure duo request/cancel guards, seven-player cap and damage eligibility.
- [x] Fixed-value instant E item/equipment/bag claiming. No inventory pickup award or shoot-to-loot.
- [x] Safe hold timer, release/range reset, configured money range and single-consumer award.
- [x] Death cash becomes one claimable bag instead of kill transfer; gun drop tracking stages E sale.
- [x] Four distinct lever IDs, two pulls open five-minute countdown, reusable reset.
- [x] Extraction candidate snapshot and durable-receipt-required confirmation boundary. No payout implementation.
- [x] Buy-phase-only guns/armor; utility blocked at buy command. Kit utility remains DeltaOps-owned.
- [x] Native staging adapter compiled: phase movement/visibility, E proximity, native ground CanUse veto, map targetname resolution, touch-trigger membership with configured sphere fallback.
- [x] Disable by default; additional nativeFlowVerified gate. Empty example coordinates/targets, never fabricated map data.

## Blocking work, NOT complete

- [ ] DeltaOps PluginCapability assignment + duo/FFA-aware abilities. deltaops-abilities-fix 4df1896 is read-only reference only; not merged. No capability exists there. Raid character selection refuses confirmation until assignment succeeds.
- [ ] Live verify all twelve ability routes, kit utility delivery/economy fencing and selection lifecycle.
- [ ] Backend-authoritative cash-in max $15,000, durable debit/settlement IDs, exact receipt reconciliation, extraction gun/unused-utility valuation and approved payouts. All money endpoints remain disabled; !cashout moves nothing. Extraction window currently reports unavailable and preserves in-match cash, NOT a successful extraction.
- [ ] Reload/magazine economy (weapon price / 20), reserve-ammo control and native affordability.
- [ ] Lucas/Ocean confirmed lobby, platform, team spawn pads, loot targetnames, four lever targetnames and extraction trigger/zone. Zero actual targets shipped.
- [ ] True lootable death equipment/utility drop completeness, money-bag visible prop/model and armor destruction.
- [ ] Trigger entry/exit reconstruction for players already inside at active start; moving dropped-weapon positions, look/line-of-sight interaction trace.
- [x] Offline adapter uses named native CHASE/ROAMING observer enums, follows living custom partner, and teleports roaming camera once to the player's saved death position.
- [ ] Live verify observer pawn availability, partner-death transition, freecam controls and partner-only visuals.
- [ ] Map-authored prop removal/restoration across native round restarts. In-memory ResetLobby alone cannot restore removed map props.
- [ ] Native round management with freely chosen T/CT, no premature native round end during lobby/freeze or sole-team raids, respawn/reconnect/hot-reload edge cases.
- [ ] Live verify engine CanUse hook denies both auto and use pickup without blocking character kit grants. Tick inventory deltas can miss same-tick creation/drop.
- [ ] Buy menu aliases/armor, native utility bypasses, spawn freeze/invisibility cleanup, purchase debit last-frame races and HUD/native spend reconciliation above $65,535.
- [ ] Isolated native QA before setting nativeFlowVerified; then reviewed merge/deploy approval.

## Storage

Match state exists only in memory; no persistent stash. Existing backend SQLite remains account links/purchase-outbox only. Do not treat local ExtractionIntent or an arbitrary string as a backend receipt. Only a trusted future backend bridge may call ConfirmExtraction after durable receipt verification.

## Offline build acceptance

Dedicated cs2-raid Docker daemon, SDK 10.0.403 and pinned CSS 1.0.379. Two Release builds, zero warnings/errors, byte-identical DLL/deps/PDB. Each run passed 19 legacy rule checks plus 10,264 flow-v2 checks, including 10,000 generated lobby-invariant assertions. This is pure-model/compiler evidence, not native gameplay acceptance. Receipt confirmation additionally requires an issued intent from the current extraction window; stale-window, forged and post-timeout intents reject. Trusted backend receipt validation is still absent, so the native adapter never invokes confirmation.

## Tonight implementation, offline only

- [x] 200 HP/max HP at spawn/lobby and raid insertion.
- [x] Free native T/CT choice during lobby/character selection using SwitchTeam without changing custom duos; block mid-raid switching.
- [x] Same-side strangers can damage via FFA cvars; pure custom-duo immunity stays independent of native teams.
- [x] Shared VData half-spread/inaccuracy baselines are saved once and restored on map end/unload; new guns patched on spawn.
- [x] HUD clamp is projection only; full long ledger survives collection, money bag death/drop and overflow rejection.
- [x] Private loss banners on death; private win helper available for trusted receipt completion only.
- [ ] Trusted extraction settlement adapter must call private ShowRaidResult(player,true) after confirmed extraction, not before backend payment.
- [ ] Live native QA of 200 HP, both-team weapon buys, same-side strangers/opposite-side duos, all gun spread modes, predicted client impacts, private banners and two-user death/freecam transitions.
- [ ] No production or native engine acceptance is implied by compiler/pure-model tests.
