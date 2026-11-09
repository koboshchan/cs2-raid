# Authorized r2 interpretation, offline only

Raw r2 is unchanged. Raid-mode slot rows are chosen, not MP/BombDefusal. This layer is interpreted, not a claim of DF-equivalent effects. Zero Max with weapon-count ammo is modeled as one ability token; no free unlimited casting. Init remains literal (0 starts charging). Seconds are inferred from Time and TimeRecoverSec field families. Non-CD skills receive a one-second anti-repeat guard, not an archive cooldown. Recharge is sequential, one token per Time; score/perk recovery is not implemented.

| Source skill / field | Raw | Chosen representation | Evidence / confidence / assumptions |
|---|---|---|---|
| 2008002/CDID | 2002 | 2002 | integer join, high |
| 2008002/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 2008002/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 2008002/Time | 60.0 | 60.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 2008005/CDID | 2001 | 2001 | integer join, high |
| 2008005/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 2008005/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 2008005/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010101/CDID | 2 | 2 | integer join, high |
| 20010101/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 20010101/Max | 3 | 3 | literal count except 0=weapon-backed one token, medium |
| 20010101/Time | 30.0 | 30.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010201/CDID | 0 | 0 | integer join, high |
| 20010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 20010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 20010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010215/CDID | 0 | 0 | integer join, high |
| 20010215/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 20010215/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 20010215/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 2002017/CDID | 14 | 14 | integer join, high |
| 2002017/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 2002017/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 2002017/Time | 75.0 | 75.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008002/CDID | 92 | 92 | integer join, high |
| 3008002/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008002/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008002/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008002/CDID | 13 | 13 | integer join, high |
| 4008002/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008002/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 4008002/Time | 50.0 | 50.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 40010201/CDID | 0 | 0 | integer join, high |
| 40010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 40010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 40010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009001/CDID | 18 | 18 | integer join, high |
| 1009001/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009001/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009001/Time | 20.0 | 20.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009002/CDID | 16 | 16 | integer join, high |
| 1009002/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009002/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009002/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009003/CDID | 17 | 17 | integer join, high |
| 1009003/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009003/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009003/Time | 70.0 | 70.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009004/CDID | 0 | 0 | integer join, high |
| 1009004/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 1009004/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 1009004/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009005/CDID | 16 | 16 | integer join, high |
| 1009005/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009005/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009005/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 10010301/CDID | 0 | 0 | integer join, high |
| 10010301/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 10010301/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 10010301/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1002016/CDID | 22 | 22 | integer join, high |
| 1002016/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1002016/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 1002016/Time | 90.0 | 90.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4009002/CDID | 20 | 20 | integer join, high |
| 4009002/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 4009002/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 4009002/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4009003/CDID | 21 | 21 | integer join, high |
| 4009003/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 4009003/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 4009003/Time | 45.0 | 45.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 40010202/CDID | 0 | 0 | integer join, high |
| 40010202/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 40010202/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 40010202/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008001/CDID | 11 | 11 | integer join, high |
| 3008001/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008001/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008001/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008008/CDID | 15 | 15 | integer join, high |
| 3008008/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008008/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008008/Time | 45.0 | 45.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010101/CDID | 12 | 12 | integer join, high |
| 30010101/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 30010101/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 30010101/Time | 90.0 | 90.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010201/CDID | 0 | 0 | integer join, high |
| 30010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010202/CDID | 1030 | 1030 | integer join, high |
| 30010202/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010202/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010202/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |

## Executable offline CS2 ports

UseInterpretedAbilities defaults true in source. This is NOT deployed configuration.
V/X route to explicit source IDs; !ability <id> exposes other source abilities. !cd
reports imported charges. Shared IDs use (operator, skill), never a global ID key.
Old handlers remain available only with UseInterpretedAbilities=false. Imported mode
bypasses legacy passives, damage/blind/grenade handlers, cooking, shared V/X cooldowns
and legacy tick effects. Fixture-v2 remains deliberately unaccepted and unavailable;
this work does not waive its independent native acceptance requirements.

