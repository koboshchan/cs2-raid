using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
namespace PrivateV24Control;
public sealed class ControlConfig:BasePluginConfig {
public bool ForceCqOff {get;set;}=false; // Change only after Lucas approves the cq_enable=1 test.
}
public sealed class Plugin:BasePlugin,IPluginConfig<ControlConfig>{
public ControlConfig Config {get;set;}=new();
public void OnConfigParsed(ControlConfig c){Config=c;}
static bool RaidOwnsRespawn(){try{var p="/tmp/kat-private-raid-flow-active";return File.Exists(p)&&(DateTime.UtcNow-File.GetLastWriteTimeUtc(p)).TotalSeconds<15&&File.ReadAllText(p)==Server.MapName&&Server.MapName=="df_spaceport_v261";}catch{return false;}}

public override string ModuleName=>"PrivateV24Control";
public override string ModuleVersion=>"1.2.0-raid-coop";
public override string ModuleAuthor=>"koboshchan";
static void Log(string x)=>File.AppendAllText("/debugtest/reports/v24-live-readback.log",DateTime.UtcNow.ToString("O")+" "+x+"\n");
static bool Allowed(){
var a=File.ReadAllText("/proc/1/cmdline").Split('\0');var i=Array.IndexOf(a,"-port");
return a.Contains("-dedicated")&&i>=0&&i+1<a.Length&&a[i+1]=="27016"&&ConVar.Find("hostport")?.GetPrimitiveValue<int>()==27016;
}
static string Values()=> $"map={Server.MapName} cq={ConVar.Find("cq_enable")!.GetPrimitiveValue<bool>()} timeout={ConVar.Find("sv_timeout")!.GetPrimitiveValue<float>()} respawnT={ConVar.Find("mp_respawn_on_death_t")!.GetPrimitiveValue<bool>()} respawnCT={ConVar.Find("mp_respawn_on_death_ct")!.GetPrimitiveValue<bool>()}";
void Apply(bool report=false){
try{
if(!Allowed()){if(report)Log("REFUSED private identity");return;}
ConVar.Find("cq_enable")!.SetValue(!Config.ForceCqOff);
ConVar.Find("sv_timeout")!.SetValue(200f);
if(!RaidOwnsRespawn()){
ConVar.Find("mp_respawn_on_death_t")!.SetValue(true);
ConVar.Find("mp_respawn_on_death_ct")!.SetValue(true);
}
if(report)Log("VERIFIED "+Values()+" spawnsT="+Utilities.FindAllEntitiesByDesignerName<SpawnPoint>("info_player_terrorist").Count(e=>e.IsValid&&e.Enabled)+" spawnsCT="+Utilities.FindAllEntitiesByDesignerName<SpawnPoint>("info_player_counterterrorist").Count(e=>e.IsValid&&e.Enabled));
}catch(Exception e){Log(e.ToString());}}
public override void Load(bool hotReload){
AddCommand("css_v24_read","Private settings apply and verified readback",(p,c)=>{if(p==null){Apply(true);c.ReplyToCommand(Values());}});
RegisterListener<Listeners.OnMapStart>(map=>{Server.NextFrame(()=>Apply(true));AddTimer(3f,()=>Apply(true));});
AddTimer(5f,()=>Apply(),TimerFlags.REPEAT);
Apply(true);
}
}