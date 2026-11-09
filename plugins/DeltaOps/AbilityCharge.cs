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
        Period=Math.Max(1,(long)Math.Ceiling(s.RechargeSeconds*1000));
        Next=Available<Capacity?checked(now+Period):0;
    }
    public void Advance(long now) {
        if(Next==0||now<Next)return;
        var count=Math.Min(Capacity-Available,1+(now-Next)/Period);
        Available+=(int)count; Next=Available==Capacity?0:checked(Next+count*Period);
    }
    public bool Ready(long now){Advance(now);return Available>0;}
    public bool Spend(long now){if(!Ready(now))return false;Available--;if(Next==0)Next=checked(now+Period);return true;}
}
