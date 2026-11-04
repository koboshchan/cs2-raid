# CS2 Raid (Delta Force CS2) master spec + TODO
Owners: Ocean = map (repo oceanwang525-creator/df-spaceport-map, shards on GH releases). Lucas = plugins/server/hosting. Martin also designs rules.
Repo koboshchan/cs2-raid (public MIT). main 5baa79e. Branches: debug-plugin 8f7bfbf, price-test, raid-gameplay (Ocean map stage verified and pushed in 743791a).
Rules: one root multi-stage Dockerfile, plugins/ (RaidPlugin, DeltaOps excluded by default via INCLUDE_DELTAOPS, Debug), NO .sh/.bat/.ps1, downloadables never in git, map stage cached on release JSON.

## Game flow (Lucas Nov3)
1. Lobby box: native T/CT freely chosen, Glock only, no damage. Shoot player = request, shoot back = duo (max 2). Broadcast names. Moved to ready platform. !cancel team -> both back to box; !team single -> solo. Avoid chat commands otherwise.
2. After team select: frozen + invisible; keep all 6 DeltaOps characters (D-Wolf, Stinger, Luna, Vyron, Hackclaw, Shepherd). Replace the DeltaOps display box with a numbered list printed in chat; choose using `!characterselect <number>`. DeltaOps API via CSS PluginCapability so raid assigns characters; abilities must respect duos.
3. Cash-in !cashout from SSG: max $15,000 into game per raid. Start $0 otherwise.
4. Buy phase 30s: weapons, armor, helmet. Vanilla prices (client menu can't show custom prices; custom pricing flag off). Chat shows charge.
5. Ammo: mag price = weapon price/20 (Deagle 700 -> 35, G3SG1 -> 250). Reload deducts money.
6. 5s countdown -> teleport teams to random spawn coords (Lucas sends), frozen 10s, then go.
7. Teams: native T/CT freely chosen before buying (weapon/buy choices), custom duos independent of native sides + teammates-are-enemies; plugin zeroes duo damage; vanilla wallbangs. Partner-only glow + name + HP tags.
8. Death: armor vanishes; drop guns/grenades/utility + money bag with their cash. No auto pickup; press E: bag cash added, weapons/utility instantly sold to balance. Ragdolls client-side, not lootable. Dead player spectates their living custom-duo partner. When the entire custom team is dead (including solos), use non-interacting roaming spectator/freecam starting at that player's death position, until end.
9. Loot: Ocean places props in Hammer with targetnames (names TBD); config table per name: spawn rate, money range, hold-E time; plugin rolls spawn at round start and removes losers. Mini safe models/props_survival/safe/safe.vmdl $300-$3,000 hold 5s; big safe custom (Ocean) $1,000-$5,000 hold 10s. Old "1%" rule replaced by per-name spawn rate.
10. Match max 30 min; alive & not extracted at 30 min die, lose all.
11. Extraction: 4 physical levers (Ocean coords or map entities, press E); any 2 pulled (any time apart) -> 5 min countdown; at end, players inside extraction zone extract. Levers reset after; reusable. Extraction zone = invisible touch-based trigger entity if Ocean adds one; otherwise a plugin-defined zone.
12. On extract: guns, unused grenades, pistols refunded into SSG balance; armor/helmet NOT. Confirmed native HUD limit is $65,535. Keep the full long balance in the plugin ledger, clamp only the HUD projection, and preserve the full balance in money bags/settlement.

## Storage
Plugin no DB (memory + JSON configs). Backend SQLite /data/raid.sqlite (account links, legacy purchase outbox, durable raid operation/receipt journal; financial enablement OFF). Need durable settlement IDs/receipts to prevent double charge/pay. SSG is balance authority. DECIDED (Lucas): keep SQLite for links/settlement; all match state in memory (dropped each game). No persistent stash.

9b. Random-spawn loot ITEMS (non-safe): press E, instant pickup, FIXED value per item name added to balance. (Safes keep hold timer + range unless Lucas says otherwise.)
4b. Buying utility DISABLED (flash, smoke, HE, molotov/incendiary, decoy). Guns/armor/helmet still buyable.
## Debug plugin (done, debug-plugin)
All-map buyzone, max money, AK 2 shots = ladder points, Glock = texture error, !report <reason>. JSONL reports.

## TODO
- [x] Character roster decided: keep all 6 DeltaOps characters.
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
