using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;

namespace Cs2Debug;

public sealed class DebugConfig : BasePluginConfig
{
    public bool Enabled { get; set; } = true;
    public string ReportDirectory { get; set; } = "/data/map-reports";
}

public sealed class DebugPlugin : BasePlugin, IPluginConfig<DebugConfig>
{
    public override string ModuleName => "Map Debug";
    public override string ModuleVersion => "0.1.0";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "Debug economy and durable map issue reports";
    public DebugConfig Config { get; set; } = new();
    private readonly Dictionary<ulong, ShotTracker> shots = [];
    private readonly ReportLimiter reportLimiter = new();
    private readonly ReportLimiter textureLimiter = new();
    private readonly ReportLimiter listLimiter = new();
    private readonly ReportLimiter resetLimiter = new();
    private ReportStore? store;
    private bool loaded;
    private bool active;
    private ConVar<int>? buyAnywhere, maxMoney;
    private ConVar<float>? buyTime;
    private MaintainedSetting<int>? buyAnywhereSetting, maxMoneySetting;
    private MaintainedSetting<float>? buyTimeSetting;
    private bool haveSnapshot;

    public void OnConfigParsed(DebugConfig config)
    {
        Config = config;
        if (loaded) Reconfigure();
    }
    public override void Load(bool hotReload)
    {
        loaded = true;
        AddCommand("css_report", "Save a map issue at your current position", ReportCommand);
        AddCommand("css_reports", "List latest reports (1-20, default 5)", ReportsCommand);
        AddCommand("css_markreset", "Reset your two AK ladder shots", (p, c) =>
        {
            if (!Ready(p, c)) return;
            if (!resetLimiter.Allow(p!.SteamID, Environment.TickCount64, 1000))
            { c.ReplyToCommand("Wait one second between marker resets."); return; }
            Tracker(p!).Reset();
            c.ReplyToCommand("AK ladder markers reset. Fire two single shots.");
        });
        RegisterEventHandler<EventWeaponFire>((e, info) =>
        {
            if (active && Human(e.Userid))
                Tracker(e.Userid!).Fire(e.Weapon, Server.TickCount);
            return HookResult.Continue;
        });
        RegisterEventHandler<EventBulletImpact>((e, info) =>
        {
            var p = e.Userid;
            if (!active || !Human(p)) return HookResult.Continue;
            var position = new Point(e.X, e.Y, e.Z);
            var tracker = Tracker(p!);
            var kind = tracker.Impact(Server.TickCount, position);
            if (kind == null) return HookResult.Continue;
            // Glock burst/spray still consumes each fire's ticket but is limited to one saved report/sec.
            if (kind == "texture" && !textureLimiter.Allow(p!.SteamID, Environment.TickCount64, 1000))
            { p.PrintToChat("Texture marker not saved: wait one second between marks."); return HookResult.Continue; }
            if (!TryPose(p!, out _, out var angles))
            { p!.PrintToChat("Marker not saved: valid living player pose unavailable."); return HookResult.Continue; }
            var start = kind == "ladder_end" ? tracker.LadderStart : null;
            var saved = Save(p!, kind == "ladder_end" && start != null ? "ladder" : kind, kind == "texture" ? "Texture error" : "Requested ladder endpoint", position, angles, start);
            if (!saved && kind == "ladder_start") tracker.InvalidateLadderStart();
            p!.PrintToChat(saved
                ? kind switch {
                    "ladder_start" => "Ladder point 1 saved (first reported impact).",
                    "ladder_end" => start == null
                        ? "Ladder point 2 saved without point 1; use !markreset and retry."
                        : "Ladder point 2 saved. Use !markreset for another ladder.",
                    _ => "Texture error saved (first reported impact)."
                }
                : "Marker NOT confirmed saved. Report storage unavailable or invalid data; ask the server owner, then !markreset.");
            return HookResult.Continue;
        });
        RegisterEventHandler<EventRoundStart>((e, info) => { shots.Clear(); return HookResult.Continue; });
        RegisterEventHandler<EventPlayerSpawn>((e, info) =>
        {
            if (e.Userid is { IsValid: true } p) shots.Remove(p.SteamID);
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((e, info) => { Forget(e.Userid); return HookResult.Continue; });
        RegisterEventHandler<EventPlayerDisconnect>((e, info) =>
        {
            ForgetId(e.Xuid);
            Forget(e.Userid);
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterListener<Listeners.OnMapEnd>(() => { RestoreCvars(); ClearState(); });
        RegisterListener<Listeners.OnMapStart>(_ => { ClearState(); if (active) Server.NextFrame(ApplyCvars); });
        RegisterListener<Listeners.OnTick>(Tick);
        Reconfigure();
    }
    public override void Unload(bool hotReload)
    {
        active = false; loaded = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null;
    }
    private void Reconfigure()
    {
        active = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null;
        if (!Config.Enabled) return;
        store = new ReportStore(Config.ReportDirectory);
        if (!store.Available)
            Logger.LogError("Map Debug reports unavailable ({Failure}); check configured directory permissions. No reports will be acknowledged.", store.Failure);
        active = true;
        ApplyCvars();
    }
    private void ClearState()
    {
        shots.Clear(); reportLimiter.Clear(); textureLimiter.Clear(); listLimiter.Clear(); resetLimiter.Clear();
    }
    private static bool Human(CCSPlayerController? p) => p is { IsValid: true, IsBot: false } && p.SteamID != 0;
    private ShotTracker Tracker(CCSPlayerController p)
    {
        if (!shots.TryGetValue(p.SteamID, out var tracker)) shots[p.SteamID] = tracker = new ShotTracker();
        return tracker;
    }
    private void Forget(CCSPlayerController? p)
    {
        if (p is not { IsValid: true }) return;
        ForgetId(p.SteamID);
    }
    private void ForgetId(ulong id)
    {
        shots.Remove(id); reportLimiter.Remove(id);
        textureLimiter.Remove(id); listLimiter.Remove(id); resetLimiter.Remove(id);
    }
    private bool Ready(CCSPlayerController? p, CommandInfo c)
    {
        if (!active) { c.ReplyToCommand("Map Debug is disabled."); return false; }
        if (!Human(p)) { c.ReplyToCommand("A connected human player is required."); return false; }
        return true;
    }
    private static bool TryPose(CCSPlayerController p, out Point position, out Point angles)
    {
        position = default; angles = default;
        var pawn = p.PlayerPawn.Value;
        if (!p.PawnIsAlive || pawn is not { IsValid: true } || pawn.AbsOrigin is not { } origin) return false;
        var eye = pawn.EyeAngles;
        position = new(origin.X, origin.Y, origin.Z); angles = new(eye.X, eye.Y, eye.Z);
        return position.IsFinite && angles.IsFinite;
    }
    private void ReportCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (!ReportInput.TryReason(c.ArgString, out var reason))
        { c.ReplyToCommand("Usage: !report <reason>, 1-300 characters, no control characters."); return; }
        if (!TryPose(p!, out var position, out var angles))
        { c.ReplyToCommand("Report not saved: valid living player position required."); return; }
        if (!reportLimiter.Allow(p!.SteamID, Environment.TickCount64, 3000))
        { c.ReplyToCommand("Report not saved: wait three seconds between reports."); return; }
        c.ReplyToCommand(Save(p!, "position", reason, position, angles)
            ? "Report saved to disk at your current position."
            : "Report NOT confirmed saved. Report storage unavailable or invalid data; ask the server owner.");
    }
    private void ReportsCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (!ReportInput.TryCount(c.ArgString, out var count))
        { c.ReplyToCommand("Usage: !reports [1-20]"); return; }
        if (!listLimiter.Allow(p!.SteamID, Environment.TickCount64, 3000))
        { c.ReplyToCommand("Wait three seconds between report listings."); return; }
        if (store is not { Available: true })
        { c.ReplyToCommand("Report storage is unavailable."); return; }
        var reports = store.Latest(count);
        if (reports.Length == 0) { c.ReplyToCommand("No recent saved reports."); return; }
        foreach (var r in reports)
            c.ReplyToCommand($"[{r.Id[..Math.Min(8, r.Id.Length)]}] {r.Timestamp:MM-dd HH:mm} {ReportInput.SafeText(r.Kind, 20)} {ReportInput.SafeText(r.Map, 64)}: {ReportInput.SafeText(r.Reason, 100)}");
    }
    private bool Save(CCSPlayerController p, string kind, string reason, Point position, Point angles, Point? start = null)
    {
        if (store is not { Available: true }) return false;
        var report = new MapReport(Guid.NewGuid().ToString("N"), DateTimeOffset.UtcNow, kind,
            ReportInput.SafeText(Server.MapName, 128), p.SteamID.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ReportInput.SafeText(p.PlayerName, 128), reason, position, angles, start);
        var saved = store.Append(report);
        if (!saved && !store.Available)
            Logger.LogError("Map Debug report append failed ({Failure}); report acceptance disabled until config reload.", store.Failure);
        return saved;
    }
    private void ApplyCvars()
    {
        if (!active) return;
        try
        {
            if (!haveSnapshot)
            {
                buyAnywhere = ConVar<int>.Find("mp_buy_anywhere");
                buyTime = ConVar<float>.Find("mp_buytime");
                maxMoney = ConVar<int>.Find("mp_maxmoney");
                if (buyAnywhere == null || buyTime == null || maxMoney == null)
                    throw new InvalidOperationException("Required economy cvars unavailable");
                buyAnywhereSetting = new(buyAnywhere.Value);
                buyTimeSetting = new(buyTime.Value);
                maxMoneySetting = new(maxMoney.Value);
                haveSnapshot = true;
            }
            // Config execs can happen after map start. Keep the latest externally changed
            // baseline for restoration, then maintain overrides while debug is active.
            if (buyAnywhereSetting!.Observe(buyAnywhere!.Value))
            { buyAnywhere.Value = 1; buyAnywhereSetting.Applied(buyAnywhere.Value); }
            if (buyTimeSetting!.Observe(buyTime!.Value))
            { buyTime.Value = 1000000f; buyTimeSetting.Applied(buyTime.Value); }
            if (maxMoneySetting!.Observe(maxMoney!.Value))
            { maxMoney.Value = 65535; maxMoneySetting.Applied(maxMoney.Value); }
        }
        catch (Exception e)
        {
            Logger.LogError("Map Debug economy disabled ({Failure}).", e.GetType().Name);
            active = false; RestoreCvars();
        }
    }
    private void RestoreCvars()
    {
        if (!haveSnapshot) return;
        try { buyAnywhere!.Value = buyAnywhereSetting!.Baseline; }
        catch (Exception e) { Logger.LogError("Could not restore mp_buy_anywhere ({Failure}).", e.GetType().Name); }
        try { buyTime!.Value = buyTimeSetting!.Baseline; }
        catch (Exception e) { Logger.LogError("Could not restore mp_buytime ({Failure}).", e.GetType().Name); }
        try { maxMoney!.Value = maxMoneySetting!.Baseline; }
        catch (Exception e) { Logger.LogError("Could not restore mp_maxmoney ({Failure}).", e.GetType().Name); }
        haveSnapshot = false; buyAnywhere = null; buyTime = null; maxMoney = null;
    }
    private void Tick()
    {
        if (!active) return;
        ApplyCvars(); // also defeats a late +exec config on the very next tick
        if (!active || maxMoney == null) return;
        var amount = Math.Max(maxMoney.Value, 0);
        foreach (var p in Utilities.GetPlayers())
        {
            if (!Human(p) || p.InGameMoneyServices is not { } money || money.Account == amount) continue;
            money.Account = amount;
            Utilities.SetStateChanged(p, "CCSPlayerController", "m_pInGameMoneyServices");
        }
    }
}
