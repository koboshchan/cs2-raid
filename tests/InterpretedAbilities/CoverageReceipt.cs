using DeltaOps;
static class CoverageReceipt
{
 static string Calls(int id)=>id switch{
  2001010=>"CssPorts.Heal -> EnactOwned -> Pawn.Health setter + SetStateChanged",
  4008002 or 4008001 or 4001010=>"CssPorts.Recon -> enemy policy -> private PrintToChat coordinates",
  1009005=>"CssPorts.Rush -> EnactOwned -> Pawn.Teleport velocity",
  3001001 or 3001002 or 4001001=>"CssPorts.Pulse -> session-owned velocity modifier leases; optional suppression refcount",
  20010201=>"No native revive adapter; shared heal action exists but passive is incomplete",
  40010201=>"NativeTrackedHit -> EventPlayerHurt -> private PrintToChat coordinates",
  30010201=>"NativePassiveDamage -> existing engine CTakeDamageInfo.Damage modification",
  1009004=>"NativeFallPassive -> existing pure fall CTakeDamageInfo.Damage modification; speed boost absent",
  10010301=>"No native ADS movement adapter",
  40010202=>"No native recipient-filtered footstep audio adapter",
  4008010=>"Absent source row, never executable",
  4008003=>"No projectile tracking/removal adapter",
  _=>"Router rejects before native mutation; direct projectile escrow foundation exists but is not enabled"
 };
 static string Approximation(int id)=>id switch{
  2001010=>"25 HP bounded by MaxHealth, instant; committed health not rolled back",
  4008002 or 4008001 or 4001010=>"1200 Source-unit radius; one-shot coordinate telemetry, no DF-duration reveal",
  1009005=>"400 Source-units/s horizontal velocity; current vertical velocity retained",
  3001001 or 3001002 or 4001001=>"240 Source-unit radius; 2 seconds; velocity modifier .65; Hackclaw also suppresses interpreted activations",
  30010201=>"DFPROVEN SkillID30010201 DamgeCoifficient explosion multiplier .7; native blast classification is CS2 mapping",
  1009004=>"Pure fall damage multiplier .5; buff-referenced speed boost not yet interpreted",
  40010201=>"CS2ASSUMPTION post-hit allied coordinate telemetry; squad scope not proved by extracted buffs",
  _=>"No enabled approximation; blocked/incomplete"
 };
 public static void Write(string path){
  var rows=InterpretedR2.Skills.Values.OrderBy(s=>s.Operator).ThenBy(s=>s.Id).Select(s=>new{
   s.Operator,s.Id,s.Name,s.CdId,s.Initial,s.Capacity,s.RechargeSeconds,s.Passive,
   Calls=Calls(s.Id), Approximation=Approximation(s.Id),
   Evidence="DFPROVEN PC SOL mapping and literal imported fields; all other adapter mechanics CS2ASSUMPTION unless individually cited; wiki only LOWCONFIDENCE hint",
   Binding="PC SOL; sol-binding-v1/receipt.json; raw r2 immutable",
   Values="docs/ABILITY_SOL_VALUES.Generated.md; effect magnitude tuning is separate CS2 interpretation",
   Adapter=NativeAbilityRouter.Blocker(s.Operator,s.Id)==null?"offline interpretation implemented":"blocked/incomplete",
   Blocker=NativeAbilityRouter.Blocker(s.Operator,s.Id),
   NativeAcceptance=false
  }).ToArray();
  if(rows.Length!=26||rows.Any(s=>s.Id==4008010))throw new Exception("SOL coverage receipt mismatch");
  File.WriteAllText(path,System.Text.Json.JsonSerializer.Serialize(rows,new System.Text.Json.JsonSerializerOptions{WriteIndented=true}));
 }
}
