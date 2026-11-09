namespace DeltaOps;

// A session survives death/reconnect. Only a full caller-owned round reset discards it.
// Engine adapters must capture pawn serial and return an owned, retry-safe effect.
public sealed class InterpretedAbilitySession(int operatorId, long now)
{
    private readonly Dictionary<int,AbilityCharge> charges = InterpretedR2.Skills.Values
        .Where(s => s.Operator == operatorId && !s.Passive)
        .ToDictionary(s=>s.Id,s=>new AbilityCharge(s,now));
    private readonly List<IDisposable> effects=[];
    public bool Quarantined {get; private set;}
    public bool Alive {get;private set;}=true;
    public int Operator {get;}=StableOperatorIdentity.SelectableEnum(operatorId)
        ?operatorId:throw new ArgumentOutOfRangeException(nameof(operatorId));
    public bool Activate(int skill,long at,Func<bool> admission,Func<IDisposable?> enact)
    {
        if(Quarantined||!Alive||!charges.TryGetValue(skill,out var charge)||!charge.Ready(at)||!admission())return false;
        try {
            var effect=enact(); // null contract means transactional rejection, no native side effects.
            if(effect==null)return false;
            effects.Add(effect);
            return charge.Spend(at);
        }catch {
            // Never mint another effect after a native call with unknown outcome.
            Quarantined=true;return false;
        }
    }
    public bool Revoke()
    {
        Alive=false;
        for(var i=effects.Count-1;i>=0;i--) {
            try{effects[i].Dispose();effects.RemoveAt(i);}catch{Quarantined=true;}
        }
        return effects.Count==0;
    }
    public void Resume()=>Alive=!Quarantined;
}
