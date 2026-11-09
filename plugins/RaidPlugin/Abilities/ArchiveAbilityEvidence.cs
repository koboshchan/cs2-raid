using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text.Json;

namespace Cs2Raid.Abilities;

// A separate evidence boundary, NOT schema-1 gameplay data. Report hashes cannot
// satisfy SourceRow.AssetSha256. No default timings, units or adapters are emitted.
public sealed record ArchiveOperatorEvidence(
    string Label, int StableWireId, int LegacyEnumId, string ChineseName,
    string HeroRow, string ExpertRow, JsonElement RawReportOperator)
{
    public string[] SkillIds => RawReportOperator.GetProperty("skillIDs")
        .EnumerateArray().Select(x => x.GetInt64().ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray();
    public string[] MissingSkillIds => RawReportOperator.GetProperty("missingSkillIDs")
        .EnumerateArray().Select(x => x.GetInt64().ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray();
}

public sealed class ArchiveAbilityEvidence
{
    public const string R2SourceSha256 = "c80841bee9bf0d3d93452329921be736a8eb237fe7a495584159eac20a38d246";
    public const string R2IndexSha256 = "9020e5572bf89503f18f796d90650885ab00408ea8cde8eeaf9177f3f367bd59";
    private static readonly (string Label, string Chinese, string Hero, string Expert, int Wire, int Old)[] Aliases = [
        ("Stinger", "蜂医", "88000000027", "20003", 2, 1),
        ("Luna", "露娜", "88000000028", "40005", 3, 2),
        ("Vyron", "威龙", "88000000025", "10010", 4, 3),
        ("Hackclaw", "骇爪", "88000000026", "40010", 5, 4),
        ("Shepherd", "牧羊人", "88000000029", "30008", 6, 5)
    ];
    private readonly FrozenDictionary<int, ArchiveOperatorEvidence> operators;
    private readonly JsonElement raw;
    public JsonElement RawReport => raw;
    public decimal? SemanticCooldownSeconds => null;
    public decimal? SemanticDurationSeconds => null;
    public string? SemanticUnit => null;
    public string? SourceAssetSha256 => null;
    public string? ExecutableAdapterKey => null;
    public string[] Blockers => [
        "missing genuine source asset hashes",
        "numeric units not independently proven",
        "mode/package precedence unresolved",
        "dynamic cooldown modifiers unresolved",
        "transitive effects/projectiles/buffs/nested skills/Blueprint logic unresolved",
        "missing skill row 4008010",
        "RushData typing unresolved; relevance to these five not proven",
        "no executable adapters"
    ];
    public ArchiveOperatorEvidence OperatorByWireId(int id) => operators[id];
    public bool HasOperator(int id) => operators.ContainsKey(id);
    public int Count => operators.Count;

    private ArchiveAbilityEvidence(JsonElement document)
    {
        raw = document.Clone();
        var source = raw.GetProperty("operators");
        Require(source.GetArrayLength() == Aliases.Length, "Expected exact five operators");
        var scope = raw.GetProperty("scope").EnumerateArray().Select(x => x.GetString()).ToArray();
        Require(scope.SequenceEqual(Aliases.Select(x => x.Label)), "Scope differs from audited aliases");
        var result = new Dictionary<int, ArchiveOperatorEvidence>();
        foreach (var op in source.EnumerateArray()) {
            var label = op.GetProperty("requestedEnglishLabel").GetString();
            var matches = Aliases.Where(x => x.Label == label).ToArray();
            Require(matches.Length == 1, "Unaudited/excluded alias");
            var a = matches[0];
            Require(op.GetProperty("heroName").GetProperty("SourceString").GetString() == a.Chinese &&
                op.GetProperty("heroRow").GetString() == a.Hero &&
                op.GetProperty("expertRow").GetString() == a.Expert &&
                op.GetProperty("groupRow").GetString() == a.Expert &&
                op.GetProperty("ExpertID").GetInt64().ToString() == a.Expert,
                "Audited alias/archive identity mismatch");
            var skills = op.GetProperty("skills").EnumerateArray().ToArray();
            var present = new HashSet<long>();
            foreach (var skill in skills) {
                var fields = skill.GetProperty("allSerializedFields");
                var id = fields.GetProperty("SkillID").GetInt64();
                Require(skill.GetProperty("row").GetString() == id.ToString() && present.Add(id),
                    "Invalid/duplicate skill identity");
                var cdid = fields.GetProperty("CDID").GetInt64();
                Require(skill.GetProperty("CDID").GetInt64() == cdid, "CDID mismatch");
                foreach (var match in skill.GetProperty("cooldownMatches").EnumerateArray())
                    Require(cdid != 0 && match.GetProperty("field").GetString() == "CdID" &&
                        match.GetProperty("value").GetProperty("CdID").GetInt64() == cdid, "Invalid CDID join");
            }
            var all = op.GetProperty("skillIDs").EnumerateArray().Select(x => x.GetInt64()).ToArray();
            var missing = op.GetProperty("missingSkillIDs").EnumerateArray().Select(x => x.GetInt64()).ToArray();
            Require(all.Distinct().Count() == all.Length && missing.SequenceEqual(new long[] { 4008010 }) &&
                all.Except(present).Order().SequenceEqual(missing.Order()) &&
                !present.Except(all).Any(), "Missing or unexpected skill rows");
            Require(result.TryAdd(a.Wire, new(a.Label, a.Wire, a.Old, a.Chinese, a.Hero, a.Expert, op.Clone())),
                "Duplicate operator");
        }
        operators = result.ToFrozenDictionary();
    }

    // Caller supplies decompressed immutable artifact bytes, not reserialized JSON.
    // The pinned digest is also in the independently retained canonical index.
    public static ArchiveAbilityEvidence LoadR2(byte[] sourceBytes)
    {
        ArgumentNullException.ThrowIfNull(sourceBytes);
        var immutable = (byte[])sourceBytes.Clone();
        var hash = Convert.ToHexString(SHA256.HashData(immutable)).ToLowerInvariant();
        Require(hash == R2SourceSha256, "R2 immutable report digest mismatch");
        using var doc = JsonDocument.Parse(immutable);
        return new(doc.RootElement);
    }

    // No report-only values can cross into gameplay, even if a caller has loaded
    // an unrelated or synthetic catalog with the same skill ID.
    public bool TryBind(ExactAbilityCatalog catalog, int stableWireId, string skillId,
        out AbilityDefinition? ability, out string blocker)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        ability = null;
        if (!operators.TryGetValue(stableWireId, out var op)) {
            blocker = "operator excluded or unknown"; return false;
        }
        if (!op.SkillIds.Contains(skillId, StringComparer.Ordinal)) {
            blocker = "skill not joined to operator"; return false;
        }
        blocker = string.Join("; ", Blockers);
        return false;
    }
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidDataException(message);
    }
}
