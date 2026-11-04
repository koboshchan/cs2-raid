# CS2 Raid TODO

See [SPEC.md](SPEC.md). Flow changes are documentation only until go-ahead.

- [x] Character roster decided: keep all 6 DeltaOps characters.
- [ ] Replace DeltaOps display box with a numbered list printed in chat; choose using `!characterselect <number>`.
- [ ] Lucas/Ocean: spawn coords, lever coords/entities, extraction trigger name, loot prop names
- [x] map stage defaults to Ocean repo; real build, digest/ZIP verification and cache reuse verified (743791a)
- [ ] implement flow v2 (awaiting go-ahead)
- [ ] DeltaOps API + FFA/duo targeting
- [ ] ammo economy, cash-in/out SSG integration, money bags, E-interact loot/levers, 30-min timer, extraction
- [ ] test CS2 money cap on server
- [ ] live QA (buyzone, glow tags, purchases)
- [ ] price test server still running desktop E:\cs2-pricetest :27015; DeltaOps STOPPED (restore: cs2-pricetest compose stop; E:\deltaops-compose docker compose -p deltaops start)

## Confirmed requirements for the pending flow

- [ ] Non-safe loot items: instant press-E pickup, fixed value per item name; no hold timer or random payout.
- [ ] Safes: retain hold-E timer and configured money range.
- [ ] Disable buying all utility (flash, smoke, HE, molotov/incendiary, decoy); allow guns, armor and helmet.
- [ ] Keep SQLite only for account links and durable settlement/idempotency records. SSG remains balance authority.
- [ ] Keep all match state in memory and discard it each game; no persistent stash.
- [ ] Prefer Ocean's invisible touch-based extraction trigger; use a plugin-defined zone if absent.

Utility is supplied by character kits: DeltaOps characters grant their configured grenades. Utility buying remains disabled.

Future extraction presentation requirement (Lucas, Nov4): the extracting player should receive a private MVP-style end-round screen. Pending native client rendering/audio/reset verification; never end the real round or award/change global MVP stats/music to force presentation. This note does not enable extraction, payouts, money routes, or any gameplay gate.


Future real raid match-end research (Lucas Nov4, documentation only). Proposed trigger: every participant has extracted or died, OR 30-minute expiry. This is separate from Debug's cosmetic previews. Timeout handling, disconnected participants, exact finalization races, MVP metric/ties, and duo outcome semantics remain undecided. A top extractor as MVP is only an example, NOT an adopted ranking.

CSS379 provides CCSGameRules.TerminateRound(float delay, RoundEndReason reason), implemented by native Windows/Linux virtual functions with platform-specific argument order. Evidence https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CCSGameRules.cs and https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Modules/Entities/Constants/RoundEndReason.cs . This is a genuine server round-ending API, unlike firing a cosmetic event. Installed game signatures/offsets, delay behavior, native auto-end conflicts, score/economy/next-round effects and MVP ordering still require isolated integration proof before implementation. Do not invoke it for Debug previews.

One native call takes ONE reason; round_end carries ONE winner/reason for the shared round, not a player-result map. T/CT victory cannot encode extracted WIN versus dead/timeout LOSE for different players on the same team. Raid all-T/FFA/duos make that mismatch important. Survival enum names are not evidence of arbitrary per-player or duo outcomes. Do not swap teams, change shared scores, or terminate separate rounds per player to force UI.

Possible future split, NOT enabled: terminate the real round once with an agreed shared result, then use explicitly private client presentation for each agreed personal outcome plus the chosen MVP. Extracted-WIN/dead-or-timeout-LOSE is Lucas's requested presentation idea; timeout rules and duo semantics must be confirmed rather than inferred. Private synthetic round_end/MVP/panel sends do not change the authoritative result, scoreboard, settlement or teammate status. Their ability to override a simultaneous real round-end broadcast is UNPROVEN; ordering/duration can conflict and the real broadcast may overwrite them. A custom shipped client HUD is a fallback design option, not a proven server-only capability. Real round-end does not by itself prove a chosen arbitrary MVP can be awarded/displayed safely. Full per-player native screen support is NOT established. Two-client mixed same-team/duo outcomes, native MVP selection and clearing/audio must be researched/tested independently before choosing an implementation. No global MVP/round-stat/music mutation in this 0.1.7 work.

TODO: settle trigger roster/timeout/disconnect and duo rules; settle MVP metric/ties; prove one-shot real native termination offline/isolated; choose shared result separately from personal results; prove mixed private UI after a real broadcast without leaking global events; test native MVP/music/reset with two clients. Extraction, expiry handling, financial routes and payouts are not implemented or enabled by this note.
