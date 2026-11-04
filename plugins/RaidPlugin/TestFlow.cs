namespace Cs2Raid;

// Isolated fixture policy. This object NEVER receives the production game or a money bridge.
public sealed class TestFlow
{
    public GameplayState Teams { get; } = new();
    public const int Credits=15000;
    public static readonly IReadOnlyDictionary<string,int> Prices=new Dictionary<string,int>(StringComparer.Ordinal){
        ["glock"]=200,["hkp2000"]=200,["usp_silencer"]=200,["deagle"]=700,
        ["ak47"]=2700,["m4a1"]=3100,["m4a1_silencer"]=2900,["awp"]=4750,
        ["mp9"]=1250,["mac10"]=1050,["nova"]=1050,["ssg08"]=1700
    };
    public Dictionary<string,int> Wallet { get; }=[];
    public List<string> Ledger { get; }=[];
    public bool Manual {get;set;}
    public bool Join(string id){if(Teams.Phase!=GamePhase.Lobby||!Teams.Join(id))return false;Wallet.Add(id,Credits);return true;}
    public void Leave(string id){Teams.Leave(id);Wallet.Remove(id);}
    public bool Spend(string id,string item){
        if(Teams.Phase!=GamePhase.Buy || !Teams.Living(id) || !Prices.TryGetValue(item,out var price)||!Wallet.TryGetValue(id,out var balance)||balance<price)return false;
        Wallet[id]=balance-price;Ledger.Add($"{id} buy {item} debit {price} remain {Wallet[id]}");return true;
    }
    public void Delivery(string id,string item,bool delivered)=>Ledger.Add($"{id} deliver {item} {(delivered?"OK":"FAILED_OR_UNKNOWN_NO_REFUND")}");
    public int? Tick(DateTimeOffset now,bool ready)=>Teams.Tick(now,ready);
}
