using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
public sealed class Plugin:BasePlugin{
public override string ModuleName=>"PrivateV24Spawn32";
public override string ModuleVersion=>"1.2.0-lift100";
public override string ModuleAuthor=>"koboshchan";
readonly List<SpawnPoint> owned=new();
record Site(string Id,float X,float Y,float Z,float Yaw);
readonly Site[] sites={
new("2030206001",-9706.32421875f,-1247.044921875f,-82.193203125f,179.617584f),
new("2030206002",-9910.79296875f,5274f,58.20328125f,186.105957f),
new("2030206004",-1292.6953125f,7667.525390625f,155.4103125f,116.019043f),
new("2030206005",9957.744140625f,2545.20703125f,-200.986171875f,90f),
new("2030206006",3523.67578125f,3953.84765625f,11.4103125f,266.278473f),
new("2030206007",6850.79296875f,7858.072265625f,-202.79671875f,54.803429f),
new("2030206008",6598.44140625f,-3688.20703125f,-195.5896875f,320.667191f)};
bool Allowed(){var a=File.ReadAllText("/proc/1/cmdline").Split('\0');var i=Array.IndexOf(a,"-port");return a.Contains("-dedicated")&&i>=0&&i+1<a.Length&&a[i+1]=="27016"&&ConVar.Find("hostport")?.GetPrimitiveValue<int>()==27016;}
static SpawnPoint[] Spawns(string c)=>Utilities.FindAllEntitiesByDesignerName<SpawnPoint>(c).Where(e=>e.IsValid).ToArray();
static object Snapshot()=>new{utc=DateTime.UtcNow,map=Server.MapName,T=Spawns("info_player_terrorist").Select(Row).ToArray(),CT=Spawns("info_player_counterterrorist").Select(Row).ToArray(),players=Utilities.GetPlayers().Where(p=>p.IsValid).Select(p=>new{p.PlayerName,p.TeamNum,p.IsBot}).ToArray()};
static object Row(SpawnPoint e)=>new{e.Index,e.Enabled,e.Priority,position=e.AbsOrigin==null?null:new[]{e.AbsOrigin.X,e.AbsOrigin.Y,e.AbsOrigin.Z}};
static void Save(object o)=>File.AppendAllText("/debugtest/reports/v24-spawn-fix.jsonl",JsonSerializer.Serialize(o)+"\n");
record Exact(string Class,float X,float Y,float Z);
readonly Exact[] exact={
new("info_player_terrorist",-9642.324f,-1183.0449f,-86.35036f),
new("info_player_terrorist",6850.793f,7794.0723f,-205.10965f),
new("info_player_terrorist",9957.744f,2481.207f,-196.51047f),
new("info_player_terrorist",-9706.324f,-1311.0449f,-86.35988f),
new("info_player_terrorist",6850.793f,7922.0723f,-205.10965f),
new("info_player_terrorist",-9910.793f,5338f,59.08008f),
new("info_player_terrorist",6534.4414f,-3688.207f,-213.2285f),
new("info_player_terrorist",9893.744f,2545.207f,-196.52252f),
new("info_player_terrorist",-9770.324f,-1247.0449f,-86.35985f),
new("info_player_terrorist",6914.793f,7858.0723f,-203.23068f),
new("info_player_terrorist",10021.744f,2545.207f,-196.49806f),
new("info_player_terrorist",-9846.793f,5274f,59.08008f),
new("info_player_terrorist",6598.4414f,-3688.207f,-213.22826f),
new("info_player_terrorist",3523.6758f,3953.8477f,13.000092f),
new("info_player_terrorist",-1292.6953f,7667.5254f,163.52805f),
new("info_player_terrorist",-9706.324f,-1247.0449f,-86.35985f),
new("info_player_counterterrorist",10021.744f,2609.207f,-196.4986f),
new("info_player_counterterrorist",6598.4414f,-3752.207f,-213.22833f),
new("info_player_counterterrorist",3523.6758f,3889.8477f,13.000092f),
new("info_player_counterterrorist",-9910.793f,5210f,59.08008f),
new("info_player_counterterrorist",6598.4414f,-3624.207f,-213.22821f),
new("info_player_counterterrorist",9957.744f,2609.207f,-196.51047f),
new("info_player_counterterrorist",-9706.324f,-1183.0449f,-86.339516f),
new("info_player_counterterrorist",6786.793f,7858.0723f,-205.02771f),
new("info_player_counterterrorist",-9974.793f,5274f,59.08008f),
new("info_player_counterterrorist",6662.4414f,-3688.207f,-213.22803f),
new("info_player_counterterrorist",3587.6758f,3953.8477f,13.000092f),
new("info_player_counterterrorist",-1228.6953f,7667.5254f,170.00821f),
new("info_player_counterterrorist",-9642.324f,-1247.0449f,-86.35985f),
new("info_player_counterterrorist",6850.793f,7858.0723f,-205.10965f),
new("info_player_counterterrorist",9957.744f,2545.207f,-196.51047f),
new("info_player_counterterrorist",-9910.793f,5274f,60.964783f),
};
void RestoreExact(){
if(!Allowed())return;
var t=Spawns("info_player_terrorist");var ct=Spawns("info_player_counterterrorist");
if(t.Length==16&&ct.Length==16&&t.Concat(ct).All(e=>e.Enabled)){Save(new{persistentSkip=Snapshot()});return;}
if(t.Length!=0||ct.Length!=0){Save(new{persistentRefused="nonzero native/foreign baseline",state=Snapshot()});return;}
try {
foreach(var x in exact){
 var e=Utilities.CreateEntityByName<SpawnPoint>(x.Class);
 if(e==null||!e.IsValid)throw new Exception("exact spawn allocation failed");
 owned.Add(e);e.Teleport(new Vector(x.X,x.Y,x.Z+100f),new QAngle(0,0,0),new Vector(0,0,0));e.DispatchSpawn();e.Enabled=true;e.Priority=0;
}
Save(new{persistentExactRestored=Snapshot(),source="spawn32-validated-source.json immutable groundvalidated receipt",owned=owned.Select(e=>e.Index).ToArray()});
}catch(Exception ex){foreach(var e in owned.Where(e=>e.IsValid))e.Remove();owned.Clear();Save(new{persistentError=ex.Message,utc=DateTime.UtcNow});}
}
void LiftExisting(){
if(!Allowed())return;
var all=Spawns("info_player_terrorist").Concat(Spawns("info_player_counterterrorist")).ToArray();
if(all.Length!=32||all.Any(e=>!e.Enabled||e.AbsOrigin==null))throw new Exception("Lift requires 32 enabled spawns");
foreach(var e in all){var v=e.AbsOrigin!;var x=exact.SingleOrDefault(x=>x.Class==e.DesignerName&&Math.Abs(x.X-v.X)<0.01&&Math.Abs(x.Y-v.Y)<0.01);if(x==null||Math.Abs(v.Z-x.Z)>0.01&&Math.Abs(v.Z-x.Z-80)>0.01&&Math.Abs(v.Z-x.Z-130)>0.01&&Math.Abs(v.Z-x.Z-100)>0.01)throw new Exception("Unexpected spawn baseline");}
foreach(var e in all){var v=e.AbsOrigin!;var x=exact.Single(x=>x.Class==e.DesignerName&&Math.Abs(x.X-v.X)<0.01&&Math.Abs(x.Y-v.Y)<0.01);e.Teleport(new Vector(x.X,x.Y,x.Z+100),e.AbsRotation,new Vector(0,0,0));}
Save(new{lift100=Snapshot()});
}
public override void Load(bool hotReload){
AddCommand("css_v24_spawn32_lift100","Idempotent approved +100 Z lift",(p,c)=>{if(p!=null||!Allowed())return;LiftExisting();c.ReplyToCommand("Lift100 applied");});
AddCommand("css_v24_spawn32_restore_exact","Restore exact previously validated 32 spawns only on zero baseline",(p,c)=>{if(p!=null||!Allowed())return;RestoreExact();c.ReplyToCommand(JsonSerializer.Serialize(Snapshot()));});
RegisterListener<Listeners.OnMapStart>(map=>{owned.Clear();Server.NextFrame(RestoreExact);AddTimer(3f,RestoreExact);});
if(hotReload)Server.NextFrame(RestoreExact);

AddCommand("css_v24_spawn32_audit","Read-only spawn state",(p,c)=>{if(p!=null||!Allowed())return;var a=Snapshot();Save(a);c.ReplyToCommand(JsonSerializer.Serialize(a));});
AddCommand("css_v24_spawn32_apply","Validate accepted ground then add missing private spawns",(p,c)=>{
if(p!=null||!Allowed())return;
bool preview=c.ArgString.Trim()=="preview";
if(!preview && (Spawns("info_player_terrorist").Length!=0||Spawns("info_player_counterterrorist").Length!=0)){c.ReplyToCommand("REFUSED baseline not zero");return;}
Save(new{baseline=Snapshot(),source="accepted-20261105 seven non-launch POIs local offsets <=91 units, >=40XY separation; live ground+hull validated"});
var valid=new List<(Site,Vector)>();var checks=new List<object>();
var opt=new TraceOptions{InteractsWith=Masks.PlayerSolid};
foreach(var offset in new (float X,float Y)[]{(0,0),(64,0),(-64,0),(0,64),(0,-64),(64,64),(-64,64),(64,-64),(-64,-64)}) foreach(var original in sites){
var s=original with {X=original.X+offset.X,Y=original.Y+offset.Y};
var tr=Trace.TraceEndShape(new Vector(s.X,s.Y,s.Z+96),new Vector(s.X,s.Y,s.Z-128),null!,opt);
bool ok=tr.DidHit()&&!tr.IsAllSolid&&tr.Normal.Z>=0.7f&&Math.Abs(tr.HitPoint.Z-s.Z)<=96;
var pos=new Vector(s.X,s.Y,tr.HitPoint.Z+4);
bool clear=false;
if(ok){var h=Trace.TraceHullShape(pos,pos+new Vector(0,0,1),new Vector(-16,-16,0),new Vector(16,16,72),null!,opt);clear=!h.DidHit()&&!h.IsAllSolid;}
checks.Add(new{s.Id,hit=tr.DidHit(),solid=tr.IsAllSolid,ground=new[]{tr.HitPoint.X,tr.HitPoint.Y,tr.HitPoint.Z},normalZ=tr.Normal.Z,clear,accepted=ok&&clear});
if(ok&&clear && valid.All(v=>Math.Abs(v.Item2.X-pos.X)>=40 || Math.Abs(v.Item2.Y-pos.Y)>=40))valid.Add((s,pos));
if(valid.Count>=32)break;
}
Save(new{groundChecks=checks});
if(valid.Count<32){c.ReplyToCommand("NO VALID GROUND "+JsonSerializer.Serialize(checks));return;}
if(preview){c.ReplyToCommand("VALIDATED "+valid.Count+" nonoverlapping candidates");return;}
try{
for(int j=0;j<32;j++){int team=j%2;var v=valid[j];
var e=Utilities.CreateEntityByName<SpawnPoint>(team==0?"info_player_terrorist":"info_player_counterterrorist");
if(e==null||!e.IsValid)throw new Exception("spawn allocation failed");
owned.Add(e);e.Enabled=true;e.Priority=0;e.Teleport(v.Item2,new QAngle(0,v.Item1.Yaw,0),new Vector(0,0,0));e.DispatchSpawn();e.Enabled=true;
}
Save(new{applied=Snapshot(),owned=owned.Select(e=>e.Index).ToArray()});c.ReplyToCommand(JsonSerializer.Serialize(Snapshot()));
}catch{foreach(var e in owned.Where(e=>e.IsValid))e.Remove();owned.Clear();throw;}
});
AddCommand("css_v24_spawn32_rollback","Remove only owned hotfix spawn handles",(p,c)=>{if(p!=null||!Allowed())return;foreach(var e in owned.Where(e=>e.IsValid))e.Remove();owned.Clear();Save(new{rollback=Snapshot()});c.ReplyToCommand("owned spawns removed");});
}
public override void Unload(bool hotReload){foreach(var e in owned.Where(e=>e.IsValid))e.Remove();owned.Clear();}
}