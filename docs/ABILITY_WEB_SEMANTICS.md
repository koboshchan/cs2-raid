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
