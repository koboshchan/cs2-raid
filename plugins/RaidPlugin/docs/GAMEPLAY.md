# Flow v2 offline staging

The master requirements are in docs/SPEC.md; docs/TODO.md separates offline code from missing native and money integration.

The old shoot-to-loot, automatic native-pickup cash award and killer cash-transfer paths are removed. Ground claims require living active members, proximity and E. Items/equipment/bags pay fixed configured dollars; safes require continuous E within 96 units for the configured hold duration and use their configured range. Claims are single-consumer for the match. This is in-game cash only, not SSG credit.

All participants must pick a team, then one of all five characters. The state advances through 30s buy, 5s countdown, teleport, 10s freeze, 30min active. Late timer execution starts the next phase's full duration rather than skipping freeze. Buy debits are observed engine debits, not duplicate catalog debits. Utility purchases are rejected; character kits remain the sole utility source. Cash-in and reload charging are not implemented.

Map loot is keyed by exact targetname, with per-rule spawn rate. Non-safe items use Value, no Min/Max/hold; safes use Min/Max/HoldSeconds. Duplicate or absent entities are skipped, not guessed. Four lever identities are configured individually; two different pulls start five minutes. The adapter prefers the configured trigger_multiple's touch callbacks, otherwise an explicitly configured sphere. Missing coordinates/targetnames leave features unavailable.

Both enabled and nativeFlowVerified default false; coordinatesConfirmed alone cannot enable the unfinished native adapter. Actual character assignment is deliberately unavailable until the separate DeltaOps capability and duo filters are implemented. The DeltaOps numbered chat command patch has no capability and is not imported here.

Extraction candidates are pure intent snapshots, not payments. ConfirmExtraction is a trusted future receipt boundary, not an HTTP handler. The adapter NEVER calls it because the backend's /extract route is 503. At countdown expiry the adapter reports settlement unavailable, keeps local cash and resets levers; this is NOT working extraction. No mint/refund/payout is made.

Only account linking uses the existing HTTP bridge. In-memory match reset discards the match and does not create a persistent stash. See TODO for native pickup, corpse drops, money-bag visibility, reload, respawn/round lifecycle, native QA and durable settlement blockers.
