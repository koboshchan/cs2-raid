namespace DeltaOps;

// Ownership is installed in the session before Create is invoked. Create must return
// an inert entity with its full identity. It must not publish/spawn or deliver damage.
// After return even a failing ledger write must retain the creation handle for cleanup.
public interface INativeEntityCreation:INativeEntityBoundary
{
    NativeEntityTicket CreateInert();
    // Creation implementations must retain their allocated handle before any marker write.
    // This hook covers exceptions before a complete ticket could be returned.
    void CleanupIncompleteCreation();
    void Activate(NativeEntityTicket ticket);
}
public sealed class NativeEntityTransaction(NativeEntityEscrow escrow,INativeEntityCreation native):IDisposable
{
    private NativeEntityTicket? created;
    private bool recorded,closed;
    public void Activate()
    {
        if(closed||created!=null)throw new InvalidOperationException("Entity transaction reused");
        try{created=native.CreateInert();}
        catch{native.CleanupIncompleteCreation();throw;}
        escrow.Record(created);
        recorded=true;
        native.Activate(created);
    }
    public void Dispose()
    {
        if(closed)return;
        if(created==null)native.CleanupIncompleteCreation();
        if(created is {} ticket){
            if(recorded){
                if(!escrow.Release(ticket,native))throw new InvalidOperationException("Entity cleanup not confirmed");
            }else if(native.Exists(ticket.Serial)){
                // Failed persistence does not erase the exact identity returned by Create.
                if(!native.Matches(ticket))throw new InvalidOperationException("Uncommitted identity changed");
                native.Remove(ticket);
                if(native.Exists(ticket.Serial))throw new InvalidOperationException("Uncommitted removal not confirmed");
            }
        }
        closed=true;
    }
}
