> SUPERSEDED numeric/admission claims: see ABILITY_STRICT_NUMERIC_CORRECTION.md. All26 activation paths are held; prior CS2 tuning is withdrawn, not authorized.

# Remaining SOL effects after scoped mobility implementation

The former ten unimplemented instances were 4008007 for each operator1/2/3/4/5, operator1/20010201, operator3/1009003, operator3/1009005, operator3/10010301 and operator4/40010202. The last five are distinct skill IDs, not interchangeable operator aliases.

Operator3/10010301 now has functional offline cooperative scoped movement. NativeScopedMobility follows admission and scoped state, acquires one movement lease, restores on unscoping/admission loss and retains cleanup on failure. NativeScopedMobilityAdapter wires the function into actor creation/replacement, but ScopedMobilityActivation remains false. IsScoped source is pinned below. The 1.1 modifier is a dimensionless CS2 assumption, not DF buff303054. No blanket DF-uncertainty prohibition was used. Non-scoped ADS and global scalar ownership remain outside the interpretation.

Nine instances remain without skill-specific effects:

| Operator | SOL SkillID | Attempt and precise present blocker |
|---|---|---|
| 1 | 4008007 | Exact escrow release exists, but no DF arrow creation/attachment or per-victim owned-arrow identity exists. Refusing to reinterpret this as deleting an arbitrary smoke/entity. |
| 2 | 4008007 | Same exact ownership prerequisite; private recon creates no arrow. Existing Release(ticket,boundary) can remove only a proven ticket, not manufacture one from classname/index. |
| 3 | 4008007 | Same prerequisite. Magnetic bomb is not an arrow and cannot be silently substituted. |
| 4 | 4008007 | Same prerequisite. Flash projectile foundation is not an attached arrow. |
| 5 | 4008007 | Same prerequisite. Trap interpretation is a tick/radius test, not an arrow entity. |
| 1 | 20010201 | No Raid downed/revive lifecycle exists. Existing committed deaths settle outcomes. Heal cannot revive a dead pawn and no sourced restore-life operation/protocol has been verified. Requires a distinct gameplay lifecycle, not a pointer/schema write. |
| 3 | 1009003 | Cannon entity/launch behavior not implemented. Generic zero-damage projectile foundation does not implement a cannon; native damaging projectile lacks resident enforcement and safe unload lifetime. No validated damage object constructor. A text or generic HE substitute would not be represented as cannon function. |
| 3 | 1009005 | No verified magnetic attachment/detachment boundary or collision contact/lifetime contract. Zero-damage generic grenade is not magnetic behavior. Damage creation and unload-independent lifetime remain separately blocked. |
| 4 | 40010202 | Reviewed listener API has no established footstep-specific cancellable contract. Generated MadeFootstepNoise/Footsteps/EmitSoundTime fields expose storage only, not an audio suppression contract. UserMessage's documented sound_type example is FireBullets, not proof all footstep audio can be intercepted. No writes to these fields or invented sound message IDs are used. |

These are blockers in the currently verified implementation/API set, not a proof no future supported extension can implement them. Revive/arrow ownership are gameplay prerequisites rather than unsafe ABI claims. Skill-specific smoke, fragmentation and flash behavior is also incomplete for five additional projectile-foundation-only instances, independently of these nine.

## Pinned official sources actually inspected

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L265-L267

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L285-L287

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayer_MovementServices.g.cs#L133-L139

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Modules/UserMessages/UserMessage.cs#L145-L175

https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/managed/CounterStrikeSharp.API/Core/Listeners.g.cs

Lifetime, damage construction and cooperative restoration limits are source-linked in ABILITY_EFFECT_BOUNDARIES.json and ABILITY_STATUS_PROJECTILE_CLOSURE.md. No live acceptance is required to compile the scoped interpretation; activation remains held independently.
