"""Offline evidence audit. No game execution, network, source mutation or catalog export."""
import gzip
import hashlib
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
DATA = ROOT / "data/exact-abilities"
ALIASES = {
    "Stinger": ("蜂医", "88000000027", "20003", 2, 1),
    "Luna": ("露娜", "88000000028", "40005", 3, 2),
    "Vyron": ("威龙", "88000000025", "10010", 4, 3),
    "Hackclaw": ("骇爪", "88000000026", "40010", 5, 4),
    "Shepherd": ("牧羊人", "88000000029", "30008", 6, 5),
}
INDEX_SHA = "9020e5572bf89503f18f796d90650885ab00408ea8cde8eeaf9177f3f367bd59"
BLOCKERS = [
    "source asset SHA256 absent for relevant hero/expert/group/skill/cooldown packages",
    "numeric semantic units not independently proven",
    "mode and package override precedence unresolved",
    "dynamic cooldown modifiers and event conditions unresolved",
    "transitive effects/projectiles/buffs/nested skills/Blueprint logic unresolved",
    "skill row 4008010 missing",
    "no executable ability adapters",
    "RushData typed value unresolved; relevance to five operators not proven",
]
def sha(b):
    return hashlib.sha256(b).hexdigest()
def read(p):
    b = p.read_bytes()
    return gzip.decompress(b) if p.suffix == ".gz" else b
def load(p):
    return json.loads(read(p))
def ids(fields):
    result = set()
    for k, v in fields.items():
        if k.endswith("SkillID") and k != "SkillGroupID":
            for n in (v if isinstance(v, list) else [v]):
                if isinstance(n, int) and n != 0:
                    result.add(n)
    return result
def validate(d):
    assert d["scope"] == list(ALIASES), "scope mismatch"
    assert len(d["operators"]) == 5
    seen = set()
    for o in d["operators"]:
        label = o["requestedEnglishLabel"]
        assert label in ALIASES and label not in seen, "excluded/duplicate operator"
        seen.add(label)
        cn, hero, expert, _, _ = ALIASES[label]
        assert (o["heroName"]["SourceString"], o["heroRow"], str(o["ExpertID"]),
                o["expertRow"], o["groupRow"]) == (cn, hero, expert, expert, expert), "identity mismatch"
        assert str(o["allExpertFields"]["ExpertID"]) == expert
        assert str(o["allGroupFields"]["SkillGroupID"]) == expert
        wanted = ids(o["allExpertFields"]) | ids(o["allGroupFields"])
        assert wanted == set(o["skillIDs"]), "direct join mismatch"
        rows = [int(s["row"]) for s in o["skills"]]
        assert len(rows) == len(set(rows))
        assert wanted - set(rows) == set(o["missingSkillIDs"]) == {4008010}
        assert not set(rows) - wanted
        for s in o["skills"]:
            assert int(s["row"]) == s["allSerializedFields"]["SkillID"]
            assert s["CDID"] == s["allSerializedFields"]["CDID"]
            for m in s["cooldownMatches"]:
                assert m["field"] == "CdID" and m["value"]["CdID"] == s["CDID"]
                assert s["CDID"] != 0
    return d

def table_rows(full):
    result = {}
    for section in ("tables", "cooldownTables", "supplementaryOperatorAndBaseSkillTables"):
        for ti, t in enumerate(full[section]):
            for ei, exp in enumerate(t.get("decoded", [])):
                for row, v in exp.get("value", {}).get("Rows", {}).items():
                    key = (t["package"], row)
                    ptr = f"/{section}/{ti}/decoded/{ei}/value/Rows/{row}"
                    assert key not in result
                    result[key] = (v, ptr)
    return result

