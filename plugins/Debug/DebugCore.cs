using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Cs2Debug;

public readonly record struct Point(float X, float Y, float Z)
{
    public bool IsFinite => float.IsFinite(X) && float.IsFinite(Y) && float.IsFinite(Z);
}
public sealed record MapReport(string Id, DateTimeOffset Timestamp, string Kind, string Map,
    string SteamId, string Name, string Reason, Point Position, Point Angles, Point? LadderStart = null);

// Source events have no projectile ID. Only accept the first impact in the SAME server tick
// after a qualifying fire, and clear the ticket for every fire (including other weapons).
public sealed record ShotTicket(string Weapon, string Tag);
public sealed class ShotTracker
{
    private int? pendingTick;
    private string? pendingWeapon;
    public void Fire(string weapon, int tick)
    {
        var normalized = weapon.StartsWith("weapon_", StringComparison.Ordinal) ? weapon[7..] : weapon;
        pendingWeapon = normalized is "glock" or "m4a1" or "m4a1_silencer" ? normalized : null;
        pendingTick = pendingWeapon == null ? null : tick;
    }
    public ShotTicket? Impact(int tick, Point position)
    {
        var accept = pendingTick == tick;
        var weapon = pendingWeapon;
        Reset();
        return accept && position.IsFinite && weapon != null
            ? new ShotTicket(weapon, weapon == "glock" ? "texture" : "nocollide") : null;
    }
    public void Reset() { pendingTick = null; pendingWeapon = null; }

}

// Tracks the last value the engine actually accepted, including any cvar clamp.
// A later map config change becomes the new baseline restored on disable/unload.
public sealed class MaintainedSetting<T>(T baseline) where T : IEquatable<T>
{
    public T Baseline { get; private set; } = baseline;
    private T appliedValue = baseline;
    private bool applied;
    public bool Observe(T current)
    {
        if (applied && current.Equals(appliedValue)) return false;
        Baseline = current;
        return true;
    }
    public void Applied(T actual) { appliedValue = actual; applied = true; }
}

public static class ReportInput
{
    public static bool TryReason(string? raw, out string reason)
    {
        reason = (raw ?? "").Trim();
        if (reason.Length is < 1 or > 300 || reason.Any(char.IsControl)) return false;
        return true;
    }
    public static string SafeText(string? text, int max) =>
        new((text ?? "").Where(c => !char.IsControl(c)).Take(max).ToArray());
    public static bool TryCount(string? raw, out int count)
    {
        if (string.IsNullOrWhiteSpace(raw)) { count = 5; return true; }
        return int.TryParse(raw, NumberStyles.None, CultureInfo.InvariantCulture, out count) && count is >= 1 and <= 20;
    }
}

public sealed class ReportLimiter
{
    private readonly Dictionary<ulong, long> next = [];
    public bool Allow(ulong player, long nowMilliseconds, int intervalMilliseconds)
    {
        if (next.TryGetValue(player, out var until) && nowMilliseconds < until) return false;
        next[player] = nowMilliseconds + intervalMilliseconds; return true;
    }
    public void Remove(ulong player) => next.Remove(player);
    public void Clear() => next.Clear();
}

