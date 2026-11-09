# Authority correction, supersedes lower wording

Extracted DF tables, source Lua and native evidence govern behavior AND values.
Corrected PC SOL joins select applicable rows. Wikis and supplied summaries are
LOWCONFIDENCE hints only, never mandates and never overrides of extracted flags.
Contradictory hints are discarded.

DFPROVEN Shepherd SkillID30010201 has DamgeCoifficient[0]:
fModifyRateExplodeDamage=0.7, fModifyRateAbilitAssault=1.0, LimitPlayMode=[].
This directly establishes explosion-specific modification in the selected raw
row. NativePassiveDamage now uses the extracted .7 constant, replacing the old
.9 CS2 tuning. Mapping DF explosion to CSS DMG_BLAST is CS2ASSUMPTION pending native
QA; the Wiki is NOT authority for this change. The raw r2 bytes are unchanged.

DFPROVEN Luna SkillID40010201 is Scout/Passive, AddBuffs=[8011,100008],
bEnableSniperMark=true, bOverrideAimingCheck=true, TeamPlayerIcon=None.
Those fields establish a marking-related passive, but do NOT establish allied
recipient scope, post-hit trigger, duration, wall/smoke visibility or minimap.
Ally coordinate telemetry is LOWCONFIDENCE CS2ASSUMPTION, not usermandated and
not DFPROVEN. No contradictory decoded field is identified; None icon is not
proof of private-only visibility. Resolving buffs8011/100008 and relevant native
implementation is required before claiming faithful squad marking.
Current output uses Raid allies only as an explicit experimental interpretation.

Vyron1009004 AddSkills=[1009001], AddBuffs=[303016,8013], UseCustomHeight=true
do not by themselves prove a .5 fall modifier or a particular speed trigger.
These remain explicit CS2ASSUMPTION, not exact extracted behavior.

All mechanic claims in the following historical fetch summary are subordinate
to this correction. Lowconfidence web descriptions are not behavioral authority.

# Qualitative references, not numeric authority

Full fetched responses are retained at /home/kit/out/ability-web-semantics-sources.json.
All successful pages were read in full, with no continuation omitted. All web
numbers are excluded. Raw r2 and corrected PC SOL bindings remain authoritative
for imported numeric fields; undecrypted buff magnitudes are not filled from web.

Read successfully:
https://delta-force.fandom.com/wiki/Stinger
https://dfwiki.net/operator/luna
https://delta-force.fandom.com/wiki/Vyron
https://delta-force.fandom.com/wiki/Hackclaw
https://gamingonphone.com/guides/delta-force-shepherd-guide-gameplay-tips
https://www.playdeltaforce.com/en/universe/

The official universe URL returned a landing page, not detailed ability evidence.
Liquipedia Luna, Vyron and Hackclaw URLs were blocked by bot/rate checks. No quote
or independent verification is attributed to those pages.

Implemented semantic corrections:
Shepherd blast mitigation is supported by the GamingOnPhone guide, not universal
damage reduction. NativePassiveDamage now filters DMG_BLAST before the existing
explicit CS2 multiplier. That multiplier is NOT sourced from this guide.
Luna post-hit marking is supported by dfwiki. Squad sharing is specifically the
user's supplied reference, NOT independently confirmed by that fetched page.
NativeTrackedHit shares one-shot coordinates only with living, admitted allies
through Raid policy. This is an approximation of squad marking, not minimap
tracking or a duration-matched reveal.

Further semantic constraints:
Stinger Fandom supports healing utility, directional smoke and rescue semantics.
Homing, sustained focus, selfheal and converting only owned smoke require dedicated
adapters; current instant health setter does not claim those behaviors. Conflicting
full-health rescue descriptions are not an imported heal amount.
Luna arrow sensors, moving targets and volt continuous damage require owned
trajectory/sensor and damage adapters, not the current instant recon/pulse.
Vyron Fandom distinguishes compressed-air knockdown from magnetic explosive.
Do not implement the air shell as HE blast. Dash reset versus reduction conflicts
remain explicit interpretation questions; PC SOL numbers are not changed.
Hackclaw Fandom semantics require signals, directed flash drone, electronic
suppression and crouched stealth, not generic damage or invented signal counts.
Shepherd sonic firerate/recon interference, resonating-device destruction and
entry alerts are not proved by current movement slow. Other gadgets retain raw
source names only; this guide does not authorize inventing their mechanics.

No new web numeric value was imported. Existing interpretation magnitudes remain
separately labeled and do not establish DF-equivalent behavior.
