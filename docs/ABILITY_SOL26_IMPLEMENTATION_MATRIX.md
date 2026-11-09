# PC SOL implementation and admission matrix

Not a completion claim. Exactly 26 executable operator-skill instances; missing source row 4008010 is excluded. Implementation means a partial documented CS2 interpretation, not full DF equivalence. Deployment approval is separate from offline implementation. All engine QA remains unperformed. Raw r2 is unchanged.

| Operator | Skill | Implementation | Activation | Concrete native safety blocker |
|---|---|---|---|---|
| 1 | 2008002 | shared owned-projectile foundation implemented; skill-specific behavior unimplemented | held, no live approval | Native inert creation/type/spawn preservation and unload-independent projectile expiry not established. |
| 1 | 2008005 | shared owned-projectile foundation implemented; skill-specific behavior unimplemented | held, no live approval | Native inert creation/type/spawn preservation and unload-independent projectile expiry not established. |
| 1 | 4008007 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 1 | 20010101 | implemented partial committed heal interpretation | held, no live approval | none identified for this partial boundary; not native QA approval |
| 1 | 20010201 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 2 | 2002017 | implemented partial private names-and-distances interpretation | normal configured private telemetry admission | none identified for this partial boundary; not native QA approval |
| 2 | 3008002 | shared owned-projectile foundation implemented; skill-specific behavior unimplemented | held, no live approval | Native inert creation/type/spawn preservation and unload-independent projectile expiry not established. |
| 2 | 4008002 | implemented partial owned status interpretation | held, no live approval | Reversible shared scalar cannot distinguish identical-value competing writers; unload failure cannot guarantee restoration. |
| 2 | 4008007 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 2 | 40010201 | implemented partial post-hit telemetry interpretation | held, no live approval | none identified for this partial boundary; not native QA approval |
| 3 | 1009001 | implemented partial velocity-only impulse interpretation | held, no live approval | none identified for this partial boundary; not native QA approval |
| 3 | 1009003 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 3 | 1009004 | implemented partial borrowed damage-info modifier | held, no live approval | Damage classification and other resident hook interactions require separate review; no damage construction is used. |
| 3 | 1009005 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 3 | 4008007 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 3 | 10010301 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 4 | 1002016 | implemented partial private names-and-distances interpretation | normal configured private telemetry admission | none identified for this partial boundary; not native QA approval |
| 4 | 4008007 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 4 | 4009002 | implemented partial owned status interpretation | held, no live approval | Reversible shared scalar cannot distinguish identical-value competing writers; unload failure cannot guarantee restoration. |
| 4 | 4009003 | shared owned-projectile foundation implemented; skill-specific behavior unimplemented | held, no live approval | Native inert creation/type/spawn preservation and unload-independent projectile expiry not established. |
| 4 | 40010202 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 5 | 3008001 | implemented partial owned status interpretation | held, no live approval | Reversible shared scalar cannot distinguish identical-value competing writers; unload failure cannot guarantee restoration. |
| 5 | 3008008 | shared owned-projectile foundation implemented; skill-specific behavior unimplemented | held, no live approval | Native inert creation/type/spawn preservation and unload-independent projectile expiry not established. |
| 5 | 4008007 | skill-specific effect unimplemented | held, no live approval | No skill-specific native effect boundary implemented. |
| 5 | 30010101 | implemented partial owned status interpretation | held, no live approval | Reversible shared scalar cannot distinguish identical-value competing writers; unload failure cannot guarantee restoration. |
| 5 | 30010201 | implemented partial borrowed damage-info modifier | held, no live approval | Damage classification and other resident hook interactions require separate review; no damage construction is used. |

Exact API URLs, typed initial/capacity/CD join/recharge fields, CS2 tuning defaults and units are embedded per row in ABILITY_SOL26_COVERAGE.json. Full per-skill missing DF behavior remains in ABILITY_SOL26_COVERAGE.md. No DF-to-Source distance conversion is asserted. HP is integer health points, scalar modifiers dimensionless, cooldown seconds become monotonic milliseconds, motion uses Source units/second and yaw degrees.
