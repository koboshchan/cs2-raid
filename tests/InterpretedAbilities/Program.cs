using DeltaOps;
int count=0;
void Check(bool x){count++;if(!x)throw new Exception($"check {count}");}
Check(InterpretedR2.Skills.Count==26);
foreach(var skill in InterpretedR2.Skills.Values)
 Check(NativeApiCapabilityEvidence.ReadOnlyRecon(skill.Operator,skill.Id)==
       ((skill.Operator==2&&skill.Id==2002017)||(skill.Operator==4&&skill.Id==1002016)));
Check(!NativeApiCapabilityEvidence.ReadOnlyRecon(1,2002017));
Check(!NativeApiCapabilityEvidence.ReadOnlyRecon(4,4008010));
foreach(var skill in InterpretedR2.Skills.Values) {
    var expected=NativeAbilityRouter.Blocker(skill.Operator,skill.Id);
    var actual=NativeApiCapabilityEvidence.Blocker(skill.Operator,skill.Id);
    Check(actual==null == (expected==null && NativeApiCapabilityEvidence.ReadOnlyRecon(skill.Operator,skill.Id)));
    if(expected!=null)Check(actual==expected);
}
Check(NativeApiCapabilityEvidence.Blocker(1,4008010)!=null);
Check(!NativeApiCapabilityEvidence.EntityCreation);
Check(!NativeApiCapabilityEvidence.ScalarMutation);
Check(!NativeApiCapabilityEvidence.MovementMutation);
Check(!NativeApiCapabilityEvidence.DamageMutation);
Check(!NativeApiCapabilityEvidence.HitMark);
foreach(var excluded in new[]{(1,20010215),(1,30010203),(2,30010203),(3,1009002),(3,30010203),(4,30010203),(5,30010202)}){
 Check(!InterpretedR2.Skills.ContainsKey(excluded));
 var rejectedPort=new FakePort();
 Check(NativeAbilityRouter.Enact(excluded.Item1,excluded.Item2,rejectedPort)==null);
 Check(rejectedPort.Calls==0);
 var excludedSession=new InterpretedAbilitySession(excluded.Item1,0);
 Check(!excludedSession.Activate(excluded.Item2,999999,()=>true,()=>new Effect()));
}
Check(InterpretedR2.Skills[(3,1009005)].CdId==16);
Check(!InterpretedR2.Skills.ContainsKey((3,1009002)));
Check(InterpretedR2.Skills.Values.Count(s=>s.Passive)==6);
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
// Actual disk-backed handoff, not a mock claiming to be durable.
var tempDirectory=Path.Combine(Path.GetTempPath(),"ability-escrow-test-"+Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(tempDirectory);
try{
 var file=Path.Combine(tempDirectory,"ownership.json");var store=new NativeEntityFileStore(file);
 Check(store.Read().Count==0);store.Write(new[]{first,second});
 Check(new NativeEntityFileStore(file).Read().SequenceEqual(new[]{first,second}));
 var recovered=new NativeEntityEscrow("map-A",new NativeEntityFileStore(file).Read(),store.Write);
 var recoveryNative=new EntityMock();recoveryNative.Entities[first.Serial]=first;
 recoveryNative.Entities[second.Serial]=second;recoveryNative.FailRemove=true;
 Check(!recovered.Drain(recoveryNative));Check(new NativeEntityFileStore(file).Read().Count==2);
 recoveryNative.FailRemove=false;Check(recovered.Drain(recoveryNative));
 Check(new NativeEntityFileStore(file).Read().Count==0);
 Check(Directory.GetFiles(tempDirectory).Length==1);
 File.WriteAllText(file,"corrupt");
 try{store.Read();Check(false);}catch(System.Text.Json.JsonException){}
 File.WriteAllText(file,"{\"Schema\":2,\"Tickets\":[]}");
 try{store.Read();Check(false);}catch(InvalidDataException){}
 Check(File.ReadAllText(file).Contains("2"));
}finally{Directory.Delete(tempDirectory,recursive:true);}
// Production uses this exact identity decision before touching saved native serials.
var processId="123-456:";var worldId=processId+Guid.NewGuid().ToString("N");
var anchorTicket=Ticket(12,worldId) with {Kind="info_target"};
var worldEffect=Ticket(13,worldId);var worldSaved=new[]{anchorTicket,worldEffect};
Check(NativeWorldRecoveryPolicy.Decide([],processId,false,_=>false)==NativeWorldRecoveryDecision.CreateNew);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,processId,false,_=>true)==NativeWorldRecoveryDecision.RecoverExisting);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,processId,false,_=>false)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,processId,true,_=>throw new Exception())==NativeWorldRecoveryDecision.CreateNew);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,"123-457:",false,_=>throw new Exception())==NativeWorldRecoveryDecision.CreateNew);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,"124-456:",false,_=>throw new Exception())==NativeWorldRecoveryDecision.CreateNew);
Check(NativeWorldRecoveryPolicy.Decide(new[]{worldEffect},processId,false,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,anchorTicket},processId,false,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,worldEffect with{World="other"}},processId,false,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket with{World="corrupt"}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket with{Marker="foreign"}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(worldSaved,"",false,_=>true)==NativeWorldRecoveryDecision.Block);
// Shared actual creation/activation/rollback transaction used by the CSS adapter.
disk=[];var transactionEscrow=new NativeEntityEscrow("map-A",[],Save);
var creation=new CreationMock(Ticket(800));var transaction=new NativeEntityTransaction(transactionEscrow,creation);
failSave=true;
try{transaction.Activate();Check(false);}catch(IOException){}
Check(creation.Activations==0);Check(creation.Exists(800));Check(transactionEscrow.Pending==0);
creation.FailRemove=true;
try{transaction.Dispose();Check(false);}catch(Exception){}
Check(creation.Exists(800));creation.FailRemove=false;transaction.Dispose();Check(!creation.Exists(800));
failSave=false;creation=new CreationMock(Ticket(801)){FailActivate=true};
transaction=new NativeEntityTransaction(transactionEscrow,creation);
try{transaction.Activate();Check(false);}catch(InvalidOperationException){}
Check(transactionEscrow.Pending==1);Check(disk.Count==1);
creation.FailRemove=true;try{transaction.Dispose();Check(false);}catch(Exception){}
Check(transactionEscrow.Pending==1);creation.FailRemove=false;transaction.Dispose();Check(transactionEscrow.Pending==0);
creation=new CreationMock(Ticket(802));transaction=new NativeEntityTransaction(transactionEscrow,creation);
transaction.Activate();Check(creation.Activations==1);Check(disk.Count==1);
failSave=true;try{transaction.Dispose();Check(false);}catch(IOException){}
Check(!creation.Exists(802));Check(transactionEscrow.Pending==1);
failSave=false;transaction.Dispose();Check(disk.Count==0);Check(transactionEscrow.Pending==0);
var deletes=creation.Removes;transaction.Dispose();Check(creation.Removes==deletes);
try{transaction.Activate();Check(false);}catch(InvalidOperationException){}
// Releasing one effect cannot remove another effect's ticket or entity.
var a=Ticket(810);var b=Ticket(811);transactionEscrow.Record(a);transactionEscrow.Record(b);
var releaseNative=new EntityMock();releaseNative.Entities[a.Serial]=a;releaseNative.Entities[b.Serial]=b;
Check(transactionEscrow.Release(a,releaseNative));Check(releaseNative.Exists(b.Serial));Check(disk.Single()==b);
Check(!transactionEscrow.Release(b with{Marker=a.Marker},releaseNative));Check(releaseNative.Exists(b.Serial));
Check(transactionEscrow.Release(b,releaseNative));
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,worldEffect with{Serial=anchorTicket.Serial}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,worldEffect with{Marker=anchorTicket.Marker}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,worldEffect with{Marker=null!}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,worldEffect with{Serial=0}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket with{World="bad:"+Guid.NewGuid().ToString("N")}},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
Check(NativeWorldRecoveryPolicy.Decide(new[]{anchorTicket,null!},processId,true,_=>true)==NativeWorldRecoveryDecision.Block);
// Incomplete creation failure is retained in the native adapter, before ticket return.
creation=new CreationMock(Ticket(820)){FailCreate=true};
transaction=new NativeEntityTransaction(transactionEscrow,creation);
try{transaction.Activate();Check(false);}catch(InvalidOperationException){}
Check(!creation.Exists(820));Check(transactionEscrow.Pending==0);Check(creation.Activations==0);
creation=new CreationMock(Ticket(821)){FailCreate=true,FailRemove=true};
transaction=new NativeEntityTransaction(transactionEscrow,creation);
try{transaction.Activate();Check(false);}catch(Exception){}
Check(creation.Exists(821));Check(transactionEscrow.Pending==0);
creation.FailRemove=false;transaction.Dispose();Check(!creation.Exists(821));
// Production projectile damage gate, exhaustive truth table against independently stated policy.
for(var mask=0;mask<1024;mask++){
 bool Bit(int index)=>(mask&(1<<index))!=0;
 var expected=true;
 for(var n=0;n<6;n++)if(!Bit(n))expected=false;
 if(Bit(6)){if(!Bit(7)||!Bit(8))expected=false;}
 else if(!Bit(9))expected=false;
 Check(NativeProjectileDamagePolicy.Allow(Bit(0),Bit(1),Bit(2),Bit(3),Bit(4),Bit(5),Bit(6),Bit(7),Bit(8),Bit(9))==expected);
}
// Session owns pre-ticket failures too, including cleanup throwing twice then recovering.
var preTicketSession=new InterpretedAbilitySession(1,0);
creation=new CreationMock(Ticket(830)){FailCreate=true,FailRemove=true};
transaction=new NativeEntityTransaction(transactionEscrow,creation);
Check(!preTicketSession.Activate(2008002,0,()=>true,
 ()=>preTicketSession.EnactOwned(transaction.Activate,transaction.Dispose)));
