using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;
using CounterStrikeSharp.API.Modules.Utils;

namespace Cs2Debug;

public sealed class DebugConfig : BasePluginConfig
{
    public bool Enabled { get; set; } = true;
    // Opt-in only for isolated LAN map tests. Never enable on a public server.
    public bool EveryoneIsAdmin { get; set; } = false;
    public string ReportDirectory { get; set; } = "/data/map-reports";
}

public sealed class DebugPlugin : BasePlugin, IPluginConfig<DebugConfig>
{
    public override string ModuleName => "Map Debug";
    public override string ModuleVersion => "0.1.11";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "Debug economy and durable map issue reports";
    public DebugConfig Config { get; set; } = new();
    private readonly Dictionary<ulong, ShotTracker> shots = [];
    private readonly ReportLimiter reportLimiter = new();
    private readonly ReportLimiter textureLimiter = new();
    private readonly ReportLimiter listLimiter = new();
    private readonly ReportLimiter resetLimiter = new();
    private readonly ReportLimiter bannerLimiter = new();
    private readonly ReportLimiter bannerResetLimiter = new();
    private readonly ReportLimiter mvpLimiter = new();
    private ReportStore? store;
    private CollisionStore? marks;
    private readonly Dictionary<ulong, (Point Pos, Point Angles, Point Eye)> firePoses = [];
    private readonly Dictionary<string, List<CBeam>> markerEntities = [];
    private readonly LinkedList<string> markerOrder = [];
    private readonly List<CBeam> extractBoxBeams = [];
    private const string Safe7Model="models/props/de_dust/hr_dust/dust_soccerball/dust_soccer_ball001.vmdl";
    private CDynamicProp? safe7Marker;
    private void ClearSafe7()
    {
        var marker=safe7Marker;safe7Marker=null;
        try { if(marker is {IsValid:true})marker.Remove(); }
        catch(Exception e) { Logger.LogError(e,"Safe7 marker cleanup failed"); }
    }
    private void Safe7Command(CCSPlayerController? p, CommandInfo c)
    {
        if(!active || !Config.EveryoneIsAdmin ||
           ConVar<int>.Find("hostport")?.Value!=27016 || ConVar<bool>.Find("sv_lan")?.Value!=true ||
           (Server.MapName!="df_spaceport_kat_linked" && Server.MapName!="df_spaceport_player_scale_raytrace"))
        { c.ReplyToCommand("Safe7 marker requires the private Spaceport LAN server on 27016."); return; }
        if(p!=null&&!Human(p))return;
        if(c.ArgCount>2 || (c.ArgCount==2 && c.GetArg(1) is not ("on" or "off")))
        { c.ReplyToCommand("Usage: !safe7 [on|off]");return; }
        ClearSafe7();
        if(c.ArgCount==2 && c.GetArg(1)=="off")
        { c.ReplyToCommand("Safe7 marker off.");return; }
        try {
            var e=Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic")
                ??throw new InvalidOperationException("prop_dynamic unavailable");
            safe7Marker=e;
            e.Entity!.Name="kat_debug_safe7_marker";
            e.SetModel(Safe7Model);
            e.Collision.SolidType=SolidType_t.SOLID_NONE;e.TakesDamage=false;
            e.Render=System.Drawing.Color.Yellow;
            e.Glow.GlowColorOverride=System.Drawing.Color.Yellow;
            e.Glow.GlowType=3;e.Glow.GlowRange=10000;e.Glow.Glowing=true;
            e.DispatchSpawn();
            e.Collision.SolidType=SolidType_t.SOLID_NONE;e.MoveType=MoveType_t.MOVETYPE_NONE;
            if(e.CBodyComponent?.SceneNode is {} node)node.Scale=3f;
            e.Teleport(new Vector(-196.56f,-767.88f,868.32f),new QAngle(0,0,0),new Vector(0,0,0));
            c.ReplyToCommand($"Safe7 marker on: valid={e.IsValid}, solid={e.Collision.SolidType}, model={e.CBodyComponent?.SceneNode?.GetSkeletonInstance().ModelState.ModelName}, position={e.AbsOrigin}. !safe7 off to remove.");
        } catch(Exception e) {
            ClearSafe7();Logger.LogError(e,"Safe7 marker failed");
            c.ReplyToCommand("Safe7 marker failed; partial entity removed.");
        }
    }

