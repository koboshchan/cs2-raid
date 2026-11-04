Trusted raid backend

Node 24 only. npm ci, npm test and npm run build use fake transports and no live SSG charges. The image must use a protected durable /data volume. Links, dormant shop outbox and raid financial operations share /data/raid.sqlite with WAL and synchronous FULL.

SSG_API_ORIGIN must be the approved HTTPS origin. SSG_CLIENT_ID and SSG_APP_SECRET stay exclusively on the backend. BRIDGE_TOKEN must contain at least 32 random characters and must never reach clients. Backend and native plugin belong on a private authenticated network. All HTTP routes require Authorization Bearer BRIDGE_TOKEN, reject any Origin header and disable caching. A bridge token authenticates the trusted native server, not arbitrary player IDs.

createServer optionally accepts {ledger, api, native}. These dependencies open the implemented cash-in/extraction routes in code. Default CLI startup deliberately supplies none when RAID_FINANCIAL_ENABLED is not exactly1, so production remains closed with 503 until an independently tested native adapter and approved SSG configuration are wired. RAID_FINANCIAL_ENABLED=1 explicitly installs the outbound NativeHttpBridge and ledger at CLI boot; absent or any other value remains closed. This flag does not bypass native validation or configure upstream policy. GET /health distinguishes injected versus not_ready. Shop routes remain disabled.

POST /cashin and /cashout both mean cash INTO the raid (legacy command naming), with integer amountCents from 1 through 1500000 and a server-authenticated raid/session. POST /extract, /payout and /settlement accept strict extraction snapshots. POST /settlement/proof only reads a durable paid/settled operation by operationKey and exact requestFingerprint. It cannot perform a payment or native completion. There is no /finalize route and no HTTP bridge command executor. Never accept caller-provided receipt text as authority.

RaidLedger persists immutable operation snapshots and server catalog values before networking. Identity is server+raid+SteamID+operation; payload changes under that identity conflict. Extraction reserves globally unique item instances, rejects armor and helmet catalog kinds, and values guns/pistols and unused utility at FULL server catalog price per SPEC section12. Historical 90% sellback is a distinct older rule and remains an explicit ambiguity for the caller. Amount precision is checked; upstream still controls request-size and wallet limits. SSG simulation currency is not bank USD.

Native integration implements RaidNativeBridge from raid-ledger.ts. validateAndFreeze must atomically validate authenticated Steam, connection/pawn/map generations, current raid and phase, cash, item instances/ownership and extraction window, then lock value. Retrying the same key/snapshot must be idempotent. finalize receives a structured proof built only from a matching durable SSG receipt; it must apply cash-in once or consume frozen extraction value once, on the same original game-thread session. Rejections remain unresolved, not automatically refunded or regranted. Restart without the original in-memory raid must reject.

The ledger follows prepared, frozen, paid and settled states, with manual on canonical receipt mismatch. Lost HTTP responses reconcile GET transactions/by-key with the original key before retrying a write. SSG performs idempotent commits and live scope/consent/credential/budget checks; the backend never bypasses those. An idempotent native adapter is mandatory even when SQLite already has a financial receipt. Native ack loss can be retried only under the identical persisted operation and proof. Zero-value extraction has a durable local settlement with transactionId null, not a fabricated SSG transaction.

New account link requests ask for balance:debit AND balance:credit. User consent remains necessary. Existing debit-only links cannot mint; their approved scopes must not be silently upgraded or their bindings overwritten. Local bindings are Steam/app-user/grant IDs; upstream authenticates each new financial mutation. Changed SSG policy or stale/revoked grants can reject operations even when Lucas has globally approved unlimited raid mint budgets.

Lucas explicitly approved raid mint and unlimited mint/payout budgets and per-operation policy on Nov4. This is not proof those policies are deployed/configured, credentials approved, or accounts consented. The separate SSG source owner implements -1 semantics. No config or grants were enabled by this backend work. SSG debits decrease player wallet and room issuance; there is no app wallet/escrow. Extraction credits use mint, not mislabelled original-debit refunds. The dormant shop outbox retains its separate $1000 purchase safety gate and bounded undelivered-original-payer refund implementation.

The full integration contract and acceptance conditions are in the handoff /home/kit/out/raid-native-interface.md. Unit tests cover crash/reopen, response loss, immutable replay, duplicate workers, session rejection, failed scope/budget/auth, item reuse, server pricing and authenticated persistent proof lookup. Native live end-to-end QA has not happened.

Durable native recovery is available even with financial enablement off. The
backend opens its SQLite operation journal without injecting the money execution
or native callback gateway. Recovery does not create operations. The trusted
native host mirrors an already-issued original operation with POST
/native/recovery (alias /native/journal/phase). Body fields are operationKey,
requestFingerprint, nativeSnapshot (immutable serialized string, up to 131072
characters), phase, evidence, and proof for every phase after snapshot. The
sequential phases are snapshot, consumption_intent, consumed, cleanup_complete.
Proof is the exact structured settlement proof, excluding the lookup's state.
Snapshots and each phase's first accepted evidence are retained; earlier phase
replay acknowledges the current journal without replacing evidence or regressing.
The HTTP byte cap is 196608, including JSON escaping. Use local fsync durability
before submitting the initial financial request and before each economic action.

POST /native/journal/read accepts only operationKey and requestFingerprint. It
returns original request/session, durable canonical proof when available, phase
and audit evidence, and disposition (quarantined or unresolved). The native bearer
credential can read this even when no admin credential is installed in the game
container. Snapshot content is native-authored evidence, not backend proof of
physical isolation or inventory correctness.

POST /admin/reconciliation/quarantine requires the separate configured
RAID_RECONCILIATION_ADMIN_TOKEN (at least 32 characters, different from
BRIDGE_TOKEN). Never install this admin credential in the game container. Its
strict body contains operationKey, requestFingerprint, operator, reason,
evidence, isolationEvidence. This action queries the original upstream key,
verifies any canonical receipt, preserves evidence and records terminal
quarantine. Failed lookups do not terminalize. Absent lookups are not authority
to refund, release claims, or regrant. No financial movement is performed.
Terminal quarantine never means economic consumption or cleanup completed;
native phase updates and execute are then rejected, while audit reads remain
available. The operator is responsible for establishing physical isolation,
and the native adapter must observe the disposition before allowing unaffected
lifecycle progression. A token holder's asserted isolation is not runtime QA.

Run one backend process and one RaidLedger owner per database. Per-operation
execution and manual quarantine are serialized within that owner; this is not a
distributed lock. SQLite FULL/WAL transactions durably couple phase updates and
audit records, but multiple independently executing backend owners are not an
accepted topology. Backup the database with its WAL safely. Do not enable money
until native failure-path, disconnect/map/unload and physical quarantine QA pass.
