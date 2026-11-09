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
// Exercise the exact mutation/cleanup boundary used by CSS pulse/trap, not router self-consistency.
var boundary=new InterpretedAbilitySession(1,0);
float nativeValue=1;uint serial=42;int writes=0,notifications=0;
NativeStatusLease Lease()=>new(()=>serial==42,()=>nativeValue,v=>{writes++;nativeValue=v;},
    ()=>{notifications++;},1,0.5f);
var lease=Lease();
Check(boundary.Activate(2008002,0,()=>true,()=>boundary.EnactOwned(lease.Apply,lease.Dispose)));
Check(nativeValue==0.5f);Check(boundary.PendingCleanup==1);
Check(boundary.Revoke());Check(nativeValue==1);Check(writes==2);Check(notifications==2);
var other=Lease();other.Apply();nativeValue=0.8f;var previousWrites=writes;
other.Dispose();Check(nativeValue==0.8f);Check(writes==previousWrites);
nativeValue=1;var replaced=Lease();replaced.Apply();serial=43;previousWrites=writes;
replaced.Dispose();Check(nativeValue==0.5f);Check(writes==previousWrites);
serial=42;nativeValue=1;
var failedNative=new InterpretedAbilitySession(1,0);int rollbackAttempts=0;
Check(!failedNative.Activate(2008002,0,()=>true,()=>failedNative.EnactOwned(
    ()=>{nativeValue=0.5f;throw new Exception("native post-write");},
    ()=>{if(++rollbackAttempts==1)throw new Exception("rollback unavailable");nativeValue=1;})));
Check(failedNative.Quarantined);Check(failedNative.PendingCleanup==1);
Check(failedNative.Revoke());Check(nativeValue==1);Check(rollbackAttempts==2);
Check(failedNative.Available(2008002,0)==0);
int notifyCalls=0;nativeValue=1;writes=0;
var notifyRetry=new NativeStatusLease(()=>true,()=>nativeValue,v=>{nativeValue=v;writes++;},
    ()=>{if(++notifyCalls==2)throw new Exception("state changed");},1,0.5f);
notifyRetry.Apply();try{notifyRetry.Dispose();Check(false);}catch{Check(true);}
Check(nativeValue==1);Check(writes==2);notifyRetry.Dispose();Check(writes==2);Check(notifyCalls==3);
var lateAdmission=new InterpretedAbilitySession(1,0);int lateCalls=0;var lateEffect=new Effect();
Check(!lateAdmission.Activate(2008002,0,()=>++lateCalls==1?true:throw new Exception("admission"),()=>lateEffect));
Check(lateEffect.Disposed);Check(lateAdmission.PendingCleanup==0);
foreach(var initial in new[]{true,false}){
 NativeConfigTransition.Validate(false,!initial,initial,new object());Check(true);
 NativeConfigTransition.Validate(true,initial,initial,new object());Check(true);
 try{NativeConfigTransition.Validate(true,initial,!initial,new object());Check(false);}catch(InvalidOperationException){Check(true);}
 try{NativeConfigTransition.Validate(true,initial,initial,null);Check(false);}catch(ArgumentNullException){Check(true);}
}
foreach(var grant in new[]{(1,2008002),(1,2008005),(5,3008002),(5,3008008),(4,4009003)}){
 var grants=new FakePort();Check(NativeAbilityRouter.Blocker(grant.Item1,grant.Item2)!=null);
 Check(NativeAbilityRouter.Enact(grant.Item1,grant.Item2,grants)==null);Check(grants.Calls==0);
}
var suppressionCounts=new Dictionary<uint,int>();
var ownSuppression=new NativeSuppressionLease(suppressionCounts,42);
var anotherSuppression=new NativeSuppressionLease(suppressionCounts,42);
ownSuppression.Acquire();ownSuppression.Acquire();anotherSuppression.Acquire();
Check(suppressionCounts[42]==2);ownSuppression.Dispose();Check(suppressionCounts[42]==1);
ownSuppression.Dispose();Check(suppressionCounts[42]==1);
Check(!suppressionCounts.ContainsKey(43));anotherSuppression.Dispose();Check(suppressionCounts.Count==0);
foreach(var end in new[]{"disconnect","pawn replacement","unload"}){
 var life=new InterpretedAbilitySession(1,0);nativeValue=1;
 var scalar=Lease();var suppression=new NativeSuppressionLease(suppressionCounts,42);
 Check(life.Activate(2008002,0,()=>true,()=>life.EnactOwned(
    ()=>{suppression.Acquire();scalar.Apply();},
    ()=>{scalar.Dispose();suppression.Dispose();})));
 Check(suppressionCounts[42]==1);Check(life.Revoke());Check(nativeValue==1);Check(suppressionCounts.Count==0);
}
// Durable entity ownership boundary, independent from router/ability labels.
NativeEntityTicket Ticket(uint serial,string world="map-A") =>
 new(world,serial,"deltaops-owned-"+Guid.NewGuid().ToString("N"),"smokegrenade_projectile");
