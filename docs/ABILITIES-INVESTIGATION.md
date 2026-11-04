# DeltaOps ability investigation

Scope: isolated source fix only, based on raid-gameplay 4f374638a93dea095aba52e38d40f00102239284. No server start, deployment, main update, container change, or volume write.

## Proven shared input defect

The original stopped server stdout contained 24,165 lines, including 3,066 occurrences of "bind: no inputsystem for bindings". The plugin attempted both ExecuteClientCommand and ExecuteClientCommandFromServer for V/X on connect, spawn and selection, plus retries. Those calls cannot reliably edit local client input bindings. CounterStrikeSharp documents the first as limited to FCVAR_CLIENT_CAN_EXECUTE commands and the second as executing the server command handler in player context.

Reference: https://github.com/roflmuffin/CounterStrikeSharp/blob/main/managed/CounterStrikeSharp.API/Core/Model/CCSPlayerController.cs

The fix removes remote bind attempts/retry timers and false success messages. Players can use !v / !x immediately, or run bind "v" "css_v"; bind "x" "css_x" themselves in their client console. !bind always prints instructions. The legacy AutoBindSkillKeys setting now controls one-time automatic help only.

## Not evidence that every ability implementation is broken

Read-only extraction of the stopped container's CounterStrikeSharp logs found 1,002 distinct lines after deduplicating per-plugin and combined files. Handler-success messages exist for nine skill routes across all six characters. Counts: Vyron V 167 / X 77, D-Wolf V 19 / X 24, Stinger V 9 / X 10, Luna V 1, Hackclaw V 2, Shepherd V 6. These are handler return values, NOT client-visible effect verification. No plugin exception/error lines were found; the only two warning lines concerned a missing admin data file.

No success line for a route is not proof of a defect. No speculative native damage, glow, smoke or grenade rewrite has been made.

## Selection change

All six characters remain in their original order. !characterselect prints the complete numbered chat list; !characterselect <1-6> selects one. Legacy !op is retained. The center-screen menu/pagination is removed, while timeout/random fallback, cooldowns, human-player checks and post-warmup selection lock remain. Countdown emits only existing 30/15/10/5-second warnings, not the full roster every second.

## Build acceptance

Built using the dedicated non-production Docker host through the root deltaops-artifacts target. SDK 10.0.403, CounterStrikeSharp 1.0.379. Two Release builds each had zero compiler warnings/errors and identical DLL/deps/PDB. All 29 source-contract checks passed with warnings treated as errors. These check registrations, selection guards/range, roster order, remote-bind removal, bounded reminders and all six V/X dispatch routes. They are not native gameplay tests.

DLL SHA256: 1c6e0a9aa86adf88685096acd985f91dcb689b379247a890d78a9a00b4d5e54d

Build evidence is exported outside git under /home/kit/deltaops-live-fix-artifacts with build logs at /home/kit/deltaops-live-fix-build.log.

## Remaining acceptance

Test !characterselect and !v / !x with a real client before claiming all abilities fixed. Exercise all twelve routes with live targets, native projectiles, smoke, glow visibility and cooldowns. Existing TeamNum targeting is incompatible with raid FFA/duos; the automatic raid-gameplay disable gate is intentionally retained. Native damage offsets and granted-utility economy fencing remain unresolved integration risks documented in plugins/DeltaOps/INTEGRATION.md.
