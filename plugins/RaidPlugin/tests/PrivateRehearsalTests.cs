using Cs2Raid;
static class PrivateRehearsalTests {
public static void Run(){
int n=0;void C(bool ok){n++;if(!ok)throw new Exception("Private rehearsal check "+n);}
var now=DateTimeOffset.UnixEpoch;var g=new GameplayState();
C(!g.BeginPrivateRehearsalBuy(now));C(g.Join("a"));C(g.Join("b"));C(!g.BeginPrivateRehearsalBuy(now));
C(!g.ShootRequest("a","b"));C(g.ShootRequest("b","a"));C(g.SameTeam("a","b"));C(g.BeginPrivateRehearsalBuy(now));C(g.Phase==GamePhase.Buy);
C(g.BuyOpen(now));C(g.BuyOpen(now.AddSeconds(44.999)));C(!g.BuyOpen(now.AddSeconds(45)));C(!g.BeginPrivateRehearsalBuy(now));
g.Tick(now.AddSeconds(45),true);C(g.Phase==GamePhase.Countdown);g.Tick(now.AddSeconds(50),true);C(g.Phase==GamePhase.Freeze);g.Tick(now.AddSeconds(60),true);C(g.Phase==GamePhase.Active);
C(g.PullLever("a",1,now.AddSeconds(60)));C(g.ExtractionAt==null);C(!g.PullLever("b",1,now.AddSeconds(60)));C(g.PullLever("b",2,now.AddSeconds(60)));C(g.ExtractionAt==now.AddSeconds(360));
C(g.ExtractionCandidates(now.AddSeconds(359),new[]{"a"}).Length==0);
var intents=g.ExtractionCandidates(now.AddSeconds(360),new[]{"a"});C(intents.Length==1);C(g.CompletePrivateRehearsalExtraction(intents[0]));C(g.Players["a"].Extracted);C(g.Players["a"].MoneyExtractedCents==0);C(!g.CompletePrivateRehearsalExtraction(intents[0]));C(g.FinishExtractionWindow(now.AddSeconds(360)));C(!g.Players["b"].Extracted);
C(PrivateExtractionBounds.Contains(715.278076,-6013.118164));C(PrivateExtractionBounds.Contains(1484.720825,-5940.820312));
C(!PrivateExtractionBounds.Contains(715.278075,-6000));C(!PrivateExtractionBounds.Contains(1484.720826,-6000));C(!PrivateExtractionBounds.Contains(1000,-6013.118165));C(!PrivateExtractionBounds.Contains(1000,-5940.820311));C(!PrivateExtractionBounds.Contains(double.NaN,-6000));
C(PrivateExtractionBounds.Warning(330.5567015,-6049.26709));C(PrivateExtractionBounds.Warning(1869.4421995,-5904.671386));C(!PrivateExtractionBounds.Warning(330.5567,-6000));
Console.WriteLine($"{n} private rehearsal checks passed (model, not native QA)");
}}
