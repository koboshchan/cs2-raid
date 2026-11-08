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
    public override string ModuleVersion => "0.2.0-private-flow";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "7-player FFA extraction scaffold; map and money delivery fail closed";
    private RaidState raid = new();
    private ShopItem[] catalog = [];
    private readonly HttpClient http = new(new MoneyOffHandler()) { Timeout = TimeSpan.FromSeconds(15) };
    private Guid mapGeneration = Guid.NewGuid();
    private readonly HashSet<string> linking = []; // Retained for frozen fixture preflight.
    private PrivateLink? privateLinks;
    private readonly HttpClient linkHttp=new(new LinkOnlyHandler()) {Timeout=Timeout.InfiniteTimeSpan};
    public override void Load(bool hotReload)
    {
        if(LoadPrivateRehearsal())return;
        privateLinks=new PrivateLink(linkHttp,(session,message)=>privateLinks?.Schedule(session,action=>Server.NextFrame(action),()=>{
            if(privateLinks?.Current(session)!=true||session.Generation!=mapGeneration||testUnloaded)return;
            var current=Utilities.GetPlayers().FirstOrDefault(p=>p.IsValid&&p.EntityHandle.Raw==session.Raw);
            if(current is {IsValid:true,IsBot:false}&&current.AuthorizedSteamID!=null&&current.SteamID.ToString()==session.Steam)
                current.PrintToChat(message);
        }));
        RegisterEventHandler<EventPlayerConnectFull>((e,i)=>{
            PrivateLink.NativeSafe(()=>BeginLink(e.Userid,true));return HookResult.Continue;
        });
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
        AddCommand("css_linkpoll", "Check SSG link approval (private)", (p,c) => BeginLink(p, false));
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
            PrivateLink.NativeSafe(()=>{
            var p = e.Userid;
            if(p is {IsValid:true})privateLinks?.Disconnect(p.EntityHandle.Raw);
            if (p is { IsValid: true } && raid.Players.TryGetValue(p.SteamID.ToString(), out var s) && s.Alive && !s.Extracted)
                raid.Die(p.SteamID.ToString());
            });
            return HookResult.Continue;
        });
        RegisterListener<Listeners.OnMapEnd>(()=>privateLinks?.EndMap(()=>mapGeneration=Guid.NewGuid()));
        RegisterListener<Listeners.OnMapStart>(_ => { raid = new RaidState(); mapGeneration = Guid.NewGuid(); linking.Clear(); privateLinks?.Reset(); });
        AddTimer(1.0f, () => raid.Tick(DateTimeOffset.UtcNow), TimerFlags.REPEAT);
    }
    private void BeginLink(CCSPlayerController? p, bool join)
    {
        PrivateLink.NativeSafe(()=>{
            if(testUnloaded || p is not {IsValid:true,IsBot:false} || p.AuthorizedSteamID==null)return;
            privateLinks?.Start(p.SteamID.ToString(),p.EntityHandle.Raw,mapGeneration,join);
        });
    }
    public override void Unload(bool hotReload) {
        if(privateRehearsal){StopPrivateRehearsal();http.Dispose();linkHttp.Dispose();return;}
        privateLinks?.Dispose();linkHttp.Dispose();
        ResetFixtureDeltaOps();StopFixtureDeltaOpsBridge();
        testUnloaded=true;mapGeneration=Guid.NewGuid();
        TestModeState.FinalizeAll(TestCleanupError,StopTestMode,StopDeltaOpsBridge,UnhookGameplay,StopGameplay,()=>settlementRecovery?.Dispose(),http.Dispose);
    }
    // TODO trusted map hooks call PullLever only on use within range, and Extract only
    // on living authorized player's zone entry. Persist bag + settlement outbox first.
    // TODO native pickup/drop tracking, corpse menu, strip default weapons, reconnect
    // generation fence and collision-free unlimited logical backpack equip adapter.
}
