# Status ownership and projectile lifetime closure attempts

This is an offline feasibility record, not native acceptance or completed DF scope.

## Reversible status

The pinned schema ref and SetStateChanged contracts are recorded in ABILITY_EFFECT_BOUNDARIES.json. They expose scalar reads/writes, not a lease token or writer identity.

Implemented additional defenses: verify current full pawn serial and exact finite captured baseline before the first mutation; reject repeat Apply and Apply after disposal; retain cleanup after failed mutation/notification; preserve a different competing scalar; retry notification without replaying a completed restore. Deterministic tests cover stale baseline, invalid baseline, replay and disposal.

Rejected alternative: treating compare-before-restore as exclusive ownership. An unrelated writer can write the same float, and neither schema ref nor network notification identifies it. Process-local arbitration can coordinate our own effects, but cannot prove ownership against Raid, other plugins or engine updates. A periodic timer does not solve restoration after plugin disposal. Therefore reversible native status activation remains blocked. A private text-only status substitute would be safe but would not implement a movement or suppression effect.

A throwing restore setter also has uncertain outcome; compare-before-restore avoids a subsequent overwrite if another value is observed, but cannot prove which writer produced an identical value. No exact rollback claim is made.

## Projectile lifetime

Read pinned official native implementation:
https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/src/scripting/natives/natives_entities.cpp#L558-L575

AddEntityIOEvent checks a gamedata function pointer, reads raw target/activator/caller pointers, constructs a local variant_t, and delegates into the engine queue. The wrapper does not specify queued target retention, serial reuse protection, Kill delivery or post-plugin-unload execution. Replacing this with a plugin timer cannot survive BasePlugin.Dispose, whose timer/listener removal is separately source reviewed.

Read generated CBaseGrenade.g.cs lines 49-51: DetonateTime is a schema float ref. It is not a documented expiry/think scheduler or proof that setting it on an allocated grenade schedules safe removal. CBaseEntity.ThinkFunctions is generated schema access, not permission to install arbitrary native callbacks. Neither is used as an invented lifetime fix.

Implemented additional defense: a NativeEntityTransaction is single-attempt even when CreateInert throws before returning a ticket. It cannot allocate again and lose its first incomplete allocation cleanup authority. Existing durable identity, failed-unload recovery and exact single-ticket release remain available offline. All adapter blast damage/radius remains zero; the unloaded ability hook is not treated as resident damage enforcement.

Remaining viable approaches require independently sourced engine contract evidence, a separately reviewed resident owner that survives ability unload, or explicit acceptance that a failed unload leaves utility with only next-loader cleanup. No such resident owner is currently implemented. Next-loader durable recovery is not guaranteed expiry while no plugin is loaded. Activation remains blocked for these concrete lifetime/spawn contracts, not merely because DF behavior is unknown.

## Integration decision

No target integration yet: full remaining scope is materially incomplete. The implementation worktree remains the only writer. Target /home/kit/extract-zone-flow was last checked clean at bbff2eb3457cd502a0d84463242456df704875f1; separate /home/kit/raid-flow-v2 is not the target. Fresh ownership, cleanliness and ancestry checks are still required immediately before any eventual local integration.
