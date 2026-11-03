# Delta Ops CS 1.3.0

CounterStrikeSharp server plugin combining standard CS2 gameplay with selectable
operator passives plus independent V and X abilities.

This is a mechanics-inspired fan adaptation. It does not include or redistribute
Delta Force models, textures, audio, weapon assets, or client mods.

## Commands

```text
!op 1 ... !op 6   Select an operator during the 120-second selection window
V                 Use the V ability when its cooldown is ready
X                 Use the X ability when its cooldown is ready
!v / !x           Chat-command fallbacks
!bind             Print the reliable client bind commands
!cd               Show remaining V/X cooldown
!ops              Show operator and cooldown state
!switch           Reopen operator selection during a live match
!switch 1-6       Switch directly to an operator during a live match
!roles            Show all operator names
```

Server-console test command:

```text
css_ops_use <slot> v
css_ops_use <slot> x
```

The slot number is the player ID shown by the `status` command.

The plugin attempts to bind V/X on connect, spawn, and after operator selection.
This survives respawn, team change, and reconnection. CS2 builds can still block
server-issued bind commands; if V/X do not respond, run this once in the game
developer console:

```text
bind "v" "css_v"
bind "x" "css_x"
```

After the live match begins, a selected operator is locked until the match ends
or a server operator runs `css_ops_reset`.

The server sets the HE grenade carry limit to 3. Items granted by abilities are
added to the inventory and immediately selected in the player's hand.

## Roster

1. 红狼 / D-Wolf
   - V: 4 seconds of +35% movement speed
   - X: forward leap and 15 HP recovery
   - Passive: killing an enemy restores 50 HP
2. 蜂医 / Stinger
   - V: heals yourself and nearby teammates for 60 HP
   - X: creates a long smoke trail from five real CS2 smoke segments
   - Passive: +20 maximum health
3. 露娜 / Luna
   - V: scans enemies within 46 meters and reports names/distances
   - X: the enemy most recently damaged by Luna takes five 5-HP damage ticks
     and is outlined for 5 seconds
   - Passive: damage or flash hits outline the target for the whole team for
     2.5 seconds
4. 威龙 / Vyron
   - V: forward jet propulsion
   - X: grants a standard HE grenade; enemies caught in the explosion are
     disarmed and rooted for 4 seconds
   - Passive: +6% base movement speed
5. 骇爪 / Hackclaw
   - V: scans and marks enemies in range; the target receives an alert
   - X: press X to pull the fuse, press X again to throw a timed flashbang
   - Passive: +4% base movement speed
6. 牧羊人 / Shepherd
   - V: places a proximity mine that triggers when an enemy enters its radius
   - X: press X to pull the fuse, press X again to throw a timed HE grenade
     with a larger radius and 50% higher damage
   - Passive: at least 15 armor on spawn

## State behavior

- Human operators are keyed by SteamID64.
- Operator choice survives death, respawn, team change, and reconnect in the
  same server process.
- V and X have independent cooldowns. Defaults are 18 and 24 seconds and can be
  changed in `DeltaOps.json`.
- A cooldown starts only after the skill executes successfully.
- Cooldowns continue through death, respawn, team change, and reconnection.
- Both cooldowns reset at the start of every round.
- Disconnect does not delete the operator state.
- Match end and map change clear operator state.
- Timers are canceled on use completion, round end, match reset, map change,
  disconnect cleanup, and plugin unload.
- Bots can be targeted and healed by operator abilities but do not select operators.
- Switching operators removes the previous Luna-issued flashbang and
  Shepherd-awarded passive armor before applying the new passive.

## Build

From PowerShell:

```powershell
cd C:\path\to\DeltaOps
.\build.ps1 -CssApiDir 'C:\cs2server\game\csgo\addons\counterstrikesharp\api'
```

Install `bin\DeltaOps.dll` under:

```text
C:\cs2server\game\csgo\addons\counterstrikesharp\plugins\DeltaOps\DeltaOps.dll
```

The generated config is:

```text
C:\cs2server\game\csgo\addons\counterstrikesharp\configs\plugins\DeltaOps\DeltaOps.json
```
