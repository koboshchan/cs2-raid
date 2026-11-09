# Offline integration truth table

This handoff does not authorize starting or deploying a server. Parent alone owns working-branch pushes. Main, SSG and money gates are unchanged. All native acceptance below is pending; source/model tests are not native proof.

| Scope | Implemented offline | Runtime truth |
|---|---|---|
| Reviewed automatic human/bot Lobby, round rebuild and recovery | Retained, deterministic builds | Prior separate native receipts do not certify these new combined binaries |
| Knife | Stock default/t/ct only, fixture ownership tracking and cleanup | Knife display/invariant and death-drop ordering unverified |
| Balance | Authoritative finite fixture wallet, clamped native cash, composed full-number center alert, exact-service snapshot or fail-closed replacement denial | Exact client placement/visibility including spectator and banner contention unverified; not production HUD completion |
| Clock | Typed GameRules phase clock, replication dirty mark, exact native identity restore | Client display and engine expiry side effects unverified |
| Native B menu | Inactive catalog/actor-bound reservation ledger; cancel-before-attempt refund only; ambiguous grant locks Unknown | NOT enabled. Existing custom fixture buy remains fallback, not B-menu acceptance |
| Reload | Inactive catalog/serial/generation-bound state machine and verified-adapter interface | NOT wired. No native ammo writes or TestFlow debit; requested paid reload runtime unavailable |
| DeltaOps | Reviewed disabled v2 scaffold, serial human/bot custom-team policy, provenance/quarantine tests and lifecycle hooks | Original KIT STUB remains. Assignment returns false; all five native kits unavailable |
| Native damage | Unsafe hardcoded 0x88 call removed | Damage abilities disabled in ALL DeltaOps modes, intentional legacy safety regression |
| Utility | BUY utility blocked independently; kit utility bookkeeping is nonredeemable | No fixture kit utility delivered; ownership/revocation unproven |
| Financial recovery | Reviewed durable protocol retained; fixture preflight rejects historical state | External money HARD OFF; no reconciliation economic replay performed |

## Inactive reload contract

ReloadPolicy.IVanillaCatalog resolves a vanilla item quote carrying engine-build and catalog-digest metadata. It must be implemented from the actual engine's authoritative catalog, never the fixture's custom 12-gun price table. No production catalog implementation is fabricated here. Unknown, zero/free or noncatalog/utility items deny paid provisioning until separately decided; this is conservative unavailable behavior, not a newly approved free-weapon rule.

Price is positive integer HALF_UP vanilla buy price /20, using long intermediate arithmetic. Actor identity includes generation, serial-inclusive controller and weapon. Reserve debits this isolated policy ledger once; a busy/unknown actor cannot reserve another input. Begin marks the native attempt. Only cancellation before Begin refunds; duplicate cancellation never refunds twice. Disconnect/reset invalidates the object, refunds unattempted reservations only, and locks attempted ambiguity Unknown. An adapter must never attach the old ledger to a reconnect.

A successful partial-magazine reload is still one charged reload. Proof records exact before/after clip and consumed reserve delta, which must agree. Planned reserve is exactly maxClip during the reserved native attempt, otherwise zero; a native adapter must prove how remaining reserve is normalized after actual consumption. There is no tick-based refill or event-triggered debit. Insufficient balance plans zero. No native ammo guarantee is claimed.

IVerifiedReloadAdapter.Verify validates a CommitProof bound to input, generation, controller, weapon, clip delta and native-operation identity. It is a trust boundary for a future independently verified hook, not a magic public event token. The pure ledger cannot attest native ownership. Mutex serialization plus reentrant verifier fences prevent duplicate reservation/commit. Verifier exceptions invalidate without refunding ambiguous attempts. Duplicate commit is a rejected no-op, never another debit.

## Exact missing native evidence

CSS 1.0.379 typed GiveNamedItem<T>, serial CHandle/EntityHandle.Raw, weapon inventory, CCSWeaponBaseVData.Price and player weapon-service ammo are possible primitives. CanAcquire is eligibility, not committed payment or delivered ownership. item_purchase and weapon_reload events do not prove transactional rollback/cancellation, exact ownership or ordering; suppressing an event broadcast does not cancel engine reload. No build-bound reload-start/commit/cancel gamedata signature or callable ABI was established. No arbitrary signature/offset was invented.

A future adapter must demonstrate native engine build/catalog provenance, all B/autobuy/rebuy/console paths, actual ownership/delivery, reserve/clip consumption, cancellation, disconnect/reset, same-frame repeated input and callback order. Then bind an unforgeable adapter operation identity to the ledger's actor/generation and prove exactly-once delivery before enabling. The interfaces are usable policy contracts; no unconditional-false fake native adapter is installed.

Research references
https://docs.cssharp.dev/api/CounterStrikeSharp.API.Core.CCSWeaponBaseVData.html
https://docs.cssharp.dev/api/CounterStrikeSharp.API.Core.CPlayer_WeaponServices.html
https://docs.cssharp.dev/api/CounterStrikeSharp.API.Modules.Utils.CHandle-1.html
https://docs.cssharp.dev/examples/WithVirtualFunctions.html
https://github.com/roflmuffin/CounterStrikeSharp/issues/377

## Remaining acceptance work, not completed TODOs

Obtain independently verified catalog and purchase/reload adapters before runtime wiring. Prove native ownership/revocation and safe damage ABI separately for each operator, movement/effect lifecycle and delayed callbacks. Run authorized native matrix for T/CT GUI, failure/cancel/phase changes, insufficient balance, repeated input, partial magazines, reserve behavior, real reconnect, bots, two round cycles, OFF/unload restore, banners, HUD/clock and knife. No fake reconnect or native acceptance from compiler success. Read the exact ability worker handoff at /home/kit/out/raid-fixture-abilities-v2-handoff.md for all five BLOCKED operator rows.

Reproduce with root Docker targets raid-artifacts and deltaops-artifacts. Dependency image/CSS archive are pinned; compile/test RUN steps have network=none. Each target compiles twice and compares DLL/PDB/deps. Raid model tests and separate FixtureAbilitiesV2 policy tests are included. Build receipts under /home/kit/out/raid-final-offline-combined record no server start/deploy.

## 45-second Buy follow-up

The automatic Buy duration is now45 seconds, not the earlier custom30. Current-engine help confirms mp_buytime uses seconds. Fixed GameRules clock anchoring, deadline-based purchase admission and typed scoped cvar restoration are implemented offline. See native-buy45.md. Native B-menu rendering/purchasing and runtime lifecycle acceptance are still unverified; no production/native/financial proof gate is enabled.
