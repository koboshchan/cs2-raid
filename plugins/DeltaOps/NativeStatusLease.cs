namespace DeltaOps;

// Engine boundary shared by CSS ports and deterministic mocks. Full pawn serial
// validation belongs in samePawn, never an entity-index lookup.
public sealed class NativeStatusLease(Func<bool> samePawn,Func<float> read,Action<float> write,Action notify,float before,float applied)
{
    private bool restored,closed;
    public void Apply(){write(applied);notify();}
    public void Dispose(){
        if(closed)return;
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