Actual CSS implementations live in NativeAbilityPorts.cs, not the synthetic router.
Stinger 20010101 writes Health and replicates m_iHealth; self-heal only, no revive or
aimed hormone projectile. 2008002/2008005 grant a native smoke grenade using
GiveNamedItem<CBasePlayerWeapon>; native throw/fuse/smoke, not a DF healing hive.
Luna 2002017 and Hackclaw 1002016 use current pawn origins and custom-enemy policy,
then owner-only PrintToChat contact telemetry. These are instantaneous no-LOS scans,
not arrow flight, persistent tracking, team outlines or a deciphering animation.
Luna 3008002 and Shepherd 3008008 grant a native HE; raid-mode HE delivery is BLOCKED
until native grenade custom-team damage enforcement is verified, with no token spent.
Hackclaw 4009003 grants a native flashbang, not an autonomous tracking drone.
Vyron 1009001 calls pawn.Teleport with velocity only and preserved Z velocity.
No origin teleport, collision disable, vertical launch or timed collision restoration.
Shepherd 3008001 installs a game-thread proximity trap, checks custom enemy policy,
writes/replicates VelocityModifier once, and restores the prior value after slow/end.
It is an invisible local sensor, not an explosion, rendered device or destructible prop.
It avoids stacking and uses serial-pawn and compare-before-restore guards; other plugins
writing identical modifier values still cannot be distinguished by engine properties.

NativePorts configuration defaults are explicit CS2 balancing choices, NOT decoded
DF magnitudes: HealHp25; ReconRadiusSourceUnits1200; RushSpeedSourceUnits400;
TrapRadiusSourceUnits160, TrapLifetimeSeconds30, TrapSlowSeconds2,
TrapVelocityModifier0.5. All adapters validate numeric bounds before touching CSS.
RushData's partially typed 1000cm/1s evidence is NOT silently converted to these values.
Smoke, HE and flash engine radius/fuse/effect duration are stock CS2 and are not exact
DF values. No legacy Config damage, scan, movement or cooldown values are reused.

Blockers are per ability, visible to !ability and !cd. Luna4008002 needs electric
projectile/status/damage rows and safe native damage path. Vyron1009002/1009005 needs
magnetic attachment and explosion data, 1009003 cannon projectile/damage. Hackclaw
4009002 needs knife projectile/electronic suppression rows. Shepherd30010101 needs
sonic destruction/damage definitions. Passives need actual Buff rows (e.g.8009,
8011/100008,303016/8013,100020/202001,401001/8010), triggers and magnitudes, not just
AddBuffs IDs. 4008007 has no owned arrow to remove. 30010202/30010203 need repair
target/resource/rate semantics. 4008010 is missing altogether. These reject and spend
nothing; they do not stop supported skills of the same operator.

Sessions retain charges through death/disconnect/pawn replacement and prevent
operator switching to replenish resources. Serial pawn changes revoke owned effects
before rebinding. Raid phase loss revokes effects without resetting charges. Only
full caller-owned reset clears successful-cleanup sessions; standalone round reset is
explicit. Native exceptions quarantine the actor, failed cleanup retains ownership for
retry. Granted weapon handles use full entity serials, including dropped grants, and
are removed at lifecycle end; converted in-flight grenade projectiles remain engine-
owned and are NOT proven cancellable by weapon removal. Do not treat these grants as
accepted for fixture-v2 or claim strict projectile rollback. Trap has no external timer;
tick polls are removed on cleanup. Instant health/recon/velocity are committed actions,
not undone on lifecycle revoke. No live reload/config toggle acceptance tested.

Offline verification: tests/InterpretedAbilities has 234 focused routing/token/cleanup
checks, including every imported skill, operator-slot mappings, cross-operator denial,
transactional rejection, exception quarantine and failed-cleanup retry. These use fake
ports and do not pretend to execute native CSS. The complete DeltaOps DLL compiles
against installed CSS API with zero warnings/errors. Live/native gameplay, client
visuals, drop/throw/projectile lifecycle, custom-team damage and concurrency with
other movement plugins remain unverified. No live load/restart/push authorized here.

