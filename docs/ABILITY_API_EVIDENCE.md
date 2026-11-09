# Ability API evidence audit

INCOMPLETE. New interpreted native admission and pre-admission anchor creation are disabled by NativeApiEvidenceComplete=false. Offline only. No target integration or push/live action.

Official upstream checkout v1.0.379 at f58e383b0553be16c0f8c6dd68a8194de17691e9 was read locally and is clean. The referenced build assembly informational version matches this source commit exactly (reflection); actual server binary and gamedata equivalence is NOT established. Source signature evidence is not native acceptance. No build or mock test fills that gap.

This ledger inventories audited boundary groups, not an exhaustive completed per-call audit. Unlisted calls and dependencies remain unverified. Registering inherited legacy listeners is not asserted fully audited. C#/BCL and all generated event payload accessors remain pending detailed call-site verification.

Structured exact symbols, pinned source URLs, assumptions and disabled status are in ABILITY_API_EVIDENCE.json.

## Utilities.CreateEntityByName<T>(string)
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Utilities.cs#L59
Calls UTIL_CreateEntityByName(name,-1), wraps returned pointer. Generic constraint CBaseEntity.
Unproved: Does not establish classname/subclass correctness, inertness, pre-spawn safety or native allocation failure behavior.
Disabled new effect path: True

## CBaseEntity.DispatchSpawn()
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L70
Valid entity guard then NativeAPI.DispatchSpawn(handle,zero).
Unproved: No guarantee marker or damage fields survive spawn; reentry and pre-spawn configuration require engine/source proof.
Disabled new effect path: True

## CBaseEntity.Teleport(Vector?,QAngle?,Vector?)
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L15
Nullable position/angle/velocity forwarded; at least one specified. Guard checks valid entity.
Unproved: Velocity-only signature is valid; no proof of dash movement/collision/knockback outcomes or unchanged position in all engine cases.
Disabled new effect path: True

## CEntityInstance.Remove()
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L35
Valid guard then UTIL_Remove(handle).
Unproved: Does not promise synchronous invalidation; immediate IsValid check is conservative pending, not proof of failed engine removal.
Disabled new effect path: True

## CEntityInstance.AddEntityIOEvent
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L104
inputName,activator,caller,value,delay seconds,outputId; forwarded to native queue.
Unproved: Exact Kill input support, serial-safe deferred target retention, delivery after unload and map transition not proved. No unload-expiry guarantee.
Disabled new effect path: True

## CEntityInstance.IsValid/DesignerName
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L26
Handle validity plus nonzero pointer; DesignerName reads identity if valid.
Unproved: Not thread-safety or immunity to stale handle reuse; marker/class/process guards remain app-level assumptions.
Disabled new effect path: True

## CHandle<T>(uint),Raw,Value/Get
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Modules/Utils/CHandle.cs#L49
Raw uint includes index+serial; Get resolves EntitySystem by handle and constructs wrapper. Raw setter may write backing memory.
Unproved: Lifetime must remain within valid native entity; no persistent-global identity across map/server restart.
Disabled new effect path: True

## CEntityIdentity.Name
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CEntityIdentity.g.cs#L27
Schema string getter/setter for m_name.
Unproved: Random marker persistence through spawn/map changes not promised; constructor and aliasing still native contracts.
Disabled new effect path: True

## CBaseGrenade.Damage
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseGrenade.g.cs#L59
Generated schema field or handle accessor; no guessed offsets.
Unproved: Not permission to initialize a directly-created grenade; zero damage/radius does not prove no blind/smoke/team side effects.
Disabled new effect path: True

## CBaseGrenade.DmgRadius
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseGrenade.g.cs#L47
Generated schema field or handle accessor; no guessed offsets.
Unproved: Not permission to initialize a directly-created grenade; zero damage/radius does not prove no blind/smoke/team side effects.
Disabled new effect path: True

## CBaseGrenade.DetonateTime
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseGrenade.g.cs#L51
Generated schema field or handle accessor; no guessed offsets.
Unproved: Not permission to initialize a directly-created grenade; zero damage/radius does not prove no blind/smoke/team side effects.
Disabled new effect path: True

## CBaseGrenade.Thrower
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseGrenade.g.cs#L79
Generated schema field or handle accessor; no guessed offsets.
Unproved: Not permission to initialize a directly-created grenade; zero damage/radius does not prove no blind/smoke/team side effects.
Disabled new effect path: True

## CBaseGrenade.OriginalThrower
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseGrenade.g.cs#L87
Generated schema field or handle accessor; no guessed offsets.
Unproved: Not permission to initialize a directly-created grenade; zero damage/radius does not prove no blind/smoke/team side effects.
Disabled new effect path: True

## CTakeDamageInfo.Damage/BitsDamageType/Attacker/Inflictor
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CTakeDamageInfo.g.cs#L51
Generated accessors wrap existing native pointer.
Unproved: Borrowed hook object only; no allocator, constructor, destructor, alignment or ownership proof. Never retain hook pointer.
Disabled new effect path: True

