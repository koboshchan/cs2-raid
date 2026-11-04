namespace Cs2Raid;

public sealed record RaidOutcome(string Player, bool Extracted, DateTimeOffset At, DateTimeOffset CameraAt);
public sealed record RaidEndProjection(string Recipient, string[] OwnRaidTeam, bool Won, string? GlobalMvp);
// Game-thread only. No timers, native handles, payouts or callbacks survive their owning match.
public sealed class RaidOutcomes
{
    public const int DelaySeconds = 5;
    private readonly Dictionary<string,RaidOutcome> outcomes = [];
    private readonly HashSet<string> cameraReleased = [];
    private readonly HashSet<string> disconnected = [];
    private bool ended;
    public DateTimeOffset? ClosedAt { get; private set; }
    public bool Record(string id,bool extracted,DateTimeOffset now) {
        if(ended || outcomes.ContainsKey(id))return false;
        outcomes.Add(id,new(id,extracted,now,now.AddSeconds(DelaySeconds)));return true;
    }
    public bool Waiting(string id,DateTimeOffset now) => outcomes.TryGetValue(id,out var o)&&now<o.CameraAt;
    public bool CanObserve(string id,DateTimeOffset now) => !disconnected.Contains(id)&&outcomes.TryGetValue(id,out var o)&&now>=o.CameraAt;
    public void Disconnect(string id) { disconnected.Add(id);cameraReleased.Add(id); }
    // Reconnects/new pawns never inherit a delayed callback or replay a banner.
    public bool TakeCamera(string id,DateTimeOffset now) => !disconnected.Contains(id)&&
        outcomes.TryGetValue(id,out var o)&&now>=o.CameraAt&&cameraReleased.Add(id);
    public void Close(DateTimeOffset now) { ClosedAt??=now; }
    public bool ReadyToFinish(DateTimeOffset now) => ClosedAt is {} at && now>=at.AddSeconds(DelaySeconds)&&
        outcomes.Values.All(o=>now>=o.CameraAt);
    public RaidEndProjection[] Finish(GameplayState game,DateTimeOffset now) {
        if(ended || !ReadyToFinish(now))return [];
        ended=true;
        // Final verified extraction money only, never kills, inventory value or profit.
        var mvp=game.Players.Values.Where(p=>p.Extracted).OrderByDescending(p=>p.MoneyExtractedCents)
            .ThenBy(p=>p.ExtractedOrder).FirstOrDefault()?.Id;
        return game.Players.Values.Where(p=>!disconnected.Contains(p.Id)).Select(p=>new RaidEndProjection(
            p.Id,game.Players.Values.Where(m=>m.Id==p.Id || game.SameTeam(p.Id,m.Id)).Select(m=>m.Id).ToArray(),
            mvp!=null&&p.Extracted,mvp)).ToArray();
    }
}
