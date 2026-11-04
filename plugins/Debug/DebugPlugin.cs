using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;

namespace Cs2Debug;

public sealed class DebugConfig : BasePluginConfig
{
    public bool Enabled { get; set; } = true;
    // Operator asset attestation and exclusive center-channel approval, never inferred from map filename.
    public bool ExtractHudOnly {get;set;}
    public bool ExtractHudVerifiedNoRaid {get;set;}
    public string ExtractHudPrivateTarget {get;set;}="";
    public string ExtractHudRelease {get;set;}="";
    public bool ExtractHudExclusive {get;set;}

    // Opt-in only for isolated LAN map tests. Never enable on a public server.
    public bool EveryoneIsAdmin { get; set; } = false;
    public string ReportDirectory { get; set; } = "/data/map-reports";
}

public sealed partial class DebugPlugin : BasePlugin, IPluginConfig<DebugConfig>
{
    public override string ModuleName => "Map Debug";
    public override string ModuleVersion => "0.1.4-hudonly.7";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "Debug economy and durable map issue reports";
    public DebugConfig Config { get; set; } = new();
    private readonly Dictionary<ulong, ShotTracker> shots = [];
    private readonly ReportLimiter reportLimiter = new();
    private readonly ReportLimiter textureLimiter = new();
    private readonly ReportLimiter listLimiter = new();
    private readonly ReportLimiter resetLimiter = new();
    private readonly ReportLimiter bannerLimiter = new();
    private ReportStore? store;
    private readonly HashSet<ulong> grantedAdmins = [];
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
        AddCommand("css_extracthud", "Private probe: !extracthud [seconds] [css|png] or stop", ExtractHudCommand);
        AddCommand("css_banner", "Private native banner: win|lose|tie gold|blue|red|gray", ColoredBannerCommand);
        AddCommand("css_testwin", "Preview a private native round-win banner", (p, c) => BannerCommand(p, c, BannerResult.Win));
        AddCommand("css_testlose", "Preview a private native round-loss banner", (p, c) => BannerCommand(p, c, BannerResult.Lose));
        AddCommand("css_testdraw", "Preview a private native round-draw banner", (p, c) => BannerCommand(p, c, BannerResult.Draw));
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
        RegisterListener<Listeners.OnMapEnd>(() => { previewNative.Stop(); ClearState(); RestoreCvars(); });
        RegisterListener<Listeners.OnMapStart>(_ => { previewNative.Stop(); ClearState(); if (active) Server.NextFrame(ApplyCvars); });
        RegisterListener<Listeners.OnTick>(Tick);
        Reconfigure();
    }
    public override void Unload(bool hotReload)
    {
        previewNative.Stop(); // Global availability is not guaranteed during error-unload/teardown.
        RevokeTestAdmins();
        active = false; loaded = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null;
    }
    private void Reconfigure()
    {
        RevokeTestAdmins();
        active = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null;
        // HUD-only never reaches legacy activation, report store, cvars, admin grants or money tick.
        if (Config.ExtractHudOnly) { if(!RaidInactiveForHud())ClearPreviews(); return; }
        if (!PreviewActivation.Legacy(Config.Enabled,Config.ExtractHudOnly)) return;
        // Debug's unlimited-money/Glock markers and gameplay are mutually exclusive.
        var raidConfig = Path.Combine(ModuleDirectory, "..", "RaidPlugin", "gameplay.json");
        if (File.Exists(raidConfig))
        {
            try {
                using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(raidConfig));
                if (doc.RootElement.EnumerateObject().Any(p => p.Name.Equals("enabled", StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind == System.Text.Json.JsonValueKind.True))
                { Logger.LogInformation("Map Debug disabled while Raid gameplay is enabled. Reload both plugins after mode changes."); return; }
            } catch (Exception e) { Logger.LogError("Map Debug disabled: cannot read gameplay mode ({Failure}).", e.GetType().Name); return; }
        }
        store = new ReportStore(Config.ReportDirectory);
        if (!store.Available)
            Logger.LogError("Map Debug reports unavailable ({Failure}); check configured directory permissions. No reports will be acknowledged.", store.Failure);
        active = true;
        ApplyCvars();
    }
    private void ClearState()
    {
        ClearPreviews();
        shots.Clear(); reportLimiter.Clear(); textureLimiter.Clear(); listLimiter.Clear(); resetLimiter.Clear(); bannerLimiter.Clear();
    }
    private static bool Human(CCSPlayerController? p) => p is { IsValid: true, IsBot: false } && p.SteamID != 0;
    private ShotTracker Tracker(CCSPlayerController p)
    {
        if (!shots.TryGetValue(p.SteamID, out var tracker)) shots[p.SteamID] = tracker = new ShotTracker();
        return tracker;
    }
    private void Forget(CCSPlayerController? p)
    {
        if(p is {IsValid:true})StopPreview(p);
        if (p is not { IsValid: true }) return;
        ForgetId(p.SteamID);
    }
    private void ForgetId(ulong id)
    {
        if (grantedAdmins.Remove(id)) AdminManager.RemovePlayerPermissions(new SteamID(id), "@css/root");
        shots.Remove(id); reportLimiter.Remove(id);
        textureLimiter.Remove(id); listLimiter.Remove(id); resetLimiter.Remove(id); bannerLimiter.Remove(id);
    }
    private bool Ready(CCSPlayerController? p, CommandInfo c)
    {
        if (!active) { c.ReplyToCommand("Map Debug is disabled."); return false; }
        if (!Human(p)) { c.ReplyToCommand("A connected human player is required."); return false; }
        return true;
    }
    private void ColoredBannerCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (c.ArgCount != 3 || !BannerInput.TryParse(c.GetArg(1), c.GetArg(2), out var result))
        { c.ReplyToCommand("Usage: !banner <win|lose|tie> <gold|blue|red|gray>"); return; }
        BannerCommand(p, c, result, c.GetArg(2));
    }
    private void BannerCommand(CCSPlayerController? p, CommandInfo c, BannerResult result, string? requestedColor = null)
    {
        if (!Ready(p, c)) return;
        if (!RoundBanner.TryCreate(p!.TeamNum, result, out var banner))
        { c.ReplyToCommand("Join T or CT before testing a round banner."); return; }
        if (!bannerLimiter.Allow(p.SteamID, Environment.TickCount64, 3000))
        { c.ReplyToCommand("Wait three seconds between banner previews."); return; }
        if (requestedColor != null && !BannerInput.Matches(p.TeamNum, result, requestedColor))
            c.ReplyToCommand($"Native banner cannot show {requestedColor.ToLowerInvariant()} for this result/team; showing {BannerInput.NativeColor(p.TeamNum, result)} instead. Gold requires a T win, blue a CT win, red a loss, gray a tie.");
        try
        {
            // Direct client delivery only: never FireEvent(), TerminateRound(), or change rules.
            var end = new EventRoundEnd(true);
            try
            {
                end.Winner = banner.Winner;
                end.Reason = banner.Reason;
                end.Message = banner.Message;
                end.Nomusic = 1;
                end.PlayerCount = 1;
                end.Time = 0;
                end.Legacy = 0;
                end.FireEventToClient(p);
            }
            finally { end.Free(); } // FireEventToClient does NOT own/free the event.
            var panel = new EventCsWinPanelRound(true);
            try
            {
                panel.FinalEvent = banner.PanelFinalEvent; // RoundEndReason, NOT a match-final boolean.
                panel.FunfactToken = "";
                panel.ShowTimerAttack = false;
                panel.ShowTimerDefend = false;
                panel.TimerTime = 0;
                panel.FireEventToClient(p);
            }
            finally { panel.Free(); }
            c.ReplyToCommand($"Private {result.ToString().ToLowerInvariant()} preview sent. The real round is unchanged.");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Private round banner preview failed.");
            c.ReplyToCommand("Banner preview failed; check the Debug plugin log.");
        }
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
    private void RevokeTestAdmins()
    {
        foreach (var id in grantedAdmins)
            AdminManager.RemovePlayerPermissions(new SteamID(id), "@css/root");
        grantedAdmins.Clear();
    }
    private void MaintainTestAdmin(CCSPlayerController p)
    {
        if (!Config.EveryoneIsAdmin || p.AuthorizedSteamID == null ||
            AdminManager.PlayerHasPermissions(p, "@css/root")) return;
        AdminManager.AddPlayerPermissions(p, "@css/root");
        if (AdminManager.PlayerHasPermissions(p, "@css/root"))
            grantedAdmins.Add(p.SteamID);
    }
    private void Tick()
    {
        TickPreviews();
        if (!active) return;
        ApplyCvars(); // also defeats a late +exec config on the very next tick
        if (!active || maxMoney == null) return;
        var amount = Math.Max(maxMoney.Value, 0);
        foreach (var p in Utilities.GetPlayers())
        {
            if (!Human(p)) continue;
            MaintainTestAdmin(p);
            if (p.InGameMoneyServices is not { } money || money.Account == amount) continue;
            money.Account = amount;
            Utilities.SetStateChanged(p, "CCSPlayerController", "m_pInGameMoneyServices");
        }
    }
}
