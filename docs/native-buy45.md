# Offline native Buy timing, 45 seconds

This supersedes the historical 30-second automatic Buy timing in follow-up notes. It does not certify or enable native purchasing.

## Current-engine units receipt

Primary evidence is the read-only current-engine help/find batch recorded November 4, 2026, commands executed sequentially between 10:09:50 and 10:09:57 America/Vancouver. Individual command timestamps were not captured. Source receipt is cs2-raid-build:/home/kit/out/mp-buytime-current-engine-help.txt.

Engine PatchVersion 1.41.9.5, ClientVersion/ServerVersion 2000940, SourceRevision 11198439, VersionDate Nov 02 2026, VersionTime 16:17:08.

Exact sanitized help response:

    mp_buytime = 45 (default 90, min 0) [game replicated] - How many seconds after round start players can buy items for.

The find command independently reports the same current45/default90 and seconds wording. No maximum was displayed. No cvar write, config change, reload, restart or phase change was used to obtain this receipt. mp_buytime45 means 45 seconds, NOT 45 minutes or 0.75. Older Source-engine minute documentation must not be substituted for this evidence.

## Implementation and ownership

GameplayState.BuySeconds is the shared 45-second duration. BuyOpen(now) is a half-open interval from the Buy deadline minus45 through strictly before the deadline. Production native purchase admission and the isolated fixture's existing buy-command admission both check this absolute deadline, so a delayed Tick cannot allow a late command. Buy expiry still advances when readiness is lost. Countdown5, Freeze10 and the 30-minute active raid remain unchanged.

The existing gated production phase caller anchors GameRules.RoundStartTime once at Buy entry and sets RoundTime45. The isolated fixture phase caller and presentation updates share the same fixed engine-time anchor, keyed by unique match id. Its per-second refresh does not slide the Buy window. The custom center-alert Buy countdown suffix has been removed; the requested countdown belongs to the engine HUD, not HTML. Other phase text and the finite-credit display remain status information, not proof of a native menu.

mp_buytime is set to45 for owned Buy and0 outside Buy, behind the pre-existing production/session or dedicated-fixture admission gates. The runtime CSS type is probed without numeric access; only Float32 and Int32 are supported, each using matching ConVar<T> read/set/restore. Unexpected types fail closed. The engine help receipt establishes units, not native storage type; the offline build does not falsely certify that runtime type.

Original cvars and rules are captured before mutation. Rules restoration uses entity serial plus rules handle identity fences. Fixture OFF/map end/round rebuild/unload use existing generation-fenced cleanup; fresh matches cannot reuse an old Buy clock key. Production lobby reset/unload restore its scoped baseline. A failed restoration remains queued for retry, rather than discarding the original. Existing lifecycle callbacks remain the only callers; no new live engine hook has been proven.

## Gates and limitations

All native-purchase, reload, six-kit, coordinates and financial/native-flow proof gates remain unchanged and OFF. ProductionReady remains blocked. There are no SSG/backend calls or money changes in this patch.

The test fixture still has its existing finite-credit command/grant mechanism. It is not a native B-menu purchase implementation. This patch supplies clock ownership and admission boundaries, not a replacement shop or a claim that both native T/CT menus work. Native buyzone/menu rendering, exact engine menu closure, round-clock interaction, normal/fixture cvar restoration, disconnect and map/unload behavior require explicit runtime acceptance before enabling any gate.

No public/private server was started, restarted, deployed or altered for this offline work. Offline tests prove model boundaries and compile/type compatibility, not native visual acceptance.

## Required runtime acceptance, not performed

On a separately approved isolated runtime, verify T and CT native Buy HUD counts45 down to0, stops at the exact boundary, denies after expiry/death/disconnect, and does not extend on repeated presentation ticks. Verify lobby/reset, Countdown/Freeze/Active transitions, OFF, disconnect, round rebuild, map end/start, unload and stale callbacks restore baseline state without touching another rules entity. Confirm actual cvar type and exact native buyzone/menu closure. Native menu purchases, paid reloads, abilities and financial integration remain separately gated.
