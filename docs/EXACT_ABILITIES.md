# Exact-data ability scaffold (offline only)

This is a native-free framework, not implemented DF abilities. No production operator
names, skill IDs, timings, damage, charges or movement values are bundled. The table
inventory URL is not a row export and has not been used as gameplay data. Serialized
UseCD=0 must not be treated as an actual cooldown, nor CDID as a duration.

The existing DeltaOps plugin and Raid capability bridges are unchanged. Their legacy
config defaults are **not** imported. No Load hook, keybind, selection menu, command,
native adapter, live configuration, deployment or server restart is included. Money
and SSG are untouched. A DLL build is not native gameplay QA or authorization to load.

## Files and integration boundary

Abilities/ExactAbilityCatalog.cs retains raw decoded rows and rejects corrupt hashes,
duplicate IDs/JSON properties, missing referenced rows and mismatched source fields.
Abilities/ExactAbilityRuntime.cs is a single-game-thread selection/activation ledger
with injected monotonic clock, admission policy and an initially empty adapter registry.

An integration owner must explicitly deliver death, disconnect, pawn replacement,
map end and unload cleanup; call MapEnd and honor a false return when cleanup failed.
Revoke quarantines the pawn and retains failed effect ownership. Selection tombstones
prevent death/reconnect/reselection from resetting charges within one generation.
Only successful full MapEnd cleanup advances generation and permits new selection.
Generation starts at 1 as a local fencing token, not a DF gameplay value.
No callbacks are registered by these files.

Adapters must prove transactional rejection and return an owned, retry-safe IDisposable
on success. An exception quarantines the actor; this cannot magically undo native
effects created before an adapter throws. Any future native adapter needs separate
ownership/rollback and failure-injection QA. Registry keys are implementation mappings,
never inferred operator names. Admission must validate current pawn, game phase,
team and session. This scaffold cannot establish those facts itself.

## Decoder handoff format

Supply UTF-8 JSON with case-sensitive property names matching AbilityExport.
SchemaVersion is 1. Rows, Operators and Abilities are arrays (empty arrays allowed).
Unknown JSON members and duplicate JSON properties are rejected.

Each SourceRow contains Table (exact package path), Row (exact row key), AssetSha256
(64 hex characters), RawJson (the unmodified JSON object text for that decoded row),
and RowSha256 (SHA-256 of the UTF-8 RawJson bytes, without normalization).
The asset digest is recorded provenance, not independently verified against an asset
by this in-memory loader. Decoder completeness, row counts, archive identity and
unsupported property report must accompany the handoff for review.

FieldEvidence contains Table, Row, JsonPointer (RFC 6901 object/array path) and RawJson
(the exact JSON token text at that path). IDs must equal their source token values.
ExactNumber contains Value, Unit, Evidence, Unresolved and UnitEvidence.
A resolved number requires numeric Evidence and source-backed UnitEvidence. A raw
number without a known unit must stay in its raw row, with normalized Value, Unit,
Evidence and UnitEvidence null and a nonempty Unresolved reason. Do not annotate
seconds, meters, charges or counts by guesswork. Unsupported source unit metadata or
enum translation needs an explicitly reviewed importer extension, not rewritten raw
rows. No automatic unit conversion is implemented.

ExactChoice contains Value, Evidence and Unresolved. Unknown values use null Value
and Evidence plus a nonempty reason. AbilityDefinition contains SkillId, Identity,
AdapterKey (null while unresolved), Cooldown, Duration, Charges, ResourceMode,
CooldownStart, Parameters (named ExactNumber records), and References.
RowReference contains Name, Table, Row and Unresolved. A resolved target must exist
in Rows; an unresolved reference has null Table/Row and a nonempty reason preserving
the original key, such as a CDID whose target table is still unknown. Keep every raw
foreign key in its SourceRow. Row existence does not prove a semantic join: the
decoder/integration review must prove operator membership and reference resolution.

OperatorDefinition contains OperatorId, Identity and SkillIds. IDs and skills are
strings so no numbering or operator-name convention is invented. Do not derive
operator identity from a guessed substring of a skill ID.

Activation requires resolved parameters/references and an explicitly registered
adapter. Currently supported ledger semantics are source-backed ResourceMode
"finite" (non-recharging consumable count) or "unlimited", and CooldownStart
"activation". Any other policy remains blocked. These are supported contracts, not
claims that DF serializes those strings. If DF uses other enums, keep them unresolved
until a reviewed semantic mapping is added. An unlimited charge value is null with an
explicit not-applicable reason, not an invented count. Finite charges must be integral
positive counts. Timing units must be proven seconds and exactly representable in
TimeSpan ticks; unsupported precision or oversized values refuse activation. Damage
and movement fields are adapter-specific parameters, not enacted by this ledger.

The actual cooldown lookup behind CDID, charge/recharge rules, timing start policy,
runtime/BP modifiers, operator-to-skill links and native effects are still dependencies.
Do not substitute serialized UseCD, client-only fields, legacy configuration or fixture
values when those dependencies are unresolved.

## Offline verification

Run with /home/kit/extract-zone-sdk/dotnet:

    dotnet run --project tests/ExactAbilities/ExactAbilities.csproj -c Release
    dotnet run --project plugins/RaidPlugin/tests/RaidState.Tests.csproj -c Release
    dotnet build plugins/RaidPlugin/RaidPlugin.csproj -c Release -p:CSS_API_DIR=/home/kit/debug-api-inspect/bin/Debug/net10.0

The separate exact-ability tests use conspicuously synthetic identities and values.
They are not DF exports and must never be copied into production data. No production
data JSON is provided, so unresolved work cannot accidentally acquire fallback values.
Live integration or loading requires explicit parent/user approval.


The pinned r2 evidence import now lives in data/exact-abilities, with reproducible tools and regression tests described in tools/exact-abilities/README.md. It preserves original r1 separately and verifies the canonical r2 index plus all six artifacts. The evidence-only loader cannot supply a gameplay AbilityDefinition. Source report hashes are not source asset hashes.

Only Stinger, Luna, Vyron, Hackclaw and Shepherd are selectable. Their legacy enum values remain 1,2,3,4,5 and their original public bridge/command IDs remain 2,3,4,5,6. Retired D-Wolf enum 0 and wire 1 are never reused. Menus show stable public IDs, not new contiguous display indexes. Invalid stored selections latch fail-closed recovery until an explicit session reset; Raid uses its existing StopGameplay cleanup before financial/extraction processing. Tests cover every mapping, invalid-enum retirement, stale selections in Selection/Buy/Countdown/Freeze/Active and no spending or phase advancement.

The preserved legacy DeltaOps effects still use historical guessed defaults for the remaining five characters. Their retention is not an exact-value implementation or a migration of imported raw values into gameplay. No native adapters have been registered, no ability hooks have been added and no live plugin has been loaded.
