using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
namespace DeltaOps;

public sealed class NativePortTuning
{
    // Explicit CS2 port choices, NONE are decoded DF effect magnitudes.
    public int HealHp {get;set;}=25;
    public float ReconRadiusSourceUnits {get;set;}=1200;
    public float RushSpeedSourceUnits {get;set;}=400;
    public float TrapRadiusSourceUnits {get;set;}=160;
    public float TrapLifetimeSeconds {get;set;}=30;
    public float TrapSlowSeconds {get;set;}=2;
    public float TrapVelocityModifier {get;set;}=0.5f;
}
public sealed partial class DeltaOpsPlugin
{
    private sealed record NativeActor(uint Controller,uint Pawn,InterpretedAbilitySession Session);
    private readonly Dictionary<string,NativeActor> _nativeActors=[];
    private readonly List<Action> _nativeTicks=[];
    private bool NativeAdmission(CCSPlayerController p,uint controller,uint pawn)=>
        !_fixtureMode && IsHumanController(p) && p.EntityHandle.Raw==controller && p.PawnIsAlive &&
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
            }
            old.Session.Resume();return old;
        }
        var actor=new NativeActor(p.EntityHandle.Raw,pawn.EntityHandle.Raw,new(op,Environment.TickCount64));
        _nativeActors[key]=actor;return actor;
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
        var blocker=NativeAbilityRouter.Blocker((int)selected.Value,id);
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
            p.PrintToChat($" [r2] {s.Id} {s.Name}: {actor.Session.Available(s.Id,Environment.TickCount64)}/{s.Capacity}; recharge {s.RechargeSeconds}s; {NativeAbilityRouter.Blocker(op,s.Id)??"CS2 port"}");
    }
    private void RevokeNative(string key){if(_nativeActors.TryGetValue(key,out var a))a.Session.Revoke();}
    private void TickNative()
    {
        foreach(var pair in _nativeActors.ToArray()) {
            var p=FindPlayerByKey(pair.Key);
            if(p==null||!NativeAdmission(p,pair.Value.Controller,pair.Value.Pawn))pair.Value.Session.Revoke();
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
            if(!Valid()||plugin.Config.NativePorts.HealHp is <1 or >200||Pawn.Health>=Pawn.MaxHealth)return null;
            var effect=new OwnedAbilityEffect(()=>{}); // Instant health is committed, not rolled back on death.
            Pawn.Health=(int)Math.Min(Pawn.MaxHealth,(long)Pawn.Health+plugin.Config.NativePorts.HealHp);
            Utilities.SetStateChanged(Pawn,"CBaseEntity","m_iHealth");return effect;
        }
        public IDisposable? Grant(string weapon)
        {
            if(!Valid())return null;
            if(plugin._raidMode && weapon=="weapon_hegrenade") {player.PrintToChat(" [r2 blocked] Native HE custom-team damage policy has not been verified; no grant or token spent.");return null;}
            // One pending grant per native weapon type; inventory-full rejection spends no token.
            if(Pawn.WeaponServices?.MyWeapons.Any(w=>w.Value is {IsValid:true} e&&e.DesignerName==weapon)!=false)return null;
            var item=player.GiveNamedItem<CBasePlayerWeapon>(weapon);
            if(item==null||!item.IsValid)throw new InvalidOperationException("Native grant outcome unknown; quarantine rather than retry minting.");
            var raw=item.EntityHandle.Raw;
            return new OwnedAbilityEffect(()=>{
                var owned=new CHandle<CBasePlayerWeapon>(raw).Value;
                // Full serial handle, never entity index. Remove even dropped grants at lifecycle end.
                if(owned is {IsValid:true}){owned.Remove();if(owned.IsValid)throw new InvalidOperationException("Grant cleanup not confirmed");}
            });
        }
        public IDisposable? Recon()
        {
            var radius=plugin.Config.NativePorts.ReconRadiusSourceUnits;
            if(!Valid()||!float.IsFinite(radius)||radius<=0||radius>10000)return null;
            var targets=plugin.GetEnemies(player).Where(e=>Distance(player,e)<=radius).ToArray();
            // Private owner-only telemetry, no globally replicated wallhack or native-team targeting.
            var text=targets.Length==0?"no contacts":string.Join(", ",targets.Select(t=>$"{t.PlayerName} {Distance(player,t):0}su"));
            player.PrintToChat($" [CS2 recon, no LOS] {text}");return new OwnedAbilityEffect(()=>{});
        }
        public IDisposable? Rush()
        {
            var speed=plugin.Config.NativePorts.RushSpeedSourceUnits;
            if(!Valid()||!float.IsFinite(speed)||speed<=0||speed>1000)return null;
            var yaw=Pawn.EyeAngles.Y*Math.PI/180;
            // Velocity only, never origin teleport or noclip. Collision is entirely engine-owned.
            Pawn.Teleport(null,null,new Vector((float)Math.Cos(yaw)*speed,(float)Math.Sin(yaw)*speed,Pawn.AbsVelocity.Z));
            return new OwnedAbilityEffect(()=>{});
        }
        public IDisposable? Trap()
        {
            var t=plugin.Config.NativePorts;
            if(!Valid()||Pawn.AbsOrigin==null||!float.IsFinite(t.TrapRadiusSourceUnits)||t.TrapRadiusSourceUnits<=0||t.TrapRadiusSourceUnits>1000||
                !float.IsFinite(t.TrapLifetimeSeconds)||t.TrapLifetimeSeconds<=0||t.TrapLifetimeSeconds>300||
                !float.IsFinite(t.TrapSlowSeconds)||t.TrapSlowSeconds<=0||t.TrapSlowSeconds>10||
                !float.IsFinite(t.TrapVelocityModifier)||t.TrapVelocityModifier<=0||t.TrapVelocityModifier>1)return null;
            var origin=Pawn.AbsOrigin;var point=new Vector(origin.X,origin.Y,origin.Z);
            var end=Environment.TickCount64+(long)(t.TrapLifetimeSeconds*1000);bool closed=false;
            CCSPlayerPawn? targetPawn=null;uint targetSerial=0;float previous=1,applied=1;long restore=0;
            Action? tick=null;OwnedAbilityEffect? effect=null;
            void Cleanup(){
                if(closed)return;
                if(targetPawn is {IsValid:true} && targetPawn.EntityHandle.Raw==targetSerial && targetPawn.VelocityModifier==applied){
                    targetPawn.VelocityModifier=previous;Utilities.SetStateChanged(targetPawn,"CCSPlayerPawn","m_flVelocityModifier");
                }
                closed=true;plugin._nativeTicks.Remove(tick!);
            }
            effect=new OwnedAbilityEffect(Cleanup);
            tick=()=>{
                try {
                if(closed)return;
                if(!Valid()||Environment.TickCount64>=end||restore!=0&&Environment.TickCount64>=restore){effect.Dispose();return;}
                if(restore!=0)return;
                var enemy=plugin.GetEnemies(player).FirstOrDefault(e=>DistanceToPoint(e,point)<=t.TrapRadiusSourceUnits);
                var body=enemy?.PlayerPawn.Value;if(body==null||!body.IsValid)return;
                // Do not stack with another movement modifier; avoid restoring over another owner.
                if(body.VelocityModifier!=1f)return;
                targetPawn=body;targetSerial=body.EntityHandle.Raw;previous=body.VelocityModifier;applied=t.TrapVelocityModifier;
                restore=Environment.TickCount64+(long)(t.TrapSlowSeconds*1000);
                body.VelocityModifier=applied;Utilities.SetStateChanged(body,"CCSPlayerPawn","m_flVelocityModifier");
                }catch{actor.Session.Quarantine();actor.Session.Revoke();}
            };
            plugin._nativeTicks.Add(tick);return effect;
        }
    }
}
