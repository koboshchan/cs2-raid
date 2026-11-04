# CS2 Raid (Delta Force CS2) master spec + TODO
Owners: Ocean = map (repo oceanwang525-creator/df-spaceport-map, shards on GH releases). Lucas = plugins/server/hosting. Martin also designs rules.
Repo koboshchan/cs2-raid (public MIT). main 5baa79e. Branches: debug-plugin 8f7bfbf, price-test, raid-gameplay (Ocean map stage verified and pushed in 743791a).
Rules: one root multi-stage Dockerfile, plugins/ (RaidPlugin, DeltaOps excluded by default via INCLUDE_DELTAOPS, Debug), NO .sh/.bat/.ps1, downloadables never in git, map stage cached on release JSON.

## Game flow (Lucas Nov3)
1. Lobby box: all T, Glock only, no damage. Shoot player = request, shoot back = duo (max 2). Broadcast names. Moved to ready platform. !cancel team -> both back to box; !team single -> solo. Avoid chat commands otherwise.
2. After team select: frozen + invisible; keep all 6 DeltaOps characters (D-Wolf, Stinger, Luna, Vyron, Hackclaw, Shepherd). Replace the DeltaOps display box with a numbered list printed in chat; choose using `!characterselect <number>`. DeltaOps API via CSS PluginCapability so raid assigns characters; abilities must respect duos.
3. Cash-in !cashout from SSG: max $15,000 into game per raid. Start $0 otherwise.
4. Buy phase 30s: weapons, armor, helmet. Vanilla prices (client menu can't show custom prices; custom pricing flag off). Chat shows charge.
5. Ammo: mag price = weapon price/20 (Deagle 700 -> 35, G3SG1 -> 250). Reload deducts money.
6. 5s countdown -> teleport teams to random spawn coords (Lucas sends), frozen 10s, then go.
7. Teams: all T + teammates-are-enemies; plugin zeroes duo damage; vanilla wallbangs. Partner-only glow + name + HP tags.
8. Death: armor vanishes; drop guns/grenades/utility + money bag with their cash. No auto pickup; press E: bag cash added, weapons/utility instantly sold to balance. Ragdolls client-side, not lootable. Duo dead -> spectate partner; whole team dead -> noclip invisible, can't interact, until end.
9. Loot: Ocean places props in Hammer with targetnames (names TBD); config table per name: spawn rate, money range, hold-E time; plugin rolls spawn at round start and removes losers. Mini safe models/props_survival/safe/safe.vmdl $300-$3,000 hold 5s; big safe custom (Ocean) $1,000-$5,000 hold 10s. Old "1%" rule replaced by per-name spawn rate.
10. Match max 30 min; alive & not extracted at 30 min die, lose all.
11. Extraction: 4 physical levers (Ocean coords or map entities, press E); any 2 pulled (any time apart) -> 5 min countdown; at end, players inside extraction zone extract. Levers reset after; reusable. Extraction zone = invisible touch-based trigger entity if Ocean adds one; otherwise a plugin-defined zone.
12. On extract: guns, unused grenades, pistols refunded into SSG balance; armor/helmet NOT. Money cap: CS2 likely $100k (some refs 65,535) untested; revisit 65,535 HUD clamp.

## Storage
Plugin no DB (memory + JSON configs). Backend SQLite /data/raid.sqlite (account links, purchase outbox, payouts disabled). Need durable settlement IDs/receipts to prevent double charge/pay. SSG is balance authority. DECIDED (Lucas): keep SQLite for links/settlement; all match state in memory (dropped each game). No persistent stash.

9b. Random-spawn loot ITEMS (non-safe): press E, instant pickup, FIXED value per item name added to balance. (Safes keep hold timer + range unless Lucas says otherwise.)
4b. Buying utility DISABLED (flash, smoke, HE, molotov/incendiary, decoy). Guns/armor/helmet still buyable.
## Debug plugin (done, debug-plugin)
All-map buyzone, max money, AK 2 shots = ladder points, Glock = texture error, !report <reason>. JSONL reports.

## TODO
- [x] Character roster decided: keep all 6 DeltaOps characters.
- [x] DeltaOps source uses a numbered chat list and `!characterselect <number>` (six characters retained; isolated branch, not deployed).
- [ ] Lucas/Ocean: spawn coords, lever coords/entities, extraction trigger name, loot prop names
- [x] map stage defaults to Ocean repo; real build, digest/ZIP verification and cache reuse verified (743791a)
- [ ] implement flow v2 (awaiting go-ahead)
- [ ] DeltaOps API + FFA/duo targeting
- [ ] ammo economy, cash-in/out SSG integration, money bags, E-interact loot/levers, 30-min timer, extraction
- [ ] test CS2 money cap on server
- [ ] live QA (buyzone, glow tags, purchases)
- [ ] price test server still running desktop E:\cs2-pricetest :27015; DeltaOps STOPPED (restore: cs2-pricetest compose stop; E:\deltaops-compose docker compose -p deltaops start)

Utility is supplied by character kits: DeltaOps characters grant their configured grenades. Utility buying remains disabled.
