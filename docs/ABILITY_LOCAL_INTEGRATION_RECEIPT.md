# Safe partial local integration receipt

The intended /home/kit/extract-zone-flow worktree on raid-flow-v2 was fast-forwarded locally from bbff2eb3457cd502a0d84463242456df704875f1 to 8423ee5 after checking both worktrees clean, exact target branch/base, ancestor relationship and no matching active dotnet/project writer processes. Prior competing workers remain held under the exclusive owner handoff. No remote push, plugin load, server restart or deployment occurred. The separate /home/kit/raid-flow-v2 clone remains d700a860cca94b957b9cf5b92580f645616d3005 and was not modified.

This is safe partial source integration, NOT full DF completion. The next receipt-only commit is integrated by the same guarded local fast-forward.

## Actual 26-entry coverage

11 operator-skill instances have partial skill-specific offline interpretations: one committed heal, two private names-and-distances recon paths, one velocity impulse, four status effects, two borrowed pre-damage modifiers and one post-hit telemetry effect. Five more instances share owned-projectile foundation but have no complete skill-specific behavior. Ten have no implemented skill-specific effect. No entry is claimed fully DF-equivalent. Missing 4008010 is excluded in addition to these unresolved behaviors.

The exact per-operator/skill list and gaps are in ABILITY_SOL26_COVERAGE.md and ABILITY_SOL26_IMPLEMENTATION_MATRIX.md. Per-entry source refs, implementation/admission/safety/fidelity axes and typed units are in ABILITY_SOL26_COVERAGE.json. Raw r2 evidence is unchanged; SOL binding and SkillID-field indexing are tested separately.

## Short interpretation table

| Boundary | Implemented interpretation | Fidelity/safety boundary |
|---|---|---|
| Healing | CS2-only instant 25 HP cap; owned notification retry, no HP replay/rollback | Not targeting/ADS/sustained heal or smoke conversion; activation held |
| Recon | Private names and Source-unit distances, default radius 1200 | No printed XYZ, LOS, arrow, sensor or DF marker mechanics |
| Rush | Finite yaw-degree velocity impulse, default 400 Source units/second, original Z retained | No origin displacement, knockdown or DF speed claim; activation held |
| Status | Timed scalar lease groundwork plus explicit cooperative reservation/session cleanup | Cooperative boundary not wired globally; identical-value nonparticipant and unload rollback gaps remain; activation held |
| Passive reductions | Borrowed engine pre-hook; Shepherd extracted 0.7 blast factor; Vyron CS2-only 0.5 fall factor | No damage constructor; installed-engine/hook QA and activation held |
| Hit telemetry | Private allied post-hit XYZ text assumption | Distinct from names-and-distances recon; DF trigger/scope unproven; activation held |
| Projectiles | Durable full identity escrow, pre-ticket cleanup, single-attempt transaction | Zero damage/radius; no verified inert spawn or post-unload expiry contract; creation/activation disabled |

The cooperative alternative is implemented and tested but does not establish universal writer participation. No lingering damaging projectile is enabled. DetonateTime/ThinkFunctions are not invented lifetime schedulers. Unsupported lifetime and damage-construction contracts remain disabled.

## Tests actually run

Interpreted lifecycle/router: 3179 checks. Archive: 104. Attributed damage boundary: 4177, explicitly not native ABI acceptance. Exact scaffold: 95. Fixture policy: 85. Python import/binding/passive evidence: 25 tests.

Integrated target Raid suites: 19 rules, 10527 flow, 41 settlement, 261 testmode, 166 bot, 111 durable settlement, 193 native-buy, 39 reload, 52 link, 61 outcomes, 227 spawn, 60 rehearsal, 92 safety checks passed. Raid test compilation has existing CS0649 PrivateRehearsalHardOff warning. Both plugin Release builds pass with zero warnings/errors. Integrated target DeltaOps build and lifecycle tests were rerun after fast-forward.

## Evidence and exclusions

Named API contracts and pinned official URLs are in ABILITY_EFFECT_BOUNDARIES.json/.md; comprehensive semantic inventory candidates remain in ABILITY_API_CALLS.json and are not all engine-contract proofs. Damage ABI exclusions are in ABILITY_DAMAGE_ABI_AUDIT.md. Status/projectile feasibility attempts are in ABILITY_STATUS_PROJECTILE_CLOSURE.md.

Unimplemented revive/downed lifecycle, ADS movement, silent footsteps, magnetic attachment, cannon, arrow removal and skill-specific projectile behaviors remain real gaps. This receipt does not rename them completed or pretend 4008010 is the only exclusion. No native QA was performed.
