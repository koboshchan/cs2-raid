namespace Cs2Raid;

// Stage-2 adapter building block ONLY. Not wired to TestFlow, engine purchasing, production or SSG.
// A native adapter MUST authenticate input, obtain authoritative vanilla price, and prove delivery.
// Neither CanAcquire nor item_purchase alone supplies that proof. No enabling switch exists here.
public sealed class NativeBuyPolicy
{
    public enum Outcome { Reserved, Attempted, Delivered, Cancelled, Unknown }
    public sealed record Receipt(long Request,int Generation,uint Controller,string Item,int Price,Outcome State,uint? Entity);
    public readonly record struct Actor(int Generation,uint Controller,bool Dedicated,bool Scope,bool Human,bool Enrolled,bool Buy,bool Alive);
    private sealed record Entry(Receipt Receipt,Actor Actor);
    private readonly Dictionary<string,Entry> entries=new(StringComparer.Ordinal);
    private readonly HashSet<uint> deliveredEntities=[];
    private long next;
    public long Balance {get;private set;}
    public int Generation {get;}
    public uint Controller {get;}
    public IReadOnlyList<Receipt> Receipts=>entries.Values.Select(e=>e.Receipt).ToArray();
    public int HudProjection=>(int)Math.Clamp(Balance,0,65535);
    public string FullBalance=>Balance.ToString(System.Globalization.CultureInfo.InvariantCulture);

    // Vanilla catalog identifiers, not arbitrary entities. Native adapter still has to validate loadout/team.
    public static readonly IReadOnlySet<string> Catalog=new HashSet<string>(StringComparer.Ordinal){
        "glock","hkp2000","usp_silencer","elite","p250","tec9","fiveseven","cz75a","deagle","revolver",
        "nova","xm1014","mag7","sawedoff","m249","negev","mac10","mp9","mp7","mp5sd","ump45","p90","bizon",
        "galilar","famas","ak47","m4a1","m4a1_silencer","ssg08","aug","sg556","awp","g3sg1","scar20",
        "vest","vesthelm"
    };
    public static readonly IReadOnlySet<string> Knives=new HashSet<string>(StringComparer.Ordinal){"default","t","ct"};
    public static bool NeedsKnife(bool dedicated,bool scope,bool enabled,bool living,int ownedNativeKnives)
        => dedicated&&scope&&enabled&&living&&ownedNativeKnives==0;
    public static int MagazinePrice(int vanillaWeaponPrice) {
        if(vanillaWeaponPrice<=0)throw new ArgumentOutOfRangeException(nameof(vanillaWeaponPrice));
        // Positive HALF_UP /20, whole dollar. Long intermediate prevents integer overflow.
        return Math.Max(1,checked((int)(((long)vanillaWeaponPrice+10)/20)));
    }
    public static bool ReloadAffordable(long balance,int vanillaWeaponPrice,int reserve,int clip,int maxClip)
        => reserve==0&&clip>=0&&maxClip>0&&clip<maxClip&&balance>=MagazinePrice(vanillaWeaponPrice);
    public NativeBuyPolicy(int generation,uint controller,long balance){
        if(generation<0||controller==0||balance<0)throw new ArgumentOutOfRangeException();
        Generation=generation;Controller=controller;Balance=balance;
    }
    private bool Same(Actor a)=>!invalid&&a.Generation==Generation&&a.Controller==Controller;
    private bool Allowed(Actor a)=>Same(a)&&a.Dedicated&&a.Scope&&a.Human&&a.Enrolled&&a.Buy&&a.Alive;
    // Input key is adapter-generated, bounded and unique per authenticated input. Duplicate replay never reserves again.
    // A busy actor cannot reserve a second input until native delivery/cancel is resolved, including reentrant calls.
    public Receipt? Reserve(string input,Actor a,string item,int vanillaPrice){
        if(string.IsNullOrWhiteSpace(input)||input.Length>96||entries.ContainsKey(input)||entries.Count>=1024||
           !Allowed(a)||!Catalog.Contains(item)||vanillaPrice<=0||Balance<vanillaPrice||
           entries.Values.Any(e=>e.Receipt.State is Outcome.Reserved or Outcome.Attempted or Outcome.Unknown))return null;
        Balance-=vanillaPrice;
        var receipt=new Receipt(++next,Generation,Controller,item,vanillaPrice,Outcome.Reserved,null);
        entries.Add(input,new(receipt,a));return receipt;
    }
    public bool Begin(string input,Actor a){
        if(!Allowed(a)||!entries.TryGetValue(input,out var e)||e.Receipt.State!=Outcome.Reserved)return false;
        entries[input]=e with{Receipt=e.Receipt with{State=Outcome.Attempted}};return true;
    }
    // Cancel/refund is legal only BEFORE invoking a native grant/purchase. Native failures become Unknown.
    public bool Cancel(string input,Actor a){
        if(!Same(a)||!entries.TryGetValue(input,out var e)||e.Receipt.State!=Outcome.Reserved)return false;
        Balance=checked(Balance+e.Receipt.Price);
        entries[input]=e with{Receipt=e.Receipt with{State=Outcome.Cancelled}};return true;
    }
    // The adapter must establish native ownership, exact entity serial, no pre-existing inventory and item identity.
    // Those observations are mandatory arguments, NOT assertions this pure ledger can verify.
    public bool Delivered(string input,Actor a,uint entity,bool ownedNewMatchingEntity){
        if(!Allowed(a)||entity==0||!ownedNewMatchingEntity||deliveredEntities.Contains(entity)||
           !entries.TryGetValue(input,out var e)||e.Receipt.State!=Outcome.Attempted)return false;
        deliveredEntities.Add(entity);entries[input]=e with{Receipt=e.Receipt with{State=Outcome.Delivered,Entity=entity}};
        return true;
    }
    public bool Unknown(string input){
        if(!entries.TryGetValue(input,out var e)||e.Receipt.State!=Outcome.Attempted)return false;
        entries[input]=e with{Receipt=e.Receipt with{State=Outcome.Unknown}};return true;
    }
    // Phase/disconnect/round invalidation: unattempted reservations refund; attempted remain charged+locked Unknown.
    // Caller discards this actor-bound ledger after native reconciliation, never attaches it to a reconnect.
    public void Invalidate(){
        foreach(var key in entries.Keys.ToArray()){
            var e=entries[key];
            if(e.Receipt.State==Outcome.Reserved){Balance=checked(Balance+e.Receipt.Price);entries[key]=e with{Receipt=e.Receipt with{State=Outcome.Cancelled}};}
            else if(e.Receipt.State==Outcome.Attempted)entries[key]=e with{Receipt=e.Receipt with{State=Outcome.Unknown}};
        }
        invalid=true;
    }
    private bool invalid;
}
