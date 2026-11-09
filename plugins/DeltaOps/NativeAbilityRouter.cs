namespace DeltaOps;

// Adapter boundary is testable without loading CSS. These are CS2 ports, not DF equivalence.
public interface INativeAbilityPort
{
    IDisposable? Heal();
    IDisposable? Grant(string weapon);
    IDisposable? Recon();
    IDisposable? Rush();
    IDisposable? Trap();
    IDisposable? Pulse(bool suppress);
}
public static class NativeAbilityRouter
{
    public static int Slot(int op,bool v)=> (op,v) switch {
        (1,true)=>20010101,(1,false)=>2008005,
        (2,true)=>2002017,(2,false)=>4008002,
        (3,true)=>1009001,(3,false)=>1009005,
        (4,true)=>1002016,(4,false)=>4009003,
        (5,true)=>3008001,(5,false)=>30010101,_=>0};
    public static string? Blocker(int op,int id) {
        if(!InterpretedR2.Skills.TryGetValue((op,id),out var s))return "Skill not in this operator's source rows (4008010 is missing).";
        if(s.Passive)return id switch {
            20010201 or 20010215=>"Revive passive requires a downed/revive state; Raid death currently creates death outcomes, not a revivable pawn.",
            40010201=>"Tracking passive needs per-owner hit reveal ownership; global Glow leaks to every squad and is not safe.",
            1009004 or 10010301=>"Kinetic fall/recovery and ADS movement triggers need fall-damage classification and weapon-state semantics; no movement baseline ownership contract.",
            40010202=>"Footstep attenuation is client/audio-path behavior; player speed alone is not silent footsteps.",
            30010201=>null,
            _=>"Passive buff has no mapped CS2 trigger."};
        return id switch {
            2008002 or 2008005 or 20010101 or 2002017 or 3008002 or 1009001 or 1002016 or 4009003 or 3008001 or 3008008 or 4008002 or 4009002 or 30010101=>null,

            1009002 or 1009005=>"Magnetic attachment, explosion and custom-team damage hooks unresolved; HE is not an equivalent substitute.",
            1009003=>"Tiger cannon projectile/damage rows unresolved; no native cannon entity.",


            4008007=>"No owned DF arrow projectile exists to remove.",
            30010202 or 30010203=>"Repair/tool interaction target, rate and resource semantics unresolved.",
            _=>"No supported native adapter."};
    }
    public static IDisposable? Enact(int op,int id,INativeAbilityPort port) {
        if(Blocker(op,id)!=null || InterpretedR2.Skills[(op,id)].Passive)return null;
        return id switch {
            2008002 or 2008005=>port.Grant("weapon_smokegrenade"),
            20010101=>port.Heal(),
            2002017 or 1002016=>port.Recon(),
            3008002 or 3008008=>port.Grant("weapon_hegrenade"),
            4009003=>port.Grant("weapon_flashbang"),
            4008002 or 30010101=>port.Pulse(false),4009002=>port.Pulse(true),
            1009001=>port.Rush(),3008001=>port.Trap(),_=>null};
    }
}
public sealed class OwnedAbilityEffect(Action cleanup):IDisposable
{
    private bool disposed;
    public void Dispose(){if(disposed)return;cleanup();disposed=true;}
}

public static class NativePassiveRules
{
    public static float Reduce(float damage,float multiplier)=>float.IsFinite(damage)&&damage>0&&float.IsFinite(multiplier)&&multiplier is >0 and <=1 ?damage*multiplier:damage;
}
