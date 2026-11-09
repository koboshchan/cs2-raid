namespace Cs2Raid;

// Resident boundary. The native driver must be proven for the exact installed ABI;
// absence of a driver is a denial, not permission to write Health.
public readonly record struct AbilityDamageRequest(Guid Event,long Generation,
    uint AttackerController,uint AttackerPawn,uint TargetController,uint TargetPawn,
    uint Inflictor,float Damage);
public enum AbilityDamageOutcome { Denied, Unsupported, Applied, NativeRejected, Indeterminate, Duplicate }
public interface IAttributedDamageDriver
{
    bool Supported { get; }
    // Synchronous, game-thread only. NativeRejected means proved no effect, not a retry permit.
    bool Apply(AbilityDamageRequest request);
}
public sealed class AttributedAbilityDamage(
    Func<bool> gameThread,Func<AbilityDamageRequest,bool> authorize,IAttributedDamageDriver driver)
{
    private readonly HashSet<(long,Guid)> consumed=[];
    private bool applying;
    public AbilityDamageOutcome Apply(AbilityDamageRequest request)
    {
        if(!gameThread()||applying||request.Event==Guid.Empty||request.Generation<=0||
           request.AttackerController==0||request.AttackerPawn==0||request.TargetController==0||
           request.TargetPawn==0||request.Inflictor==0||
           request.AttackerController==request.TargetController||
           !float.IsFinite(request.Damage)||request.Damage<=0)return AbilityDamageOutcome.Denied;
        if(consumed.Contains((request.Generation,request.Event)))return AbilityDamageOutcome.Duplicate;
        applying=true;
        try{
            if(!authorize(request))return AbilityDamageOutcome.Denied;
            if(!driver.Supported)return AbilityDamageOutcome.Unsupported;
            // Consume BEFORE invoking engine. A thrown call has unknown outcome and is never retried.
            consumed.Add((request.Generation,request.Event));
            try{return driver.Apply(request)?AbilityDamageOutcome.Applied:AbilityDamageOutcome.NativeRejected;}
            catch{return AbilityDamageOutcome.Indeterminate;}
        }catch{return AbilityDamageOutcome.Denied;}
        finally{applying=false;}
    }
}
// No zero-allocation assumption. Schema size alone does not construct either native class.
public sealed class UnvalidatedAbilityDamageDriver:IAttributedDamageDriver
{
    public bool Supported=>false;
    public bool Apply(AbilityDamageRequest request)=>false;
}

public readonly record struct AbilityDamageAuthority(bool ResidentLoaded,bool OnGameThread,
    long CurrentGeneration,bool Active,bool AttackerLiving,bool TargetLiving,
    bool FinancialAllowed,bool Hostile,bool AttackerIdentity,bool TargetIdentity,
    bool InflictorOwned,bool OutcomeOpen);
public static class AbilityDamageAuthorization
{
    public static bool Allow(AbilityDamageRequest request,AbilityDamageAuthority a)=>
        a.ResidentLoaded&&a.OnGameThread&&a.CurrentGeneration==request.Generation&&a.Active&&
        a.AttackerLiving&&a.TargetLiving&&a.FinancialAllowed&&a.Hostile&&
        a.AttackerIdentity&&a.TargetIdentity&&a.InflictorOwned&&a.OutcomeOpen;
}
