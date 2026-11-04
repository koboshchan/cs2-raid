using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private sealed record SpreadBaseline(CCSWeaponBaseVData Data, float[][] Modes, float JumpInitial, float JumpApex, float Reload);
    private readonly Dictionary<nint,SpreadBaseline> spreadOriginals = [];
    private bool spreadMapEnded;
    private void AbandonExpiredSpread(){
        // Called at map-end finally (after best-effort restoration), and BEFORE map-start cleanup.
        // Do not ever dereference a retained old-map VData pointer in the next map.
        var abandoned=TestModeState.ExpireEntries(spreadOriginals);
        if(abandoned!=0)try{TestCleanupError(new InvalidOperationException($"Abandoned {abandoned} expired VData baselines at map boundary; native restoration unproven."));}catch{/* Logging cannot resurrect or retain expired pointers. */}
    }
    private readonly Dictionary<string,Position> deathPositions = [];
    private readonly Dictionary<string,uint> roamingPlaced = [];
    private readonly Dictionary<string,int> deathTeams = [];
    private readonly HashSet<string> resultBanners = [];

    private void RegisterTonightGameplay()
    {
        AddCommandListener("jointeam",ChooseNativeTeam,HookMode.Pre);
        RegisterListener<Listeners.OnEntitySpawned>(entity=>{
            if(!sessionLoaded || !entity.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))return;
            var raw=entity.EntityHandle.Raw;var generation=mapGeneration;
            Server.NextFrame(()=>{
                if(generation!=mapGeneration || !sessionLoaded)return;
                var weapon=new CHandle<CCSWeaponBase>(raw).Value;
                if(weapon is {IsValid:true})PatchSpread(weapon);
            });
        });
    }
    private HookResult ChooseNativeTeam(CCSPlayerController? p,CommandInfo c)
    {
        if(!sessionLoaded || !Human(p))return HookResult.Continue;
        if(FinancialFrozen(Id(p)))return HookResult.Handled;
        if(c.ArgCount==2 && int.TryParse(c.GetArg(1),out var team) && TonightRules.ChooseNativeTeam(game.Phase,team)) {
            // SwitchTeam does not kill/respawn, reset cash, or alter the raid duo.
            p.SwitchTeam((CsTeam)team);
            if(!p.PawnIsAlive && game.Phase==GamePhase.Lobby)p.Respawn();
        } else p.PrintToChat("Choose T or CT in the lobby/character phase. Native side never changes your raid duo.");
        return HookResult.Handled;
    }
    private static void SpawnHealth(CCSPlayerController p)
    {
        if(p.PlayerPawn.Value is not {IsValid:true} pawn || !p.PawnIsAlive)return;
        pawn.MaxHealth=TonightRules.SpawnHealth;pawn.Health=TonightRules.SpawnHealth;
        Utilities.SetStateChanged(pawn,"CBaseEntity","m_iHealth");
        Utilities.SetStateChanged(pawn,"CBaseEntity","m_iMaxHealth");
    }
    private static CFiringModeFloat[] SpreadModes(CCSWeaponBaseVData data) =>
        [data.Spread,data.InaccuracyCrouch,data.InaccuracyStand,data.InaccuracyJump,
         data.InaccuracyLand,data.InaccuracyLadder,data.InaccuracyFire,data.InaccuracyMove];
    private void PatchSpread(CCSWeaponBase weapon)
    {
        if(spreadMapEnded || (!sessionLoaded && !testMode.NativeActive) || PriceItem(weapon.DesignerName) is not {IsGun:true} || weapon.VData is not {} data || data.Handle==nint.Zero)return;
        if(!spreadOriginals.TryGetValue(data.Handle,out var baseline)) {
            baseline=new(data,SpreadModes(data).Select(x=>x.Values.ToArray()).ToArray(),
                data.InaccuracyJumpInitial,data.InaccuracyJumpApex,data.InaccuracyReload);
            spreadOriginals.Add(data.Handle,baseline);
        }
        // VData is shared by many weapons. Always assign from the saved baseline,
        // never multiply already patched values on spawn, pickup or round reset.
        var modes=SpreadModes(data);
        for(var i=0;i<modes.Length;i++)for(var n=0;n<modes[i].Values.Length;n++)
            modes[i].Values[n]=TonightRules.Half(baseline.Modes[i][n]);
        data.InaccuracyJumpInitial=TonightRules.Half(baseline.JumpInitial);
        data.InaccuracyJumpApex=TonightRules.Half(baseline.JumpApex);
        data.InaccuracyReload=TonightRules.Half(baseline.Reload);
    }
    private void PatchExistingSpread()
    {
        foreach(var item in catalog.Where(x=>x.IsGun))
            foreach(var weapon in Utilities.FindAllEntitiesByDesignerName<CCSWeaponBase>(item.Entity))
                if(weapon.IsValid)PatchSpread(weapon);
    }
    private void RestoreSpread()
    {
        foreach(var baseline in spreadOriginals.Values) {
            var modes=SpreadModes(baseline.Data);
            for(var i=0;i<modes.Length;i++)baseline.Modes[i].AsSpan().CopyTo(modes[i].Values);
            baseline.Data.InaccuracyJumpInitial=baseline.JumpInitial;
            baseline.Data.InaccuracyJumpApex=baseline.JumpApex;
            baseline.Data.InaccuracyReload=baseline.Reload;
        }
        spreadOriginals.Clear();
    }
    private void RememberDeath(CCSPlayerController p,Position? pos)
    {
        var id=Id(p);
        if(pos is {Valid:true})deathPositions[id]=pos;
        deathTeams[id]=p.TeamNum;
        roamingPlaced.Remove(id);
        // Armor is never dropped or included in a settlement.
        if(p.PlayerPawn.Value is {} pawn) {
            pawn.ArmorValue=0;
            if(pawn.ItemServices is {} items)items.As<CCSPlayer_ItemServices>().HasHelmet=false;
        }

    }
    // Invoke win ONLY after trusted backend receipt confirmation, never on an intent.
    private void ShowRaidResult(CCSPlayerController p,bool won)
    {
        var id=Id(p);
        if(resultBanners.Contains(id))return;
        var team=deathTeams.GetValueOrDefault(id,p.TeamNum);
        if(team is not (2 or 3))return;
        var result=TonightRules.Banner(team,won);
        var end=new EventRoundEnd(true);
        try {
            end.Winner=result.Winner;end.Reason=result.Reason;end.Message=result.Message;
            end.Nomusic=1;end.PlayerCount=1;end.Time=0;end.Legacy=0;
            end.FireEventToClient(p);
        } finally {end.Free();}
        var panel=new EventCsWinPanelRound(true);
        try {
            panel.FinalEvent=result.PanelFinalEvent;panel.FunfactToken="";panel.ShowTimerAttack=false;
            panel.ShowTimerDefend=false;panel.TimerTime=0;panel.FireEventToClient(p);
        } finally {panel.Free();}
        resultBanners.Add(id);
    }
    private void ClearTonightMatch()
    {
        deathPositions.Clear();roamingPlaced.Clear();deathTeams.Clear();resultBanners.Clear();
    }
}
