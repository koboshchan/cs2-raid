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
    private bool _fixtureMode;
    private bool _fixtureExplicit;
    private static readonly PluginCapability<Func<string>> FixtureStatus =
        new("deltaops:fixture-status:v2");
    private readonly FixtureAbilityPolicyV2 _fixtureKits=new();
    // Acceptance is source-controlled, not operator-configurable: no native ownership/revocation
    // proof has been obtained. Keep false until independently reviewed installed-engine evidence.
    private static bool FixtureNativeKitAccepted(int character)=>false;
    private bool DeliverFixtureKit(CounterStrikeSharp.API.Core.CCSPlayerController player,int number,long generation)
    {
        // Future implementation must track every utility raw handle before returning success,
        // fence deferred callbacks by generation/controller/pawn and prove cleanup before replacement.
        // No SelectOperator call here: legacy passives can grant untracked Luna flash.
        return false;
    }
    private static readonly PluginCapability<Func<long>> FixtureGeneration =
        new("raid:deltaops-fixture-generation:v2");
    private static readonly PluginCapability<Func<int,uint,uint,long,bool>> FixturePolicy =
        new("raid:deltaops-fixture-policy:v2");
    private static readonly PluginCapability<Func<uint,int,long,bool>> FixtureAssign =
        new("deltaops:fixture-assign:v2");
    private static readonly PluginCapability<Func<long,bool>> FixtureReset =
        new("deltaops:fixture-reset:v2");
    private long CurrentFixtureGeneration() {
        if(!_bridgeLoaded || !_fixtureMode || !_fixtureExplicit)return 0;
        try { return Math.Max(0,FixtureGeneration.Get()?.Invoke()??0); } catch { return 0; }
    }
    private bool QueryFixture(int op,uint a,uint b,long generation) {
        if(generation<=0 || CurrentFixtureGeneration()!=generation)return false;
        try{return FixturePolicy.Get()?.Invoke(op,a,b,generation)==true;}catch{return false;}
    }
    private bool AssignFixtureCharacter(uint raw,int number,long generation) {
        if(!_fixtureExplicit || number is <2 or >6 || !QueryFixture(0,raw,0,generation))return false;
        var player=Utilities.GetPlayers().FirstOrDefault(p=>IsHumanController(p)&&p.EntityHandle.Raw==raw&&p.PawnIsAlive);
        if(player==null)return false;
        var pawn=player.PlayerPawn.Value;
        if(pawn==null || !pawn.IsValid)return false;
        if(_fixtureKits.Generation!=generation && !_fixtureKits.Begin(generation))return false;
        return _fixtureKits.CommitKit(generation,raw,pawn.EntityHandle.Raw,number,true,true,
            FixtureNativeKitAccepted(number),()=>QueryFixture(0,raw,0,generation)
                && DeliverFixtureKit(player,number,generation));
    }
    private bool ResetFixtureCharacters(long generation) {
        if(!_fixtureMode || !_fixtureExplicit || generation<=0 || CurrentFixtureGeneration()!=generation)return false;
        // This adapter cannot create native effects or grants. Never reset legacy cooldowns
        // or heal the entire server as a side effect of fixture lifecycle notifications.
        return QueryFixture(4,0,0,generation);
    }
    private bool _bridgeLoaded;
    private bool _bridgeAssigning;

    private void ConfigureRaidBridge()
    {
        _fixtureExplicit = Environment.GetEnvironmentVariable("RAID_DELTAOPS_MODE")=="fixture-v2";
        _fixtureMode = Environment.GetEnvironmentVariable("RAID_TEST_SERVER")=="1" || _fixtureExplicit;
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
        if(_fixtureMode || _fixtureExplicit)_raidMode=true; // Dedicated mode never falls back to vanilla.
        _bridgeLoaded = true;
        Capabilities.RegisterPluginCapability(FixtureStatus,()=>()=>
            "UNAVAILABLE: all native ability acceptance BLOCKED; utility ownership/revocation and native effects unverified; unsafe damage ABI disabled");
        Capabilities.RegisterPluginCapability(FixtureAssign,()=>AssignFixtureCharacter);
        Capabilities.RegisterPluginCapability(FixtureReset,()=>ResetFixtureCharacters);
        Capabilities.RegisterPluginCapability(AssignCapability, () => AssignRaidCharacter);
        Capabilities.RegisterPluginCapability(ResetCapability, () => ResetRaidCharacters);
    }

    private bool ResetRaidCharacters()
    {
        if (_fixtureMode || !_bridgeLoaded || !_raidMode || !QueryRaid(4, 0)) return false;
        ResetAllPlayersAndState("Raid lobby reset", broadcast: false);
        return true;
    }

    private bool QueryRaid(int operation, ulong first, ulong second = 0)
    {
        if (_fixtureMode || !_bridgeLoaded) return false;
        try { return PolicyCapability.Get()?.Invoke(operation, first, second) == true; }
        catch { return false; }
    }

    private bool RaidActive(ulong player) => QueryRaid(3, player);
    private bool AssignRaidCharacter(ulong steamId, int number)
    {
        if (!_bridgeLoaded || !_raidMode || !StableOperatorIdentity.SelectableWire(number) ||
            !QueryRaid(0, steamId)) return false;
        var player = Utilities.GetPlayers().FirstOrDefault(p =>
            IsHumanController(p) && p.SteamID == steamId && p.PawnIsAlive);
        if (player == null) return false;
        var state = GetExistingState(player);
        if (_states.GetValueOrDefault(GetPlayerKey(player))?.SelectionRetired == true) return false;
        // Repeated assignment cannot reset cooldowns, heal or mint another utility kit.
        if (state?.Operator != null) return StableOperatorIdentity.Wire((int)state.Operator.Value) == number;
        _bridgeAssigning = true;
        try
        {
            SelectOperator(player, GetPlayerKey(player), _operators.Single(op => StableOperatorIdentity.Wire((int)op.Id) == number), automatic: false);
            return _states.GetValueOrDefault(GetPlayerKey(player))?.SelectionRetired == false && GetExistingState(player)?.Operator == _operators.Single(op => StableOperatorIdentity.Wire((int)op.Id) == number).Id;
        }
        finally { _bridgeAssigning = false; }
    }

    private bool IsAlly(CounterStrikeSharp.API.Core.CCSPlayerController first,
        CounterStrikeSharp.API.Core.CCSPlayerController second) =>
        _fixtureMode ? QueryFixture(1,first.EntityHandle.Raw,second.EntityHandle.Raw,CurrentFixtureGeneration()) :
        _raidMode ? QueryRaid(1, first.SteamID, second.SteamID) :
        first.TeamNum == second.TeamNum;

    private bool IsEnemy(CounterStrikeSharp.API.Core.CCSPlayerController first,
        CounterStrikeSharp.API.Core.CCSPlayerController second) =>
        first.EntityHandle.Raw != second.EntityHandle.Raw && (_fixtureMode ?
            QueryFixture(2,first.EntityHandle.Raw,second.EntityHandle.Raw,CurrentFixtureGeneration()) : _raidMode ?
            QueryRaid(2, first.SteamID, second.SteamID) : first.TeamNum != second.TeamNum);
}
