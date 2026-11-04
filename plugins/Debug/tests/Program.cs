using System.Text.Json;
using Cs2Debug;

var checks = 0;
void Check(bool condition, string label)
{
    if (!condition) throw new InvalidOperationException(label);
    ++checks; Console.WriteLine("PASS " + label);
}
var a = new Point(1, 2, 3);
var b = new Point(4, 5, 6);
var tracker = new ShotTracker();
Check(tracker.Impact(10, a) == null, "impact without fire ignored");
tracker.Fire("ak47", 10);
Check(tracker.Impact(10, a) == "ladder_start", "AK first shot marks point one");
Check(tracker.LadderStart == a, "ladder endpoint retained as value");
Check(tracker.Impact(10, b) == null, "penetration impact ignored");
tracker.Fire("weapon_ak47", 11);
Check(tracker.Impact(11, b) == "ladder_end", "AK second shot marks point two");
tracker.Fire("ak47", 12);
Check(tracker.Impact(12, a) == null, "third AK spray ignored");
for (var i = 0; i < 10000; ++i) tracker.Fire("ak47", 12);
Check(tracker.Impact(12, a) == null, "continued AK spray never rearms");
tracker.Reset();
Check(tracker.LadderStart == null, "reset clears endpoints");
tracker.Fire("ak47", 30);
Check(tracker.Impact(31, a) == null, "next tick impact expires");
Check(tracker.Impact(30, a) == null, "expired ticket consumed even when tick rewinds");
tracker.Fire("ak47", 32);
Check(tracker.Impact(32, b) == "ladder_end", "missed first shot still consumes first ordinal");
tracker.Reset();
tracker.Fire("glock", 50);
Check(tracker.Impact(50, a) == "texture", "Glock first impact marks texture");
Check(tracker.Impact(50, b) == null, "Glock penetration ignored");
tracker.Fire("weapon_glock", 51);
Check(tracker.Impact(51, b) == "texture", "each Glock fire gets a ticket");
tracker.Fire("glock", 52);
tracker.Fire("awp", 52);
Check(tracker.Impact(52, a) == null, "unsupported weapon invalidates ticket");
tracker.Fire("glock", 53);
Check(tracker.Impact(53, new(float.NaN, 0, 0)) == null, "nonfinite first impact rejected");
Check(tracker.Impact(53, a) == null, "invalid impact still consumes first impact");
tracker.Fire("ak47", 54); tracker.Reset();
Check(tracker.Impact(54, a) == null, "disconnect death spawn round map reset clears pending");
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
tracker.Fire("ak47", 100);
Check(tracker.Impact(100, a) == "ladder_start", "new first marker candidate");
tracker.InvalidateLadderStart();
Check(tracker.LadderStart == null, "failed durable first save invalidates ladder start");
tracker.Fire("ak47", 101);
Check(tracker.Impact(101, b) == "ladder_end" && tracker.LadderStart == null, "second endpoint cannot claim durable pair after failed first save");

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
