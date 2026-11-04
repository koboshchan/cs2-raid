using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
namespace Cs2Debug;
public sealed partial class DebugPlugin
{
    private sealed record LootOwned(CDynamicProp Entity,uint Generation);
    private readonly List<LootOwned> lootOwned=[];
    private readonly HashSet<string> lootPrecached=[];
    private readonly Dictionary<string,string> lootPrecacheFailures=[];
    private bool lootOn,lootFault;
    private void InitLoot(){
        AddCommand("css_lootprops","Static private visual probe on|off",LootPropsCommand);
        RegisterListener<Listeners.OnServerPrecacheResources>(manifest=>{
            lootPrecached.Clear();lootPrecacheFailures.Clear();
            if(!Config.ExtractHudOnly||Config.ExtractHudPrivateTarget!="192.168.1.162:27016"||Config.ExtractHudRelease!=LootCatalog.Release)return;
            // Only called in native BuildGameSessionManifest while manifest pointer is live.
            foreach(var group in LootCatalog.Entries.GroupBy(x=>x.Asset)){
                var model=group.First().Model;
                if(model==null){lootPrecacheFailures[group.Key]="UNRESOLVED_RESOURCE_PATH";continue;}
                try{manifest.AddResource(model);lootPrecached.Add(model);}catch(Exception e){lootPrecacheFailures[model]=e.GetType().Name;}
            }
        });
    }
    private void ClearLoot(){
        lootOn=false;
        foreach(var w in lootOwned.ToArray())try{
            if(w.Entity.IsValid&&w.Entity.EntityHandle.Raw==w.Generation)w.Entity.Remove();
            lootOwned.Remove(w);
        }catch{lootFault=true;}
    }
    private void LootPropsCommand(CCSPlayerController? p,CommandInfo c){
        if(!HudReady||!Config.ExtractHudOnly||!Human(p)||!LootCatalog.MapAllowed(Server.MapName,Config.ExtractHudRelease)){c.ReplyToCommand("Loot props private27016 exact-map HUD-only gate denied.");return;}
        if(c.ArgCount!=2||c.GetArg(1) is not ("on" or "off")){c.ReplyToCommand("Usage !lootprops on|off");return;}
        if(c.GetArg(1)=="off"){ClearLoot();c.ReplyToCommand($"Loot props OFF; remaining owned cleanup refs={lootOwned.Count}.");return;}
        if(lootOn){c.ReplyToCommand($"Loot props already ON; created={lootOwned.Count}; no duplicates.");return;}
        if(lootFault||lootOwned.Count!=0){c.ReplyToCommand("Loot props cleanup fault; no creation until clean restart.");return;}
        lootOn=true;int created=0,blocked=0,runtime=0;
        foreach(var entry in LootCatalog.Entries){
            if(entry.Model==null||!lootPrecached.Contains(entry.Model)){
                blocked++;continue;
            }
            try{
                var entity=Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override")??throw new InvalidOperationException("Create failed");
                lootOwned.Add(new(entity,entity.EntityHandle.Raw)); // Track before any fallible operation.
                entity.SetModel(entry.Model);entity.Collision.SolidType=SolidType_t.SOLID_NONE;
                entity.Collision.SolidFlags=4; // FSOLID_NOT_SOLID; no physics entity is used.
                entity.Teleport(new Vector((float)entry.X,(float)entry.Y,(float)entry.Z),new QAngle(0,0,0),null);
                entity.DispatchSpawn();entity.Collision.SolidType=SolidType_t.SOLID_NONE;entity.Collision.SolidFlags=4;
                created++;
            }catch(Exception e){runtime++;Logger.LogError("LOOT RUNTIME FAILED model={Model} id={Id} error={Error}",entry.Model,entry.Id,e.GetType().Name);lootFault=true;break;}
        }
        foreach(var g in LootCatalog.Entries.Where(x=>x.Model==null||!lootPrecached.Contains(x.Model)).GroupBy(x=>x.Model??("UNRESOLVED_ASSET:"+x.Asset)))
            c.ReplyToCommand($"LOOT PRECACHE BLOCKED path={g.Key} affected={g.Count()} (no path guessed/no fallback; manifest registration is not load success).");
        if(lootFault)ClearLoot();
        c.ReplyToCommand($"LOOT created={created}/247 precacheBlocked={blocked} runtimeFailures={runtime} retained={lootOwned.Count}; excluded88 never spawned. Positions/visibility/client appearance NOT proven.");
    }
}
