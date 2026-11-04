# Cached map release build

The map is an independent Docker build branch. It does not rebuild the plugins or download/start CS2. The default is the latest published public release of `oceanwang525-creator/df-spaceport-map`.

```sh
docker build --target map-artifacts --output type=local,dest=artifacts/map .
docker compose build server
```

`MAP_REPO` defaults to Ocean's public repository in both Dockerfile and Compose. `MAP_TAG` is an API endpoint selector, not a plain Git tag. Use `latest` (the default) or `tags/map-v1` to pin the existing release.

```sh
MAP_REPO=koboshchan/cs2-raid MAP_TAG=tags/map-v1 docker compose build server
docker build --target map-artifacts \
  --build-arg MAP_REPO=koboshchan/cs2-raid \
  --build-arg MAP_TAG=tags/map-v1 \
  --output type=local,dest=artifacts/map .
```

A remote `ADD` reads GitHub's release JSON at build time. BuildKit revalidates that URL; the JSON content is the cache input to the download/extract layer. A changed release or asset digest changes the metadata and invalidates that layer. Unchanged metadata permits reuse without redownloading the multi-gigabyte assets. Docker does not include the remote file's Last-Modified timestamp in its ADD cache checksum. Run a new image build to check for updates; restarting an existing container does not query GitHub. Normal Docker/BuildKit remote HTTP cache revalidation still applies.

Remote ADD has no GitHub authentication support here. Both the repository and release assets must be publicly accessible. Private releases and the previous `github_token` BuildKit-secret flow are unsupported for this map stage. No token is accepted, sent, or baked into the image. GitHub's unauthenticated API rate limits apply.

## Release layout and integrity

Publish exactly one ZIP or one set of shards named `<archive>.zip.part001`, `<archive>.zip.part002`, and so on. Shards must use equal-width numeric suffixes, start at 1, and have no gaps. Their order in GitHub's JSON does not matter. Do not publish a full ZIP alongside its shards or multiple map archives in one release.

Optionally publish `SHA256SUMS`, in standard `sha256sum` format, with an entry for every downloaded ZIP/shard. An optional entry for the joined ZIP is verified too. Every available GitHub `sha256:` asset digest is verified; malformed or mismatching digests fail closed. Missing digests are allowed, with full ZIP CRC testing as the fallback. The builder checks download sizes, available GitHub digests, manifest entries when supplied, and the optional joined-archive hash. It always tests CRC integrity for every ZIP member before extracting selected files. CRC alone detects corruption, not authenticity. It permits only HTTPS downloads from the requested GitHub repository and known GitHub release-asset redirect hosts.

The current map-v1 release has six shards and the joined archive hash `c0ec824ac4b05359412251411cb38b0861a7111c8b12c0f1865eb84044d01f62`. The historical `config/map-v1.lock.json` is not used to pin the dynamic release stage. Each build exports its resolved tag, release ID, archive hash, and extracted-file hashes in `map-release.json`.

Digests detect corruption and disagreement, not a malicious release publisher. Use a trusted public repository and pin a tag when needed; GitHub tags/releases can still be edited.

## Extraction and output

Accepted archives contain `game/csgo/maps/*.vpk` and optionally `game/csgo/cfg/*.cfg`, either directly or beneath exactly one packaging directory. Only those direct files are staged. Scripts, plugins, binaries outside maps, nested files, and unrelated members are never installed. Map cfg files are trusted server configuration, so review the publisher's config as well as its VPKs.

Directory ZIP entries are accepted. Absolute/traversal/backslash/drive paths, duplicate members or output paths, symlinks, special files, encrypted members, missing VPKs, and oversized archives fail the build. Limits are 16 GiB compressed, 32 GiB selected extracted files, and 10,000 ZIP entries. Extracted files have mode 0644. Python's ZIP reader verifies each extracted member's CRC while streaming.

`map-download` exposes `/output/game` and `/output/map-release.json`. The `map-artifacts` target exports these without the downloads or builder runtime. The server stage can copy this output independently of its plugin build layers. The startup integration must refresh persistent CS2 volumes from the baked map tree, since a mounted volume hides image files.

## Verification evidence

The real public map-v1 release was built through the default latest selector using the isolated build daemon. All six asset digests and manifest entries passed, as did the joined archive hash. The export contained exactly four VPKs, one cfg, and the receipt; exported file hashes were independently rechecked. A second identical build revalidated the JSON ADD and reported CACHED for the ADD, Python download/extract RUN, and artifact COPY. No archive assets were fetched again. Twenty-two synthetic success/failure cases cover shard ordering, pinned selectors, full ZIPs, directory entries, optional archive hashes, malformed or missing integrity metadata, corruption, traversal, symlinks, external URLs, duplicates, and missing maps. Evidence logs are in `/home/kit/map-cache-evidence/` on the build host. No CS2 server was downloaded or started by these map tests.

## Ocean release verification

Default `latest` resolved to `map-build-20261104`, five `df_spaceport_full40_latest.zip.part01` through `part05` assets. No separate checksum asset; all five GitHub SHA-256 digests verified. Joined ZIP SHA-256 `76685c8cb31206a89768c86c50fcd5ce5f385949ab3e6ba7804ce24daebc9204`. Real map-artifacts build passed ZIP integrity and exported 4,985 files (3,115,483,051 bytes). A repeat build reused the release-JSON/download/extraction layers. Nineteen synthetic layout/integrity/security checks and Docker build check passed. No server build/start/deploy.

Ocean's `./addon/` layout is translated to `game/csgo/`, preserving nested map configs and compiled models/materials needed by the map. Only map VPKs, CFGs, and allowlisted compiled model/material assets are copied; source files and launch scripts are omitted. Leading `./` and Windows separators are normalized before traversal checks. Legacy `game/csgo/` packages remain supported.
