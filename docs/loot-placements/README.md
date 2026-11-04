Ocean full40 cell loot placements

config/loot-placements.json contains 247 regenerated active positions and all 88 unchanged excluded entries. Original coordinates, source IDs, exact-case assets, rotation, model mapping and exclusion policy are preserved. Extraction markers and power switches are unchanged. This is data only, not loaded by a plugin.

The exact user-supplied Ocean full40 CELL transform is in regeneration.md and calibration.json. It uses floor (including negative coordinates), 40-unit cells, the specified origin clamps before local residual addition, and Z offset 220.48575. It is not the old map-v1 scale/translation fit. Positions are not snapped or rounded. Landmark displays alone are rounded to three decimals.

Old surface residuals and the 24-outlier classification are obsolete for these new coordinates. Historical reports remain recoverable at baseline 56c25b57cfe9d3abf183652ecde72d0a511fd2bd. outliers-over5.json is an explicit obsolete marker, not evidence of zero outliers. No new collision/floor or asset-pivot validation was performed; every active placement remains marked for geometry review. This mapping is not a claim about newer map geometry.

Run the Python block in regeneration.md from the repository root with Python 3. It uses only the standard library, reads the immutable baseline through git, and deterministically regenerates active coordinates. The Python block in tests.md verifies the checked-in result. No plugin source changed, so no compile target is affected. No merge, deployment, server or game API calls are involved.

Floating-point display note. Landmark 2 Y is mathematically -295.9195. The specified binary-float operation order computes -295.91949999999997, which Python round(..., 3) displays as -295.919 rather than the supplied -295.920. Decimal half-away-from-zero display gives -295.920 exactly. The stored full-precision result follows the literal formula unchanged; tests check both the binary result tolerance and the decimal display. No constants or formula were adjusted to force rounding.
