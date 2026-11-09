namespace DeltaOps;

// Adapter boundary is testable without loading CSS. These are CS2 ports, not DF equivalence.
public interface INativeAbilityPort
{
    IDisposable? Heal();
    IDisposable? Grant(string weapon);
    IDisposable? Recon();
    IDisposable? Rush();
    IDisposable? Trap();
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
        if(s.Passive)return "Buff rows and passive triggers unresolved; no legacy passive fallback.";
        return id switch {
            2008002 or 2008005 or 20010101 or 2002017 or 3008002 or 1009001 or 1002016 or 4009003 or 3008001 or 3008008=>null,
            4008002=>"Volt projectile, damage/radius and status rows unresolved; native damage ABI not established.",
            1009002 or 1009005=>"Magnetic attachment, explosion and custom-team damage hooks unresolved; HE is not an equivalent substitute.",
            1009003=>"Tiger cannon projectile/damage rows unresolved; no native cannon entity.",
            4009002=>"Knife projectile, electronic suppression and status parameters unresolved.",
            30010101=>"Sonic projectile/damage and destruction parameters unresolved.",
            4008007=>"No owned DF arrow projectile exists to remove.",
            30010202 or 30010203=>"Repair/tool interaction target, rate and resource semantics unresolved.",
            _=>"No supported native adapter."};
    }
    public static IDisposable? Enact(int op,int id,INativeAbilityPort port) {
        if(Blocker(op,id)!=null)return null;
        return id switch {
            2008002 or 2008005=>port.Grant("weapon_smokegrenade"),
            20010101=>port.Heal(),
            2002017 or 1002016=>port.Recon(),
            3008002 or 3008008=>port.Grant("weapon_hegrenade"),
            4009003=>port.Grant("weapon_flashbang"),
            1009001=>port.Rush(),3008001=>port.Trap(),_=>null};
    }
}
public sealed class OwnedAbilityEffect(Action cleanup):IDisposable
{
    private bool disposed;
    public void Dispose(){if(disposed)return;cleanup();disposed=true;}
}
