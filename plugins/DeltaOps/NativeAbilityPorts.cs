using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
namespace DeltaOps;

public sealed class NativePortTuning
{
    // Compatibility property names only. No unsupported behavior defaults.
    // User config values cannot bypass NativeNumericEvidence.
    public float ScopedMobilityModifier {get;set;}=float.NaN;
    public float VyronFallDamageMultiplier {get;set;}=float.NaN;
    public float ShepherdDamageMultiplier {get;set;}=float.NaN;
    public float PulseRadiusSourceUnits {get;set;}=float.NaN;
    public float PulseSeconds {get;set;}=float.NaN;
    public float PulseSlowModifier {get;set;}=float.NaN;
    public int HealHp {get;set;}=0; // unresolved sentinel, never applied
    public float ReconRadiusSourceUnits {get;set;}=float.NaN;
    public float RushSpeedSourceUnits {get;set;}=float.NaN;
    public float TrapRadiusSourceUnits {get;set;}=float.NaN;
    public float TrapLifetimeSeconds {get;set;}=float.NaN;
    public float TrapSlowSeconds {get;set;}=float.NaN;
    public float TrapVelocityModifier {get;set;}=float.NaN;
}
public sealed partial class DeltaOpsPlugin
{
    private HookResult NativePassiveDamage(CBaseEntity entity,CTakeDamageInfo info)
    {
        if(!NativeApiCapabilityEvidence.DamageMutation||!Config.UseInterpretedAbilities||_fixtureMode)return HookResult.Continue;
        var victim=Utilities.GetPlayers().FirstOrDefault(p=>IsHumanController(p)&&p.PlayerPawn.Value is {IsValid:true} pawn&&pawn.EntityHandle.Raw==entity.EntityHandle.Raw);
        if(victim==null||GetExistingState(victim)?.Operator!=OperatorId.Shepherd)return HookResult.Continue;
        if(!_nativeActors.TryGetValue(GetPlayerKey(victim),out var actor)||!actor.Session.Alive||actor.Session.Quarantined)return HookResult.Continue;
        var pawn=victim.PlayerPawn.Value!;
        if(!NativeAdmission(victim,victim.EntityHandle.Raw,pawn.EntityHandle.Raw))return HookResult.Continue;
        // Reduction only. Never create damage, revive an attack or overrule Raid's blocking hook.
        info.Damage=NativePassiveRules.ReduceBlast(info.Damage,ExtractedPassiveValues.ShepherdExplosionMultiplier,(uint)info.BitsDamageType);
        return HookResult.Continue;
    }
    private sealed record NativeActor(uint Controller,uint Pawn,InterpretedAbilitySession Session);
    private readonly Dictionary<string,NativeActor> _nativeActors=[];
    private readonly List<Action> _nativeTicks=[];
    private readonly Dictionary<uint,int> _nativeSuppressed=[];
    private bool NativeAdmission(CCSPlayerController p,uint controller,uint pawn)=>
        Config.UseInterpretedAbilities && !_fixtureMode && !_nativeSuppressed.ContainsKey(pawn) && IsHumanController(p) && p.EntityHandle.Raw==controller && p.PawnIsAlive &&
        p.PlayerPawn.Value is {IsValid:true} body && body.EntityHandle.Raw==pawn && (!_raidMode||RaidActive(p.SteamID));

