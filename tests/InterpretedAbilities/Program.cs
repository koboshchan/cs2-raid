using DeltaOps;
int count=0;
void Check(bool x){count++;if(!x)throw new Exception($"check {count}");}
Check(InterpretedR2.Skills.Count==33);
foreach(var s in InterpretedR2.Skills.Values){
 var c=new AbilityCharge(s,1000);Check(c.Available==Math.Clamp(s.Initial,0,s.Capacity));
 c.Advance(1000+c.Period*100);Check(c.Available==c.Capacity);
 for(int i=0;i<c.Capacity;i++)Check(c.Spend(1000+c.Period*100));
 Check(!c.Spend(1000+c.Period*100));
}
var session=new InterpretedAbilitySession(1,1000);
Check(!session.Activate(3008002,1000,()=>true,()=>new Effect()));
Check(!session.Activate(2008002,1000,()=>false,()=>new Effect()));
Check(!session.Activate(2008002,1000,()=>true,()=>null));
var effect=new Effect();Check(session.Activate(2008002,1000,()=>true,()=>effect));
Check(!session.Activate(2008002,1000,()=>true,()=>new Effect()));
Check(session.Revoke());Check(effect.Disposed);session.Resume();
Check(!session.Activate(2008002,1000,()=>true,()=>new Effect()));
Check(session.Activate(2008002,61000,()=>true,()=>new Effect()));
var failed=new InterpretedAbilitySession(1,0);
Check(!failed.Activate(2008002,0,()=>true,()=>throw new Exception()));Check(failed.Quarantined);
failed.Resume();Check(!failed.Alive);
Console.WriteLine($"{count} interpreted lifecycle checks PASS; no native integration claimed");
sealed class Effect:IDisposable{public bool Disposed;public void Dispose()=>Disposed=true;}
