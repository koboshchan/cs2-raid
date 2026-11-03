CS2 Raid

Private starter for the Space City, seven-player free-for-all extraction mode. This is a buildable scaffold, not a finished game or a deployed server. Money movement is deliberately disabled until native delivery, inventory persistence and extraction verification are implemented. SSG money is simulation currency.

Setup

Use an x86-64 Linux Docker host with Compose v2, enough RAM for CS2, and ample disk for the current dedicated server install. The install is downloaded on first startup with SteamCMD app 730, not embedded in the image. A named cs2-data volume retains it between rebuilds. Never run docker compose down -v unless you intend to delete the install and backend ledger.

Copy .env.example to .env, restrict it to owner-readable permissions and fill the values. Generate separate random SERVER_PASSWORD, RCON_PASSWORD and BRIDGE_TOKEN values, for example using openssl rand -hex 24. Supply a GSLT appropriate for the server and an administrator-approved SSG client ID and full confidential app credential. The SSG secret is passed only to the backend, never the game plugin. Do not publish the backend port. Do not echo docker compose config with real credentials into a shared log.

The zip contains neither the Space City map nor its workshop ID. MAP defaults to de_dust2 for infrastructure smoke testing only. Install the agreed map and replace MAP before gameplay. Nothing here fabricates the map, locked-room/keycard logic, two levers or extraction zone.

Run docker compose build, then docker compose up -d. The first SteamCMD download can take a long time. Logs are available with docker compose logs -f server. An A2S health response proves only that the server answers, not that the plugin loaded or the rules work. Verify meta list and css_plugins list from the actual server console. Ports 27015/UDP and TCP are published; restrict RCON at your firewall to trusted addresses. GSLT can appear in the server process arguments as required by CS2; protect host access. Use private friend-server access, not an unrestricted public deployment.

UPDATE_ON_START=0 avoids repeated updates once the install exists. VALIDATE=1 forces validation on an update-enabled startup. Steam updates can break Metamod/CSS signatures; pinned frameworks must be reviewed after upgrades. Startup refreshes framework binaries and gameinfo.gi in the install volume, preserves framework configs and writes generated server credentials with mode 0600. It never launches the old DeltaOps DLL alongside the new plugin.

Included rules

The pure C# state model locks a roster of seven, runs a 30-minute raid, tracks two distinct levers and opens extraction five minutes after the second pull. Duplicate lever pulls do not shorten or reset the countdown. The logical backpack has no weapon-slot limit. Corpse transfers are one-shot, death/disconnect forfeits the bag, and timeout eliminates anyone who has not extracted. No kills generate money. Extraction keeps owned guns and creates a 90% sale calculation for looted guns, removing sold instances so they cannot be sold twice. This calculation is not an SSG credit or a persisted stash. The original owner's integer purchase cents are the sale basis; changing prices does not rewrite old item values.

config/prices.json contains all 42 supplied prices in simulation dollars. Backend snapshots convert to integer cents. config/raid.json documents intended settings; fixed rule constants in RaidState.cs currently enforce 7, 1800 seconds, 300 seconds and 90%, rather than loading arbitrary edits to that file. Do not mistake that file for a working map/engine configuration.

The C# plugin loads the JSON shop, provides !shop, !raid_status, !link and !linkpoll, and records death/disconnect in the active model. !link shows the four-word code privately to the authenticated player, and !linkpoll checks approval through the backend. No native controller/event handle crosses an asynchronous boundary. The internal bridge token is a trusted-server credential, not a player credential, and must never be sent to a client. !buy always refuses without making a charge.

Native testing commands css_raid_start and css_raid_lever are server-console-only. Start additionally requires RAID_ENABLE_SCAFFOLD=1, which is intentionally off in Compose. They exercise the model, not map extraction. There is no client cheat command for pulling levers, looting or extracting.

Remaining work

A mapper must supply Space City and authoritative named lever/zone hooks, open all keycard doors, and ensure no map-spawned loot exists. A native adapter must enforce identity/session/map/pawn generation fences, shop phase, death, disconnect, spectator handling and round setup. It must track purchased engine weapons, prevent native buy/pickup/default-loadout bypasses, validate corpse proximity, provide unlimited logical-bag equip/drop menus, strip death equipment and prove actual item delivery. Starting a model does not respawn or balance players or equip inventory. The server.cfg is a baseline, not proof of engine behavior.

Durable stash/corpse/inventory storage, single-use extraction receipts and an extraction settlement outbox are still required. Do not enable spending before those crash boundaries and the native delivery adapter are tested against the installed CSS build. The included purchase outbox is dormant and unit-tested, not connected to a live delivery implementation. Ambiguous delivery must remain unresolved for operator reconciliation, never blindly refunded or re-given.

SSG currently caps an operation at $1000, so many supplied guns are above the supported platform ceiling. This repo does not split a purchase into debits to evade that cap or silently change prices. Platform/admin policy changes and fresh player consent are needed before those purchases can work. Default daily/lifetime and app-wide limits also apply. Approved arbitrary minting and balance:credit consent are necessary for looted-gun sellback. Refunding the victim's debit cannot legally implement a reward to the killer. Sellback and all other money routes fail closed until this is reviewed and implemented.

Backend details and routes are documented in backend/README.md and the supplied SSG contract is in docs/ssg-app-api.md. Never infer a live SSG deployment or app approval from having those files.

Build and test

Build the plugin and run pure rules tests with docker build --target builder -t cs2-raid-builder:qa . . Build the backend with docker build -f backend/Dockerfile -t cs2-raid-backend:qa . . The final runner can be built without downloading CS2 using docker build --target runner -t cs2-raid:qa . . All test networking is mocked. No app registration, money mutation or gameplay QA is performed by builds.

Provenance

The supplied DeltaOps-Friend-Server zip was inspected and its C# source/docs retained under upstream/ for reference. It contains operator abilities, not this extraction ruleset; that old DLL is neither trusted as a binary nor installed. The runner scripts and dependency pins adapt the earlier deltaops-compose c6fac46 setup. CounterStrikeSharp 1.0.379 and Metamod 2.0.0-git1483 archives have explicit SHA-256 checks in Dockerfile. The original source has no license grant in the zip, so this private project does not relicense or publicly redistribute it. Check contributor permissions before changing visibility.