    private NativeActor? NativeActorFor(CCSPlayerController p,int op)
    {
        var key=GetPlayerKey(p);var pawn=p.PlayerPawn.Value;
        if(pawn==null||!pawn.IsValid)return null;
        if(_nativeActors.TryGetValue(key,out var old)) {
            if(old.Session.Operator!=op)return null; // Cannot reselect to mint charges.
            if(old.Pawn!=pawn.EntityHandle.Raw||old.Controller!=p.EntityHandle.Raw) {
                if(!old.Session.Revoke())return null;
                old=old with {Controller=p.EntityHandle.Raw,Pawn=pawn.EntityHandle.Raw};_nativeActors[key]=old;
                old.Session.Resume();AttachScopedMobility(p,old);
            }
            old.Session.Resume();return old;
        }
        var actor=new NativeActor(p.EntityHandle.Raw,pawn.EntityHandle.Raw,new(op,Environment.TickCount64));
        _nativeActors[key]=actor;AttachScopedMobility(p,actor);return actor;
    }
    [ConsoleCommand("css_ability","Use imported operator skill by its source ID; !ability <id>")]
    public void NativeAbilityCommand(CCSPlayerController? p,CommandInfo cmd)
    {
        if(p==null||!Config.UseInterpretedAbilities||cmd.ArgCount!=2||!int.TryParse(cmd.GetArg(1),out var id)) {
            cmd.ReplyToCommand("Usage: !ability <source skill ID>; interpreted CS2 ports only.");return;
        }
        UseNativeAbility(p,id);
    }
    private bool UseNativeAbility(CCSPlayerController p,int id)
    {
        var selected=GetExistingState(p)?.Operator;
        if(selected==null||_fixtureMode||!p.PawnIsAlive)return false;
        var actor=NativeActorFor(p,(int)selected.Value);
        if(actor==null||!NativeAdmission(p,actor.Controller,actor.Pawn))return false;
        var blocker=NativeApiCapabilityEvidence.Blocker((int)selected.Value,id);
        if(blocker!=null){p.PrintToChat($" [r2 blocked] {id}: {blocker}");return false;}
        var ok=actor.Session.Activate(id,Environment.TickCount64,
            ()=>NativeAdmission(p,actor.Controller,actor.Pawn),
            ()=>NativeAbilityRouter.Enact(actor.Session.Operator,id,new CssAbilityPort(this,p,actor)));
        p.PrintToChat(ok?$" [CS2 port] {id} activated; interpreted r2 charge spent.":$" [r2] {id} unavailable, charging, or native delivery rejected.");
        return ok;
    }
    private void PrintNativeStatus(CCSPlayerController p,int op)
    {
        var actor=NativeActorFor(p,op);if(actor==null)return;
        foreach(var s in InterpretedR2.Skills.Values.Where(s=>s.Operator==op&&!s.Passive))
            p.PrintToChat($" [r2] {s.Id} {s.Name}: {actor.Session.Available(s.Id,Environment.TickCount64)}/{s.Capacity}; recharge {s.RechargeSeconds}s; {NativeApiCapabilityEvidence.Blocker(op,s.Id)??"partial CS2 recon telemetry"}");
    }
    private void RevokeNative(string key){if(_nativeActors.TryGetValue(key,out var a))a.Session.Revoke();}
    private void TickNative()
    {
        foreach(var pair in _nativeActors.ToArray()) {
            var p=FindPlayerByKey(pair.Key);
            if(!pair.Value.Session.Alive||pair.Value.Session.Quarantined||p==null||!NativeAdmission(p,pair.Value.Controller,pair.Value.Pawn))pair.Value.Session.Revoke();
        }
        foreach(var action in _nativeTicks.ToArray())action();
    }
    private void ResetNative()
    {
        foreach(var pair in _nativeActors.ToArray())if(pair.Value.Session.Revoke())_nativeActors.Remove(pair.Key);
        // Failed cleanup actors remain quarantined and retain their retryable ownership.
    }
    private sealed class CssAbilityPort(DeltaOpsPlugin plugin,CCSPlayerController player,NativeActor actor):INativeAbilityPort
    {
        private CCSPlayerPawn Pawn=>player.PlayerPawn.Value!;
        private bool Valid()=>plugin.NativeAdmission(player,actor.Controller,actor.Pawn);
        public IDisposable? Heal()
        {
            if(!NativeNumericEvidence.Allowed(20010101)||!NativeApiCapabilityEvidence.HealingActivation||!Valid()||plugin.Config.NativePorts.HealHp is <1 or >200||Pawn.Health>=Pawn.MaxHealth)return null;
            // Register the committed instant-effect boundary before a native setter can throw.
            // Health is never rolled back across a competing damage/heal event.
            var body=Pawn;
            var serial=actor.Pawn;
            // Pinned source contracts and unresolved engine semantics:
            // docs/ABILITY_EFFECT_BOUNDARIES.md (committed healing).
            var heal=new NativeCommittedHeal(
                ()=>body.IsValid&&body.EntityHandle.Raw==serial,
                ()=>body.Health,()=>body.MaxHealth,v=>body.Health=v,
                ()=>Utilities.SetStateChanged(body,"CBaseEntity","m_iHealth"),
                plugin.Config.NativePorts.HealHp);
            return actor.Session.EnactOwned(heal.Apply,heal.Dispose);
        }
        public IDisposable? Grant(string weapon)
        {
            if(!NativeNumericEvidence.Allowed(2008002)||!NativeApiCapabilityEvidence.EntityCreation||!Valid()||plugin._entityEscrow==null)return null;
            var designer=weapon switch{
                "weapon_smokegrenade"=>"smokegrenade_projectile",
                "weapon_hegrenade"=>"hegrenade_projectile",
                "weapon_flashbang"=>"flashbang_projectile",_=>null};
            if(designer==null)return null;
            var creation=new CssProjectileCreation(plugin,player,actor.Pawn,designer,float.NaN,grenade=>{
                // DF detonation delay UNRESOLVED; no authored delay applied.
                // Native blast damage cannot be enabled merely by having a loaded hook.
                // If unload removal fails, the hook disappears while the entity remains.
                // Keep inert damage until an independent resident authority can enforce
                // custom-team policy through failed unload. Router retains the QA gate.
                grenade.Damage=0;grenade.DmgRadius=0;
                if(grenade is CSmokeGrenadeProjectile smoke){
                    // Color UNRESOLVED; no invented color applied.
                }
            });
            var transaction=new NativeEntityTransaction(plugin._entityEscrow,creation);
            Action? tick=null;bool complete=false;
            void Cleanup(){
                if(complete)return;
                transaction.Dispose();
                plugin._nativeTicks.Remove(tick!);
                complete=true;
            }
            var end=long.MaxValue; // No invented 25s expiry; creation stays strictly blocked.
            tick=()=>{
                try{if(!Valid()||Environment.TickCount64>=end)Cleanup();}
                catch{actor.Session.Quarantine();actor.Session.Revoke();}
            };
            return actor.Session.EnactOwned(()=>{
                plugin._nativeTicks.Add(tick);
                transaction.Activate();
            },Cleanup);
        }

