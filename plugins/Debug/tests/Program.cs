using System.Text.Json;
using Cs2Debug;

var checks = 0;
void Check(bool condition, string label)
{
    if (!condition) throw new InvalidOperationException(label);
    ++checks; Console.WriteLine("PASS " + label);
}
foreach (var team in new[] { 2, 3 })
{
    Check(RoundBanner.TryCreate(team, BannerResult.Win, out var win) && win.Winner == team, $"team {team} win uses own native team");
    Check(RoundBanner.TryCreate(team, BannerResult.Lose, out var lose) && lose.Winner == 5 - team, $"team {team} loss uses opposite native team");
    Check(RoundBanner.TryCreate(team, BannerResult.Draw, out var draw) && draw == new RoundBanner(0, 10, "#SFUI_Notice_Round_Draw"), $"team {team} draw has no winner");
    foreach (var result in new[] { BannerResult.Win, BannerResult.Lose, BannerResult.Draw })
    {
        RoundBanner.TryCreate(team, result, out var panelBanner);
        Check(panelBanner.PanelFinalEvent == panelBanner.Reason && panelBanner.PanelFinalEvent is 8 or 9 or 10, $"team {team} {result} panel has explicit end reason");
    }
    foreach (var result in new[] { BannerResult.Win, BannerResult.Lose })
    {
        RoundBanner.TryCreate(team, result, out var banner);
        Check(banner.Reason == (banner.Winner == 2 ? 9 : 8), $"team {team} {result} reason matches winner");
        Check(banner.Message == (banner.Winner == 2 ? "#SFUI_Notice_Terrorists_Win" : "#SFUI_Notice_CTs_Win"), $"team {team} {result} localization matches winner");
    }
}
foreach (var team in new[] { -1, 0, 1, 4, 255 })
    foreach (var result in new[] { BannerResult.Win, BannerResult.Lose, BannerResult.Draw })
        Check(!RoundBanner.TryCreate(team, result, out _), $"team {team} {result} rejected");

foreach (var team in new[] { 2, 3 })
foreach (var action in new[] { "win", "lose", "draw", "tie" })
{
    Check(BannerInput.TryParse(1, action, out var parsed), $"parse {action}");
    var expected = action == "lose" ? "red" : action is "draw" or "tie" ? "gray" : team == 2 ? "gold" : "blue";
    Check(BannerInput.NativeColor(team, parsed) == expected, $"native color {team} {action}");
}
Check(BannerInput.TryParse(1, "WIN", out var upper) && upper == BannerResult.Win, "case insensitive banner");
foreach (var argumentCount in new[] { -1, 0, 2, 3, 99 })
    Check(!BannerInput.TryParse(argumentCount, "win", out _), $"reject argument count {argumentCount}");
foreach (var action in new string?[] { null, "", " ", "win gold", "win;quit", "gold", "red", "<font>", "unknown", " win", "draw " })
    Check(!BannerInput.TryParse(1, action, out _), $"reject invalid action {action}");
Check(!RoundBanner.TryCreate(2, (BannerResult)99, out _), "undefined banner result rejected");
var a = new Point(1, 2, 3);
var b = new Point(4, 5, 6);
var tracker = new ShotTracker();
Check(tracker.Impact(10, a) == null, "impact without fire ignored");
tracker.Fire("ak47", 10);
Check(tracker.Impact(10,a) == null, "AK feature removed");
tracker.Fire("weapon_ak47", 10);
Check(tracker.Impact(10,a) == null, "prefixed AK feature removed");
tracker.Fire("glock", 50);
Check(tracker.Impact(50,a) is { Weapon: "glock", Tag: "texture" }, "Glock nocollide");
Check(tracker.Impact(50,b) == null, "penetration ignored");
tracker.Fire("weapon_glock",51);
Check(tracker.Impact(51,b) is { Weapon: "glock", Tag: "texture" }, "prefixed Glock");
tracker.Fire("glock",52); tracker.Fire("awp",52);
Check(tracker.Impact(52,a) == null, "other weapon clears");
tracker.Fire("glock",53);
Check(tracker.Impact(54,a) == null, "stale impact rejected");
tracker.Fire("glock",54);
Check(tracker.Impact(54,new(float.NaN,0,0)) == null, "invalid impact rejected");
Check(tracker.Impact(54,a) == null, "invalid impact consumed");
tracker.Fire("glock",55); tracker.Reset();
Check(tracker.Impact(55,a) == null, "reset clears ticket");
Check(ReportInput.TryReason("  missing ladder  ", out var reason) && reason == "missing ladder", "reason trim");
Check(!ReportInput.TryReason(" \t ", out _), "empty reason rejected");
Check(!ReportInput.TryReason("bad\nreason", out _), "control characters rejected");
Check(!ReportInput.TryReason(new string('a', 301), out _), "oversized reason rejected");
Check(ReportInput.TryReason(new string('a', 300), out _), "maximum reason accepted");
Check(ReportInput.TryReason("梯子没贴合", out _), "Unicode reason accepted");
Check(ReportInput.TryCount("", out var count) && count == 5, "listing default");
Check(ReportInput.TryCount("20", out count) && count == 20, "listing upper bound");
Check(!ReportInput.TryCount("21", out _), "listing too large rejected");
Check(!ReportInput.TryCount("-1", out _), "negative listing rejected");
Check(!ReportInput.TryCount("1 extra", out _), "extra listing args rejected");
Check(ReportInput.SafeText("a\x01b", 1) == "a", "chat text sanitized bounded");
var limiter = new ReportLimiter();
Check(limiter.Allow(1, 0, 3000), "first report rate accepted");
Check(!limiter.Allow(1, 2999, 3000), "rapid report rejected");
Check(limiter.Allow(2, 1, 3000), "rate per player");
Check(limiter.Allow(1, 3000, 3000), "rate boundary accepted");
limiter.Remove(1);
Check(limiter.Allow(1, 3001, 3000), "disconnect removes limiter state");
limiter.Clear();
Check(limiter.Allow(2, 2, 3000), "map reset clears limiter");