Check(preTicketSession.Quarantined);Check(preTicketSession.PendingCleanup==1);
Check(creation.Exists(830));Check(!preTicketSession.Revoke());
creation.FailRemove=false;Check(preTicketSession.Revoke());Check(!creation.Exists(830));
Check(preTicketSession.PendingCleanup==0);
// A returned entity with post-spawn error and failed remove is durably reconstructible.
var spawnSession=new InterpretedAbilitySession(1,0);
creation=new CreationMock(Ticket(831)){FailActivate=true,FailRemove=true};
transaction=new NativeEntityTransaction(transactionEscrow,creation);
Check(!spawnSession.Activate(2008002,0,()=>true,
 ()=>spawnSession.EnactOwned(transaction.Activate,transaction.Dispose)));
Check(spawnSession.PendingCleanup==1);Check(disk.Single().Serial==831);
var nextOwner=new NativeEntityEscrow("map-A",disk,Save);
creation.FailRemove=false;Check(nextOwner.Drain(creation));
Check(disk.Count==0);Check(!creation.Exists(831));
Check(spawnSession.Revoke());Check(spawnSession.PendingCleanup==0);
// Concrete failed-unload -> engine expiry while plugin absent -> next-loader recovery.
disk=[];var expiryStore=new NativeEntityEscrow("map-A",[],Save);
creation=new CreationMock(Ticket(840));transaction=new NativeEntityTransaction(expiryStore,creation);
transaction.Activate();creation.FailRemove=true;
Check(!expiryStore.Drain(creation));Check(disk.Single().Serial==840);
var unloadRemoveCount=creation.Removes;
creation.EngineExpire(); // Independent engine lifetime, not a plugin timer or cleanup callback.
Check(creation.Removes==unloadRemoveCount);Check(!creation.Exists(840));
Check(disk.Count==1); // No promise of file cleanup while unloaded.
var expiryReload=new NativeEntityEscrow("map-A",disk,Save);
Check(expiryReload.Drain(creation));Check(disk.Count==0);Check(creation.Removes==unloadRemoveCount);
// Expired record plus a foreign full serial still cannot authorize foreign deletion.
creation=new CreationMock(Ticket(841));transaction=new NativeEntityTransaction(expiryStore,creation);
transaction.Activate();creation.FailRemove=true;Check(!expiryStore.Drain(creation));
creation.EngineExpire();
var reusedForeign=new EntityMock();
reusedForeign.Entities[841]=Ticket(841) with {Marker="deltaops-owned-"+Guid.NewGuid().ToString("N")};
var foreignReload=new NativeEntityEscrow("map-A",disk,Save);
Check(!foreignReload.Drain(reusedForeign));Check(reusedForeign.Entities.Count==1);
Check(disk.Single().Serial==841);
for(var mask=0;mask<64;mask++){
 bool TrackBit(int index)=>(mask&(1<<index))!=0;
 foreach(var selected in Enumerable.Range(1,5))
 foreach(var healthDamage in new[]{-1,0,1,100}){
  var expected=selected==2&&mask==59&&healthDamage>0;
  Check(NativeHitTrackingPolicy.Allow(selected,TrackBit(0),TrackBit(1),TrackBit(2),
      TrackBit(3),TrackBit(4),TrackBit(5)?healthDamage:0)==(expected&&TrackBit(5)));
 }
}
// Passive readmission on a new pawn must not mint a fresh charge ledger.
var readmission=new InterpretedAbilitySession(2,0);
Check(readmission.Activate(4008002,0,()=>true,()=>new OwnedAbilityEffect(()=>{})));
Check(readmission.Available(4008002,0)==0);
Check(readmission.Revoke());Check(!readmission.Alive);
readmission.Resume();Check(readmission.Alive);Check(readmission.Available(4008002,0)==0);
readmission.Quarantine();readmission.Resume();Check(!readmission.Alive);
if(args.Length==2&&args[0]=="--coverage")CoverageReceipt.Write(args[1]);
for(var mask=0;mask<8;mask++)
foreach(var selected in Enumerable.Range(1,5))
foreach(var damageType in new uint[]{0,2,32,34,64}){
 var actual=NativeFallPassivePolicy.Apply(100,damageType,selected,(mask&1)!=0,(mask&2)!=0,(mask&4)!=0,.5f);
 Check(actual==(mask==3&&selected==3&&damageType==32?50:100));
}
foreach(var multiplier in new[]{-1f,2f,float.NaN,float.PositiveInfinity})
Check(NativeFallPassivePolicy.Apply(100,32,4,true,true,false,multiplier)==100);
foreach(var flags in new uint[]{0,1,2,32,64,66,96,128})
Check(NativePassiveRules.ReduceBlast(100,.9f,flags)==((flags&64)!=0?90:100));
Check(ExtractedPassiveValues.ShepherdExplosionMultiplier==.7f);
Check(NativePassiveRules.ReduceBlast(100,ExtractedPassiveValues.ShepherdExplosionMultiplier,64)==70);
Check(NativePassiveRules.ReduceBlast(100,ExtractedPassiveValues.ShepherdExplosionMultiplier,2)==100);

