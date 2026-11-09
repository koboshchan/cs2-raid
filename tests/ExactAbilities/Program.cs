using Cs2Raid.Abilities;
using System.Text.Json;

// ALL identities, values, units and policies below are SYNTHETIC TEST FIXTURES, not DF data.
int checks = 0;
void Check(bool ok) { checks++; if (!ok) throw new Exception($"Check {checks} failed"); }
void Reject(Action action) { try { action(); } catch (InvalidDataException) { checks++; return; } throw new Exception($"Expected invalid data at {checks}"); }
const string raw = """{"skill":"synthetic-skill","operator":"synthetic-operator","cooldown":7,"duration":2,"charges":2,"mode":"finite","start":"activation","damage":13,"nested":{"a/b":[9]},"secondsUnit":"seconds","countUnit":"count","pointsUnit":"points"}""";
var row = new SourceRow("synthetic/table", "synthetic-row", new string('a',64), raw, ExactAbilityCatalog.Sha256(raw));
FieldEvidence E(string pointer, string value) => new(row.Table, row.Row, pointer, value);
ExactNumber N(decimal value, string unit, string field) => new(value, unit, E("/"+field, value.ToString(System.Globalization.CultureInfo.InvariantCulture)), null, E("/"+unit+"Unit",JsonSerializer.Serialize(unit)));
var unknown = new ExactNumber(null,null,null,"Not decoded in synthetic fixture");
var skill = new AbilityDefinition("synthetic-skill", E("/skill", "\"synthetic-skill\""), "synthetic-adapter",
    N(7,"seconds","cooldown"), N(2,"seconds","duration"), N(2,"count","charges"),
    new("finite", E("/mode","\"finite\""),null), new("activation",E("/start","\"activation\""),null),
    new() { ["damage"] = N(13,"points","damage") }, []);
