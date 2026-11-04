Deterministic full40 coordinate regeneration. Run from the repository root. Metadata is not rewritten except JSON serialization; no external dependencies.

```python
import math

def transform(position):
    x, y, z = position
    bx, by, bz = y / 100, -x / 100, z / 100
    gx, gy = math.floor(bx / 40), math.floor(by / 40)
    cx, cy = gx * 40, gy * 40
    ox, oy, oz = 512 + cx - 6693.91505, 160 + cy - 4578.046, 220.48575
    ox = min(512 + 14336, max(512 - 14336, ox))
    oy = min(160 + 14336, max(160 - 14336, oy))
    oz = min(8192, max(-8192, oz))
    return [ox + by - cy, oy - (bx - cx), oz + bz]

if __name__ == '__main__':
    import json
    from pathlib import Path
    path = Path('config/loot-placements.json')
    data = json.loads(path.read_text())
    for item in data['loot']:
        item['cs2_position'] = transform(item['df_native_position'])
    path.write_text(json.dumps(data, indent=2, ensure_ascii=False) + '\n')
```
