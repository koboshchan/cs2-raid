"""Offline source/fixture guards, not native world-text visibility or engine QA."""
import json
import re
from collections import Counter
from pathlib import Path

root = Path(__file__).resolve().parents[3]
source = (root / "plugins/Debug/DebugPlugin.cs").read_text()
fixture = json.loads((root / "config/spawns/spaceport-other-red-layer-confirmed.json").read_text())
short_tags = {102004: "RedItem", 102005: "Keycard", 102007: "Satcom",
              102008: "Fuel", 102009: "Trophy", 102012: "TreasurePile"}
expected = {}
for row in fixture:
    record = row["decoded_poi_record"]
    assert str(record["Id"]) == row["id"]
    assert (record["MapId"], record["MapMode"], record["Active"]) == (203, 2, 1)
    assert row["id"] not in expected
    expected[row["id"]] = short_tags[record["ItemId"]]
assert len(expected) == 136
mapping = source.split('private static readonly IReadOnlyDictionary<string,string> PurpleMarkerLabels=', 1)[1].split('    };', 1)[0]
entries = re.findall(r'\["([^"]+)"\]="([^"]+)"', mapping)
assert len(entries) == len(dict(entries)) == 136
assert dict(entries) == expected
purple_ids = re.findall(r'\("([^"]+)","confirmed_other_red_layer",new Point\(', source)
assert len(purple_ids) == len(set(purple_ids)) == 136
assert set(purple_ids) == set(expected)
for suffix in (2013, 2014, 2030, 2043, 2066):
    assert expected['203020' + str(suffix)] == "Keycard"
print("PASS exact 136 decoded ItemIds, localized short tags, five fallback Keycards and purple marker ID coverage", dict(Counter(expected.values())))

spawn = source.split('    private void SpawnPurpleMarkerLabel(', 1)[1].split('    private void ClearPurpleMarkerLabels()', 1)[0]
assert spawn.count('Utilities.CreateEntityByName<CPointWorldText>("point_worldtext")') == 1
assert spawn.index('purpleMarkerLabels.Count>=PurpleMarkerLabels.Count') < spawn.index('Utilities.CreateEntityByName')
assert spawn.index('purpleMarkerLabels.Add(text);') < spawn.index('text.Entity!')
assert 'text.MessageText=label;' in spawn
assert 'text.ReorientMode=PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_AROUND_UP;' in spawn
assert 'pos.Z+30.0f' in spawn
assert spawn.count('text.Collision.SolidType=SolidType_t.SOLID_NONE;') == 2
assert 'text.MoveType=MoveType_t.MOVETYPE_NONE;' in spawn
assert 'text.TakesDamage=false;' in spawn
for forbidden in ('AddTimer', 'OnTick', 'NextFrame', 'AcceptInput', 'SetParent'):
    assert forbidden not in spawn
assert source.count('SpawnPurpleMarkerLabel(id,pos);') == 1
assert 'if(type=="confirmed_other_red_layer")SpawnPurpleMarkerLabel(id,pos);' in source
cleanup = source.split('    private void ClearPurpleMarkerLabels()', 1)[1].split('    private CounterStrikeSharp', 1)[0]
assert cleanup.index('purpleMarkerLabels.Clear();') < cleanup.index('label.Remove();')
assert 'if(label.IsValid)label.Remove();' in cleanup
assert 'catch(Exception e)' in cleanup
clear_balls = source.split('    private void ClearBigSafes()', 1)[1].split('    private void BigSafeCommand', 1)[0]
assert 'ClearPurpleMarkerLabels();' in clear_balls
batch = source.split('    private void SpawnLootBatch()', 1)[1].split('    private void LootStatusCommand', 1)[0]
assert 'catch(Exception e)' in batch and 'ClearBigSafes();' in batch
clear_state = source.split('    private void ClearState()', 1)[1].split('\n    private ', 1)[0]
assert 'ClearBigSafes();' in clear_state
assert 'RegisterListener<Listeners.OnMapEnd>(() => { RestoreCvars(); ClearState(); });' in source
unload = source.split('    public override void Unload(', 1)[1].split('\n    private ', 1)[0]
assert 'ClearState();' in unload
print("PASS bounded one-label spawn, no per-label updates, noncollision, tracked partial-failure/off/unload/map-end cleanup")
