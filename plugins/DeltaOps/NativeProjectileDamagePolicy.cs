namespace DeltaOps;
public static class NativeProjectileDamagePolicy
{
    // All arguments are current native observations, never values captured at throw.
    public static bool Allow(bool ownerMatches,bool victimMatches,bool ownerAlive,bool victimAlive,
        bool worldMatches,bool attackerMatches,bool raidMode,bool raidOwnerActive,bool raidCanDamage,
        bool nativeEnemy)=>ownerMatches&&victimMatches&&ownerAlive&&victimAlive&&worldMatches&&attackerMatches&&
        (raidMode?raidOwnerActive&&raidCanDamage:nativeEnemy);
}
