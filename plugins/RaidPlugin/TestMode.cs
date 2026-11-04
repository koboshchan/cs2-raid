using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;

namespace Cs2Raid;
public sealed partial class RaidPlugin
{
    // A dedicated test server remains money-hard-off even when the fixture is OFF.
    private static bool TestServer => Environment.GetEnvironmentVariable("RAID_TEST_SERVER")=="1";
    private sealed class MoneyOffHandler : DelegatingHandler
    {
        public MoneyOffHandler():base(new HttpClientHandler()){}
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            if(TestServer)throw new InvalidOperationException("TEST SERVER: all bridge HTTP is hard OFF.");
            return base.SendAsync(request,cancellationToken);
        }
    }
    private readonly TestModeState testMode=new();
    private readonly Dictionary<string,uint> testControllers=[];
    private readonly Dictionary<string,Position> testDeaths=[];
    private readonly Dictionary<string,uint> testRoamed=[];
    private readonly Dictionary<uint,(int Health,int Max,MoveType_t Move,MoveType_t Actual,Color Render,RenderMode_t Mode)> testPawns=[];
    private readonly Dictionary<uint,(byte Mode,bool Forced,uint Target)> testObservers=[];
    private readonly Dictionary<string,int> testCvars=[];
    private bool TestScope() => TestModeState.Scope(Environment.GetEnvironmentVariable("RAID_TEST_SERVER"),
        ConVar<bool>.Find("sv_lan")?.Value==true,ConVar<int>.Find("hostport")?.Value??0,
        ConVar<string>.Find("ip")?.Value);
    private bool TestParticipant(CCSPlayerController? p)=>Human(p)&&testMode.Participants.ContainsKey(Id(p))&&testControllers.GetValueOrDefault(Id(p))==p.EntityHandle.Raw;
    private sealed class TestStartup { public bool DefaultEnabled {get;set;} }
    private void RegisterTestMode()
    {
        var path=Path.Combine(ModuleDirectory,"testmode.json");
        var startup=File.Exists(path)?JsonSerializer.Deserialize<TestStartup>(File.ReadAllText(path),new JsonSerializerOptions{PropertyNameCaseInsensitive=true}):null;
        if(startup?.DefaultEnabled==true)Server.NextFrame(()=>{
            try { StartTestMode();Server.PrintToConsole(TestStatus()); }
            catch(Exception ex){Server.PrintToConsole("TEST startup rejected: "+ex.Message);}
        });
        AddCommand("css_raid_test","Console-only: on/off/status/join/duo/character/next",(p,c)=>{
            if(p!=null){c.ReplyToCommand("Server console only.");return;}
            try {
                var op=c.ArgCount>1?c.GetArg(1):"status";
                if(op=="off"){StopTestMode();c.ReplyToCommand(TestStatus());return;}
                if(op=="status"){c.ReplyToCommand(TestStatus());return;}
                if(op=="on"){
                    StartTestMode();
                } else {
                    if(!testMode.Enabled || !TestScope())throw new InvalidOperationException("Test mode OFF or LAN scope invalid.");
                    CCSPlayerController Participant(int arg) {
                        if(c.ArgCount<=arg || !int.TryParse(c.GetArg(arg),out var user))throw new InvalidOperationException("Expected engine userid.");
                        var player=Utilities.GetPlayerFromUserid(user);
                        if(!Human(player))throw new InvalidOperationException("Authenticated human required.");
                        return player;
                    }
                    switch(op){
                        case "join":
                            var a=Participant(2);testMode.Join(Id(a),Human(a));testControllers[Id(a)]=a.EntityHandle.Raw;TestHealth(a);break;
                        case "duo":testMode.Duo(Id(Participant(2)),Id(Participant(3)));break;
                        case "character":
                            var who=Participant(2);
                            if(c.ArgCount!=4||!int.TryParse(c.GetArg(3),out var label))throw new InvalidOperationException("character userid label(1-6)");
                            testMode.Select(Id(who),label);break;
                        case "next":testMode.Advance();ApplyTestPhase();break;
                        default:throw new InvalidOperationException("on/off/status/join userid/duo userid userid/character userid 1-6/next");
                    }
                }
                c.ReplyToCommand(TestStatus());
            }catch(Exception ex){c.ReplyToCommand("TEST rejected: "+ex.Message);}
        });
        AddCommandListener("jointeam",(p,c)=>{
            if(!testMode.Enabled || !TestParticipant(p))return HookResult.Continue;
            if(c.ArgCount==2 && int.TryParse(c.GetArg(1),out var side)&&side is 2 or 3)p!.SwitchTeam((CsTeam)side);
            return HookResult.Handled;
        },HookMode.Pre);
        foreach(var cmd in new[]{"buy","autobuy","rebuy"})
            AddCommandListener(cmd,(p,c)=>testMode.Enabled?HookResult.Handled:HookResult.Continue,HookMode.Pre);
        RegisterListener<Listeners.OnTick>(TestTick);
        RegisterListener<Listeners.OnMapEnd>(StopTestMode);
        RegisterListener<Listeners.OnMapStart>(_=>{spreadOriginals.Clear();StopTestMode();});
        RegisterListener<Listeners.OnEntitySpawned>(e=>{
            if(!testMode.Enabled || !e.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))return;
            var raw=e.EntityHandle.Raw;var generation=testMode.Generation;
            Server.NextFrame(()=>{
                if(!testMode.Enabled || generation!=testMode.Generation)return;
                if(new CHandle<CCSWeaponBase>(raw).Value is {IsValid:true} w)PatchSpread(w);
            });
        });
        RegisterEventHandler<EventRoundStart>((e,i)=>{StopTestMode();return HookResult.Continue;},HookMode.Pre);
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(TestParticipant(e.Userid)){
                var id=Id(e.Userid!);var generation=testMode.Generation;
                Server.NextFrame(()=>{
                    if(!testMode.Enabled||generation!=testMode.Generation)return;
                    var p=Find(id);if(!TestParticipant(p))return;
                    testDeaths.Remove(id);testRoamed.Remove(id);TestHealth(p!);ApplyTestPresentation(p!);
                });
            }return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(testMode.Enabled&&TestParticipant(e.Userid)){
                var p=e.Userid!;if(Location(p) is {} pos)testDeaths[Id(p)]=pos;testRoamed.Remove(Id(p));
            }return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{
            if(TestParticipant(e.Userid)){var id=Id(e.Userid!);RestoreTestPlayer(e.Userid!);testMode.Leave(id);testControllers.Remove(id);testDeaths.Remove(id);testRoamed.Remove(id);}
            return HookResult.Continue;
        },HookMode.Pre);
    }
    private void StartTestMode(){
        if(sessionLoaded || PendingSettlement || linking.Count!=0 || gameplayConfig.Enabled)
            throw new InvalidOperationException("Production config/session or pending bridge conflicts. Disable production gameplay first.");
        testMode.Start(TestScope());
        try {
            TestCvar("mp_autoteambalance",0);TestCvar("mp_limitteams",0);
            TestCvar("mp_ignore_round_win_conditions",1);
            TestCvar("mp_respawn_on_death_t",0);TestCvar("mp_respawn_on_death_ct",0);
            PatchExistingSpread();
        }catch{StopTestMode();throw;}
    }
    private string TestStatus()=>$"TEST {(testMode.Enabled?"ON":"OFF")} | MONEY {(TestServer?"HARD OFF":"production gates")} | phase {testMode.Phase} | {testMode.Participants.Count} opted-in | 200HP / spread 0.5 shared SERVER-WIDE / CHASE living duo else ROAMING death+64 | KIT STUB labels only, NO DeltaOps | no teleport insertion, no loot/economy/session | generation {testMode.Generation}";
    private void TestCvar(string name,int value){
        var cv=ConVar<int>.Find(name)??throw new InvalidOperationException("Missing "+name);
        testCvars.TryAdd(name,cv.Value);cv.Value=value;
    }
    private void SaveTestPawn(CCSPlayerController p){
        if(p.PlayerPawn.Value is not {IsValid:true} pawn)return;
        testPawns.TryAdd(pawn.EntityHandle.Raw,(pawn.Health,pawn.MaxHealth,pawn.MoveType,pawn.ActualMoveType,pawn.Render,pawn.RenderMode));
    }
    private void TestHealth(CCSPlayerController p){SaveTestPawn(p);SpawnHealth(p);}
    private void ApplyTestPresentation(CCSPlayerController p){
        SaveTestPawn(p);
        SetPresentation(p,testMode.Phase is TestModeState.Stage.Selection or TestModeState.Stage.Countdown or TestModeState.Stage.Freeze,false);
    }
    private void ApplyTestPhase(){
        foreach(var p in Utilities.GetPlayers().Where(TestParticipant)) {
            ApplyTestPresentation(p);
            if(testMode.Phase==TestModeState.Stage.Freeze)TestHealth(p);
        }
        if(testMode.Phase==TestModeState.Stage.Closed){
            // No real raid close, settlement, suicide or session mutations.
            foreach(var p in Utilities.GetPlayers().Where(TestParticipant))RestoreTestPlayer(p);
        }
    }
    private void TestTick(){
        if(!testMode.Enabled)return;
        if(!TestScope() || sessionLoaded){StopTestMode();return;}
        if(testMode.Phase==TestModeState.Stage.Closed)return;
        foreach(var id in testMode.Participants.Keys.ToArray()){
            var p=Find(id);if(!TestParticipant(p)){testMode.Leave(id);testControllers.Remove(id);continue;}
            if(!testDeaths.TryGetValue(id,out var death)||p!.PawnIsAlive)continue;
            var pawn=p.ObserverPawn.Value;if(pawn is not {IsValid:true}||pawn.ObserverServices is not {} obs)continue;
            var mate=testMode.Participants[id];var target=mate==null?null:Find(mate);
            if(!TestParticipant(target)||!target!.PawnIsAlive)target=null;
            testObservers.TryAdd(pawn.EntityHandle.Raw,(obs.ObserverMode,obs.ForcedObserverMode,obs.ObserverTarget.Raw));
            obs.ObserverMode=(byte)(target==null?ObserverMode_t.OBS_MODE_ROAMING:ObserverMode_t.OBS_MODE_CHASE);
            obs.ForcedObserverMode=true;obs.ObserverTarget.Raw=target?.PlayerPawn.Raw??uint.MaxValue;
            Utilities.SetStateChanged(pawn,"CBasePlayerPawn","m_pObserverServices");
            if(target==null&&testRoamed.GetValueOrDefault(id)!=pawn.EntityHandle.Raw){
                pawn.Teleport(new Vector(death.X,death.Y,death.Z+64),new QAngle(0,death.Yaw,0),new Vector(0,0,0));testRoamed[id]=pawn.EntityHandle.Raw;
            }
        }
    }
    private void RestoreTestPlayer(CCSPlayerController p){
        if(p.PlayerPawn.Value is {IsValid:true} pawn&&testPawns.Remove(pawn.EntityHandle.Raw,out var saved)){
            if(p.PawnIsAlive)pawn.Health=saved.Health;pawn.MaxHealth=saved.Max;pawn.MoveType=saved.Move;pawn.ActualMoveType=saved.Actual;pawn.Render=saved.Render;pawn.RenderMode=saved.Mode;
            Utilities.SetStateChanged(pawn,"CBaseEntity","m_iHealth");Utilities.SetStateChanged(pawn,"CBaseEntity","m_iMaxHealth");
            Utilities.SetStateChanged(pawn,"CBaseEntity","m_MoveType");Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_clrRender");Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_nRenderMode");
        }
        if(p.ObserverPawn.Value is {IsValid:true} observer&&testObservers.Remove(observer.EntityHandle.Raw,out var old)&&observer.ObserverServices is {} obs){
            obs.ObserverMode=old.Mode;obs.ForcedObserverMode=old.Forced;obs.ObserverTarget.Raw=old.Target;
            Utilities.SetStateChanged(observer,"CBasePlayerPawn","m_pObserverServices");
        }
    }
    private void StopTestMode(){
        testMode.Stop(); // Fence deferred spawns before cleanup.
        try{foreach(var p in Utilities.GetPlayers().Where(Human))RestoreTestPlayer(p);}
        finally{
            if(TestServer)RestoreSpread();
            foreach(var (name,value) in testCvars)RestoreCvar(name,value);
            testCvars.Clear();testControllers.Clear();testDeaths.Clear();testRoamed.Clear();testPawns.Clear();testObservers.Clear();
        }
    }
}
