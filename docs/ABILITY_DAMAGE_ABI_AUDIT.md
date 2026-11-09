# Native damage construction audit

The installed CSS API exposes Schema.GetClassSize(string) and
CBaseEntity_TakeDamageOld(CEntityInstance, CTakeDamageInfo, CTakeDamageResult).
Reflection receipt: /home/kit/out/ability-damage-api-surface.txt.

Source checked:
https://github.com/roflmuffin/CounterStrikeSharp/blob/main/managed/CounterStrikeSharp.API/Modules/Memory/VirtualFunctions.cs
https://github.com/roflmuffin/CounterStrikeSharp/blob/main/managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs
https://github.com/roflmuffin/CounterStrikeSharp/blob/main/managed/CounterStrikeSharp.API/Core/Model/CTakeDamageInfo.cs
https://github.com/roflmuffin/CounterStrikeSharp/pull/1147

VirtualFunctions dispatches the signature from GameData. Schema delegates class
size lookup to NativeAPI. Neither source provides a damage-info/result native
constructor, destructor, alignment contract or lifetime guarantee. The partial
damage-info model only supplies GetHitGroup, which itself dereferences internal
pointers. A zero-filled block sized from Schema is therefore NOT a validated
constructed object. Client schema sizes, 0x88 allocations, memcpy of a callback
object, null result pointers and raw Health writes are not used.

AttributedAbilityDamage is a resident-owned transaction contract, NOT a native
implementation. It consumes event identity before an engine call, rejects
reentry/off-thread invocation, distinguishes rejected/indeterminate outcomes,
and never retries ambiguous calls. AbilityDamageAuthorization explicitly
requires current generation, full participant identity, owned inflictor,
hostility, financial admission, active living participants and open outcome.
Tests exercise the shared production primitives, not engine ABI.

The driver remains UnvalidatedAbilityDamageDriver and is not published as an
enabled damage capability. Binding these checks to live observations and
implementing a native driver remain outstanding. No claim of resident
enforcement or completed magnetic/cannon damage is made.

Acceptance requires exact installed server build and CSS binary provenance;
server-side sizes AND alignment; proved initialization/destruction of both
native classes; synchronous game-thread lifetime; full serial attacker,
victim and inflictor attribution; reentry behavior; native armor/death events;
Raid hostility/financial policy and exactly-once outcome settlement; supported
layout rejection. These cannot be proved by the offline mocks.
