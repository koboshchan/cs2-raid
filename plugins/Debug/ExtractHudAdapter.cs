using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
namespace Cs2Debug;
public sealed partial class DebugPlugin
{
    private sealed record Preview(CCSPlayerController Player,IntPtr Controller,uint Pawn,PreviewClock Clock);
    private readonly Dictionary<ulong,Preview> previews=[];
    private readonly PreviewNativeLifecycle previewNative=new();
    private static bool Same(CCSPlayerController p,Preview v)=>p.Handle==v.Controller&&p.PlayerPawn.Raw==v.Pawn;
    private void StopPreview(CCSPlayerController p){
        if(previews.Remove(p.SteamID,out var v))v.Clock.Clear(false);
    }
    private void ClearPreviews(){
        var detached=previews.Values.ToArray();
        previews.Clear(); // Managed cancellation only; the alert has no owned native entity.
        foreach(var v in detached)v.Clock.Clear(false);
    }
    private bool RaidInactiveForHud() => RaidHudGuard.ReadInactive(
        ()=>File.ReadAllText(Path.Combine(ModuleDirectory,"..","RaidPlugin","gameplay.json")),
        Config.ExtractHudOnly&&Config.ExtractHudVerifiedNoRaid);
    private bool HudOnlyReady() {
        var port=CounterStrikeSharp.API.Modules.Cvars.ConVar<int>.Find("hostport");
        var lan=CounterStrikeSharp.API.Modules.Cvars.ConVar<bool>.Find("sv_lan");
        return PreviewActivation.HudOnly(Config.ExtractHudOnly,Config.ExtractHudPrivateTarget,
            port?.Value??0,lan?.Value??false);
    }
    private bool HudReady => RaidInactiveForHud()&&(Config.ExtractHudOnly?HudOnlyReady():active);
    private void ExtractHudCommand(CCSPlayerController? p,CommandInfo c){
        try{ExtractHudCommandCore(p,c);}catch{previewNative.Stop();ClearPreviews();}
    }
    private void ExtractHudCommandCore(CCSPlayerController? p,CommandInfo c){
        if(!previewNative.Available||!HudReady||!Human(p)){ClearPreviews();c.ReplyToCommand("HUD mode unavailable or private scope not attested.");return;}
        if(!Config.ExtractHudExclusive||Config.ExtractHudRelease!="default-20261105"){
            c.ReplyToCommand("Requires explicit exclusive HUD channel approval and default-20261105 asset attestation.");return;
        }
        var a=PreviewArgs.Parse(Enumerable.Range(1,c.ArgCount-1).Select(c.GetArg).ToArray());
        if(a==null){c.ReplyToCommand("Usage: !extracthud [1..3600] or !extracthud stop");return;}
        StopPreview(p!);if(a.Stop)return;
        if(Server.MapName!="df_spaceport_player_scale_raytrace"||!p!.PawnIsAlive||p.TeamNum<2||p.PlayerPawn.Value is not {IsValid:true}){
            c.ReplyToCommand("Requires correct map and current living T/CT pawn.");return;
        }
        previews[p.SteamID]=new(p,p.Handle,p.PlayerPawn.Raw,new PreviewClock(Environment.TickCount64,a.Seconds));
        c.ReplyToCommand("Private per-player alert started. Lifetime/dismissal unverified; no raid or money changes.");
    }
    private void TickPreviews(){
        try{TickPreviewsCore();}catch{previewNative.Stop();ClearPreviews();}
    }
    private void TickPreviewsCore(){
        if(lootOn&&!HudReady)ClearLoot();
        if(lootFault&&lootOwned.Count>0)ClearLoot();
        previewNative.OnTick();
        if(!HudReady){ClearPreviews();return;}
        var now=Environment.TickCount64;
        foreach(var (id,v) in previews.ToArray()){
            var p=Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&p.SteamID==id);
            if(p==null){previews.Remove(id);continue;}
            if(!Same(p,v)){previews.Remove(id);continue;}
            var valid=HudReady&&Config.ExtractHudExclusive&&Config.ExtractHudRelease=="default-20261105"&&Server.MapName=="df_spaceport_player_scale_raytrace"&&p.PawnIsAlive&&p.TeamNum>=2&&
                p.PlayerPawn.Value is {IsValid:true,AbsOrigin:not null};
            var origin=valid?p.PlayerPawn.Value!.AbsOrigin:null;
            var zone=origin==null?0:PreviewClock.Zone(origin.X,origin.Y,origin.Z);
            var text=v.Clock.Tick(now,zone,valid,MenuManager.GetActiveMenu(p)==null);
            if(!string.IsNullOrEmpty(text)){
                try{p.PrintToCenterAlert(text);}
                catch{previews.Remove(id);v.Clock.Clear(false);previewNative.Stop();throw;}
            }
            if(!valid||now>=v.Clock.End)previews.Remove(id);
        }
    }
}
