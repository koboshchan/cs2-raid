using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
namespace Cs2Debug;
public sealed partial class DebugPlugin
{
    private sealed record Preview(CCSPlayerController Player,IntPtr Controller,uint Pawn,PreviewClock Clock,string Style);
    private readonly Dictionary<ulong,Preview> previews=[];
    private readonly PreviewNativeLifecycle previewNative=new();
    private static bool Same(CCSPlayerController p,Preview v)=>p.Handle==v.Controller&&p.PlayerPawn.Raw==v.Pawn;
    private void StopPreview(CCSPlayerController p){
        // Blank HTML reopens/retains the native panel on the observed client.
        // Until full-dismiss semantics are verified, cancel only and allow the last Duration2 to expire.
        RemoveWorld(p.SteamID);
        if(previews.Remove(p.SteamID,out var v))v.Clock.Clear(false);
    }
    private void ClearPreviews(){
        ClearWorld();
        var detached=previews.Values.ToArray();
        previews.Clear(); // Managed cancellation before any callbacks; never enumerate globals on teardown.
        foreach(var v in detached)v.Clock.Clear(false);
    }
    private void ClearProbe(CCSPlayerController p,string mode,CommandInfo c){
        if(!previews.TryGetValue(p.SteamID,out var v)||!Same(p,v)||!v.Clock.Owned||(v.Style=="world"||!PreviewDispatch.IsHtml(v.Style))||
            MenuManager.GetActiveMenu(p)!=null||!p.PawnIsAlive||Server.MapName!="df_spaceport_player_scale_raytrace"){
            c.ReplyToCommand("Clear probe requires your active owned HTML preview on this living pawn.");return;
        }
        previews.Remove(p.SteamID); // Detach before native call, no automatic retries or follow-up blank sends.
        v.Clock.Clear(false);
        var duration=PreviewDismiss.ProbeDuration(mode);
        if(duration is not {} seconds)return;
        var ev=new EventShowSurvivalRespawnStatus(true){LocToken="",Duration=seconds,Userid=p};
        try{ev.FireEventToClient(p);}finally{ev.Free();}
        c.ReplyToCommand("Private dismissal PROBE sent once. Full panel hiding is UNVERIFIED; report remaining bar and time to disappear.");
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
        if(!previewNative.Available||!HudReady||!Human(p)){ClearPreviews();c.ReplyToCommand("HUD mode unavailable or private scope not attested.");return;}
        if(!Config.ExtractHudExclusive||Config.ExtractHudRelease!="default-20261105"){
            c.ReplyToCommand("Requires explicit exclusive HUD channel approval and default-20261105 asset attestation.");return;
        }
        if(c.ArgCount==2&&PreviewDismiss.ProbeDuration(c.GetArg(1))!=null){ClearProbe(p!,c.GetArg(1),c);return;}
        if(Enumerable.Range(1,c.ArgCount-1).Select(c.GetArg).Any(a=>a is "instructor" or "game_text" or "env_hudhint" or "roundclock")){
            c.ReplyToCommand("UNAVAILABLE: private full lifecycle/cleanup not verified for this mode; no entity or event spawned.");return;
        }
        var a=PreviewArgs.Parse(Enumerable.Range(1,c.ArgCount-1).Select(c.GetArg).ToArray());
        if(a==null){c.ReplyToCommand("Usage: !extracthud [1..3600] [css|png|plain|tags|classes|alert] or !extracthud stop");return;}
        if(a.Style=="world"&&(!Config.ExtractHudOnly||!HudOnlyReady()||worldFault)){
            c.ReplyToCommand("World probe unavailable outside private HUD-only LAN scope or after entity failure.");return;
        }
        StopPreview(p!);if(a.Stop)return;
        if(Server.MapName!="df_spaceport_player_scale_raytrace"||!p!.PawnIsAlive||p.TeamNum<2||p.PlayerPawn.Value is not {IsValid:true}){
            c.ReplyToCommand("Requires correct map and current living T/CT pawn.");return;
        }
        previews[p.SteamID]=new(p,p.Handle,p.PlayerPawn.Raw,new PreviewClock(Environment.TickCount64,a.Seconds),a.Style);
        c.ReplyToCommand(a.Style=="world"?"WORLD PROBE: world-visible, NOT private. Stops rendering if another human is present. Upper-view placement/appearance unverified. !extracthud stop removes owned entity.":a.Style=="alert"?"Private per-player alert probe started. Alert lifetime/dismissal and appearance unverified; no raid or money changes.":"Private live-position HTML capability probe started. CSS/image/layout support unverified; no raid timer or money changes.");
    }
    private void TickPreviews(){
        DrainWorld();
        previewNative.OnTick();
        if(!HudReady){ClearPreviews();return;}
        var now=Environment.TickCount64;
        foreach(var (id,v) in previews.ToArray()){
            var p=Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&p.SteamID==id);
            if(p==null){RemoveWorld(id);previews.Remove(id);continue;}
            if(!Same(p,v)){RemoveWorld(id);previews.Remove(id);continue;}
            var valid=HudReady&&Config.ExtractHudExclusive&&Config.ExtractHudRelease=="default-20261105"&&Server.MapName=="df_spaceport_player_scale_raytrace"&&p.PawnIsAlive&&p.TeamNum>=2&&
                p.PlayerPawn.Value is {IsValid:true,AbsOrigin:not null};
            var origin=valid?p.PlayerPawn.Value!.AbsOrigin:null;
            var zone=origin==null?0:PreviewClock.Zone(origin.X,origin.Y,origin.Z);
            if(v.Style=="world"){
                WorldTick(id,v,p,zone,now,valid&&MenuManager.GetActiveMenu(p)==null);
                if(!valid||now>=v.Clock.End)previews.Remove(id);
                continue;
            }
            var text=v.Clock.Tick(now,zone,valid,MenuManager.GetActiveMenu(p)==null,v.Style);
            if(PreviewDismiss.ShouldRender(text)){
                try{PreviewDispatch.Send(v.Style,text,msg=>p.PrintToCenterHtml(msg,2),msg=>p.PrintToCenterAlert(msg));}
                catch{previews.Remove(id);v.Clock.Clear(false);previewNative.Stop();throw;}
            }
            if(!valid||now>=v.Clock.End)previews.Remove(id);
        }
    }
}
