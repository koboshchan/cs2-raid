namespace DeltaOps;
public static class NativeHitTrackingPolicy
{
    public static bool Allow(int selectedOperator,bool admitted,bool actorAlive,bool quarantined,
        bool enemy,bool targetAlive,int healthDamage)=>selectedOperator==2&&admitted&&actorAlive&&
        !quarantined&&enemy&&targetAlive&&healthDamage>0;
}
