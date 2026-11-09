namespace DeltaOps;

// Engine boundary shared by CSS ports and deterministic mocks. Full pawn serial
// validation belongs in samePawn, never an entity-index lookup.
public sealed class NativeStatusLease(Func<bool> samePawn,Func<float> read,Action<float> write,Action notify,float before,float applied)
{
    private bool restored,closed,attempted,mutationPossible;
    public void Apply(){
        if(closed||attempted)throw new InvalidOperationException("Status lease cannot be replayed");
        attempted=true;
        if(!float.IsFinite(before)||!float.IsFinite(applied)||!samePawn()||read()!=before)
            throw new InvalidOperationException("Status baseline or pawn changed");
        // A throwing native setter can already have committed.
        mutationPossible=true;
        write(applied);notify();
    }
    public void Dispose(){
        if(closed)return;
        if(!mutationPossible){closed=true;return;}
        if(!samePawn()){closed=true;return;}
        if(!restored){
            // Preserve a different writer. Identical-value concurrent writers cannot
            // be distinguished by the CSS scalar API; native acceptance remains gated.
            if(read()!=applied){closed=true;return;}
            write(before);restored=true;
        }
        notify(); // If this throws, retry notification without another scalar write.
        closed=true;
    }
}
public static class NativeConfigTransition
{
    public static void Validate(bool loaded,bool current,bool requested,object? tuning){
        ArgumentNullException.ThrowIfNull(tuning);
        if(loaded&&current!=requested)
            throw new InvalidOperationException("Ability mode changes require offline teardown and reviewed reload; runtime switch rejected.");
    }
}

// Full serial keys prevent a disconnected/replaced pawn inheriting suppression.
// Each effect releases only its own reference; another owner's count survives.
public sealed class NativeSuppressionLease(IDictionary<uint,int> counts,uint serial)
{
    private bool acquired;
    public void Acquire(){if(acquired)return;counts[serial]=counts.TryGetValue(serial,out var n)?n+1:1;acquired=true;}
    public void Dispose(){
        if(!acquired)return;
        if(counts.TryGetValue(serial,out var n)){if(n<=1)counts.Remove(serial);else counts[serial]=n-1;}
        acquired=false;
    }
}
