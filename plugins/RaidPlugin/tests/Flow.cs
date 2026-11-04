using Cs2Raid;
public static class FlowTests
{
    public static void Run()
    {
        var now=DateTimeOffset.UnixEpoch;int checks=0;
        void Check(bool ok) {if(!ok)throw new Exception("flow assertion "+checks);checks++;}
        void Reject(Action action){try{action();}catch(InvalidOperationException){checks++;return;}throw new Exception("expected config rejection");}
        GameplayState Lobby(params string[] ids){var s=new GameplayState();foreach(var id in ids)s.Join(id);return s;}
        GameplayState Ready(){
            var s=Lobby("a","b","c");s.ShootRequest("a","b");s.ShootRequest("b","a");s.Solo("c");return s;
        }
        GameplayState Buy(){var s=Ready();s.Tick(now,true);foreach(var id in s.Players.Keys)s.SelectCharacter(id,1);s.Tick(now,true);return s;}
        GameplayState Active(){var s=Buy();s.Tick(now.AddSeconds(45),true);s.Tick(now.AddSeconds(50),true);s.Tick(now.AddSeconds(60),true);return s;}
        var g=Lobby("a","b","c");
        Check(!g.ShootRequest("a","a"));Check(!g.ShootRequest("ghost","a"));Check(!g.ShootRequest("a","b"));Check(g.ShootRequest("b","a"));
        Check(g.SameTeam("a","b"));Check(!g.ShootRequest("a","c"));Check(!g.Solo("a"));Check(g.Solo("c"));
        Check(g.CanSeePartner("a","b"));Check(!g.CanSeePartner("a","a"));Check(!g.CanSeePartner("c","a"));
        g.Tick(now,false);Check(g.Phase==GamePhase.Lobby);
        g.Tick(now,true);Check(g.Phase==GamePhase.CharacterSelect);Check(g.StartsAt==null);
        Check(!g.SelectCharacter("a",0));Check(!g.SelectCharacter("a",7));Check(!g.SelectCharacter("ghost",1));
        Check(g.SelectCharacter("a",6));Check(g.Players["a"].Character==6);g.Tick(now.AddMinutes(1),true);Check(g.Phase==GamePhase.CharacterSelect);
        Check(g.Cancel("a").Length==2);Check(g.Phase==GamePhase.Lobby);Check(g.Players["a"].Character==null);
        Check(!g.ShootRequest("b","a"));Check(g.ShootRequest("a","b"));g.Tick(now,true);
        Check(g.Join("d"));Check(g.Phase==GamePhase.Lobby);Check(g.Solo("d"));g.Tick(now,true);
        g.Leave("b");Check(g.Players["a"].Team==null);Check(!g.Players.ContainsKey("b"));
        // Buy is half-open [0,45), independent of one-second Tick jitter.
        var boundary=Buy();
        Check(GameplayState.BuySeconds==45);
        Check(!boundary.BuyOpen(now.AddTicks(-1)));
        Check(boundary.BuyOpen(now));
        Check(boundary.BuyOpen(now.AddSeconds(30)));
        Check(boundary.BuyOpen(now.AddSeconds(45).AddTicks(-1)));
        Check(!boundary.BuyOpen(now.AddSeconds(45)));
        Check(!boundary.BuyOpen(now.AddSeconds(46)));
        var end=boundary.StartsAt!.Value;
        var anchor=TonightRules.BuyRoundStart(100,now,end);
        Check(anchor==100);
        Check(TonightRules.BuyRoundStart(130,now.AddSeconds(30),end)==anchor);
        Check(TonightRules.BuyRoundStart(145,now.AddSeconds(45),end)==anchor);
        Reject(()=>TonightRules.BuyRoundStart(146,now.AddSeconds(46),end));
        boundary.Tick(now.AddSeconds(45),false); // lost readiness/disconnect cannot extend native Buy
        Check(boundary.Phase==GamePhase.Countdown&&!boundary.BuyOpen(now.AddSeconds(45)));
        var cutoff=boundary.StartsAt;
        boundary.Tick(now.AddSeconds(45),true);
        Check(boundary.StartsAt==cutoff&&boundary.Phase==GamePhase.Countdown);
        var resetBuy=new GameplayState();
        Check(!resetBuy.BuyOpen(now)&&resetBuy.MatchId!=boundary.MatchId);
        g=Buy();Check(g.Phase==GamePhase.Buy);Check(g.StartsAt==now.AddSeconds(45));Check(!g.Join("late"));Check(g.Cancel("a").Length==0);
        Check(!g.TryPurchase("a",1));Check(!g.TryPurchase("ghost",0));Check(!g.TryPurchase("a",-1));g.Players["a"].Cash=100;
        Check(g.TryPurchase("a",30));Check(g.Players["a"].Cash==70);Check(g.TryPurchase("a",70));Check(g.Players["a"].Cash==0);
        Check(!g.TryPurchase("a",1));Check(!Lobby("a").TryPurchase("a",0));Check(!g.CanDamage("a","c"));
        g.Players["a"].Cash=50;g.Tick(now.AddSeconds(44),true);Check(g.Phase==GamePhase.Buy);
        g.Tick(now.AddSeconds(45),true);Check(g.Phase==GamePhase.Countdown);Check(g.StartsAt==now.AddSeconds(50));Check(!g.TryPurchase("a",1));
        g.Tick(now.AddSeconds(49),true);Check(g.Phase==GamePhase.Countdown);
        g.Tick(now.AddSeconds(50),true);Check(g.Phase==GamePhase.Freeze);Check(g.StartsAt==now.AddSeconds(60));Check(!g.CanDamage("a","c"));
        g.Tick(now.AddSeconds(59),true);Check(g.Phase==GamePhase.Freeze);
        g.Tick(now.AddSeconds(60),true);Check(g.Phase==GamePhase.Active);Check(g.EndsAt==now.AddSeconds(1860));Check(g.Players["a"].Cash==50);
        Check(!g.CanDamage("a","b"));Check(g.CanDamage("a","c"));Check(!g.CanDamage("ghost","a"));Check(!g.TryPurchase("a",1));
        var origin=new Position(0,0,0);var rng=new Random(42);var t=now.AddSeconds(60);
        Check(g.AddLoot(new("item",origin,LootKind.Item,100,100)));Check(!g.AddLoot(new("item",origin,LootKind.Item,100,100)));
        Check(!g.UseLoot("a","item",new(1000,0,0),true,t,rng));Check(!g.UseLoot("a","item",origin,false,t,rng));
        Check(!g.UseLoot("ghost","item",origin,true,t,rng));Check(g.UseLoot("a","item",origin,true,t,rng));Check(g.Players["a"].Cash==150);
        Check(!g.UseLoot("b","item",origin,true,t,rng));Check(!g.AddLoot(new("item",origin,LootKind.Item,100,100)));
        Check(!g.AddLoot(new("bad",origin,LootKind.Item,1,2)));Check(!g.AddLoot(new("bad",origin,LootKind.Item,1,1,1)));
        Check(!g.AddLoot(new("bad",origin,LootKind.Safe,1,int.MaxValue,5)));Check(!g.AddLoot(new("bad",origin,LootKind.Safe,1,2,double.NaN)));
        Check(g.AddLoot(new("mini",origin,LootKind.Safe,300,3000,5)));Check(!g.UseLoot("c","mini",origin,true,t,rng));
        Check(!g.UseLoot("c","mini",origin,true,t.AddSeconds(4.99),rng));g.ReleaseUse("c");
        Check(!g.UseLoot("c","mini",origin,true,t.AddSeconds(5),rng));Check(!g.UseLoot("c","mini",new(100,0,0),true,t.AddSeconds(9),rng));
        Check(!g.UseLoot("c","mini",origin,true,t.AddSeconds(10),rng));Check(g.UseLoot("c","mini",origin,true,t.AddSeconds(15),rng));
        Check(g.Players["c"].Cash>=300 && g.Players["c"].Cash<=3000);
        Check(g.AddLoot(new("big",origin,LootKind.Safe,1000,5000,10)));Check(!g.UseLoot("a","big",origin,true,t,rng));
        Check(!g.UseLoot("b","big",origin,true,t,rng));Check(g.UseLoot("a","big",origin,true,t.AddSeconds(10),rng));Check(!g.UseLoot("b","big",origin,true,t.AddSeconds(10),rng));
        Check(g.DieWithBag("c",origin));Check(g.Players["c"].Cash==0);Check(g.Players["a"].Cash>=1150);Check(g.Ground.ContainsKey("bag:c"));
        var before=g.Players["a"].Cash;var bag=g.Ground["bag:c"].Min;
        Check(g.UseLoot("a","bag:c",origin,true,t,rng));Check(g.Players["a"].Cash==before+bag);Check(!g.UseLoot("b","bag:c",origin,true,t,rng));
        Check(!g.Kill("c","a"));Check(!g.UseLoot("c","big",origin,true,t,rng));Check(g.Kill("a",null));Check(g.Players["a"].Cash==0);
        Check(g.Spectate("a")=="b");Check(!g.CanSeePartner("a","b"));Check(g.Kill("b",null));Check(g.Spectate("a")==null);
        g.Tick(t.AddSeconds(1),true);Check(g.Phase==GamePhase.Closed);
        g=Active();g.Players["c"].Cash=1000;
        Check(!g.PullLever("a",0,t));Check(!g.PullLever("a",5,t));Check(!g.PullLever("ghost",1,t));
        Check(g.PullLever("a",1,t));Check(!g.PullLever("b",1,t.AddMinutes(3)));Check(g.ExtractionAt==null);
        Check(g.PullLever("b",4,t.AddMinutes(4)));Check(g.ExtractionAt==t.AddMinutes(9));
        Check(!g.PullLever("a",2,t.AddMinutes(5)));Check(g.ExtractionCandidates(t.AddMinutes(8),["c"]).Length==0);
        Check(g.ExtractionCandidates(t.AddMinutes(9),[]).Length==0);
        var candidates=g.ExtractionCandidates(t.AddMinutes(9),["ghost","c","c"]);Check(candidates.Length==1);
        Check(candidates[0].Cash==1000);Check(!new GameplayConfig().FinancialNativeVerified);
        Check(g.Players["c"].Cash==1000 && !g.Players["c"].Extracted);Check(!g.ConfirmVerifiedExtraction(candidates[0] with { Cash=2000 }));
        Check(g.ConfirmVerifiedExtraction(candidates[0]));Check(!g.ConfirmVerifiedExtraction(candidates[0]));
        Check(g.Players["c"].Extracted && g.Players["c"].Cash==0 && !g.Living("c"));Check(!g.CanDamage("c","a"));
        Check(!g.FinishExtractionWindow(t.AddMinutes(8)));Check(g.FinishExtractionWindow(t.AddMinutes(9)));Check(g.ExtractionAt==null);
        Check(g.PullLever("a",1,t.AddMinutes(10)));Check(g.PullLever("b",2,t.AddMinutes(10)));Check(g.ExtractionAt==t.AddMinutes(15));
        g.Players["a"].Cash=3000;g.Tick(t.AddMinutes(30),true);Check(g.Phase==GamePhase.Closed);Check(!g.Players["a"].Alive && g.Players["a"].Cash==0);
        Check(g.Players["c"].Extracted);Check(g.ExtractionCandidates(t.AddMinutes(31),["a"]).Length==0);
        // HUD is only a projection, not a ledger ceiling, including death bags.
        Check(TonightRules.SpawnHealth==200);
        Check(TonightRules.Hud(-1)==0);Check(TonightRules.Hud(0)==0);
        Check(TonightRules.Hud(65534)==65534);Check(TonightRules.Hud(65535)==65535);
        Check(TonightRules.Hud(65536)==65535);Check(TonightRules.Hud(long.MaxValue)==65535);
        foreach(var phase in Enum.GetValues<GamePhase>())foreach(var side in Enumerable.Range(0,5))
            Check(TonightRules.ChooseNativeTeam(phase,side)==(phase is GamePhase.Lobby or GamePhase.CharacterSelect && side is 2 or 3));
        foreach(var side in new[]{2,3})foreach(var win in new[]{true,false}) {
            var banner=TonightRules.Banner(side,win);
            Check(banner.PanelFinalEvent==banner.Reason);Check(banner.Winner==(win?side:5-side));Check(banner.Reason==(banner.Winner==2?9:8));
            Check(banner.Message==(banner.Winner==2?"#SFUI_Notice_Terrorists_Win":"#SFUI_Notice_CTs_Win"));
        }
        foreach(var baseline in new[]{0f,0.01f,0.8f,2f}) {
            var shared=TonightRules.Half(baseline);
            for(var spawn=0;spawn<100;spawn++)shared=TonightRules.Half(baseline);
            Check(shared==baseline*0.5f);
        }
        g=Active();g.Players["c"].Cash=(long)int.MaxValue+12345;
        Check(g.DieWithBag("c",origin));Check(g.Ground["bag:c"].Min==(long)int.MaxValue+12345);
        Check(g.UseLoot("a","bag:c",origin,true,t,rng));Check(g.Players["a"].Cash==(long)int.MaxValue+12345);
        Check(g.Players["c"].Cash==0);Check(!g.CanDamage("a","b"));Check(g.CanDamage("a","ghost")==false);
        g=Active();g.Players["c"].Cash=long.MaxValue;
        Check(g.DieWithBag("c",origin));Check(g.UseLoot("a","bag:c",origin,true,t,rng));
        Check(g.Players["a"].Cash==long.MaxValue);
        Check(g.AddLoot(new("overflow",origin,LootKind.Item,1,1)));
        Check(!g.UseLoot("a","overflow",origin,true,t,rng));Check(g.Players["a"].Cash==long.MaxValue);
        Check(g.Ground.ContainsKey("overflow")); // Failed overflow cannot consume the loot.
        var conf=new GameplayConfig();Check(!conf.Ready(0));conf.Enabled=true;conf.CoordinatesConfirmed=true;conf.Spawns=[new(1,2,3)];Check(!conf.Ready(1));
        conf.NativeFlowVerified=true;Check(conf.Ready(1));Check(!conf.Ready(2));
        conf.MapLoot=[new("safe",LootKind.Safe,double.NaN)];Reject(conf.Validate);
        conf.MapLoot=[new("item",LootKind.Item,0.5,100,HoldSeconds:1)];Reject(conf.Validate);
        conf.MapLoot=[new("safe",LootKind.Safe,1,Min:300,Max:3000,HoldSeconds:5),new("item",LootKind.Item,0.5,100)];conf.Validate();Check(true);
        conf.Levers=[new(1,"a"),new(1,"b")];Reject(conf.Validate);conf.Levers=[];conf.ExtractionCenter=new(float.NaN,0,0);Reject(conf.Validate);
        Check(!origin.Within(new(float.NaN,0,0),96));Check(!origin.Within(origin,double.NaN));Check(!origin.Within(origin,-1));
        foreach(var order in Enumerable.Range(0,100)){
            var sim=Lobby("a","b","c","d","e","f","g");var rand=new Random(order);var ids=sim.Players.Keys.ToArray();
            Check(!sim.Join("overflow"));
            for(int i=0;i<100;i++){
                var a=ids[rand.Next(ids.Length)];var b=ids[rand.Next(ids.Length)];
                if(rand.Next(4)==0)sim.Cancel(a);else sim.ShootRequest(a,b);
                Check(sim.Players.Values.Where(x=>x.Team!=null).GroupBy(x=>x.Team).All(x=>x.Count()<=2));
            }
        }
        var issued=Active();Check(!issued.ConfirmVerifiedExtraction(new(issued.MatchId+":a","a",0)));
        issued.PullLever("a",1,t);issued.PullLever("b",2,t);
        var oldIntent=issued.ExtractionCandidates(t.AddMinutes(5),["a"]).Single();
        issued.FinishExtractionWindow(t.AddMinutes(5));issued.PullLever("a",1,t.AddMinutes(6));issued.PullLever("b",2,t.AddMinutes(6));
        Check(!issued.ConfirmVerifiedExtraction(oldIntent));
        var timed=issued.ExtractionCandidates(t.AddMinutes(11),["a"]).Single();
        issued.Tick(t.AddMinutes(30),true);Check(!issued.ConfirmVerifiedExtraction(timed));
        Check(!Active().AddLoot(new("invalid",origin,(LootKind)99,1,1)));
        conf=new GameplayConfig {MapLoot=[new("invalid",(LootKind)99,1)]};Reject(conf.Validate);
        Check(!new GameplayConfig().CustomPricingEnabled);
        Check(!System.Text.Json.JsonSerializer.Deserialize<GameplayConfig>("{}")!.CustomPricingEnabled);
        Check(GameplayConfig.NativeSpend(1000,700)==300);Check(GameplayConfig.NativeSpend(700,700)==0);Check(GameplayConfig.NativeSpend(700,1000)==0);
        Check(GameplayConfig.NativeSpend(100,-1)==100);
        foreach(var utility in new[]{"flashbang","weapon_smokegrenade","hegrenade","molotov","incgrenade","decoy"})Check(GameplayConfig.Utility(utility));
        foreach(var allowed in new[]{"weapon_ak47","vest","vesthelm"})Check(!GameplayConfig.Utility(allowed));
        Console.WriteLine(checks+" flow v2 checks passed");
    }
}
