namespace Cs2Raid;
// INACTIVE policy only. Does not hook reload, mutate ammo, debit TestFlow or grant reserves.
// Native start/cancel/consumption evidence is required before an adapter can use it.
public sealed class ReloadPolicy
{
    public enum State { Reserved, Attempted, Consumed, Cancelled, Unknown }
    public readonly record struct Actor(int Generation,uint Controller,uint Weapon,bool Dedicated,bool Scope,bool Human,bool Enrolled,bool Alive);
    public sealed record Entry(string Input,Actor Actor,int Price,int Magazine,State Status);
    public Actor Owner {get;}
    public long Balance {get;private set;}
    public Entry? Current {get;private set;}
    private bool invalid;
    private readonly HashSet<string> used=[];
    public ReloadPolicy(Actor owner,long balance){
        if(owner.Controller==0||owner.Weapon==0||owner.Generation<0||balance<0)throw new ArgumentOutOfRangeException();
        Owner=owner;Balance=balance;
    }
    private bool Same(Actor a)=>!invalid&&a==Owner;
    private bool Allowed(Actor a)=>Same(a)&&a.Dedicated&&a.Scope&&a.Human&&a.Enrolled&&a.Alive;
    public bool Reserve(string input,Actor actor,int vanillaPrice,int clip,int maxClip,int observedReserve){
        if(!Allowed(actor)||string.IsNullOrWhiteSpace(input)||input.Length>96||used.Count>=1024||used.Contains(input)||
           Current?.Status is State.Reserved or State.Attempted or State.Unknown||
           maxClip<=0||clip<0||clip>=maxClip||observedReserve!=0||vanillaPrice<=0)return false;
        var cost=NativeBuyPolicy.MagazinePrice(vanillaPrice);
        if(Balance<cost)return false;
        used.Add(input);Balance-=cost;Current=new(input,actor,cost,maxClip,State.Reserved);return true;
    }
    public bool Begin(Actor actor){
        if(!Allowed(actor)||Current is not {Status:State.Reserved} e)return false;
        Current=e with{Status=State.Attempted};return true;
    }
    public bool CancelBeforeNative(Actor actor){
        if(!Same(actor)||Current is not {Status:State.Reserved} e)return false;
        Balance=checked(Balance+e.Price);Current=e with{Status=State.Cancelled};return true;
    }
    public bool ConfirmConsumption(Actor actor,bool sameOwnedWeapon,bool nativeConsumptionProven){
        if(!Allowed(actor)||!sameOwnedWeapon||!nativeConsumptionProven||Current is not {Status:State.Attempted} e)return false;
        Current=e with{Status=State.Consumed};return true;
    }
    public void Invalidate(){
        if(Current is {Status:State.Reserved} r){Balance=checked(Balance+r.Price);Current=r with{Status=State.Cancelled};}
        else if(Current is {Status:State.Attempted} a)Current=a with{Status=State.Unknown};
        invalid=true;
    }
    // Zero means do not provision. This is not an engine ammo write.
    public int PlannedReserve(Actor actor)=>Allowed(actor)&&Current is {Status:State.Attempted} e?e.Magazine:0;
}
