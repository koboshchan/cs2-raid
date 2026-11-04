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
        var state=new TestModeState();Check(!state.Enabled);
        Reject(()=>state.Start(false));Reject(()=>state.Join("a",true));
        state.Start(true);var gen=state.Generation;Reject(()=>state.Start(true));
        Reject(()=>state.Join("a",false));Reject(()=>state.Advance());
        state.Join("a",true);state.Join("b",true);state.Join("a",true);Check(state.Participants.Count==2);
        Reject(()=>state.Duo("a","a"));Reject(()=>state.Duo("a","unknown"));
        state.Duo("a","b");Check(state.Participants["a"]=="b"&&state.Participants["b"]=="a");
        Reject(()=>state.Duo("a","b"));Reject(()=>state.Select("a",1));
        state.Advance();Check(state.Phase==TestModeState.Stage.Selection);
        Reject(()=>state.Advance());Reject(()=>state.Select("a",0));Reject(()=>state.Select("missing",1));
        state.Select("a",1);state.Select("b",6);state.Advance();Check(state.Phase==TestModeState.Stage.Buy);
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
