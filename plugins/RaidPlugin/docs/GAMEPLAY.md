# Raid gameplay mode

This is an opt-in, engine-adapter implementation, not live-server acceptance. No CS2 instance was started. The old extraction/SSG scaffold remains separate and fail-closed. No real funds are charged or minted.

## Configuration and startup

The Docker entrypoint copies `config/gameplay.example.json` into the RaidPlugin directory as `gameplay.json` only if absent. The example is disabled and has unconfirmed zero coordinates. Edit the installed plugin's JSON, supply actual lobby/ready-platform positions and at least one spacious spawn pad per team (seven for a full solo roster), loot points/tiers and a map-available prop model. Set `coordinatesConfirmed` and `enabled` true, then restart all plugins/server. Do not hot-toggle one plugin while the others remain running. Do not enable without verified coordinates. Configuration failures fail closed; no automatic starts from placeholders.

The lobby box and ready platform are map geometry, not plugin-built walls. Supply coordinates inside the mapper's box/platform. Duo spawn positions use a 48-unit X offset; pads must accommodate both players without geometry overlap. Spawn pads are sampled without replacement per game.

Everyone plays on native T. Custom teams are disjoint solos/duos. Shoot a player with a bullet to request; reciprocal shots finalize the duo and announce their names. No lobby damage. `!cancel team`, `!cancelteam` and `!team single` are the new team commands. Joining/cancelling/disconnecting during countdown cancels it. A continuously ready roster starts after five seconds, $0 each. Manual team switching/native buy commands are blocked. Late arrivals during a match spectate until the next lobby. No revives or reconnect resurrection. All-dead or thirty-minute timeout returns the roster to the lobby.

Partner visuals use a separate non-solid, damage-disabled glow prop plus a name/HP worldtext. CheckTransmit removes ONLY these cosmetic entities for everyone except their living duo partner. Actual player pawns are never hidden. Filtering re-evaluates current teams/liveness on every transmit, including cancellation/disconnect. Floating text faces the partner and tracks the subject every tick; glow uses model parenting/bonemerge. CheckTransmit does not force entities outside the PVS to transmit, so distant/occluded rendering must be checked on the real map. Animation alignment, glow alpha/material behavior, tag orientation and health updates require two-client in-engine QA.

Duo damage is blocked before engine application, including explosions. Native T FFA permits other teams to damage each other. Bullet collision, penetration and wallbang mechanics are intentionally UNMODIFIED, per Lucas's latest instruction. Damage suppression is not bullet pass-through.

A dead player is an engine observer, not a resurrected invisible live pawn. Observer mode follows a living teammate (mode 5); after team wipe, free-roam observer (mode 6), with no alive pawn to shoot, collide or use objects. Observer-pawn availability, forced mode behavior and engine round lifecycle require live QA.

Cash is an in-memory game ledger in dollars, with a HUD projection capped at 65535. Kills transfer the victim's full balance, never a fixed bonus; suicide/environment/disconnect destroys that balance. Enemy dropped-gun value comes from `prices.json`, once per weapon entity for the match. Native handle serials are preserved, not just entity indices. Friendly/original-team first pickup consumes eligibility so later dropping cannot farm awards. Pickups/drops are detected by per-tick inventory differences, including death; creation and pickup entirely between two ticks could be missed and must be tested live. Guns remain native ground entities; death-drop cvar is enabled.

Loot objects are configured by ID, position and tier. Bullet damage against a registered object atomically claims its inclusive random money range, removes the object and blocks normal damage. Other damage cannot claim it. Spawn chance is provisionally `lootSpawnChance: 0.01`, interpreted as a 1% independent chance **per loot point per game**, NOT a 1% player cap. This interpretation is explicitly pending Lucas's clarification. The default model must be validated against the supplied map/assets; collider/bullet-hit registration requires engine QA.

## Reconciliation

The new custom-team game replaces the old single-player FFA roster rule. New loot safes replace the former 'players only' loot restriction. Kill cash transfer replaces 'no kill bonus', but does not create extra money. Native death drops/value pickup replace corpse-menu-only inventory behavior. New team-ready automatic starts replace console-only scaffold starts; the old console start is disabled while gameplay mode is enabled.