        public IDisposable? Recon()
        {
            var radius=plugin.Config.NativePorts.ReconRadiusSourceUnits;
            if(!NativeNumericEvidence.Allowed(2002017)||!Valid()||!float.IsFinite(radius)||radius<=0||radius>10000)return null;
            var targets=plugin.GetEnemies(player).Where(e=>Distance(player,e)<=radius).ToArray();
            // Private owner-only telemetry, no globally replicated wallhack or native-team targeting.
            var text=targets.Length==0?"no contacts":string.Join(", ",targets.Select(t=>$"{t.PlayerName} {Distance(player,t):0}su"));
            player.PrintToChat($" [CS2 recon, no LOS] {text}");return new OwnedAbilityEffect(()=>{});
        }
        public IDisposable? Rush()
        {
            var speed=plugin.Config.NativePorts.RushSpeedSourceUnits;
            if(!NativeNumericEvidence.Allowed(1009001)||!NativeApiCapabilityEvidence.MovementMutation||!Valid()||!float.IsFinite(speed)||speed<=0||speed>1000)return null;
            var body=Pawn;
            if(!NativeRushImpulse.TryCreate(body.EyeAngles.Y,speed,body.AbsVelocity.Z,out var impulse))return null;
            // Official source: CBaseEntity.cs L15-L59 at CSS f58e383b.
            // docs/ABILITY_EFFECT_BOUNDARIES.json contains exact URL/contract.
            // Committed impulse, not a status lease: no later velocity rollback.
            return actor.Session.EnactOwned(()=>
                body.Teleport(null,null,new Vector(impulse.X,impulse.Y,impulse.Z)),
                ()=>{});
        }
        public IDisposable? Pulse(bool suppress)
        {
            if(!NativeNumericEvidence.Allowed(3008001)||!NativeApiCapabilityEvidence.ScalarMutation)return null;
            var t=plugin.Config.NativePorts;
            if(!Valid()||!float.IsFinite(t.PulseRadiusSourceUnits)||t.PulseRadiusSourceUnits<=0||t.PulseRadiusSourceUnits>1000||
                !float.IsFinite(t.PulseSeconds)||t.PulseSeconds<=0||t.PulseSeconds>10||
                !float.IsFinite(t.PulseSlowModifier)||t.PulseSlowModifier<=0||t.PulseSlowModifier>1)return null;
            var targets=plugin.GetEnemies(player).Where(e=>Distance(player,e)<=t.PulseRadiusSourceUnits)
                .Select(e=>e.PlayerPawn.Value).Where(p=>p is {IsValid:true} && p.VelocityModifier==1f).ToArray();
            if(targets.Length==0)return null;
            var owned=new List<(NativeStatusLease Lease,NativeSuppressionLease? Suppression)>();
            var until=Environment.TickCount64+(long)(t.PulseSeconds*1000);Action? tick=null;bool disposed=false;
            void Cleanup(){
                if(disposed)return;
                for(var i=owned.Count-1;i>=0;i--){
                    var x=owned[i];
                    x.Lease.Dispose();
                    x.Suppression?.Dispose();
                    owned.RemoveAt(i);
                }
                disposed=true;plugin._nativeTicks.Remove(tick!);
            }
            // Register ownership before the first mutation so exceptions retain cleanup.
            tick=()=>{
                try{if(!Valid()||Environment.TickCount64>=until)Cleanup();}
                catch{actor.Session.Quarantine();actor.Session.Revoke();}
            };
            return actor.Session.EnactOwned(()=>{
                plugin._nativeTicks.Add(tick);
                foreach(var body in targets){
                    var serial=body!.EntityHandle.Raw;
                    var lease=new NativeStatusLease(
                        ()=>body.IsValid&&body.EntityHandle.Raw==serial,
                        ()=>body.VelocityModifier,v=>body.VelocityModifier=v,
                        ()=>Utilities.SetStateChanged(body,"CCSPlayerPawn","m_flVelocityModifier"),
                        body.VelocityModifier,t.PulseSlowModifier);
                    var suppression=suppress?new NativeSuppressionLease(plugin._nativeSuppressed,serial):null;
                    owned.Add((lease,suppression));
                    suppression?.Acquire();
                    lease.Apply();
                }
            },Cleanup);
        }
        public IDisposable? Trap()
        {
            if(!NativeNumericEvidence.Allowed(3008001)||!NativeApiCapabilityEvidence.ScalarMutation)return null;
            var t=plugin.Config.NativePorts;
            if(!Valid()||Pawn.AbsOrigin==null||!float.IsFinite(t.TrapRadiusSourceUnits)||t.TrapRadiusSourceUnits<=0||t.TrapRadiusSourceUnits>1000||
                !float.IsFinite(t.TrapLifetimeSeconds)||t.TrapLifetimeSeconds<=0||t.TrapLifetimeSeconds>300||
                !float.IsFinite(t.TrapSlowSeconds)||t.TrapSlowSeconds<=0||t.TrapSlowSeconds>10||
                !float.IsFinite(t.TrapVelocityModifier)||t.TrapVelocityModifier<=0||t.TrapVelocityModifier>1)return null;
            var origin=Pawn.AbsOrigin;var point=new Vector(origin.X,origin.Y,origin.Z);
            var end=Environment.TickCount64+(long)(t.TrapLifetimeSeconds*1000);bool closed=false;
            NativeStatusLease? slow=null;long restore=0;
            Action? tick=null;
            void Cleanup(){
                if(closed)return;
                slow?.Dispose();
                closed=true;plugin._nativeTicks.Remove(tick!);
            }
            tick=()=>{
                try {
                if(closed)return;
                if(!Valid()||Environment.TickCount64>=end||restore!=0&&Environment.TickCount64>=restore){Cleanup();return;}
                if(restore!=0)return;
                var enemy=plugin.GetEnemies(player).FirstOrDefault(e=>DistanceToPoint(e,point)<=t.TrapRadiusSourceUnits);
                var body=enemy?.PlayerPawn.Value;if(body==null||!body.IsValid)return;
                // Do not stack with another movement modifier; avoid restoring over another owner.
                if(body.VelocityModifier!=1f)return;
                var serial=body.EntityHandle.Raw;
                slow=new NativeStatusLease(()=>body.IsValid&&body.EntityHandle.Raw==serial,
                    ()=>body.VelocityModifier,v=>body.VelocityModifier=v,
                    ()=>Utilities.SetStateChanged(body,"CCSPlayerPawn","m_flVelocityModifier"),
                    body.VelocityModifier,t.TrapVelocityModifier);
                restore=Environment.TickCount64+(long)(t.TrapSlowSeconds*1000);
                slow.Apply();
                }catch{actor.Session.Quarantine();actor.Session.Revoke();}
            };
            return actor.Session.EnactOwned(()=>plugin._nativeTicks.Add(tick),Cleanup);
        }
    }
}
