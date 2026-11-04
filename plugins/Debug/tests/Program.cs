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
foreach (var result in new[] { "win", "lose", "tie" })
foreach (var color in new[] { "gold", "blue", "red", "gray" })
{
    Check(BannerInput.TryParse(result, color, out var parsed), $"parse {result} {color}");
    var expected = result == "lose" ? "red" : result == "tie" ? "gray" : team == 2 ? "gold" : "blue";
    Check(BannerInput.NativeColor(team, parsed) == expected, $"native color {team} {result}");
    Check(BannerInput.Matches(team, parsed, color) == (color == expected), $"color support {team} {result} {color}");
}
Check(BannerInput.TryParse("WIN", "GOLD", out var upper) && upper == BannerResult.Win, "case insensitive banner");
Check(!BannerInput.TryParse("draw", "red", out _), "reject invalid result");
Check(!BannerInput.TryParse("win", "<font color=green>", out _), "reject color injection");
Check(!BannerInput.TryParse("win", "green", out _), "reject removed green");
Check(!BannerInput.TryParse("win", "white", out _), "reject removed white");
Check(!BannerInput.TryParse("win", "purple", out _), "reject unadvertised color");
Check(!RoundBanner.TryCreate(2, (BannerResult)99, out _), "undefined banner result rejected");
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
Check(PreviewArgs.Parse([])==new PreviewArgs(300,"alert",false),"preview default");
foreach(var bad in new[]{new[]{"red"},new[]{"15","red"},new[]{"plain","tags","classes"},new[]{"15","16"},new[]{"0"},new[]{"3601"},new[]{"stop","css"}})
 Check(PreviewArgs.Parse(bad)==null,"invalid preview args rejected");
Check(PreviewArgs.Parse(["15","png"])==null&&PreviewArgs.Parse(["15","css"])==null,"retired backgrounds rejected no fallback");
Check(PreviewArgs.Parse(["stop"])!.Stop,"canonical stop");
var clock=new PreviewClock(0,15);
Check(clock.Tick(0,0,true,true,"tags")==null,"outside invisible");
Check(clock.Remaining(4000)==11,"outside clock advances");
Check(clock.Tick(4000,1,true,true,"tags")!.Contains("00:11"),"reentry shortened box");
Check(clock.Tick(4050,2,true,true,"tags")!.Contains("#00FF00"),"real immediately green");
Check(clock.Tick(5000,1,true,true,"tags")!.Contains("[ 10 ]"),"ten bracket");
Check(clock.Tick(14000,2,true,true,"tags")!.Contains("[ 1 ]"),"one bracket");
Check(clock.Tick(15000,2,true,true,"tags")=="","zero clear");
Check(clock.Tick(15001,2,true,true,"tags")==null,"idempotent clear");
foreach(var z in new[]{-1e200,0,1e200}){
 Check(PreviewClock.Zone(943.874,-6000,z)==2,"real edge priority");
 Check(PreviewClock.Zone(513.673,-6000,z)==1,"warning edge");
}
Check(PreviewClock.Zone(1100,-6000,double.NaN)==0,"nonfinite reject");
foreach(var style in new[]{"plain","tags","classes"}){
 Check(PreviewClock.Markup(300,false,style).Contains("05:00"),"five minute format");
 Check(PreviewClock.Markup(264,false,style).Contains("04:24"),"reference format");
 var h=new PreviewClock(0,15);h.Tick(0,2,true,true,style);
 Check(h.Tick(1,2,false,true,style)=="","invalid/dead/spec clears");
 Check(h.Clear(true)==null,"clear ownership revoked");
 h=new(0,15);h.Tick(0,2,true,true,style);Check(h.Tick(1,2,true,false,style)==null,"menu no unowned clear");
}

Check(PreviewActivation.HudOnly(true,"192.168.1.162:27016",27016,true),"explicit private HUD scope");
Check(!PreviewActivation.HudOnly(false,"192.168.1.162:27016",27016,true),"HUD defaults off");
Check(!PreviewActivation.HudOnly(true,"192.168.1.162:27016",27015,true),"public port denied");
Check(!PreviewActivation.HudOnly(true,"192.168.1.162:27016",27016,false),"non LAN denied");
Check(!PreviewActivation.HudOnly(true,"",27016,true),"target attestation required");
Check(!PreviewActivation.Legacy(true,true),"HUD-only excludes legacy economy activation");
Check(PreviewActivation.Legacy(true,false),"legacy consumer behavior preserved");
Check(!PreviewActivation.Legacy(false,false),"legacy disabled preserved");