## CTakeDamageInfo(IntPtr)
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CTakeDamageInfo.g.cs#L19
Wraps supplied pointer; does not construct native damage state.
Unproved: Native allocation/invocation remains disabled. Schema size alone and zero-filled buffer are insufficient.
Disabled new effect path: True

## Listeners.OnEntityTakeDamagePre
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Listeners.g.cs#L48
CBaseEntity,CTakeDamageInfo; Handled or greater prevents entire damage application.
Unproved: Native hook sequencing, borrowed lifetime and installed version require separate verification; no damage driver proven.
Disabled new effect path: True

## BasePlugin.RegisterEventHandler<T>
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L163
Default HookMode.Post; handler GameEventHandler<T>(event,GameEventInfo).
Unproved: Post hurt is telemetry after damage, not cancellable authority. Event payload ownership not fully audited.
Disabled new effect path: False

## BasePlugin.RegisterListener<T>
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L280
Requires named listener delegate; wrapper converts callback args.
Unproved: Remaining native forwarding and teardown implementation still to audit.
Disabled new effect path: False

## BasePlugin.AddTimer
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L368
Seconds,Action,nullable TimerFlags; Timer stored in plugin timers.
Unproved: Not independent of plugin unload. No new timer assumed to survive unload.
Disabled new effect path: True

## Utilities.SetStateChanged
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Utilities.cs#L231
Checks schema networked field; warns/returns when not networked; derives schema offsets.
Unproved: Does not validate game semantic ownership or persistence. Exact class/field must be audited for every use.
Disabled new effect path: True

## CBaseEntity.Health
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs#L67
Generated schema accessor, not custom raw native memory.
Unproved: Setter availability alone does not prove gameplay correctness, replication, stacking or restore ownership.
Disabled new effect path: True

## CBaseEntity.MaxHealth
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs#L71
Generated schema accessor, not custom raw native memory.
Unproved: Setter availability alone does not prove gameplay correctness, replication, stacking or restore ownership.
Disabled new effect path: True

## CCSPlayerPawn.VelocityModifier
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L319
Generated schema accessor, not custom raw native memory.
Unproved: Setter availability alone does not prove gameplay correctness, replication, stacking or restore ownership.
Disabled new effect path: True

## CCSPlayerPawn.EyeAngles
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L459
Generated schema accessor, not custom raw native memory.
Unproved: Setter availability alone does not prove gameplay correctness, replication, stacking or restore ownership.
Disabled new effect path: True

## Explicitly disabled / absent

Native damage creation and TakeDamageOld invocation, fixed-size native damage buffers, bare health-subtraction damage, raw offset writes, native knockdown, unowned weapon-to-projectile conversion and guessed audio/collision fields remain disabled or unimplemented.
All interpreted effects now fail admission while evidence review is incomplete, even ones with known signatures. The owned grenade router already rejects smoke/HE/flash. Delayed Kill is a verified queue API signature, NOT proven post-unload cleanup.

## Next audit scope

Verify reference assembly version/hash against pinned CSS source; enumerate every new external invocation/property through semantic symbol binding; audit native implementation of queue/handle/remove and BasePlugin listener teardown. Audit server schema fields, smoke color/owner, player and event accessors, chat, commands, capabilities, movement networking, map listeners and all timer uses. Verify BCL persistence/process APIs separately. Keep runtime QA separate from source evidence.

## Native implementation follow-up

AddEntityIOEvent

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/src/scripting/natives/natives_entities.cpp#L558

Missing signature reports native error. Reads float delay argument5 and delegates target raw pointer + variant to engine queue. Does not prove engine target lifetime/serial retention or Kill support.

EntitySystem.GetEntityByHandle<T>

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Modules/Entities/EntitySystem.cs#L28

Checks foundHandle.Raw equals requested.Raw before returning pointer. This supports full serial matching within this implementation; does not prove reference assembly/server equivalence.

BasePlugin.Dispose(bool)

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L684

Disposes subscriber dictionaries and kills all stored timers. Plugin timers cannot provide cleanup after disposal. No assumption that listeners survive failed custom Unload.

Timer(float,Action,TimerFlags?),Kill()

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Modules/Timers/Timer.cs#L39

CreateTimer then NativeAPI.KillTimer. Flags repeat bit0/mapchange bit1. Engine scheduling and callback disposal not fully audited.

Reference API assembly SHA256 652ac51d86a963d3cf795bad9271f821579a3bb65bc871313b900ba834f9d1ec. Hash recorded; reflection confirms referenced assembly self-reports the exact source commit. This is not reproducible binary proof or actual server/gamedata verification.

## Semantic call-site inventory

ABILITY_API_CALLS.json records 1,629 external invocation/constructor/member-access occurrences, including 1,060 CSS occurrences across 195 distinct constructed symbols. All have explicit pending review state. Roslyn binding compiled with zero errors against the hashed reference API. This includes legacy paths and is not proof every boundary is new; implicit operators/indexers still need inventory. Reproduction code is tools/api-audit/Inventory.cs.txt.
