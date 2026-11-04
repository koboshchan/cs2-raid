using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core.Capabilities;

namespace DeltaOps;

// Only framework delegates and primitives cross plugin load contexts. Server-thread only.
public sealed partial class DeltaOpsPlugin
{
    private static readonly PluginCapability<Func<ulong, int, bool>> AssignCapability =
        new("deltaops:raid-assign:v1");
    private static readonly PluginCapability<Func<int, ulong, ulong, bool>> PolicyCapability =
        new("raid:deltaops-policy:v1");
    private static readonly PluginCapability<Func<bool>> ResetCapability =
        new("deltaops:raid-reset:v1");
    private bool _raidMode;
    private bool _bridgeLoaded;
    private bool _bridgeAssigning;

    private void ConfigureRaidBridge()
    {
        var path = Path.Combine(ModuleDirectory, "..", "RaidPlugin", "gameplay.json");
        try
        {
            if (File.Exists(path))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(path));
                _raidMode = doc.RootElement.EnumerateObject().Any(p =>
                    p.Name.Equals("enabled", StringComparison.OrdinalIgnoreCase) &&
                    p.Value.ValueKind == JsonValueKind.True);
            }
        }
        catch { _raidMode = true; } // Unreadable raid config must never enable vanilla targeting.
        _bridgeLoaded = true;
        Capabilities.RegisterPluginCapability(AssignCapability, () => AssignRaidCharacter);
        Capabilities.RegisterPluginCapability(ResetCapability, () => ResetRaidCharacters);
    }

    private bool ResetRaidCharacters()
    {
        if (!_bridgeLoaded || !_raidMode || !QueryRaid(4, 0)) return false;
        ResetAllPlayersAndState("Raid lobby reset", broadcast: false);
        return true;
    }

    private bool QueryRaid(int operation, ulong first, ulong second = 0)
    {
        if (!_bridgeLoaded) return false;
        try { return PolicyCapability.Get()?.Invoke(operation, first, second) == true; }
        catch { return false; }
    }

    private bool RaidActive(ulong player) => QueryRaid(3, player);
    private bool AssignRaidCharacter(ulong steamId, int number)
    {
        if (!_bridgeLoaded || !_raidMode || number is < 1 or > 6 ||
            !QueryRaid(0, steamId)) return false;
        var player = Utilities.GetPlayers().FirstOrDefault(p =>
            IsHumanController(p) && p.SteamID == steamId && p.PawnIsAlive);
        if (player == null) return false;
        var state = GetExistingState(player);
        // Repeated assignment cannot reset cooldowns, heal or mint another utility kit.
        if (state?.Operator != null) return (int)state.Operator.Value == number - 1;
        _bridgeAssigning = true;
        try
        {
            SelectOperator(player, GetPlayerKey(player), _operators[number - 1], automatic: false);
            return GetExistingState(player)?.Operator == _operators[number - 1].Id;
        }
        finally { _bridgeAssigning = false; }
    }

    private bool IsAlly(CounterStrikeSharp.API.Core.CCSPlayerController first,
        CounterStrikeSharp.API.Core.CCSPlayerController second) =>
        _raidMode ? QueryRaid(1, first.SteamID, second.SteamID) :
        first.TeamNum == second.TeamNum;

    private bool IsEnemy(CounterStrikeSharp.API.Core.CCSPlayerController first,
        CounterStrikeSharp.API.Core.CCSPlayerController second) =>
        first.Handle != second.Handle && (_raidMode ?
            QueryRaid(2, first.SteamID, second.SteamID) : first.TeamNum != second.TeamNum);
}
