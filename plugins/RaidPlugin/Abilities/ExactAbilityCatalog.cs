using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cs2Raid.Abilities;

// Exported raw JSON is retained verbatim. Hashes identify decoded rows, not gameplay truth.
public sealed record SourceRow(string Table, string Row, string AssetSha256, string RawJson, string RowSha256);
public sealed record FieldEvidence(string Table, string Row, string JsonPointer, string RawJson);
public sealed record ExactNumber(decimal? Value, string? Unit, FieldEvidence? Evidence, string? Unresolved, FieldEvidence? UnitEvidence = null);
public sealed record ExactChoice(string? Value, FieldEvidence? Evidence, string? Unresolved);
public sealed record RowReference(string Name, string? Table, string? Row, string? Unresolved);
public sealed record AbilityDefinition(
    string SkillId, FieldEvidence Identity, string? AdapterKey,
    ExactNumber Cooldown, ExactNumber Duration, ExactNumber Charges,
    ExactChoice ResourceMode, ExactChoice CooldownStart,
    Dictionary<string, ExactNumber> Parameters, RowReference[] References);
public sealed record OperatorDefinition(string OperatorId, FieldEvidence Identity, string[] SkillIds);
public sealed record AbilityExport(int SchemaVersion, SourceRow[] Rows,
    OperatorDefinition[] Operators, AbilityDefinition[] Abilities);

