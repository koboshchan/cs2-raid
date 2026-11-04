# CS2 Raid TODO

See [SPEC.md](SPEC.md). Flow changes are documentation only until go-ahead.

- [ ] Lucas: which 4 characters
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