def main():
    if not __debug__:
        raise RuntimeError("Run importer without -O; evidence assertions are mandatory")
    index_bytes = read(DATA / "r2/index.json")
    assert sha(index_bytes) == INDEX_SHA, "immutable index hash mismatch"
    index = json.loads(index_bytes)
    sources = []
    for item in index["downloads"]:
        # Filenames are taken from the pinned index; importer never fetches them.
        p = DATA / "r2" / (item["url"].rsplit("/", 1)[1] + ".gz")
        b = read(p)
        assert len(b) == item["bytes"] and sha(b) == item["sha256"], "artifact hash mismatch"
        sources.append(dict(item, storedAs=str(p.relative_to(ROOT))))
    r1 = validate(load(DATA / "r1/five-operators-exact.json"))
    r2 = validate(load(DATA / "r2/five-operators-exact-vyron-sourceaudit-r2.json.gz"))
    # Compare every operator field, skill raw value and CDID match, not summaries.
    assert r1["operators"] == r2["operators"], "gameplay/operator evidence changed"
    fullpath = DATA / "r2/deltaops-exact-values-vyron-sourceaudit-r2.json.gz"
    full = load(fullpath)
    allrows = table_rows(full)
    extracted = {}
    operators = []
    def take(package, row, expected=None):
        row = str(row)
        value, pointer = allrows[(package, row)]
        if expected is not None:
            assert value == expected, "full export raw row mismatch"
        key = package + "#" + row
        if key not in extracted:
            # This hash describes normalized decoded JSON, NEVER the archive asset.
            raw = json.dumps(value, ensure_ascii=False, separators=(",", ":"))
            extracted[key] = dict(package=package, row=row, sourcePointer=pointer,
                sourceReportSha256=sha(read(fullpath)), assetSha256=None,
                rawJson=raw, normalizedRowSha256=sha(raw.encode()),
                numericSemanticValues=None, unitEvidence=None)
        return key
    for o in r2["operators"]:
        hero = take(o["heroPackage"], o["heroRow"])
        h = allrows[(o["heroPackage"], o["heroRow"])][0]
        assert h["Name"] == o["heroName"] and h["ExpertID"] == o["ExpertID"]
        assert str(h["HeroId"]) == o["heroRow"]
        refs = [hero, take(o["expertPackage"], o["expertRow"], o["allExpertFields"]),
                take(o["groupPackage"], o["groupRow"], o["allGroupFields"])]
        for s in o["skills"]:
            refs.append(take(s["package"], s["row"], s["allSerializedFields"]))
            for m in s["cooldownMatches"]:
                refs.append(take(m["package"], m["row"], m["value"]))
        cn, hr, ex, wire, old_enum = ALIASES[o["requestedEnglishLabel"]]
        operators.append(dict(label=o["requestedEnglishLabel"], archiveName=cn,
            heroRow=hr, expertRow=ex, stableWireId=wire, legacyEnumId=old_enum,
            sourceRows=refs, skillIDs=o["skillIDs"], missingSkillIDs=o["missingSkillIDs"],
            activationAllowed=False, blockers=BLOCKERS))
    # Search all metadata keys for claimed source package hashes. The lone Rush
    # container hash is not a hash of any relevant package.
    hash_fields = []
    def scan(v, p=""):
        if isinstance(v, dict):
            for k, x in v.items():
                q = p + "/" + k.replace("~", "~0").replace("/", "~1")
                if "sha256" in k.lower(): hash_fields.append(dict(pointer=q, value=x))
                scan(x, q)
        elif isinstance(v, list):
            for i, x in enumerate(v): scan(x, p + "/" + str(i))
    scan(full)
    manifest = dict(schema="archive-evidence-only-v1", indexSha256=INDEX_SHA,
        sources=sources, r1Sources=[dict(path=str(p.relative_to(ROOT)), sha256=sha(read(p)))
                                  for p in sorted((DATA/"r1").iterdir())],
        semanticDiff=dict(operatorAndGameplayValuesUnchanged=True,
                          compared="entire operators arrays including all raw fields and cooldown matches"),
        sourceHashFields=hash_fields, operators=operators, rows=list(extracted.values()),
        blockers=BLOCKERS, executableAdapters=[], catalogExport=None,
        rush=dict(relevance="unproven; not imported as actionable parameter",
                  evidence=full["boundedRushRecovery"]))
    (DATA / "normalized-evidence.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2)+"\n")
    print(json.dumps(dict(operators=len(operators), rows=len(extracted),
        distinctSkills=len(set(n for o in operators for n in o["skillIDs"])),
        skillInstances=sum(len(o["skillIDs"])-len(o["missingSkillIDs"]) for o in operators),
        activationAllowed=False, hashFields=hash_fields), ensure_ascii=False))
if __name__ == "__main__":
    main()