// Committed healing never rolls back intervening damage and never repeats its write.
foreach(var failWrite in new[]{false,true})
foreach(var failNotify in new[]{false,true}) {
 int hp=60,healWrites=0,healNotifications=0;bool valid=true,throwNotification=failNotify;
 var heal=new NativeCommittedHeal(()=>valid,()=>hp,()=>100,v=>{
     hp=v;healWrites++;if(failWrite)throw new Exception("setter outcome unknown");
 },()=>{healNotifications++;if(throwNotification)throw new Exception("network failure");},25);
 try{heal.Apply();}catch{}
 Check(hp==85&&healWrites==1);
 hp=49;throwNotification=false;heal.Dispose();heal.Dispose();
 Check(hp==49&&healWrites==1);
 Check(healNotifications==(failWrite?1:failNotify?2:1));
 try{heal.Apply();Check(false);}catch(InvalidOperationException){Check(true);}
}
{
 int hp=90,healWrites=0,notifies=0;bool valid=true;
 var heal=new NativeCommittedHeal(()=>valid,()=>hp,()=>100,v=>{hp=v;healWrites++;},
     ()=>{notifies++;throw new Exception();},int.MaxValue);
 try{heal.Apply();}catch{}
 Check(hp==100&&healWrites==1&&notifies==1);
 valid=false;heal.Dispose();Check(notifies==1);
}
foreach(var initial in new[]{0,100,101}) {
 int hp=initial,healWrites=0;
 var heal=new NativeCommittedHeal(()=>true,()=>hp,()=>100,v=>{hp=v;healWrites++;},()=>{},25);
 try{heal.Apply();Check(false);}catch(InvalidOperationException){Check(true);}
 heal.Dispose();Check(hp==initial&&healWrites==0);
}

