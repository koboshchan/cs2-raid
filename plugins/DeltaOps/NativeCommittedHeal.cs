namespace DeltaOps;

// A committed scalar write is not a reversible status lease. In particular, a
// failed network notification must never replay healing or undo later damage.
// This class supplies local failure semantics, not DF healing behavior proof.
public sealed class NativeCommittedHeal(Func<bool> samePawn,Func<int> health,
    Func<int> maximum,Action<int> write,Action notify,int amount):IDisposable
{
    private bool attempted,notificationPending,closed;
    public void Apply()
    {
        if(closed||attempted)throw new InvalidOperationException("Heal mutation cannot be replayed");
        attempted=true;
        if(!samePawn())throw new InvalidOperationException("Heal pawn changed");
        var before=health();var cap=maximum();
        if(amount<=0||before<=0||cap<=0||before>=cap)
            throw new InvalidOperationException("Heal no longer admissible");
        // Set before setter: it may commit and then throw. Cleanup can only notify,
        // never retry the write and never infer that a throwing setter did nothing.
        notificationPending=true;
        write((int)Math.Min((long)cap,(long)before+amount));
        Notify();
    }
    private void Notify(){if(notificationPending){notify();notificationPending=false;}}
    public void Dispose()
    {
        if(closed)return;
        if(!samePawn()){closed=true;return;}
        Notify();
        closed=true;
    }
}
