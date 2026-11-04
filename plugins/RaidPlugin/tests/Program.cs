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
int before=checks;
GameplayState Lobby(params string[] ids) {var g=new GameplayState();foreach(var id in ids)g.Join(id);return g;}
GameplayState Active() {var g=Lobby("a","b","c");g.ShootRequest("a","b");g.ShootRequest("b","a");g.Solo("c");g.Tick(now,true);g.Tick(now.AddSeconds(5),true);return g;}
var g=Lobby("a","b","c");
Check(!g.ShootRequest("a","a"));Check(!g.ShootRequest("ghost","a"));Check(!g.ShootRequest("a","b"));Check(g.ShootRequest("b","a"));
Check(g.SameTeam("a","b"));Check(!g.ShootRequest("a","c"));Check(!g.Solo("a"));Check(g.Solo("c"));
Check(g.CanSeePartner("a","b"));Check(!g.CanSeePartner("a","a"));Check(!g.CanSeePartner("c","a"));
Check(g.Tick(now,false)==null);Check(g.Phase==GamePhase.Lobby);Check(g.Tick(now,true)==5);Check(g.Tick(now.AddSeconds(1),true)==4);
Check(g.Cancel("a").Length==2);Check(g.Phase==GamePhase.Lobby);Check(g.Players["a"].Team==null&&g.Players["b"].Team==null);
Check(!g.ShootRequest("b","a"));Check(g.ShootRequest("a","b"));Check(g.Tick(now,true)==5);Check(g.Join("d"));Check(g.Phase==GamePhase.Lobby);Check(g.Solo("d"));Check(g.Tick(now,true)==5);
g.Leave("b");Check(g.Players["a"].Team==null);Check(!g.Players.ContainsKey("b"));Check(g.Solo("a"));Check(g.Tick(now,true)==5);Check(g.Tick(now.AddSeconds(4),true)==1);Check(g.Tick(now.AddSeconds(5),true)==0);Check(g.Phase==GamePhase.Active);Check(!g.Join("late"));Check(g.Cancel("a").Length==0);Check(!g.Solo("a"));
g=Active();Check(!g.CanDamage("a","b"));Check(g.CanDamage("a","c"));Check(!g.CanDamage("ghost","a"));
Check(g.Claim("safe","c",50,50,new Random(1)));Check(g.Players["c"].Cash==50);Check(!g.Claim("safe","a",50,50,new Random(1)));Check(g.Kill("c","a"));Check(g.Players["a"].Cash==50);Check(g.Players["c"].Cash==0);Check(!g.Kill("c","b"));Check(!g.Claim("other","c",1,1,new Random()));
Check(g.Kill("a",null));Check(g.Players["a"].Cash==0);Check(g.Spectate("a")=="b");Check(!g.CanSeePartner("a","b"));Check(g.Kill("b",null));Check(g.Spectate("a")==null);g.Tick(now.AddSeconds(6),true);Check(g.Phase==GamePhase.Closed);
g=Active();g.Drop(100,"a","weapon_ak47",3100);Check(g.Pickup(100,"c"));Check(g.Players["c"].Cash==3100);Check(!g.Pickup(100,"c"));g.Drop(100,"c","weapon_ak47",3100);Check(!g.Pickup(100,"a"));
g.Drop(101,"a","weapon_glock",200);Check(!g.Pickup(101,"b"));g.Drop(101,"b","weapon_glock",200);Check(!g.Pickup(101,"c"));
Check(!g.Claim("bad","a",20,10,new Random()));Check(!g.Claim("bad","a",0,int.MaxValue,new Random()));Check(!g.Claim("bad","a",-1,5,new Random()));Check(g.Claim("bad","a",0,0,new Random()));
g.Leave("a");Check(!g.Living("a"));Check(g.Spectate("a")=="b");g.Tick(now.AddMinutes(31),true);Check(g.Phase==GamePhase.Closed);
var conf=new GameplayConfig();Check(!conf.Ready(0));conf.Enabled=true;conf.CoordinatesConfirmed=true;conf.Spawns=[new(1,2,3)];Check(conf.Ready(1));Check(!conf.Ready(2));conf.LootSpawnChance=double.NaN;Reject(conf.Validate);conf.LootSpawnChance=1.1;Reject(conf.Validate);conf.LootSpawnChance=0.01;conf.Validate();Check(true);
// Exhaustive small roster request/cancel orders preserve disjoint teams of size <=2.
foreach(var order in Enumerable.Range(0,100)) {
 var sim=Lobby("a","b","c","d","e","f","g");var rng=new Random(order);var ids=sim.Players.Keys.ToArray();
 for(int i=0;i<100;i++){var a=ids[rng.Next(ids.Length)];var b=ids[rng.Next(ids.Length)];if(rng.Next(4)==0)sim.Cancel(a);else sim.ShootRequest(a,b);Check(sim.Players.Values.Where(x=>x.Team!=null).GroupBy(x=>x.Team).All(x=>x.Count()<=2));}
}
Console.WriteLine($"{checks-before} gameplay checks passed; {checks} total");

var shop=Active();Check(!shop.TryPurchase("a",1));Check(!shop.TryPurchase("ghost",0));Check(!shop.TryPurchase("a",-1));Check(shop.Claim("funds","a",100,100,new Random()));Check(shop.TryPurchase("a",30));Check(shop.Players["a"].Cash==70);Check(shop.TryPurchase("a",70));Check(shop.Players["a"].Cash==0);Check(!shop.TryPurchase("a",1));Check(shop.Kill("a",null));Check(!shop.TryPurchase("a",0));Check(!Lobby("a").TryPurchase("a",0));Console.WriteLine("12 purchase checks passed");