## Follow-up ports and source-path integration audit

Supersedes the corresponding blocked entries above. Electric4008002 and sonic30010101
now have an explicit nearby pulse approximation: current custom enemies within
PulseRadiusSourceUnits240 receive a 0.65 VelocityModifier for PulseSeconds2. No
projectile, damage, destruction or electric damage-over-time is claimed. Knife4009002
uses the same pulse plus imported-ability suppression keyed by victim pawn serial;
it does not silence all native weapons or affect unrelated plugins. These values are
CS2 port tuning, not exact raw scalars. No-target use is transactional rejection.
Cleanup restores owned changes, removes suppression reference counts and tick action;
exception fences quarantine the actor. This implementation intentionally avoids unsafe
CTakeDamageInfo allocation or health subtraction masquerading as attributed damage.

Shepherd passive30010201 now has an OnEntityTakeDamagePre port reducing positive
incoming damage by configured multiplier0.9, only on admitted living Shepherd actors.
It cannot increase damage, undo a Handled result, or override Raid policy. It is not a
claim of exact DF explosion-only resistance or its scalar. Other passive blockers are
individual: Stinger needs a downed/revive state that Raid does not have; Luna's global
Glow would reveal contacts to every squad; Vyron fall/recovery/ADS behavior needs
fall-damage and movement baseline ownership; Hackclaw footsteps need per-recipient
sound control, not a speed multiplier. Exact buff magnitudes remain unexported.

Magnet1009002/1009005 still lacks a safe owned attachment/explosion implementation.
Cannon1009003 lacks an attributed projectile/damage implementation. Native damage
pre-hooks are available and cancellable, but this is not an allocator or constructor
for CTakeDamageInfo. Installed historical DealDamage uses an unvalidated layout/ABI
and stays disabled. A native HE would be a grenade approximation, not a cannon or
magnetic bomb; it remains blocked in Raid until the projectile owner-to-custom-team
attribution path is accepted. Damage must never be emulated by bare Health subtraction,
which bypasses Raid death outcomes, attacker attribution and policy hooks.

Official API checked: https://docs.cssharp.dev/api/CounterStrikeSharp.API.Core.Listeners.OnEntityTakeDamagePre.html
Documents Handled as cancelling entire damage application. chub had no CSS entry.

Read-only integration target cs2-raid-build:/home/kit/extract-zone-flow is clean at
raid-flow-v2 bbff2eb. This worktree descends from bbff2eb; it has NOT been merged back.
The parent-owned target was not edited. Eight prior commits include initial scaffold,
roster and import; do not cherry-pick only the newest ports without their dependencies.
DeltaOps/Raid primitive capability names/signatures remain compatible. In this worktree
LoadGameplay reads gameplay.json then requires GameplayConfig.Enabled (default false).
Ready additionally requires NativeFlowVerified, CoordinatesConfirmed and valid spawn
coordinates; this change does not set them. DeltaOps ConfigureRaidBridge reads sibling
RaidPlugin/gameplay.json. Fixture/TestServer mode returns before native registration.
Nonfixture imported mode defaults true, routes skill slots through UseNativeAbility and
NativeAbilityRouter; old mode false continues legacy effects. RaidActive uses capability
operation3 (Active + living); selection op0 is CharacterSelect; target operation2 uses
CanDamage; full reset operation4 is Lobby. Financial freeze checks remain Raid-side.
NativeAdmission fences human controller and exact pawn serial, life, suppression and
RaidActive. No source setting or test pass proves the currently installed server uses
this path. Fixture-v2 is still a separate, unavailable adapter, not silently enabled.

Fresh after this change: 239 router/lifecycle/passive-rule checks PASS; DeltaOps and
RaidPlugin Release build zero warnings/errors; Raid suites PASS including10527 flow,
261 testmode,166 bot-combat and all other existing suites. Raid test project emits its
existing CS0649 PrivateRehearsalHardOff warning. These tests do not prove native pulse,
trap, projectile cleanup, footsteps or rendered abilities. Native acceptance remains
for separate review/load approval; no target worktree merge, push or live operation.
