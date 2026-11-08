using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
public sealed class Plugin:BasePlugin{
public override string ModuleName=>"PrivateSpawnHullAudit";
public override string ModuleVersion=>"1.0.0";
public override string ModuleAuthor=>"koboshchan";
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

bool Allowed(){var a=File.ReadAllText("/proc/1/cmdline").Split('\0');var i=Array.IndexOf(a,"-port");return a.Contains("-dedicated")&&i>=0&&i+1<a.Length&&a[i+1]=="27016"&&ConVar.Find("hostport")?.GetPrimitiveValue<int>()==27016&&Server.MapName=="df_spaceport_v261";}
static readonly TraceOptions opt=new(){InteractsWith=Masks.PlayerSolid};
static object V(Vector p)=>new[]{p.X,p.Y,p.Z};
static TraceResult Hull(Vector p,CBaseEntity? ignore=null)=>Trace.TraceHullShape(p,p+new Vector(0,0,1),new Vector(-16,-16,0),new Vector(16,16,72),ignore,opt);
static bool Clear(Vector p,CBaseEntity? ignore=null){var h=Hull(p,ignore);return !h.DidHit()&&!h.IsAllSolid;}
static object H(Vector p,CBaseEntity? ignore=null){var h=Hull(p,ignore);return new{position=V(p),hit=h.DidHit(),solid=h.IsAllSolid,h.Fraction,point=V(h.HitPoint),normal=V(h.Normal)};}
static object Ray(float x,float y,float top,float bottom,CBaseEntity? ignore=null){var h=Trace.TraceEndShape(new Vector(x,y,top),new Vector(x,y,bottom),ignore,opt);return new{top,bottom,hit=h.DidHit(),solid=h.IsAllSolid,h.Fraction,point=V(h.HitPoint),normal=V(h.Normal)};}
public override void Load(bool hotReload){
AddCommand("css_spawn_hull_fix","Console-only exact fixed Z for audited blocked spawns",(p,c)=>{
if(p!=null||!Allowed())return;
int n=int.Parse(c.GetArg(1));if(n!=13&&n!=18&&n!=26){c.ReplyToCommand("REFUSED not audited blocked");return;}
var x=exact[n];float d=n==26?56:97;
var m=Utilities.FindAllEntitiesByDesignerName<SpawnPoint>(x.Class).Where(e=>e.IsValid&&e.AbsOrigin!=null&&Math.Abs(e.AbsOrigin.X-x.X)<0.02&&Math.Abs(e.AbsOrigin.Y-x.Y)<0.02).ToArray();
if(m.Length!=1)throw new Exception("Ambiguous spawn");
var e=m[0];var old=e.AbsOrigin!;var next=new Vector(x.X,x.Y,x.Z+d);
if(Math.Abs(old.Z-x.Z-100)>0.02&&Math.Abs(old.Z-next.Z)>0.02)throw new Exception("Unexpected Z");
if(!Clear(next))throw new Exception("Proposed hull not clear");
var before=V(old);
e.Teleport(next,e.AbsRotation,new Vector(0,0,0));
c.ReplyToCommand(JsonSerializer.Serialize(new{n,index=e.Index,before,after=H(e.AbsOrigin!),delta=d,ground=Ray(x.X,x.Y,next.Z,next.Z-256)}));
});
AddCommand("css_spawn_hull","Console-only private spawn hull investigation",(p,c)=>{
if(p!=null||!Allowed())return;
int n=int.Parse(c.GetArg(1)); if(n<0||n>=exact.Length)return;
var x=exact[n];var matches=Utilities.FindAllEntitiesByDesignerName<SpawnPoint>(x.Class).Where(e=>e.IsValid&&e.AbsOrigin!=null&&Math.Abs(e.AbsOrigin.X-x.X)<0.02&&Math.Abs(e.AbsOrigin.Y-x.Y)<0.02).ToArray();
if(matches.Length!=1){c.ReplyToCommand("REFUSED ambiguous "+matches.Length);return;}
var e=matches[0];var v=e.AbsOrigin!;
var players=Utilities.GetPlayers().Where(q=>q.IsValid&&!q.IsBot&&q.PlayerPawn.Value is {IsValid:true,AbsOrigin:not null}).Select(q=>{var pawn=q.PlayerPawn.Value!;var f=pawn.AbsOrigin!;return new{name=q.PlayerName,q.TeamNum,feet=V(f),hull=H(f,pawn),floor=Ray(f.X,f.Y,f.Z+1,f.Z-256,pawn)};}).ToArray();
var clear=new List<int>();
for(int d=100;d>=0;d--)if(Clear(new Vector(x.X,x.Y,x.Z+d)))clear.Add(d);
c.ReplyToCommand(JsonSerializer.Serialize(new{n,index=e.Index,e.Enabled,original=x,current=H(v),delta=v.Z-x.Z,groundFromAbove=Ray(x.X,x.Y,x.Z+200,x.Z-256),groundFromSpawn=Ray(x.X,x.Y,v.Z,v.Z-256),groundAtReported=Ray(x.X,x.Y,229.12f,0),atReportedGround=H(new Vector(x.X,x.Y,228.12f)),atReportedPlus100=H(new Vector(x.X,x.Y,328.12f)),highestClearDelta=clear.Count>0?(int?)clear[0]:null,clearCount=clear.Count,players}));
});
}
}