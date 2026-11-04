# LAN fixture knife follow-up (pending, not implemented)

Do not delay reviewed bot-enemy lifecycle fix for knife variants. No DeltaOps abilities.

Required next change: every enrolled human and bot has at least one native default knife after fixture spawn/lobby inventory reset, purchase, remove-weapons or drop cleanup. Idempotent native-serial-handle ownership checks; never recreate duplicates, mutate another player's knife, or retain handles across map lifetime. Verify actual CounterStrikeSharp knife APIs and current dependency documentation before implementation.

Add !knife and !knife <allowlistedtype>, bounded exact type list only; selection local per player per match. No arbitrary entity strings, economic value, extraction value, SSG calls or production behavior. Variants require dedicated private-LAN test fixture, enabled, enrolled actor, current scope and money hard-off. Normal/public/OFF commands fail closed. Default knife invariant follows the same approved fixture scope. Bots get default knife without human commands.

Need extracted allowlist/ownership/idempotence/scope policy tests and native spawn/inventory/round-cycle/cleanup tests. Separate independent review and coordinated target-only deployment. Existing native bot weapons are not modified by current bot fix; human lobby removal currently grants only tracked Glock. Thus current build does NOT claim universal default-knife invariant.