var setting = new MaintainedSetting<int>(16000);
Check(setting.Observe(16000), "economy initial override required");
setting.Applied(65535);
Check(!setting.Observe(65535) && setting.Baseline == 16000, "economy own override never replaces baseline");
Check(setting.Observe(0) && setting.Baseline == 0, "late map exec zero becomes restore baseline");
setting.Applied(65535);
Check(!setting.Observe(65535) && setting.Baseline == 0, "late config remains baseline after maintenance");
Check(setting.Observe(50000) && setting.Baseline == 50000, "later external max money preserved");
setting.Applied(60000);
Check(!setting.Observe(60000) && setting.Baseline == 50000, "engine clamp not mistaken for external change");
var buytime = new MaintainedSetting<float>(0f);
Check(buytime.Observe(0f), "zero buytime requires override");
buytime.Applied(1000000f);
Check(!buytime.Observe(1000000f) && buytime.Baseline == 0f, "buytime restores intended zero baseline");
tracker.Reset();

var root = Path.Combine(Path.GetTempPath(), "debug-test-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
MapReport Report(int n) => new(n.ToString("D8"), DateTimeOffset.UtcNow, "position", "test_map",
    "76561190000000000", "Tester", "wall offset \"quoted\" 梯子 " + n, a, b);
try
{
    var dir = Path.Combine(root, "durable");
    using (var store = new ReportStore(dir))
    {
        Check(store.Available, "directory created and writer opened");
        Check(store.Append(Report(1)), "append acknowledged after durable flush");
        var path = Path.Combine(dir, "reports.jsonl");
        Check(File.ReadAllLines(path).Length == 1, "flushed record visible while writer open");
        var doc = JsonDocument.Parse(File.ReadAllLines(path)[0]);
        Check(doc.RootElement.GetProperty("Reason").GetString()!.Contains("梯子"), "JSON quotes and Unicode roundtrip");
        Check(!File.ReadAllText(path).Contains("IpAddress"), "record contains no IP metadata");
        Check(!store.Append(Report(2) with { Position = new(float.PositiveInfinity, 0, 0) }), "nonfinite report rejected");
        Check(!store.Append(Report(2) with { Angles = new(0, float.NaN, 0) }), "nonfinite angles rejected");
        Check(!store.Append(Report(2) with { LadderStart = new(0, 0, float.NaN) }), "nonfinite ladder start rejected");
        Check(!store.Append(Report(2) with { Reason = "" }), "invalid report reason rejected");
        for (var i = 2; i <= 80; ++i) Check(store.Append(Report(i)), "durable append " + i);
        Check(store.Latest(999).Length == 20, "latest list bounded");
        Check(store.Latest(1)[0].Id == "00000080", "latest ordering");
    }
    using (var store = new ReportStore(dir))
    {
        Check(store.Latest(20).Length == 20 && store.Latest(1)[0].Id == "00000080", "latest survives reopen");
        Check(store.Append(Report(81)), "append after reopen");
    }
    File.AppendAllText(Path.Combine(dir, "reports.jsonl"), "{\"torn\":");
    using (var store = new ReportStore(dir))
    {
        Check(store.Available, "torn suffix recoverable");
        Check(store.Append(Report(82)), "new append after torn suffix");
    }
    Check(File.ReadAllLines(Path.Combine(dir, "reports.jsonl")).All(line =>
    { using var doc = JsonDocument.Parse(line); return true; }), "torn suffix repaired not concatenated");
    var onlyTorn = Path.Combine(root, "only-torn");
    Directory.CreateDirectory(onlyTorn);
    File.WriteAllText(Path.Combine(onlyTorn, "reports.jsonl"), "{");
    using (var store = new ReportStore(onlyTorn))
        Check(store.Append(Report(1)), "entirely torn first record repaired");
    var corrupt = Path.Combine(root, "corrupt");
    Directory.CreateDirectory(corrupt);
    File.WriteAllText(Path.Combine(corrupt, "reports.jsonl"), "{}\ninvalid-json\n");
    using (var store = new ReportStore(corrupt))
    {
        Check(store.Available && store.Latest(20).Length == 0, "malformed historical objects skipped safely");
        Check(!store.Append(Report(1) with { Id = null! }), "null report ID rejected");
        Check(!store.Append(Report(1) with { Map = null! }), "null map rejected");
        Check(store.Append(Report(1)), "corrupt historical rows do not block new valid reports");
    }
    if (OperatingSystem.IsLinux() && File.Exists("/dev/full"))
    {
        var full = Path.Combine(root, "full");
        Directory.CreateDirectory(full);
        File.CreateSymbolicLink(Path.Combine(full, "reports.jsonl"), "/dev/full");
        using var store = new ReportStore(full);
        Check(!store.Append(Report(1)) && !store.Available, "storage device failure never acknowledged and disables acceptance");
    }
    var fileNotDir = Path.Combine(root, "file");
    File.WriteAllText(fileNotDir, "x");
    using (var store = new ReportStore(fileNotDir))
    {
        Check(!store.Available && store.Failure != null, "directory failure disables acceptance");
        Check(!store.Append(Report(1)), "unavailable writer never acknowledges");
    }
    using (var store = new ReportStore("relative-path"))
        Check(!store.Available, "relative directory rejected");
    var closed = new ReportStore(Path.Combine(root, "closed"));
    closed.Dispose();
    Check(!closed.Append(Report(1)), "disposed store never acknowledges");
}
finally { Directory.Delete(root, true); }
Console.WriteLine($"ALL {checks} CHECKS PASSED");

var collisionDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
try {
    var m = new CollisionMark("a",DateTimeOffset.UtcNow,"nocollide","map","1",a,new(0,0,1),b,a);
    using(var cs = new CollisionStore(collisionDir)) {
        cs.Add(m);
        cs.Add(m with { Id = "b", SteamId = "2" });
        cs.Add(m with { Id = "c", Map = "other" });
        Check(cs.Undo("1","map")?.Id == "a", "undo scoped by player and map");
        Check(cs.Active.Count == 2, "undo removes only own mark");
        Check(cs.Undo("1","map") == null, "empty undo");
    }
    using(var cs = new CollisionStore(collisionDir)) {
        Check(cs.Active.Count == 2, "durable reload and tombstone");
        cs.Add(m with {Id="d"});
        Check(cs.Undo("1","map")?.Id == "d", "new latest after restart");
    }
} finally { Directory.Delete(collisionDir,true); }

var outline=ExtractBoxGeometry.Lines();
Check(outline.Length==32,"bounded 32 extraction beams");
Check(outline.Count(x=>x.Warning)==16,"16 red and 16 green beams");
Check(outline.All(x=>x.Start.IsFinite&&x.End.IsFinite),"finite outline geometry");
Check(outline.All(x=>x.Start.Z>=0&&x.End.Z<=300),"outline 0 through 300 without limiting extraction Z");
Check(outline.Count(x=>x.Start.Z!=x.End.Z)==8,"eight tall corner beams");
Check(outline.Any(x=>!x.Warning&&x.Start.X==(float)715.278076&&x.Start.Y==(float)-6013.118164),"green new corner");
Check(outline.Any(x=>x.Warning&&x.Start.X==(float)-406.461407382345&&x.Start.Y==(float)-6132.109927366835),"red new corner");

Check(outline.Any(x=>!x.Warning&&x.Start.X==(float)1039.138794&&x.Start.Y==(float)-6369.889648),"rotated second corner");
Check(outline.Any(x=>!x.Warning&&x.Start.X==(float)1095.046509&&x.Start.Y==(float)-5576.641113),"rotated fourth corner");
Check(outline.Where(x=>!x.Warning&&x.Start.Z==x.End.Z).All(x=>x.Start.X!=x.End.X&&x.Start.Y!=x.End.Y),"no axis-aligned green edge");
Console.WriteLine($"ALL {checks} checks passed");

foreach (var weapon in new[]{"m4a1","weapon_m4a1","m4a1_silencer","weapon_m4a1_silencer"}) {
 tracker.Fire(weapon,100); var ticket=tracker.Impact(100,a);
 Check(ticket is {Tag:"nocollide"} && ticket.Weapon==weapon.Replace("weapon_",""),"M4 fire normalization "+weapon);
 Check(tracker.Impact(100,b)==null,"M4 first impact only");
}
tracker.Fire("glock",110); // No active weapon queried at impact, so switching cannot reclassify.
Check(tracker.Impact(110,a) is {Weapon:"glock",Tag:"texture"},"weapon captured at fire across active weapon swap");
var typedDir=Path.Combine(Path.GetTempPath(),"debug-typed-"+Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(typedDir);
try {
 var legacy=new CollisionMark("legacy",DateTimeOffset.UtcNow,"nocollide","df_spaceport_v24","76561199818744151",a,new(0,0,1),b,a);
 var legacyLine=System.Text.Json.JsonSerializer.Serialize(new CollisionEntry("add",legacy,null,legacy.Timestamp))+"\n";
 File.WriteAllText(Path.Combine(typedDir,"nocollide.jsonl"),legacyLine);
 using(var cs=new CollisionStore(typedDir)) {
  Check(cs.Active.Single().Weapon==null && cs.Active.Single().Tag==null,"legacy stays untyped");
  cs.Add(legacy with {Id="typedG",Weapon="glock",Tag="texture"});
  cs.Add(legacy with {Id="typedM",Weapon="m4a1_silencer",Tag="nocollide"});
  Check(cs.Undo("other","df_spaceport_v24")==null,"undo ownership isolation");
  Check(cs.Undo(legacy.SteamId,legacy.Map)?.Id=="typedM","typed undo");
  Check(cs.Undo(legacy.SteamId,legacy.Map)?.Id=="typedG","Glock typed undo");
  Check(cs.Active.Single().Id=="legacy","legacy remains");
 }
 Check(File.ReadAllText(Path.Combine(typedDir,"nocollide.jsonl")).StartsWith(legacyLine),"old journal bytes unchanged");
 using(var cs=new CollisionStore(typedDir)) {Check(cs.Active.Single().Tag==null,"legacy replay untyped");Check(cs.Undo(legacy.SteamId,legacy.Map)?.Id=="legacy","legacy undo supported");}
} finally {Directory.Delete(typedDir,true);}
Console.WriteLine("Typed tag fixtures passed.");

var nd=Path.Combine(Path.GetTempPath(),"debug-note-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(nd);
try {
 var m=new CollisionMark("g",DateTimeOffset.UtcNow,"nocollide","v24","owner",a,new(0,0,1),b,a,"glock","texture");
 using(var cs=new CollisionStore(nd)) {
 cs.Add(m with {Id="legacy",Weapon=null,Tag=null});
 Check(cs.Annotate("owner","v24","no legacy")==null,"notes never classify legacy");
 cs.Add(m); cs.Add(m with {Id="m4",Weapon="m4a1",Tag="nocollide"});
 Check(cs.Annotate("other","v24","not mine")==null,"note ownership isolation");
 Check(cs.Annotate("owner","other","wrongmap")==null,"note map isolation");
 Check(cs.Annotate("owner","v24","墙壁\n<test>")==m,"note skips M4 and preserves Unicode");
 bool empty=false;try{cs.Annotate("owner","v24"," \n ");}catch(ArgumentException){empty=true;}Check(empty,"empty note rejected");
 Check(cs.Active.Count==3,"annotations not marks");
 }
 using(var cs=new CollisionStore(nd)) {
 Check(cs.Active.Count==3,"annotations reload without resurrection");
 cs.Undo("owner","v24");Check(cs.Undo("owner","v24")?.Id=="g","undo original Glock ID");
 Check(cs.Annotate("owner","v24","gone")==null,"undone Glock ineligible");
 }
 using(var cs=new CollisionStore(nd))Check(cs.Active.Single().Id=="legacy","annotation never resurrects undone mark");
}finally{Directory.Delete(nd,true);}
Console.WriteLine("Annotation fixtures passed.");
