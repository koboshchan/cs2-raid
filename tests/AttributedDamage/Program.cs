using Cs2Raid;
int n=0;void Check(bool b){n++;if(!b)throw new Exception("check "+n);}
var request=new AbilityDamageRequest(Guid.NewGuid(),1,1,2,3,4,5,10);
var driver=new Driver();bool thread=true,allowed=true;
AttributedAbilityDamage? boundary=null;
boundary=new(()=>thread,_=>allowed,driver);
Check(boundary.Apply(request)==AbilityDamageOutcome.Applied);
Check(boundary.Apply(request)==AbilityDamageOutcome.Duplicate);Check(driver.Calls==1);
allowed=false;Check(boundary.Apply(request with{Event=Guid.NewGuid()})==AbilityDamageOutcome.Denied);
allowed=true;thread=false;Check(boundary.Apply(request with{Event=Guid.NewGuid()})==AbilityDamageOutcome.Denied);
thread=true;driver.Throw=true;var uncertain=request with{Event=Guid.NewGuid()};
Check(boundary.Apply(uncertain)==AbilityDamageOutcome.Indeterminate);
Check(boundary.Apply(uncertain)==AbilityDamageOutcome.Duplicate);Check(driver.Calls==2);
driver.Throw=false;driver.Callback=()=>Check(boundary.Apply(request with{Event=Guid.NewGuid()})==AbilityDamageOutcome.Denied);
Check(boundary.Apply(request with{Event=Guid.NewGuid()})==AbilityDamageOutcome.Applied);
driver.Callback=null;driver.Accept=false;var rejected=request with{Event=Guid.NewGuid()};
Check(boundary.Apply(rejected)==AbilityDamageOutcome.NativeRejected);
Check(boundary.Apply(rejected)==AbilityDamageOutcome.Duplicate);
Check(new AttributedAbilityDamage(()=>true,_=>true,new UnvalidatedAbilityDamageDriver()).Apply(request)==AbilityDamageOutcome.Unsupported);
foreach(var d in new[]{float.NaN,float.PositiveInfinity,-1f,0f})
Check(boundary.Apply(request with{Event=Guid.NewGuid(),Damage=d})==AbilityDamageOutcome.Denied);
for(int mask=0;mask<32;mask++){
 var calls=driver.Calls;
 var r=request with{Event=Guid.NewGuid(),AttackerController=(mask&1)==0?0u:1u,
 AttackerPawn=(mask&2)==0?0u:2u,TargetController=(mask&4)==0?0u:3u,
 TargetPawn=(mask&8)==0?0u:4u,Inflictor=(mask&16)==0?0u:5u};
 Check(boundary.Apply(r)==(mask==31?AbilityDamageOutcome.NativeRejected:AbilityDamageOutcome.Denied));
 Check(driver.Calls==calls+(mask==31?1:0));
}
for(int mask=0;mask<2048;mask++){
 bool B(int bit)=>(mask&(1<<bit))!=0;
 var authority=new AbilityDamageAuthority(B(0),B(1),request.Generation,B(2),B(3),
 B(4),B(5),B(6),B(7),B(8),B(9),B(10));
 Check(AbilityDamageAuthorization.Allow(request,authority)==(mask==2047));
 Check(!AbilityDamageAuthorization.Allow(request with{Generation=2},authority));
}
Console.WriteLine($"Attributed damage boundary {n} checks PASS; native ABI NOT accepted");
sealed class Driver:IAttributedDamageDriver{
 public bool Supported=>true;public bool Throw,Accept=true;public int Calls;public Action? Callback;
 public bool Apply(AbilityDamageRequest r){Calls++;Callback?.Invoke();if(Throw)throw new Exception();return Accept;}
}
