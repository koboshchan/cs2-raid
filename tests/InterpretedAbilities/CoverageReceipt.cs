using DeltaOps;
static class CoverageReceipt
{
 public static void Write(string path){
  var rows=InterpretedR2.Skills.Values.OrderBy(s=>s.Operator).ThenBy(s=>s.Id).Select(s=>new{
   s.Operator,s.Id,s.Name,s.CdId,s.Initial,s.Capacity,s.RechargeSeconds,s.Passive,
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
