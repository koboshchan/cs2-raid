using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Memory;
using System.Net.Http;
using System.Text.Json;

namespace Cs2Raid;
public sealed partial class RaidPlugin
{
    // A dedicated test server remains money-hard-off even when the fixture is OFF.
    private static bool TestServer => Environment.GetEnvironmentVariable("RAID_TEST_SERVER")=="1";
    private readonly TestModeState testMode=new();
    private bool testUnloaded;
    private int testTraceRemaining;
    private TestFlow testFlow=new();
    private readonly List<Position> testSpawns=[];
    private readonly HashSet<uint> testWeapons=[];
    private int testLastCount=-1;
    private DateTimeOffset testNextWaiting;
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
    private static string BotId(CCSPlayerController p)=>"bot:"+p.EntityHandle.Raw;
    private bool TestBot(CCSPlayerController? p)=>TestServer&&p is {IsValid:true,IsBot:true}&&testFlow.Bots.ContainsKey(BotId(p));
    private CCSPlayerController? TestController(CBaseEntity entity)=>Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&(p.EntityHandle.Raw==entity.EntityHandle.Raw||p.PlayerPawn.Raw==entity.EntityHandle.Raw));
    private void AutoEnrollTestBots(){
        if(!testMode.AutoEnrollAllowed(TestServer,TestScope(),true,testUnloaded))return;
        try{
        foreach(var p in Utilities.GetPlayers().Where(p=>p.IsValid&&p.IsBot)){
            if(!testFlow.EnrollBot(BotId(p),true,true))continue;
            if(!p.PawnIsAlive){if(p.TeamNum is not (2 or 3))p.SwitchTeam(CsTeam.CounterTerrorist);p.Respawn();}
            else{TestHealth(p);ApplyTestPresentation(p);}
        }
        }catch(Exception ex){FailTestNative(ex);}
    }
    private sealed class TestStartup { public bool DefaultEnabled {get;set;} }
    private void RegisterTestMode()
    {
        var path=Path.Combine(ModuleDirectory,"testmode.json");
        var startup=File.Exists(path)?JsonSerializer.Deserialize<TestStartup>(File.ReadAllText(path),new JsonSerializerOptions{PropertyNameCaseInsensitive=true}):null;
        var startupGeneration=testMode.Generation;
        if(startup?.DefaultEnabled==true)Server.NextFrame(()=>{
try{
            if(!testMode.CallbackAllowed(startupGeneration,testUnloaded))return;
            try { StartTestMode();Server.PrintToConsole(TestStatus()); }
            catch(Exception ex){Server.PrintToConsole("TEST startup rejected: "+ex.Message);}
        
}catch(Exception ex){FailTestNative(ex);}
});
        AddCommand("css_raid_test","Console-only: on/off/status/join/spawn/duo/character/next",(p,c)=>{
            if(p!=null){c.ReplyToCommand("Server console only.");return;}
            try {
                var op=c.ArgCount>1?c.GetArg(1):"status";
                if(op=="off"){StopTestMode();c.ReplyToCommand(TestStatus());return;}
                if(op=="status"){c.ReplyToCommand(TestStatus());return;}
                if(op=="state"){
                    if(!TestServer||!TestScope())throw new InvalidOperationException("Private dedicated scope required.");
                    var proxy=Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault(x=>x.IsValid);
                    var rules=proxy?.GameRules;
                    if(rules==null||rules.Handle==IntPtr.Zero)c.ReplyToCommand("GameRules unavailable; warmup/freeze UNKNOWN.");
                    else c.ReplyToCommand($"NATIVE RULES unixMs={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()} time={Server.CurrentTime} warmup={rules.WarmupPeriod} warmupEnd={rules.WarmupPeriodEnd} freeze={rules.FreezePeriod} roundTime={rules.RoundTime} restartRoundTime={rules.RestartRoundTime} fixturePhase={testMode.Phase} generation={testMode.Generation}");
                    return;
                }
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
                        case "trace":
                            if(c.ArgCount!=3||!int.TryParse(c.GetArg(2),out var count)||count<0||count>32)throw new InvalidOperationException("trace 0..32, bounded console-only damage decisions.");
                            testTraceRemaining=count;c.ReplyToCommand($"Trace armed {count} decisions; cleared on OFF/round.");break;
                        case "inspect":
                            if(c.ArgCount!=3||!int.TryParse(c.GetArg(2),out var inspectUser))throw new InvalidOperationException("inspect engine userid");
                            var inspected=Utilities.GetPlayerFromUserid(inspectUser);
                            if(inspected is not {IsValid:true})throw new InvalidOperationException("Valid native controller required.");var native=inspected.PlayerPawn.Value;
                            c.ReplyToCommand($"NATIVE userid={inspected.UserId} team={inspected.TeamNum} alive={inspected.PawnIsAlive} health={native?.Health} max={native?.MaxHealth} bot={inspected.IsBot} enrolled={TestParticipant(inspected)||TestBot(inspected)} solo-id={(inspected.IsBot?BotId(inspected):Id(inspected))} phase={testMode.Phase} generation={testMode.Generation} move={native?.MoveType} actualmove={native?.ActualMoveType} flags={native?.Flags}");
                            if(!spreadMapEnded && native?.WeaponServices?.ActiveWeapon.Value is CCSWeaponBase gun && gun.VData is {} vd && spreadOriginals.TryGetValue(vd.Handle,out var original)){
                                var current=SpreadModes(vd);
                                for(var mode=0;mode<current.Length;mode++)
                                    c.ReplyToCommand($"NATIVE gun={gun.DesignerName} mode={mode} baseline=[{string.Join(",",original.Modes[mode])}] current=[{string.Join(",",current[mode].Values.ToArray())}] expected=[{string.Join(",",original.Modes[mode].Select(TonightRules.Half))}]");
                            }else c.ReplyToCommand("No held gun with saved current-map baseline; spread ratio NOT verified.");
                            break;
                        case "capture":
                            var captured=Participant(2);
                            if(testMode.Phase!=TestModeState.Stage.Lobby || !TestParticipant(captured) || !captured.PawnIsAlive || Location(captured) is not {Valid:true} point)
                                throw new InvalidOperationException("Capture requires living enrolled Lobby player.");
                            if(!testSpawns.Any(x=>x.Within(point,64)))testSpawns.Add(point);
                            c.ReplyToCommand($"Captured actual position {point}; spawn-count={testSpawns.Count}");break;
                        case "join":
                            var a=Participant(2);
                            if(!testFlow.Enroll(testMode,Id(a),Human(a)))throw new InvalidOperationException("Enrollment rejected atomically: authenticated Lobby only, maximum seven participants.");
                            testControllers[Id(a)]=a.EntityHandle.Raw;
                            if(a.PawnIsAlive)PrepareTestLobby(a);else c.ReplyToCommand("Enrolled but dead/spectator. Use css_raid_test spawn <userid> <2|3> in Lobby; then verify 200HP before next.");break;
                        case "spawn":
                            var spawning=Participant(2);
                            if(!testMode.CanRespawn(TestParticipant(spawning))||c.ArgCount!=4||!int.TryParse(c.GetArg(3),out var team)||team is not (2 or 3))
                                throw new InvalidOperationException("Lobby-only enrolled spawn userid 2|3.");
                            if(spawning.PawnIsAlive)throw new InvalidOperationException("Already alive; no respawn performed.");
                            spawning.SwitchTeam((CsTeam)team);spawning.Respawn();break;
                        case "duo":
                            var first=Id(Participant(2));var second=Id(Participant(3));testMode.Duo(first,second);
                            testFlow.Teams.ShootRequest(first,second);testFlow.Teams.ShootRequest(second,first);break;
                        case "character":
                            var who=Participant(2);
                            if(c.ArgCount!=4||!int.TryParse(c.GetArg(3),out var label))throw new InvalidOperationException("character userid label(1-6)");
                            testMode.Select(Id(who),label);testFlow.Teams.SelectCharacter(Id(who),label);break;
                        case "give":
                            throw new InvalidOperationException("Free give removed. Use native buy <allowlisted gun> during automatic Buy; finite fixture credits.");
                        case "next":
                            if(testMode.Phase==TestModeState.Stage.Lobby && testMode.Participants.Keys.Any(id=>Find(id) is not {} live || !TestParticipant(live)||!live.PawnIsAlive))
                                throw new InvalidOperationException("All enrolled players must be alive. Use spawn userid 2|3 in Lobby.");
                            testFlow.Manual=true;testMode.Advance();ApplyTestPhase();break;
                        default:throw new InvalidOperationException("on/off/status/join userid/spawn userid 2|3/capture userid/duo userid userid/character userid 1-6/next (manual QA disables auto)");
                    }
                }
                c.ReplyToCommand(TestStatus());
            }catch(Exception ex){c.ReplyToCommand("TEST rejected: "+ex.Message);}
        });
        AddCommandListener("jointeam",(p,c)=>{
            if(!testMode.NativeActive || !TestParticipant(p))return HookResult.Continue;
            if(testMode.Phase==TestModeState.Stage.Lobby && c.ArgCount==2 && int.TryParse(c.GetArg(1),out var side)&&side is 2 or 3)p!.SwitchTeam((CsTeam)side);
            return HookResult.Handled;
        },HookMode.Pre);
        if(TestServer){
            AddCommand("css_team","Fixture solo readiness: !team single",(p,c)=>{
                if(!TestPlayerCommand(p,c))return;
                if(c.ArgString.Trim()=="single" && testFlow.Teams.Solo(Id(p!)))p!.PrintToChat("TEST solo ready. !cancelteam to cancel before Buy.");
            });
            AddCommand("css_cancelteam","Cancel fixture solo/duo before Buy",(p,c)=>{
                if(!TestPlayerCommand(p,c))return;
                var changed=testFlow.Teams.Cancel(Id(p!));
                if(changed.Length>0){SyncTestTeams();testMode.Sync(TestModeState.Stage.Lobby);ApplyTestPhase();TestAnnounce("Team cancelled. Shoot each other or !team single.");}
            });
            AddCommand("css_characterselect","Fixture character state 1..6, no DeltaOps abilities",(p,c)=>{
                if(!TestPlayerCommand(p,c))return;
                if(c.ArgCount==2&&int.TryParse(c.GetArg(1),out var n)&&testFlow.Teams.SelectCharacter(Id(p!),n))
                    p!.PrintToChat("TEST selected "+GameplayState.Characters[n-1]+"; KIT STUB, NO DeltaOps abilities.");
                else p!.PrintToChat("Select during Selection with !characterselect 1..6.");
            });
            AddCommand("css_testshop","Fixture credits and strict gun prices",(p,c)=>{
                if(!TestPlayerCommand(p,c))return;
                p!.PrintToChat($"TEST credits {testFlow.Wallet.GetValueOrDefault(Id(p))}; NO SSG/real cash. Native console buy <gun> or !testbuy <gun>.");
                foreach(var (name,price) in TestFlow.Prices)p.PrintToChat($"{name} {price} fixture credits");
            });
            AddCommand("css_testbuy","Native gun fixture purchase",(p,c)=>{
                if(TestPlayerCommand(p,c))TestPurchase(p!,c.ArgCount==2?c.GetArg(1):"");
            });
        }
        AddCommandListener("buy",(p,c)=>{
            if(!TestServer||!testMode.NativeActive)return HookResult.Continue;
            if(TestParticipant(p))TestPurchase(p!,c.ArgCount==2?c.GetArg(1):"");
            return HookResult.Handled;
        },HookMode.Pre);
        foreach(var cmd in new[]{"autobuy","rebuy","drop"})
            AddCommandListener(cmd,(p,c)=>TestServer&&testMode.NativeActive?HookResult.Handled:HookResult.Continue,HookMode.Pre);
        RegisterListener<Listeners.OnEntityTakeDamagePre>(TestDamage);
        AddTimer(1,TestSecond,TimerFlags.REPEAT);
        RegisterListener<Listeners.OnTick>(TestTick);
        RegisterListener<Listeners.OnMapEnd>(()=>{try{StopTestMode();}finally{spreadMapEnded=true;testWeapons.Clear();AbandonExpiredSpread();}});
        RegisterListener<Listeners.OnMapStart>(_=>{testWeapons.Clear();AbandonExpiredSpread();spreadMapEnded=false;StopTestMode();});
        RegisterListener<Listeners.OnEntitySpawned>(e=>{
            if(!testMode.NativeActive || !e.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))return;
            var raw=e.EntityHandle.Raw;var generation=testMode.Generation;
            Server.NextFrame(()=>{
try{
                if(!testMode.NativeActive || !testMode.CallbackAllowed(generation,testUnloaded))return;
                if(new CHandle<CCSWeaponBase>(raw).Value is {IsValid:true} w)PatchSpread(w);
            
}catch(Exception ex){FailTestNative(ex);}
});
        });
        // Native round start USED TO unconditionally StopTestMode, clearing all enrollment.
        // Team elimination/restart can cause this event even with custom flow enabled.
        RegisterEventHandler<EventRoundStart>((e,i)=>{
            if(!testMode.RebuildRound(TestServer,TestScope(),testUnloaded))return HookResult.Continue;
            // Fence old spawn/timer callbacks, restore prior resources, then rebuild a fresh fixture.
            // Round restart discards TEST credits/teams only, never production economic state.
            StopTestMode();
            try{StartTestMode();}
            catch(Exception ex){Server.PrintToConsole("TEST round rebuild failed closed: "+ex.Message);}
            return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventPlayerConnectFull>((e,i)=>{
            if(TestServer&&testMode.Enabled&&e.Userid is {IsValid:true,IsBot:false} connected){
                var generation=testMode.Generation;var raw=connected.EntityHandle.Raw;
                Server.NextFrame(()=>{
try{
                    if(!testMode.CallbackAllowed(generation,testUnloaded))return;
                    var current=Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&p.EntityHandle.Raw==raw);
                    if(Human(current)&&current.Connected==PlayerConnectedState.Connected)AutoEnrollTestHumans();
                
}catch(Exception ex){FailTestNative(ex);}
});
            }
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(testMode.NativeActive&&TestBot(e.Userid)){
                var raw=e.Userid!.EntityHandle.Raw;var generation=testMode.Generation;
                Server.NextFrame(()=>{
try{
                    if(!testMode.NativeActive||!testMode.CallbackAllowed(generation,testUnloaded)||!TestScope())return;
                    var bot=Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&p.EntityHandle.Raw==raw);
                    if(!TestBot(bot))return;
                    if(!testFlow.BotLiving(BotId(bot!))){bot!.ChangeTeam(CsTeam.Spectator);return;}
                    TestHealth(bot!);ApplyTestPresentation(bot!);
                
}catch(Exception ex){FailTestNative(ex);}
});
            }
            if(testMode.NativeActive&&TestParticipant(e.Userid)){
                var id=Id(e.Userid!);var generation=testMode.Generation;
                Server.NextFrame(()=>{
try{
                    if(!testMode.NativeActive||!testMode.CallbackAllowed(generation,testUnloaded))return;
                    var p=Find(id);if(!TestParticipant(p))return;
                    testDeaths.Remove(id);testRoamed.Remove(id);if(testMode.Phase==TestModeState.Stage.Lobby)PrepareTestLobby(p!);
                    else {
                        if(!testFlow.Manual&&!testFlow.Teams.Living(id)){p!.ChangeTeam(CsTeam.Spectator);return;}
                        TestHealth(p!);ApplyTestPresentation(p!);
                    }

                
}catch(Exception ex){FailTestNative(ex);}
});
            }return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(testMode.NativeActive&&TestBot(e.Userid)&&testMode.Phase==TestModeState.Stage.Active)testFlow.KillBot(BotId(e.Userid!));
            if(testMode.NativeActive&&testMode.Phase==TestModeState.Stage.Lobby&&(TestParticipant(e.Userid)||TestBot(e.Userid))){
                var raw=e.Userid!.EntityHandle.Raw;var generation=testMode.Generation;
                Server.NextFrame(()=>{
try{
                        if(!testMode.CallbackAllowed(generation,testUnloaded)||testMode.Phase!=TestModeState.Stage.Lobby||!TestScope())return;
                        var current=Utilities.GetPlayers().FirstOrDefault(x=>x.IsValid&&x.EntityHandle.Raw==raw);
                        if(current==null||!TestFlow.LobbyRecovery(testMode.CallbackAllowed(generation,testUnloaded),testMode.Phase==TestModeState.Stage.Lobby,TestScope(),TestParticipant(current)||TestBot(current),current.PawnIsAlive))return;
                        // Preserve controller-bound human teams, wallet and selection; no Enroll/rebuild here.
                        if(current.TeamNum is not (2 or 3))current.SwitchTeam(current.IsBot?CsTeam.CounterTerrorist:CsTeam.Terrorist);
                        current.Respawn();
                
}catch(Exception ex){FailTestNative(ex);}
});
                return HookResult.Continue;
            }
            if(testMode.NativeActive&&TestParticipant(e.Userid)){
                var p=e.Userid!;testFlow.Teams.Kill(Id(p),null);if(Location(p) is {} pos)testDeaths[Id(p)]=pos;testRoamed.Remove(Id(p));
            }return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{
            if(TestBot(e.Userid)){try{RestoreTestPlayer(e.Userid!);}finally{testFlow.Bots.Remove(BotId(e.Userid!));}}
            if(TestParticipant(e.Userid)){var id=Id(e.Userid!);try{RestoreTestPlayer(e.Userid!);}finally{testMode.Leave(id);testFlow.Leave(id);testControllers.Remove(id);testDeaths.Remove(id);testRoamed.Remove(id);}}
            return HookResult.Continue;
        },HookMode.Pre);
    }
    private void StartTestMode(){
        if(testUnloaded || spreadMapEnded || testPawns.Count!=0 || testObservers.Count!=0 || testCvars.Count!=0 || testWeapons.Count!=0 || TestServer&&spreadOriginals.Count!=0)
            throw new InvalidOperationException("Unloaded or pending native cleanup. Retry off; inspect saved baseline counters.");
        if(sessionLoaded || PendingSettlement || linking.Count!=0 || gameplayConfig.Enabled)
            throw new InvalidOperationException("Production config/session or pending bridge conflicts. Disable production gameplay first.");
        testMode.Start(TestServer&&TestScope());testFlow=new();testSpawns.Clear();testLastCount=-1;ReadTestSpawns();
        try {
            TestCvar("mp_autoteambalance",false);TestCvar("mp_limitteams",0);
            TestCvar("mp_ignore_round_win_conditions",true);
            TestCvar("mp_maxrounds",0);TestCvar("mp_endmatch_votenextmap",false);
            // Installed engine may omit this legacy cvar. Absence is optional, a type mismatch is NOT.
            if(ConVar<bool>.Find("mp_do_warmup_period")!=null)TestCvar("mp_do_warmup_period",false);
            else Server.PrintToConsole("TEST optional mp_do_warmup_period absent; using mp_warmup_end only.");
            TestCvar("mp_freezetime",0);
            TestCvar("mp_roundtime",30f);TestCvar("mp_roundtime_defuse",30f);TestCvar("mp_roundtime_hostage",30f);
            TestCvar("mp_teammates_are_enemies",true);TestCvar("mp_friendlyfire",true);
            TestCvar("ff_damage_reduction_bullets",1f);TestCvar("ff_damage_reduction_other",1f);
            TestCvar("ff_damage_reduction_grenade",1f);
            TestCvar("mp_playercashawards",false);TestCvar("mp_teamcashawards",false);
            TestCvar("mp_respawn_on_death_t",false);TestCvar("mp_respawn_on_death_ct",false);
            PatchExistingSpread();
            QueueTestEnrollment();
            // Irreversible engine transition: cvar baselines restore, elapsed warmup does not.
            // Constant server command, ONLY after dedicated/private scope and typed capture succeed.
            Server.ExecuteCommand("mp_warmup_end");
        }catch{StopTestMode();throw;}
    }
    private string TestStatus()=>$"TEST {(testMode.Enabled?"ON":"OFF")} | MONEY {(TestServer?"HARD OFF":"production gates")} | phase {testMode.Phase} | {testMode.Participants.Count} opted-in | 200HP saved-pawns={testPawns.Count} / spread 0.5 baselines={spreadOriginals.Count} shared SERVER-WIDE / CHASE living duo else ROAMING death+64 observers={testObservers.Count} | solo-enemy bots={testFlow.Bots.Count}/{TestFlow.BotCapacity} (no wallet/selection) | KIT STUB labels only, NO DeltaOps | custom duo immunity / same-side FFA (native QA pending) | native-map/captured insertion, finite TEST credits only; no loot/SSG/session | flow={(testFlow.Manual?"MANUAL QA":"AUTO")} / spawns={testSpawns.Count} / ledger={testFlow.Ledger.Count} pending-cvars={testCvars.Count} | generation {testMode.Generation}";
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
        foreach(var p in Utilities.GetPlayers().Where(p=>TestParticipant(p)||TestBot(p))) {
            ApplyTestPresentation(p);
            if(testMode.Phase==TestModeState.Stage.Freeze)TestHealth(p);
        }
    }
    private void TestTick(){
        if(!testMode.Enabled)return;
        if(!TestScope() || sessionLoaded){StopTestMode();return;}
        if(testMode.Phase==TestModeState.Stage.Closed)return;
        foreach(var id in testMode.Participants.Keys.ToArray()){
            var p=Find(id);if(!TestParticipant(p)){testMode.Leave(id);testFlow.Leave(id);testControllers.Remove(id);continue;}
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
    private void QueueTestEnrollment(){
        var generation=testMode.Generation;
        Server.NextFrame(()=>{
try{
            if(!testMode.CallbackAllowed(generation,testUnloaded))return;
            AutoEnrollTestHumans();AutoEnrollTestBots();
        
}catch(Exception ex){FailTestNative(ex);}
});
    }
    private void AutoEnrollTestHumans(){
        if(!testMode.AutoEnrollAllowed(TestServer,TestScope(),true,testUnloaded))return;
        foreach(var p in Utilities.GetPlayers().Where(p=>Human(p)&&p.Connected==PlayerConnectedState.Connected)){
            if(TestParticipant(p))continue;
            try{
                if(!testFlow.Enroll(testMode,Id(p),Human(p))){
                    // Extras remain entirely outside participants/wallet/teams; no per-second chat spam.
                    continue;
                }
                testControllers[Id(p)]=p.EntityHandle.Raw;
                if(!p.PawnIsAlive){
                    if(p.TeamNum is not (2 or 3))p.SwitchTeam(CsTeam.Terrorist);
                    p.Respawn(); // spawn callback is generation/connection fenced
                }else PrepareTestLobby(p);
            }catch(Exception ex){
                // A native setup failure must not silently leave an unprepared participant.
                try{Server.PrintToConsole("TEST automatic enrollment failed: "+ex.Message);}
                finally{StopTestMode();}
                return;
            }
        }
    }
    private bool TestPlayerCommand(CCSPlayerController? p,CommandInfo c){
        if(!testMode.NativeActive||!TestParticipant(p)){c.ReplyToCommand("TEST only: ask console owner to enable and enroll you with css_raid_test join <userid>.");return false;}
        if(!TestScope()){StopTestMode();return false;}return true;
    }
    private void TestAnnounce(string message){
        foreach(var p in Utilities.GetPlayers().Where(TestParticipant))p.PrintToChat("TEST MONEY HARD OFF | "+message);
    }
    private void PrepareTestLobby(CCSPlayerController p){
        TestHealth(p);ApplyTestPresentation(p);
        // Explicit enrollment/respawn resets inventory for a bounded fixture loadout.
        // Inventory is not an economic snapshot and is never restored/refunded on OFF.
        p.RemoveWeapons();
        GrantTestGun(p,"glock");
        p.PrintToChat("TEST lobby: shoot each other to duo; !team single; !cancelteam. Then !characterselect 1..6. !testshop lists finite-credit guns. NO DeltaOps abilities / SSG.");
    }
    private bool GrantTestGun(CCSPlayerController p,string name){
        var gun=p.GiveNamedItem<CCSWeaponBase>("weapon_"+name);
        if(gun is not {IsValid:true})return false;
        testWeapons.Add(gun.EntityHandle.Raw);PatchSpread(gun);return true;
    }
    private void TestPurchase(CCSPlayerController p,string name){
        // Native buy INPUT, explicit custom native grant. No engine purchase event/debit is trusted.
        if(!TestParticipant(p)||!TestScope()||!testMode.NativeActive||testFlow.Manual || testMode.Phase!=TestModeState.Stage.Buy||!p.PawnIsAlive ||
            !testFlow.Spend(Id(p),name)){
            p.PrintToChat("TEST purchase rejected. Buy phase only, strict gun names, finite credits; !testshop.");return;
        }
        bool delivered=false;
        try{delivered=GrantTestGun(p,name);}
        catch(Exception ex){Server.PrintToConsole("TEST grant failed/unknown: "+ex.Message);}
        finally{testFlow.Delivery(Id(p),name,delivered);}
        p.PrintToChat($"TEST {name}: {(delivered?"native grant OK":"failed/unknown; no automatic refund")}; credits {testFlow.Wallet[Id(p)]}. NO SSG.");
    }
    private void ReadTestSpawns(){
        foreach(var name in new[]{"info_player_terrorist","info_player_counterterrorist"})
            foreach(var entity in Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name)){
                if(!entity.IsValid || entity.AbsOrigin is not {} v)continue;
                var point=new Position(v.X,v.Y,v.Z,entity.AbsRotation?.Y??0);
                if(point.Valid&&!testSpawns.Any(x=>x.Within(point,64)))testSpawns.Add(point);
            }
    }
    private void SyncTestTeams(){
        foreach(var id in testMode.Participants.Keys.ToArray()){
            testMode.Participants[id]=testFlow.Teams.Players.TryGetValue(id,out var member)&&member.Team is {} team
                ?testFlow.Teams.Players.Values.FirstOrDefault(x=>x.Id!=id&&x.Team==team)?.Id:null;
        }
        testMode.Characters.Clear();
        foreach(var member in testFlow.Teams.Players.Values)if(member.Character is {} character)testMode.Characters[member.Id]=character;
    }
    private HookResult TestDamage(CBaseEntity entity,CTakeDamageInfo info){
        if(!TestServer||!testMode.NativeActive)return HookResult.Continue;
        if(!TestScope()){StopTestMode();return HookResult.Continue;}
        var victim=TestController(entity);
        // Unlike production Controller(), resolve ALL controllers, including bots and projectile owners.
        CCSPlayerController? ResolveActor(CBaseEntity? source){
            var visited=new HashSet<uint>();
            for(int depth=0;depth<8&&source is {IsValid:true}&&visited.Add(source.EntityHandle.Raw);depth++){
                var found=Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&
                    (p.EntityHandle.Raw==source.EntityHandle.Raw||p.PlayerPawn.Raw==source.EntityHandle.Raw));
                if(found!=null)return found;
                source=source.OwnerEntity.Value;
            }return null;
        }
        var damageSource=info.Attacker.Value;var inflictor=info.Inflictor.Value;
        var attacker=ResolveActor(damageSource)??ResolveActor(inflictor);
        // Only explicit worldspawn (or an absent attacker AND inflictor) is actual world damage.
        // Unknown/map/projectile entities without resolvable player ownership fail closed.
        bool World(CBaseEntity? source)=>source==null||source is {IsValid:true,DesignerName:"worldspawn"};
        var actualWorld=attacker==null&&World(damageSource)&&World(inflictor);
        var enrolledVictim=TestParticipant(victim)||TestBot(victim);var enrolledAttacker=TestParticipant(attacker)||TestBot(attacker);
        if(!enrolledVictim&&!enrolledAttacker)return HookResult.Continue;
        // No cross-fixture damage (including bots/unenrolled humans). World damage only in Active.
        HookResult Decision(bool allow,string reason){
            if(testTraceRemaining>0){
                testTraceRemaining--;
                Server.PrintToConsole($"TEST DAMAGE {(allow?"ALLOW":"BLOCK")} reason={reason} phase={testMode.Phase} gen={testMode.Generation} victim={victim?.UserId} vbot={victim?.IsBot} venrolled={enrolledVictim} attacker={attacker?.UserId} abot={attacker?.IsBot} aenrolled={enrolledAttacker} world={actualWorld} preHP={victim?.PlayerPawn.Value?.Health} damage={info.Damage} remaining={testTraceRemaining}");
            }return allow?HookResult.Continue:HookResult.Handled;
        }
        if(!enrolledVictim || !enrolledAttacker&&!actualWorld)return Decision(false,"cross-fixture-or-unknown-owner");
        if(testMode.Phase==TestModeState.Stage.Lobby){
            // Explicit dedicated Lobby combat exception for enrolled bot enemies only.
            // Human-human bullets retain mutual pairing and zero damage, regardless native T/CT.
            if(TestFlow.LobbyBotCombat(testMode.Phase==TestModeState.Stage.Lobby,enrolledVictim,enrolledAttacker,
                TestBot(victim),TestBot(attacker),victim?.PawnIsAlive==true,attacker?.PawnIsAlive==true))
                return Decision(true,"lobby-enrolled-bot-combat");
            if(TestParticipant(attacker)&&TestParticipant(victim)&&info.Damage>0&&(info.BitsDamageType&DamageTypes_t.DMG_BULLET)!=0){
                if(testFlow.Teams.ShootRequest(Id(attacker!),Id(victim!))){
                    SyncTestTeams();TestAnnounce(SafeName(attacker!.PlayerName)+" and "+SafeName(victim!.PlayerName)+" duo ready.");
                }else attacker!.PrintToChat("TEST team request sent if both unpaired; the other player must shoot back.");
            }return Decision(false,"lobby-human-pairing-or-dead");
        }
        return Decision(TestFlow.DamageAllowed(testMode.Phase==TestModeState.Stage.Active,enrolledVictim,enrolledAttacker,actualWorld,
            TestBot(victim)?testFlow.BotLiving(BotId(victim!)):testFlow.Teams.Living(Id(victim!)),
            enrolledAttacker&&testFlow.EnemyDamage(TestBot(attacker)?BotId(attacker!):Id(attacker!),TestBot(victim)?BotId(victim!):Id(victim!)))
            , "active-enemy-or-phase-duo-living-policy");
    }
    private void TestSecond(){
        if(!testMode.NativeActive||testUnloaded)return;
        try{
            if(!TestScope()||sessionLoaded){StopTestMode();return;}
            if(testFlow.Manual)return;
            // Full-connect may precede Steam authorization; retry only in Lobby, no mid-match credit grants.
            AutoEnrollTestHumans();AutoEnrollTestBots();
            if(!testMode.NativeActive)return;
            var live=testMode.Participants.Keys.All(id=>Find(id) is {} p&&TestParticipant(p)&&p.PawnIsAlive);
            // Do not let readiness/selection clocks start while an enrolled player cannot spawn.
            var phase=testFlow.Teams.Phase;
            var ready=phase is GamePhase.Active or GamePhase.Closed ||
                live&&testSpawns.Count>=testMode.Participants.Count;
            var before=testFlow.Teams.Phase;
            var count=testFlow.Tick(DateTimeOffset.UtcNow,ready);
            SyncTestTeams();
            var after=testFlow.Teams.Phase;
            var stage=after==GamePhase.CharacterSelect?TestModeState.Stage.Selection:(TestModeState.Stage)(int)after;
            testMode.Sync(stage);
            if(before!=after){
                if(after==GamePhase.Freeze){
                    // Real current-map positions only, unique per participant. No production coordinates.
                    var members=testFlow.Teams.Players.Values.OrderBy(x=>x.Team).ThenBy(x=>x.Id).ToArray();
                    if(testSpawns.Count<members.Length)throw new InvalidOperationException("Insufficient real spawn positions.");
                    for(int index=0;index<members.Length;index++){
                        var p=Find(members[index].Id)??throw new InvalidOperationException("Insertion participant left.");
                        if(!TestParticipant(p)||!p.PawnIsAlive)throw new InvalidOperationException("Insertion participant not alive.");
                        Place(p,testSpawns[index]);TestHealth(p);
                    }
                }
                ApplyTestPhase();testLastCount=-1;
                TestAnnounce(after switch{
                    GamePhase.CharacterSelect=>"Selection: !characterselect 1..6 (state complete, KIT STUB only, no DeltaOps). !cancelteam allowed.",
                    GamePhase.Buy=>"Buy 30s: native console buy <gun> or !testbuy <gun>; !testshop. Free 15000 fixture credits; no SSG or projected cash.",
                    GamePhase.Countdown=>"Countdown 5s, then real-map insertion.",
                    GamePhase.Freeze=>"Inserted, frozen 10s at actual native/captured map positions.",
                    GamePhase.Active=>"ACTIVE: 200HP, half spread; custom duo immunity / native same-side FFA. No loot/extraction/economy.",
                    GamePhase.Closed=>"CLOSED, native baselines restored; console off/on for next fixture.",
                    _=>"Lobby: mutual bullets to duo or !team single."
                });
            }
            if(count is {} n&&n!=testLastCount){testLastCount=n;TestAnnounce(after+" "+n+"s");}
            if(!ready&&(after is GamePhase.Lobby or GamePhase.CharacterSelect) && DateTimeOffset.UtcNow>=testNextWaiting){
                testNextWaiting=DateTimeOffset.UtcNow.AddSeconds(10);
                TestAnnounce($"Waiting for alive enrolled players / enough real map spawn positions ({testSpawns.Count}/{testMode.Participants.Count}). Console capture <userid> adds an actual current position.");
            }
        }catch(Exception ex){
            try{TestAnnounce("Flow failed closed: "+ex.Message);}finally{StopTestMode();}
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
            ()=>{foreach(var raw in testWeapons.ToArray())try{if(new CHandle<CCSWeaponBase>(raw).Value is {IsValid:true} gun)gun.Remove();testWeapons.Remove(raw);}catch(Exception ex){try{TestCleanupError(ex);}catch{}}},
            ()=>{if(TestServer)TestModeState.RestoreEntries(spreadOriginals,(_,baseline)=>{
                var modes=SpreadModes(baseline.Data);
                for(var i=0;i<modes.Length;i++)baseline.Modes[i].AsSpan().CopyTo(modes[i].Values);
                baseline.Data.InaccuracyJumpInitial=baseline.JumpInitial;
                baseline.Data.InaccuracyJumpApex=baseline.JumpApex;
                baseline.Data.InaccuracyReload=baseline.Reload;
            },TestCleanupError);},
            ()=>TestModeState.RestoreEntries(testCvars,(_,restore)=>restore(),TestCleanupError));
    }
    private void FailTestNative(Exception ex){
        // Fence/cleanup first even if console logging itself fails.
        try{StopTestMode();}
        finally{try{Server.PrintToConsole("TEST native setup failed closed OFF: "+ex.Message);}catch{}}
    }
    private void StopTestMode(){
        testTraceRemaining=0;
        testMode.Stop(); // Fence all callbacks before touching native resources.
        try{CleanupTestNative();}
        finally{testFlow.Bots.Clear();testControllers.Clear();testDeaths.Clear();testRoamed.Clear();testSpawns.Clear();}
    }
}
