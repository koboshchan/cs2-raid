using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Timers;
namespace Cs2Raid;

// This adapter is intentionally separate from financial/native-QA gameplay.
// No financial, account-link, DeltaOps or settlement callbacks are registered.
public sealed partial class RaidPlugin
{
    private const string PrivateFlowLease="/tmp/kat-private-raid-flow-active";
    private bool privateRehearsal,privateStopping;
    private int privateEpoch;
    private DateTimeOffset privateClosedAt;
    private readonly Dictionary<string,Position> privateOrigins=[];
    private readonly HashSet<string> privateSpectators=[];
    private sealed record PrivateSettings(bool Enabled=false);
    private static bool PrivateIdentity() {
        var a=File.ReadAllText("/proc/1/cmdline").Split('\0');var i=Array.IndexOf(a,"-port");
        return a.Contains("-dedicated")&&i>=0&&i+1<a.Length&&a[i+1]=="27016"&&
            ConVar<int>.Find("hostport")?.Value==27016;
    }
    private bool LoadPrivateRehearsal() {
        var path=Path.Combine(ModuleDirectory,"private-rehearsal.json");
        if(!File.Exists(path)||JsonSerializer.Deserialize<PrivateSettings>(File.ReadAllText(path),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})?.Enabled!=true)return false;
        if(TestServer)throw new InvalidOperationException("Private rehearsal requires RAID_TEST_SERVER=0; native economy is unavailable in isolated testmode.");
        if(!PrivateIdentity())throw new InvalidOperationException("Rehearsal refused outside dedicated private27016.");
        privateRehearsal=true;MoneyOffHandler.PrivateRehearsalHardOff=true;
        AddCommand("css_raid_status","Local private rehearsal status",(p,c)=>c.ReplyToCommand($"PRIVATE REHEARSAL | map={Server.MapName} loaded={sessionLoaded} phase={game.Phase} players={game.Players.Count}/7 buySeconds={GameplayState.BuySeconds} ends={game.EndsAt:O} extraction={game.ExtractionAt:O} HTTP=HARD_OFF; kits=OFF; no payouts"));
        AddCommand("css_team","Ready solo with !team single",(p,c)=>{if(sessionLoaded&&Human(p)&&c.ArgString.Trim().Equals("single",StringComparison.OrdinalIgnoreCase)){if(game.Solo(Id(p)))p.PrintToChat("Ready solo. All humans must ready before buying.");}});
        AddCommand("css_cancelteam","Cancel lobby team",(p,c)=>{if(sessionLoaded&&Human(p))game.Cancel(Id(p));});
        AddCommand("css_cancel","Cancel lobby team",(p,c)=>{if(sessionLoaded&&Human(p)&&c.ArgString.Trim().Equals("team",StringComparison.OrdinalIgnoreCase))game.Cancel(Id(p));});
        AddCommand("css_raid_rehearsal_reset","Private console-only return to lobby",(p,c)=>{if(p==null){PrivateReset();c.ReplyToCommand("Local rehearsal reset; money hard OFF.");}});
        AddCommand("css_raid_rehearsal_lever","Private console-only extraction lever 1..4",(p,c)=>{
            if(p!=null||!sessionLoaded||c.ArgCount!=2||!int.TryParse(c.GetArg(1),out var n))return;
            var id=game.Players.Keys.FirstOrDefault(game.Living);
            c.ReplyToCommand(id!=null&&game.PullLever(id,n,DateTimeOffset.UtcNow)?$"Lever {n}; extraction={game.ExtractionAt:O}":"Lever refused. Requires active raid and two distinct lever numbers.");
        });
        AddCommand("css_cashout","Real money disabled",(p,c)=>c.ReplyToCommand("PRIVATE REHEARSAL: no deposits, payouts or SSG routes."));
        AddCommand("css_link","Link disabled",(p,c)=>c.ReplyToCommand("PRIVATE REHEARSAL: account linking/HTTP disabled."));
        AddCommandListener("jointeam",(p,c)=>{if(!sessionLoaded||!Human(p))return HookResult.Continue;if(game.Phase==GamePhase.Lobby&&c.ArgCount==2&&int.TryParse(c.GetArg(1),out var side)&&side is 2 or 3)p.SwitchTeam((CsTeam)side);return HookResult.Handled;});
        foreach(var cmd in new[]{"joinclass","autobuy","rebuy"})AddCommandListener(cmd,(p,c)=>sessionLoaded&&Human(p)?HookResult.Handled:HookResult.Continue);
        AddCommandListener("buy",(p,c)=>sessionLoaded&&Human(p)&&(!game.BuyOpen(DateTimeOffset.UtcNow)||!game.Living(Id(p)))?HookResult.Handled:HookResult.Continue);
        AddCommandListener("buyammo1",(p,c)=>sessionLoaded&&Human(p)&&!game.BuyOpen(DateTimeOffset.UtcNow)?HookResult.Handled:HookResult.Continue);
        AddCommandListener("buyammo2",(p,c)=>sessionLoaded&&Human(p)&&!game.BuyOpen(DateTimeOffset.UtcNow)?HookResult.Handled:HookResult.Continue);
        RegisterListener<Listeners.OnEntityTakeDamagePre>((entity,info)=>{
            if(!sessionLoaded)return HookResult.Continue;
            var victim=Controller(entity);if(victim==null)return HookResult.Continue;
            var attacker=Controller(info.Attacker.Value);
            if(game.Phase==GamePhase.Lobby){
                if(attacker!=null&&info.Damage>0&&(info.BitsDamageType&DamageTypes_t.DMG_BULLET)!=0&&game.ShootRequest(Id(attacker),Id(victim)))Server.PrintToChatAll(SafeName(attacker.PlayerName)+" + "+SafeName(victim.PlayerName)+" ready as duo.");
                return HookResult.Handled;
            }
            return game.Phase!=GamePhase.Active||!game.Living(Id(victim))||attacker!=null&&!game.CanDamage(Id(attacker),Id(victim))?HookResult.Handled:HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{if(sessionLoaded&&Human(e.Userid)){game.Leave(Id(e.Userid));privateOrigins.Remove(Id(e.Userid));}return HookResult.Continue;},HookMode.Pre);
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(sessionLoaded&&Human(e.Userid)&&game.Kill(Id(e.Userid),null)){
                if(Location(e.Userid) is {} pos)deathPositions[Id(e.Userid)]=pos;
                raidOutcomes.Record(Id(e.Userid),false,DateTimeOffset.UtcNow);
                e.Userid.PrintToChat("Eliminated. Partner camera or freecam in 5 seconds.");
            }return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(Human(e.Userid)){var id=Id(e.Userid);var epoch=privateEpoch;Server.NextFrame(()=>{
                if(epoch!=privateEpoch||!sessionLoaded||Find(id) is not {} p)return;
                if(game.Phase==GamePhase.Lobby){if(!game.Players.ContainsKey(id))game.Join(id);PrivatePrepare(p);}
                else if(game.Living(id))SpawnHealth(p);
                else p.ChangeTeam(CsTeam.Spectator);
            });}return HookResult.Continue;
        });
        RegisterListener<Listeners.OnTick>(PrivateTick);
        RegisterListener<Listeners.OnMapEnd>(()=>{PrivateStopRound();privateEpoch++;});
        RegisterListener<Listeners.OnMapStart>(_=>{var epoch=++privateEpoch;AddTimer(4,()=>{if(epoch==privateEpoch)PrivateReset();},TimerFlags.STOP_ON_MAPCHANGE);});
        AddTimer(1,PrivateSecond,TimerFlags.REPEAT);
        File.Delete("/tmp/kat-private-raid-flow-debug-ready");
        File.WriteAllText(PrivateFlowLease,Server.MapName);
        AddTimer(2,PrivateReset,TimerFlags.STOP_ON_MAPCHANGE);
        return true;
    }
    private Position[] NativePrivateSpawns()=>Utilities.FindAllEntitiesByDesignerName<SpawnPoint>("info_player_terrorist")
        .Concat(Utilities.FindAllEntitiesByDesignerName<SpawnPoint>("info_player_counterterrorist"))
        .Where(e=>e.IsValid&&e.Enabled&&e.AbsOrigin!=null).Select(e=>new Position(e.AbsOrigin!.X,e.AbsOrigin.Y,e.AbsOrigin.Z+4)).ToArray();
    private void PrivateReset(){
        PrivateStopRound(true);privateEpoch++;privateStopping=false;
        if(!PrivateIdentity()||Server.MapName!="df_spaceport_v261")return;
        var ack="/tmp/kat-private-raid-flow-debug-ready";
        if(!File.Exists(ack)||File.ReadAllText(ack)!=Server.MapName||(DateTime.UtcNow-File.GetLastWriteTimeUtc(ack)).TotalSeconds>10){PrivateStopRound();Server.PrintToConsole("PRIVATE RAID refused: cooperative Debug economy acknowledgement missing.");return;}
        var spawns=NativePrivateSpawns();if(spawns.Length!=32){Server.PrintToConsole("PRIVATE RAID refuses startup without existing 32 native spawns; helper untouched.");return;}
        File.WriteAllText(PrivateFlowLease,Server.MapName);
        game=new();raidOutcomes=new();privateSpectators.Clear();deathPositions.Clear();roamingPlaced.Clear();nativeProjections.Clear();
        try{
            CaptureBuyValue(raidBuyBaselines,"mp_autoteambalance",false);CaptureBuyValue(raidBuyBaselines,"mp_limitteams",0);
            CaptureBuyValue(raidBuyBaselines,"mp_ignore_round_win_conditions",true);CaptureBuyValue(raidBuyBaselines,"mp_teammates_are_enemies",true);
            CaptureBuyValue(raidBuyBaselines,"mp_friendlyfire",true);CaptureBuyValue(raidBuyBaselines,"mp_respawn_on_death_t",false);CaptureBuyValue(raidBuyBaselines,"mp_respawn_on_death_ct",false);
            CaptureBuyValue(raidBuyBaselines,"mp_startmoney",0);CaptureBuyValue(raidBuyBaselines,"mp_playercashawards",false);CaptureBuyValue(raidBuyBaselines,"mp_teamcashawards",false);
            CaptureBuyValue(raidBuyBaselines,"mp_maxmoney",65535);CaptureBuyValue(raidBuyBaselines,"mp_buy_anywhere",0);SetNativeBuyTime(raidBuyBaselines,0);
            sessionLoaded=true;
            foreach(var p in Utilities.GetPlayers().Where(Human)){if(game.Join(Id(p)))PrivatePrepare(p);else p.ChangeTeam(CsTeam.Spectator);}
            Server.PrintToChatAll("Private raid rehearsal. Shoot each other with Glocks for a duo, or !team single. Native buys use 15000 LOCAL test credits. No SSG money or character kits.");
        }catch(Exception e){PrivateStopRound();Server.PrintToConsole("PRIVATE RAID startup refused: "+e.Message);}
    }
    private void PrivatePrepare(CCSPlayerController p){
        if(!sessionLoaded||!game.Players.ContainsKey(Id(p)))return;
        if(p.TeamNum is not (2 or 3))p.SwitchTeam(CsTeam.Terrorist);
        if(!p.PawnIsAlive){p.Respawn();return;}
        if(Location(p) is {} pos)privateOrigins.TryAdd(Id(p),pos);
        p.RemoveWeapons();p.GiveNamedItem("weapon_glock");p.GiveNamedItem("weapon_knife");SpawnHealth(p);SetPresentation(p,false,false);
        if(p.PlayerPawn.Value is {} pawn){pawn.ArmorValue=0;if(pawn.ItemServices is {} items)items.As<CCSPlayer_ItemServices>().HasHelmet=false;}
        SetMoney(p,0);
    }
    private void PrivatePhase(){
        foreach(var p in Utilities.GetPlayers().Where(Human))if(game.Living(Id(p)))SetPresentation(p,game.Phase is GamePhase.Countdown or GamePhase.Freeze,false);
        if(game.Phase==GamePhase.Buy){
            foreach(var m in game.Players.Values){m.Cash=15000;if(Find(m.Id) is {} p)SetMoney(p,m.Cash);}
            SyncRaidBuyClock();CaptureBuyValue(raidBuyBaselines,"mp_buy_anywhere",1);
            Server.PrintToChatAll("Buy phase 45s. Press B, native prices. LOCAL test credits only; no character abilities.");
        }
        if(game.Phase==GamePhase.Countdown){RestoreRaidBuyRules();SetNativeBuyTime(raidBuyBaselines,0);CaptureBuyValue(raidBuyBaselines,"mp_buy_anywhere",0);}
        if(game.Phase==GamePhase.Freeze){
            // Use the helper's real validated points, not invented partner offsets.
            var remaining=NativePrivateSpawns().OrderBy(_=>random.Next()).ToList();
            foreach(var team in game.Players.Values.GroupBy(m=>m.Team)){
                if(remaining.Count<team.Count())throw new InvalidOperationException("Insufficient real spawn points.");
                var first=remaining[0];var pair=remaining.OrderBy(p=>Math.Pow(p.X-first.X,2)+Math.Pow(p.Y-first.Y,2)).Take(team.Count()).ToArray();
                var j=0;foreach(var m in team){if(Find(m.Id) is {} p){Place(p,pair[j++]);SpawnHealth(p);}}
                remaining.RemoveAll(p=>p.Within(first,512));
            }
        }
        if(game.Phase==GamePhase.Active)Server.PrintToChatAll("Raid active. 200HP, no respawn. Extraction timer requires two distinct console test levers; physical levers not bound yet.");
        if(game.Phase==GamePhase.Closed){privateClosedAt=DateTimeOffset.UtcNow;foreach(var m in game.Players.Values.Where(m=>!m.Extracted))if(Find(m.Id) is {} p){raidOutcomes.Record(m.Id,false,privateClosedAt);p.RemoveWeapons();p.ChangeTeam(CsTeam.Spectator);}}
    }
    private void PrivateSecond(){
        try{PrivateSecondCore();}catch(Exception e){PrivateStopRound();Server.PrintToConsole("PRIVATE RAID fail-closed: "+e.Message);}
    }
    private void PrivateSecondCore(){
        if(privateStopping)return;
        if(Server.MapName!="df_spaceport_v261"){if(sessionLoaded)PrivateStopRound();return;}
        if(!sessionLoaded)return;
        File.WriteAllText(PrivateFlowLease,Server.MapName);
        var now=DateTimeOffset.UtcNow;
        foreach(var p in Utilities.GetPlayers().Where(Human)){
            if(game.Phase==GamePhase.Lobby&&!game.Players.ContainsKey(Id(p))){if(game.Join(Id(p)))PrivatePrepare(p);else p.ChangeTeam(CsTeam.Spectator);}
            else if(!game.Players.ContainsKey(Id(p))&&p.TeamNum!=(int)CsTeam.Spectator)p.ChangeTeam(CsTeam.Spectator);
        }
        var previous=game.Phase;
        if(game.Phase==GamePhase.Lobby)game.BeginPrivateRehearsalBuy(now);else game.Tick(now,true);
        if(previous!=game.Phase)try{PrivatePhase();}catch(Exception e){Server.PrintToConsole("PRIVATE RAID stopped: "+e.Message);PrivateStopRound();return;}
        if(game.Phase==GamePhase.Active&&game.ExtractionAt is {} at&&now>=at){
            foreach(var intent in game.ExtractionCandidates(now,game.Players.Keys.Where(InPrivateExtraction))){
                if(!game.CompletePrivateRehearsalExtraction(intent)||Find(intent.Player) is not {} p)continue;
                raidOutcomes.Record(intent.Player,true,now);p.RemoveWeapons();SetMoney(p,0);p.ChangeTeam(CsTeam.Spectator);
                p.PrintToChat("LOCAL rehearsal extraction complete. No payout, funds or inventory persisted.");
            }
            game.FinishExtractionWindow(now);
        }
        foreach(var m in game.Players.Values.Where(m=>!game.Living(m.Id))){
            if(raidOutcomes.CanObserve(m.Id,now)&&privateSpectators.Add(m.Id)&&Find(m.Id) is {} p)p.ChangeTeam(CsTeam.Spectator);
        }
        if(game.Phase==GamePhase.Closed&&(now-privateClosedAt).TotalSeconds>=7)PrivateReset();
    }
    private bool InPrivateExtraction(string id)=>game.Living(id)&&Find(id) is {} p&&Location(p) is {} v&&PrivateExtractionBounds.Contains(v.X,v.Y);
    private void PrivateTick(){
        if(!sessionLoaded||privateStopping)return;
        UpdateSpectators();var now=DateTimeOffset.UtcNow;
        foreach(var p in Utilities.GetPlayers().Where(Human)){
            if(!game.Living(Id(p)))continue;
            ObserveNativeSpend(p);SetMoney(p,game.Players[Id(p)].Cash);
            if(p.PlayerPawn.Value is {} pawn)pawn.InBuyZone=game.BuyOpen(now);
            if(Server.TickCount%16!=0)continue;
            var v=Location(p);var zone=v!=null&&PrivateExtractionBounds.Contains(v.X,v.Y)?"IN EXTRACTION STRIP":v!=null&&PrivateExtractionBounds.Warning(v.X,v.Y)?"RED WARNING ZONE":"";
            var countdown=game.ExtractionAt is {} at?$"Extraction {Math.Max(0,(int)Math.Ceiling((at-now).TotalSeconds))}s": "Extraction awaiting 2 levers";
            var phase=game.StartsAt is {} end?$"{game.Phase} {Math.Max(0,(int)Math.Ceiling((end-now).TotalSeconds))}s":game.Phase.ToString();
            p.PrintToCenter($"LOCAL RAID | {phase}\n{countdown}\n{zone}");
        }
    }
    private void PrivateStopRound(bool retainLease=false){
        sessionLoaded=false;
        foreach(var p in Utilities.GetPlayers().Where(Human))SetPresentation(p,false,false);
        RestoreRaidBuyClock();try{if(!retainLease)File.WriteAllText(PrivateFlowLease,"");}catch(Exception e){Server.PrintToConsole("Private raid lease release failed: "+e.Message);}
    }
    private void StopPrivateRehearsal(){privateStopping=true;privateEpoch++;PrivateStopRound();}
}
