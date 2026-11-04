# CS2 Raid TODO

Authoritative requirements remain in [SPEC.md](SPEC.md). Implementation status below is offline only; no server, main merge or deployment.

## Flow v2 implemented offline

- [x] Pure phase model: lobby, six-character selection, native buy 45s, countdown 5s, spawn freeze 10s, active 30min, close.
- [x] Pure duo request/cancel guards, seven-player cap and damage eligibility.
- [x] Fixed-value instant E item/equipment/bag claiming. No inventory pickup award or shoot-to-loot.
- [x] Safe hold timer, release/range reset, configured money range and single-consumer award.
- [x] Death cash becomes one claimable bag instead of kill transfer; gun drop tracking stages E sale.
- [x] Four distinct lever IDs, two pulls open five-minute countdown, reusable reset.
- [x] Extraction candidate snapshot and internal structured-proof-only confirmation boundary; disabled native settlement adapter.
- [x] Buy-phase-only guns/armor; utility blocked at buy command. Kit utility remains DeltaOps-owned.
- [x] Native staging adapter compiled: phase movement/visibility, E proximity, native ground CanUse veto, map targetname resolution, touch-trigger membership with configured sphere fallback.
- [x] Disable by default; additional nativeFlowVerified gate. Empty example coordinates/targets, never fabricated map data.

## Blocking work, NOT complete

- [x] Offline DeltaOps PluginCapability assignment, six characters, custom duo/FFA targeting and fail-closed reset lifecycle. Unsafe global enemy reveals suppressed; live ability/utility QA remains blocked.
- [ ] Live verify all twelve ability routes, kit utility delivery/economy fencing and selection lifecycle.
- [ ] Financial ENABLEMENT remains blocked. Offline backend durable debit/mint receipt journal and gated native polling/proof adapter exist. Cash-in max $15,000; full-price guns and unused utility extraction. Backend RAID_FINANCIAL_ENABLED defaults unset and native FinancialNativeVerified defaults false. Native partial-completion recovery, durable cleanup evidence, manual reconciliation/lifecycle, live input freeze and end-to-end QA remain unfinished. No automatic refunds/regrants.
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

Match state exists only in memory; no persistent stash. Backend SQLite contains links, legacy outbox and durable raid operation/receipt journal. Native allowlist and cleanup progress remain memory-only, a financial enablement blocker. No public arbitrary-receipt completion API remains. ConfirmVerifiedExtraction is internal and called only after locally issued request and authenticated backend structured-proof validation.

## Offline build acceptance

Dedicated cs2-raid Docker daemon, SDK 10.0.403 and pinned CSS 1.0.379. Two Release builds, zero warnings/errors, byte-identical DLL/deps/PDB. Each run passed 19 legacy rule checks plus 10,336 flow-v2 checks plus 41 settlement-protocol checks, including 10,000 generated lobby-invariant assertions. This is pure-model/compiler evidence, not native gameplay acceptance. Receipt confirmation additionally requires an issued intent from the current extraction window; stale-window, forged and post-timeout intents reject. Authenticated private-backend proof validation is present behind OFF gates; native success/failure paths are not covered by pure-model tests. Backend passed 47 tests and TypeScript build. Debug passed 182 checks. Raid and DeltaOps repeat builds were byte-identical with zero warnings/errors.

## Tonight implementation, offline only

- [x] 200 HP/max HP at spawn/lobby and raid insertion.
- [x] Free native T/CT choice during lobby/character selection using SwitchTeam without changing custom duos; block mid-raid switching.
- [x] Same-side strangers can damage via FFA cvars; pure custom-duo immunity stays independent of native teams.
- [x] Shared VData half-spread/inaccuracy baselines are saved once and restored on map end/unload; new guns patched on spawn.
- [x] HUD clamp is projection only; full long ledger survives collection, money bag death/drop and overflow rejection.
- [x] Private loss banners on death; private win helper available for trusted receipt completion only.
- [x] Gated extraction adapter latches a private win request at trusted logical consumption before fallible native cleanup; rendering failure does not retry payment. Native runtime and partial-consumption recovery remain blocked.
- [ ] Live native QA of 200 HP, both-team weapon buys, same-side strangers/opposite-side duos, all gun spread modes, predicted client impacts, private banners and two-user death/freecam transitions.
- [ ] No production or native engine acceptance is implied by compiler/pure-model tests.

