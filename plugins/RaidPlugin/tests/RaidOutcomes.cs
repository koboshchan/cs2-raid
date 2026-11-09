using Cs2Raid;
public static class RaidOutcomeTests
{
    public static void Run() {
        int checks=0;
        void Check(bool ok){checks++;if(!ok)throw new Exception("outcome assertion "+checks);}
        var now=DateTimeOffset.UnixEpoch;
        GameplayState Active(){
            var g=new GameplayState();foreach(var id in new[]{"a","b","c"})g.Join(id);
            g.ShootRequest("a","b");g.ShootRequest("b","a");g.Solo("c");g.Tick(now,true);
            foreach(var id in g.Players.Keys)g.SelectCharacter(id,2);
            g.Tick(now,true);g.Tick(now.AddSeconds(45),true);g.Tick(now.AddSeconds(50),true);g.Tick(now.AddSeconds(60),true);return g;
        }
        var clock=now.AddMinutes(6);
        var state=Active();var outcomes=new RaidOutcomes();
        Check(state.Kill("a",null));Check(outcomes.Record("a",false,clock));
        Check(!outcomes.Record("a",true,clock));Check(!state.Living("a"));
        Check(!state.CanDamage("a","c"));Check(!state.PullLever("a",1,clock));
        Check(!state.TryPurchase("a",0));Check(!state.Kill("a",null));
        Check(outcomes.Waiting("a",clock.AddSeconds(5).AddTicks(-1)));
        Check(!outcomes.CanObserve("a",clock.AddSeconds(5).AddTicks(-1)));
        Check(outcomes.CanObserve("a",clock.AddSeconds(5)));
        Check(!outcomes.Waiting("a",clock.AddSeconds(5)));
        Check(outcomes.TakeCamera("a",clock.AddSeconds(5)));Check(!outcomes.TakeCamera("a",clock.AddSeconds(6)));
        Check(state.Spectate("a")=="b");state.Kill("b",null);Check(state.Spectate("a")==null);
        outcomes.Disconnect("a");Check(!outcomes.CanObserve("a",clock.AddSeconds(7)));
        Check(!outcomes.TakeCamera("a",clock.AddSeconds(7)));
        // New match owns a new coordinator; old identities never migrate.
        var next=new RaidOutcomes();Check(!next.CanObserve("a",clock.AddSeconds(10)));
        Check(next.Record("a",false,clock.AddSeconds(10)));Check(next.Waiting("a",clock.AddSeconds(14)));
        outcomes.Close(clock);Check(!outcomes.ReadyToFinish(clock.AddSeconds(5).AddTicks(-1)));
        Check(outcomes.Finish(state,clock.AddSeconds(4)).Length==0);
        var losers=outcomes.Finish(state,clock.AddSeconds(5));Check(losers.Length==2);
        Check(losers.All(x=>!x.Won&&x.GlobalMvp==null));Check(outcomes.Finish(state,clock.AddSeconds(6)).Length==0);
        Check(!outcomes.Record("c",true,clock.AddSeconds(6)));
        // Trusted extraction stores cash before zeroing, independently of native team.
        state=Active();outcomes=new();state.Players["a"].Cash=500;state.Players["c"].Cash=500;
        state.PullLever("a",1,clock);state.PullLever("a",2,clock);
        var intents=state.ExtractionCandidates(clock.AddMinutes(5),new[]{"c","a"});
        foreach(var intent in intents){Check(state.ConfirmVerifiedExtraction(intent));Check(outcomes.Record(intent.Player,true,clock));Check(!state.ConfirmVerifiedExtraction(intent));}
        Check(state.Players["c"].MoneyExtractedCents==50000&&state.Players["c"].Cash==0);
        Check(!state.Living("c"));Check(!state.Kill("c",null));Check(!state.CanDamage("c","b"));
        state.Kill("b",null);outcomes.Record("b",false,clock.AddSeconds(2));outcomes.Close(clock);
        Check(!outcomes.ReadyToFinish(clock.AddSeconds(5)));Check(outcomes.ReadyToFinish(clock.AddSeconds(7)));
        var result=outcomes.Finish(state,clock.AddSeconds(7));
        Check(result.Length==3&&result.All(x=>x.GlobalMvp=="c"));
        Check(result.Single(x=>x.Recipient=="a").OwnRaidTeam.SequenceEqual(new[]{"a","b"}));
        Check(result.Single(x=>x.Recipient=="c").OwnRaidTeam.SequenceEqual(new[]{"c"}));
        Check(!result.Single(x=>x.Recipient=="b").Won);Check(result.Single(x=>x.Recipient=="a").Won);
        state=Active();state.Players["a"].Cash=499;state.Players["c"].Cash=500;
        state.PullLever("a",1,clock);state.PullLever("a",2,clock);
        foreach(var i in state.ExtractionCandidates(clock.AddMinutes(5),new[]{"a","c"}))Check(state.ConfirmVerifiedExtraction(i,i.Player=="a"?100000:50000));
        outcomes=new();outcomes.Close(clock);Check(outcomes.Finish(state,clock.AddSeconds(5)).All(x=>x.GlobalMvp=="a"));
        // Trusted FINAL money, not cash alone or native team, controls ranking.
        Check(state.Players["a"].MoneyExtractedCents==100000);
        state=Active();state.PullLever("a",1,clock);state.PullLever("a",2,clock);
        var rejected=state.ExtractionCandidates(clock.AddMinutes(5),new[]{"a"})[0];
        Check(!state.ConfirmVerifiedExtraction(rejected,-1));Check(state.Living("a"));
        state.Kill("a",null);Check(!state.ConfirmVerifiedExtraction(rejected,100));
        // Match-end overlap never moves an existing outcome deadline or duplicates it.
        outcomes=new();Check(outcomes.Record("a",false,clock));outcomes.Close(clock.AddSeconds(1));
        outcomes.Close(clock.AddSeconds(3));Check(outcomes.ClosedAt==clock.AddSeconds(1));
        Check(!outcomes.Record("a",false,clock.AddSeconds(4)));Check(outcomes.CanObserve("a",clock.AddSeconds(5)));
        Check(!outcomes.ReadyToFinish(clock.AddSeconds(6).AddTicks(-1)));Check(outcomes.ReadyToFinish(clock.AddSeconds(6)));
        var cleaned=new RaidOutcomes();Check(!cleaned.CanObserve("a",clock.AddSeconds(100)));Check(!cleaned.TakeCamera("a",clock.AddSeconds(100)));
        state=Active();state.Tick(now.AddSeconds(1860),true);Check(state.Phase==GamePhase.Closed);Check(state.Players.Values.All(p=>!state.Living(p.Id)));
        Console.WriteLine($"{checks} raid outcome checks passed");
    }
}
