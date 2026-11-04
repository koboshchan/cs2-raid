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
        if(!previewNative.Available)return;
        if(previews.Remove(p.SteamID,out var v)&&Same(p,v)&&v.Clock.Clear(MenuManager.GetActiveMenu(p)==null) is {} text)
            p.PrintToCenterHtml(text,1);
    }
    private void ClearPreviews(){
        // Empty startup state and teardown require no globals, player enumeration or native reads.
        if(previews.Count==0)return;
        var detached=previews.Values.ToArray();
        previewNative.Clear(()=>PreviewCleanup.Each(detached,v=>{
            var p=v.Player;
            if(Human(p)&&Same(p,v)&&v.Clock.Clear(MenuManager.GetActiveMenu(p)==null) is {} text)
                p.PrintToCenterHtml(text,1);
        }),()=>previews.Clear());
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
        var a=PreviewArgs.Parse(Enumerable.Range(1,c.ArgCount-1).Select(c.GetArg).ToArray());
        if(a==null){c.ReplyToCommand("Usage: !extracthud [1..3600] [css|png] or !extracthud stop");return;}
        StopPreview(p!);if(a.Stop)return;
        if(Server.MapName!="df_spaceport_player_scale_raytrace"||!p!.PawnIsAlive||p.TeamNum<2||p.PlayerPawn.Value is not {IsValid:true}){
            c.ReplyToCommand("Requires correct map and current living T/CT pawn.");return;
        }
        previews[p.SteamID]=new(p,p.Handle,p.PlayerPawn.Raw,new PreviewClock(Environment.TickCount64,a.Seconds),a.Style);
        c.ReplyToCommand("Private live-position HTML capability probe started. CSS/image/layout support unverified; no raid timer or money changes.");
    }
    private void TickPreviews(){
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
            var text=v.Clock.Tick(now,zone,valid,MenuManager.GetActiveMenu(p)==null,v.Style);
            if(text!=null)p.PrintToCenterHtml(text,1);
            if(!valid||now>=v.Clock.End)previews.Remove(id);
        }
    }
}
