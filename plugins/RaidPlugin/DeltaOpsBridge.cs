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
    private bool deltaOpsCharactersReady;

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
        deltaOpsCharactersReady = false;
        assignCharacter = null;
    }

    private bool DeltaOpsQuery(int operation, ulong first, ulong second)
    {
        if (!deltaOpsBridgeLoaded || !gameplayConfig.Enabled || !sessionLoaded) return false;
        string a = first.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string b = second.ToString(System.Globalization.CultureInfo.InvariantCulture);
        if(operation!=4 && (FinancialFrozen(a) || FinancialFrozen(b)))return false;
        return operation switch
        {
            0 => deltaOpsCharactersReady && game.Phase == GamePhase.CharacterSelect && game.Living(a) &&
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

// Fixture adapter is intentionally independent from production gameplay/session and money.
public sealed partial class RaidPlugin
{
    private static readonly PluginCapability<Func<long>> FixtureGenerationV2 =
        new("raid:deltaops-fixture-generation:v2");
    private static readonly PluginCapability<Func<int,uint,uint,long,bool>> FixturePolicyV2 =
        new("raid:deltaops-fixture-policy:v2");
    private static readonly PluginCapability<Func<uint,int,long,bool>> FixtureAssignV2 =
        new("deltaops:fixture-assign:v2");
    private static readonly PluginCapability<Func<long,bool>> FixtureResetV2 =
        new("deltaops:fixture-reset:v2");
    private bool fixtureBridgeV2;
    private long FixtureGenerationNow() =>
        fixtureBridgeV2 && TestServer && !testUnloaded && testMode.Enabled && TestScope()
        && Environment.GetEnvironmentVariable("RAID_DELTAOPS_MODE")=="fixture-v2"
        ? testMode.Generation : 0;
    private void RegisterFixtureDeltaOpsBridge()
    {
        if(!TestServer)return;
        fixtureBridgeV2=true;
        Capabilities.RegisterPluginCapability(FixtureGenerationV2,()=>FixtureGenerationNow);
        Capabilities.RegisterPluginCapability(FixturePolicyV2,()=>FixtureDeltaOpsQuery);
    }
    private bool ResetFixtureDeltaOps()
    {
        var generation=FixtureGenerationNow();
        if(generation<=0)return false;
        try{return FixtureResetV2.Get()?.Invoke(generation)==true;}catch{return false;}
    }
    private void StopFixtureDeltaOpsBridge(){fixtureBridgeV2=false;}
    private bool AssignFixtureDeltaOps(CCSPlayerController player,int number)
    {
        var generation=FixtureGenerationNow();
        if(number is <1 or >6 || !FixtureDeltaOpsQuery(0,player.EntityHandle.Raw,0,generation))return false;
        try{return FixtureAssignV2.Get()?.Invoke(player.EntityHandle.Raw,number,generation)==true
            && FixtureGenerationNow()==generation && TestParticipant(player);}
        catch{return false;}
    }
    private bool FixtureDeltaOpsQuery(int op,uint first,uint second,long generation)
    {
        if(generation<=0 || FixtureGenerationNow()!=generation)return false;
        if(op==4)return true; // Cleanup in any phase, while this generation is still published.
        var a=CounterStrikeSharp.API.Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&p.EntityHandle.Raw==first);
        var b=CounterStrikeSharp.API.Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&p.EntityHandle.Raw==second);
        if(a==null || !a.PawnIsAlive || !(TestParticipant(a)||TestBot(a)))return false;
        var aHuman=TestParticipant(a);
        var aId=aHuman?Id(a):BotId(a);
        var aLiving=aHuman?testFlow.Teams.Living(aId):testFlow.BotLiving(aId);
        if(!aLiving)return false;
        if(op==0)return aHuman && testMode.Phase==TestModeState.Stage.Selection;
        if(op==3)return aHuman && testMode.Phase==TestModeState.Stage.Active;
        if(b==null || !b.PawnIsAlive || !(TestParticipant(b)||TestBot(b)))return false;
        var bHuman=TestParticipant(b);
        var bId=bHuman?Id(b):BotId(b);
        var bLiving=bHuman?testFlow.Teams.Living(bId):testFlow.BotLiving(bId);
        if(!bLiving)return false;
        if(op==1)return aHuman && bHuman && testFlow.Teams.SameTeam(aId,bId);
        return op==2 && first!=second && testMode.Phase==TestModeState.Stage.Active
            && testFlow.EnemyDamage(aId,bId);
    }
}