// One append-only file, one server-thread writer. No paths derived from player or map input.
public sealed class ReportStore : IDisposable
{
    private FileStream? stream;
    private readonly Queue<MapReport> recent = new();
    public bool Available => stream != null;
    public string? Failure { get; private set; }
    public ReportStore(string directory)
    {
        try
        {
            if (!Path.IsPathFullyQualified(directory)) throw new IOException("Report directory must be absolute");
            Directory.CreateDirectory(directory);
            stream = new FileStream(Path.Combine(directory, "reports.jsonl"), FileMode.OpenOrCreate,
                FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.WriteThrough);
            LoadTailAndRepair();
        }
        catch (Exception e) when (IsStorageError(e)) { Fail(e); }
    }
    private void LoadTailAndRepair()
    {
        var file = stream!;
        // Recover a torn final JSONL record before any append. Read at most 256 KiB.
        var length = file.Length;
        var size = (int)Math.Min(length, 262144);
        var bytes = new byte[size];
        file.Position = length - size;
        file.ReadExactly(bytes);
        var lastNewline = Array.LastIndexOf(bytes, (byte)'\n');
        if (length > 0 && (size == 0 || bytes[^1] != (byte)'\n'))
        {
            if (lastNewline < 0 && length > size) throw new IOException("Unrecoverable oversized trailing record");
            file.SetLength(length - size + lastNewline + 1);
            file.Flush(true);
        }
        var start = length > size ? Array.IndexOf(bytes, (byte)'\n') + 1 : 0;
        var end = lastNewline + 1;
        if (end > start)
            foreach (var line in Encoding.UTF8.GetString(bytes, start, end - start).Split('\n', StringSplitOptions.RemoveEmptyEntries))
                try { var report = JsonSerializer.Deserialize<MapReport>(line); if (report != null && ValidRecord(report)) Remember(report); }
                catch (JsonException) { /* a corrupt historical record is not a successful new append */ }
        file.Position = file.Length;
    }
    public bool Append(MapReport report)
    {
        if (stream == null) return false;
        if (!ValidRecord(report)) return false;
        try
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report) + "\n");
            stream.Write(bytes);
            stream.Flush(true); // only acknowledge after the OS flush-to-disk request succeeds
            Remember(report);
            return true;
        }
        catch (Exception e) when (IsStorageError(e)) { Fail(e); return false; }
    }
    private static bool ValidRecord(MapReport report) =>
        !string.IsNullOrEmpty(report.Id) && report.Id.Length <= 64 &&
        !string.IsNullOrEmpty(report.Kind) && report.Kind.Length <= 32 &&
        report.Map is { Length: <= 128 } && report.SteamId is { Length: <= 32 } &&
        report.Name is { Length: <= 128 } && report.Position.IsFinite && report.Angles.IsFinite &&
        report.LadderStart is not { IsFinite: false } && ReportInput.TryReason(report.Reason, out _);
    public MapReport[] Latest(int count) => recent.Reverse().Take(Math.Clamp(count, 1, 20)).ToArray();
    private void Remember(MapReport report) { recent.Enqueue(report); while (recent.Count > 64) recent.Dequeue(); }
    private static bool IsStorageError(Exception e) => e is IOException or UnauthorizedAccessException or
        ArgumentException or NotSupportedException or System.Security.SecurityException;
    private void Fail(Exception e) { Failure = e.GetType().Name; Dispose(); }
    public void Dispose() { try { stream?.Dispose(); } catch (IOException) { } finally { stream = null; } }
}


// CS2 native teams are 2 (T) and 3 (CT); 0 is the draw winner.
// Event reasons match CounterStrikeSharp RoundEndReason (CTsWin=8, TerroristsWin=9, RoundDraw=10).
public enum BannerResult { Win, Lose, Draw }
public readonly record struct RoundBanner(int Winner, int Reason, string Message)
{
    // cs_win_panel_round.final_event carries the end reason; zero means Unknown.
    public int PanelFinalEvent => Reason;
    public static bool TryCreate(int nativeTeam, BannerResult result, out RoundBanner banner)
    {
        banner = default;
        if (nativeTeam is not (2 or 3) || !Enum.IsDefined(result)) return false;
        var winner = result == BannerResult.Draw ? 0 : result == BannerResult.Win ? nativeTeam : 5 - nativeTeam;
        banner = winner switch
        {
            2 => new(2, 9, "#SFUI_Notice_Terrorists_Win"),
            3 => new(3, 8, "#SFUI_Notice_CTs_Win"),
            _ => new(0, 10, "#SFUI_Notice_Round_Draw")
        };
        return true;
    }
}

// Native result styling is owned by client Panorama, not arbitrary event strings.
public static class BannerInput
{
    // argumentCount excludes the command itself. Exactly one action is accepted.
    public static bool TryParse(int argumentCount, string? action, out BannerResult parsed)
    {
        parsed = action?.ToLowerInvariant() switch {
            "win" => BannerResult.Win, "lose" => BannerResult.Lose,
            "draw" or "tie" => BannerResult.Draw, _ => (BannerResult)(-1)
        };
        return argumentCount == 1 && Enum.IsDefined(parsed);
    }
    public static string NativeColor(int team, BannerResult result) =>
        result == BannerResult.Lose ? "red" : result == BannerResult.Draw ? "gray" : team == 2 ? "gold" : "blue";
}

public sealed record CollisionMark(string Id, DateTimeOffset Timestamp, string Label, string Map,
    string SteamId, Point Position, Point Normal, Point PlayerPosition, Point EyeAngles, string? Weapon = null, string? Tag = null);
public sealed record CollisionEntry(string Action, CollisionMark? Mark, string? RemovedId, DateTimeOffset Timestamp, string? ReferenceMarkId = null, string? Note = null, string? AuthorSteamID = null);

