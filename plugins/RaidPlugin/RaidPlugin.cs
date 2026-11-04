using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
namespace Cs2Raid;

public sealed partial class RaidPlugin : BasePlugin
{
    public override string ModuleName => "CS2 Raid";
    public override string ModuleVersion => "0.1.0";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "7-player FFA extraction scaffold; map and money delivery fail closed";
    private RaidState raid = new();
    private ShopItem[] catalog = [];
    private readonly HttpClient http = new(new MoneyOffHandler()) { Timeout = TimeSpan.FromSeconds(15) };
    private Guid mapGeneration = Guid.NewGuid();
    private readonly HashSet<string> linking = [];
    public override void Load(bool hotReload)
    {
        LoadGameplay();
        RegisterTestMode();
        catalog = JsonSerializer.Deserialize<ShopItem[]>(File.ReadAllText(Path.Combine(ModuleDirectory, "prices.json")),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidOperationException("Missing prices");
        // Never launch a partially configured raid or resume an in-memory raid after reload.
        AddCommand("css_shop", "Show pre-raid shop", (p,c) => {
            if (p is not { IsValid: true, IsBot: false }) return;
            foreach (var i in catalog) p.PrintToChat($"{i.Id}: ${i.PriceDollars}");
            p.PrintToChat("Purchases disabled until verified native delivery bridge is installed.");
        });
        AddCommand("css_buy", "Pre-raid buy (fail closed scaffold)", (p,c) =>
            c.ReplyToCommand("No charge made. Native delivery/session bridge not implemented."));
        AddCommand("css_link", "SSG account linking (private)", (p,c) => BeginLink(p, false));
        AddCommand("css_linkpoll", "Check SSG link approval (private)", (p,c) => BeginLink(p, true));
        AddCommand("css_raid_status", "Raid status", (p,c) =>
            c.ReplyToCommand($"{raid.Phase}; {raid.Players.Count}/7; ends {raid.EndsAt}; extraction {raid.ExtractionAt}"));
        // Server console ONLY. No client can trigger levers, extraction or start by spoofing chat.
        AddCommand("css_raid_start", "Console-only start scaffold", (p,c) => {
            if (p != null) { c.ReplyToCommand("Server console only"); return; }
            if (TestServer) { c.ReplyToCommand("TEST SERVER: real raid disabled, MONEY HARD OFF."); return; }
            if (gameplayConfig.Enabled) { c.ReplyToCommand("Legacy start disabled in gameplay mode"); return; }
            if (Environment.GetEnvironmentVariable("RAID_ENABLE_SCAFFOLD") != "1") {
                c.ReplyToCommand("Disabled. Space City entity integration and gameplay QA required."); return;
            }
            raid = new RaidState();
            try {
                foreach (var human in Utilities.GetPlayers().Where(x => x.IsValid && !x.IsBot && x.AuthorizedSteamID != null))
                    raid.Join(human.SteamID.ToString());
                raid.Start(DateTimeOffset.UtcNow);
                Server.PrintToChatAll("Raid started. 30 minutes. No respawns, no kill bonus.");
            } catch (InvalidOperationException e) { c.ReplyToCommand(e.Message); raid = new RaidState(); }
        });
        AddCommand("css_raid_lever", "Console-only map lever test", (p,c) => {
            if (p != null || c.ArgCount != 2 || !int.TryParse(c.GetArg(1), out var n)) return;
            try { raid.PullLever(n, DateTimeOffset.UtcNow); c.ReplyToCommand($"Extraction at {raid.ExtractionAt}"); }
            catch (InvalidOperationException e) { c.ReplyToCommand(e.Message); }
        });
        RegisterEventHandler<EventPlayerDeath>((e, info) => {
            // Never retain native event/controller handles across awaits or frames.
            var victim = e.Userid;
            if (victim is { IsValid: true, IsBot: false } && raid.Players.TryGetValue(victim.SteamID.ToString(), out var state) && state.Alive && !state.Extracted)
                raid.Die(victim.SteamID.ToString());
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDisconnect>((e, info) => {
            var p = e.Userid;
            if (p is { IsValid: true } && raid.Players.TryGetValue(p.SteamID.ToString(), out var s) && s.Alive && !s.Extracted)
                raid.Die(p.SteamID.ToString());
            return HookResult.Continue;
        });
        RegisterListener<Listeners.OnMapStart>(_ => { raid = new RaidState(); mapGeneration = Guid.NewGuid(); linking.Clear(); });
        AddTimer(1.0f, () => raid.Tick(DateTimeOffset.UtcNow), TimerFlags.REPEAT);
    }
    private void BeginLink(CCSPlayerController? p, bool poll)
    {
        if(TestServer)return;
        if (p is not { IsValid: true, IsBot: false } || p.AuthorizedSteamID == null || p.UserId is not { } userId) return;
        var steam = p.SteamID.ToString();
        if (!linking.Add(steam)) { p.PrintToChat("A link request is already in progress."); return; }
        var generation = mapGeneration;
        _ = LinkAsync(steam, userId, generation, poll);
    }
    private async Task LinkAsync(string steam, int userId, Guid generation, bool poll)
    {
        if(TestServer)return;
        string message;
        try {
            var origin = Environment.GetEnvironmentVariable("RAID_BACKEND_URL");
            var token = Environment.GetEnvironmentVariable("BRIDGE_TOKEN") ?? "";
            if (origin != "http://backend:3000" || token.Length < 32) throw new InvalidOperationException("Bridge configuration");
            using var req = new HttpRequestMessage(HttpMethod.Post, origin + (poll ? "/link/poll" : "/link"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = JsonContent.Create(new { steamId64 = steam });
            using var response = await http.SendAsync(req);
            if (!response.IsSuccessStatusCode) {
                message = response.StatusCode == System.Net.HttpStatusCode.TooManyRequests
                    ? "Wait before polling again. Use !linkpoll after the required interval."
                    : "Link unavailable. Check approval/configuration with the server owner.";
            } else {
                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                if (poll) message = "SSG link state: " + doc.RootElement.GetProperty("state").GetString();
                else {
                    var words = doc.RootElement.GetProperty("user_code").GetString() ?? "";
                    var uri = doc.RootElement.GetProperty("verification_uri").GetString() ?? "";
                    // Never display upstream control sequences, markup or arbitrary URLs in chat.
                    if (!System.Text.RegularExpressions.Regex.IsMatch(words, @"^[a-z]+ [a-z]+ [a-z]+ [a-z]+$") ||
                        !Uri.TryCreate(uri, UriKind.Absolute, out var url) || url.Scheme != "https" || uri.Any(char.IsControl))
                        throw new InvalidOperationException("Invalid link response");
                    message = $"Your SSG code: {words}. Open {uri}, confirm, then !linkpoll (wait at least 5 seconds). Never share this code.";
                }
            }
        } catch { message = "Link request unavailable. No purchase or charge was made."; }
        // Reacquire controller on game thread. Never carry controller/event handles over await.
        Server.NextFrame(() => {
            if (generation != mapGeneration) return;
            linking.Remove(steam);
            var current = Utilities.GetPlayerFromUserid(userId);
            if (current is { IsValid: true, IsBot: false } && current.AuthorizedSteamID != null && current.SteamID.ToString() == steam)
                current.PrintToChat(message);
        });
    }
    public override void Unload(bool hotReload) {
        testUnloaded=true;mapGeneration=Guid.NewGuid();
        TestModeState.FinalizeAll(TestCleanupError,StopTestMode,StopDeltaOpsBridge,UnhookGameplay,StopGameplay,http.Dispose);
    }
    // TODO trusted map hooks call PullLever only on use within range, and Extract only
    // on living authorized player's zone entry. Persist bag + settlement outbox first.
    // TODO native pickup/drop tracking, corpse menu, strip default weapons, reconnect
    // generation fence and collision-free unlimited logical backpack equip adapter.
}
