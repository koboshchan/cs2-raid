using CounterStrikeSharp.API.Core;
namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
 private HookResult NativeFallPassive(CEntityInstance entity,CTakeDamageInfo info){
  if(!Config.UseInterpretedAbilities||_fixtureMode||entity.DesignerName!="player")return HookResult.Continue;
  var pawn=entity.As<CCSPlayerPawn>();var player=pawn.Controller.Value?.As<CCSPlayerController>();
  if(player==null||!IsHumanController(player)||
      !_nativeActors.TryGetValue(GetPlayerKey(player),out var actor))return HookResult.Continue;
  info.Damage=NativeFallPassivePolicy.Apply(info.Damage,(uint)info.BitsDamageType,
      (int?)GetExistingState(player)?.Operator??0,NativeAdmission(player,actor.Controller,actor.Pawn),
      actor.Session.Alive,actor.Session.Quarantined,Config.NativePorts.VyronFallDamageMultiplier);
  return HookResult.Continue;
 }
}
