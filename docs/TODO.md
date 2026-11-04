# CS2 Raid TODO

Authoritative spec: [SPEC.md](SPEC.md). Flow implementation awaits go-ahead.

- [ ] Lucas: which 4 characters
- [ ] Lucas/Ocean: spawn coords, lever coords/entities, extraction trigger name, loot prop names
- [ ] map stage -> Ocean repo (task 7cf85ab8 building)
- [ ] implement flow v2 (awaiting go-ahead)
- [ ] DeltaOps API + FFA/duo targeting
- [ ] ammo economy, cash-in/out SSG integration, money bags, E-interact loot/levers, 30-min timer, extraction
- [ ] test CS2 money cap on server
- [ ] live QA (buyzone, glow tags, purchases)
- [ ] price test server still running desktop E:\cs2-pricetest :27015; DeltaOps STOPPED (restore: cs2-pricetest compose stop; E:\deltaops-compose docker compose -p deltaops start)

Extraction must use Ocean's invisible touch-based trigger when available, otherwise a plugin-defined zone.
