This is an offline evidence import, not an ability implementation. It never emits a schema-1 AbilityExport, never assigns guessed units and never supplies executable adapters. ArchiveAbilityEvidence.TryBind always rejects report-only data, even with another catalog present.

Run from the worktree root with Python 3 and .NET 10.

```sh
python3 tools/exact-abilities/import_evidence.py
python3 -m unittest discover -s tools/exact-abilities -p 'test_*.py' -v
/home/kit/extract-zone-sdk/dotnet run --project tests/ArchiveAbilities/ArchiveAbilities.csproj
```

The original local r1 files remain separate. All six canonical r2 index downloads are retained as deterministic gzip snapshots of the original bytes. Decompressed length and SHA256 are checked against the pinned full index. The index SHA256 is 9020e5572bf89503f18f796d90650885ab00408ea8cde8eeaf9177f3f367bd59. The r2 five-operator JSON SHA256 is c80841bee9bf0d3d93452329921be736a8eb237fe7a495584159eac20a38d246. These identify reports, not game assets.

The normalized evidence retains 58 distinct full decoded source rows with original report pointers, report hashes and separate normalized-row hashes. It covers five full HeroData rows, five expert rows, five group rows, 26 distinct present skill IDs and their CDID matches. There are 27 distinct referenced skill IDs, 33 present operator-skill instances and five references to missing row 4008010. Full raw mode arrays, modifiers and nested serialized values remain intact. Hero/skill identities and every included expert/group/skill/cooldown row are independently compared with the broader report.

Both revisions have identical entire operator arrays. All original full-report sections are unchanged in r2, including tables, property proof, cooldown matches, supplementary operator/base tables and bounded Rush recovery. Additions are audit metadata, not changed gameplay values.

Official CN/EN identity evidence is in data/exact-abilities/identity-audit.json. It establishes 威龙 as Vyron, never 红狼/D-Wolf. Web names do not establish archive IDs. The actual HeroData row 88000000025 and ExpertID 10010 are separately raw-field verified, with eight present config rows. Stable wire IDs remain Stinger 2, Luna 3, Vyron 4, Hackclaw 5 and Shepherd 6; legacy enum IDs remain 1 through 5.

No relevant source asset SHA256 is supplied. A recursive metadata scan finds only boundedRushRecovery.rawContainerSHA256, which belongs to RushData.uexp and must not be reused for unrelated assets. Missing hashes prevent construction of honest SourceRow records for the current strict catalog. Numeric semantic values and units therefore remain null rather than inventing provenance.

Only row 4008010 is missing from the direct join. That is not the only implementation unknown. Runtime mode/package precedence, dynamic cooldown modifiers, transitive effects/projectiles/buffs/nested skills/Blueprint behavior and units remain unresolved. UseCD is never substituted for cooldown. Rush has exact 28-byte evidence and a partial interpretation, but unknown struct typing and no proven five-operator join. It is retained as unresolved provenance, not an actionable ability value.

No live code path, hooks, server changes, restarts, pushes or commits are performed here. Existing catalog/runtime/roster files belong to the parent and are not edited.