// Append-only journal. Undo tombstones remove marks from the active set, never erase history.
public sealed class CollisionStore : IDisposable
{
    private readonly FileStream file;
    private readonly List<CollisionMark> active = [];
    public IReadOnlyList<CollisionMark> Active => active;
    public CollisionStore(string directory)
    {
        if (!Path.IsPathFullyQualified(directory)) throw new IOException("Absolute directory required");
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "nocollide.jsonl");
        file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read,
            4096, FileOptions.WriteThrough);
        try
        {
            using var reader = new StreamReader(file, Encoding.UTF8, false, 4096, leaveOpen: true);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var entry = JsonSerializer.Deserialize<CollisionEntry>(line) ?? throw new IOException("Invalid journal");
                if (entry.Action == "add" && entry.Mark is {} mark && Valid(mark)) active.Add(mark);
                else if (entry.Action == "undo" && entry.RemovedId != null) active.RemoveAll(x => x.Id == entry.RemovedId);
                else if (entry.Action == "annotation" && entry.ReferenceMarkId != null && entry.Note != null && entry.AuthorSteamID != null) { /* linked metadata, never a mark */ }
                else throw new IOException("Invalid journal entry");
            }
            // A valid last record without LF must still be separated from the next append.
            if (file.Length > 0) {
                file.Position = file.Length - 1;
                if (file.ReadByte() != 10) { file.Position = file.Length; file.WriteByte(10); file.Flush(true); }
            }
            file.Position = file.Length;
        }
        catch { file.Dispose(); throw; } // Never silently discard corrupt history.
    }
    private static bool Valid(CollisionMark m) => m.Label == "nocollide" && m.Position.IsFinite &&
        m.Normal.IsFinite && m.PlayerPosition.IsFinite && m.EyeAngles.IsFinite &&
        m.Normal.X*m.Normal.X+m.Normal.Y*m.Normal.Y+m.Normal.Z*m.Normal.Z is > 0.5f and < 1.5f &&
        ((m.Weapon == null && m.Tag == null) ||
         (m.Weapon == "glock" && m.Tag == "texture") ||
         (m.Weapon is "m4a1" or "m4a1_silencer" && m.Tag == "nocollide"));
    private bool failed;
    private void Append(CollisionEntry entry)
    {
        if (failed) throw new IOException("Journal disabled after failed write; reload required");
        try {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entry) + "\n");
            file.Write(bytes); file.Flush(true);
        } catch { failed = true; throw; }
    }
    public void Add(CollisionMark mark)
    {
        if (!Valid(mark)) throw new ArgumentException("Invalid mark");
        Append(new("add", mark, null, mark.Timestamp));
        active.Add(mark);
    }
    public CollisionMark? Annotate(string steamId, string map, string raw)
    {
        var note = new string(raw.Select(c => char.IsControl(c) || c is '<' or '>' ? ' ' : c).ToArray()).Trim();
        if (note.Length is < 1 or > 300) throw new ArgumentException("Note must contain 1-300 characters");
        var mark = active.LastOrDefault(x => x.SteamId == steamId && x.Map == map && x.Weapon == "glock" && x.Tag == "texture");
        if (mark == null) return null;
        Append(new("annotation", null, null, DateTimeOffset.UtcNow, mark.Id, note, steamId));
        return mark;
    }
    public CollisionMark? Undo(string steamId, string map)
    {
        var mark = active.LastOrDefault(x => x.SteamId == steamId && x.Map == map);
        if (mark == null) return null;
        Append(new("undo", null, mark.Id, DateTimeOffset.UtcNow));
        active.Remove(mark); return mark;
    }
    public void Dispose() => file.Dispose();
}

// In-world debug geometry only; never changes collisions or extraction eligibility.
public static class ExtractBoxGeometry
{
    public sealed record Line(Point Start, Point End, bool Warning);
    public static Line[] Lines()
    {
        using var json = JsonDocument.Parse(typeof(ExtractBoxGeometry).Assembly.GetManifestResourceStream("SpaceportZoneProfile")
            ?? throw new InvalidOperationException("Missing shared extraction bounds"));
        var result = new List<Line>();
        foreach (var key in new[]{"extractionZone","warningZone"})
        {
            var box=json.RootElement.GetProperty(key);
            var corners=box.GetProperty("vertices").EnumerateArray()
                .Select(v=>(v.GetProperty("x").GetSingle(),v.GetProperty("y").GetSingle())).ToArray();
            if(corners.Length!=4 || corners.Any(v=>!float.IsFinite(v.Item1)||!float.IsFinite(v.Item2)))
                throw new InvalidOperationException("Expected four finite extraction corners");
            var warning=key=="warningZone";
            foreach(var z in new[]{0f,150f,300f})
                for(var i=0;i<4;i++) {
                    var a=corners[i];var b=corners[(i+1)%4];
                    result.Add(new(new(a.Item1,a.Item2,z),new(b.Item1,b.Item2,z),warning));
                }
            foreach(var (x,y) in corners)result.Add(new(new(x,y,0),new(x,y,300),warning));
        }
        return result.ToArray();
    }
}
