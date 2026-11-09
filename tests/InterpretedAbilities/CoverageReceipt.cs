using DeltaOps;
static class CoverageReceipt
{
 static string Calls(int id)=>id switch{
  20010101=>"CssPorts.Heal -> EnactOwned -> NativeCommittedHeal -> Pawn.Health setter + retry-only SetStateChanged",
  2002017 or 1002016=>"CssPorts.Recon -> enemy policy -> private PrintToChat names and distances (not XYZ)",
  1009001=>"CssPorts.Rush -> EnactOwned -> Pawn.Teleport velocity",
  4008002 or 30010101 or 4009002=>"CssPorts.Pulse -> session-owned velocity modifier leases; optional suppression refcount",
  20010201=>"No native revive adapter; shared heal action exists but passive is incomplete",
  40010201=>"NativeTrackedHit -> EventPlayerHurt -> private PrintToChat coordinates",
  30010201=>"NativePassiveDamage -> existing engine CTakeDamageInfo.Damage modification",
  1009004=>"NativeFallPassive -> existing pure fall CTakeDamageInfo.Damage modification; speed boost absent",
  10010301=>"No native ADS movement adapter",
  40010202=>"No native recipient-filtered footstep audio adapter",
  4008010=>"Absent source row, never executable",
  3008001=>"CssPorts.Trap -> owned zone -> serial-safe velocity leases",
  1009005=>"Blocked magnetic attachment and damage; NOT dash",
  1009003=>"Blocked compressed-air/cannon adapter",
  4008007=>"No projectile tracking/removal adapter",
  _=>"Router rejects before native mutation; direct projectile escrow foundation exists but is not enabled"
 };
 static string Approximation(int id)=>id switch{
  20010101=>"DISABLED CS2-only 25 HP tuning bounded by MaxHealth; pending network notification owned, HP never replayed or rolled back",
  2002017 or 1002016=>"CS2-only configured radius (default 1200 Source units); one-shot names and distances, no LOS, no printed XYZ, no DF sensor/projectile mechanics",
  1009001=>"400 Source-units/s horizontal velocity; current vertical velocity retained",
  4008002 or 30010101 or 4009002=>"240 Source-unit radius; 2 seconds; velocity modifier .65; Hackclaw also suppresses interpreted activations",
  30010201=>"DFPROVEN SkillID30010201 DamgeCoifficient explosion multiplier .7; native blast classification is CS2 mapping",
  1009004=>"Pure fall damage multiplier .5; buff-referenced speed boost not yet interpreted",
  40010201=>"CS2ASSUMPTION post-hit allied coordinate telemetry; squad scope not proved by extracted buffs",
  _=>"No enabled approximation; blocked/incomplete"
 };
 public static void Write(string path){
  var rows=InterpretedR2.Skills.Values.OrderBy(s=>s.Operator).ThenBy(s=>s.Id).Select(s=>new{
   s.Operator,s.Id,s.Name,s.CdId,s.Initial,s.Capacity,s.RechargeSeconds,s.Passive,
   Calls=Calls(s.Id), Approximation=Approximation(s.Id),
   Implementation=NativeImplementationEvidence.Implementation(s.Id),
   Activation=NativeImplementationEvidence.Approval(s.Operator,s.Id),
   NativeSafetyBlocker=NativeImplementationEvidence.SafetyBlocker(s.Id),
   Fidelity="Partial CS2 interpretation; remaining DF behavior is separately listed in ABILITY_SOL26_COVERAGE.md, not a blanket prohibition on offline implementation",
   TypedSourceFields=new{
       CdId=new{Raw=s.CdId,Type="DF CDID join key",Unit="identifier, not duration"},
       Initial=new{Raw=s.Initial,Type="DF initial charge count",Unit="charges"},
       Capacity=new{Raw=s.Capacity,Type="DF capacity",Unit="charges"},
       Recharge=new{Raw=s.RechargeSeconds,Type="derived exact SOL cooldown join; raw evidence retained",Unit="seconds; monotonic runtime conversion to milliseconds"}
   },
   DfBehaviorEvidence=s.Id switch{
    30010201=>"allSerializedFields.DamgeCoifficient[0]={fModifyRateExplodeDamage:0.7,fModifyRateAbilitAssault:1.0,LimitPlayMode:[]}",
    40010201=>"allSerializedFields.AddBuffs=[8011,100008]; bEnableSniperMark=true; bOverrideAimingCheck=true; TeamPlayerIcon.AssetPathName=None; trigger and squad scope unresolved",
    1009004=>"allSerializedFields.AddSkills=[1009001]; AddBuffs=[303016,8013]; UseCustomHeight=true; decoded buff effects unavailable",
    _=>"Literal allSerializedFields retained in raw r2; full runtime behavior not recovered"
   },
   Evidence="DFPROVEN PC SOL mapping and literal imported fields; all other adapter mechanics CS2ASSUMPTION unless individually cited; wiki only LOWCONFIDENCE hint",
   Binding="PC SOL; sol-binding-v1/receipt.json; raw r2 immutable",
   Values="docs/ABILITY_SOL_VALUES.Generated.md; effect magnitude tuning is separate CS2 interpretation",
   Adapter=NativeAbilityRouter.Blocker(s.Operator,s.Id)==null?"partial experimental implementation only; consult per-capability NativeAdmission; NOT full DF effect completion":"blocked/incomplete",
   Blocker=NativeAbilityRouter.Blocker(s.Operator,s.Id),
   NativeAdmission=NativeApiCapabilityEvidence.ReadOnlyRecon(s.Operator,s.Id)?"source-reviewed private telemetry implementation only; requires configured interpreted mode and normal Raid/pawn admission; NOT DF recon/nativeQA":"disabled pending per-capability API contract audit; NativeApiEvidenceGate.cs",
   ApiLedger="docs/ABILITY_EFFECT_BOUNDARIES.json; docs/ABILITY_API_EVIDENCE.json; call-site inventory docs/ABILITY_API_CALLS.json",
   SignatureEvidence="reference API self-reports pinned source SHA; bound symbols inventoried; contract review incomplete",
   EngineContractEvidence="per-boundary partial; unsupported damage construction/expiry/competing scalar ownership fail closed",
   NativeAcceptance=false
  }).ToArray();
  if(rows.Any(r=>r.Blocker==null && r.Calls.StartsWith("Router rejects")))throw new Exception("Enabled path missing literal call mapping");
  if(rows.Length!=26||rows.Any(s=>s.Id==4008010))throw new Exception("SOL coverage receipt mismatch");
  File.WriteAllText(path,System.Text.Json.JsonSerializer.Serialize(rows,new System.Text.Json.JsonSerializerOptions{WriteIndented=true}));
 }
}
