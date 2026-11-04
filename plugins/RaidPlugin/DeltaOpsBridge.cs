using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private static readonly PluginCapability<Func<ulong, int, bool>> DeltaOpsAssignment =
        new("deltaops:raid-assign:v1");
    private static readonly PluginCapability<Func<int, ulong, ulong, bool>> DeltaOpsPolicy =
        new("raid:deltaops-policy:v1");
    private static readonly PluginCapability<Func<bool>> DeltaOpsReset =
        new("deltaops:raid-reset:v1");
    private bool deltaOpsBridgeLoaded;

    // Call after game.ResetLobby and sessionLoaded=true, before any new selection.
    private bool ResetDeltaOpsCharacters()
    {
        try { return DeltaOpsReset.Get()?.Invoke() == true; }
        catch { return false; }
    }

    // Call once from LoadGameplay after Enabled is checked, before registering selection.
    private void RegisterDeltaOpsBridge()
    {
        deltaOpsBridgeLoaded = true;
        Capabilities.RegisterPluginCapability(DeltaOpsPolicy, () => DeltaOpsQuery);
        assignCharacter = AssignDeltaOpsCharacter;
    }

    // Call before UnloadGameplay; stale capability delegates fail closed after unload.
    private void StopDeltaOpsBridge()
    {
        deltaOpsBridgeLoaded = false;
        assignCharacter = null;
    }

    private bool DeltaOpsQuery(int operation, ulong first, ulong second)
    {
        if (!deltaOpsBridgeLoaded || !gameplayConfig.Enabled || !sessionLoaded) return false;
        string a = first.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string b = second.ToString(System.Globalization.CultureInfo.InvariantCulture);
        return operation switch
        {
            0 => game.Phase == GamePhase.CharacterSelect && game.Living(a) &&
                game.Players[a].Team != null,
            1 => game.Living(a) && game.Living(b) && game.SameTeam(a, b),
            2 => first != second && game.CanDamage(a, b),
            3 => game.Phase == GamePhase.Active && game.Living(a),
            4 => game.Phase == GamePhase.Lobby,
            _ => false
        };
    }

    private bool AssignDeltaOpsCharacter(CCSPlayerController player, int number)
    {
        if (!Human(player) || number is < 1 or > 6 || !DeltaOpsQuery(0, player.SteamID, 0))
            return false;
        try { return DeltaOpsAssignment.Get()?.Invoke(player.SteamID, number) == true; }
        catch { return false; } // Missing/reloaded/failing provider never confirms selection.
    }
}
