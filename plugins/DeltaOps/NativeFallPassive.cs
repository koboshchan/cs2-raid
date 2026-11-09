using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
 private HookResult NativeFallPassive(CEntityInstance entity,CTakeDamageInfo info){
  if(!NativeNumericEvidence.Allowed(1009004)||!NativeApiCapabilityEvidence.DamageMutation||!Config.UseInterpretedAbilities||_fixtureMode||entity.DesignerName!="player")return HookResult.Continue;
  // As<T> is only a pointer wrapper, not runtime type validation. Resolve through
  // current connected controllers and their typed pawn handles instead.
  var player=Utilities.GetPlayers().FirstOrDefault(p=>IsHumanController(p)&&
      p.PlayerPawn.Value is {IsValid:true} pawn&&pawn.EntityHandle.Raw==entity.EntityHandle.Raw);
  if(player==null||!_nativeActors.TryGetValue(GetPlayerKey(player),out var actor))return HookResult.Continue;
  info.Damage=NativeFallPassivePolicy.Apply(info.Damage,(uint)info.BitsDamageType,
      (int?)GetExistingState(player)?.Operator??0,NativeAdmission(player,actor.Controller,actor.Pawn),
      actor.Session.Alive,actor.Session.Quarantined,Config.NativePorts.VyronFallDamageMultiplier);
  return HookResult.Continue;
 }
}
