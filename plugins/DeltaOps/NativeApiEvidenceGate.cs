namespace DeltaOps;

// Capability-specific admission, not a claim of DF equivalence or runtime QA.
// Read-only private names-and-distances telemetry has verified managed API contracts.
// Mutating effects retain independent blockers documented in the source ledger.
public static class NativeApiCapabilityEvidence
{
    public static bool ReadOnlyRecon(int operatorId,int skillId)=>
        (operatorId,skillId) is (2,2002017) or (4,1002016);
    // Status and activation must use the same blocker, not merely router coverage.
    public static string? Blocker(int operatorId,int skillId)=>
        NativeAbilityRouter.Blocker(operatorId,skillId) ??
        (ReadOnlyRecon(operatorId,skillId)?null:
            (NativeImplementationEvidence.SafetyBlocker(skillId) ??
             "Offline interpretation implemented; native activation held pending reviewed deployment approval."));
    public static bool EntityCreation => false; // Engine allocation/spawn/expiry contract pending.
    public static bool HealingActivation => false; // Reviewed committed setter; activation approval pending.
    public static bool ScalarMutation => false; // Competing-owner restore/teardown not proved.
    public static bool MovementMutation => false; // Velocity-only API reviewed; activation approval/native QA pending.
    public static bool DamageMutation => false; // Borrowed pre-hook reductions reviewed; activation approval held. No damage constructor.
    public static bool HitMark => false; // Private telemetry implementation reviewed; activation approval pending.
}
