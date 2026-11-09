using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
namespace DeltaOps;
public sealed partial class DeltaOpsPlugin
{
    // Direct inert creation, never GiveNamedItem and never an unowned weapon-to-projectile
    // conversion. This adapter is offline implementation, not installed-engine acceptance.
    private sealed class CssProjectileCreation(
        DeltaOpsPlugin plugin,CCSPlayerController player,uint pawnSerial,
        string designer,float lifetime,Action<CBaseGrenade> configure):INativeEntityCreation
    {
        private CBaseGrenade? allocated;
        private uint allocatedSerial;
        private readonly CssEntityBoundary boundary=new();
        public NativeEntityTicket CreateInert(){
            if(!plugin.EntityWorldReady()||player.PlayerPawn.Raw!=pawnSerial)
                throw new InvalidOperationException("Projectile world/pawn changed");
            allocated=Utilities.CreateEntityByName<CBaseGrenade>(designer);
            if(allocated is not {IsValid:true})throw new InvalidOperationException("Projectile allocation failed");
            allocatedSerial=allocated.EntityHandle.Raw; // Retain before marker/schema writes.
            var identity=allocated.Entity??throw new InvalidOperationException("Projectile identity absent");
            var marker="deltaops-owned-"+Guid.NewGuid().ToString("N");
            identity.Name=marker;
            return new(plugin._entityAnchor!.World,allocatedSerial,marker,designer);
        }
        public void CleanupIncompleteCreation(){
            // The direct returned object is authority only for an uncommitted allocation.
            // Never recover it by index/classname, and never touch a reused full handle.
            if(allocated is not {IsValid:true})return;
            if(allocatedSerial==0||allocated.EntityHandle.Raw!=allocatedSerial)
                throw new InvalidOperationException("Incomplete projectile handle changed");
            allocated.Remove();
            if(allocated.IsValid)throw new InvalidOperationException("Incomplete projectile cleanup pending");
        }
        public void Activate(NativeEntityTicket ticket){
            if(!plugin.EntityWorldReady()||!Matches(ticket)||player.PlayerPawn.Raw!=pawnSerial||
                player.PlayerPawn.Value is not {IsValid:true} pawn||!player.PawnIsAlive)
                throw new InvalidOperationException("Projectile admission changed");
            if(!float.IsFinite(lifetime)||lifetime<=0||lifetime>60)
                throw new InvalidOperationException("Invalid projectile lifetime");
            var grenade=allocated!;
            grenade.Thrower.Raw=pawnSerial;
            grenade.OriginalThrower.Raw=pawnSerial;
            grenade.OwnerEntity.Raw=pawnSerial;
            grenade.Damage=0; // A specialized adapter must explicitly configure safe damage.
            grenade.DmgRadius=0;
            configure(grenade);
            // Engine-owned delayed IO does not depend on plugin timers after unload.
            // Delivery and full-handle targeting still require installed-engine verification.
            grenade.DispatchSpawn();
            if(!Matches(ticket))throw new InvalidOperationException("Projectile identity changed during spawn");
            grenade.AddEntityIOEvent("Kill",grenade,grenade,"",lifetime);
            var origin=pawn.AbsOrigin??throw new InvalidOperationException("No throw origin");
            var yaw=pawn.EyeAngles.Y*Math.PI/180;
            grenade.Teleport(new Vector(origin.X,origin.Y,origin.Z+48),null,
                new Vector((float)Math.Cos(yaw)*600,(float)Math.Sin(yaw)*600,140));
        }
        public bool Exists(uint serial)=>boundary.Exists(serial);
        public bool Matches(NativeEntityTicket ticket)=>boundary.Matches(ticket);
        public void Remove(NativeEntityTicket ticket)=>boundary.Remove(ticket);
    }
}
