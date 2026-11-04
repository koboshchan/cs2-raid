namespace Cs2Raid;

public enum GamePhase { Lobby, CharacterSelect, Buy, Countdown, Freeze, Active, Closed }
public sealed class Member(string id)
{
    public string Id { get; } = id;
    public int? Team { get; internal set; }
    public int? Character { get; internal set; }
    public bool Alive { get; internal set; } = true;
    public bool Extracted { get; internal set; }
    public long Cash { get; internal set; }
    public long MoneyExtractedCents { get; internal set; }
    public long ExtractedOrder { get; internal set; }
}
public enum LootKind { Item, Safe, MoneyBag, Equipment }
public sealed record GroundLoot(string Id, Position Position, LootKind Kind, long Min, long Max, double HoldSeconds = 0);
public sealed record ExtractionIntent(string Id, string Player, long Cash);
// Single game-thread owner. No external currency, native handles or persistent inventory.
public sealed class GameplayState
{
    public const int BuySeconds = 45;
    public bool BuyOpen(DateTimeOffset now) => Phase==GamePhase.Buy && StartsAt is {} end && now<end && now>=end.AddSeconds(-BuySeconds);
    public static readonly string[] Characters = ["D-Wolf", "Stinger", "Luna", "Vyron", "Hackclaw", "Shepherd"];
    public string MatchId { get; } = Guid.NewGuid().ToString("N");
    public Dictionary<string, Member> Players { get; } = [];
    public Dictionary<string, GroundLoot> Ground { get; } = [];
    private readonly HashSet<(string From, string To)> requests = [];
    private readonly Dictionary<string,(string Loot,DateTimeOffset Since)> holds = [];
    private readonly HashSet<string> usedLoot = [];
    private readonly HashSet<int> levers = [];
    private readonly Dictionary<string,ExtractionIntent> extractionIntents = [];
    private int nextTeam;
    private long extractionOrder;
    private readonly HashSet<string> cashIns=[];
    public string ExtractionWindowId { get; private set; } = "";
    internal bool ApplyCashIn(string id,long dollars) {
        if(Phase!=GamePhase.Buy || !Living(id) || dollars is <1 or >15000 || Players[id].Cash!=0 || !cashIns.Add(id))return false;
        Players[id].Cash=dollars;return true;
    }
    internal bool IsIssuedExtraction(ExtractionIntent intent) => extractionIntents.TryGetValue(intent.Player,out var issued) && issued==intent;
    internal bool ConfirmVerifiedExtraction(ExtractionIntent intent,long? verifiedMoneyCents=null) {
        if(verifiedMoneyCents is <0 || Phase!=GamePhase.Active || !IsIssuedExtraction(intent) || !Living(intent.Player) || Players[intent.Player].Cash!=intent.Cash || ExtractionAt==null)return false;
        var p=Players[intent.Player];p.MoneyExtractedCents=verifiedMoneyCents??checked(intent.Cash*100);p.ExtractedOrder=++extractionOrder;p.Extracted=true;p.Cash=0;holds.Remove(p.Id);extractionIntents.Remove(p.Id);return true;
    }
    public GamePhase Phase { get; private set; }
    public DateTimeOffset? StartsAt { get; private set; }
    public DateTimeOffset? EndsAt { get; private set; }
    public DateTimeOffset? ExtractionAt { get; private set; }
    public bool Join(string id)
    {
        if (Phase is not (GamePhase.Lobby or GamePhase.CharacterSelect) || Players.Count >= 7 || Players.ContainsKey(id)) return false;
        Players.Add(id, new(id)); Phase=GamePhase.Lobby; return true;
    }
    public void Leave(string id)
    {
        if (!Players.TryGetValue(id, out var p)) return;
        if (Phase is GamePhase.Active or GamePhase.Freeze) { p.Alive=false; p.Cash=0; holds.Remove(id); return; }
        if (Phase == GamePhase.Closed) return;
        Cancel(id); Players.Remove(id); ForgetRequests(id);
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
        // Once buying starts, no cancellation/re-entry can reset purchased items or cash.
        if (Phase is not (GamePhase.Lobby or GamePhase.CharacterSelect) || !Players.TryGetValue(id, out var p)) return [];
        var group = p.Team is int t ? Players.Values.Where(x=>x.Team==t).ToArray() : [p];
        foreach (var m in group) { m.Team=null; m.Character=null; ForgetRequests(m.Id); }
        Phase=GamePhase.Lobby; StartsAt=null; return group.Select(x=>x.Id).ToArray();
    }
    private void ForgetRequests(string id) => requests.RemoveWhere(x=>x.From==id || x.To==id);
    public bool SelectCharacter(string id,int number)
    {
        if (Phase!=GamePhase.CharacterSelect || !Players.TryGetValue(id,out var p) || p.Team==null || number<1 || number>Characters.Length) return false;
        p.Character=number; return true;
    }
    public int? Tick(DateTimeOffset now, bool configured)
    {
        if (!configured && Phase is not (GamePhase.Lobby or GamePhase.Closed)) return null; // adapter must halt on lost configuration
        if (Phase==GamePhase.Lobby && configured && Players.Count>0 && Players.Values.All(x=>x.Team!=null))
            Phase=GamePhase.CharacterSelect;
        if (Phase==GamePhase.CharacterSelect && Players.Values.All(x=>x.Character!=null)) { Phase=GamePhase.Buy; StartsAt=now.AddSeconds(BuySeconds); }
        else if (Phase==GamePhase.Buy && now>=StartsAt) { Phase=GamePhase.Countdown; StartsAt=now.AddSeconds(5); }
        else if (Phase==GamePhase.Countdown && now>=StartsAt) { Phase=GamePhase.Freeze; StartsAt=now.AddSeconds(10); }
        else if (Phase==GamePhase.Freeze && now>=StartsAt) { Phase=GamePhase.Active; StartsAt=null; EndsAt=now.AddMinutes(30); }
        if (Phase==GamePhase.Active && now>=EndsAt) {
            foreach(var p in Players.Values.Where(x=>!x.Extracted)) { p.Alive=false; p.Cash=0; }
            holds.Clear(); Phase=GamePhase.Closed;
        }
        if (Phase==GamePhase.Active && Players.Values.All(x=>!x.Alive || x.Extracted)) Phase=GamePhase.Closed;
        return StartsAt is {} at ? Math.Max(0,(int)Math.Ceiling((at-now).TotalSeconds)) : null;
    }
    public bool TryPurchase(string id,int price)
    {
        if(Phase!=GamePhase.Buy || !Living(id) || price<0 || Players[id].Cash<price)return false;
        Players[id].Cash-=price;return true;
    }
    public bool SameTeam(string a,string b) => Players.TryGetValue(a,out var x) && Players.TryGetValue(b,out var y) && x.Team!=null && x.Team==y.Team;
    public bool CanSeePartner(string viewer,string owner) => viewer!=owner && Phase!=GamePhase.Closed && Living(viewer) && Living(owner) && SameTeam(viewer,owner);
    public bool CanDamage(string a,string b) => Phase==GamePhase.Active && Living(a) && Living(b) && !SameTeam(a,b);
    public bool Living(string id) => Players.TryGetValue(id,out var p) && p.Alive && !p.Extracted;
    public bool Kill(string victim,string? killer) // killer intentionally earns nothing: cash becomes a bag
    {
        if (Phase!=GamePhase.Active || !Living(victim)) return false;
        var v=Players[victim]; v.Cash=0; v.Alive=false; holds.Remove(victim); return true;
    }
    public bool DieWithBag(string victim,Position position)
    {
        if(Phase!=GamePhase.Active || !Living(victim) || !position.Valid)return false;
        var cash=Players[victim].Cash;
        if(cash>0 && !AddLoot(new("bag:"+victim,position,LootKind.MoneyBag,cash,cash)))return false;
        return Kill(victim,null);
    }
    public string? Spectate(string id) => Players.TryGetValue(id,out var p) && p.Team!=null ? Players.Values.FirstOrDefault(x=>x.Id!=id && x.Team==p.Team && Living(x.Id))?.Id : null;
    public bool AddLoot(GroundLoot loot)
    {
        if(!Enum.IsDefined(loot.Kind) || Phase!=GamePhase.Active || !loot.Position.Valid || string.IsNullOrWhiteSpace(loot.Id) || loot.Min<0 || loot.Max<loot.Min || (loot.Kind==LootKind.Safe && loot.Max>=int.MaxValue) ||
            !double.IsFinite(loot.HoldSeconds) || loot.HoldSeconds<0 ||
            (loot.Kind!=LootKind.Safe && (loot.Min!=loot.Max || loot.HoldSeconds!=0)) ||
            Ground.ContainsKey(loot.Id) || usedLoot.Contains(loot.Id))return false;
        Ground.Add(loot.Id,loot);return true;
    }
    public void ReleaseUse(string id) => holds.Remove(id);
    public bool UseLoot(string id,string key,Position position,bool holding,DateTimeOffset now,Random random,double range=96)
    {
        if(!holding || Phase!=GamePhase.Active || !Living(id) || !Ground.TryGetValue(key,out var loot) ||
           !position.Within(loot.Position,range)) { holds.Remove(id);return false; }
        if(!holds.TryGetValue(id,out var hold) || hold.Loot!=key) { hold=(key,now);holds[id]=hold; }
        if((now-hold.Since).TotalSeconds<loot.HoldSeconds)return false;
        var value=loot.Min==loot.Max ? loot.Min : random.NextInt64(loot.Min,loot.Max+1);
        if(Players[id].Cash>long.MaxValue-value)return false;
        Players[id].Cash+=value; Ground.Remove(key);usedLoot.Add(key);
        foreach(var player in holds.Where(x=>x.Value.Loot==key).Select(x=>x.Key).ToArray())holds.Remove(player);
        return true;
    }
    public bool PullLever(string id,int number,DateTimeOffset now)
    {
        if(Phase!=GamePhase.Active || !Living(id) || number<1 || number>4 || ExtractionAt!=null || !levers.Add(number))return false;
        if(levers.Count>=2){ExtractionAt=now.AddMinutes(5);ExtractionWindowId=Guid.NewGuid().ToString("N");}
        return true;
    }
    public ExtractionIntent[] ExtractionCandidates(DateTimeOffset now,IEnumerable<string> inZone)
    {
        if(Phase!=GamePhase.Active || ExtractionAt is not {} at || now<at || now>=EndsAt)return [];
        var candidates=inZone.Distinct().Where(Living).Select(id=>new ExtractionIntent(MatchId+":"+id,id,Players[id].Cash)).ToArray();
        foreach(var intent in candidates)extractionIntents[intent.Player]=intent;
        return candidates;
    }
    public bool FinishExtractionWindow(DateTimeOffset now)
    {
        if(ExtractionAt is not {} at || now<at)return false;
        ExtractionAt=null;levers.Clear();extractionIntents.Clear();return true;
    }
}
public sealed record Position(float X,float Y,float Z,float Yaw=0)
{
    public bool Valid => float.IsFinite(X)&&float.IsFinite(Y)&&float.IsFinite(Z)&&float.IsFinite(Yaw);
    public bool Within(Position other,double range) => Valid && other.Valid && double.IsFinite(range) && range>=0 &&
        Math.Pow((double)X-other.X,2)+Math.Pow((double)Y-other.Y,2)+Math.Pow((double)Z-other.Z,2)<=range*range;
}
public sealed record MapLoot(string Targetname,LootKind Kind,double SpawnRate,int Value=0,int Min=0,int Max=0,double HoldSeconds=0);
public sealed record MapLever(int Number,string Targetname);
public sealed class GameplayConfig
{
    public bool Enabled { get; set; }
    public bool CustomPricingEnabled { get; set; } = false;
    public static int NativeSpend(int projected,int current) => Math.Max(0,projected-Math.Max(0,current));
    public static bool Utility(string name) => name.Replace("weapon_","").ToLowerInvariant() is "flashbang" or "smokegrenade" or "hegrenade" or "molotov" or "incgrenade" or "decoy";
    public bool CoordinatesConfirmed { get; set; }
    public bool FinancialNativeVerified { get; set; } = false;
    public bool NativeFlowVerified { get; set; } // explicit live-QA gate, never enabled by example
    public Position Lobby { get; set; } = new(0,0,0);
    public Position ReadyPlatform { get; set; } = new(0,0,0);
    public Position[] Spawns { get; set; } = [];
    public MapLoot[] MapLoot { get; set; } = [];
    public MapLever[] Levers { get; set; } = [];
    public string ExtractionTrigger { get; set; } = "";
    public Position? ExtractionCenter { get; set; }
    public double ExtractionRadius { get; set; }
    public bool Ready(int teams) => Enabled && NativeFlowVerified && CoordinatesConfirmed && Lobby.Valid && ReadyPlatform.Valid && Spawns.Length>=teams && Spawns.Length>0 && Spawns.All(x=>x.Valid);
    public void Validate()
    {
        if (!Lobby.Valid || !ReadyPlatform.Valid || Spawns.Any(x=>!x.Valid) ||
            MapLoot.Select(x=>x.Targetname).Distinct().Count()!=MapLoot.Length ||
            MapLoot.Any(x=>!Enum.IsDefined(x.Kind) || string.IsNullOrWhiteSpace(x.Targetname) || !double.IsFinite(x.SpawnRate) || x.SpawnRate<0 || x.SpawnRate>1 ||
                x.Kind is LootKind.MoneyBag or LootKind.Equipment || x.Value<0 || x.Min<0 || x.Max<x.Min || x.Max==int.MaxValue ||
                !double.IsFinite(x.HoldSeconds) || x.HoldSeconds<0 || (x.Kind==LootKind.Item && (x.HoldSeconds!=0 || x.Min!=0 || x.Max!=0))) ||
            Levers.Any(x=>x.Number<1 || x.Number>4 || string.IsNullOrWhiteSpace(x.Targetname)) ||
            Levers.Select(x=>x.Number).Distinct().Count()!=Levers.Length || Levers.Select(x=>x.Targetname).Distinct().Count()!=Levers.Length ||
            !double.IsFinite(ExtractionRadius) || ExtractionRadius<0 || ExtractionCenter is {Valid:false})
            throw new InvalidOperationException("Invalid raid gameplay config");
    }
}