// Integration with admitted session: failed notify retains cleanup until retry,
// spends only once, and later health changes survive quarantine/revocation.
{
 var hs=new InterpretedAbilitySession(1,0);
 int hp=50,healWriteCount=0,notifyCount=0;bool broken=true;
 var h=new NativeCommittedHeal(()=>true,()=>hp,()=>100,v=>{hp=v;healWriteCount++;},
     ()=>{notifyCount++;if(broken)throw new Exception();},25);
 Check(!hs.Activate(20010101,30000,()=>true,()=>hs.EnactOwned(h.Apply,h.Dispose)));
 Check(hs.Quarantined&&hs.PendingCleanup==1&&hp==75&&healWriteCount==1);
 hp=31;broken=false;Check(hs.Revoke());
 Check(hs.PendingCleanup==0&&hp==31&&healWriteCount==1&&notifyCount==3);
}
{
 int hp=50;
 var h=new NativeCommittedHeal(()=>true,()=>hp,()=>100,v=>hp=v,()=>{},25);
 h.Dispose();
 try{h.Apply();Check(false);}catch(InvalidOperationException){Check(true);}
 Check(hp==50);
}

foreach(var yawDegrees in new[]{0f,90f,-90f,180f,360f,float.MaxValue}) {
 Check(NativeRushImpulse.TryCreate(yawDegrees,400,-37,out var impulse));
 Check(float.IsFinite(impulse.X)&&float.IsFinite(impulse.Y)&&impulse.Z==-37);
 Check(Math.Abs(Math.Sqrt((double)impulse.X*impulse.X+(double)impulse.Y*impulse.Y)-400)<0.001);
}
foreach(var invalid in new[]{float.NaN,float.PositiveInfinity,float.NegativeInfinity}) {
 Check(!NativeRushImpulse.TryCreate(invalid,400,0,out _));
 Check(!NativeRushImpulse.TryCreate(0,invalid,0,out _));
 Check(!NativeRushImpulse.TryCreate(0,400,invalid,out _));
}
Check(!NativeRushImpulse.TryCreate(0,0,0,out _));
Check(!NativeRushImpulse.TryCreate(0,1001,0,out _));

