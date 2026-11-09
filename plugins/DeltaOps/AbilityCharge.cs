namespace DeltaOps;
// Pure monotonic-time lifecycle, separate from engine effects and immutable raw evidence.
public sealed class AbilityCharge
{
    public int Available {get; private set;}
    public int Capacity {get;}
    public long Period {get;}
    public long Next {get;private set;}
    public AbilityCharge(InterpretedSkill s,long now) {
        Capacity=s.Capacity; Available=Math.Clamp(s.Initial,0,Capacity);
        // No invented 1ms fallback for zero/unresolved recharge.
        Period=double.IsFinite(s.RechargeSeconds)&&s.RechargeSeconds>0?
            checked((long)Math.Ceiling(s.RechargeSeconds*1000)):0;
        Next=Period>0&&Available<Capacity?checked(now+Period):0;
    }
    public void Advance(long now) {
        if(Period<=0||Next==0||now<Next)return;
        var count=Math.Min(Capacity-Available,1+(now-Next)/Period);
        Available+=(int)count; Next=Available==Capacity?0:checked(Next+count*Period);
    }
    internal (int Available,long Next) Snapshot()=> (Available,Next);
    internal void Restore((int Available,long Next) state){Available=state.Available;Next=state.Next;}
    public bool Ready(long now){Advance(now);return Available>0;}
    public bool Spend(long now){if(!Ready(now))return false;Available--;if(Period>0&&Next==0)Next=checked(now+Period);return true;}
}