Legacy two-lever/five-minute extraction, 90% external sellback and persistent backpacks remain engine-independent scaffold code with its original nineteen tests; they are NOT wired to the new game ledger or match end. No external cash-out is implemented. Clarify how extraction should end a duo/player's raid and settle the new loot cash before connecting it. The thirty-minute duration and seven server slots are retained.

DeltaOps is disabled at load in gameplay mode pending custom-team/ability integration: its native-TeamNum targeting and free utilities would be wrong here. Map Debug is also disabled in gameplay mode: its max-money/buy-anywhere overrides and Glock marker reports conflict. Both remain independently buildable and usable with gameplay disabled. Their mode checks read the sibling RaidPlugin config and fail closed on unreadable JSON. Restart all plugins after editing mode.

## Verified offline

Raid artifacts build twice with .NET 10.0.403 / CSS 1.0.379, zero warnings/errors, identical DLL/deps/PDB. Each run passes 19 original extraction checks and 10072 gameplay checks (10091 total), including 100 seeded request/cancel simulations of 100 steps each. Cases cover reciprocal requests, max-two invariant, cancellation, solo readiness, countdown invalidation, late joins, damage/visibility ACLs, kill transfer, duplicate loot claim, no pickup farming, timeout and invalid config.

Debug artifacts build zero warnings/errors and pass all 152 checks. DeltaOps artifacts build twice zero warnings/errors and deterministically match. Root multi-stage Dockerfile remains the only Dockerfile; no operational script files or downloaded binaries/maps are committed. Only plugin artifact targets were built, never the final server target. No merge or deployment.

## Remaining decisions and acceptance

Need real lobby/platform/spawn/loot coordinates, confirmed loot model and value tiers, the exact meaning of 'only 1%', starting combat loadout/shop timing beyond the lobby Glock, and extraction/cash-out semantics. Current start retains the lobby Glock. Ready-up lacks a minimum player count; one ready solo can start. If a minimum or admission grace window is desired, specify it.

Before deployment test two clients plus a third nonpartner for visual privacy and damage isolation, request/cancel/disconnect during countdown, T enforcement, spawning and respawn fences, native observer behavior, real weapon drops/pickups, one-claim object hitboxes, model precache, round restart/end, reconnect and map transitions. Offline compilation does not establish these engine behaviors.


## Pricing follow-up

DeltaOps source and its independent artifact target are retained, but the default server build selects the empty `deltaops-disabled` stage and neither builds nor installs DeltaOps. Opt in with `--build-arg INCLUDE_DELTAOPS=enabled`. This selects the separately built plugin binaries. Default startup removes stale DeltaOps DLL/deps from an existing persistent CS2 volume. This does not override the gameplay-mode ability safety fence.

The CS2 client buy menu continues to show client-defined prices; server plugins cannot reliably change that display (Lucas tested). Purchases use catalog prices in `config/prices.json`. Following the price-test approach, real weapon VData is patched server-side, `buy` commands are correlated with same-tick item_purchase events, and the authoritative game ledger is charged the custom price. Each verified purchase immediately prints its actual charge, for example `AK-47: $3100`, then corrects the HUD immediately and next frame. Multiple buys debit sequentially. Failed/insufficient purchases never debit the ledger. Uncorrelated engine events produce an explicit unverified-purchase message, never a speculative refund. Autobuy/rebuy remain blocked, as do unknown catalog items, malformed commands and purchases outside a living active match. Native buy-zone/time and native affordability checks still apply. A weapon whose VData has not yet existed can retain its native affordability gate on its first purchase; the client menu itself is never represented as authoritative. Human purchase event order and native affordability remain live-QA items, not established by compilation.

The additional twelve pure purchase checks cover insufficient funds, negative price, unknown player, exact balance, sequential debits, dead players and lobby rejection. They supplement the previous 10091 raid/gameplay checks. Optional-stage exports were both tested: default contains only a disabled marker, enabled contains only DeltaOps DLL/deps. No final server image was built or started.
