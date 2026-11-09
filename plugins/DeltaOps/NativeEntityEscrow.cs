namespace DeltaOps;

// Persist only identity, never delegates or engine pointers. The store must atomically
// replace + flush its snapshot. A failed write is a failed transaction, not permission
// to mutate. World identifies one engine process/map generation; Marker is a random
// per-entity name installed while the entity is still inert.
public sealed record NativeEntityTicket(string World, uint Serial, string Marker, string Kind);
public interface INativeEntityBoundary
{
    bool Exists(uint serial);
    bool Matches(NativeEntityTicket ticket);
    void Remove(NativeEntityTicket ticket);
}
public sealed class NativeEntityEscrow
{
    private readonly string world;
    private readonly Action<IReadOnlyList<NativeEntityTicket>> persist;
    private readonly Dictionary<uint,NativeEntityTicket> tickets=[];
    private bool busy;
    public NativeEntityEscrow(string world,IEnumerable<NativeEntityTicket> saved,
        Action<IReadOnlyList<NativeEntityTicket>> persist)
    {
        if(string.IsNullOrWhiteSpace(world))throw new ArgumentException("Missing world identity");
        this.world=world;this.persist=persist;
        foreach(var ticket in saved){
            // Old map/process records can never authorize operations in a new world.
            if(ticket.World!=world)continue;
            Validate(ticket);
            if(!tickets.TryAdd(ticket.Serial,ticket))throw new ArgumentException("Duplicate serial ticket");
        }
    }
    public int Pending=>tickets.Count;
    private static void Validate(NativeEntityTicket t){
        if(t.Serial==0||t.Serial==uint.MaxValue||string.IsNullOrWhiteSpace(t.World)||
            !t.Marker.StartsWith("deltaops-owned-",StringComparison.Ordinal)||
            !Guid.TryParseExact(t.Marker["deltaops-owned-".Length..],"N",out _)||
            string.IsNullOrWhiteSpace(t.Kind))
            throw new ArgumentException("Incomplete native identity");
    }
    public void Record(NativeEntityTicket ticket)
    {
        Validate(ticket);
        if(busy||ticket.World!=world)throw new InvalidOperationException("Escrow transaction rejected");
        if(tickets.TryGetValue(ticket.Serial,out var old)){
            if(old!=ticket)throw new InvalidOperationException("Serial already has another owner");
            return;
        }
        busy=true;
        try{
            var next=tickets.Values.Append(ticket).ToArray();
            persist(Array.AsReadOnly(next)); // Durable BEFORE publish/spawn/damage/timer.
            tickets.Add(ticket.Serial,ticket);
        }finally{busy=false;}
    }
    public bool Release(NativeEntityTicket ticket,INativeEntityBoundary native)
    {
        if(busy||ticket.World!=world)return false;
        if(!tickets.TryGetValue(ticket.Serial,out var actual))return true;
        if(actual!=ticket)return false;
        busy=true;
        try{
            if(native.Exists(ticket.Serial)){
                if(!native.Matches(ticket))return false;
                native.Remove(ticket);
                if(native.Exists(ticket.Serial))return false;
            }
            var remaining=tickets.Values.Where(t=>t!=ticket).ToArray();
            persist(Array.AsReadOnly(remaining));
            tickets.Remove(ticket.Serial);
            return true;
        }finally{busy=false;}
    }
    // False means admission must remain closed. Cleanup never weakens identity to
    // an index/classname/owner lookup, including after disconnect and plugin reload.
    public bool Drain(INativeEntityBoundary native)
    {
        if(busy)return false;
        busy=true;
        try{
            foreach(var ticket in tickets.Values.ToArray()){
                try{
                    if(native.Exists(ticket.Serial)){
                        if(!native.Matches(ticket))continue; // Foreign/reused handle: never remove.
                        native.Remove(ticket);
                        if(native.Exists(ticket.Serial))continue; // Native deletion not confirmed.
                    }
                    var remaining=tickets.Values.Where(t=>t!=ticket).ToArray();
                    persist(Array.AsReadOnly(remaining)); // On write failure retain for recovery.
                    tickets.Remove(ticket.Serial);
                }catch{
                    // One failed entity must not starve unrelated rollback.
                }
            }
            return tickets.Count==0;
        }finally{busy=false;}
    }
}