Check(RaidHudGuard.InactiveJson("{\"enabled\":false}"),"explicit inactive raid allowed");
foreach(var json in new[]{"{\"enabled\":true}","{}","[]","null","{","{\"enabled\":0}","{\"enabled\":false,\"Enabled\":true}"})
 Check(!RaidHudGuard.InactiveJson(json),"unsafe raid configuration blocks");
Check(!RaidHudGuard.ReadInactive(()=>throw new FileNotFoundException(),false),"missing not implicitly inactive");
Check(RaidHudGuard.ReadInactive(()=>throw new FileNotFoundException(),true),"verified absent minimal loader allowed");
Check(!RaidHudGuard.ReadInactive(()=>throw new UnauthorizedAccessException(),true),"permissions fail closed");
Check(!RaidHudGuard.ReadInactive(()=>throw new IOException(),true),"IO failure not absent");
var blockedClock=new PreviewClock(0,300);blockedClock.Tick(0,2,true,true,"tags");
Check(blockedClock.Tick(1,2,RaidHudGuard.InactiveJson("{\"enabled\":true}"),true,"tags")=="","active transition owned clear");
Check(blockedClock.Clear(true)==null,"transition clear idempotent");
var menuClock=new PreviewClock(0,300);menuClock.Tick(0,2,true,true,"tags");
Check(menuClock.Tick(1,2,false,false,"css")==null,"active transition cannot clear menu");

var lifecycle=new PreviewNativeLifecycle();int nativeCalls=0,managedCalls=0;
lifecycle.Clear(()=>nativeCalls++,()=>managedCalls++);
Check(nativeCalls==0&&managedCalls==1,"initial Load no globals/native cleanup");
lifecycle.Stop();lifecycle.Clear(()=>nativeCalls++,()=>managedCalls++);
Check(nativeCalls==0&&managedCalls==2,"error Unload no globals/native cleanup");
lifecycle.OnTick();lifecycle.Clear(()=>nativeCalls++,()=>managedCalls++);
Check(nativeCalls==1&&managedCalls==3,"native cleanup only after tick availability");
lifecycle.Stop();lifecycle.Stop();lifecycle.Clear(()=>nativeCalls++,()=>managedCalls++);
Check(nativeCalls==1&&managedCalls==4,"idempotent teardown no native call");

var failureLife=new PreviewNativeLifecycle();failureLife.OnTick();
var owned=new List<int>{1,2,3};var attempted=new List<int>();
var detached=owned.ToArray();bool cleanupFailed=false;
try{failureLife.Clear(()=>PreviewCleanup.Each(detached,x=>{attempted.Add(x);if(x==1)throw new InvalidOperationException("simulated native failure");}),()=>owned.Clear());}
catch(AggregateException e){cleanupFailed=e.InnerExceptions.Count==1;}
Check(cleanupFailed&&owned.Count==0&&attempted.SequenceEqual(new[]{1,2,3}),"throw cancels all and attempts later owned contexts, errors visible");
failureLife.OnTick();Check(owned.Count==0,"guard recovery cannot resume cancelled previews");
failureLife.Stop();int afterStopCalls=0;
failureLife.Clear(()=>afterStopCalls++,()=>owned.Clear());
Check(!failureLife.Available&&afterStopCalls==0,"mapend unavailable first prevents native teardown calls");
var cancelFirst=new PreviewNativeLifecycle();cancelFirst.OnTick();bool detachedFirst=false;
try{cancelFirst.Clear(()=>{Check(detachedFirst,"cancellation before throwing native work");throw new InvalidOperationException();},()=>detachedFirst=true);}catch(InvalidOperationException){}
Check(detachedFirst,"failure preserves cancellation");

Check(!cancelFirst.Available,"native cleanup failure invalidates availability before rethrow");

