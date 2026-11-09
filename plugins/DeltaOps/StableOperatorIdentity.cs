namespace DeltaOps;

// Legacy enum 0 / wire 1 belonged to retired D-Wolf. Never compact these IDs.
public static class StableOperatorIdentity
{
    public const int Stinger = 1, Luna = 2, Vyron = 3, Hackclaw = 4, Shepherd = 5;
    public static bool SelectableEnum(int value) => value is >= Stinger and <= Shepherd;
    public static bool SelectableWire(int value) => value is >= Stinger+1 and <= Shepherd+1;
    public static int Wire(int value) => SelectableEnum(value) ? value+1 :
        throw new ArgumentOutOfRangeException(nameof(value));
    public static int EnumFromWire(int value) => SelectableWire(value) ? value-1 :
        throw new ArgumentOutOfRangeException(nameof(value));
}

// Invalid legacy selections require an explicit session reset, not another assignment.
public sealed class StableOperatorSelection
{
    private int? selected;
    public bool Retired { get; private set; }
    public int? Value {
        get => Retired ? null : selected;
        set {
            if(value is {} id && !StableOperatorIdentity.SelectableEnum(id)) {Retired=true;selected=null;}
            else if(!Retired)selected=value;
        }
    }
}
