namespace DeltaOps;

// Capability-specific admission, not a claim of DF equivalence or runtime QA.
// Read-only private coordinate telemetry has verified managed API contracts.
// Mutating effects retain independent blockers documented in the source ledger.
public static class NativeApiCapabilityEvidence
{
    public static bool ReadOnlyRecon(int operatorId,int skillId)=>
        (operatorId,skillId) is (2,2002017) or (4,1002016);
    // Status and activation must use the same blocker, not merely router coverage.
    public static string? Blocker(int operatorId,int skillId)=>
        NativeAbilityRouter.Blocker(operatorId,skillId) ??
        (ReadOnlyRecon(operatorId,skillId)?null:
            "Native capability contract incomplete; effect disabled. See API evidence ledger.");
    public static bool EntityCreation => false; // Engine allocation/spawn/expiry contract pending.
    public static bool ScalarMutation => false; // Competing-owner restore/teardown not proved.
    public static bool MovementMutation => false; // Collision and displacement acceptance pending.
    public static bool DamageMutation => false; // Native classification/driver contracts incomplete.
    public static bool HitMark => false; // DF trigger/scope unresolved; no silent assumption.
}
