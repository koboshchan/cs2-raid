namespace DeltaOps;

// Cooperative ownership only: every participating writer must acquire this boundary
// before touching the shared scalar. This never claims protection against outsiders.
// Single server-thread use; no callbacks are invoked while a reservation is acquired.
public sealed class CooperativeStatusOwnership
{
    private readonly Dictionary<(string World,uint Pawn,string Field),Guid> owners=[];
    public IDisposable? Reserve(string world,uint pawn,string field)
    {
        if(string.IsNullOrWhiteSpace(world)||pawn is 0 or uint.MaxValue||
            string.IsNullOrWhiteSpace(field))throw new ArgumentException("Incomplete cooperative identity");
        var key=(world,pawn,field);
        if(owners.ContainsKey(key))return null;
        var token=Guid.NewGuid();owners.Add(key,token);
        return new OwnedAbilityEffect(()=>{
            if(owners.TryGetValue(key,out var current)&&current==token)owners.Remove(key);
        });
    }
    public IDisposable? Apply(string world,uint pawn,string field,Action mutation,Action rollback)
    {
        var reservation=Reserve(world,pawn,field);
        if(reservation==null)return null;
        // Retain reservation if rollback fails. A participating second writer must
        // not enter while cleanup is unresolved, even if scalar equals baseline.
        var effect=new CooperativeEffect(reservation,rollback);
        try{mutation();return effect;}
        catch{
            try{effect.Dispose();}catch{throw new CooperativeStatusCleanupException(effect);}
            throw;
        }
    }
    private sealed class CooperativeEffect(IDisposable reservation,Action rollback):IDisposable
    {
        private bool closed;
        public void Dispose(){if(closed)return;rollback();reservation.Dispose();closed=true;}
    }
}
public sealed class CooperativeStatusCleanupException(IDisposable pending):Exception(
    "Cooperative status rollback incomplete; caller must retain PendingCleanup")
{
    public IDisposable PendingCleanup {get;}=pending;
}
