# Delta Ops CS 1.3.2

CounterStrikeSharp server plugin combining standard CS2 gameplay with selectable
operator passives plus independent V and X abilities.

This is a mechanics-inspired fan adaptation. It does not include or redistribute
Delta Force models, textures, audio, weapon assets, or client mods.

## Commands

```text
!characterselect  Print the numbered six-character list in chat
!characterselect <1-6> Select an operator (legacy !op <1-6> still works)
V                 Use the V ability when its cooldown is ready
X                 Use the X ability when its cooldown is ready
!v / !x           Chat-command fallbacks
!bind             Print the reliable client bind commands
!cd               Show remaining V/X cooldown
!ops              Show operator and cooldown state
!roles            Show all operator names
```

Server-console test command:

```text
css_ops_use <slot> v
css_ops_use <slot> x
```

The slot number is the player ID shown by the `status` command.

Selection is chat-only, with all six numbered operators shown at once. There is
no center-screen display box and no per-second list spam. The 120-second timeout,
random fallback, cooldowns, and post-warmup selection lock are unchanged.

The plugin does not change client key bindings. Use !v / !x immediately, or run
these once yourself in the game developer console:

```text
bind "v" "css_v"
bind "x" "css_x"
```

The legacy AutoBindSkillKeys config key now controls one-time help per player state,
not remote binding. !bind always prints the instructions, even if this key is false.

After the live match begins, a selected operator is locked until the match ends
or a server operator runs `css_ops_reset`. Player-facing live switching is
disabled; `css_ops_assign` remains server-console only for testing.

When V or X finishes cooling down, the owning player receives an alert below
the crosshair with the slot and skill name.

The server sets the HE grenade carry limit to 3. Items granted by abilities are
added to the inventory and immediately selected in the player's hand.

## Roster

1. 红狼 / D-Wolf
   - V: 4 seconds of +35% movement speed
   - X: leap in the current WASD direction and recover 15 HP; default forward
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
   - V: jet propulsion in the current WASD direction; default forward
   - X: grants a standard HE grenade; enemies caught in the explosion are
     disarmed and rooted for 4 seconds
   - Passive: +6% base movement speed
5. 骇爪 / Hackclaw
   - V: scans and marks enemies in range; the target receives an alert
   - X: grants one native flashbang
   - Native pin: hold primary attack to start a visible 3-second fuse; cancel
     the pull to clear it
   - Passive: +4% base movement speed
6. 牧羊人 / Shepherd
   - V: places a proximity mine that triggers when an enemy enters its radius
   - X: grants one native enhanced HE grenade
   - Native pin: hold primary attack to start a visible 3-second fuse; cancel
     the pull to clear it
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
- The native CS2 pin-pull state starts the timer. Releasing the pin throws the
  grenade with its remaining fuse; right-clicking, switching weapons, or
  otherwise cancelling the pull clears the timer.
- The HUD refreshes every 0.1 seconds while the pin remains pulled. V skills
  remain usable during this timer.
- If the timer expires while the grenade is still held, one grenade is consumed,
  spawned at the player's feet with zero velocity, and detonated immediately.

## Build

From PowerShell:

```powershell
cd C:\path\to\DeltaOps
$env:DOTNET_CLI_HOME = Join-Path $env:TEMP 'deltaops-dotnet-home'
$env:NUGET_PACKAGES = Join-Path $env:TEMP 'deltaops-nuget-packages'
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

Repository build note

The original build.ps1 mentioned above is intentionally omitted by owner request. Use only the root Dockerfile targets documented in the repository README. No host scripts are retained.
