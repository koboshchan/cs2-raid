namespace Cs2Raid;

public enum GamePhase { Lobby, Countdown, Active, Closed }
public sealed class Member(string id)
{
    public string Id { get; } = id;
    public int? Team { get; internal set; }
    public bool Alive { get; internal set; } = true;
    public long Cash { get; internal set; }
}
public sealed record GroundGun(string Entity, int Team, int Value);
// Single game-thread owner. No native handles or external currency in this model.
public sealed class GameplayState
{
    public Dictionary<string, Member> Players { get; } = [];
    private readonly HashSet<(string From, string To)> requests = [];
    private readonly Dictionary<uint, GroundGun> guns = [];
    private readonly HashSet<uint> awardedGuns = [];
    private readonly HashSet<string> claimedLoot = [];
    private int nextTeam;
    public GamePhase Phase { get; private set; }
    public DateTimeOffset? StartsAt { get; private set; }
    public DateTimeOffset? EndsAt { get; private set; }
    public bool Join(string id)
    {
        if (Phase is GamePhase.Active or GamePhase.Closed || Players.ContainsKey(id)) return false;
        Players.Add(id, new(id)); InvalidateCountdown(); return true;
    }
    public void Leave(string id)
    {
        if (!Players.TryGetValue(id, out var p)) return;
        if (Phase == GamePhase.Active) { p.Alive = false; p.Cash = 0; return; }
        if (Phase == GamePhase.Closed) return;
        Cancel(id); Players.Remove(id); ForgetRequests(id); InvalidateCountdown();
    }
    public bool ShootRequest(string from, string to)
    {
        if (Phase != GamePhase.Lobby || from == to || !Players.TryGetValue(from, out var a) || !Players.TryGetValue(to, out var b) || a.Team != null || b.Team != null) return false;
        requests.Add((from,to));
        if (!requests.Contains((to,from))) return false;
        a.Team = b.Team = ++nextTeam; ForgetRequests(from); ForgetRequests(to); return true;
    }
    public bool Solo(string id)
    {
        if (Phase != GamePhase.Lobby || !Players.TryGetValue(id, out var p) || p.Team != null) return false;
        p.Team = ++nextTeam; ForgetRequests(id); return true;
    }
    public string[] Cancel(string id)
    {
        if (Phase is GamePhase.Active or GamePhase.Closed || !Players.TryGetValue(id, out var p)) return [];
        var group = p.Team is int t ? Players.Values.Where(x=>x.Team==t).ToArray() : [p];
        foreach (var m in group) { m.Team = null; ForgetRequests(m.Id); }
        InvalidateCountdown(); return group.Select(x=>x.Id).ToArray();
    }
    private void ForgetRequests(string id) => requests.RemoveWhere(x=>x.From==id || x.To==id);
    private void InvalidateCountdown() { if (Phase == GamePhase.Countdown) Phase = GamePhase.Lobby; StartsAt = null; }
    public int? Tick(DateTimeOffset now, bool configured)
    {
        if (Phase == GamePhase.Lobby && configured && Players.Count>0 && Players.Values.All(x=>x.Team!=null))
        { Phase=GamePhase.Countdown; StartsAt=now.AddSeconds(5); }
        if (Phase == GamePhase.Countdown)
        {
            if (!configured) { InvalidateCountdown(); return null; }
            if (now>=StartsAt) { Phase=GamePhase.Active; EndsAt=now.AddMinutes(30); foreach(var p in Players.Values) p.Cash=0; return 0; }
            return (int)Math.Ceiling((StartsAt!.Value-now).TotalSeconds);
        }
        if (Phase==GamePhase.Active && (now>=EndsAt || Players.Values.All(x=>!x.Alive))) Phase=GamePhase.Closed;
        return null;
    }
    public bool TryPurchase(string id,int price)
    {
        if(Phase!=GamePhase.Active || !Living(id) || price<0 || Players[id].Cash<price)return false;
        Players[id].Cash-=price;return true;
    }
    public bool SameTeam(string a,string b) => Players.TryGetValue(a,out var x) && Players.TryGetValue(b,out var y) && x.Team!=null && x.Team==y.Team;
    public bool CanSeePartner(string viewer,string owner) => viewer!=owner && Phase!=GamePhase.Closed && Living(viewer) && Living(owner) && SameTeam(viewer,owner);
    public bool CanDamage(string a,string b) => Phase==GamePhase.Active && Living(a) && Living(b) && !SameTeam(a,b);
    public bool Living(string id) => Players.TryGetValue(id,out var p) && p.Alive;
    public bool Kill(string victim,string? killer)
    {
        if (Phase!=GamePhase.Active || !Living(victim)) return false;
        var v=Players[victim];
        if (killer!=null && CanDamage(killer,victim)) Players[killer].Cash=checked(Players[killer].Cash+v.Cash);
        v.Cash=0; v.Alive=false; return true;
    }
    public string? Spectate(string id) => Players.TryGetValue(id,out var p) ? Players.Values.FirstOrDefault(x=>x.Id!=id && x.Team==p.Team && x.Alive)?.Id : null;
    public void Drop(uint handle,string owner,string entity,int value)
    {
        if (Phase==GamePhase.Active && Players.TryGetValue(owner,out var p) && p.Team is int t && value>0 && !awardedGuns.Contains(handle))
            guns.TryAdd(handle,new(entity,t,value));
    }
    public bool Pickup(uint handle,string id)
    {
        if (Phase!=GamePhase.Active || !Living(id) || !guns.Remove(handle,out var gun)) return false;
        awardedGuns.Add(handle);
        // First pickup consumes the award even for the original team; no drop/pickup farming.
        if (Players[id].Team==gun.Team) return false;
        Players[id].Cash=checked(Players[id].Cash+gun.Value); return true;
    }
    public bool Claim(string key,string id,int min,int max,Random random)
    {
        if (Phase!=GamePhase.Active || !Living(id) || min<0 || max<min || max==int.MaxValue || claimedLoot.Contains(key)) return false;
        var cash=checked(Players[id].Cash+random.Next(min,max+1));
        claimedLoot.Add(key); Players[id].Cash=cash; return true;
    }
}
public sealed record Position(float X,float Y,float Z,float Yaw=0)
{
    public bool Valid => float.IsFinite(X)&&float.IsFinite(Y)&&float.IsFinite(Z)&&float.IsFinite(Yaw);
}
public sealed record LootTier(int Min,int Max);
public sealed record LootPoint(string Id,Position Position,string Tier);
public sealed class GameplayConfig
{
    public bool Enabled { get; set; }
    public bool CustomPricingEnabled { get; set; } = false;
    public static int NativeSpend(int projected,int current) => Math.Max(0,projected-Math.Max(0,current));
    public bool CoordinatesConfirmed { get; set; }
    public Position Lobby { get; set; } = new(0,0,0);
    public Position ReadyPlatform { get; set; } = new(0,0,0);
    public Position[] Spawns { get; set; } = [];
    public Dictionary<string,LootTier> Tiers { get; set; } = [];
    public LootPoint[] Loot { get; set; } = [];
    // Provisional interpretation ONLY: independent per-object spawn probability.
    public double LootSpawnChance { get; set; } = 0.01;
    public string LootModel { get; set; } = "models/props/de_dust/hr_dust/dust_crates/dust_crate_style_01_64x64x64.vmdl";
    public bool Ready(int teams) => Enabled && CoordinatesConfirmed && Lobby.Valid && ReadyPlatform.Valid && Spawns.Length>=teams && Spawns.All(x=>x.Valid);
    public void Validate()
    {
        if(!double.IsFinite(LootSpawnChance)||LootSpawnChance<0||LootSpawnChance>1 || !Lobby.Valid || !ReadyPlatform.Valid || Spawns.Any(x=>!x.Valid) || Loot.Select(x=>x.Id).Distinct().Count()!=Loot.Length || Loot.Any(x=>string.IsNullOrWhiteSpace(x.Id)||!x.Position.Valid||!Tiers.ContainsKey(x.Tier)) || Tiers.Values.Any(x=>x.Min<0||x.Max<x.Min||x.Max==int.MaxValue)) throw new InvalidOperationException("Invalid raid gameplay config");
    }
}
