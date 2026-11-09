using CounterStrikeSharp.API.Core;
namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
    private HookResult NativeTrackedHit(EventPlayerHurt evt,GameEventInfo _)
    {
        if(!Config.UseInterpretedAbilities||_fixtureMode)return HookResult.Continue;
        var source=evt.Attacker;var target=evt.Userid;
        if(source==null||target==null||!IsHumanController(source)||!IsHumanController(target))
            return HookResult.Continue;
        if(!_nativeActors.TryGetValue(GetPlayerKey(source),out var actor))return HookResult.Continue;
        if(!NativeHitTrackingPolicy.Allow((int?)GetExistingState(source)?.Operator??0,
            NativeAdmission(source,actor.Controller,actor.Pawn),actor.Session.Alive,
            actor.Session.Quarantined,IsEnemy(source,target),target.PawnIsAlive,evt.DmgHealth))
            return HookResult.Continue;
        if(target.PlayerPawn.Value?.AbsOrigin is not {} origin)return HookResult.Continue;
        // Private text telemetry is the documented CS2 interpretation. No globally
        // networked Glow, no shared scalar and no lingering reveal to clean up.
        source.PrintToChat(FormattableString.Invariant(
            $" [Luna hit tracking] target position ({origin.X:0}, {origin.Y:0}, {origin.Z:0})"));
        return HookResult.Continue;
    }
}