var op = new OperatorDefinition("synthetic-operator",E("/operator","\"synthetic-operator\""),[skill.SkillId]);
AbilityExport Export(AbilityDefinition? replacement = null) => new(1,[row],[op],[replacement??skill]);
var catalog = new ExactAbilityCatalog(Export());
Check(catalog.ActivationBlocker(skill.SkillId) == null);
Check(catalog.ActivationBlocker("missing") != null);
var loaded = ExactAbilityCatalog.Load(JsonSerializer.Serialize(Export()));
Check(loaded.HasOperator(op.OperatorId));
Reject(() => ExactAbilityCatalog.Load("{\"SchemaVersion\":1,\"SchemaVersion\":1}"));
Reject(() => new ExactAbilityCatalog(Export() with { SchemaVersion=2 }));
Reject(() => new ExactAbilityCatalog(Export() with { Rows=[row,row] }));
Reject(() => new ExactAbilityCatalog(Export() with { Rows=[row with { RawJson=raw+" " }] }));
Reject(() => new ExactAbilityCatalog(Export() with { Rows=[row with { AssetSha256="bad" }] }));
Reject(() => new ExactAbilityCatalog(Export() with { Abilities=[skill,skill] }));
Reject(() => new ExactAbilityCatalog(Export() with { Operators=[op,op] }));
Reject(() => new ExactAbilityCatalog(Export() with { Operators=[op with { SkillIds=["missing"] }] }));
Reject(() => new ExactAbilityCatalog(Export() with { Operators=[op with { SkillIds=[skill.SkillId,skill.SkillId] }] }));
Reject(() => new ExactAbilityCatalog(Export(skill with { Cooldown=skill.Cooldown with { Value=8 } })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Cooldown=skill.Cooldown with { UnitEvidence=null } })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Cooldown=skill.Cooldown with { Unit="milliseconds" } })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Identity=E("/skill","\"other\"") })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Duration=unknown with { Unit="seconds" } })));
Reject(() => new ExactAbilityCatalog(Export(skill with { References=[new("CDID","missing","90",null)] })));
Reject(() => new ExactAbilityCatalog(Export(skill with { References=[new("CDID",row.Table,row.Row,"unknown")] })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Parameters=new(){ ["bad"]=N(9,"count","absent") } })));
Reject(() => new ExactAbilityCatalog(Export(skill with { Parameters=new(){ ["bad"]=new(9,"count",E("/nested/a~2b/0","9"),null) } })));
var nested = new ExactAbilityCatalog(Export(skill with {
    Parameters=new(){ ["nested"]=new(9,"count",E("/nested/a~1b/0","9"),null,E("/countUnit","\"count\"")) }
}));
Check(nested.ActivationBlocker(skill.SkillId)==null);
var exported = Export();
var copied = new ExactAbilityCatalog(exported);
exported.Abilities[0].Parameters.Clear(); exported.Operators[0].SkillIds[0]="mutated";
Check(copied.Skill(skill.SkillId).Parameters.ContainsKey("damage"));
var publicSkill = copied.Skill(skill.SkillId); publicSkill.Parameters.Clear();
Check(copied.Skill(skill.SkillId).Parameters.Count==1);
skill = copied.Skill(skill.SkillId); op = copied.Operator(op.OperatorId);
var unresolved = new ExactAbilityCatalog(Export(skill with { Cooldown=unknown, References=[new("CDID",null,null,"unresolved 90")] }));
Check(unresolved.ActivationBlocker(skill.SkillId)!=null);
var fake = new FakeAdapter(); var adapters = new ExactAbilityAdapters(); adapters.Register("synthetic-adapter",fake);
var clock = new FakeClock(); bool admitted = true;
var runtime = new ExactAbilityRuntime(copied,adapters,clock,_=>admitted);
var actor = new AbilityActor(100,123,runtime.Generation);
Check(!runtime.Select(actor with { PawnHandle=0 },op.OperatorId));
Check(runtime.Select(actor,op.OperatorId));
Check(!runtime.Select(actor,op.OperatorId));
Check(runtime.Activate(actor with { PawnHandle=124 },skill.SkillId)!=null);
Check(runtime.Activate(actor,skill.SkillId)==null);
Check(fake.Calls==1);
Check(runtime.Activate(actor,skill.SkillId)!=null);
clock.Elapsed=TimeSpan.FromSeconds(2); runtime.Tick(); Check(fake.Disposals==1);
Check(runtime.Activate(actor,skill.SkillId)!=null);
clock.Elapsed=TimeSpan.FromSeconds(7); Check(runtime.Activate(actor,skill.SkillId)==null);
clock.Elapsed=TimeSpan.FromSeconds(14); runtime.Tick();
Check(runtime.Activate(actor,skill.SkillId)!=null); Check(fake.Calls==2); Check(fake.Disposals==2);
runtime.Death(actor); Check(!runtime.Select(actor with { PawnHandle=125 },op.OperatorId));
Check(runtime.MapEnd()); actor=actor with { Generation=runtime.Generation,PawnHandle=125 };
Check(runtime.Select(actor,op.OperatorId)); Check(runtime.Activate(actor,skill.SkillId)==null);
runtime.Disconnect(actor); Check(fake.Disposals==3);
Check(!runtime.Select(actor,op.OperatorId)); Check(runtime.MapEnd());
Check(runtime.Activate(actor,skill.SkillId)!=null);
actor=actor with { Generation=runtime.Generation };
Check(runtime.Select(actor,op.OperatorId)); admitted=false;
Check(runtime.Activate(actor,skill.SkillId)!=null); admitted=true;
fake.Fail=true; Check(runtime.Activate(actor,skill.SkillId)!=null); fake.Fail=false;
Check(runtime.Activate(actor,skill.SkillId)==null); // Proven transactional rejection doesn't consume.
fake.DisposeFails=true; runtime.Death(actor); Check(!runtime.MapEnd());
fake.DisposeFails=false; Check(runtime.MapEnd()); // Retried ownership cleanup.
actor=actor with { Generation=runtime.Generation };
Check(runtime.Select(actor,op.OperatorId)); fake.Throw=true;
Check(runtime.Activate(actor,skill.SkillId)!=null); fake.Throw=false;
Check(runtime.Activate(actor,skill.SkillId)!=null); // Ambiguous failure quarantines.
Check(runtime.MapEnd());
actor=actor with { Generation=runtime.Generation }; Check(runtime.Select(actor,op.OperatorId));
Check(runtime.Activate(actor,skill.SkillId)==null);
clock.Elapsed=TimeSpan.FromSeconds(1); runtime.Tick();
Check(runtime.Activate(actor,skill.SkillId)!=null); // Regressed clock revokes.
Check(runtime.MapEnd());
var none = new ExactAbilityRuntime(copied,new(),new FakeClock(),_=>true);
var noneActor = new AbilityActor(20,321,none.Generation); Check(none.Select(noneActor,op.OperatorId));
Check(none.Activate(noneActor,skill.SkillId)=="adapter unavailable");
var blocked = new ExactAbilityRuntime(unresolved,adapters,new FakeClock(),_=>true);
var blockedActor = new AbilityActor(30,333,blocked.Generation); Check(blocked.Select(blockedActor,op.OperatorId));
var before = fake.Calls; Check(blocked.Activate(blockedActor,skill.SkillId)!=null); Check(fake.Calls==before);
var denied = new ExactAbilityRuntime(copied,adapters,new FakeClock(),_=>throw new Exception());
Check(!denied.Select(new(50,500,denied.Generation),op.OperatorId));

