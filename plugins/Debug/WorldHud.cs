using Microsoft.Extensions.Logging;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
namespace Cs2Debug;
public sealed partial class DebugPlugin
{
    private sealed record WorldOwned(CPointWorldText Entity,uint Generation);
    private readonly Dictionary<ulong,WorldOwned> worldOwned=[];
    private readonly List<WorldOwned> worldRetired=[];
    private bool worldFault;
    private bool worldDraining;
    private static bool WorldValid(WorldOwned w)=>w.Entity.IsValid&&w.Entity.EntityHandle.Raw==w.Generation;
    private void RemoveWorld(ulong id){
        if(worldOwned.Remove(id,out var w)){worldRetired.Add(w);DrainWorld();}
    }
    private void DrainWorld(){
        if(worldDraining)return;
        worldDraining=true;
        try{
        foreach(var w in worldRetired.Take(64).ToArray()){
            if(WorldCleanup.Attempt(()=>WorldValid(w),()=>w.Entity.Enabled=false,()=>w.Entity.Remove(),()=>worldFault=true))worldRetired.Remove(w);
            // Failed removal retains the exact generation; optional disable cannot skip removal. // Retain owned generation for bounded next-tick retry; never touch recycled indices.
        }
        }finally{worldDraining=false;}
    }
    private void ClearWorld(){
        worldRetired.AddRange(worldOwned.Values);worldOwned.Clear(); // detach all before native work
        DrainWorld();
    }
    private void FinalWorldCleanup(){
        ClearPreviews();
        WorldCleanup.FinalDrain(DrainWorld,()=>worldRetired.Count,n=>{
            worldFault=true;
            Logger.LogError("WORLD cleanup unresolved {Count} generations {Generations}; no post-unload retry exists. Verify private server entities or restart before reload.",n,string.Join(",",worldRetired.Select(w=>w.Generation)));
        });
    }
    private void WorldTick(ulong id,Preview v,CCSPlayerController p,int zone,long now,bool valid){
        try{
        // Waiver applies only to attested HUD-only LAN target; never claim recipient privacy.
        if(worldOwned.Count+worldRetired.Count>=64&&!worldOwned.ContainsKey(id)){worldFault=true;return;}
        if(worldFault||!HudOnlyReady()||!Config.ExtractHudOnly||!valid||zone==0||now>=v.Clock.End||
           Utilities.GetPlayers().Any(q=>Human(q)&&q.Handle!=p.Handle)) {RemoveWorld(id);return;}
            if(!worldOwned.TryGetValue(id,out var w)){
                var entity=Utilities.CreateEntityByName<CPointWorldText>("point_worldtext")??throw new InvalidOperationException("No world entity");
                w=new(entity,entity.EntityHandle.Raw);worldOwned.Add(id,w); // Own BEFORE any fallible schema/spawn call.
                entity.Enabled=false;entity.DrawBackground=false;entity.Fullbright=true;
                entity.FontName="Arial";entity.FontSize=48;entity.WorldUnitsPerPx=0.05f;
                entity.JustifyHorizontal=PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_CENTER;
                entity.JustifyVertical=PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_CENTER;
                entity.ReorientMode=PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;
                entity.DispatchSpawn();
            }
            if(!WorldValid(w))throw new InvalidOperationException("World generation changed");
            var pawn=p.PlayerPawn.Value!;var o=pawn.AbsOrigin!;var a=pawn.EyeAngles;
            var pos=WorldProbeGeometry.Place(o.X,o.Y,o.Z+pawn.ViewOffset.Z,a.X,a.Y);
            w.Entity.Teleport(new Vector(pos.X,pos.Y,pos.Z),new QAngle(0,a.Y+270,90),null);
            var text=WorldProbeGeometry.Text(v.Clock.Remaining(now),zone==1);
            if(w.Entity.MessageText!=text){w.Entity.MessageText=text;Utilities.SetStateChanged(w.Entity,"CPointWorldText","m_messageText");}
            var color=zone==1?Color.Red:Color.Lime;
            if(w.Entity.Color!=color){w.Entity.Color=color;Utilities.SetStateChanged(w.Entity,"CPointWorldText","m_Color");}
            w.Entity.Enabled=true;Utilities.SetStateChanged(w.Entity,"CPointWorldText","m_bEnabled");
        }catch{worldFault=true;if(previews.Remove(id,out var failed))failed.Clock.Clear(false);RemoveWorld(id);} // Stop creation after any error, don't retry-create every tick.
    }
}
