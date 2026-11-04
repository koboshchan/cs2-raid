namespace Cs2Raid;
// INACTIVE policy only. Does not hook reload, mutate ammo, debit TestFlow or grant reserves.
// Native start/cancel/consumption evidence is required before an adapter can use it.
public sealed class ReloadPolicy
{
    public enum State { Reserved, Attempted, Consumed, Cancelled, Unknown }
    public readonly record struct Actor(int Generation,uint Controller,uint Weapon,bool Dedicated,bool Scope,bool Human,bool Enrolled,bool Alive);
    public sealed record Entry(string Input,Actor Actor,int Price,int Magazine,State Status);
    // A future adapter must implement both interfaces with build-bound engine evidence.
    // Policy callers cannot turn an observed event into a trusted catalog/commit token.
    public sealed record VanillaQuote(string Item,int Price,string EngineBuild,string CatalogDigest);
    public interface IVanillaCatalog {
        VanillaQuote? Resolve(string item);
    }
    public readonly record struct CommitProof(string Input,int Generation,uint Controller,uint Weapon,
        int BeforeClip,int AfterClip,int ReserveConsumed,string NativeOperation);
    public interface IVerifiedReloadAdapter {
        bool Verify(CommitProof proof,Entry reservation);
    }
    public Actor Owner {get;}
    public long Balance {get;private set;}
    public Entry? Current {get;private set;}
    private readonly object sync=new();
    private bool verifying;
    private bool invalid;
    private string? resolvedItem;
    private int beforeClip;
    private readonly HashSet<string> used=[];
    public ReloadPolicy(Actor owner,long balance){
        if(owner.Controller==0||owner.Weapon==0||owner.Generation<0||balance<0)throw new ArgumentOutOfRangeException();
        Owner=owner;Balance=balance;
    }
    private bool Same(Actor a)=>!invalid&&a==Owner;
    private bool Allowed(Actor a)=>Same(a)&&a.Dedicated&&a.Scope&&a.Human&&a.Enrolled&&a.Alive;
    public bool ReserveFromCatalog(string input,Actor actor,string item,IVanillaCatalog catalog,int clip,int maxClip,int reserve){
        lock(sync)return ReserveFromCatalogCore(input,actor,item,catalog,clip,maxClip,reserve);
    }
    public bool Begin(Actor actor){lock(sync)return !verifying&&BeginCore(actor);}
    public bool CancelBeforeNative(Actor actor){lock(sync)return !verifying&&CancelBeforeNativeCore(actor);}
    public bool ConfirmConsumption(Actor actor,CommitProof proof,IVerifiedReloadAdapter adapter){
        lock(sync)return ConfirmConsumptionCore(actor,proof,adapter);
    }
    public void Invalidate(){lock(sync)InvalidateCore();}
    public int PlannedReserve(Actor actor){lock(sync)return PlannedReserveCore(actor);}
    private bool ReserveFromCatalogCore(string input,Actor actor,string item,IVanillaCatalog catalog,int clip,int maxClip,int observedReserve){
        var quote=catalog.Resolve(item);
        // Unknown/zero/free prices deny paid reload until an explicit free-item policy exists.
        if(quote==null||quote.Item!=item||quote.Price<=0||string.IsNullOrWhiteSpace(quote.EngineBuild)||
           string.IsNullOrWhiteSpace(quote.CatalogDigest)||!NativeBuyPolicy.Catalog.Contains(item))return false;
        if(!Reserve(input,actor,quote.Price,clip,maxClip,observedReserve))return false;
        resolvedItem=item;beforeClip=clip;return true;
    }
    private bool Reserve(string input,Actor actor,int vanillaPrice,int clip,int maxClip,int observedReserve){
        if(verifying||!Allowed(actor)||string.IsNullOrWhiteSpace(input)||input.Length>96||used.Count>=1024||used.Contains(input)||
           Current?.Status is State.Reserved or State.Attempted or State.Unknown||
           maxClip<=0||clip<0||clip>=maxClip||observedReserve!=0||vanillaPrice<=0)return false;
        var cost=NativeBuyPolicy.MagazinePrice(vanillaPrice);
        if(Balance<cost)return false;
        used.Add(input);Balance-=cost;Current=new(input,actor,cost,maxClip,State.Reserved);return true;
    }
    private bool BeginCore(Actor actor){
        if(!Allowed(actor)||Current is not {Status:State.Reserved} e)return false;
        Current=e with{Status=State.Attempted};return true;
    }
    private bool CancelBeforeNativeCore(Actor actor){
        if(!Same(actor)||Current is not {Status:State.Reserved} e)return false;
        Balance=checked(Balance+e.Price);Current=e with{Status=State.Cancelled};return true;
    }
    private bool ConfirmConsumptionCore(Actor actor,CommitProof proof,IVerifiedReloadAdapter adapter){
        if(!Allowed(actor)||resolvedItem==null||Current is not {Status:State.Attempted} e||
           proof.Input!=e.Input||proof.Generation!=actor.Generation||proof.Controller!=actor.Controller||
           proof.Weapon!=actor.Weapon||proof.BeforeClip!=beforeClip||proof.AfterClip<=beforeClip||
           proof.AfterClip>e.Magazine||proof.ReserveConsumed!=proof.AfterClip-beforeClip||
           string.IsNullOrWhiteSpace(proof.NativeOperation)||verifying)return false;
        bool accepted;
        verifying=true;
        try{accepted=adapter.Verify(proof,e);}catch{InvalidateCore();throw;}finally{verifying=false;}
        if(!accepted||!Allowed(actor)||Current!=e)return false;
        Current=e with{Status=State.Consumed};return true;
    }
    private void InvalidateCore(){
        if(Current is {Status:State.Reserved} r){Balance=checked(Balance+r.Price);Current=r with{Status=State.Cancelled};}
        else if(Current is {Status:State.Attempted} a)Current=a with{Status=State.Unknown};
        invalid=true;
    }
    // Zero means do not provision. This is not an engine ammo write.
    private int PlannedReserveCore(Actor actor)=>Allowed(actor)&&Current is {Status:State.Attempted} e?e.Magazine:0;
}
