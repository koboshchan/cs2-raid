CS2 Raid

Space City seven-player FFA extraction scaffold and DeltaOps 1.3.2 operator abilities. This is not a finished raid mode or a deployed server. Purchases, native item delivery, persistent stash and sellback remain fail-closed. SSG currency is simulated.

Builds

All build and operational logic is inline in the single root Dockerfile. No committed host build/launch/configuration scripts exist. Use Linux AMD64 with modern Docker BuildKit. Plugin targets share only a pinned SDK/API dependency stage; deltaops-build and raid-build are sibling stages and neither depends on the other's source or compilation.

From the repository root, these exact commands independently compile twice, compare DLL/deps/PDB hashes and export separate artifacts. RaidPlugin also runs all 19 pure rules checks twice. Compilation steps have networking disabled and run as UID 1000; Docker must be isolated from production. Untrusted source compilation is still not a guarantee against hostile compiler inputs.

    docker build --platform=linux/amd64 --target deltaops-artifacts --output type=local,dest=/tmp/cs2-DeltaOps-artifacts .
    docker build --platform=linux/amd64 --target raid-artifacts --output type=local,dest=/tmp/cs2-RaidPlugin-artifacts .

For compiler-stage-only checks, use docker build --platform=linux/amd64 --target deltaops-build . or docker build --platform=linux/amd64 --target raid-build . . Use fresh output directories. Each export contains build logs, SHA256SUMS, source hashes, receipt.json and a deterministic tar.gz. Never commit the exports. CI uses the same independent artifact targets.

The verified build host uses a dedicated daemon, so the equivalent executed commands prefix docker with sudo docker -H unix:///var/run/cs2-raid/docker.sock and use /home/kit/plugin-artifacts/root-final-DeltaOps and root-final-RaidPlugin as output paths. Both plugin targets passed with zero warnings/errors and identical clean-build hashes. SDK workload verification may emit an informational notice; upstream also suppresses obsolete native damage warnings. Compilation does not prove game compatibility.

    docker build --target backend -t cs2-raid-backend:local .

This separate target runs backend unit tests and TypeScript compilation. It makes no SSG transactions. Backend source/test code is retained, not treated as an operational host script.

Server image, not yet accepted

    docker build --platform=linux/amd64 --target server -t cs2-raid:local .

Do not run this command just to test plugin compilation. The server target downloads the mutable CS2 app 730 through SteamCMD during image build, downloads the hash-locked map, installs Metamod/CSS and both built plugin assemblies, and defines the Docker-only startup command. It has NOT been built or gameplay-tested in this task because CS2 download/start was not authorized. Its packages, Steam self-updates and app content are not fully reproducible despite pinned framework and base-image checksums. No CS2 server was downloaded or started during acceptance.

For private map-release access, pass --secret id=github_token,env=GITHUB_TOKEN to the server build. Authentication uses a BuildKit secret mount only, never ARG/ENV or a committed token. Anonymous download works for the currently public repo. Do not put credentials in build arguments, URLs, Compose output or logs.

Copy .env.example to an owner-readable .env and fill separate random SERVER_PASSWORD, RCON_PASSWORD and BRIDGE_TOKEN values, an appropriate GSLT, and an approved SSG app credential. Compose is optional and targets server/backend in the same root Dockerfile. Starting it is a deployment, not part of build acceptance. The server image defines tini/inline startup logic, validates MAP/passwords, prepares gameinfo.gi and private cfg files, refreshes both plugin assemblies, and launches the map/cfg. A2S health only proves server response, not plugin correctness. Protect RCON and host process access; GSLT may appear in CS2 process arguments.

The cs2-data volume retains installation and configs. New volumes inherit the image's baked install; existing volumes may need an explicit operator-controlled Steam update. INSTALL_ON_START and UPDATE_ON_START default off. Never remove the volume to update casually. Framework updates can break engine signatures.

Both plugins will be installed by the requested server architecture, but that is NOT an integration safety approval. DeltaOps still treats TeamNum as alliance membership in FFA and grants free utility; inspect plugins/DeltaOps/INTEGRATION.md before any real use. No extraction/lever bridge exists between them. Enable neither spending nor real gameplay before that review and engine QA.

Included rules