Check(!NativeApiCapabilityEvidence.HealingActivation);
Check(NativeImplementationEvidence.SafetyBlocker(20010101)==null);
Check(NativeImplementationEvidence.SafetyBlocker(1009001)==null);
Check(NativeImplementationEvidence.SafetyBlocker(3008001)!=null);
foreach(var sk in InterpretedR2.Skills.Values) {
 Check(!string.IsNullOrEmpty(NativeImplementationEvidence.Implementation(sk.Id)));
 Check(!string.IsNullOrEmpty(NativeImplementationEvidence.Approval(sk.Operator,sk.Id)));
}
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


sealed class CreationMock(NativeEntityTicket ticket):INativeEntityCreation{
 private bool exists;
 public bool FailActivate,FailRemove,FailCreate;public int Activations,Removes;
 public NativeEntityTicket CreateInert(){
  exists=true;if(FailCreate)throw new InvalidOperationException("marker write failed");return ticket;
 }
 public void CleanupIncompleteCreation(){if(exists)Remove(ticket);}
 public void EngineExpire(){exists=false;}
 public void Activate(NativeEntityTicket t){
  if(t!=ticket||!exists)throw new Exception("invalid activation");
  Activations++;if(FailActivate)throw new InvalidOperationException("post-spawn failure");
 }
 public bool Exists(uint serial)=>exists&&serial==ticket.Serial;
 public bool Matches(NativeEntityTicket t)=>exists&&t==ticket;
 public void Remove(NativeEntityTicket t){
  if(!Matches(t))throw new Exception("foreign delete");
  Removes++;if(FailRemove)throw new Exception("remove failed");exists=false;
 }
}
