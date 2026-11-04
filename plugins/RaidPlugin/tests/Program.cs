using Cs2Raid;
var now = DateTimeOffset.UnixEpoch;
int checks = 0;
void Check(bool value) { checks++; if (!value) throw new Exception($"Assertion {checks}"); }
void Reject(Action action) { try { action(); } catch (InvalidOperationException) { checks++; return; } throw new Exception("Expected rejection"); }
RaidState Make() { var r = new RaidState(); r.Join("a"); r.Join("b"); return r; }
var r = Make();
for (int i = 0; i < 5; i++) r.Join("x" + i);
Check(r.Players.Count == 7); Reject(() => r.Join("overflow"));
r.RecordPurchasedGun("b", new("ak","weapon_ak47",3100,"rifle"));
r.Start(now); Reject(() => r.Join("late")); Reject(() => r.RecordPurchasedGun("a",new("ak","weapon_ak47",3100,"rifle")));
r.PullLever(1, now); r.PullLever(1, now.AddSeconds(10)); Check(r.ExtractionAt == null);
r.PullLever(2, now.AddSeconds(20)); Check(r.ExtractionAt == now.AddSeconds(320));
r.PullLever(2, now.AddSeconds(30)); Check(r.ExtractionAt == now.AddSeconds(320));
Reject(() => r.Extract("a",now.AddSeconds(319)));
r.Die("b"); Check(r.Players["b"].Backpack.Count == 0); r.Loot("a","b"); Reject(() => r.Loot("a","b"));
var extraction=r.Extract("a",now.AddSeconds(320)); Check(extraction.SellbackCents == 279000); Check(extraction.Sold.Count==1); Check(r.Players["a"].Backpack.Count==0); Reject(()=>r.Extract("a",now.AddSeconds(321)));
var timeout=Make();timeout.Start(now);timeout.Tick(now.AddSeconds(1800));Check(timeout.Phase==RaidPhase.Closed);Check(!timeout.Players["a"].Alive);Reject(()=>timeout.PullLever(1,now.AddSeconds(1800)));
var large=Make();for(int i=0;i<1000;i++)large.RecordPurchasedGun("b",new("glock","weapon_glock",200,"pistol"));large.Start(now);large.Die("b");large.Loot("a","b");Check(large.Players["a"].Backpack.Count==1000);large.Die("a");Check(large.Players["a"].Backpack.Count==0);
Console.WriteLine($"{checks} raid rule checks passed");

FlowTests.Run();
