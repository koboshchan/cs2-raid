Standard-library offline checks. Extract and execute this Python block from the repository root.

```python
import copy
import hashlib
import json
import math
from pathlib import Path
import subprocess
from decimal import Decimal, ROUND_HALF_UP

BASE = '56c25b57cfe9d3abf183652ecde72d0a511fd2bd'
root = Path('.')
recipe = Path('docs/loot-placements/regeneration.md').read_text().split('```python\n')[1].split('```')[0]
ns = {'__name__': 'full40_tests'}
exec(compile(recipe, 'regeneration.md', 'exec'), ns)
transform = ns['transform']
old = json.loads(subprocess.check_output(['git', 'show', BASE + ':config/loot-placements.json']))
new = json.loads(Path('config/loot-placements.json').read_text())

def literal(p):
    # Independent literal evaluation with explicit pre-residual origin clamps.
    bx, by, bz = p[1]/100, -p[0]/100, p[2]/100
    cx, cy = math.floor(bx/40)*40, math.floor(by/40)*40
    ox = max(512-14336, min(512+14336, 512+cx-6693.91505))
    oy = max(160-14336, min(160+14336, 160+cy-4578.046))
    oz = max(-8192, min(8192, 220.48575))
    return [ox+by-cy, oy-(bx-cx), oz+bz]

assert len(new['loot']) == len(old['loot']) == 247
assert len(new['excluded_loot']) == len(old['excluded_loot']) == 88
for key in old:
    if key not in ('loot', 'cs2_transform', 'coordinate_system', 'notes'):
        assert new[key] == old[key], key
assert new['coordinate_system']['order'] == old['coordinate_system']['order']
assert new['coordinate_system']['original_semantics'] == old['coordinate_system']['original_semantics']
changed = 0
for before, after in zip(old['loot'], new['loot']):
    assert {k:v for k,v in before.items() if k not in ('cs2_position','placement_review')} == {k:v for k,v in after.items() if k not in ('cs2_position','placement_review')}
    assert after['cs2_position'] == literal(before['df_native_position']) == transform(before['df_native_position'])
    assert all(math.isfinite(v) for v in after['cs2_position'])
    assert after['placement_review']['requires_review'] is True
    assert 'nearest_floor_signed_residual' not in after['placement_review']
    changed += before['cs2_position'] != after['cs2_position']
assert changed == 247

landmarks = [([-417680,661800,-21404],[434.885,-276.046,6.446]),([-419906.49,663787.35,-21756.84],[457.150,-295.920,2.917]),([-469791.60,643468.90,-21817.41],[236.001,227.265,2.312])]
for point, expected in landmarks:
    actual = transform(point)
    assert all(abs(a-b) <= 0.000501 for a,b in zip(actual,expected))
    # Decimal arithmetic gives the requested three-decimal display, including the exact half tie.
    x,y,z = map(lambda v: Decimal(str(v)), point)
    bx,by,bz = y/100,-x/100,z/100
    cx=(bx/40).to_integral_value(rounding='ROUND_FLOOR')*40
    cy=(by/40).to_integral_value(rounding='ROUND_FLOOR')*40
    ox=min(Decimal(14848),max(Decimal(-13824),512+cx-Decimal('6693.91505')))
    oy=min(Decimal(14496),max(Decimal(-14176),160+cy-Decimal('4578.046')))
    exact=[ox+by-cy,oy-(bx-cx),Decimal('220.48575')+bz]
    assert [float(v.quantize(Decimal('.001'),rounding=ROUND_HALF_UP)) for v in exact] == expected

# Both signs and exact cell edges, including tiny negative values where truncation fails.
checks=0
for bx in (-1e6,-400,-40.000001,-40,-39.999999,-.000001,0,.000001,39.999999,40,40.000001,1e6):
    for by in (-1e6,-40.000001,-40,-.000001,0,39.999999,40,40.000001,1e6):
        for z in (-1e7,0,1e7):
            point=[-by*100,bx*100,z]
            assert transform(point) == literal(point)
            checks += 1
# Hand-calculated negative cell case and positive/negative XY clamp branches.
assert all(abs(a-b)<1e-9 for a,b in zip(transform([.1,-.1,0]),[-6181.91605,-4498.045,220.48575]))
assert transform([-100000000,100000000,0]) == [14848,14496,220.48575]
assert transform([100000000,-100000000,0]) == [-13824,-14176,220.48575]
# Clamps apply to origin only, not final positions or input Z.
assert transform([-100000001,100000001,10000000])[2] > 8192
assert transform([-100000001,100000001,0])[0] > 14848
outliers=json.loads(Path('docs/loot-placements/outliers-over5.json').read_text())
assert outliers['current_outlier_count'] is None and not outliers['loot']
assert outliers['status'].startswith('OBSOLETE')
original=Path('config/loot-placements.json').read_bytes()
exec(compile(recipe,'regeneration.md','exec'),{'__name__':'__main__'})
assert Path('config/loot-placements.json').read_bytes() == original
exec(compile(recipe,'regeneration.md','exec'),{'__name__':'__main__'})
assert Path('config/loot-placements.json').read_bytes() == original
print(f'PASS 247 literal positions; 247 changed; 88 excluded and original metadata preserved; 3 landmarks; {checks} boundary/clamp cases; hand expectations; deterministic generation twice')
print('sha256',hashlib.sha256(original).hexdigest())
```
