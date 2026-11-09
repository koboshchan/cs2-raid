namespace DeltaOps;

// Implementation, activation approval, native contract safety and DF fidelity
// are independent axes. Unknown DF mechanics do not prohibit an explicitly
// documented offline CS2 interpretation.
public static class NativeImplementationEvidence
{
    public static string Implementation(int id)=>id switch {
        10010301=>"implemented partial cooperative scoped-mobility interpretation",
        20010101=>"implemented partial committed heal interpretation",
        2002017 or 1002016=>"implemented partial private names-and-distances interpretation",
        1009001=>"implemented partial velocity-only impulse interpretation",
        4008002 or 30010101 or 4009002 or 3008001=>"implemented partial owned status interpretation",
        30010201 or 1009004=>"implemented partial borrowed damage-info modifier",
        40010201=>"implemented partial post-hit telemetry interpretation",
        2008002 or 2008005 or 3008002 or 3008008 or 4009003=>
            "shared owned-projectile foundation implemented; skill-specific behavior unimplemented",
        _=>"skill-specific effect unimplemented"
    };
    public static string? SafetyBlocker(int id)=>!NativeNumericEvidence.Allowed(id)?NativeNumericEvidence.Unresolved:id switch {
        10010301=>"Cooperative-only scalar lease; nonparticipant writers and unload restoration remain outside guarantee.",
        20010101=>null, // Committed write semantics reviewed; NOT a DF healing equivalence claim.
        2002017 or 1002016=>null,
        1009001=>null, // Verified velocity-only API; deployment/gamedata and native QA still pending.
        4008002 or 30010101 or 4009002 or 3008001=>
            "Reversible shared scalar cannot distinguish identical-value competing writers; unload failure cannot guarantee restoration.",
        2008002 or 2008005 or 3008002 or 3008008 or 4009003=>
            "Native inert creation/type/spawn preservation and unload-independent projectile expiry not established.",
        30010201 or 1009004=>null, // Borrowed pre-hook refs and enum constants source-reviewed; activation/native QA held.
        40010201=>null,
        _=>"No skill-specific native effect boundary implemented."
    };
    public static string Approval(int op,int id)=>
        NativeApiCapabilityEvidence.ReadOnlyRecon(op,id)?
        "source-reviewed private telemetry path; normal configuration and Raid admission required":
        "activation held pending reviewed deployment approval; offline implementation permitted";
}