var reentrantAdapter = new FakeAdapter(); var reentrantAdapters = new ExactAbilityAdapters();
reentrantAdapters.Register("synthetic-adapter",reentrantAdapter);
var reentrantClock = new FakeClock();
var reentrant = new ExactAbilityRuntime(copied,reentrantAdapters,reentrantClock,_=>true);
var reentrantActor = new AbilityActor(60,600,reentrant.Generation);
Check(reentrant.Select(reentrantActor,op.OperatorId));
reentrantAdapter.OnActivate=()=>reentrant.Death(reentrantActor);
Check(reentrant.Activate(reentrantActor,skill.SkillId)!=null);
Check(reentrantAdapter.Disposals==1); Check(reentrant.MapEnd());
reentrantActor=reentrantActor with {Generation=reentrant.Generation};
Check(reentrant.Select(reentrantActor,op.OperatorId)); reentrantAdapter.OnActivate=null;
Check(reentrant.Activate(reentrantActor,skill.SkillId)==null);
reentrantAdapter.OnDispose=()=>reentrant.Revoke(reentrantActor);
reentrant.Death(reentrantActor); Check(reentrantAdapter.Disposals==2); Check(reentrant.MapEnd());
reentrantAdapter.OnDispose=null;
reentrantActor=reentrantActor with {Generation=reentrant.Generation};
Check(reentrant.Select(reentrantActor,op.OperatorId));
Check(reentrant.Activate(reentrantActor,skill.SkillId)==null);
string? nestedResult=null;
reentrantAdapter.OnDispose=()=>nestedResult=reentrant.Activate(reentrantActor,skill.SkillId);
reentrantClock.Elapsed=TimeSpan.FromSeconds(7);
Check(reentrant.Activate(reentrantActor,skill.SkillId)==null);
Check(nestedResult!=null); Check(reentrantAdapter.Calls==4);
reentrantAdapter.OnDispose=null; Check(reentrant.MapEnd());

Console.WriteLine($"{checks} exact ability scaffold checks passed; synthetic data only.");

sealed class FakeClock : IAbilityClock { public TimeSpan Elapsed {get;set;} }
sealed class FakeAdapter : IExactAbilityAdapter
{
    public Action? OnActivate,OnDispose; public int Calls,Disposals; public bool Fail,Throw,DisposeFails;
    public bool TryActivate(AbilityActor actor,AbilityDefinition skill,out IDisposable? effect) {
        Calls++; effect=null; if(Throw)throw new Exception("synthetic failure");
        if(Fail)return false;
        OnActivate?.Invoke(); effect=new Effect(this);return true;
    }
    private sealed class Effect(FakeAdapter owner) : IDisposable {
        public void Dispose() { if(owner.DisposeFails)throw new Exception("synthetic cleanup failure"); owner.Disposals++; owner.OnDispose?.Invoke(); }
    }
}