var dedupe=new PreviewClock(0,15);
Check(dedupe.Tick(0,2,true,true,"classes")!=null,"first render");
for(int ms=1;ms<200;ms++)Check(dedupe.Tick(ms,2,true,true,"classes")==null,"before HTML keepalive deadline");
Check(dedupe.Tick(200,2,true,true,"classes")!=null,"same content required HTML keepalive");
Check(dedupe.Tick(1000,2,true,true,"classes")!=null,"next second render");
Check(dedupe.Tick(1050,1,true,true,"classes")!.Contains("#FF0000"),"immediate red");
Check(dedupe.Tick(1100,1,true,true,"tags")!=null,"immediate style");
Check(dedupe.Tick(1101,0,true,true,"tags")=="","exit immediate clear");
Check(dedupe.Tick(1102,0,true,true,"tags")==null,"exit clear once");
Check(dedupe.Tick(1150,2,true,true,"tags")!=null,"reenter immediate");
Check(dedupe.Tick(5000,2,true,true,"classes")!.Contains("[ 10 ]"),"shape boundary");
foreach(var variant in new[]{"plain","tags","classes"})Check(PreviewArgs.Parse(["15",variant])!.Style==variant,"bounded matrix parser");
Check(!PreviewClock.Markup(15,false,"classes").Contains("style="),"matrix no browser css");

foreach(var z in new[]{double.MinValue,-1e100,0,1e100,double.MaxValue}){
 foreach(var x in new[]{943.874,1374.076})foreach(var y in new[]{-6816.610,-5143.672}){
  Check(PreviewClock.Zone(x,y,z)==2,"exact real corner precedence finiteZ");
  Check(PreviewClock.Zone(Math.BitIncrement(x),y,z)>=1,"epsilon corner neighborhood");
 }
 foreach(var x in new[]{513.673,1804.277})foreach(var y in new[]{-8489.547,-3470.735})Check(PreviewClock.Zone(x,y,z)==1,"warning corners inclusive");
 Check(PreviewClock.Zone(800,-5980.141,z)==1,"left ring");Check(PreviewClock.Zone(1600,-5980.141,z)==1,"right ring");
 Check(PreviewClock.Zone(1158.975,-7500,z)==1,"bottom ring");Check(PreviewClock.Zone(1158.975,-4000,z)==1,"top ring");
 foreach(var x in new[]{943.874,1374.076})Check(PreviewClock.Zone(x,-5980.141,z)==2,"real xedge");
 foreach(var y in new[]{-6816.610,-5143.672})Check(PreviewClock.Zone(1158.975,y,z)==2,"real yedge");
 Check(PreviewClock.Zone(Math.BitDecrement(943.874),-5980.141,z)==1,"left real epsilon");
 Check(PreviewClock.Zone(Math.BitIncrement(1374.076),-5980.141,z)==1,"right real epsilon");
 Check(PreviewClock.Zone(1158.975,Math.BitDecrement(-6816.610),z)==1,"bottom real epsilon");
 Check(PreviewClock.Zone(1158.975,Math.BitIncrement(-5143.672),z)==1,"top real epsilon");
 Check(PreviewClock.Zone(Math.BitDecrement(513.673),-5980.141,z)==0,"left warning epsilon");
 Check(PreviewClock.Zone(Math.BitIncrement(1804.277),-5980.141,z)==0,"right warning epsilon");
 Check(PreviewClock.Zone(1158.975,Math.BitDecrement(-8489.547),z)==0,"bottom warning epsilon");
 Check(PreviewClock.Zone(1158.975,Math.BitIncrement(-3470.735),z)==0,"top warning epsilon");
}

Check(PreviewDismiss.ProbeDuration("clearblank")==1,"explicit blank Duration1 comparison");
Check(PreviewDismiss.ProbeDuration("clearttl")==0,"explicit TTL0 comparison not hide promise");
Check(PreviewDismiss.ProbeDuration("globalclear")==null,"no arbitrary dismiss event");
Check(!PreviewDismiss.ShouldRender(null)&&!PreviewDismiss.ShouldRender(""),"automatic clear never reopens empty panel");
Check(PreviewDismiss.ShouldRender("[ 10 ]"),"nonempty render retained");
var dismissClock=new PreviewClock(0,15);dismissClock.Tick(0,2,true,true,"classes");
Check(dismissClock.Clear(true)==""&&dismissClock.Clear(true)==null,"owned clear once");
Check(!PreviewDismiss.ShouldRender(dismissClock.Tick(1,0,true,true,"classes")),"outside no repeated blank event");

