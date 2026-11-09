namespace DeltaOps;
public enum NativeWorldRecoveryDecision { CreateNew, RecoverExisting, Block }
public static class NativeWorldRecoveryPolicy
{
    public static NativeWorldRecoveryDecision Decide(IReadOnlyList<NativeEntityTicket> saved,
        string processIdentity,bool witnessedNewMap,Func<NativeEntityTicket,bool> matches)
    {
        if(string.IsNullOrWhiteSpace(processIdentity)||!processIdentity.EndsWith(':'))
            return NativeWorldRecoveryDecision.Block;
        if(saved.Count==0)return NativeWorldRecoveryDecision.CreateNew;
        try{foreach(var ticket in saved)NativeEntityEscrow.Validate(ticket);}
        catch(ArgumentException){return NativeWorldRecoveryDecision.Block;}
        if(saved.Select(t=>t.Serial).Distinct().Count()!=saved.Count ||
            saved.Select(t=>t.Marker).Distinct().Count()!=saved.Count)
            return NativeWorldRecoveryDecision.Block;
        var anchors=saved.Where(t=>t.Kind=="info_target").ToArray();
        if(anchors.Length!=1)return NativeWorldRecoveryDecision.Block;
        var anchor=anchors[0];
        if(saved.Any(t=>t.World!=anchor.World))return NativeWorldRecoveryDecision.Block;
        // Validate syntax even for an old world. Corruption is never silently discarded.
        var parts=anchor.World.Split(':');
        var processParts=parts[0].Split('-');
        if(parts.Length!=2||processParts.Length!=2||
            !int.TryParse(processParts[0],out var pid)||pid<=0||
            !long.TryParse(processParts[1],out var started)||started<=0||
            !Guid.TryParseExact(parts[1],"N",out _) ||
            !anchor.Marker.StartsWith("deltaops-owned-",StringComparison.Ordinal) ||
            !Guid.TryParseExact(anchor.Marker["deltaops-owned-".Length..],"N",out _) ||
            anchor.Serial is 0 or uint.MaxValue)return NativeWorldRecoveryDecision.Block;
        if(!anchor.World.StartsWith(processIdentity,StringComparison.Ordinal)||witnessedNewMap)
            return NativeWorldRecoveryDecision.CreateNew;
        return matches(anchor)?NativeWorldRecoveryDecision.RecoverExisting:NativeWorldRecoveryDecision.Block;
    }
}