## Urgent private bot combat fixture
Source-only isolated raid-bot-combat-v1 from deployed aecc49c. Stage2 retained separately untouched.
Typed captured mp_do_warmup_period=false, mp_freezetime=0, roundtime/defuse/hostage=30f; existing ignore-round-win=true. ON executes constant mp_warmup_end after scope and captures; elapsed warmup cannot be restored, but cvar baselines restore OFF/unload. No fake restoration promise.
Console inspect now accepts bots and shows native HP, max, team, alive, move/actualmove/flags, bot/enrollment/solo-id and phase. trace 0..32 prints damage policy decisions console-only, reset on OFF/round. No health-delta acceptance implied.
Lobby now allows enrolled living human-bot and bot-bot damage; human-human retains no-damage mutual-shot pairing. Selection/Buy/Countdown/Freeze remain protected; Active custom-duo immunity/FFA unchanged. No silent advance after !team single: selection still required; use !characterselect 1..6 and normal automatic phases. Live real health deltas BOTH directions, bot AI target behavior, freeze flags and cvar types still owner-native QA.
Stage3 utility from proven ability-kit provenance remains allowed as future scope; only BUY-menu utility prohibited. Stage2 inactive policy has no runtime integration.

- [x] Offline 45-second Buy model, deadline admission, fixed engine clock anchor, typed scoped mp_buytime capture/restore; current-engine seconds receipt documented.
- [ ] Native45 HUD/menu runtime acceptance on both T/CT, exact boundary, phase transitions and lifecycle restoration on an explicitly approved isolated runtime. Keep every existing native/financial proof gate OFF.


Nov4 offline five-second outcome work

The match-local pure coordinator and production adapter now defer plugin observer retargeting until the exact five-second deadline, isolate duplicate/death/extract races, cancel disconnected identities and replace all state on map/reset/stop. Trusted receipt final money and authoritative extraction sequence produce one global MVP independent of each private custom-duo/solo projection. Empty extracted roster produces no MVP and all lose. Closed-match projection is once-only after outstanding deadlines. Existing cash/consumption barriers remain authoritative and no financial gate was enabled.

Still required before runtime acceptance are actual private panel duration and reset behavior, preventing native automatic early death camera, extracted-player immediate isolation versus delayed camera, full MVP card/music, custom-duo identity rendering and native round termination/broadcast arbitration. The final projection delegate is intentionally unwired. No screen or music is claimed implemented. No extraction freecam coordinate was guessed. Approved live tests must cover two recipients with same native team but different raid duos, solos, disconnection/reconnection/new pawns, round/map/unload, partner death during the five seconds, simultaneous close/expiry/extraction and backend cleanup failures. Exact physical/native behavior is not proven by model tests or compilation.

## Seven native spawn profile, offline Nov4

Source/config loader and84 pure checks cover seven ordered precise native points, zero pitch/roll, supplied45-degree yaw, finite numeric values, serialization roundtrip, float32-only engine boundary, signed axes without transforms, exact map/release/selector mismatches, invalid metadata and unchanged disabled generic defaults. Root Dockerfile includes the profile source/tests and seeds the fixed profile only when absent in future runtime initialization; it does not overwrite an existing gameplay.json or enable the new profile. Optional qualified gameplay example remains disabled.

Native ground/collision/clearance checks, actual mounted Ocean default-20261105 provenance and duo offsets remain unverified. An operator's mapRelease declaration is not proof of actual assets. Profile-selected duo teams therefore cannot advance until a separate approved layout policy exists. Existing native/financial gates remain OFF. No client texture changes, client closure, server actions, live config installation or deployment occurred. Prior five-second outcome native feature HOLD is unchanged.

The same qualified profile now has exactly one user-provided inclusive axis-aligned native XY extraction rectangle with unboundedZ=true. Pure tests cover exact normalized decimal bounds, original/reversed corner normalization, four corners/all four edges, just-outside adjacent-double positions, arbitrary finite positive/negative heights, nonfinite rejection, serialization roundtrip and unchanged legacy bounded vertical distance. Facing is absent from the zone API by design. Runtime uses the existing extraction eligibility path only; no payouts, automatic extraction or financial gates were added. All seven original spawn values and order remain unchanged. Native ground/zone acceptance and actual release provenance remain HOLD.
