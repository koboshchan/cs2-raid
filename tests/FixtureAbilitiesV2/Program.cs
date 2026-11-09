using DeltaOps;
var checks=0;
void Check(bool value){checks++;if(!value)throw new Exception($"Fixture policy check {checks} failed");}
var p=new FixtureAbilityPolicyV2();
Check(!p.Begin(0));Check(p.Begin(1));Check(!p.Begin(1));Check(!p.Begin(0));
Check(!p.ReserveKit(1,11,21,2,false,true));
Check(!p.ReserveKit(1,11,21,2,true,false));
Check(!p.ReserveKit(2,11,21,2,true,true));
Check(!p.ReserveKit(1,0,21,2,true,true));
Check(!p.ReserveKit(1,11,uint.MaxValue,1,true,true));
Check(!p.ReserveKit(1,11,21,7,true,true));
Check(p.ReserveKit(1,11,21,2,true,true));
Check(!p.ReserveKit(1,11,21,2,true,true));
Check(!p.ReserveKit(1,11,22,2,true,true));
Check(p.Fence(1,11,21));Check(!p.Fence(1,11,22));
Check(!p.RecordUtility(1,11,21,31,false));
Check(p.RecordUtility(1,11,21,31,true));
Check(!p.RecordUtility(1,11,21,31,true));
Check(!p.Consume(1,12,21,31));Check(!p.Consume(1,11,22,31));
Check(!p.Redeemable(31));Check(!p.BuyUtilityAllowed);
Check(p.Consume(1,11,21,31));Check(!p.Consume(1,11,21,31));
Check(p.RecordUtility(1,11,21,32,true));
Check(!p.Begin(2));Check(!p.Revoke(1,11,21,_=>false));
Check(!p.Revoke(1,11,21,_=>throw new Exception()));
Check(p.Revoke(1,11,21,_=>true));
Check(!p.RecordUtility(1,11,21,32,true));
Check(!p.ReserveKit(1,11,21,2,true,true));
Check(p.Begin(2));Check(!p.Fence(1,11,21));
Check(p.ReserveKit(2,11,22,2,true,true));
Check(!p.RecordUtility(1,11,21,33,true));
Check(!p.Consume(1,11,21,31));
Check(p.ReserveKit(2,65547,23,2,true,true)); // same slot bits, different serial identity
var q=new FixtureAbilityPolicyV2();Check(q.Begin(1));var deliveries=0;
Check(!q.CommitKit(1,10,20,2,true,true,false,()=>{deliveries++;return true;}));Check(deliveries==0);
Check(!q.CommitKit(1,10,20,2,true,true,true,()=>{deliveries++;return false;}));
Check(q.Quarantined(10));Check(!q.Begin(2));
Check(!q.CommitKit(1,10,20,2,true,true,true,()=>{deliveries++;return true;}));Check(deliveries==1);
Check(q.CommitKit(1,12,22,2,true,true,true,()=>{deliveries++;return true;}));
Check(q.CommitKit(1,12,22,2,true,true,true,()=>{deliveries++;return true;}));Check(deliveries==2);
Check(!q.CommitKit(1,12,23,2,true,true,true,()=>true));
Check(!q.CommitKit(1,12,22,3,true,true,true,()=>true));
Check(!p.ReserveKit(2,88,99,1,true,true));
foreach(var (oldEnum,wire) in new[]{(StableOperatorIdentity.Stinger,2),(StableOperatorIdentity.Luna,3),(StableOperatorIdentity.Vyron,4),(StableOperatorIdentity.Hackclaw,5),(StableOperatorIdentity.Shepherd,6)}){
    Check(StableOperatorIdentity.Wire(oldEnum)==wire);Check(StableOperatorIdentity.EnumFromWire(wire)==oldEnum);
    var selection=new StableOperatorSelection{Value=oldEnum};Check(selection.Value==oldEnum&&!selection.Retired);
    selection.Value=0;Check(selection.Retired&&selection.Value==null);
    selection.Value=oldEnum;Check(selection.Retired&&selection.Value==null);
    selection.Value=null;Check(selection.Retired&&selection.Value==null);
}
Check(!StableOperatorIdentity.SelectableWire(1));Check(!StableOperatorIdentity.SelectableEnum(0));
try{StableOperatorIdentity.EnumFromWire(1);Check(false);}catch(ArgumentOutOfRangeException){Check(true);}
try{StableOperatorIdentity.Wire(0);Check(false);}catch(ArgumentOutOfRangeException){Check(true);}
Console.WriteLine($"{checks} fixture ability v2 policy checks passed");
