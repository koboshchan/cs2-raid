# CS2 Raid (Delta Force CS2) master spec + TODO
Owners: Ocean = map (repo oceanwang525-creator/df-spaceport-map, shards on GH releases). Lucas = plugins/server/hosting. Martin also designs rules.
Repo koboshchan/cs2-raid (public MIT). main 5baa79e. Branches: debug-plugin 8f7bfbf, price-test, raid-gameplay (Ocean map stage verified and pushed in 743791a).
Rules: one root multi-stage Dockerfile, plugins/ (RaidPlugin, DeltaOps excluded by default via INCLUDE_DELTAOPS, Debug), NO .sh/.bat/.ps1, downloadables never in git, map stage cached on release JSON.

## Game flow (Lucas Nov3)
1. Lobby box: native T/CT freely chosen, Glock only, no damage. Shoot player = request, shoot back = duo (max 2). Broadcast names. Moved to ready platform. !cancel team -> both back to box; !team single -> solo. Avoid chat commands otherwise.
2. After team select: frozen + invisible; keep the five DeltaOps characters (Stinger, Luna, Vyron, Hackclaw, Shepherd). Replace the DeltaOps display box with a numbered list printed in chat; choose using `!characterselect <number>`. DeltaOps API via CSS PluginCapability so raid assigns characters; abilities must respect duos.
3. Cash-in !cashout from SSG: max $15,000 into game per raid. Start $0 otherwise.
4. Native CS2 Buy phase 45s: weapons, armor, helmet. Vanilla prices (client menu can't show custom prices; custom pricing flag off). Chat shows charge.
5. Ammo: mag price = weapon price/20 (Deagle 700 -> 35, G3SG1 -> 250). Reload deducts money.
6. 5s countdown -> teleport teams to random spawn coords (Lucas sends), frozen 10s, then go.
7. Teams: native T/CT freely chosen before buying (weapon/buy choices), custom duos independent of native sides + teammates-are-enemies; plugin zeroes duo damage; vanilla wallbangs. Partner-only glow + name + HP tags.
8. Death: armor vanishes; drop guns/grenades/utility + money bag with their cash. No auto pickup; press E: bag cash added, weapons/utility instantly sold to balance. Ragdolls client-side, not lootable. Immediately request a private ROUND LOSE banner for 5 seconds, then spectate their living custom-duo partner. Authoritative death/interaction/cash loss is immediate, not delayed with the camera. When the entire custom team is dead (including solos), use non-interacting roaming spectator/freecam starting at that player's death position, until end.
9. Loot: Ocean places props in Hammer with targetnames (names TBD); config table per name: spawn rate, money range, hold-E time; plugin rolls spawn at round start and removes losers. Mini safe models/props_survival/safe/safe.vmdl $300-$3,000 hold 5s; big safe custom (Ocean) $1,000-$5,000 hold 10s. Old "1%" rule replaced by per-name spawn rate.
10. Match ends when every participant is extracted/dead or at the 30-minute deadline; alive and not extracted at expiry die and lose all. Final MVP is the GLOBAL match player with MOST MONEY EXTRACTED; equal money ties use earliest authoritative completed extraction. Nobody extracted means NO MVP and EVERYONE LOSES. These are Lucas-overridable defaults. Each recipient owns their custom raid duo/solo identity and personal extracted-win/dead-or-timeout-loss, never a native T/CT team result. The same global MVP is independent of that identity.
11. Extraction: 4 physical levers (Ocean coords or map entities, press E); any 2 pulled (any time apart) -> 5 min countdown; at end, players inside extraction zone extract. Levers reset after; reusable. Extraction zone = invisible touch-based trigger entity if Ocean adds one; otherwise a plugin-defined zone.
12. Successful extraction immediately requests a private ROUND WIN banner for 5 seconds, then spectate/freecam using the existing custom-duo availability rule. No extraction freecam starting position has been specified; preserve existing native behavior without inventing coordinates. Guns, unused grenades, pistols refunded into SSG balance; armor/helmet NOT. Confirmed native HUD limit is $65,535. Keep the full long balance in the plugin ledger, clamp only the HUD projection, and preserve the full balance in money bags/settlement.

## Storage
Plugin no DB (memory + JSON configs). Backend SQLite /data/raid.sqlite (account links, legacy purchase outbox, durable raid operation/receipt journal; financial enablement OFF). Need durable settlement IDs/receipts to prevent double charge/pay. SSG is balance authority. DECIDED (Lucas): keep SQLite for links/settlement; all match state in memory (dropped each game). No persistent stash.

9b. Random-spawn loot ITEMS (non-safe): press E, instant pickup, FIXED value per item name added to balance. (Safes keep hold timer + range unless Lucas says otherwise.)
4b. Buying utility DISABLED (flash, smoke, HE, molotov/incendiary, decoy). Guns/armor/helmet still buyable.
## Debug plugin (done, debug-plugin)
All-map buyzone, max money, AK 2 shots = ladder points, Glock = texture error, !report <reason>. JSONL reports.

## TODO
- [x] Character roster decided: keep the five DeltaOps characters.
- [x] Offline replacement of DeltaOps display box with a numbered list printed in chat; choose using `!characterselect <number>`.
- [ ] Lucas/Ocean: spawn coords, lever coords/entities, extraction trigger name, loot prop names
- [x] map stage defaults to Ocean repo; real build, digest/ZIP verification and cache reuse verified (743791a)
- [ ] implement flow v2 (awaiting go-ahead)
- [x] Offline DeltaOps capability API + FFA/duo targeting; native QA pending
- [ ] ammo economy, cash-in/out SSG integration, money bags, E-interact loot/levers, 30-min timer, extraction
- [ ] test CS2 money cap on server
- [ ] live QA (buyzone, glow tags, purchases)
- [ ] price test server still running desktop E:\cs2-pricetest :27015; DeltaOps STOPPED (restore: cs2-pricetest compose stop; E:\deltaops-compose docker compose -p deltaops start)

Utility is supplied by character kits: DeltaOps characters grant their configured grenades. Utility buying remains disabled.

## Tonight gameplay amendments (Lucas Nov4)

Every player spawns at 200 HP, with 200 maximum HP. At raid insertion reset all participants to 200 HP. Native T/CT selection in lobby/character phase does not alter custom-duo membership, partner immunity, or partner-only visuals. Team switching is blocked once buying starts, so it cannot reset inventory or evade an active fight. Set mp_teammates_are_enemies=1, friendly fire on, autobalance and team limits off, and ignore native round-win conditions while the custom raid owns the lifecycle.

All guns use half their baseline server VData spread and ballistic inaccuracy fields, both firing modes, including stand/crouch/move/jump/land/ladder/fire and jump-initial/apex/reload. Shared VData must be patched idempotently, never halved again per gun spawn or native round. Restore originals on map end/unload. Client prediction/impacts can differ from server authoritative hit calculation, so live QA is required.

Result banners are private synthetic round_end plus cs_win_panel_round events sent only to that player, not a native round termination. Death/time-out is loss. Successful extraction is win only after a trusted durable backend receipt, never when merely entering a zone or issuing an intent. Client rendering is not accepted until live tested.

Financial bridge source is intentionally disabled and unfinished. Both FinancialNativeVerified and RAID_FINANCIAL_ENABLED are required. Native paid partial completion remains quarantined and requires a durable manual-reconciliation design before enablement. No automatic refund or native regrant is authorized. Full-price extraction follows current section 12; historical 90% sellback wording is not applied to extraction. Compiler/model tests are not financial or native runtime acceptance.

Native Buy timing is 45 seconds using mp_buytime45 in current CS2 seconds units. The engine HUD owns the Buy countdown; no custom 30-second timer is authoritative. See [native Buy timing and unit proof](native-buy45.md). Offline timing work does not enable or certify native menu purchases, reloads, kits, coordinates or financial proof gates.


Offline outcome adapter acceptance (Nov4)

RaidOutcomes owns a match-local five-second observer eligibility deadline. A logical death and verified extraction are terminal immediately, before private event delivery, so a fallible banner cannot defer rule enforcement or repeat economics. Duplicate outcomes cannot reset that deadline. Observer target is looked up at transition time and on following ticks; a disconnected or dead partner is not used. Disconnect permanently cancels this coordinator's observation eligibility for that participant; reset/map-stop replaces the coordinator. There are no queued outcome callbacks. A private round_start request is emitted once at observer transition for cleanup, without claiming guaranteed native panel/audio clearing. A closed match waits at least five seconds and every outstanding outcome deadline before final private projection and lobby reset. No global native round termination was added.

Money extracted uses the existing authenticated structured extraction receipt AmountCents, which is the final money actually settled by existing extraction rules. It is not a newly calculated inventory-value metric, profit, kills or native HUD value. No payout formula, native grants or financial enablement was added. The legacy model-only no-receipt helper retains cash-to-cents behavior for existing pure tests; production passes the verified receipt explicitly. Completed extraction order breaks equal-money ties, not SteamID, native team or kills.

Native acceptance remains BLOCKED. The existing private round_end/cs_win_panel_round path is a request, not proof of a visible five-second panel. CSS/Debug source does not verify suppressing native automatic death cameras for those five seconds. Existing verified financial isolation still removes inventory and changes extracted players to Spectator immediately; it is deliberately not delayed or weakened. Only plugin observer retargeting is delayed. Immediate logical damage/use/loot/purchase denial is modeled; exact native physical attack suppression and external map interactions require runtime QA. Death freecam preserves the existing stored death position plus existing observer height. Extraction freecam coordinates remain unspecified and no teleport is invented.

Full private MVP card, own-duo artwork/roster, music and exact reset clearing remain unsupported/unverified. presentRaidEnd is an unwired optional projection interface, not an implemented or guaranteed screen. Do not enable, deploy or describe this as full native outcome acceptance. Debug017 research candidate b895f7803e47d1dacbc440da6bfd5a113dd58806 was read, not merged or edited. Existing native/financial/extraction/coordinate gates remain OFF.

## Seven user-provided native spawns, offline Nov4

Authoritative ordered decimal data is config/spawns/spaceport-default-20261105.json. Provenance is USER_PROVIDED_SETPOS, already native CS2 setpos coordinates. Do not apply Ocean's DF transform, handedness swaps, scale, offset, coordinate rounding or map-v1 calibration. Pitch and roll are zero; supplied yaw values are already multiples of45 and are preserved, including -90 and180. JSON loads into double precision and round-trips without coordinate changes. Conversion to float32 occurs only at the existing engine Position boundary; Place uses pitch0/roll0 and the supplied yaw. Float32 engine representation cannot retain every decimal digit, which is not a source-data rounding operation.

The exact binding is map df_spaceport_player_scale_raytrace and Ocean release default-20261105. The prior repo had NO release-qualified gameplay configuration loader, only generic gameplay.json plus legacy map-v1 asset lock and empty example spawns. It would be false to call the old generic config a verified release binding. This change adds explicit spawnProfile="spaceport-default-20261105" and mapRelease="default-20261105" selection. The optional disabled example is config/gameplay.spaceport-default-20261105.example.json; existing gameplay.example.json, map-v1 lock and Dust2/legacy spawn arrays are unchanged. To select in a future approved configuration, copy those explicit fields into gameplay.json and install the profile under the plugin's spawns directory. This task does NOT install or select any live config.

The loader reads only one allowlisted local filename, checks map/release/provenance/coordinate-space/count/finiteness/zero pitch-roll/45-degree yaw, and clears generic fallback spawns for any explicit profile selection. Unsupported or missing profile, different live Server.MapName or different configured release stays fail-closed; no fallback to another map's coordinates. Reset selects points in source order. Existing phase logic may randomize the assignment to teams, but does not reorder or transform source data. Map binding is checked again during model advancement and game ticks. mapRelease is an explicit operator configuration declaration, NOT cryptographic proof of mounted VPK/release contents. No offline JSON or same-name map can establish actual loaded-release provenance; an approved deployment owner must verify matching assets separately. Filename and native map name alone are insufficient proof of release identity.

No ground/collision/clearance/native spawn acceptance is claimed. All seven ground heights and safe team layouts remain unvalidated. No duo offsets were supplied, so this selected profile blocks phase advancement for custom teams larger than one rather than reusing the legacy48-unit offset, inventing another offset, or stacking duo members. Legacy configurations retain their prior behavior. This temporary conservative duo HOLD must be resolved with user-specified or independently native-validated layout policy before duo raids can use these points. All existing enabled/nativeFlowVerified/coordinatesConfirmed/financialNativeVerified gates remain OFF; explicit profile selection does not authorize enabling any gate.

### Ordered rotated extraction polygon for the qualified profile

The latest ordered quad replaces every older axis-aligned extraction box and width/length expansion rule. Green vertices in order are (715.278076,-6013.118164), (1039.138794,-6369.889648), (1484.720825,-5940.820312), (1095.046509,-5576.641113). The translation-stable shoelace AREA centroid is (1089.1912371274484,-5973.454242877722), not the vertex mean. Each red vertex is centroid + 4 * (green vertex - centroid), stored without independent rounding in the shared config/spawns/spaceport-default-20261105.json.

Ordered vertices are authoritative. minX/maxX/minY/maxY remain bounding-box compatibility metadata only and never decide membership. Inclusive point-in-polygon checks accept all vertices and edges, with only a floating arithmetic roundoff allowance for collinearity. Both zones accept any finite Z and reject nonfinite input. Green wins on overlap; the red-only ring never grants extraction.

Seven spawns remain unchanged. The adapter still requires the selected qualified map/release, living player and all existing native/financial gates. No trigger, automatic extraction, payment, financial enablement or deployment was added. Legacy unselected bounded radius behavior is unchanged. Source/build tests are not client or runtime acceptance.
