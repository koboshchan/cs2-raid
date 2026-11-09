# SOL binding correction v1

This supersedes earlier executable coverage counts and MP/BombDefusal interpretations in ABILITY_INTERPRETATIONS.md. Raw reports remain unchanged evidence, not executable mode bindings. Stable operator enum and wire IDs are unchanged.

Read the complete desktop findings and all 270 lines of SkillHelperTool.lua, retained beside receipt.json. Hero ExpertID joins and SOL prefix dispatch follow lines 10-17, 221-247 and 265-269. SkillDataConfig lookup compares the SkillID field, lines 179-186; it never infers SkillID from row labels. Blasting group lookup lines 249-262 is not used. WorldGameMode maps to SOL at lines 138-151.

## Per-operator receipt

### Stinger
Hero 88000000027 -> ExpertID/expert row 20003.

- SOLUltimateSkillID = 20010101
- SOLSupportSkillID = 2008002
- SOLActiveSkillID = 2008005
- SOLPassiveSkillID = 20010201
- SOLCommonPassiveSkillID = 0
- SOLCommonSkillID = [4008007, 4008010]
- SOLUltimateSkillID SkillID 20010101 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 20010101
- SOLSupportSkillID SkillID 2008002 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 2008002
- SOLActiveSkillID SkillID 2008005 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 2008005
- SOLPassiveSkillID SkillID 20010201 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 20010201
- SOLCommonSkillID SkillID 4008007 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008007
- SOLCommonSkillID SkillID 4008010 -> MISSING, no fallback
Old union-only entries excluded from executable SOL: [20010215, 30010203].

### Luna
Hero 88000000028 -> ExpertID/expert row 40005.

- SOLUltimateSkillID = 2002017
- SOLSupportSkillID = 4008002
- SOLActiveSkillID = 3008002
- SOLPassiveSkillID = 40010201
- SOLCommonPassiveSkillID = 0
- SOLCommonSkillID = [4008007, 4008010]
- SOLUltimateSkillID SkillID 2002017 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 2002017
- SOLSupportSkillID SkillID 4008002 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008002
- SOLActiveSkillID SkillID 3008002 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 3008002
- SOLPassiveSkillID SkillID 40010201 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 40010201
- SOLCommonSkillID SkillID 4008007 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008007
- SOLCommonSkillID SkillID 4008010 -> MISSING, no fallback
Old union-only entries excluded from executable SOL: [30010203].

### Vyron
Hero 88000000025 -> ExpertID/expert row 10010.

- SOLUltimateSkillID = 1009003
- SOLSupportSkillID = 1009001
- SOLActiveSkillID = 1009005
- SOLPassiveSkillID = 1009004
- SOLCommonPassiveSkillID = 10010301
- SOLCommonSkillID = [4008007, 4008010]
- SOLUltimateSkillID SkillID 1009003 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 1009003
- SOLSupportSkillID SkillID 1009001 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 1009001
- SOLActiveSkillID SkillID 1009005 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 1009005
- SOLPassiveSkillID SkillID 1009004 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 1009004
- SOLCommonPassiveSkillID SkillID 10010301 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 10010301
- SOLCommonSkillID SkillID 4008007 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008007
- SOLCommonSkillID SkillID 4008010 -> MISSING, no fallback
Old union-only entries excluded from executable SOL: [1009002, 30010203].

### Hackclaw
Hero 88000000026 -> ExpertID/expert row 40010.

- SOLUltimateSkillID = 1002016
- SOLSupportSkillID = 4009003
- SOLActiveSkillID = 4009002
- SOLPassiveSkillID = 40010202
- SOLCommonPassiveSkillID = 0
- SOLCommonSkillID = [4008007, 4008010]
- SOLUltimateSkillID SkillID 1002016 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 1002016
- SOLSupportSkillID SkillID 4009003 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4009003
- SOLActiveSkillID SkillID 4009002 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4009002
- SOLPassiveSkillID SkillID 40010202 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 40010202
- SOLCommonSkillID SkillID 4008007 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008007
- SOLCommonSkillID SkillID 4008010 -> MISSING, no fallback
Old union-only entries excluded from executable SOL: [30010203].

### Shepherd
Hero 88000000029 -> ExpertID/expert row 30008.

