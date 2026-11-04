namespace Cs2Raid;

// Isolated fixture policy. This object NEVER receives the production game or a money bridge.
public sealed class TestFlow
{
    internal static bool LobbyRecovery(bool callbackAllowed,bool lobby,bool scope,bool enrolled,bool alive)
        =>callbackAllowed&&lobby&&scope&&enrolled&&!alive;

    public GameplayState Teams { get; } = new();
    public const int Credits=15000;
    public static readonly IReadOnlyDictionary<string,int> Prices=new Dictionary<string,int>(StringComparer.Ordinal){
        ["glock"]=200,["hkp2000"]=200,["usp_silencer"]=200,["deagle"]=700,
        ["ak47"]=2700,["m4a1"]=3100,["m4a1_silencer"]=2900,["awp"]=4750,
        ["mp9"]=1250,["mac10"]=1050,["nova"]=1050,["ssg08"]=1700
    };
    public Dictionary<string,int> Wallet { get; }=[];
    public List<string> Ledger { get; }=[];
    // Separate fixture-only native enemies, never raid members or economic actors.
    public const int BotCapacity=32;
    public Dictionary<string,bool> Bots {get;}=[];
    public bool EnrollBot(string id,bool scoped,bool lobby){
        if(!scoped||!lobby||!id.StartsWith("bot:",StringComparison.Ordinal)||Bots.ContainsKey(id)||Bots.Count>=BotCapacity)return false;
        Bots.Add(id,true);return true;
    }
    public bool BotLiving(string id)=>Bots.GetValueOrDefault(id);
    public void KillBot(string id){if(Bots.ContainsKey(id))Bots[id]=false;}
    public bool EnemyDamage(string from,string to){
        if(Teams.Phase!=GamePhase.Active)return false;
        return from!=to&&(BotLiving(from)||Teams.Living(from))&&(BotLiving(to)||Teams.Living(to))&&
            (Bots.ContainsKey(from)||Bots.ContainsKey(to)||Teams.CanDamage(from,to));
    }
    public static bool LobbyBotCombat(bool lobby,bool enrolledVictim,bool enrolledAttacker,bool victimBot,bool attackerBot,bool victimAlive,bool attackerAlive)
        => lobby&&enrolledVictim&&enrolledAttacker&&(victimBot||attackerBot)&&victimAlive&&attackerAlive;
    public bool Manual {get;set;}
    public bool Join(string id){if(Teams.Phase!=GamePhase.Lobby||!Teams.Join(id))return false;Wallet.Add(id,Credits);return true;}
    // Single game-thread transaction; native setup occurs only AFTER all policy stores agree.
    public bool Enroll(TestModeState mode,string id,bool authenticated){
        if(!mode.Enabled||mode.Phase!=TestModeState.Stage.Lobby||!authenticated||string.IsNullOrEmpty(id))return false;
        if(mode.Participants.ContainsKey(id))return Teams.Players.ContainsKey(id)&&Wallet.ContainsKey(id);
        if(!Join(id))return false; // Shared GameplayState cap7; nothing in fixture mode changed.
        try{mode.Join(id,true);return true;}
        catch{Teams.Leave(id);Wallet.Remove(id);throw;}
    }
    // Unknown non-world sources fail closed; they must never inherit world damage permission.
    public static bool DamageAllowed(bool active,bool enrolledVictim,bool enrolledAttacker,bool actualWorld,bool living,bool canDamage)
        => active&&enrolledVictim&&living&&(enrolledAttacker?canDamage:actualWorld);
    public void Leave(string id){Teams.Leave(id);Wallet.Remove(id);}
    public bool Spend(string id,string item){
        if(Teams.Phase!=GamePhase.Buy || !Teams.Living(id) || !Prices.TryGetValue(item,out var price)||!Wallet.TryGetValue(id,out var balance)||balance<price)return false;
        Wallet[id]=balance-price;Ledger.Add($"{id} buy {item} debit {price} remain {Wallet[id]}");return true;
    }
    public void Delivery(string id,string item,bool delivered)=>Ledger.Add($"{id} deliver {item} {(delivered?"OK":"FAILED_OR_UNKNOWN_NO_REFUND")}");
    public int? Tick(DateTimeOffset now,bool ready)=>Teams.Tick(now,ready);
}
