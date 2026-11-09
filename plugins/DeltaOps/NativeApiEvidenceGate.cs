namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
    // Source ledger docs/ABILITY_API_EVIDENCE.md. Signature evidence is NOT a
    // lifecycle contract or installed-version acceptance. Fail closed until each
    // new boundary and its assumptions are audited; no config can bypass this.
    private static readonly bool NativeApiEvidenceComplete = false;
}
