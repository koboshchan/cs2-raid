using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private GameplayConfig gameplayConfig = new();
    private GameplayState game = new();
    private SpawnProfile? selectedSpawnProfile;
    private bool SpawnBindingReady => string.IsNullOrEmpty(gameplayConfig.SpawnProfile) ||
        selectedSpawnProfile?.Matches(gameplayConfig.SpawnProfile,Server.MapName,gameplayConfig.MapRelease)==true;
    private RaidOutcomes raidOutcomes = new();
    // Unwired until actual private MVP/card/reset support is verified in-game.
    private Action<RaidEndProjection>? presentRaidEnd = null;
    private readonly Random random = new();
    private readonly Dictionary<uint,(string Owner,string Entity)> observedWeapons = [];
    private readonly Dictionary<string,uint> lootEntities = [];
    private readonly Dictionary<int,uint> leverEntities = [];
    private readonly Dictionary<string,int> savedCvars = [];
    private readonly Dictionary<string,Action> raidBuyBaselines = [];
    private readonly RaidBuyOwnership raidBuyOwnership = new();
    private readonly HashSet<string> extractionTouching = [];
    private readonly HashSet<string> useDown = [];
    private uint? extractionTrigger;
    private int lastCountdown = -1;
    private string latchedExtractionWindowId = "";
    private bool sessionLoaded;
    // Set by the DeltaOps capability integration only after assignment succeeds.
    // No local imitation of character kits or utility grants.
    private Func<CCSPlayerController,int,bool>? assignCharacter = null;
    private static bool Human([NotNullWhen(true)] CCSPlayerController? p) => p is { IsValid:true, IsBot:false } && p.AuthorizedSteamID!=null && p.SteamID!=0;
    private static string Id(CCSPlayerController p) => p.SteamID.ToString();
    private static CCSPlayerController? Find(string id) => Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&Id(p)==id);
    private void LoadGameplay()
    {
        var path=Path.Combine(ModuleDirectory,"gameplay.json");
        if(File.Exists(path)) gameplayConfig=JsonSerializer.Deserialize<GameplayConfig>(File.ReadAllText(path),new JsonSerializerOptions { PropertyNameCaseInsensitive=true }) ?? new();
        gameplayConfig.Validate();
        if(!string.IsNullOrEmpty(gameplayConfig.SpawnProfile)) {
            // Explicit fixed selection only. Never load arbitrary paths or overwrite legacy spawns.
            gameplayConfig.Spawns=[];
            if(gameplayConfig.SpawnProfile==SpawnProfile.Selection) {
                var profilePath=Path.Combine(ModuleDirectory,"spawns","spaceport-default-20261105.json");
                try{selectedSpawnProfile=SpawnProfile.Parse(File.ReadAllText(profilePath));}
                catch(Exception ex){Server.PrintToConsole("Raid spawn profile unavailable: "+ex.GetType().Name);}
            }
        }
        if(TestServer || !gameplayConfig.Enabled) return;
        RegisterDeltaOpsBridge(); RegisterPartnerVisuals(); RegisterPricing(); RegisterTonightGameplay();RegisterSettlementBridge();
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook(BlockGroundPickup,HookMode.Pre);
        VirtualFunctions.CBaseTrigger_StartTouchFunc.Hook(ExtractionTouchStart,HookMode.Post);
        VirtualFunctions.CBaseTrigger_EndTouchFunc.Hook(ExtractionTouchEnd,HookMode.Post);
        foreach(var command in new[]{"joinclass","autobuy","rebuy"})
            AddCommandListener(command,(p,c)=>Human(p)&&(sessionLoaded||raidBuyOwnership.Blocked)?HookResult.Handled:HookResult.Continue);
        AddCommand("css_cancelteam","Cancel your lobby team",(p,c)=>CancelTeam(p));
        AddCommand("css_cancel","Cancel team",(p,c)=>{if(c.ArgString.Trim().Equals("team",StringComparison.OrdinalIgnoreCase))CancelTeam(p);});
        AddCommand("css_team","Ready as solo",(p,c)=>{
            if(Human(p)&&!FinancialFrozen(Id(p))&&c.ArgString.Trim().Equals("single",StringComparison.OrdinalIgnoreCase)&&game.Solo(Id(p))) Place(p,gameplayConfig.ReadyPlatform);
        });
        AddCommand("css_characterselect","Select character 1-6",(p,c)=>{
            if(!Human(p) || FinancialFrozen(Id(p)) || game.Phase!=GamePhase.CharacterSelect || c.ArgCount!=2 || !int.TryParse(c.GetArg(1),out var number) || number<1 || number>6)return;
            if(!deltaOpsCharactersReady || assignCharacter==null || !assignCharacter(p,number)) {p.PrintToChat("Character bridge unavailable. Selection not confirmed; raid stays in selection.");return;}
            if(game.SelectCharacter(Id(p),number))p.PrintToChat("Selected "+GameplayState.Characters[number-1]);
        });
        AddCommand("css_cashout","Cash in from SSG",(p,c)=>BeginCashIn(p,c.ArgCount==2?c.GetArg(1):""));
        RegisterListener<Listeners.OnEntityTakeDamagePre>(Damage);
        RegisterListener<Listeners.OnTick>(GameplayTick);
        RegisterListener<Listeners.OnMapStart>(_=>{StopGameplay();AbandonExpiredSpread();spreadMapEnded=false;game=new();});
        RegisterListener<Listeners.OnMapEnd>(()=>{try{StopGameplay();}finally{spreadMapEnded=true;AbandonExpiredSpread();}});
        RegisterEventHandler<EventRoundStart>((e,i)=>{ResetLobby();return HookResult.Continue;});
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{if(Human(e.Userid)){raidOutcomes.Disconnect(Id(e.Userid));game.Leave(Id(e.Userid));}return HookResult.Continue;},HookMode.Pre);
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(Human(e.Userid)) {
                if(FinancialFrozen(Id(e.Userid))) {InvalidateFinancialPlayer(Id(e.Userid));if(IsFinancialActorBlocked(Id(e.Userid)))TryIsolateFinancialActor(e.Userid);return HookResult.Continue;}
                ObserveNativeSpend(e.Userid);
                var pos=Location(e.Userid);
                if(sessionLoaded && game.Living(Id(e.Userid)))RememberDeath(e.Userid,pos);
                var killed=pos!=null && game.DieWithBag(Id(e.Userid),pos);
                if(!killed)killed=game.Kill(Id(e.Userid),null);
                if(killed)BeginRaidOutcome(e.Userid,false);
            }
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(Human(e.Userid)) {
                var id=Id(e.Userid);var generation=mapGeneration;
                Server.NextFrame(()=>{if(generation!=mapGeneration)return;var p=Find(id);if(p==null)return;
                    if(FinancialFrozen(id)){InvalidateFinancialPlayer(id);if(IsFinancialActorBlocked(id))TryIsolateFinancialActor(p);return;}
                    if(sessionLoaded && game.Living(id))SpawnHealth(p);
                    if(game.Phase==GamePhase.Lobby)PrepareLobby(p);
                    else if(!game.Players.ContainsKey(id)||!game.Living(id)){p.RemoveWeapons();p.ChangeTeam(CsTeam.Spectator);}
                });
            }return HookResult.Continue;
        });
        AddTimer(1,GameplaySecond,TimerFlags.REPEAT);
        Server.NextFrame(ResetLobby);
    }
    private HookResult BlockGroundPickup(DynamicHook hook)
    {
        var weapon=hook.GetParam<CBasePlayerWeapon>(1);
        if(weapon is {IsValid:true} && IsQuarantinedHandle(weapon.EntityHandle.Raw)){hook.SetReturn(false);return HookResult.Handled;}
        if(!sessionLoaded || game.Phase!=GamePhase.Active)return HookResult.Continue;
        if(weapon is {IsValid:true} && !weapon.OwnerEntity.IsValid) {hook.SetReturn(false);return HookResult.Handled;}
        return HookResult.Continue;
    }
    private HookResult ExtractionTouchStart(DynamicHook hook) => ExtractionTouch(hook,true);
    private HookResult ExtractionTouchEnd(DynamicHook hook) => ExtractionTouch(hook,false);
    private HookResult ExtractionTouch(DynamicHook hook,bool enter)
    {
        if(!sessionLoaded || extractionTrigger==null)return HookResult.Continue;
        var trigger=hook.GetParam<CBaseTrigger>(0);var entity=hook.GetParam<CBaseEntity>(1);
        if(trigger.EntityHandle.Raw!=extractionTrigger)return HookResult.Continue;
        var p=Controller(entity);if(p!=null) {if(enter)extractionTouching.Add(Id(p));else extractionTouching.Remove(Id(p));}
        return HookResult.Continue;
    }
    private void SetCvar(string name,int value)
    {
        var cv=ConVar<int>.Find(name);if(cv==null)throw new InvalidOperationException("Missing cvar "+name);
        if(!savedCvars.ContainsKey(name))savedCvars[name]=cv.Value;
        cv.Value=value;
    }
    private void ResetLobby()
    {
        if(TestServer)return;
        sessionLoaded=false;
        if(!raidBuyOwnership.BeginReset(RestoreRaidBuyClock,()=>raidBuyBaselines.Count!=0,RaidBuyError))return;
        InvalidateFinancialMatch();ClearPartnerVisuals();ClearLoot();ClearTonightMatch();raidOutcomes=new();game=new();observedWeapons.Clear();nativeProjections.Clear();lastCountdown=-1;
        if(selectedSpawnProfile!=null)
            gameplayConfig.Spawns=selectedSpawnProfile.Select(gameplayConfig.SpawnProfile,Server.MapName,gameplayConfig.MapRelease);
        sessionLoaded=SpawnBindingReady && gameplayConfig.Ready(0);
        if(!sessionLoaded){Server.PrintToChatAll("Raid disabled until map coordinates and native flow QA are confirmed.");return;}
        SetCvar("mp_autoteambalance",0);SetCvar("mp_limitteams",0);SetCvar("mp_ignore_round_win_conditions",1);
        SetCvar("mp_teammates_are_enemies",1);SetCvar("mp_friendlyfire",1);SetCvar("mp_death_drop_gun",1);
        SetCvar("mp_respawn_on_death_t",0);SetCvar("mp_respawn_on_death_ct",0);
        SetCvar("mp_startmoney",0);SetCvar("mp_playercashawards",0);SetCvar("mp_teamcashawards",0);
        SetCvar("mp_maxmoney",TonightRules.HudLimit);SetCvar("mp_buy_anywhere",0);
        if(!raidBuyOwnership.Apply(()=>SetNativeBuyTime(raidBuyBaselines,0),CleanupRaidBuyFailure,()=>raidBuyBaselines.Count!=0,RaidBuyError,false)){sessionLoaded=false;return;}
        PatchExistingSpread();
        deltaOpsCharactersReady=ResetDeltaOpsCharacters();
        foreach(var p in Utilities.GetPlayers().Where(Human)) {if(IsFinancialActorBlocked(Id(p))){TryIsolateFinancialActor(p);continue;}if(FinancialFrozen(Id(p))){HoldFinancialInventory(p);continue;}if(game.Join(Id(p)))PrepareLobby(p);else p.ChangeTeam(CsTeam.Spectator);}
    }
    private void PrepareLobby(CCSPlayerController p)
    {
        if(IsFinancialActorBlocked(Id(p))){TryIsolateFinancialActor(p);return;}
        if(FinancialFrozen(Id(p))){HoldFinancialInventory(p);return;}
        if(!sessionLoaded)return;
        game.Join(Id(p));if(p.TeamNum is not (2 or 3))p.SwitchTeam(CsTeam.Terrorist);
        if(!p.PawnIsAlive)p.Respawn();
        p.RemoveWeapons();p.GiveNamedItem("weapon_glock");SpawnHealth(p);SetPresentation(p,false,false);
        Place(p,game.Players.TryGetValue(Id(p),out var m)&&m.Team!=null?gameplayConfig.ReadyPlatform:gameplayConfig.Lobby);
        SetMoney(p,0);
    }
    private static Position? Location(CCSPlayerController p) => p.PlayerPawn.Value?.AbsOrigin is {} v ? new(v.X,v.Y,v.Z,p.PlayerPawn.Value?.EyeAngles.Y??0) : null;
    private static void Place(CCSPlayerController p,Position pos) => p.PlayerPawn.Value?.Teleport(new Vector(pos.X,pos.Y,pos.Z),new QAngle(0,pos.Yaw,0),new Vector(0,0,0));
    private static void SetPresentation(CCSPlayerController p,bool frozen,bool invisible)
    {
        if(p.PlayerPawn.Value is not {} pawn)return;
        pawn.MoveType=frozen?MoveType_t.MOVETYPE_NONE:MoveType_t.MOVETYPE_WALK;
        pawn.ActualMoveType=pawn.MoveType;
        pawn.Render=Color.FromArgb(invisible?0:255,255,255,255);
        pawn.RenderMode=invisible?RenderMode_t.kRenderTransAlpha:RenderMode_t.kRenderNormal;
        Utilities.SetStateChanged(pawn,"CBaseEntity","m_MoveType");
        Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_clrRender");
        Utilities.SetStateChanged(pawn,"CBaseModelEntity","m_nRenderMode");
    }
    private void CancelTeam(CCSPlayerController? p)
    {
        if(!Human(p) || FinancialFrozen(Id(p)))return;
        foreach(var id in game.Cancel(Id(p))) {var other=Find(id);if(other!=null && !FinancialFrozen(id)){Place(other,gameplayConfig.Lobby);SetPresentation(other,false,false);}}
        lastCountdown=-1;
    }
    private CCSPlayerController? Controller(CBaseEntity? pawn) => pawn==null?null:Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&p.PlayerPawn.Raw==pawn.EntityHandle.Raw);
    private HookResult Damage(CBaseEntity entity,CTakeDamageInfo info)
    {
        if(!sessionLoaded)return HookResult.Continue;
        if(lootEntities.Values.Contains(entity.EntityHandle.Raw))return HookResult.Handled; // never shoot-to-loot
        var attacker=Controller(info.Attacker.Value);var victim=Controller(entity);
        if(victim==null)return HookResult.Continue;
        if(FinancialFrozen(Id(victim)) || attacker!=null && FinancialFrozen(Id(attacker)))return HookResult.Handled;
        if(game.Phase==GamePhase.Lobby) {
            if(attacker!=null && info.Damage>0 && (info.BitsDamageType & DamageTypes_t.DMG_BULLET)!=0 && game.ShootRequest(Id(attacker),Id(victim))) {
                Server.PrintToChatAll(SafeName(attacker.PlayerName)+" and "+SafeName(victim.PlayerName)+" are teammates");
                Place(attacker,gameplayConfig.ReadyPlatform);Place(victim,gameplayConfig.ReadyPlatform);
            }return HookResult.Handled;
        }
        if(game.Phase!=GamePhase.Active || !game.Living(Id(victim)))return HookResult.Handled;
        if(attacker!=null && !game.CanDamage(Id(attacker),Id(victim)))return HookResult.Handled;
        return HookResult.Continue;
    }
    private static string SafeName(string s) => new(s.Where(c=>!char.IsControl(c)).Take(64).ToArray());
    private void GameplaySecond()
    {
        TickSettlementRecovery();
        if(raidBuyOwnership.Blocked){raidBuyOwnership.Cleanup(CleanupRaidBuyFailure,()=>raidBuyBaselines.Count!=0,RaidBuyError);return;}
        if(!sessionLoaded)return;
        var now=DateTimeOffset.UtcNow;
        // Eligibility is sampled once at the first game-thread second at/after the
        // deadline, before phase advancement or unrelated financial work can delay it.
        ProcessExtractionDeadline(now);
        foreach(var p in Utilities.GetPlayers().Where(Human)) {
            if(IsFinancialActorBlocked(Id(p))){TryIsolateFinancialActor(p);continue;}if(FinancialFrozen(Id(p))){HoldFinancialInventory(p);continue;}
            if(game.Phase==GamePhase.Lobby) {if(game.Join(Id(p)))PrepareLobby(p);}
            else if(!game.Players.ContainsKey(Id(p))&&p.TeamNum!=(int)CsTeam.Spectator)p.ChangeTeam(CsTeam.Spectator);
        }
        if(game.Phase==GamePhase.Lobby && !deltaOpsCharactersReady)deltaOpsCharactersReady=ResetDeltaOpsCharacters();
        var previous=game.Phase;
        var count=game.Tick(now,deltaOpsCharactersReady && SpawnBindingReady && (selectedSpawnProfile==null || SpawnProfile.LayoutReady(game.Players.Values)) && gameplayConfig.Ready(game.Players.Values.Select(x=>x.Team).Distinct().Count()));
        if(game.Phase!=previous && !raidBuyOwnership.Apply(OnPhaseChanged,CleanupRaidBuyFailure,()=>raidBuyBaselines.Count!=0,RaidBuyError,game.Phase==GamePhase.Buy)){sessionLoaded=false;return;}
        if(count is int n && n!=lastCountdown){lastCountdown=n;Server.PrintToChatAll(game.Phase+" "+n+"s");}
        if(game.Phase==GamePhase.Closed) {
            raidOutcomes.Close(now);
            foreach(var m in game.Players.Values.Where(x=>!x.Extracted)){
                var p=Find(m.Id);if(p==null)continue;
                if(IsFinancialActorBlocked(m.Id)){TryIsolateFinancialActor(p);continue;}
                if(FinancialFrozen(m.Id)){HoldFinancialInventory(p);continue;}
                BeginRaidOutcome(p,false);
                if(p.PawnIsAlive)p.CommitSuicide(false,true);
            }
            if(raidOutcomes.ReadyToFinish(now)) {
                foreach(var projection in raidOutcomes.Finish(game,now))
                    try{presentRaidEnd?.Invoke(projection);}catch(Exception ex){RaidBuyError(ex);}
                // Do not manufacture a native global winner, music or private full card.
                ResetLobby();
            }
        }
    }

    private void ProcessExtractionDeadline(DateTimeOffset now)
    {
        if(game.Phase!=GamePhase.Active || game.ExtractionAt is not {} at || now<at)return;
        var window=game.ExtractionWindowId;
        if(!extractionAdmissionClosed || latchedExtractionWindowId!=window) {
            // Close admission BEFORE any fallible entity lookup, snapshot or issuance.
            extractionAdmissionClosed=true;latchedExtractionWindowId=window;
            string[] inZone;
            try {inZone=game.Players.Keys.Where(id=>!FinancialFrozen(id)&&InExtractionZone(id)).ToArray();}
            catch(Exception) {
                inZone=[];
                Server.PrintToChatAll("Extraction eligibility snapshot unavailable. No new payouts this window; cash and inventory are not consumed. Retry the next extraction window.");
            }
            var candidates=game.ExtractionCandidates(now,inZone);
            if(FinancialReady)foreach(var intent in candidates) {
                try {
                    var p=Find(intent.Player);if(p==null)continue;
                    p.PrintToChat("Extraction eligibility locked. If your snapshot is unavailable, no payout or consumption this window; retry the next extraction window.");
                    BeginExtraction(p,intent);
                }
                catch(Exception) {
                    // Never re-admit this or a late player after a partial issuance.
                    // Other members of the already-latched cohort still get processed.
                    Server.PrintToConsole("Raid extraction cohort member unavailable; no automatic refund or regrant. Check reconciliation before retrying an issued request.");
                }
            }
            else if(candidates.Length>0)Server.PrintToChatAll("Extraction settlement disabled pending financial native QA. No SSG funds paid; cash and inventory retained. Retry the next extraction window.");
        }
        // ConfirmVerifiedExtraction still needs its issued intent and ExtractionAt.
        // Only this window's matching issued extraction work may retain that state.
        if(!PendingExtractionSettlement(window)) {
            game.FinishExtractionWindow(now);extractionAdmissionClosed=false;latchedExtractionWindowId="";
        }
    }
    private void OnPhaseChanged()
    {
        lastCountdown=-1;
        foreach(var m in game.Players.Values) {
            var p=Find(m.Id);if(p==null)continue;
            if(IsFinancialActorBlocked(m.Id)){TryIsolateFinancialActor(p);continue;}
                if(FinancialFrozen(m.Id)){HoldFinancialInventory(p);continue;}
            SetPresentation(p,game.Phase is GamePhase.CharacterSelect or GamePhase.Countdown or GamePhase.Freeze,game.Phase==GamePhase.CharacterSelect);
            if(game.Phase==GamePhase.CharacterSelect) {
                for(var i=0;i<GameplayState.Characters.Length;i++)p.PrintToChat((i+1)+". "+GameplayState.Characters[i]);
                p.PrintToChat("Choose with !characterselect <number>.");
            }
        }
        if(game.Phase==GamePhase.Buy){SyncRaidBuyClock();SetCvar("mp_buy_anywhere",1);Server.PrintToChatAll("Buy guns and armor for 45s. Utility comes from character kits, not the shop.");}
        if(game.Phase==GamePhase.Countdown){RestoreRaidBuyRules();SetNativeBuyTime(raidBuyBaselines,0);SetCvar("mp_buy_anywhere",0);}
        if(game.Phase==GamePhase.Freeze) {
            foreach(var player in Utilities.GetPlayers().Where(Human).Where(p=>!FinancialFrozen(Id(p))&&game.Living(Id(p))))SpawnHealth(player);
            var spawns=gameplayConfig.Spawns.OrderBy(_=>random.Next()).ToArray();int index=0;
            foreach(var team in game.Players.Values.GroupBy(x=>x.Team)){
                var pos=spawns[index++];int member=0;
                foreach(var m in team){var p=Find(m.Id);if(p!=null && !FinancialFrozen(m.Id))Place(p,selectedSpawnProfile==null?pos with {X=pos.X+member++*48}:pos);}
            }
        }
        if(game.Phase==GamePhase.Active){SpawnLoot();Server.PrintToChatAll("Raid started. 30 minutes. Press E for loot; hold E for safes.");}
    }
    private IEnumerable<CBaseEntity> MapEntities() => new[]{"prop_dynamic","prop_dynamic_override","prop_physics","prop_physics_multiplayer","func_button","func_rot_button","trigger_multiple"}
        .SelectMany(name=>Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(name)).Where(e=>e.IsValid);
    private void SpawnLoot()
    {
        var entities=MapEntities().ToArray();
        foreach(var rule in gameplayConfig.MapLoot) {
            var matches=entities.Where(e=>e.Entity?.Name==rule.Targetname).ToArray();
            if(matches.Length!=1){Server.PrintToConsole("Raid loot target missing or ambiguous: "+rule.Targetname);continue;}
            var e=matches[0];
            if(random.NextDouble()>=rule.SpawnRate){e.Remove();continue;}
            if(e.AbsOrigin is not {} v)continue;
            var loot=new GroundLoot(rule.Targetname,new(v.X,v.Y,v.Z),rule.Kind,
                rule.Kind==LootKind.Item?rule.Value:rule.Min,rule.Kind==LootKind.Item?rule.Value:rule.Max,rule.HoldSeconds);
            if(game.AddLoot(loot))lootEntities[loot.Id]=e.EntityHandle.Raw;
        }
        foreach(var lever in gameplayConfig.Levers) {
            var matches=entities.Where(e=>e.Entity?.Name==lever.Targetname).ToArray();
            if(matches.Length==1)leverEntities[lever.Number]=matches[0].EntityHandle.Raw;
        }
        if(!string.IsNullOrEmpty(gameplayConfig.ExtractionTrigger)){
            var matches=entities.Where(e=>e.Entity?.Name==gameplayConfig.ExtractionTrigger).ToArray();
            if(matches.Length==1 && matches[0].DesignerName=="trigger_multiple")extractionTrigger=matches[0].EntityHandle.Raw;
        }
    }
    private bool InExtractionZone(string id)
    {
        if(!game.Living(id))return false;
        if(extractionTrigger!=null)return extractionTouching.Contains(id);
        return gameplayConfig.ExtractionCenter is {} center && gameplayConfig.ExtractionRadius>0 && Find(id) is {} p && Location(p) is {} pos && pos.Within(center,gameplayConfig.ExtractionRadius);
    }
    private void SetMoney(CCSPlayerController p,long value)
    {
        if(TestServer || p.InGameMoneyServices is not {} money)return;
        money.Account=TonightRules.Hud(value);nativeProjections[Id(p)]=money.Account;
        Utilities.SetStateChanged(p,"CCSPlayerController","m_pInGameMoneyServices");
    }
    private void GameplayTick()
    {
        if(raidBuyOwnership.Blocked)return;
        if(!sessionLoaded)return;
        if(!SpawnBindingReady){StopGameplay();return;}
        UpdatePartnerVisuals();
        UpdateSpectators();
        foreach(var p in Utilities.GetPlayers().Where(Human)) {
            if(IsFinancialActorBlocked(Id(p))){TryIsolateFinancialActor(p);continue;}
            if(FinancialFrozen(Id(p))) {HoldFinancialInventory(p);continue;}
            if(!game.Living(Id(p)))continue;
            ObserveNativeSpend(p);SetMoney(p,game.Players[Id(p)].Cash);
            if(p.PlayerPawn.Value is {} pawn)pawn.InBuyZone=raidBuyOwnership.Allows(raidBuyOwnership.Generation,game.BuyOpen(DateTimeOffset.UtcNow));
        }
        if(game.Phase!=GamePhase.Active)return;
        var current=new Dictionary<uint,(string Owner,string Entity)>();
        foreach(var p in Utilities.GetPlayers().Where(Human)) {
            if(!game.Living(Id(p)) || p.PlayerPawn.Value?.WeaponServices is not {} ws)continue;
            foreach(var handle in ws.MyWeapons)if(handle.Value is {IsValid:true} weapon)current[handle.Raw]=(Id(p),weapon.DesignerName);
        }
        foreach(var (handle,old) in observedWeapons)if(!current.ContainsKey(handle)) {
            var weapon=new CHandle<CBasePlayerWeapon>(handle).Value;
            if(weapon is not {IsValid:true} || weapon.OwnerEntity.IsValid || weapon.AbsOrigin is not {} pos)continue;
            var item=catalog.FirstOrDefault(x=>x.Entity==old.Entity);
            if(FinancialFrozen(old.Owner) || item==null || (!item.IsGun && !GameplayConfig.Utility(item.Entity)))continue;
            var key="equipment:"+handle;
            if(game.AddLoot(new(key,new(pos.X,pos.Y,pos.Z),LootKind.Equipment,item.PriceDollars,item.PriceDollars)))lootEntities[key]=handle;
        }
        // No inventory-diff pickup award. Native ground pickup is denied by CanUse.
        observedWeapons.Clear();foreach(var entry in current)observedWeapons.Add(entry.Key,entry.Value);
        foreach(var p in Utilities.GetPlayers().Where(Human))Interact(p);
        UpdateSpectators();
    }
    private void Interact(CCSPlayerController p)
    {
        var id=Id(p);
        if(FinancialFrozen(id) || !game.Living(id) || Location(p) is not {} pos || (p.Buttons & PlayerButtons.Use)==0){game.ReleaseUse(id);useDown.Remove(id);return;}
        var pressed=useDown.Add(id);
        var target=game.Ground.Values.Where(x=>pos.Within(x.Position,96)).OrderBy(x=>Math.Pow(pos.X-x.Position.X,2)+Math.Pow(pos.Y-x.Position.Y,2)+Math.Pow(pos.Z-x.Position.Z,2)).FirstOrDefault();
        if(target!=null) {
            if(target.Kind!=LootKind.Safe && !pressed){game.ReleaseUse(id);return;}
            p.PrintToCenter(target.Kind==LootKind.Safe?"Hold E to open safe":"E to collect "+target.Kind);
            if(game.UseLoot(id,target.Id,pos,true,DateTimeOffset.UtcNow,random)) {
                if(lootEntities.Remove(target.Id,out var raw)){var entity=new CHandle<CBaseEntity>(raw).Value;if(entity is {IsValid:true})entity.Remove();}
                SetMoney(p,game.Players[id].Cash);
            }return;
        }
        game.ReleaseUse(id);
        if(!pressed)return;
        foreach(var (number,raw) in leverEntities.OrderBy(entry=>{
            var origin=new CHandle<CBaseEntity>(entry.Value).Value?.AbsOrigin;
            return origin==null?double.MaxValue:Math.Pow((double)pos.X-origin.X,2)+Math.Pow((double)pos.Y-origin.Y,2)+Math.Pow((double)pos.Z-origin.Z,2);
        })) {
            var e=new CHandle<CBaseEntity>(raw).Value;
            if(e is {IsValid:true} && e.AbsOrigin is {} v && pos.Within(new(v.X,v.Y,v.Z),96) && game.PullLever(id,number,DateTimeOffset.UtcNow))
            {
                Server.PrintToChatAll(game.ExtractionAt is {} at?"Extraction in 5 minutes.":"Lever "+number+" activated.");
                break; // One physical lever per fresh E press, even if two are close together.
            }
        }
    }
    private void BeginRaidOutcome(CCSPlayerController p,bool extracted)
    {
        if(!raidOutcomes.Record(Id(p),extracted,DateTimeOffset.UtcNow))return;
        try{ShowRaidResult(p,extracted);}catch(Exception ex){RaidBuyError(ex);}
    }
    private void UpdateSpectators()
    {
        if(!sessionLoaded)return;
        foreach(var m in game.Players.Values.Where(x=>!x.Alive || x.Extracted)) {
            var p=Find(m.Id);if(p==null)continue;
            if(!raidOutcomes.CanObserve(m.Id,DateTimeOffset.UtcNow))continue;
            if(raidOutcomes.TakeCamera(m.Id,DateTimeOffset.UtcNow)) {
                // Source-supported private cleanup request only, NOT guaranteed panel/audio reset.
                var reset=new EventRoundStart(true);
                try{reset.FireEventToClient(p);}catch(Exception ex){RaidBuyError(ex);}finally{reset.Free();}
            }
            var mate=game.Spectate(m.Id);var target=mate==null?null:Find(mate);
            // Resolve connected living partner NOW, not at death/extraction.
            if(target is not {PawnIsAlive:true})target=null;
            var pawn=p.ObserverPawn.Value;
            var observer=pawn?.ObserverServices;
            if(pawn is not {IsValid:true} || observer==null)continue;
            observer.ObserverMode=(byte)(target==null?ObserverMode_t.OBS_MODE_ROAMING:ObserverMode_t.OBS_MODE_CHASE);
            observer.ForcedObserverMode=true;
            observer.ObserverTarget.Raw=target?.PlayerPawn.Raw??uint.MaxValue;
            Utilities.SetStateChanged(pawn,"CBasePlayerPawn","m_pObserverServices");
            if(target==null && deathPositions.TryGetValue(m.Id,out var position) &&
               (!roamingPlaced.TryGetValue(m.Id,out var placed) || placed!=pawn.EntityHandle.Raw)) {
                pawn.Teleport(new Vector(position.X,position.Y,position.Z+64),new QAngle(0,position.Yaw,0),new Vector(0,0,0));
                roamingPlaced[m.Id]=pawn.EntityHandle.Raw; // Once only, never snap a roaming camera back each tick.
            }

        }
    }
    private void ClearLoot()
    {
        // Map-authored props belong to the map. Round/map reset restores removed ones.
        latchedExtractionWindowId="";lootEntities.Clear();leverEntities.Clear();extractionTrigger=null;extractionTouching.Clear();useDown.Clear();
    }
    private static void RestoreCvar(string name,int value) {var cv=ConVar<int>.Find(name);if(cv!=null)cv.Value=value;}
    private void UnhookGameplay()
    {
        if(!gameplayConfig.Enabled)return;
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Unhook(BlockGroundPickup,HookMode.Pre);
        VirtualFunctions.CBaseTrigger_StartTouchFunc.Unhook(ExtractionTouchStart,HookMode.Post);
        VirtualFunctions.CBaseTrigger_EndTouchFunc.Unhook(ExtractionTouchEnd,HookMode.Post);
    }
    private void StopGameplay()
    {
        raidOutcomes=new();
        var wasLoaded=sessionLoaded;
        RaidGameplayStop.Run(raidBuyOwnership,()=>sessionLoaded=false,
            CleanupRaidBuyFailure,()=>raidBuyBaselines.Count!=0,RaidBuyError,
            InvalidateFinancialMatch,
            ()=>{
                if(!wasLoaded)return;
                foreach(var p in Utilities.GetPlayers().Where(Human)){
                    try{if(IsFinancialActorBlocked(Id(p)))TryIsolateFinancialActor(p);else if(FinancialFrozen(Id(p)))HoldFinancialInventory(p);else SetPresentation(p,false,false);}
                    catch(Exception ex){RaidBuyError(ex);}
                }
            },
            RestoreSpread,ClearTonightMatch,ClearPricing,ClearPartnerVisuals,ClearLoot,observedWeapons.Clear,
            ()=>{
                foreach(var (name,value) in savedCvars.ToArray()){
                    try{RestoreCvar(name,value);savedCvars.Remove(name);}
                    catch(Exception ex){RaidBuyError(ex);}
                }
            });
    }
}