IReadOnlyList<NativeEntityTicket> disk=Array.Empty<NativeEntityTicket>();
bool failSave=false;int escrowWrites=0;
void Save(IReadOnlyList<NativeEntityTicket> snapshot){
 escrowWrites++;if(failSave)throw new IOException("disk full");disk=snapshot.ToArray();
}
var escrow=new NativeEntityEscrow("map-A",disk,Save);
var native=new EntityMock();var first=Ticket(123);var second=Ticket(456);
failSave=true;try{escrow.Record(first);Check(false);}catch(IOException){}
Check(escrow.Pending==0);Check(disk.Count==0);Check(native.Removes==0);
failSave=false;escrow.Record(first);native.Entities[first.Serial]=first;
Check(escrow.Pending==1);Check(disk.Single()==first);
int beforeWrites=escrowWrites;escrow.Record(first);Check(escrowWrites==beforeWrites);
try{escrow.Record(first with {Marker=Ticket(123).Marker});Check(false);}catch(InvalidOperationException){}
Check(disk.Single()==first);
// Simulated unload destroys the runtime object; a fresh instance owns persisted records.
escrow=new NativeEntityEscrow("map-A",disk,Save);native.FailRemove=true;
Check(!escrow.Drain(native));Check(escrow.Pending==1);Check(disk.Count==1);
native.FailRemove=false;native.KeepAfterRemove=true;
Check(!escrow.Drain(native));Check(disk.Count==1);
native.KeepAfterRemove=false;failSave=true;
Check(!escrow.Drain(native));Check(!native.Entities.ContainsKey(first.Serial));Check(disk.Count==1);
failSave=false;int beforeRemove=native.Removes;
Check(escrow.Drain(native));Check(native.Removes==beforeRemove);Check(disk.Count==0);
// A different owner's marker, even with the same serial/class, is never removed.
escrow.Record(first);native.Entities[first.Serial]=first with {Marker=Ticket(123).Marker};
Check(!escrow.Drain(native));Check(escrow.Pending==1);Check(native.Removes==beforeRemove);
escrow.Record(second);native.Entities[second.Serial]=second;
Check(!escrow.Drain(native));Check(escrow.Pending==1);Check(!native.Entities.ContainsKey(second.Serial));
// Different map/process records provide no authority, even if native handles collide.
var otherMap=new NativeEntityEscrow("map-B",disk,Save);
beforeRemove=native.Removes;Check(otherMap.Drain(native));Check(native.Removes==beforeRemove);
try{escrow.Record(Ticket(789,"map-B"));Check(false);}catch(InvalidOperationException){}
try{escrow.Record(Ticket(0));Check(false);}catch(ArgumentException){}
try{escrow.Record(Ticket(789) with {Marker="foreign"});Check(false);}catch(ArgumentException){}
// Save and engine callbacks cannot reenter Drain or publish another ticket.
var nestedTicket=Ticket(999);NativeEntityEscrow? reentryEscrow=null;bool nestedBlocked=false;
reentryEscrow=new NativeEntityEscrow("map-A",[],snapshot=>{
 Check(!reentryEscrow!.Drain(native));
 try{reentryEscrow.Record(nestedTicket);}catch(InvalidOperationException){nestedBlocked=true;}
});
reentryEscrow.Record(Ticket(888));Check(nestedBlocked);Check(reentryEscrow.Pending==1);
Console.WriteLine($"{count} interpreted lifecycle checks PASS; router + lifecycle PASS; CSS calls compile, not native QA");
sealed class Effect:IDisposable{public bool Disposed;public void Dispose()=>Disposed=true;}

sealed class FakePort:INativeAbilityPort{
 public int Calls;
 IDisposable New(){Calls++;return new OwnedAbilityEffect(()=>{});}
 public IDisposable? Pulse(bool suppress)=>New();
 public IDisposable? Heal()=>New();public IDisposable? Grant(string weapon)=>New();public IDisposable? Recon()=>New();public IDisposable? Rush()=>New();public IDisposable? Trap()=>New();
}


sealed class EntityMock:INativeEntityBoundary{
 public readonly Dictionary<uint,NativeEntityTicket> Entities=[];
 public int Removes;public bool FailRemove,KeepAfterRemove;
 public bool Exists(uint serial)=>Entities.ContainsKey(serial);
 public bool Matches(NativeEntityTicket ticket)=>Entities.TryGetValue(ticket.Serial,out var actual)&&actual==ticket;
 public void Remove(NativeEntityTicket ticket){
  if(!Matches(ticket))throw new InvalidOperationException("foreign removal");
  Removes++;if(FailRemove)throw new Exception("native removal failed");
  if(!KeepAfterRemove)Entities.Remove(ticket.Serial);
 }
}
