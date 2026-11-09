# Authorized r2 interpretation, offline only

Raw r2 is unchanged. Raid-mode slot rows are chosen, not MP/BombDefusal. This layer is interpreted, not a claim of DF-equivalent effects. Zero Max with weapon-count ammo is modeled as one ability token; no free unlimited casting. Init remains literal (0 starts charging). Seconds are inferred from Time and TimeRecoverSec field families. Non-CD skills receive a one-second anti-repeat guard, not an archive cooldown. Recharge is sequential, one token per Time; score/perk recovery is not implemented.

| Source skill / field | Raw | Chosen representation | Evidence / confidence / assumptions |
|---|---|---|---|
| 2008002/CDID | 2002 | 2002 | integer join, high |
| 2008002/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 2008002/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 2008002/Time | 60.0 | 60.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 2008005/CDID | 2001 | 2001 | integer join, high |
| 2008005/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 2008005/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 2008005/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010101/CDID | 2 | 2 | integer join, high |
| 20010101/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 20010101/Max | 3 | 3 | literal count except 0=weapon-backed one token, medium |
| 20010101/Time | 30.0 | 30.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010201/CDID | 0 | 0 | integer join, high |
| 20010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 20010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 20010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 20010215/CDID | 0 | 0 | integer join, high |
| 20010215/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 20010215/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 20010215/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 2002017/CDID | 14 | 14 | integer join, high |
| 2002017/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 2002017/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 2002017/Time | 75.0 | 75.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008002/CDID | 92 | 92 | integer join, high |
| 3008002/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008002/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008002/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008002/CDID | 13 | 13 | integer join, high |
| 4008002/Init | 1 | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008002/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 4008002/Time | 50.0 | 50.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 40010201/CDID | 0 | 0 | integer join, high |
| 40010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 40010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 40010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009001/CDID | 18 | 18 | integer join, high |
| 1009001/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009001/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009001/Time | 20.0 | 20.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009002/CDID | 16 | 16 | integer join, high |
| 1009002/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009002/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009002/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009003/CDID | 17 | 17 | integer join, high |
| 1009003/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009003/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009003/Time | 70.0 | 70.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009004/CDID | 0 | 0 | integer join, high |
| 1009004/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 1009004/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 1009004/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1009005/CDID | 16 | 16 | integer join, high |
| 1009005/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1009005/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 1009005/Time | 55.0 | 55.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 10010301/CDID | 0 | 0 | integer join, high |
| 10010301/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 10010301/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 10010301/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 1002016/CDID | 22 | 22 | integer join, high |
| 1002016/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 1002016/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 1002016/Time | 90.0 | 90.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4009002/CDID | 20 | 20 | integer join, high |
| 4009002/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 4009002/Max | 0 | 1 | literal count except 0=weapon-backed one token, medium |
| 4009002/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4009003/CDID | 21 | 21 | integer join, high |
| 4009003/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 4009003/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 4009003/Time | 45.0 | 45.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010203/CDID | 1031 | 1031 | integer join, high |
| 30010203/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010203/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010203/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 40010202/CDID | 0 | 0 | integer join, high |
| 40010202/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 40010202/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 40010202/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008001/CDID | 11 | 11 | integer join, high |
| 3008001/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008001/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008001/Time | 40.0 | 40.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 3008008/CDID | 15 | 15 | integer join, high |
| 3008008/Init | 2 | 2 | Raid slot count, high if present; absent=one adapter token |
| 3008008/Max | 2 | 2 | literal count except 0=weapon-backed one token, medium |
| 3008008/Time | 45.0 | 45.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 4008007/CDID | 0 | 0 | integer join, high |
| 4008007/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 4008007/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 4008007/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010101/CDID | 12 | 12 | integer join, high |
| 30010101/Init | 0 | 0 | Raid slot count, high if present; absent=one adapter token |
| 30010101/Max | 1 | 1 | literal count except 0=weapon-backed one token, medium |
| 30010101/Time | 90.0 | 90.0 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010201/CDID | 0 | 0 | integer join, high |
| 30010201/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010201/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010201/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
| 30010202/CDID | 1030 | 1030 | integer join, high |
| 30010202/Init | absent | 1 | Raid slot count, high if present; absent=one adapter token |
| 30010202/Max | absent | 1 | literal count except 0=weapon-backed one token, medium |
| 30010202/Time | absent | 1 seconds | SlotChargingData.Time + TimeRecoverSec, medium; absent uses explicit UseCD or one second adapter guard |