Check(PreviewArgs.Parse(["15","alert"])!.Style=="alert","alert parser");
Check(!PreviewDispatch.IsHtml("alert")&&PreviewDispatch.IsHtml("classes"),"clear probes HTML ownership only");
int htmlCalls=0,alertCalls=0;string? alertText=null;
var alertClock=new PreviewClock(0,15);
void SendAlert(long now,int zone)=>PreviewDispatch.Send("alert",alertClock.Tick(now,zone,true,true,"alert"),_=>htmlCalls++,s=>{alertCalls++;alertText=s;});
SendAlert(0,2);for(int ms=1;ms<1000;ms++)SendAlert(ms,2);
Check(alertCalls==1&&htmlCalls==0,"alert exactly once same displayed second zero HTML");
Check(alertText=="EXTRACT 00:15 Extracting","alert plain no color markup");
SendAlert(1000,2);Check(alertCalls==2,"alert next integer");
SendAlert(1001,1);Check(alertCalls==3&&alertText!.StartsWith("WARNING "),"alert zone change immediate");
SendAlert(1002,0);SendAlert(1003,0);Check(alertCalls==3&&htmlCalls==0,"alert exit no empty/native reopen");
alertClock.Clear(false);Check(!alertClock.Owned,"alert managed cancellation");
PreviewDispatch.Send("classes","test",_=>htmlCalls++,_=>alertCalls++);
Check(htmlCalls==1&&alertCalls==3,"HTML path never alert");
PreviewDispatch.Send("alert","",_=>htmlCalls++,_=>alertCalls++);
Check(htmlCalls==1&&alertCalls==3,"empty dispatch no foreign surface");

Check(PreviewArgs.Parse(["15","world"])!.Style=="world","actual world parser");
Check(WorldProbeGeometry.Text(15,false)=="00:15\nExtracting","world clock");
Check(WorldProbeGeometry.Text(10,true)=="[ 10 ]\nWARNING","world final warning");
var eye=WorldProbeGeometry.Place(0,0,64,0,0);
Check(Math.Abs(eye.X-48)<0.001&&Math.Abs(eye.Z-76)<0.001,"eye forward48 upper12");
bool worldBad=false;try{WorldProbeGeometry.Place(float.NaN,0,0,0,0);}catch(ArgumentException){worldBad=true;}
Check(worldBad,"world nonfinite fail closed");
var worldTimer=new PreviewClock(0,15);
worldTimer.Tick(0,2,true,true,"world");worldTimer.Tick(4000,0,true,true,"world");
Check(worldTimer.Remaining(6000)==9,"world outside timer continues");
Check(WorldProbeGeometry.Text(worldTimer.Remaining(6000),false)=="[ 9 ]\nExtracting","world reentry actual remaining");
worldTimer.Clear(false);Check(!worldTimer.Owned,"world clock managed cancellation");

foreach(var mode in new[]{"plain","tags","classes"}){
 var renewClock=new PreviewClock(0,300,200);
 Check(renewClock.Tick(0,2,true,true,mode)!=null,"HTML first "+mode);
 Check(renewClock.Tick(199,2,true,true,mode)==null,"HTML deadline before "+mode);
 Check(renewClock.Tick(200,2,true,true,mode)!=null,"HTML same content keepalive "+mode);
 Check(renewClock.Tick(200,1,true,true,mode)==null,"HTML aggregate cap "+mode);
 Check((renewClock.Tick(250,1,true,true,mode)!=null)==(mode!="plain"),"HTML pending latest color "+mode);
 Check(renewClock.Tick(9000,1,true,true,mode)!=null,"HTML delayed tick single current send "+mode);
 Check(renewClock.Tick(9000,1,true,true,mode)==null,"HTML no burst "+mode);
 Check(!PreviewDismiss.ShouldRender(renewClock.Tick(9001,0,true,true,mode)),"HTML outside no reopen "+mode);
 Check(!PreviewDismiss.ShouldRender(renewClock.Tick(9500,0,true,true,mode)),"HTML outside no renew "+mode);
}
var minRenew=new PreviewClock(0,300,1);minRenew.Tick(0,2,true,true,"tags");
Check(minRenew.Tick(99,2,true,true,"tags")==null&&minRenew.Tick(100,2,true,true,"tags")!=null,"HTML minimum100ms clamp");
var maxRenew=new PreviewClock(0,300,9999);maxRenew.Tick(0,2,true,true,"tags");
Check(maxRenew.Tick(499,2,true,true,"tags")==null&&maxRenew.Tick(500,2,true,true,"tags")!=null,"HTML maximum500ms clamp");

