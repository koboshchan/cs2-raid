namespace DeltaOps;
// Strict DF-only numerical behavior admission. Cannot be overridden by config.
// 30010201 value is sourced, but native activation/mapping stays separately held.
public static class NativeNumericEvidence
{
    public const string Unresolved="UNRESOLVED: exact extracted DF effect magnitudes and/or engine unit mapping not proved; activation blocked.";
    public static bool Allowed(int skillId)=>skillId==30010201;
}
