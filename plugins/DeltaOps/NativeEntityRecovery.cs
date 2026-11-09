using System.Diagnostics;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
namespace DeltaOps;

public sealed partial class DeltaOpsPlugin
{
    private NativeEntityEscrow? _entityEscrow;
    private NativeEntityTicket? _entityAnchor;
    private bool _entityRecoveryReady;
    private NativeEntityFileStore EntityStore=>new(Path.Combine(ModuleDirectory,"native-entity-ownership.json"));
    // Process start distinguishes PID reuse. Random in-engine anchor distinguishes
    // repeated maps, round resets, sessions and plugin load contexts in one process.
    private static readonly string NativeProcessIdentity=ReadEntityProcessIdentity();
    private static string EntityProcessIdentity()=>NativeProcessIdentity;
    private static string ReadEntityProcessIdentity(){
        using var process=Process.GetCurrentProcess();
        return $"{Environment.ProcessId}-{process.StartTime.ToUniversalTime().Ticks}:";
    }
    private sealed class CssEntityBoundary:INativeEntityBoundary{
        private static CBaseEntity? Entity(uint serial)=>new CHandle<CBaseEntity>(serial).Value;
        public bool Exists(uint serial)=>Entity(serial) is {IsValid:true};
        public bool Matches(NativeEntityTicket t)=>Entity(t.Serial) is {IsValid:true} e &&
            e.EntityHandle.Raw==t.Serial && e.Entity?.Name==t.Marker && e.DesignerName==t.Kind;
        public void Remove(NativeEntityTicket t){
            if(!Matches(t))throw new InvalidOperationException("Native cleanup identity changed");
            Entity(t.Serial)!.Remove();
        }
    }
    private void InitializeEntityRecovery(bool witnessedNewMap=false)
    {
        _entityRecoveryReady=false;
        // Audit gate also covers anchor allocation before ability admission.
        if(!NativeApiEvidenceComplete)return;
        try{
            var saved=EntityStore.Read(); // Corruption/IO failure never enables grants.
            var process=EntityProcessIdentity();var boundary=new CssEntityBoundary();
            var decision=NativeWorldRecoveryPolicy.Decide(saved,process,witnessedNewMap,boundary.Matches);
            if(decision==NativeWorldRecoveryDecision.Block)
                throw new InvalidDataException("Native world identity cannot be proved");
            if(decision==NativeWorldRecoveryDecision.RecoverExisting){
                var anchor=saved.Single(t=>t.Kind=="info_target");
                _entityAnchor=anchor;
                _entityEscrow=new(anchor.World,saved.Where(t=>t!=anchor),PersistEntityTickets);
                _entityRecoveryReady=_entityEscrow.Drain(boundary);
                return;
            }
            // New process or witnessed map start discards authority, never removes old serials.
            var marker="deltaops-owned-"+Guid.NewGuid().ToString("N");
            var world=process+Guid.NewGuid().ToString("N");
            var entity=Utilities.CreateEntityByName<CBaseEntity>("info_target");
            if(entity==null)throw new InvalidOperationException("World anchor allocation failed");
            try{
                if(entity.Entity==null)throw new InvalidOperationException("World anchor identity absent");
                entity.Entity.Name=marker;
                var ticket=new NativeEntityTicket(world,entity.EntityHandle.Raw,marker,"info_target");
                EntityStore.Write(new[]{ticket}); // Persist before spawn/publication.
                _entityAnchor=ticket;
                entity.DispatchSpawn();
                if(!boundary.Matches(ticket))throw new InvalidOperationException("Spawn changed world anchor");
                _entityEscrow=new(world,[],PersistEntityTickets);
                _entityRecoveryReady=true;
            }catch{
                // Anchor cannot create damage or utility; retain record if removal fails.
                try{entity.Remove();}catch{}
                throw;
            }
        }catch(Exception error){
            _entityRecoveryReady=false;
            Logger.LogError(error,"Native entity recovery blocked. Ledger retained; no grant admission. No retry is promised while unloaded.");
        }
    }
    private void PersistEntityTickets(IReadOnlyList<NativeEntityTicket> tickets)
    {
        var anchor=_entityAnchor??throw new InvalidOperationException("No native world anchor");
        EntityStore.Write(new[]{anchor}.Concat(tickets).ToArray());
    }
    private bool EntityWorldReady()=>_entityRecoveryReady && _entityAnchor is {} anchor &&
        anchor.World.StartsWith(EntityProcessIdentity(),StringComparison.Ordinal) &&
        new CssEntityBoundary().Matches(anchor);
    private bool DrainEntityRecovery()
    {
        if(_entityEscrow==null)return false;
        if(!EntityWorldReady()){_entityRecoveryReady=false;return false;}
        var complete=_entityEscrow.Drain(new CssEntityBoundary());
        if(!complete)_entityRecoveryReady=false;
        return complete;
    }
}