Check(LootCatalog.Entries.Count==247,"loot active247 only");
Check(LootCatalog.Entries.Select(e=>e.Id).Distinct().Count()==247,"loot unique ids");
Check(LootCatalog.Entries.All(e=>e.Model==null),"loot unresolved paths never guessed");
Check(LootCatalog.Entries.Select(e=>e.Asset).Distinct().Count()==17,"loot17 unresolved model assets");
Check(LootCatalog.Entries[0].X==544.23495&&LootCatalog.Entries[0].Y==41.55400000000009,"loot full40 exact coordinates");
Check(LootCatalog.MapAllowed("df_spaceport_player_scale_raytrace","default-20261105"),"loot exact binding");
Check(!LootCatalog.MapAllowed("de_dust2","default-20261105")&&!LootCatalog.MapAllowed(LootCatalog.Map,"old"),"loot map release denied");
Check(!LootCatalog.SafeModel("models/../bad.vmdl")&&!LootCatalog.SafeModel("models/bad.vmdl;quit")&&LootCatalog.SafeModel("models/test.vmdl"),"loot model path validation");

// Actual cleanup policy used by WorldHud, exercised without unsafe native calls.
{
    var faults=0;var removes=0;
    Check(WorldCleanup.Attempt(()=>true,()=>throw new Exception(),()=>removes++,()=>faults++)&&removes==1&&faults==1,"WORLD disable failure still removes");
    removes=0;faults=0;
    Check(!WorldCleanup.Attempt(()=>true,()=>{},()=>{removes++;throw new Exception();},()=>faults++),"WORLD removal failure retained");
    Check(WorldCleanup.Attempt(()=>true,()=>{},()=>removes++,()=>faults++)&&removes==2,"WORLD retained removal next attempt succeeds");
    removes=0;
    Check(WorldCleanup.Attempt(()=>false,()=>throw new Exception(),()=>removes++,()=>{})&&removes==0,"WORLD stale generation untouched");
    Check(!WorldCleanup.Attempt(()=>throw new Exception(),()=>{},()=>removes++,()=>{}),"WORLD validity exception retains generation");
    var pending=1;var passes=0;var residual=0;
    WorldCleanup.FinalDrain(()=>{passes++;if(passes==2)pending=0;},()=>pending,n=>residual=n);
    Check(passes==2&&pending==0&&residual==0,"WORLD unload synchronous retry succeeds without ticks");
    pending=1;passes=0;
    WorldCleanup.FinalDrain(()=>passes++,()=>pending,n=>residual=n);
    Check(passes==3&&residual==1,"WORLD unload persistent failure bounded and reported no callbacks");
    var active=true;var worldTestOwned=1;var cleanup=0;
    WorldCleanup.Guard(()=>throw new Exception("native gate"),()=>{active=false;worldTestOwned=0;cleanup++;});
    Check(!active&&worldTestOwned==0&&cleanup==1,"WORLD gate exception cancels managed refresh and attempts cleanup");
    var otherRemoved=0;
    foreach(var fail in new[]{true,false})WorldCleanup.Attempt(()=>true,()=>{if(fail)throw new Exception();},()=>otherRemoved++,()=>{});
    Check(otherRemoved==2,"WORLD disable failure continues other entities");
}

foreach(var unsupported in new[]{"css","png","unknown"}){
    var rejected=false;try{PreviewClock.Markup(180,false,unsupported);}catch(ArgumentException){rejected=true;}
    Check(rejected,"unsupported markup cannot fallback to white rectangle");
    Check(!PreviewDispatch.IsHtml(unsupported),"unsupported HTML dispatch rejected");
}
foreach(var textStyle in new[]{"plain","tags","classes"}){
    var markup=PreviewClock.Markup(180,false,textStyle);
    Check(!markup.Contains("<img")&&!markup.Contains("<div")&&!markup.Contains("background")&&!markup.Contains("data:"),"text-only no images/background");
}

Check(PreviewArgs.Parse(["15"])==new PreviewArgs(15,"alert",false),"final fallback duration-only selects alert");
Check(PreviewArgs.Parse(["15","alert"])!.Style=="alert","final fallback explicit alert supported");
Check(PreviewArgs.Parse(["15","world"])!.Style=="world","legacy world explicit only no silent selection");
Check(PreviewArgs.Parse(["15","css"])==null&&PreviewArgs.Parse(["15","png"])==null,"final fallback never retired backgrounds");
Check(PreviewClock.Markup(15,false,"alert")=="EXTRACT 00:15 Extracting"&&PreviewClock.Markup(15,true,"alert")=="WARNING 00:15 Extracting","alert state expressed in words not colors");
