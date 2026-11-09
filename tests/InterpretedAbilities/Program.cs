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
Check(NativePassiveRules.Reduce(100,0.9f)==90);
Check(NativePassiveRules.Reduce(0,0.9f)==0);
Check(NativePassiveRules.Reduce(-2,0.9f)==-2);
Check(NativePassiveRules.Reduce(100,2)==100);
Check(NativePassiveRules.Reduce(100,float.NaN)==100);
var port=new FakePort();
foreach(var skill in InterpretedR2.Skills.Values){
 var before=port.Calls;
 var result=NativeAbilityRouter.Enact(skill.Operator,skill.Id,port);
 Check((result!=null)==(!skill.Passive && NativeAbilityRouter.Blocker(skill.Operator,skill.Id)==null));
 Check(port.Calls==before+(result==null?0:1));result?.Dispose();
}
foreach(var op in Enumerable.Range(1,5))foreach(var v in new[]{true,false})Check(InterpretedR2.Skills.ContainsKey((op,NativeAbilityRouter.Slot(op,v))));
Check(NativeAbilityRouter.Enact(1,1009001,port)==null);
var cleanupCalls=0;var owned=new OwnedAbilityEffect(()=>{cleanupCalls++;if(cleanupCalls==1)throw new Exception();});
try{owned.Dispose();}catch{}owned.Dispose();owned.Dispose();Check(cleanupCalls==2);
var cleanupFail=new InterpretedAbilitySession(1,0);var retry=0;
Check(cleanupFail.Activate(2008002,0,()=>true,()=>new OwnedAbilityEffect(()=>{if(++retry==1)throw new Exception();})));
Check(!cleanupFail.Revoke());Check(cleanupFail.Quarantined);Check(cleanupFail.Revoke());Check(retry==2);
// Independent review regressions: nested enact/admission, revoke during enact,
// stale return cleanup failure and retry, post-enact admission epoch invalidation.
var reentrant=new InterpretedAbilitySession(1,0);int creates=0;bool inner=true;
Check(reentrant.Activate(2008002,0,()=>true,()=>{
 creates++;inner=reentrant.Activate(2008002,0,()=>true,()=>{creates++;return new Effect();});return new Effect();}));
Check(!inner);Check(creates==1);Check(reentrant.Available(2008002,0)==0);
var duringAdmission=new InterpretedAbilitySession(1,0);int admissions=0;
Check(duringAdmission.Activate(2008002,0,()=>{
 admissions++;Check(!duringAdmission.Activate(2008002,0,()=>true,()=>new Effect()));return true;
},()=>new Effect()));Check(admissions==2);
var stale=new InterpretedAbilitySession(1,0);var staleEffect=new Effect();
Check(!stale.Activate(2008002,0,()=>true,()=>{Check(!stale.Revoke());stale.Resume();Check(!stale.Alive);return staleEffect;}));
Check(staleEffect.Disposed);Check(!stale.Alive);Check(stale.Available(2008002,0)==1);Check(stale.Revoke());
var staleFail=new InterpretedAbilitySession(1,0);int staleAttempts=0;
Check(!staleFail.Activate(2008002,0,()=>true,()=>{staleFail.Revoke();return new OwnedAbilityEffect(()=>{if(++staleAttempts==1)throw new Exception();});}));
Check(staleFail.Quarantined);Check(staleAttempts==1);Check(staleFail.Revoke());Check(staleAttempts==2);
var post=new InterpretedAbilitySession(1,0);var postEffect=new Effect();int postAdmission=0;
Check(!post.Activate(2008002,0,()=>{if(++postAdmission==2)post.Revoke();return true;},()=>postEffect));
Check(postEffect.Disposed);Check(!post.Alive);Check(post.Available(2008002,0)==1);
var nullEffect=new InterpretedAbilitySession(1,0);
Check(!nullEffect.Activate(2008002,0,()=>true,()=>null));Check(nullEffect.Available(2008002,0)==1);
var admissionThrow=new InterpretedAbilitySession(1,0);
Check(!admissionThrow.Activate(2008002,0,()=>throw new Exception(),()=>new Effect()));Check(admissionThrow.Quarantined);
var disposeReentry=new InterpretedAbilitySession(1,0);int disposals=0;
Check(disposeReentry.Activate(2008002,0,()=>true,()=>new OwnedAbilityEffect(()=>{disposals++;Check(!disposeReentry.Revoke());})));
Check(disposeReentry.Revoke());Check(disposals==1);
Console.WriteLine($"{count} interpreted lifecycle checks PASS; router + lifecycle PASS; CSS calls compile, not native QA");
sealed class Effect:IDisposable{public bool Disposed;public void Dispose()=>Disposed=true;}

sealed class FakePort:INativeAbilityPort{
 public int Calls;
 IDisposable New(){Calls++;return new OwnedAbilityEffect(()=>{});}
 public IDisposable? Pulse(bool suppress)=>New();
 public IDisposable? Heal()=>New();public IDisposable? Grant(string weapon)=>New();public IDisposable? Recon()=>New();public IDisposable? Rush()=>New();public IDisposable? Trap()=>New();
}
