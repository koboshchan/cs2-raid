using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Cs2Raid.Abilities;

var path = args.Length > 0 ? args[0] : "data/exact-abilities/r2/five-operators-exact-vyron-sourceaudit-r2.json.gz";
using var file = File.OpenRead(path);
using var gz = new GZipStream(file, CompressionMode.Decompress);
using var stream = new MemoryStream();
gz.CopyTo(stream);
var bytes = stream.ToArray();
var evidence = ArchiveAbilityEvidence.LoadR2(bytes);
int assertions = 0;
void Check(bool condition) { assertions++; if (!condition) throw new Exception("assertion " + assertions); }
void Reject(byte[] b) {
    try { ArchiveAbilityEvidence.LoadR2(b); throw new Exception("accepted tampered evidence"); }
    catch (InvalidDataException) { assertions++; }
}
Check(evidence.Count == 5);
Check(!evidence.HasOperator(1)); // D-Wolf wire identity must not be reused.
Check(evidence.OperatorByWireId(4).ChineseName == "威龙");
Check(evidence.OperatorByWireId(4).HeroRow == "88000000025");
Check(evidence.OperatorByWireId(4).ExpertRow == "10010");
Check(evidence.OperatorByWireId(4).LegacyEnumId == 3);
Check(evidence.OperatorByWireId(4).RawReportOperator.GetProperty("skills").GetArrayLength() == 8);
foreach (var id in Enumerable.Range(2, 5))
    Check(evidence.OperatorByWireId(id).MissingSkillIds.SequenceEqual(new[] { "4008010" }));
Check(evidence.SemanticCooldownSeconds == null && evidence.SemanticDurationSeconds == null &&
      evidence.SemanticUnit == null && evidence.SourceAssetSha256 == null && evidence.ExecutableAdapterKey == null);
Check(evidence.Blockers.Any(x => x.Contains("precedence")));
Check(evidence.Blockers.Any(x => x.Contains("modifiers")));
Check(evidence.Blockers.Any(x => x.Contains("transitive")));
Check(evidence.Blockers.Any(x => x.Contains("RushData")));
var stinger = evidence.OperatorByWireId(2).RawReportOperator;
var smoke = stinger.GetProperty("skills").EnumerateArray().Single(x => x.GetProperty("row").GetString() == "2008002");
Check(smoke.GetProperty("allSerializedFields").GetProperty("UseCD").GetDecimal() == 0);
Check(smoke.GetProperty("cooldownMatches")[0].GetProperty("value").GetProperty("SlotChargingData")[0].GetProperty("Time").GetDecimal() == 60);
Check(smoke.GetProperty("cooldownMatches")[0].GetProperty("value").GetProperty("SlotChargingData")[1].GetProperty("Time").GetDecimal() == 45);
Check(smoke.GetProperty("cooldownMatches")[0].GetProperty("value").GetProperty("SlotChargingData")[0].GetProperty("MaxScoreReduceCDPercent").GetDecimal() == 0.25m);
var text = Encoding.UTF8.GetString(bytes);
Reject(Encoding.UTF8.GetBytes(text.Replace("Vyron", "D-Wolf")));
Reject(Encoding.UTF8.GetBytes(text.Replace("威龙", "红狼")));
Reject(Encoding.UTF8.GetBytes(text.Replace("4008010", "4008011")));
Reject(Encoding.UTF8.GetBytes(text.Replace("60.0", "0.0")));
Reject(Encoding.UTF8.GetBytes("{}"));
var catalog = new ExactAbilityCatalog(new AbilityExport(1, [], [], []));
foreach (int wire in Enumerable.Range(2, 5))
    foreach (string skill in evidence.OperatorByWireId(wire).SkillIds) {
        Check(!evidence.TryBind(catalog, wire, skill, out var ability, out var blocker));
        Check(ability == null && blocker.Contains("no executable adapters"));
    }
Check(!evidence.TryBind(catalog, 1, "10010101", out _, out var excluded) && excluded.Contains("excluded"));
Check(!evidence.TryBind(catalog, 4, "not-a-skill", out _, out var unrelated) && unrelated.Contains("not joined"));
Console.WriteLine($"ArchiveAbilities passed {assertions} assertions; no runtime values or adapters emitted.");
