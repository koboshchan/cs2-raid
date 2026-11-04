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
    private readonly TestModeState testMode=new();
    private bool testUnloaded;
    private readonly Dictionary<string,uint> testControllers=[];
    private readonly Dictionary<string,Position> testDeaths=[];
    private readonly Dictionary<string,uint> testRoamed=[];
    private readonly Dictionary<uint,(int Health,int Max,MoveType_t Move,MoveType_t Actual,Color Render,RenderMode_t Mode)> testPawns=[];
    private readonly Dictionary<uint,(byte Mode,bool Forced,uint Target)> testObservers=[];
    private readonly Dictionary<string,Action> testCvars=[];
    private bool TestScope() => TestModeState.Scope(Environment.GetEnvironmentVariable("RAID_TEST_SERVER"),
        ConVar<bool>.Find("sv_lan")?.Value==true,ConVar<int>.Find("hostport")?.Value??0,
        ConVar<string>.Find("ip")?.Value);
    private bool TestParticipant(CCSPlayerController? p)=>Human(p)&&testMode.Participants.ContainsKey(Id(p))&&testControllers.GetValueOrDefault(Id(p))==p.EntityHandle.Raw;
    private sealed class TestStartup { public bool DefaultEnabled {get;set;} }
    private void RegisterTestMode()
    {
        var path=Path.Combine(ModuleDirectory,"testmode.json");
        var startup=File.Exists(path)?JsonSerializer.Deserialize<TestStartup>(File.ReadAllText(path),new JsonSerializerOptions{PropertyNameCaseInsensitive=true}):null;
        var startupGeneration=testMode.Generation;
        if(startup?.DefaultEnabled==true)Server.NextFrame(()=>{
            if(!testMode.CallbackAllowed(startupGeneration,testUnloaded))return;
            try { StartTestMode();Server.PrintToConsole(TestStatus()); }
            catch(Exception ex){Server.PrintToConsole("TEST startup rejected: "+ex.Message);}
        });
        AddCommand("css_raid_test","Console-only: on/off/status/join/spawn/duo/character/next",(p,c)=>{
            if(p!=null){c.ReplyToCommand("Server console only.");return;}
            try {
                var op=c.ArgCount>1?c.GetArg(1):"status";
                if(op=="off"){StopTestMode();c.ReplyToCommand(TestStatus());return;}
                if(op=="status"){c.ReplyToCommand(TestStatus());return;}
                if(op=="on"){
                    StartTestMode();
                } else {
                    if((op!="inspect"&&!testMode.Enabled) || !TestScope())throw new InvalidOperationException("Test mode OFF or LAN scope invalid.");
                    CCSPlayerController Participant(int arg) {
                        if(c.ArgCount<=arg || !int.TryParse(c.GetArg(arg),out var user))throw new InvalidOperationException("Expected engine userid.");
                        var player=Utilities.GetPlayerFromUserid(user);
                        if(!Human(player))throw new InvalidOperationException("Authenticated human required.");
                        return player;
                    }
                    switch(op){
                        case "inspect":
                            var inspected=Participant(2);var native=inspected.PlayerPawn.Value;
                            c.ReplyToCommand($"NATIVE userid={inspected.UserId} team={inspected.TeamNum} alive={inspected.PawnIsAlive} health={native?.Health} max={native?.MaxHealth}");
                            if(!spreadMapEnded && native?.WeaponServices?.ActiveWeapon.Value is CCSWeaponBase gun && gun.VData is {} vd && spreadOriginals.TryGetValue(vd.Handle,out var original)){
                                var current=SpreadModes(vd);
                                for(var mode=0;mode<current.Length;mode++)
                                    c.ReplyToCommand($"NATIVE gun={gun.DesignerName} mode={mode} baseline=[{string.Join(",",original.Modes[mode])}] current=[{string.Join(",",current[mode].Values.ToArray())}] expected=[{string.Join(",",original.Modes[mode].Select(TonightRules.Half))}]");
                            }else c.ReplyToCommand("No held gun with saved current-map baseline; spread ratio NOT verified.");
                            break;
                        case "join":
                            var a=Participant(2);testMode.Join(Id(a),Human(a));testControllers[Id(a)]=a.EntityHandle.Raw;
                            if(a.PawnIsAlive)TestHealth(a);else c.ReplyToCommand("Enrolled but dead/spectator. Use css_raid_test spawn <userid> <2|3> in Lobby; then verify 200HP before next.");break;
                        case "spawn":
                            var spawning=Participant(2);
                            if(!testMode.CanRespawn(TestParticipant(spawning))||c.ArgCount!=4||!int.TryParse(c.GetArg(3),out var team)||team is not (2 or 3))
                                throw new InvalidOperationException("Lobby-only enrolled spawn userid 2|3.");
                            if(spawning.PawnIsAlive)throw new InvalidOperationException("Already alive; no respawn performed.");
                            spawning.SwitchTeam((CsTeam)team);spawning.Respawn();break;
                        case "duo":testMode.Duo(Id(Participant(2)),Id(Participant(3)));break;
                        case "character":
                            var who=Participant(2);
                            if(c.ArgCount!=4||!int.TryParse(c.GetArg(3),out var label))throw new InvalidOperationException("character userid label(1-6)");
                            testMode.Select(Id(who),label);break;
                        case "give":
                            var receiver=Participant(2);
                            if(!TestParticipant(receiver)||testMode.Phase!=TestModeState.Stage.Buy||c.ArgCount!=4 ||
                               PriceItem(c.GetArg(3)) is not {IsGun:true} item)
                                throw new InvalidOperationException("Buy-phase free gun fixture only: give userid weapon_entity");
                            receiver.GiveNamedItem(item.Entity);break;
                        case "next":
                            if(testMode.Phase==TestModeState.Stage.Lobby && testMode.Participants.Keys.Any(id=>Find(id) is not {} live || !TestParticipant(live)||!live.PawnIsAlive))
                                throw new InvalidOperationException("All enrolled players must be alive. Use spawn userid 2|3 in Lobby.");
                            testMode.Advance();ApplyTestPhase();break;
                        default:throw new InvalidOperationException("on/off/status/join userid/spawn userid 2|3/duo userid userid/character userid 1-6/give userid weapon_entity/next");
                    }
                }
                c.ReplyToCommand(TestStatus());
            }catch(Exception ex){c.ReplyToCommand("TEST rejected: "+ex.Message);}
        });
        AddCommandListener("jointeam",(p,c)=>{
            if(!testMode.NativeActive || !TestParticipant(p))return HookResult.Continue;
            if(c.ArgCount==2 && int.TryParse(c.GetArg(1),out var side)&&side is 2 or 3)p!.SwitchTeam((CsTeam)side);
            return HookResult.Handled;
        },HookMode.Pre);
        foreach(var cmd in new[]{"buy","autobuy","rebuy"})
            AddCommandListener(cmd,(p,c)=>testMode.NativeActive?HookResult.Handled:HookResult.Continue,HookMode.Pre);
        RegisterListener<Listeners.OnTick>(TestTick);
        RegisterListener<Listeners.OnMapEnd>(()=>{try{StopTestMode();}finally{spreadMapEnded=true;AbandonExpiredSpread();}});
        RegisterListener<Listeners.OnMapStart>(_=>{AbandonExpiredSpread();spreadMapEnded=false;StopTestMode();});
        RegisterListener<Listeners.OnEntitySpawned>(e=>{
            if(!testMode.NativeActive || !e.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))return;
            var raw=e.EntityHandle.Raw;var generation=testMode.Generation;
            Server.NextFrame(()=>{
                if(!testMode.NativeActive || !testMode.CallbackAllowed(generation,testUnloaded))return;
                if(new CHandle<CCSWeaponBase>(raw).Value is {IsValid:true} w)PatchSpread(w);
            });
        });
        RegisterEventHandler<EventRoundStart>((e,i)=>{StopTestMode();return HookResult.Continue;},HookMode.Pre);
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(testMode.NativeActive&&TestParticipant(e.Userid)){
                var id=Id(e.Userid!);var generation=testMode.Generation;
                Server.NextFrame(()=>{
                    if(!testMode.NativeActive||!testMode.CallbackAllowed(generation,testUnloaded))return;
                    var p=Find(id);if(!TestParticipant(p))return;
                    testDeaths.Remove(id);testRoamed.Remove(id);TestHealth(p!);ApplyTestPresentation(p!);
                });
            }return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(testMode.NativeActive&&TestParticipant(e.Userid)){
                var p=e.Userid!;if(Location(p) is {} pos)testDeaths[Id(p)]=pos;testRoamed.Remove(Id(p));
            }return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{
            if(TestParticipant(e.Userid)){var id=Id(e.Userid!);try{RestoreTestPlayer(e.Userid!);}finally{testMode.Leave(id);testControllers.Remove(id);testDeaths.Remove(id);testRoamed.Remove(id);}}
            return HookResult.Continue;
        },HookMode.Pre);
    }
    private void StartTestMode(){
        if(testUnloaded || spreadMapEnded || testPawns.Count!=0 || testObservers.Count!=0 || testCvars.Count!=0 || TestServer&&spreadOriginals.Count!=0)
            throw new InvalidOperationException("Unloaded or pending native cleanup. Retry off; inspect saved baseline counters.");
        if(sessionLoaded || PendingSettlement || linking.Count!=0 || gameplayConfig.Enabled)
            throw new InvalidOperationException("Production config/session or pending bridge conflicts. Disable production gameplay first.");
        testMode.Start(TestScope());
        try {
            TestCvar("mp_autoteambalance",false);TestCvar("mp_limitteams",0);
            TestCvar("mp_ignore_round_win_conditions",true);
            TestCvar("mp_respawn_on_death_t",false);TestCvar("mp_respawn_on_death_ct",false);
            PatchExistingSpread();
        }catch{StopTestMode();throw;}
    }
    private string TestStatus()=>$"TEST {(testMode.Enabled?"ON":"OFF")} | MONEY {(TestServer?"HARD OFF":"production gates")} | phase {testMode.Phase} | {testMode.Participants.Count} opted-in | 200HP saved-pawns={testPawns.Count} / spread 0.5 baselines={spreadOriginals.Count} shared SERVER-WIDE / CHASE living duo else ROAMING death+64 observers={testObservers.Count} | KIT STUB labels only, NO DeltaOps | native T/CT damage rules (NOT duo combat acceptance) | no teleport insertion, no loot/economy/session | pending-cvars={testCvars.Count} | generation {testMode.Generation}";
    private void TestCvar<T>(string name,T value){
        var cv=ConVar<T>.Find(name)??throw new InvalidOperationException("Missing "+name);
        TestModeState.SetCaptured(testCvars,name,()=>cv.Value,
            saved=>(ConVar<T>.Find(name)??throw new InvalidOperationException("Missing restore cvar "+name)).Value=saved,value);
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
        if(testMode.Phase==TestModeState.Stage.Closed){CleanupTestNative();return;}
        foreach(var p in Utilities.GetPlayers().Where(TestParticipant)) {
            ApplyTestPresentation(p);
            if(testMode.Phase==TestModeState.Stage.Freeze)TestHealth(p);
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
    private void TestCleanupError(Exception ex)=>Server.PrintToConsole("TEST cleanup pending: "+ex.Message);
    private void RestoreTestPlayer(CCSPlayerController p){
        // Each resource is isolated; raw handles include entity serials and never target replacements.
        var pawn=p.PlayerPawn.Raw;var observer=p.ObserverPawn.Raw;
        RestoreTestPawns(pawn);RestoreTestObservers(observer);
    }
    private void RestoreTestPawns(uint? only=null){
        var entries=only is {} raw?testPawns.Where(x=>x.Key==raw).ToDictionary():testPawns;
        TestModeState.RestoreEntries(entries,(handle,saved)=>{
            if(new CHandle<CCSPlayerPawn>(handle).Value is {IsValid:true} pawn){
                // Never resurrect a dead pawn during cleanup.
                if(pawn.Health>0)pawn.Health=saved.Health;
                pawn.MaxHealth=saved.Max;pawn.MoveType=saved.Move;pawn.ActualMoveType=saved.Actual;pawn.Render=saved.Render;pawn.RenderMode=saved.Mode;
                Utilities.SetStateChanged(pawn,"CBaseEntity","m_iHealth");Utilities.SetStateChanged(pawn,"CBaseEntity","m_iMaxHealth");
                Utilities.SetStateChanged(pawn,"CBaseEntity","m_MoveType");Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_clrRender");Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_nRenderMode");
            }
            testPawns.Remove(handle);
        },TestCleanupError);
    }
    private void RestoreTestObservers(uint? only=null){
        var entries=only is {} raw?testObservers.Where(x=>x.Key==raw).ToDictionary():testObservers;
        TestModeState.RestoreEntries(entries,(handle,old)=>{
            if(new CHandle<CCSObserverPawn>(handle).Value is {IsValid:true} observer){
                var obs=observer.ObserverServices??throw new InvalidOperationException("Observer services unavailable; baseline retained.");
                obs.ObserverMode=old.Mode;obs.ForcedObserverMode=old.Forced;obs.ObserverTarget.Raw=old.Target;
                Utilities.SetStateChanged(observer,"CBasePlayerPawn","m_pObserverServices");
            }
            testObservers.Remove(handle);
        },TestCleanupError);
    }
    private void CleanupTestNative(){
        TestModeState.FinalizeAll(TestCleanupError,
            ()=>RestoreTestPawns(),()=>RestoreTestObservers(),
            ()=>{if(TestServer)TestModeState.RestoreEntries(spreadOriginals,(_,baseline)=>{
                var modes=SpreadModes(baseline.Data);
                for(var i=0;i<modes.Length;i++)baseline.Modes[i].AsSpan().CopyTo(modes[i].Values);
                baseline.Data.InaccuracyJumpInitial=baseline.JumpInitial;
                baseline.Data.InaccuracyJumpApex=baseline.JumpApex;
                baseline.Data.InaccuracyReload=baseline.Reload;
            },TestCleanupError);},
            ()=>TestModeState.RestoreEntries(testCvars,(_,restore)=>restore(),TestCleanupError));
    }
    private void StopTestMode(){
        testMode.Stop(); // Fence all callbacks before touching native resources.
        try{CleanupTestNative();}
        finally{testControllers.Clear();testDeaths.Clear();testRoamed.Clear();}
    }
}
