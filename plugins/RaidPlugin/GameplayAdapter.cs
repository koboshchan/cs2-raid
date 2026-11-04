using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private GameplayConfig gameplayConfig = new();
    private GameplayState game = new();
    private readonly Random random = new();
    private readonly Dictionary<uint,(string Owner,string Entity)> observedWeapons = [];
    private readonly Dictionary<uint,LootPoint> lootEntities = [];
    private readonly Dictionary<string,int> savedCvars = [];
    private int lastCountdown = -1;
    private bool sessionLoaded;
    private static bool Human([NotNullWhen(true)] CCSPlayerController? p) => p is { IsValid:true, IsBot:false } && p.SteamID!=0;
    private static string Id(CCSPlayerController p) => p.SteamID.ToString();
    private static CCSPlayerController? Find(string id) => Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&Id(p)==id);
    private void LoadGameplay()
    {
        var path=Path.Combine(ModuleDirectory,"gameplay.json");
        if(File.Exists(path)) gameplayConfig=JsonSerializer.Deserialize<GameplayConfig>(File.ReadAllText(path),new JsonSerializerOptions { PropertyNameCaseInsensitive=true }) ?? new();
        gameplayConfig.Validate();
        if(!gameplayConfig.Enabled) return;
        RegisterListener<Listeners.OnServerPrecacheResources>(manifest=>manifest.AddResource(gameplayConfig.LootModel));
        RegisterPartnerVisuals();
        RegisterPricing();
        foreach(var command in new[]{"jointeam","joinclass","autobuy","rebuy"})
            AddCommandListener(command,(p,c)=>Human(p)&&sessionLoaded?HookResult.Handled:HookResult.Continue);
        AddCommand("css_cancelteam","Cancel your lobby team",(p,c)=>CancelTeam(p));
        AddCommand("css_cancel","Cancel team",(p,c)=>{if(c.ArgString.Trim().Equals("team",StringComparison.OrdinalIgnoreCase))CancelTeam(p);});
        AddCommand("css_team","Ready as solo",(p,c)=>{
            if(Human(p)&&c.ArgString.Trim().Equals("single",StringComparison.OrdinalIgnoreCase)&&game.Solo(Id(p!))) Place(p!,gameplayConfig.ReadyPlatform);
        });
        RegisterListener<Listeners.OnEntityTakeDamagePre>(Damage);
        RegisterListener<Listeners.OnTick>(GameplayTick);
        RegisterListener<Listeners.OnMapStart>(_=>{StopGameplay(); game=new(); sessionLoaded=false;});
        RegisterListener<Listeners.OnMapEnd>(StopGameplay);
        RegisterEventHandler<EventRoundStart>((e,i)=>{ResetLobby();return HookResult.Continue;});
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{if(Human(e.Userid))game.Leave(Id(e.Userid));return HookResult.Continue;},HookMode.Pre);
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(Human(e.Userid)&&game.Kill(Id(e.Userid),Human(e.Attacker)?Id(e.Attacker):null))
                Server.NextFrame(UpdateSpectators);
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerSpawn>((e,i)=>{
            if(Human(e.Userid)) {
                var id=Id(e.Userid); var generation=mapGeneration;
                Server.NextFrame(()=>{if(generation!=mapGeneration)return;var p=Find(id);if(p==null)return;
                    if(game.Phase is GamePhase.Lobby or GamePhase.Countdown) PrepareLobby(p);
                    else if(!game.Living(id)) {p.RemoveWeapons();p.ChangeTeam(CsTeam.Spectator);}
                });
            } return HookResult.Continue;
        });
        AddTimer(1,GameplaySecond,TimerFlags.REPEAT);
        Server.NextFrame(ResetLobby);
    }
    private void SetCvar(string name,int value)
    {
        var cv=ConVar<int>.Find(name);if(cv==null)throw new InvalidOperationException("Missing cvar "+name);
        if(!savedCvars.ContainsKey(name))savedCvars[name]=cv.Value;
        cv.Value=value;
    }
    private void ResetLobby()
    {
        ClearPartnerVisuals();ClearLoot();game=new();observedWeapons.Clear();lastCountdown=-1;
        sessionLoaded=gameplayConfig.Ready(0);
        if(!sessionLoaded){Server.PrintToChatAll("Raid disabled until lobby, platform and spawn coordinates are confirmed.");return;}
        SetCvar("mp_teammates_are_enemies",1);SetCvar("mp_friendlyfire",1);SetCvar("mp_death_drop_gun",1);
        SetCvar("mp_respawn_on_death_t",0);SetCvar("mp_respawn_on_death_ct",0);
        SetCvar("mp_startmoney",0);SetCvar("mp_playercashawards",0);SetCvar("mp_teamcashawards",0);
        SetCvar("mp_maxmoney",65535);SetCvar("mp_buy_anywhere",0);
        foreach(var p in Utilities.GetPlayers().Where(Human)) { game.Join(Id(p)); PrepareLobby(p); }
    }
    private void PrepareLobby(CCSPlayerController p)
    {
        if(!sessionLoaded)return;
        game.Join(Id(p));p.SwitchTeam(CsTeam.Terrorist);
        if(!p.PawnIsAlive)p.Respawn();
        p.RemoveWeapons();p.GiveNamedItem("weapon_glock");
        Place(p,game.Players.TryGetValue(Id(p),out var m)&&m.Team!=null?gameplayConfig.ReadyPlatform:gameplayConfig.Lobby);
        SetMoney(p,0);
    }
    private static void Place(CCSPlayerController p,Position pos) => p.PlayerPawn.Value?.Teleport(new Vector(pos.X,pos.Y,pos.Z),new QAngle(0,pos.Yaw,0),new Vector(0,0,0));
    private void CancelTeam(CCSPlayerController? p)
    {
        if(!Human(p))return;
        foreach(var id in game.Cancel(Id(p!))) {var other=Find(id);if(other!=null)Place(other,gameplayConfig.Lobby);}
        lastCountdown=-1;
    }
    private CCSPlayerController? Controller(CBaseEntity? pawn) => pawn==null?null:Utilities.GetPlayers().FirstOrDefault(p=>Human(p)&&p.PlayerPawn.Raw==pawn.EntityHandle.Raw);
    private HookResult Damage(CBaseEntity entity,CTakeDamageInfo info)
    {
        if(!sessionLoaded)return HookResult.Continue;
        var attacker=Controller(info.Attacker.Value);
        if(lootEntities.TryGetValue(entity.EntityHandle.Raw,out var point)) {
            if(attacker!=null && info.Damage>0 && (info.BitsDamageType & DamageTypes_t.DMG_BULLET)!=0 && game.Claim(point.Id,Id(attacker),gameplayConfig.Tiers[point.Tier].Min,gameplayConfig.Tiers[point.Tier].Max,random)) {
                lootEntities.Remove(entity.EntityHandle.Raw); entity.Remove();
            } return HookResult.Handled;
        }
        var victim=Controller(entity);
        if(victim==null)return HookResult.Continue;
        if(game.Phase is GamePhase.Lobby or GamePhase.Countdown) {
            if(attacker!=null && info.Damage>0 && (info.BitsDamageType & DamageTypes_t.DMG_BULLET)!=0 && game.ShootRequest(Id(attacker),Id(victim))) {
                Server.PrintToChatAll($"{SafeName(attacker.PlayerName)} and {SafeName(victim.PlayerName)} are teammates");
                Place(attacker,gameplayConfig.ReadyPlatform);Place(victim,gameplayConfig.ReadyPlatform);
            } return HookResult.Handled;
        }
        if(game.Phase!=GamePhase.Active || !game.Living(Id(victim)))return HookResult.Handled;
        if(attacker!=null && !game.CanDamage(Id(attacker),Id(victim)))return HookResult.Handled;
        return HookResult.Continue;
    }
    private static string SafeName(string s) => new(s.Where(c=>!char.IsControl(c)).Take(64).ToArray());
    private void GameplaySecond()
    {
        if(!sessionLoaded)return;
        foreach(var p in Utilities.GetPlayers().Where(Human)) {
            if(game.Phase is GamePhase.Lobby or GamePhase.Countdown) {if(game.Join(Id(p))) {lastCountdown=-1;PrepareLobby(p);}}
            else if(!game.Players.ContainsKey(Id(p))&&p.TeamNum!=(int)CsTeam.Spectator) p.ChangeTeam(CsTeam.Spectator);
        }
        foreach(var p in Utilities.GetPlayers().Where(Human))if(game.Living(Id(p))&&p.TeamNum!=(int)CsTeam.Terrorist)p.SwitchTeam(CsTeam.Terrorist);
        var count=game.Tick(DateTimeOffset.UtcNow,gameplayConfig.Ready(game.Players.Values.Select(x=>x.Team).Distinct().Count()));
        if(count is int n && n!=lastCountdown) {lastCountdown=n;if(n>0)Server.PrintToChatAll($"Raid starts in {n}");else StartGameplay();}
        if(game.Phase==GamePhase.Closed)ResetLobby();
    }
    private void StartGameplay()
    {
        var spawns=gameplayConfig.Spawns.OrderBy(_=>random.Next()).ToArray();int index=0;
        foreach(var team in game.Players.Values.GroupBy(x=>x.Team)) {
            var pos=spawns[index++];
            // Map owner must provide spacious pads. Offset duo members to avoid exact overlap.
            int member=0;foreach(var m in team) {var p=Find(m.Id);if(p==null)continue;Place(p,pos with {X=pos.X+member++*48});SetMoney(p,0);}
        }
        SpawnLoot();Server.PrintToChatAll("Raid started. 30 minutes. No respawns.");
    }
    private void SpawnLoot()
    {
        foreach(var point in gameplayConfig.Loot) {
            if(random.NextDouble()>=gameplayConfig.LootSpawnChance)continue;
            var prop=Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override");if(prop==null)continue;
            prop.SetModel(gameplayConfig.LootModel);prop.DispatchSpawn();
            prop.Teleport(new Vector(point.Position.X,point.Position.Y,point.Position.Z),new QAngle(0,point.Position.Yaw,0),new Vector(0,0,0));
            prop.Collision.SolidType=SolidType_t.SOLID_VPHYSICS;
            prop.Health=100000;prop.TakesDamage=true;
            lootEntities.Add(prop.EntityHandle.Raw,point);
        }
    }
    private static void SetMoney(CCSPlayerController p,long value)
    {
        if(p.InGameMoneyServices is not {} money)return;
        // Ledger is authoritative; CS HUD is limited. Never mint SSG funds here.
        money.Account=(int)Math.Min(value,65535);
        Utilities.SetStateChanged(p,"CCSPlayerController","m_pInGameMoneyServices");
    }
    private void GameplayTick()
    {
        if(!sessionLoaded)return;
        UpdatePartnerVisuals();
        if(game.Phase!=GamePhase.Active)return;
        var current=new Dictionary<uint,(string Owner,string Entity)>();
        foreach(var p in Utilities.GetPlayers().Where(Human)) {
            if(!game.Living(Id(p)))continue;
            SetMoney(p,game.Players[Id(p)].Cash);
            if(p.PlayerPawn.Value?.WeaponServices is not {} ws)continue;
            foreach(var handle in ws.MyWeapons)if(handle.Value is {IsValid:true} weapon)current[handle.Raw]=(Id(p),weapon.DesignerName);
        }
        foreach(var (handle,old) in observedWeapons)if(!current.TryGetValue(handle,out var now)||old.Owner!=now.Owner) {
            var value=catalog.FirstOrDefault(x=>x.Entity==old.Entity && x.IsGun)?.PriceDollars??0;
            game.Drop(handle,old.Owner,old.Entity,value);
        }
        foreach(var (handle,now) in current)if(!observedWeapons.TryGetValue(handle,out var old)||old.Owner!=now.Owner)game.Pickup(handle,now.Owner);
        observedWeapons.Clear();foreach(var item in current)observedWeapons.Add(item.Key,item.Value);
        UpdateSpectators();
    }
    private void UpdateSpectators()
    {
        if(!sessionLoaded)return;
        foreach(var m in game.Players.Values.Where(x=>!x.Alive)) {
            var p=Find(m.Id);if(p==null)continue;
            var mate=game.Spectate(m.Id);var target=mate==null?null:Find(mate);
            // Dead observer pawn, not a revived transparent live pawn. Cannot shoot/collide/use.
            var observer=p.ObserverPawn.Value?.ObserverServices;
            if(observer==null)continue;
            observer.ObserverMode=(byte)(target==null?6:5);
            observer.ForcedObserverMode=true;
            observer.ObserverTarget.Raw=target?.PlayerPawn.Raw??uint.MaxValue;
        }
    }
    private void ClearLoot()
    {
        foreach(var handle in lootEntities.Keys) {var e=new CHandle<CBaseEntity>(handle).Value;if(e is {IsValid:true})e.Remove();}
        lootEntities.Clear();
    }
    private static void RestoreCvar(string name,int value) { var cv=ConVar<int>.Find(name);if(cv!=null)cv.Value=value; }
    private void StopGameplay()
    {
        ClearPricing();
        sessionLoaded=false;ClearPartnerVisuals();ClearLoot();observedWeapons.Clear();
        foreach(var (name,value) in savedCvars)RestoreCvar(name,value);
        savedCvars.Clear();
    }
}
