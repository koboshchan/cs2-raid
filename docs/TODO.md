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
- [ ] Whole-dead-team invisible noclip (current adapter uses non-interacting native observer); partner spectating and partner-only visuals live QA.
- [ ] Map-authored prop removal/restoration across native round restarts. In-memory ResetLobby alone cannot restore removed map props.
- [ ] Native round management with all-T teams, no premature native round end during lobby/freeze or sole-team raids, respawn/reconnect/hot-reload edge cases.
- [ ] Live verify engine CanUse hook denies both auto and use pickup without blocking character kit grants. Tick inventory deltas can miss same-tick creation/drop.
- [ ] Buy menu aliases/armor, native utility bypasses, spawn freeze/invisibility cleanup, purchase debit last-frame races and HUD cap test.
- [ ] Isolated native QA before setting nativeFlowVerified; then reviewed merge/deploy approval.

## Storage

Match state exists only in memory; no persistent stash. Existing backend SQLite remains account links/purchase-outbox only. Do not treat local ExtractionIntent or an arbitrary string as a backend receipt. Only a trusted future backend bridge may call ConfirmExtraction after durable receipt verification.
