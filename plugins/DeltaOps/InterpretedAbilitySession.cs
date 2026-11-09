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
    private bool activating,revoking;
    private long generation;
    public bool Activate(int skill,long at,Func<bool> admission,Func<IDisposable?> enact)
    {
        if(activating||revoking||Quarantined||!Alive||!charges.TryGetValue(skill,out var charge))return false;
        activating=true;var epoch=generation;var snapshot=charge.Snapshot();
        try {
            if(!admission()||epoch!=generation||Quarantined||!Alive||!charge.Ready(at))return false;
            snapshot=charge.Snapshot();
            if(!charge.Spend(at))return false;
            var effect=enact();
            if(effect==null){charge.Restore(snapshot);return false;}
            // Own first, including stale/failed cleanup; Revoke during enact cannot orphan it.
            effects.Add(effect);
            if(epoch!=generation||Quarantined||!Alive||!admission()||epoch!=generation||Quarantined||!Alive){
                try{effect.Dispose();effects.Remove(effect);}catch{Quarantined=true;}
                charge.Restore(snapshot);return false;
            }
            return true;
        }catch {
            // Unknown native outcome consumes reserved charge and fences further activation.
            Quarantined=true;return false;
        }finally{activating=false;}
    }
    public int Available(int skill,long at){if(!charges.TryGetValue(skill,out var c))return 0;c.Advance(at);return c.Available;}
    public void Quarantine(){generation++;Quarantined=true;}
    public bool Revoke()
    {
        generation++;Alive=false;
        if(revoking)return false;
        revoking=true;
        try{
            foreach(var effect in effects.ToArray()){
                try{effect.Dispose();effects.Remove(effect);}catch{Quarantined=true;}
            }
            return effects.Count==0&&!activating;
        }finally{revoking=false;}
    }
    public void Resume(){if(!activating&&!revoking&&effects.Count==0)Alive=!Quarantined;}
}