The pure C# state model locks a roster of seven, runs a 30-minute raid, tracks two distinct levers and opens extraction five minutes after the second pull. Duplicate lever pulls do not shorten or reset the countdown. The logical backpack has no weapon-slot limit. Corpse transfers are one-shot, death/disconnect forfeits the bag, and timeout eliminates anyone who has not extracted. No kills generate money. Extraction keeps owned guns and creates a 90% sale calculation for looted guns, removing sold instances so they cannot be sold twice. This calculation is not an SSG credit or a persisted stash. The original owner's integer purchase cents are the sale basis; changing prices does not rewrite old item values.

config/prices.json contains all 42 supplied prices in simulation dollars. Backend snapshots convert to integer cents. config/raid.json documents intended settings; fixed rule constants in plugins/RaidPlugin/RaidState.cs currently enforce 7, 1800 seconds, 300 seconds and 90%, rather than loading arbitrary edits to that file. Do not mistake that file for a working map/engine configuration.

The C# plugin loads the JSON shop, provides !shop, !raid_status, !link and !linkpoll, and records death/disconnect in the active model. !link shows the four-word code privately to the authenticated player, and !linkpoll checks approval through the backend. No native controller/event handle crosses an asynchronous boundary. The internal bridge token is a trusted-server credential, not a player credential, and must never be sent to a client. !buy always refuses without making a charge.

Native testing commands css_raid_start and css_raid_lever are server-console-only. Start additionally requires RAID_ENABLE_SCAFFOLD=1, which is intentionally off in Compose. They exercise the model, not map extraction. There is no client cheat command for pulling levers, looting or extracting.

Remaining work

A mapper must supply Space City and authoritative named lever/zone hooks, open all keycard doors, and ensure no map-spawned loot exists. A native adapter must enforce identity/session/map/pawn generation fences, shop phase, death, disconnect, spectator handling and round setup. It must track purchased engine weapons, prevent native buy/pickup/default-loadout bypasses, validate corpse proximity, provide unlimited logical-bag equip/drop menus, strip death equipment and prove actual item delivery. Starting a model does not respawn or balance players or equip inventory. The server.cfg is a baseline, not proof of engine behavior.

Durable stash/corpse/inventory storage, single-use extraction receipts and an extraction settlement outbox are still required. Do not enable spending before those crash boundaries and the native delivery adapter are tested against the installed CSS build. The included purchase outbox is dormant and unit-tested, not connected to a live delivery implementation. Ambiguous delivery must remain unresolved for operator reconciliation, never blindly refunded or re-given.

SSG currently caps an operation at $1000, so many supplied guns are above the supported platform ceiling. This repo does not split a purchase into debits to evade that cap or silently change prices. Platform/admin policy changes and fresh player consent are needed before those purchases can work. Default daily/lifetime and app-wide limits also apply. Approved arbitrary minting and balance:credit consent are necessary for looted-gun sellback. Refunding the victim's debit cannot legally implement a reward to the killer. Sellback and all other money routes fail closed until this is reviewed and implemented.

Backend details and routes are documented in backend/README.md and the supplied SSG contract is in docs/ssg-app-api.md. Never infer a live SSG deployment or app approval from having those files.

Provenance and repository contents

plugins/DeltaOps contains the five retained upstream files from the supplied 1.3.2 ZIP and source/build receipts. The repository-root LICENSE covers the project under MIT, as authorized by the owner. The original build.ps1 is intentionally omitted. plugins/RaidPlugin contains the extraction source and its tests. Historical upstream wrappers were also removed at the owner's request; historical game/source docs remain as reference only, not current startup instructions. See plugins/DeltaOps/PROVENANCE.md.

The live API confirmed koboshchan/cs2-raid is PUBLIC, not private as earlier notes assumed. Visibility was not changed. Source publication waited for explicit MIT authorization. The owner subsequently authorized MIT licensing for the whole project, including the retained legacy project source. Third-party dependencies and downloaded map assets retain their own licenses. No downloaded binaries, runtimes, VPKs, ZIPs, release parts, secrets or build outputs are committed. See docs/server-files.md.

License

The whole project is licensed under the [MIT License](LICENSE), including both plugins, backend, build/configuration files, documentation and retained legacy project source. Existing author credits are preserved. Third-party dependencies and downloaded assets, including CounterStrikeSharp, Metamod, SteamCMD/CS2 and map assets, retain their own licenses and are not relicensed by this notice.
