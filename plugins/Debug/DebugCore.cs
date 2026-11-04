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
public sealed class ShotTracker
{
    private int akShots;
    private int? pendingTick;
    private string? pendingKind;
    public Point? LadderStart { get; private set; }
    public void Fire(string weapon, int tick)
    {
        pendingTick = null; pendingKind = null;
        if (weapon is "ak47" or "weapon_ak47")
        {
            if (akShots >= 2) return;
            ++akShots;
            pendingKind = akShots == 1 ? "ladder_start" : "ladder_end";
        }
        else if (weapon is "glock" or "weapon_glock") pendingKind = "texture";
        if (pendingKind != null) pendingTick = tick;
    }
    public string? Impact(int tick, Point position)
    {
        var kind = pendingTick == tick ? pendingKind : null;
        pendingTick = null; pendingKind = null; // even a malformed first impact consumes the ticket
        if (!position.IsFinite || kind == null) return null;
        if (kind == "ladder_start") LadderStart = position;
        return kind;
    }
    public void InvalidateLadderStart() => LadderStart = null;
    public void Reset() { akShots = 0; pendingTick = null; pendingKind = null; LadderStart = null; }
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