    private void ClearExtractBox()
    {
        var beams=extractBoxBeams.ToArray();extractBoxBeams.Clear();
        foreach(var beam in beams)
            try { if(beam.IsValid)beam.Remove(); }
            catch(Exception e) { Logger.LogError(e,"Extraction outline cleanup failed"); }
    }
    private void ExtractBoxCommand(CCSPlayerController? p, CommandInfo c)
    {
        // Console/RCON is permitted for this private visualization, never arbitrary remote servers.
        if (!active || !Config.EveryoneIsAdmin ||
            ConVar<int>.Find("hostport")?.Value != 27016 || ConVar<bool>.Find("sv_lan")?.Value != true)
        { c.ReplyToCommand("Extraction outlines require the private LAN debug server on 27016."); return; }
        if (p != null && !Human(p)) return;
        if (c.ArgCount > 2 || (c.ArgCount == 2 && c.GetArg(1) is not ("on" or "off")))
        { c.ReplyToCommand("Usage: !extractbox [on|off]"); return; }
        if (c.ArgCount == 2 && c.GetArg(1) == "off")
        { ClearExtractBox(); c.ReplyToCommand("Extraction outlines off."); return; }
        if (Server.MapName != "df_spaceport_kat_linked" && Server.MapName != "df_spaceport_player_scale_raytrace")
        { c.ReplyToCommand("Extraction outlines are only defined for Spaceport."); return; }
        ClearExtractBox();
        try {
            foreach(var line in ExtractBoxGeometry.Lines()) {
                var beam=Utilities.CreateEntityByName<CBeam>("beam") ?? throw new InvalidOperationException("beam unavailable");
                extractBoxBeams.Add(beam);
                beam.BeamType=BeamType_t.BEAM_POINTS;
                beam.Width=3;beam.EndWidth=3;
                beam.Render=line.Warning ? System.Drawing.Color.Red : System.Drawing.Color.Lime;
                beam.Teleport(new Vector(line.Start.X,line.Start.Y,line.Start.Z));
                beam.EndPos.X=line.End.X;beam.EndPos.Y=line.End.Y;beam.EndPos.Z=line.End.Z;
                beam.DispatchSpawn();
            }
            c.ReplyToCommand($"Extraction outlines on ({extractBoxBeams.Count} beams). Green extraction; red warning. !extractbox off to remove.");
        } catch(Exception e) {
            ClearExtractBox();Logger.LogError(e,"Extraction outline failed");
            c.ReplyToCommand("Extraction outline drawing failed; partial beams removed.");
        }
    }
    private void RemoveMarker(string id)
    {
        markerOrder.Remove(id);
        if (markerEntities.Remove(id, out var entities))
            foreach (var entity in entities) if (entity.IsValid) entity.Remove();
    }
    private void DrawMarker(CollisionMark mark)
    {
        RemoveMarker(mark.Id);
        // Bound engine entities without dropping any durable marks.
        while (markerEntities.Count >= 128) RemoveMarker(markerOrder.First!.Value);
        var entities = new List<CBeam>();
        markerEntities[mark.Id] = entities;
        markerOrder.AddLast(mark.Id);
        var p = mark.Position;
        for (var axis = 0; axis < 3; axis++)
        {
            var beam = Utilities.CreateEntityByName<CBeam>("beam") ?? throw new InvalidOperationException("beam unavailable");
            entities.Add(beam);
            beam.BeamType = BeamType_t.BEAM_POINTS;
            beam.Width = 2; beam.EndWidth = 2;
            beam.Render = System.Drawing.Color.Magenta;
            beam.Teleport(new Vector(p.X + (axis==0?-6:0), p.Y + (axis==1?-6:0), p.Z + (axis==2?-6:0)));
            beam.EndPos.X = p.X + (axis==0?6:0);
            beam.EndPos.Y = p.Y + (axis==1?6:0);
            beam.EndPos.Z = p.Z + (axis==2?6:0);
            beam.DispatchSpawn();
        }
    }
    private void RestoreMarkers()
    {
        if (!active || marks == null) return;
        foreach (var mark in marks.Active.Where(x => x.Map == Server.MapName).TakeLast(128))
            try { DrawMarker(mark); } catch (Exception e) { Logger.LogError(e, "Restore marker failed"); }
    }
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
        RegisterListener<Listeners.OnServerPrecacheResources>(m=>m.AddResource(Safe7Model));
        AddCommand("css_safe7", "Draw yellow Safe7 marker: on|off", Safe7Command);
        AddCommand("css_extractbox", "Draw green/red extraction outlines: on|off", ExtractBoxCommand);
        AddCommand("css_mvp", "Preview your private native round MVP panel", MvpCommand);
        AddCommand("css_bannerreset", "Reset your private native banner preview", BannerResetCommand);
        AddCommand("css_banner", "Private native banner: win|lose|draw|reset", SimpleBannerCommand);
        AddCommand("css_testwin", "Preview a private native round-win banner", (p, c) => BannerCommand(p, c, BannerResult.Win));
        AddCommand("css_testlose", "Preview a private native round-loss banner", (p, c) => BannerCommand(p, c, BannerResult.Lose));
        AddCommand("css_testdraw", "Preview a private native round-draw banner", (p, c) => BannerCommand(p, c, BannerResult.Draw));
        AddCommand("css_report", "Save a map issue at your current position", ReportCommand);
        AddCommand("css_reports", "List latest reports (1-20, default 5)", ReportsCommand);
        AddCommand("css_undo", "Undo your latest nocollide mark on this map", (p, c) =>
        {
            if (!Ready(p, c)) return;
            try {
                var removed = marks?.Undo(p!.SteamID.ToString(), Server.MapName);
                if (removed == null) { c.ReplyToCommand("No mark to undo."); return; }
                RemoveMarker(removed.Id);
                c.ReplyToCommand("Last nocollide mark removed.");
            } catch (Exception e) { Logger.LogError(e, "Undo failed"); c.ReplyToCommand("Undo NOT saved."); }
        });
        AddCommand("css_marks", "Count your active nocollide marks on this map", (p, c) =>
        {
            if (!Ready(p, c)) return;
            c.ReplyToCommand(marks == null ? "Mark storage unavailable." :
                $"Your nocollide marks on this map: {marks.Active.Count(x => x.SteamId == p!.SteamID.ToString() && x.Map == Server.MapName)}");
        });
        RegisterEventHandler<EventWeaponFire>((e, info) =>
        {
            if (active && Human(e.Userid))
            {
                Tracker(e.Userid!).Fire(e.Weapon, Server.TickCount);
                firePoses.Remove(e.Userid!.SteamID);
                if (TryPose(e.Userid!, out var pos, out var angles))
                {
                    var pawn = e.Userid!.PlayerPawn.Value!;
                    var offset = pawn.ViewOffset;
                    firePoses[e.Userid.SteamID] = (pos, angles, new Point(pos.X + offset.X, pos.Y + offset.Y, pos.Z + offset.Z));
                }
            }
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
            if (marks == null || !firePoses.Remove(p!.SteamID, out var pose))
            { p!.PrintToChat("Mark NOT saved: storage or shot pose unavailable."); return HookResult.Continue; }
            try
            {
                // Trace across the actual impact along its bullet direction, not the current crosshair.
                var dx = position.X - pose.Eye.X; var dy = position.Y - pose.Eye.Y; var dz = position.Z - pose.Eye.Z;
                var length = MathF.Sqrt(dx*dx + dy*dy + dz*dz);
                if (length < 0.01f) throw new InvalidOperationException("Invalid shot ray");
                dx /= length; dy /= length; dz /= length;
                var hit = Trace.TraceEndShape(new Vector(position.X-dx*4, position.Y-dy*4, position.Z-dz*4),
                    new Vector(position.X+dx*4, position.Y+dy*4, position.Z+dz*4), p.PlayerPawn.Value);
                var n = hit.Normal;
                var normal = new Point(n.X, n.Y, n.Z);
                if (!hit.DidHit() || hit.IsAllSolid || !normal.IsFinite ||
                    n.X*n.X+n.Y*n.Y+n.Z*n.Z < 0.5f)
                    throw new InvalidOperationException("Surface normal unavailable");
                var mark = new CollisionMark(Guid.NewGuid().ToString("N"), DateTimeOffset.UtcNow,
                    "nocollide", Server.MapName, p.SteamID.ToString(), position, normal, pose.Pos, pose.Angles);
                marks.Add(mark);
                p.PrintToChat("nocollide mark saved.");
                try { DrawMarker(mark); }
                catch (Exception ex) { Logger.LogError(ex, "Marker visual failed"); p.PrintToChat("Saved, but marker drawing failed."); }
            }
            catch (Exception ex) { Logger.LogError(ex, "nocollide mark failed"); p.PrintToChat("Mark NOT saved: storage/normal unavailable."); }
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
        RegisterListener<Listeners.OnMapStart>(_ => { ClearState(); if (active) Server.NextFrame(() => { ApplyCvars(); RestoreMarkers(); }); });
        RegisterListener<Listeners.OnTick>(Tick);
        Reconfigure();
    }
    public override void Unload(bool hotReload)
    {
        RevokeTestAdmins();
        active = false; loaded = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null; marks?.Dispose(); marks = null;
    }
    private void Reconfigure()
    {
        RevokeTestAdmins();
        active = false;
        RestoreCvars(); ClearState(); store?.Dispose(); store = null; marks?.Dispose(); marks = null;
        if (!Config.Enabled) return;
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
        try { marks = new CollisionStore(Config.ReportDirectory); }
        catch (Exception e) { Logger.LogError(e, "nocollide storage unavailable"); }
        active = true;
        RestoreMarkers();
        ApplyCvars();
    }
    private void ClearState()
    {
        ClearExtractBox();
        ClearSafe7();
        foreach (var id in markerEntities.Keys.ToArray()) RemoveMarker(id);
        firePoses.Clear();
        shots.Clear(); reportLimiter.Clear(); textureLimiter.Clear(); listLimiter.Clear(); resetLimiter.Clear(); bannerLimiter.Clear(); bannerResetLimiter.Clear(); mvpLimiter.Clear();
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
        if (grantedAdmins.Remove(id)) AdminManager.RemovePlayerPermissions(new SteamID(id), "@css/root");
        shots.Remove(id); firePoses.Remove(id); reportLimiter.Remove(id);
        textureLimiter.Remove(id); listLimiter.Remove(id); resetLimiter.Remove(id); bannerLimiter.Remove(id); bannerResetLimiter.Remove(id); mvpLimiter.Remove(id);
    }
    private bool Ready(CCSPlayerController? p, CommandInfo c)
    {
        if (!active) { c.ReplyToCommand("Map Debug is disabled."); return false; }
        if (!Human(p)) { c.ReplyToCommand("A connected human player is required."); return false; }
        return true;
    }
    private void MvpCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (c.ArgCount != 1)
        { c.ReplyToCommand("Usage: !mvp"); return; }
        if (!RoundBanner.TryCreate(p!.TeamNum, BannerResult.Win, out var banner))
        { c.ReplyToCommand("Join T or CT before testing an MVP panel."); return; }
        if (!mvpLimiter.Allow(p.SteamID, Environment.TickCount64, 3000))
        { c.ReplyToCommand("Wait three seconds between MVP previews."); return; }
        try
        {
            // Snapshot real read-only values. Never award an MVP or modify inventory/stats.
            var kills = Math.Max(0, p.ActionTrackingServices?.NumRoundKills ?? 0);
            var musicId = Math.Max(0, p.MusicKitID);
            var musicMvps = Math.Max(0, p.MusicKitMVPs);
            var noMusic = p.MvpNoMusic ? 1 : 0;
            // Same native cosmetic context as !banner win, private to this connection.
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
            finally { end.Free(); }
            var mvp = new EventRoundMvp(true);
            try
            {
                mvp.Userid = p;
                // 1 = kills; 7 = generic winner (Valve HudWinPanel._SetMVP).
                // Do not invent kills when testing alone with bots disabled.
                mvp.Reason = kills > 0 ? 1 : 7;
                mvp.Value = kills;
                mvp.Musickitid = musicId;
                mvp.Musickitmvps = musicMvps;
                mvp.Nomusic = noMusic;
                mvp.FireEventToClient(p);
            }
            finally { mvp.Free(); }
            var panel = new EventCsWinPanelRound(true);
            try
            {
                panel.FinalEvent = banner.PanelFinalEvent;
                panel.FunfactToken = "";
                panel.ShowTimerAttack = false;
                panel.ShowTimerDefend = false;
                panel.TimerTime = 0;
                panel.FireEventToClient(p);
            }
            finally { panel.Free(); }
            c.ReplyToCommand("Private MVP preview notification sent. Card/music depend on the client and equipped kit; no real MVP awarded. Use !banner reset to request cleanup.");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Private MVP preview failed.");
            c.ReplyToCommand("MVP preview failed; use !banner reset if partially displayed and check the Debug plugin log.");
        }
    }
    private void BannerResetCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (c.ArgCount != 1)
        { c.ReplyToCommand("Usage: !bannerreset (or !banner reset)"); return; }
        ResetBanner(p!, c);
    }
    private void ResetBanner(CCSPlayerController p, CommandInfo c)
    {
        if (!bannerResetLimiter.Allow(p!.SteamID, Environment.TickCount64, 1000))
        { c.ReplyToCommand("Wait one second between banner resets."); return; }
        try
        {
            // Cosmetic client notification ONLY, not a real round start.
            // CSS delivers directly to this player's legacy event listener, bypassing
            // the server event manager and all server/plugin round-start handlers.
            // No invented event fields and no replicated round-state writes.
            var reset = new EventRoundStart(true);
            try { reset.FireEventToClient(p); }
            finally { reset.Free(); }
            c.ReplyToCommand("Private banner/MVP reset notification sent. Visual/audio cleanup depends on the client; the real round is unchanged.");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Private banner reset failed.");
            c.ReplyToCommand("Banner reset failed; check the Debug plugin log.");
        }
    }
    private void SimpleBannerCommand(CCSPlayerController? p, CommandInfo c)
    {
        if (!Ready(p, c)) return;
        if (c.ArgCount != 2)
        { c.ReplyToCommand("Usage: !banner <win|lose|draw|reset>"); return; }
        var action = c.GetArg(1);
        if (action.Equals("reset", StringComparison.OrdinalIgnoreCase))
        { ResetBanner(p!, c); return; }
        if (!BannerInput.TryParse(c.ArgCount - 1, action, out var result))
        { c.ReplyToCommand("Usage: !banner <win|lose|draw|reset>"); return; }
        BannerCommand(p, c, result);
    }
    private void BannerCommand(CCSPlayerController? p, CommandInfo c, BannerResult result)
    {
        if (!Ready(p, c)) return;
        if (!RoundBanner.TryCreate(p!.TeamNum, result, out var banner))
        { c.ReplyToCommand("Join T or CT before testing a round banner."); return; }
        if (!bannerLimiter.Allow(p.SteamID, Environment.TickCount64, 3000))
        { c.ReplyToCommand("Wait three seconds between banner previews."); return; }
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
