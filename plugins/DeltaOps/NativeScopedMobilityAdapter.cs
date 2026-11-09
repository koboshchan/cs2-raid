using CounterStrikeSharp.API;
namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
    private readonly CooperativeStatusOwnership _cooperativeStatus=new();
    private void AttachScopedMobility(CounterStrikeSharp.API.Core.CCSPlayerController player,NativeActor actor)
    {
        if(!NativeNumericEvidence.Allowed(10010301)||!NativeApiCapabilityEvidence.ScopedMobilityActivation||actor.Session.Operator!=3)return;
        var body=player.PlayerPawn.Value;
        if(body==null||!body.IsValid)return;
        // CSS f58e383b generated CCSPlayerPawn.g.cs L265-267: ref bool IsScoped.
        // This is a scoped-state interpretation, not proof of all DF ADS triggers.
        var state=new NativeScopedMobility(
            ()=>NativeAdmission(player,actor.Controller,actor.Pawn)&&actor.Session.Alive&&!actor.Session.Quarantined,
            ()=>body.IsValid&&body.EntityHandle.Raw==actor.Pawn&&body.IsScoped,
            ()=>{
                var modifier=Config.NativePorts.ScopedMobilityModifier;
                if(!float.IsFinite(modifier)||modifier<=0||!float.IsFinite(modifier))return null;
                var lease=new NativeStatusLease(
                    ()=>body.IsValid&&body.EntityHandle.Raw==actor.Pawn,
                    ()=>body.VelocityModifier,v=>body.VelocityModifier=v,
                    ()=>Utilities.SetStateChanged(body,"CCSPlayerPawn","m_flVelocityModifier"),
                    body.VelocityModifier,modifier);
                // This instance arbitrates cooperating DeltaOps writers only.
                return _cooperativeStatus.Enact(actor.Session,NativeProcessIdentity,actor.Pawn,
                    "CCSPlayerPawn.m_flVelocityModifier",lease.Apply,lease.Dispose);
            });
        Action tick=()=>{
            try{state.Tick();}
            catch{actor.Session.Quarantine();actor.Session.Revoke();}
        };
        actor.Session.EnactOwned(()=>_nativeTicks.Add(tick),()=>{
            state.Dispose();_nativeTicks.Remove(tick);
        });
    }
}