- SOLUltimateSkillID = 30010101
- SOLSupportSkillID = 3008001
- SOLActiveSkillID = 3008008
- SOLPassiveSkillID = 30010201
- SOLCommonPassiveSkillID = 0
- SOLCommonSkillID = [4008007, 4008010]
- SOLUltimateSkillID SkillID 30010101 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 30010101
- SOLSupportSkillID SkillID 3008001 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 3008001
- SOLActiveSkillID SkillID 3008008 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 3008008
- SOLPassiveSkillID SkillID 30010201 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 30010201
- SOLCommonSkillID SkillID 4008007 -> DeltaForce/Content/DataTables/Skill/SkillDataConfig.uasset row 4008007
- SOLCommonSkillID SkillID 4008010 -> MISSING, no fallback
Old union-only entries excluded from executable SOL: [30010202].

## Coverage correction

Old generated executable count 33 operator-skill instances becomes 26. Passives 7 becomes 6 (Stinger 20010215 is BombDefusal-only in this join). Nonpassive rows 26 becomes 20. Removed Stinger/Luna/Vyron/Hackclaw toolbox 30010203, Shepherd engineer spray 30010202, and MP Vyron magnet 1009002. SOL Vyron magnet 1009005 remains CDID16. No valid supported native action became accepted or disappeared: those seven union rows were individually blocked, but their charge tests and coverage counts were misleading. The raw archive evidence still legitimately contains all 33 mixed-mode report instances.

SOLCommonSkillID contains 4008007 and missing4008010 for all five. Five missing references are retained and never given a fabricated row. No SkillID-vs-row-label difference occurs in these actual selected base config rows; deliberate swapped-label tests prove field-based resolution. Config index construction rejects duplicate SkillIDs across or within packages, including equal duplicate rows. No MP fallback, no group fallback, no row-label fallback.

Cooldown magnitudes for the 26 retained instances are unchanged. CDID joins and Raid SlotChargingData choices still come from raw DataTables, not Lua; dynamic modifiers and effect magnitudes remain separately interpreted/unproved. Tools/spray are not claimed as SOL skills without new SOL evidence.

## Raw snapshot preservation

- EXACT_ABILITIES-vyron-sourceaudit-r2.md.gz: stored SHA256 a5a0cbcf20b8795c8e64a446dee0fef4164d0138bd8b14bc60786ef6c33e2114, baseline byte-identical.
  Decompressed SHA256 b7ecb000d2977ee7fbf50f857fc15914628572d066d8383ab37c43bf15417223.
- deltaops-exact-values-vyron-sourceaudit-r2.json.gz: stored SHA256 c196a502ee969d62324411503991c74e911790defd28c593a31157f0be1e0c43, baseline byte-identical.
  Decompressed SHA256 eba371ead6334450f9e3d1b73ce22e539870de0fff29f8de2e5b19fdfd06b69b.
- deltaops-exact-values-vyron-sourceaudit-r2.txt.gz: stored SHA256 a4cfdf3f6bb7de8422b198584ef543a88b1e7b40479019575aefd9210b005a6b, baseline byte-identical.
  Decompressed SHA256 89c968fd1adf74b623a2e306b95c9c0e890ef317dc108a7478095baad5220d0d.
- deltaops-summary-vyron-sourceaudit-r2.json.gz: stored SHA256 c6cbb9c27fb40fce21c9ff817f5596b65b4684cf4b8f16ce0b0b3d53a2c71662, baseline byte-identical.
  Decompressed SHA256 2605d3068c4d2982860155c4bbdf4edb8273c0689a3e5f531941cbd6c5868ac9.
- five-operators-exact-vyron-sourceaudit-r2.json.gz: stored SHA256 dfc5afbbc38aeebe6eb610e28a793da92790b62684c5db9967b2c914c2895f5f, baseline byte-identical.
  Decompressed SHA256 c80841bee9bf0d3d93452329921be736a8eb237fe7a495584159eac20a38d246.
- five-operators-readable-vyron-sourceaudit-r2.txt.gz: stored SHA256 a5a0cbcf20b8795c8e64a446dee0fef4164d0138bd8b14bc60786ef6c33e2114, baseline byte-identical.
  Decompressed SHA256 b7ecb000d2977ee7fbf50f857fc15914628572d066d8383ab37c43bf15417223.
- index.json: stored SHA256 9020e5572bf89503f18f796d90650885ab00408ea8cde8eeaf9177f3f367bd59, baseline byte-identical.

## Verification

22 Python evidence/binding regression tests pass, including deliberately unequal row labels/SkillIDs, distinct MP/SOL values, missing fields/IDs, duplicate rows and no fallback. Focused interpreted lifecycle suite now passes406 checks, lower than417 because seven invalid charge/router cases were removed and explicit exclusion tests added. DeltaOps Release compiles with zero warnings/errors. Tests do not provide native acceptance. No push, live operation or target integration performed at this checkpoint.