public sealed class ExactAbilityCatalog
{
    private readonly FrozenDictionary<string, SourceRow> rows;
    private readonly FrozenDictionary<string, AbilityDefinition> abilities;
    private readonly FrozenDictionary<string, OperatorDefinition> operators;
    // Return copies, so callers cannot mutate a validated definition.
    public AbilityDefinition Skill(string id) => Copy(abilities[id]);
    public OperatorDefinition Operator(string id) => operators[id] with { SkillIds = [.. operators[id].SkillIds] };
    public bool HasSkill(string id) => abilities.ContainsKey(id);
    public bool HasOperator(string id) => operators.ContainsKey(id);
    private static string Key(string table, string row) => table + "\0" + row;
    public static string Sha256(string text) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text))).ToLowerInvariant();
    public static ExactAbilityCatalog Load(string json)
    {
        using var document = JsonDocument.Parse(json);
        RejectDuplicateProperties(document.RootElement);
        var data = JsonSerializer.Deserialize<AbilityExport>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = false, UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
        }) ?? throw new InvalidDataException("Null ability export");
        return new(data);
    }
    private static void RejectDuplicateProperties(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Object) {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in value.EnumerateObject()) {
                Require(seen.Add(p.Name), "Duplicate JSON property");
                RejectDuplicateProperties(p.Value);
            }
        } else if (value.ValueKind == JsonValueKind.Array)
            foreach (var item in value.EnumerateArray()) RejectDuplicateProperties(item);
    }
    public ExactAbilityCatalog(AbilityExport data)
    {
        Require(data.SchemaVersion == 1, "Unsupported export schema");
        Require(data.Rows != null && data.Operators != null && data.Abilities != null, "Missing export sections");
        var r = new Dictionary<string, SourceRow>(StringComparer.Ordinal);
        foreach (var row in data.Rows!) {
            Require(row != null && Text(row.Table) && Text(row.Row) && Hash(row.AssetSha256) && Hash(row.RowSha256), "Missing row provenance");
            using var parsed = JsonDocument.Parse(row!.RawJson);
            Require(parsed.RootElement.ValueKind == JsonValueKind.Object, "Row must be an object");
            RejectDuplicateProperties(parsed.RootElement);
            Require(Sha256(row.RawJson).Equals(row.RowSha256, StringComparison.OrdinalIgnoreCase), "Row hash mismatch");
            Require(r.TryAdd(Key(row.Table, row.Row), row), "Duplicate source row");
        }
        Require(r.Values.GroupBy(x => x.Table).All(g => g.Select(x => x.AssetSha256.ToLowerInvariant()).Distinct().Count() == 1),
            "Conflicting asset hashes within table");
        rows = r.ToFrozenDictionary(StringComparer.Ordinal);
        var a = new Dictionary<string, AbilityDefinition>(StringComparer.Ordinal);
        foreach (var skill in data.Abilities!) {
            Require(skill != null && Text(skill.SkillId) && skill.Parameters != null && skill.References != null, "Missing skill fields");
            Evidence(skill!.Identity);
            Require(IdentityValue(skill.Identity) == skill.SkillId, "Skill identity differs from source");
            Number(skill.Cooldown); Number(skill.Duration); Number(skill.Charges);
            Choice(skill.ResourceMode); Choice(skill.CooldownStart);
            foreach (var p in skill.Parameters!) { Require(Text(p.Key), "Empty parameter"); Number(p.Value); }
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var reference in skill.References!) {
                Require(reference != null && Text(reference.Name) && names.Add(reference.Name), "Duplicate/empty reference");
                if (Text(reference!.Unresolved))
                    Require(reference.Table == null && reference.Row == null, "Unresolved reference cannot claim a target");
                else Require(Text(reference.Table) && Text(reference.Row) && rows.ContainsKey(Key(reference.Table!, reference.Row!)), "Missing reference target");
            }
            Require(a.TryAdd(skill.SkillId, Copy(skill)), "Duplicate skill ID");
        }
        abilities = a.ToFrozenDictionary(StringComparer.Ordinal);
        var o = new Dictionary<string, OperatorDefinition>(StringComparer.Ordinal);
        foreach (var op in data.Operators!) {
            Require(op != null && Text(op.OperatorId) && op.SkillIds != null && op.SkillIds.Length > 0, "Missing operator fields");
            Evidence(op!.Identity);
            Require(IdentityValue(op.Identity) == op.OperatorId, "Operator identity differs from source");
            Require(op.SkillIds!.Distinct(StringComparer.Ordinal).Count() == op.SkillIds!.Length &&
                op.SkillIds.All(abilities.ContainsKey), "Duplicate/missing operator skill");
            Require(o.TryAdd(op.OperatorId, op with { SkillIds = [.. op.SkillIds] }), "Duplicate operator ID");
        }
        operators = o.ToFrozenDictionary(StringComparer.Ordinal);
    }
    private static AbilityDefinition Copy(AbilityDefinition skill) => skill with {
        Parameters = new(skill.Parameters, StringComparer.Ordinal), References = [.. skill.References]
    };
    private static string IdentityValue(FieldEvidence evidence)
    {
        using var doc = JsonDocument.Parse(evidence.RawJson);
        return doc.RootElement.ValueKind == JsonValueKind.String ? doc.RootElement.GetString()! : doc.RootElement.GetRawText();
    }
    private void Evidence(FieldEvidence? evidence)
    {
        Require(evidence != null && Text(evidence.Table) && Text(evidence.Row) && evidence.JsonPointer != null, "Missing field provenance");
        Require(rows.TryGetValue(Key(evidence!.Table, evidence.Row), out var source), "Evidence row missing");
        using var doc = JsonDocument.Parse(source!.RawJson);
        var current = doc.RootElement;
        if (evidence.JsonPointer != "") {
            Require(evidence.JsonPointer!.StartsWith('/'), "Invalid JSON pointer");
            foreach (var token in evidence.JsonPointer[1..].Split('/')) {
                Require(!System.Text.RegularExpressions.Regex.IsMatch(token, "~(?![01])"), "Invalid JSON pointer escape");
                var key = token.Replace("~1", "/").Replace("~0", "~");
                if (current.ValueKind == JsonValueKind.Object) {
                    Require(current.TryGetProperty(key, out var child), "Evidence field missing"); current = child;
                } else if (current.ValueKind == JsonValueKind.Array) {
                    Require(int.TryParse(key, out var index) && index >= 0 && index < current.GetArrayLength(), "Evidence array index missing");
                    current = current[index];
                } else throw new InvalidDataException("Evidence pointer traverses scalar");
            }
        }
        Require(current.GetRawText() == evidence.RawJson, "Evidence raw value mismatch");
    }
    private void Number(ExactNumber? number)
    {
        Require(number != null, "Number requires explicit unresolved record");
        if (number!.Value == null) {
            Require(Text(number.Unresolved) && number.Unit == null && number.Evidence == null && number.UnitEvidence == null, "Unresolved number must not claim value/unit/evidence"); return;
        }
        Require(number.Unresolved == null && Text(number.Unit), "Resolved number requires verified unit");
        Evidence(number.Evidence);
        Evidence(number.UnitEvidence);
        Require(IdentityValue(number.UnitEvidence!) == number.Unit, "Unit differs from source");
        using var doc = JsonDocument.Parse(number.Evidence!.RawJson);
        Require(doc.RootElement.ValueKind == JsonValueKind.Number && doc.RootElement.TryGetDecimal(out var value) && value == number.Value,
            "Numeric value differs from exact source; conversions require a future audited mapping");
    }
    private void Choice(ExactChoice? choice)
    {
        Require(choice != null, "Choice requires explicit unresolved record");
        if (choice!.Value == null) {
            Require(Text(choice.Unresolved) && choice.Evidence == null, "Unresolved choice must not claim evidence"); return;
        }
        Require(choice.Unresolved == null && Text(choice.Value), "Resolved choice invalid");
        Evidence(choice.Evidence);
        Require(IdentityValue(choice.Evidence!) == choice.Value, "Choice differs from source");
    }
    public string? ActivationBlocker(string skillId)
    {
        if (!abilities.TryGetValue(skillId, out var skill)) return "unknown skill";
        if (!Text(skill.AdapterKey)) return "adapter mapping unresolved";
        if (skill.References.Any(r => r.Unresolved != null)) return "cross-reference unresolved";
        if (skill.Parameters.Values.Any(n => n.Value == null)) return "parameter unresolved";
        if (skill.Cooldown.Value == null || skill.Duration.Value == null ||
            skill.Cooldown.Unit != "seconds" || skill.Duration.Unit != "seconds" ||
            skill.Cooldown.Value < 0 || skill.Duration.Value < 0 ||
            skill.Cooldown.Value > (decimal)TimeSpan.MaxValue.TotalSeconds ||
            skill.Duration.Value > (decimal)TimeSpan.MaxValue.TotalSeconds ||
            !ExactTicks(skill.Cooldown.Value.Value) || !ExactTicks(skill.Duration.Value.Value)) return "timing unresolved or unsupported";
        if (skill.CooldownStart.Value != "activation") return "cooldown start unresolved or unsupported";
        if (skill.ResourceMode.Value == "unlimited") {
            if (skill.Charges.Value != null) return "unlimited mode conflicts with charges";
        } else if (skill.ResourceMode.Value == "finite") {
            if (skill.Charges.Value == null || skill.Charges.Unit != "count" || skill.Charges.Value < 1 ||
                skill.Charges.Value > int.MaxValue || decimal.Truncate(skill.Charges.Value.Value) != skill.Charges.Value)
                return "charges unresolved or unsupported";
        } else return "resource policy unresolved or unsupported";
        return null;
    }
    private static bool ExactTicks(decimal seconds) => seconds <= TimeSpan.MaxValue.Ticks / (decimal)TimeSpan.TicksPerSecond &&
        decimal.Truncate(seconds * TimeSpan.TicksPerSecond) == seconds * TimeSpan.TicksPerSecond;
    private static bool Text(string? text) => !string.IsNullOrWhiteSpace(text);
    private static bool Hash(string? hash) => hash != null && hash.Length == 64 && hash.All(Uri.IsHexDigit);
    private static void Require(bool ok, string message) { if (!ok) throw new InvalidDataException(message); }
}
