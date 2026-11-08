Private spawn hull investigation
================================

Console-only commands, private port 27016 and df_spaceport_v261 guarded.
No event listeners, timers, player teleport, automatic entity edits, or unload removal.

Build with /home/kit/extract-zone-sdk/dotnet build -c Release.
css_spawn_hull N reads original row N, the actual matching entity, player feet,
a 32x32x72 standing hull with 1u upward sweep, candidate offsets 100 down to 0,
and support rays. Native tracing only runs inside the console command callback.
Player-inclusive traces can hit nearby player bodies; check pawn-ignored player
floor readings, or repeat after that player moves clear, before interpreting
a ray as world floor.

css_spawn_hull_fix N changes only the three independently audited blocked
rows, rechecking expected baseline, unique class/XY identity and hull clearance.
No XY changes and no player movement.

Nov 7 2026 live result, all 32 enabled, 16 T and 16 CT, 32 standing hulls clear.
29 retain original +100. Row 13 T (3523.6758,3953.8477) and row 18 CT
(3523.6758,3889.8477) use +97, Z 110.00009. Row 26 CT
(3587.6758,3953.8477) uses +56, Z 69.00009.
Their original Z is 13.000092 and their support floor is Z 9.00009.
These are highest clear whole-unit offsets within original through original+100.
Static clearance and ground rays were verified, not a full falling hull sweep.

Receipts on desktop D:/raid-private-flow-20261107/spawnhull/
summary.json, all32-initial.json, all32-final.json, target-initial.json,
helper-persistence.json, after-helper-persistence.jsonl.
RCON replay from the existing private container:
python3 /debugtest/reports/nocollide-rcon.py "css_spawn_hull 13"

Persistent helper has FixedLift overrides for those three points in creation
and idempotent apply paths. Helper DLL atomically replaced on disk only after
verifying PluginHotReloadEnabled=false, preserving old DLL on D.
Running helper was never unloaded/reloaded; its reported live version remains
1.2.0-lift100. Next load uses 1.2.1-hullfixed. Do not run the currently loaded
old css_v24_spawn32_lift100 because it would reset the three live exceptions.
Audit B remains console-only; older audit A autoload DLL is renamed disabled.
