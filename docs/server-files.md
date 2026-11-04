Server file inventory

Only source, Dockerfile/Compose, small custom cfg/json, checksum manifests and documentation are committed. There is one root Dockerfile; all plugin, map, backend and server targets live there. No operational script files are committed, including historical upstream wrappers. Literal startup/configuration/health and map-download logic is defined inline inside the Dockerfile and materialized only inside images where needed.

The map archive contains four compiled VPKs, one custom df_spaceport_server.cfg, documentation and launchers. Only that small cfg and the release/hash lock are retained in git. No original map launchers, editable Blender/VMAP files, CS2 game binaries or map VPKs are committed. The map cfg has the original 25-minute/sixteen-player package assumptions; actual raid startup uses deltaops-mode.cfg, seven players and the 30-minute model.

Downloads and exact pins

SDK 10.0.403-noble image digest de4fe90a98845d75910b9769662465c84de1a934665c431279d1382b2bddc02b.

CounterStrikeSharp 1.0.379 archive SHA256 bd7e52265346a66a3106934c1f1085b642ec5560b7f070a9b10be510341d583a.

Metamod 2.0.0-git1483 archive SHA256 0fdcccaa369937ed7f521c78462314b5266f39c1d65a84e4c2d3847324cd7776.

SteamCMD Linux bootstrap SHA256 cebf0046bfd08cf45da6bc094ae47aa39ebf4155e5ede41373b579b8f1071e7c. SteamCMD self-updates and CS2 app 730 content are mutable. The bootstrap checksum does not pin those future downloads. Ubuntu apt repositories are also mutable.

Ubuntu 24.04 image digest fe81560990fae86dc4743202b2cb4d3df98d0bcf949173cecef15ce1127ef852. Node 24-bookworm-slim image digest c86ecead9e55a5210ac1727de38a4ca840bf03dcaf34dc9749addbf383b1920c; backend npm dependencies use package-lock.json integrity metadata.

Map releases are resolved through the selected public GitHub repository and endpoint. The map stage verifies release asset digests and the SHA256SUMS manifest before reassembly and extracts only validated VPK/map cfg paths. See map-build.md for the exact bundle contract and caching. config/map-v1.lock.json remains a historical provenance receipt, not the active selector. No private token is passed in ARGs, URLs or layers. Remote ADD release discovery requires public access.

Validation boundaries

Both plugin targets were compiled twice independently with zero warnings/errors and identical DLL/deps/PDB hashes. RaidPlugin passed all 19 pure state checks each time. The backend target built with its 19 tests and TypeScript compilation. Root Dockerfile static checks and Compose validation were run with synthetic credentials only.

The final server target was not built, CS2 was not downloaded, and gameplay was not started. A prior local map-only Docker verification used the original archive and checked its full hash and five-file extraction. Network map-stage acceptance results are recorded separately from server acceptance; no authenticated/private release support is claimed. Current downloads can fail closed when upstream content changes; do not silently replace expected hashes.

Both assemblies are installed by the server target at the owner's request, not certified compatible with FFA. TeamNum targeting, free utility ownership, raw engine damage offsets, persistent inventory and extraction/lever hooks still need work. See plugins/DeltaOps/INTEGRATION.md.
