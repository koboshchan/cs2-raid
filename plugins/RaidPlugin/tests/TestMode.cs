using Cs2Raid;
static class TestModeTests
{
    public static void Run(){
        int checks=0;
        void Check(bool ok){checks++;if(!ok)throw new Exception("testmode "+checks);}
        void Reject(Action a){try{a();}catch(InvalidOperationException){checks++;return;}throw new Exception("expected rejection");}
        foreach(var bind in new[]{"0.0.0.0","::","207.81.35.60","8.8.8.8","","localhost"})
            Check(!TestModeState.Scope("1",true,27016,bind));
        foreach(var bind in new[]{"127.0.0.1","192.168.1.162","10.1.2.3","172.16.0.2"}){
            Check(TestModeState.Scope("1",true,27016,bind));
            Check(!TestModeState.Scope("0",true,27016,bind));
            Check(!TestModeState.Scope("1",false,27016,bind));
            Check(!TestModeState.Scope("1",true,27015,bind));
        }
        // Extracted policy used by the native hooks, not an engine simulation.
        var policy=new TestModeState();var queued=policy.Generation;
        Check(policy.CallbackAllowed(queued,false));Check(!policy.CallbackAllowed(queued,true));
        policy.Stop();Check(!policy.CallbackAllowed(queued,false));
        policy.Start(true);Check(policy.NativeActive);Check(policy.CanRespawn(true));Check(!policy.CanRespawn(false));
        policy.Join("native",true);policy.Advance();Check(!policy.CanRespawn(true));
        policy.Select("native",2);
        while(policy.Phase!=TestModeState.Stage.Closed)policy.Advance();
        Check(!policy.NativeActive);Check(!policy.CanRespawn(true));
        foreach(var fail in new[]{"pawn","observer","spread","cvar"}){
            var pending=new Dictionary<string,int>{{"pawn",1},{"observer",2},{"spread",3},{"cvar",4}};
            var restored=new List<string>();var errors=0;
            TestModeState.RestoreEntries(pending,(key,value)=>{if(key==fail)throw new Exception(key);restored.Add(key);},_=>errors++);
            Check(errors==1&&restored.Count==3&&pending.Count==1&&pending.ContainsKey(fail));
            TestModeState.RestoreEntries(pending,(_,_)=>{},_=>errors++);
            Check(pending.Count==0&&errors==1);
        }
        for(var failure=0;failure<6;failure++){
            int ran=0,errors=0;var actions=Enumerable.Range(0,6).Select(index=>(Action)(()=>{ran++;if(index==failure)throw new Exception("injected");})).ToArray();
            TestModeState.FinalizeAll(_=>errors++,actions);Check(ran==6&&errors==1);
        }
        var loggerPending=new Dictionary<string,int>{{"bad",1},{"good",2}};int loggerRan=0;
        TestModeState.RestoreEntries(loggerPending,(key,_)=>{loggerRan++;if(key=="bad")throw new Exception();},_=>throw new Exception("logger"));
        Check(loggerRan==2&&loggerPending.Count==1);
        int finalAfterLogger=0;TestModeState.FinalizeAll(_=>throw new Exception("logger"),()=>throw new Exception("cleanup"),()=>finalAfterLogger++);
        Check(finalAfterLogger==1);
        var expired=new Dictionary<nint,Action>{{(nint)123,()=>throw new Exception("expired native pointer dereferenced")}};
        Check(TestModeState.ExpireEntries(expired)==1&&expired.Count==0);
        TestModeState.RestoreEntries(expired,(_,touch)=>touch(),_=>throw new Exception());
        Check(TestModeState.ExpireEntries(expired)==0);
        var cvars=new Dictionary<string,Action>();bool boolValue=true;int intValue=2;
        TestModeState.SetCaptured(cvars,"bool",()=>boolValue,v=>boolValue=v,false);
        TestModeState.SetCaptured(cvars,"int",()=>intValue,v=>intValue=v,0);
        Check(!boolValue&&intValue==0&&cvars.Count==2);
        TestModeState.SetCaptured(cvars,"bool",()=>boolValue,v=>boolValue=v,false);
        TestModeState.RestoreEntries(cvars,(_,restore)=>restore(),_=>throw new Exception());
        Check(boolValue&&intValue==2&&cvars.Count==0);
        bool failWrite=true;
        try{TestModeState.SetCaptured(cvars,"partial",()=>boolValue,v=>{if(failWrite)throw new InvalidOperationException();boolValue=v;},false);}catch(InvalidOperationException){}
        Check(cvars.Count==1);
        failWrite=false;TestModeState.RestoreEntries(cvars,(_,restore)=>restore(),_=>throw new Exception());
        Check(boolValue&&cvars.Count==0);
        try{TestModeState.SetCaptured<bool>(cvars,"badread",()=>throw new InvalidOperationException(),_=>{},false);}catch(InvalidOperationException){}
        Check(cvars.Count==0);
        var state=new TestModeState();Check(!state.Enabled);
        Reject(()=>state.Start(false));Reject(()=>state.Join("a",true));
        state.Start(true);var gen=state.Generation;Reject(()=>state.Start(true));
        Reject(()=>state.Join("a",false));Reject(()=>state.Advance());
        state.Join("a",true);state.Join("b",true);state.Join("a",true);Check(state.Participants.Count==2);
        Reject(()=>state.Duo("a","a"));Reject(()=>state.Duo("a","unknown"));
        state.Duo("a","b");Check(state.Participants["a"]=="b"&&state.Participants["b"]=="a");
        Reject(()=>state.Duo("a","b"));Reject(()=>state.Select("a",2));
        state.Advance();Check(state.Phase==TestModeState.Stage.Selection);
        Reject(()=>state.Advance());Reject(()=>state.Select("a",0));Reject(()=>state.Select("missing",1));
        Reject(()=>state.Select("a",1));state.Select("a",2);state.Select("b",6);state.Advance();Check(state.Phase==TestModeState.Stage.Buy);
        state.Advance();Check(state.Phase==TestModeState.Stage.Countdown);
        state.Advance();Check(state.Phase==TestModeState.Stage.Freeze);
        state.Advance();Check(state.Phase==TestModeState.Stage.Active);
        state.Advance();Check(state.Phase==TestModeState.Stage.Closed);Reject(()=>state.Advance());
        state.Leave("a");Check(state.Participants["b"]==null);
        state.Stop();Check(!state.Enabled&&state.Generation>gen&&state.Participants.Count==0&&state.Characters.Count==0);
        for(int i=0;i<20;i++){state.Start(true);state.Join("new",true);state.Stop();Check(!state.Enabled&&state.Participants.Count==0);}
        // Unknown maps work because neither policy nor fixture accepts a map name/coordinates.
        foreach(var map in new[]{"de_dust2","unknown/custom-map","Ocean-future-map"}){
            state.Start(TestModeState.Scope("1",true,27016,"192.168.1.162"));Check(state.Enabled);state.Stop();
        }
        // Exact isolated production teaming/timer/damage helper used by native adapter.
        var full=new TestFlow();var now=DateTimeOffset.UnixEpoch;
        Check(full.Join("a")&&full.Join("b")&&full.Join("c"));
        Check(!full.Join("a")&&full.Wallet["a"]==15000);
        Check(!full.Teams.ShootRequest("a","b")&&!full.Teams.SameTeam("a","b"));
        Check(full.Teams.ShootRequest("b","a")&&full.Teams.SameTeam("a","b"));
        Check(full.Teams.Solo("c"));
        full.Tick(now,false);Check(full.Teams.Phase==GamePhase.Lobby);
        full.Tick(now,true);Check(full.Teams.Phase==GamePhase.CharacterSelect);
        Check(!full.Teams.SelectCharacter("a",0)&&!full.Teams.SelectCharacter("a",7));
        Check(full.Teams.SelectCharacter("a",2)&&full.Teams.SelectCharacter("b",6));
        Check(!full.Spend("a","glock")&&!full.Teams.CanDamage("a","c"));
        Check(full.Teams.Cancel("a").Length==2&&full.Teams.Phase==GamePhase.Lobby);
        Check(!full.Teams.SameTeam("a","b")&&full.Teams.Players["a"].Character==null);
        Check(full.Teams.Solo("a")&&full.Teams.Solo("b"));full.Tick(now,true);
        foreach(var id in new[]{"a","b","c"})Check(full.Teams.SelectCharacter(id,3));
        full.Tick(now,true);Check(full.Teams.Phase==GamePhase.Buy);
        foreach(var invalid in new[]{"","weapon_awp","AWP","awp;quit","hegrenade","c4","not-a-gun"})
            Check(!full.Spend("a",invalid)&&full.Wallet["a"]==15000);
        Check(full.Spend("a","awp")&&full.Wallet["a"]==10250);
        full.Delivery("a","awp",false);Check(full.Wallet["a"]==10250&&full.Ledger.Last().Contains("NO_REFUND"));
        Check(full.Spend("a","awp")&&full.Spend("a","awp")&&!full.Spend("a","awp")&&full.Wallet["a"]==750);
        Check(full.Teams.Cancel("a").Length==0);
        full.Tick(now.AddSeconds(44),true);Check(full.Teams.Phase==GamePhase.Buy);
        full.Tick(now.AddSeconds(45),true);Check(full.Teams.Phase==GamePhase.Countdown&&!full.Spend("b","glock"));
        full.Tick(now.AddSeconds(49),true);Check(full.Teams.Phase==GamePhase.Countdown);
        full.Tick(now.AddSeconds(50),true);Check(full.Teams.Phase==GamePhase.Freeze);
        full.Tick(now.AddSeconds(59),true);Check(full.Teams.Phase==GamePhase.Freeze);
        full.Tick(now.AddSeconds(60),true);Check(full.Teams.Phase==GamePhase.Active&&full.Teams.CanDamage("a","b"));
        full.Teams.Kill("a",null);Check(!full.Teams.CanDamage("a","b"));
        full.Leave("b");Check(!full.Wallet.ContainsKey("b")&&!full.Teams.Living("b"));
        full.Teams.Kill("c",null);full.Tick(now.AddSeconds(61),true);Check(full.Teams.Phase==GamePhase.Closed);
        Check(!full.Join("late")&&!full.Spend("c","glock"));
        var duoFlow=new TestFlow();duoFlow.Join("x");duoFlow.Join("y");duoFlow.Join("z");
        duoFlow.Teams.ShootRequest("x","y");Check(duoFlow.Teams.ShootRequest("y","x"));
        duoFlow.Teams.Solo("z");duoFlow.Tick(now,true);
        foreach(var id in new[]{"x","y","z"})duoFlow.Teams.SelectCharacter(id,2);
        duoFlow.Tick(now,true);duoFlow.Tick(now.AddSeconds(45),true);duoFlow.Tick(now.AddSeconds(50),true);duoFlow.Tick(now.AddSeconds(60),true);
        Check(!duoFlow.Teams.CanDamage("x","y")&&duoFlow.Teams.CanDamage("x","z")&&duoFlow.Teams.Spectate("x")=="y");
        duoFlow.Teams.Kill("y",null);Check(duoFlow.Teams.Spectate("x")==null);
        duoFlow.Tick(now.AddMinutes(31),true);Check(duoFlow.Teams.Phase==GamePhase.Closed);
        var capped=new TestFlow();var enrollment=new TestModeState();enrollment.Start(true);
        for(int i=0;i<7;i++)Check(capped.Enroll(enrollment,"cap"+i,true));
        Check(!capped.Enroll(enrollment,"eighth",true));
        Check(capped.Teams.Players.Count==7&&capped.Wallet.Count==7&&enrollment.Participants.Count==7);
        Check(!capped.Teams.Players.ContainsKey("eighth")&&!capped.Wallet.ContainsKey("eighth")&&!enrollment.Participants.ContainsKey("eighth"));
        Check(capped.Enroll(enrollment,"cap0",true)&&capped.Wallet["cap0"]==15000);
        Check(!capped.Enroll(enrollment,"unauth",false)&&!capped.Enroll(enrollment,"",true));
        capped.Leave("cap0");enrollment.Leave("cap0");
        Check(capped.Enroll(enrollment,"replacement",true)&&capped.Wallet.Count==7&&enrollment.Participants.Count==7);
        enrollment.Advance();Check(!capped.Enroll(enrollment,"late",true));
        // Source classification is native adapter responsibility; exact final policy exercised here.
        Check(!TestFlow.DamageAllowed(true,true,false,false,true,false)); // bot or unenrolled human/projectile
        Check(TestFlow.DamageAllowed(true,true,false,true,true,false)); // actual world in Active
        Check(!TestFlow.DamageAllowed(false,true,false,true,true,false)); // world in Lobby
        Check(!TestFlow.DamageAllowed(true,true,true,false,true,false)); // custom partner
        Check(TestFlow.DamageAllowed(true,true,true,false,true,true)); // enrolled enemy
        Check(!TestFlow.DamageAllowed(true,false,true,false,true,true)); // cross-fixture victim
        Check(!TestFlow.DamageAllowed(true,true,false,true,false,false)); // dead victim
        var lifecycle=new TestModeState();
        Check(!lifecycle.RebuildRound(true,true,false));
        Check(!lifecycle.AutoEnrollAllowed(true,true,true,false));
        lifecycle.Start(true);
        Check(lifecycle.RebuildRound(true,true,false));
        Check(!lifecycle.RebuildRound(false,true,false)&&!lifecycle.RebuildRound(true,false,false)&&!lifecycle.RebuildRound(true,true,true));
        Check(lifecycle.AutoEnrollAllowed(true,true,true,false));
        Check(!lifecycle.AutoEnrollAllowed(true,true,false,false)&&!lifecycle.AutoEnrollAllowed(false,true,true,false));
        Check(!lifecycle.AutoEnrollAllowed(true,false,true,false)&&!lifecycle.AutoEnrollAllowed(true,true,true,true));
        for(int round=0;round<5;round++){
            var oldGeneration=lifecycle.Generation;var roundFlow=new TestFlow();
            Check(roundFlow.Enroll(lifecycle,"human",true));
            lifecycle.Advance();
            Check(lifecycle.RebuildRound(true,true,false)&&!lifecycle.AutoEnrollAllowed(true,true,true,false));
            lifecycle.Stop();Check(!lifecycle.CallbackAllowed(oldGeneration,false));
            lifecycle.Start(true);var rebuilt=new TestFlow();
            Check(rebuilt.Enroll(lifecycle,"human",true)&&rebuilt.Wallet["human"]==15000);
            Check(lifecycle.Enabled&&lifecycle.Phase==TestModeState.Stage.Lobby);
            lifecycle.Stop();lifecycle.Start(true);
        }
        lifecycle.Stop();Check(!lifecycle.RebuildRound(true,true,false)); // explicit OFF/map end never auto-ON
        var botflow=new TestFlow();var botmode=new TestModeState();botmode.Start(true);
        Check(!botflow.EnrollBot("bot:1",false,true)&&!botflow.EnrollBot("bot:1",true,false));
        Check(!botflow.EnrollBot("human",true,true));
        for(int i=0;i<32;i++)Check(botflow.EnrollBot("bot:"+i,true,true));
        Check(!botflow.EnrollBot("bot:33",true,true)&&!botflow.EnrollBot("bot:1",true,true));
        Check(botflow.Wallet.Count==0&&botflow.Teams.Players.Count==0);
        Check(botflow.Enroll(botmode,"h1",true)&&botflow.Enroll(botmode,"h2",true)&&botflow.Enroll(botmode,"h3",true));
        botflow.Teams.ShootRequest("h1","h2");botflow.Teams.ShootRequest("h2","h1");botflow.Teams.Solo("h3");
        var bt=DateTimeOffset.UtcNow;botflow.Tick(bt,true);
        Check(botflow.Teams.Phase==GamePhase.CharacterSelect);
        foreach(var h in new[]{"h1","h2","h3"})botflow.Teams.SelectCharacter(h,2);
        botflow.Tick(bt,true);Check(botflow.Teams.Phase==GamePhase.Buy);
        Check(!botflow.Spend("bot:1","glock"));
        botflow.Tick(bt.AddSeconds(46),true);botflow.Tick(bt.AddSeconds(52),true);botflow.Tick(bt.AddSeconds(63),true);
        Check(botflow.Teams.Phase==GamePhase.Active);
        Check(botflow.EnemyDamage("h1","bot:1")&&botflow.EnemyDamage("bot:1","h1"));
        Check(!botflow.EnemyDamage("h1","h2")&&!botflow.EnemyDamage("h2","h1"));
        Check(botflow.EnemyDamage("h1","h3")&&botflow.EnemyDamage("bot:1","bot:2"));
        botflow.KillBot("bot:1");Check(!botflow.BotLiving("bot:1")&&!botflow.EnemyDamage("bot:1","h1")&&!botflow.EnemyDamage("h1","bot:1"));
        Check(botflow.Wallet.Count==3&&botflow.Teams.Players.Count==3&&botmode.Participants.Count==3);
        var resetbots=new TestFlow();Check(resetbots.EnrollBot("bot:1",true,true)&&resetbots.BotLiving("bot:1"));
        var oldServer=Environment.GetEnvironmentVariable("RAID_TEST_SERVER");
        var oldMoney=Environment.GetEnvironmentVariable("RAID_FINANCIAL_ENABLED");
        var oldScaffold=Environment.GetEnvironmentVariable("RAID_ENABLE_SCAFFOLD");
        try{
            Environment.SetEnvironmentVariable("RAID_TEST_SERVER","1");
            Environment.SetEnvironmentVariable("RAID_FINANCIAL_ENABLED","1");
            Environment.SetEnvironmentVariable("RAID_ENABLE_SCAFFOLD","1");
            var inner=new CountingTransport();
            using var client=new HttpClient(new MoneyOffHandler(inner));
            foreach(var route in new[]{"/link","/link/poll","/cashin","/extract","/native/next","/native/reply","/future-money"}){
                Reject(()=>client.PostAsync("http://backend:3000"+route,new StringContent("{}")).GetAwaiter().GetResult());
                Check(inner.Calls==0);
            }
            state.Start(true);state.Stop(); // OFF remains hard money OFF on dedicated server.
            Reject(()=>client.GetAsync("http://backend:3000/link").GetAwaiter().GetResult());Check(inner.Calls==0);
        }finally{
            Environment.SetEnvironmentVariable("RAID_TEST_SERVER",oldServer);
            Environment.SetEnvironmentVariable("RAID_FINANCIAL_ENABLED",oldMoney);
            Environment.SetEnvironmentVariable("RAID_ENABLE_SCAFFOLD",oldScaffold);
        }
        Console.WriteLine($"{checks} isolated testmode policy checks passed");
    }
}

sealed class CountingTransport : HttpMessageHandler {
    public int Calls;
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage r,CancellationToken c){
        Calls++;return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
    }
}
