# syntax=docker/dockerfile:1
ARG INCLUDE_DELTAOPS=disabled
FROM mcr.microsoft.com/dotnet/sdk:10.0.403-noble@sha256:de4fe90a98845d75910b9769662465c84de1a934665c431279d1382b2bddc02b AS dependencies
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 DOTNET_NOLOGO=1
WORKDIR /deps
RUN curl -fL --retry 3 https://github.com/roflmuffin/CounterStrikeSharp/releases/download/v1.0.379/counterstrikesharp-with-runtime-linux-1.0.379.zip -o css.zip \
 && echo "bd7e52265346a66a3106934c1f1085b642ec5560b7f070a9b10be510341d583a  css.zip" | sha256sum -c - \
 && pwsh -NoLogo -NoProfile -Command 'Expand-Archive -LiteralPath /deps/css.zip -DestinationPath /deps/css' \
 && rm css.zip
FROM dependencies AS deltaops-build
ENV HOME=/work/home DOTNET_CLI_HOME=/work/home NUGET_PACKAGES=/work/nuget
RUN --network=none mkdir /work && chown 1000:1000 /work
COPY --chown=1000:1000 plugins/DeltaOps/DeltaOps.csproj plugins/DeltaOps/DeltaOpsConfig.cs plugins/DeltaOps/DeltaOpsPlugin.cs plugins/DeltaOps/RaidCapabilityBridge.cs plugins/DeltaOps/config.example.json plugins/DeltaOps/source.sha256 /input/
USER 1000:1000
WORKDIR /work
RUN --network=none <<'BUILD'
set -eu
test "$(dotnet --version)" = "10.0.403"
mkdir -p /work/home /work/nuget /work/artifacts
cd /input
sha256sum -c source.sha256
cat > /work/NuGet.Config <<'EOF'
<configuration><packageSources><clear /></packageSources></configuration>
EOF
for n in 1 2; do
  mkdir -p /work/run$n/src /work/artifacts/build-$n
  cp /input/DeltaOps.csproj /input/DeltaOpsConfig.cs /input/DeltaOpsPlugin.cs /input/RaidCapabilityBridge.cs /work/run$n/src/
  dotnet build /work/run$n/src/DeltaOps.csproj -c Release --configfile /work/NuGet.Config \
    -o /work/artifacts/build-$n -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api \
    -p:Deterministic=true -p:ContinuousIntegrationBuild=true -p:PathMap=/work/run$n/src=/_/deltaops \
    -p:UseSharedCompilation=false -p:TreatWarningsAsErrors=true > /work/artifacts/build-$n.log 2>&1 || { cat /work/artifacts/build-$n.log; exit 1; }
  cat /work/artifacts/build-$n.log
  grep -q '0 Warning(s)' /work/artifacts/build-$n.log
  grep -q '0 Error(s)' /work/artifacts/build-$n.log
done
for ext in dll deps.json pdb; do
  cmp /work/artifacts/build-1/DeltaOps.$ext /work/artifacts/build-2/DeltaOps.$ext
done
cd /work/artifacts/build-1
sha256sum DeltaOps.dll DeltaOps.deps.json DeltaOps.pdb > ../SHA256SUMS
tar --sort=name --mtime=@0 --owner=0 --group=0 --numeric-owner --mode=644 -cf - DeltaOps.dll DeltaOps.deps.json DeltaOps.pdb | gzip -n > ../DeltaOps-1.3.2-build.tar.gz
cd ..
sha256sum DeltaOps-1.3.2-build.tar.gz >> SHA256SUMS
cp /input/source.sha256 source.sha256
cat > receipt.json <<EOF
{
  "plugin": "DeltaOps",
  "sdk": "mcr.microsoft.com/dotnet/sdk:10.0.403-noble@sha256:de4fe90a98845d75910b9769662465c84de1a934665c431279d1382b2bddc02b",
  "css_version": "1.0.379",
  "css_zip_sha256": "bd7e52265346a66a3106934c1f1085b642ec5560b7f070a9b10be510341d583a",
  "build_count": 2,
  "warnings_per_build": 0,
  "errors_per_build": 0,
  "deterministic_match": true,
  "raid_rule_checks_per_build": 0,
  "archive_sha256": "$(sha256sum DeltaOps-1.3.2-build.tar.gz | cut -d ' ' -f1)",
  "dll_sha256": "$(sha256sum build-1/DeltaOps.dll | cut -d ' ' -f1)",
  "deps_json_sha256": "$(sha256sum build-1/DeltaOps.deps.json | cut -d ' ' -f1)",
  "pdb_sha256": "$(sha256sum build-1/DeltaOps.pdb | cut -d ' ' -f1)",
  "source_zip_sha256": "465fdb7b8d36a03d9d819ab9303ee08e518d66d27c004df78e63c9483d9c8030",
  "api_dll_sha256": "$(sha256sum /deps/css/addons/counterstrikesharp/api/CounterStrikeSharp.API.dll | cut -d ' ' -f1)",
  "logging_dll_sha256": "$(sha256sum /deps/css/addons/counterstrikesharp/api/Microsoft.Extensions.Logging.Abstractions.dll | cut -d ' ' -f1)",
  "plugin_executed": false,
  "server_started": false,
  "deployed": false
}
EOF
cat receipt.json
BUILD
FROM scratch AS deltaops-artifacts
COPY --from=deltaops-build /work/artifacts/ /
FROM dependencies AS raid-build
ENV HOME=/work/home DOTNET_CLI_HOME=/work/home NUGET_PACKAGES=/work/nuget
RUN --network=none mkdir /work && chown 1000:1000 /work
COPY --chown=1000:1000 plugins/RaidPlugin/RaidPlugin.csproj plugins/RaidPlugin/RaidPlugin.cs plugins/RaidPlugin/RaidState.cs plugins/RaidPlugin/GameplayState.cs plugins/RaidPlugin/GameplayAdapter.cs plugins/RaidPlugin/PartnerVisuals.cs plugins/RaidPlugin/PurchasePricing.cs plugins/RaidPlugin/TestMoneyHandler.cs plugins/RaidPlugin/TestMode.cs plugins/RaidPlugin/TestFlow.cs plugins/RaidPlugin/TestModeState.cs plugins/RaidPlugin/TonightGameplay.cs plugins/RaidPlugin/TonightRules.cs plugins/RaidPlugin/DeltaOpsBridge.cs plugins/RaidPlugin/SettlementProtocol.cs plugins/RaidPlugin/SettlementBridge.cs plugins/RaidPlugin/source.sha256 /input/
COPY --chown=1000:1000 plugins/RaidPlugin/tests/ /input/tests/
USER 1000:1000
WORKDIR /work
RUN --network=none <<'BUILD'
set -eu
test "$(dotnet --version)" = "10.0.403"
mkdir -p /work/home /work/nuget /work/artifacts
cd /input
sha256sum -c source.sha256
cat > /work/NuGet.Config <<'EOF'
<configuration><packageSources><clear /></packageSources></configuration>
EOF
for n in 1 2; do
  mkdir -p /work/run$n/src /work/artifacts/build-$n
  cp /input/RaidPlugin.csproj /input/RaidPlugin.cs /input/RaidState.cs /input/GameplayState.cs /input/GameplayAdapter.cs /input/PartnerVisuals.cs /input/PurchasePricing.cs /input/TestMoneyHandler.cs /input/TestMode.cs /input/TestFlow.cs /input/TestModeState.cs /input/TonightGameplay.cs /input/TonightRules.cs /input/DeltaOpsBridge.cs /input/SettlementProtocol.cs /input/SettlementBridge.cs /work/run$n/src/; cp -R /input/tests /work/run$n/src/
  dotnet build /work/run$n/src/RaidPlugin.csproj -c Release --configfile /work/NuGet.Config \
    -o /work/artifacts/build-$n -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api \
    -p:Deterministic=true -p:ContinuousIntegrationBuild=true -p:PathMap=/work/run$n/src=/_/raidplugin \
    -p:UseSharedCompilation=false -p:TreatWarningsAsErrors=true > /work/artifacts/build-$n.log 2>&1 || { cat /work/artifacts/build-$n.log; exit 1; }
  cat /work/artifacts/build-$n.log
  grep -q '0 Warning(s)' /work/artifacts/build-$n.log
  grep -q '0 Error(s)' /work/artifacts/build-$n.log
  dotnet restore /work/run$n/src/tests/RaidState.Tests.csproj --configfile /work/NuGet.Config >> /work/artifacts/build-$n.log 2>&1
  dotnet build /work/run$n/src/tests/RaidState.Tests.csproj -c Release --no-restore -p:TreatWarningsAsErrors=true >> /work/artifacts/build-$n.log 2>&1
  dotnet /work/run$n/src/tests/bin/Release/net10.0/RaidState.Tests.dll >> /work/artifacts/build-$n.log 2>&1
  grep '19 raid rule checks passed' /work/artifacts/build-$n.log
  grep 'flow v2 checks passed' /work/artifacts/build-$n.log
done
for ext in dll deps.json pdb; do
  cmp /work/artifacts/build-1/RaidPlugin.$ext /work/artifacts/build-2/RaidPlugin.$ext
done
cd /work/artifacts/build-1
sha256sum RaidPlugin.dll RaidPlugin.deps.json RaidPlugin.pdb > ../SHA256SUMS
tar --sort=name --mtime=@0 --owner=0 --group=0 --numeric-owner --mode=644 -cf - RaidPlugin.dll RaidPlugin.deps.json RaidPlugin.pdb | gzip -n > ../RaidPlugin-build.tar.gz
cd ..
sha256sum RaidPlugin-build.tar.gz >> SHA256SUMS
cp /input/source.sha256 source.sha256
cat > receipt.json <<EOF
{
  "plugin": "RaidPlugin",
  "sdk": "mcr.microsoft.com/dotnet/sdk:10.0.403-noble@sha256:de4fe90a98845d75910b9769662465c84de1a934665c431279d1382b2bddc02b",
  "css_version": "1.0.379",
  "css_zip_sha256": "bd7e52265346a66a3106934c1f1085b642ec5560b7f070a9b10be510341d583a",
  "build_count": 2,
  "warnings_per_build": 0,
  "errors_per_build": 0,
  "deterministic_match": true,
  "raid_rule_checks_per_build": 19,
  "flow_v2_checks_per_build": "see build logs",
  "archive_sha256": "$(sha256sum RaidPlugin-build.tar.gz | cut -d ' ' -f1)",
  "dll_sha256": "$(sha256sum build-1/RaidPlugin.dll | cut -d ' ' -f1)",
  "deps_json_sha256": "$(sha256sum build-1/RaidPlugin.deps.json | cut -d ' ' -f1)",
  "pdb_sha256": "$(sha256sum build-1/RaidPlugin.pdb | cut -d ' ' -f1)",
  "source_zip_sha256": "not applicable",
  "api_dll_sha256": "$(sha256sum /deps/css/addons/counterstrikesharp/api/CounterStrikeSharp.API.dll | cut -d ' ' -f1)",
  "logging_dll_sha256": "$(sha256sum /deps/css/addons/counterstrikesharp/api/Microsoft.Extensions.Logging.Abstractions.dll | cut -d ' ' -f1)",
  "plugin_executed": false,
  "server_started": false,
  "deployed": false
}
EOF
cat receipt.json
BUILD
FROM scratch AS raid-artifacts
COPY --from=raid-build /work/artifacts/ /

FROM dependencies AS debug-build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 HOME=/work/home DOTNET_CLI_HOME=/work/home
RUN --network=none mkdir -p /work/home && chown -R 1000:1000 /work
WORKDIR /work
COPY --chown=1000:1000 plugins/Debug/ /work/src/
USER 1000:1000
RUN --network=none <<'BUILD'
set -eu
mkdir -p /work/artifacts
printf '<configuration><packageSources><clear /></packageSources></configuration>\n' > /work/NuGet.Config
for n in 1 2; do
  mkdir -p /work/debug-run$n
  cp -R /work/src/. /work/debug-run$n/
  dotnet build /work/debug-run$n/Debug.csproj -c Release --configfile /work/NuGet.Config -o /work/artifacts/build-$n -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api -p:TreatWarningsAsErrors=true -p:UseSharedCompilation=false -p:Deterministic=true -p:ContinuousIntegrationBuild=true -p:PathMap=/work/debug-run$n=/_/debug
  dotnet restore /work/debug-run$n/tests/Debug.Tests.csproj --configfile /work/NuGet.Config
  dotnet build /work/debug-run$n/tests/Debug.Tests.csproj -c Release --no-restore -p:TreatWarningsAsErrors=true
  dotnet /work/debug-run$n/tests/bin/Release/net10.0/Debug.Tests.dll
done
for ext in dll deps.json pdb; do cmp /work/artifacts/build-1/Debug.$ext /work/artifacts/build-2/Debug.$ext; done
cp -R /work/artifacts/build-1 /work/artifacts/plugin
cd /work/artifacts/plugin
sha256sum Debug.dll Debug.deps.json Debug.pdb > ../SHA256SUMS
BUILD
FROM scratch AS debug-artifacts
COPY --from=debug-build /work/artifacts/ /

FROM dependencies AS server-dependencies
RUN curl -fL --retry 3 https://mms.alliedmods.net/mmsdrop/2.0/mmsource-2.0.0-git1483-linux.tar.gz -o /deps/metamod.tar.gz \
 && echo "0fdcccaa369937ed7f521c78462314b5266f39c1d65a84e4c2d3847324cd7776  /deps/metamod.tar.gz" | sha256sum -c - \
 && mkdir /deps/metamod && tar -xzf /deps/metamod.tar.gz -C /deps/metamod && rm /deps/metamod.tar.gz

FROM python:3.13-slim-bookworm@sha256:281d39ff02e3b389401586134ff9e6572954687822a58055ec07a60ded4e0b7f AS map-tools
# MAP_TAG is a GitHub release endpoint selector: latest or tags/<release-tag>.
ARG MAP_REPO=oceanwang525-creator/df-spaceport-map
ARG MAP_TAG=latest

FROM map-tools AS map-download
# BuildKit revalidates remote ADD. Changed JSON invalidates ONLY the map branch.
ADD https://api.github.com/repos/${MAP_REPO}/releases/${MAP_TAG} /input/release.json
RUN MAP_REPO="${MAP_REPO}" MAP_TAG="${MAP_TAG}" python3 - <<'MAP_FETCH'
import hashlib
import json
import os
from pathlib import Path, PurePosixPath
import re
import shutil
import stat
import urllib.parse
import urllib.request
import zipfile

repo = os.environ['MAP_REPO']
selector = os.environ['MAP_TAG']
if not re.fullmatch(r'[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+', repo):
    raise ValueError('MAP_REPO must be owner/repository')
if selector != 'latest' and not re.fullmatch(r'tags/[A-Za-z0-9_.-]+', selector):
    raise ValueError('MAP_TAG must be latest or tags/<release-tag>')
release = json.loads(Path('/input/release.json').read_text())
if not isinstance(release.get('tag_name'), str) or release.get('draft'):
    raise ValueError('Expected a published GitHub release')
if selector != 'latest' and release['tag_name'] != selector.removeprefix('tags/'):
    raise ValueError('Release tag does not match requested selector')
work = Path('/work/map')
output = Path('/output')
work.mkdir(parents=True, exist_ok=True)
output.mkdir(parents=True, exist_ok=True)
MAX_ARCHIVE = 16 * 1024**3
MAX_EXTRACTED = 32 * 1024**3

def safe_name(name):
    if not isinstance(name, str) or not re.fullmatch(r'[A-Za-z0-9_][A-Za-z0-9_.-]*', name):
        raise ValueError('Unsafe release asset or checksum filename')
    return name

def sha(path):
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()

assets = {}
for asset in release.get('assets', []):
    name = safe_name(asset['name'])
    if name in assets:
        raise ValueError('Duplicate release asset')
    assets[name] = asset
parts = []
archives = []
for name in assets:
    match = re.fullmatch(r'(.+\.zip)\.part([0-9]+)', name)
    if match:
        parts.append((match[1], match[2], name))
    elif name.endswith('.zip'):
        archives.append(name)
if parts:
    bases = {p[0] for p in parts}
    widths = {len(p[1]) for p in parts}
    if len(bases) != 1 or len(widths) != 1 or archives:
        raise ValueError('Expected exactly one unambiguous ZIP shard set')
    parts.sort(key=lambda p: int(p[1]))
    if [int(p[1]) for p in parts] != list(range(1, len(parts) + 1)):
        raise ValueError('ZIP shards must be contiguous starting at 1')
    archive_name = parts[0][0]
    downloads = [p[2] for p in parts]
else:
    if len(archives) != 1:
        raise ValueError('Expected exactly one ZIP archive or shard set')
    archive_name = archives[0]
    downloads = [archive_name]

# Reject insecure redirects as well as insecure initial URLs. No credentials are used.
allowed_hosts = {'github.com', 'api.github.com', 'release-assets.githubusercontent.com', 'objects.githubusercontent.com'}
def check_url(url):
    parsed = urllib.parse.urlsplit(url)
    if parsed.scheme != 'https' or parsed.hostname not in allowed_hosts or parsed.username or parsed.password or parsed.port not in (None, 443):
        raise ValueError('Unexpected release asset URL or redirect')

class SecureRedirect(urllib.request.HTTPRedirectHandler):
    def redirect_request(self, request, fp, code, msg, headers, newurl):
        check_url(newurl)
        return super().redirect_request(request, fp, code, msg, headers, newurl)

opener = urllib.request.build_opener(SecureRedirect())
def fetch(name, limit):
    asset = assets[name]
    expected = asset.get('digest')
    if expected is not None and (not isinstance(expected, str) or not re.fullmatch(r'sha256:[0-9a-fA-F]{64}', expected)):
        raise ValueError('Invalid GitHub SHA256 digest for ' + name)
    size = asset.get('size')
    if type(size) is not int or not 0 < size <= limit:
        raise ValueError('Invalid or excessive asset size')
    url = asset['browser_download_url']
    check_url(url)
    parsed = urllib.parse.urlsplit(url)
    if parsed.hostname != 'github.com' or not parsed.path.startswith('/' + repo + '/releases/download/'):
        raise ValueError('Asset is outside requested release repository')
    path = work / name
    request = urllib.request.Request(url, headers={'User-Agent': 'cs2-raid-map-builder', 'Accept': 'application/octet-stream'})
    count = 0
    with opener.open(request, timeout=120) as response, path.open('xb') as target:
        while chunk := response.read(1024 * 1024):
            count += len(chunk)
            if count > size:
                raise ValueError('Asset exceeds declared size')
            target.write(chunk)
    if count != size or (expected is not None and sha(path) != expected.removeprefix('sha256:').lower()):
        raise ValueError('GitHub asset size or SHA256 mismatch for ' + name)
    return path

checksums = {}
if 'SHA256SUMS' in assets:
    manifest_path = fetch('SHA256SUMS', 1024 * 1024)
    for line in manifest_path.read_text(encoding='utf-8').splitlines():
        if not line.strip():
            continue
        match = re.fullmatch(r'([0-9a-fA-F]{64}) [ *](.+)', line)
        if not match:
            raise ValueError('Malformed SHA256SUMS entry')
        name = safe_name(match[2])
        if name in checksums:
            raise ValueError('Duplicate SHA256SUMS entry')
        checksums[name] = match[1].lower()
    for name in downloads:
        if name not in checksums:
            raise ValueError('Missing SHA256SUMS entry for ' + name)
        digest = assets[name].get('digest')
        if digest is not None and checksums[name] != digest.removeprefix('sha256:').lower():
            raise ValueError('SHA256SUMS and GitHub digest disagree for ' + name)
if sum(assets[name]['size'] for name in downloads) > MAX_ARCHIVE:
    raise ValueError('Map archive exceeds size limit')
archive_path = work / 'assembled.zip'
with archive_path.open('xb') as joined:
    for name in downloads:
        path = fetch(name, MAX_ARCHIVE)
        if name in checksums and sha(path) != checksums[name]:
            raise ValueError('SHA256SUMS mismatch for ' + name)
        with path.open('rb') as source:
            shutil.copyfileobj(source, joined, 1024 * 1024)
        path.unlink()
archive_hash = sha(archive_path)
if archive_name in checksums and archive_hash != checksums[archive_name]:
    raise ValueError('Reassembled archive SHA256 mismatch')

selected = []
seen = set()
with zipfile.ZipFile(archive_path) as bundle:
    if len(bundle.infolist()) > 10000:
        raise ValueError('Too many ZIP entries')
    for entry in bundle.infolist():
        # Windows ZIP writers use backslash separators; validate AFTER normalization.
        raw = entry.filename.replace(chr(92), "/")
        # ZIPs created from the current directory commonly include ./ prefixes.
        if raw in (".", "./") and entry.is_dir():
            continue
        while raw.startswith("./"):
            raw = raw[2:]
        path = PurePosixPath(raw)
        if '\\' in raw or '\x00' in raw or path.is_absolute() or any(p in ('', '.', '..') for p in raw.rstrip('/').split('/')) or any(':' in p for p in path.parts):
            raise ValueError('Unsafe ZIP member path: ' + repr(entry.filename))
        mode = entry.external_attr >> 16
        if stat.S_IFMT(mode) not in (0, stat.S_IFREG, stat.S_IFDIR) or entry.flag_bits & 1:
            raise ValueError('Links, special files and encrypted ZIP entries are not allowed')
        if raw.casefold() in seen:
            raise ValueError('Duplicate ZIP member')
        seen.add(raw.casefold())
        if entry.is_dir():
            continue
        # Support legacy game/csgo packaging and Ocean's loose addon tree.
        parts = path.parts
        if parts[0] == 'addon':
            parts = ('game', 'csgo') + parts[1:]
        elif len(parts) >= 5 and parts[1:3] == ('game', 'csgo'):
            parts = parts[1:]
        if len(parts) < 4 or parts[:2] != ('game', 'csgo'):
            continue
        kind, name = parts[2], parts[-1]
        if not all(re.fullmatch(r'[A-Za-z0-9_][A-Za-z0-9_.-]*', component) for component in parts):
            raise ValueError('Unsafe map filename')
        permitted = ((kind == 'maps' and len(parts) == 4 and name.endswith('.vpk')) or
                     (kind == 'cfg' and name.endswith('.cfg')) or
                     (kind in ('models', 'materials') and name.endswith(('.vmdl_c', '.vmesh_c', '.vphys_c', '.vmat_c', '.vtex_c'))))
        if not permitted:
            continue
        dest = Path(*parts)
        if any(dest == prior[1] for prior in selected):
            raise ValueError('Duplicate output path')
        selected.append((entry, dest))
    if not any(dest.suffix == '.vpk' for _, dest in selected):
        raise ValueError('No map VPK files found')
    if sum(entry.file_size for entry in bundle.infolist()) > MAX_EXTRACTED:
        raise ValueError('ZIP integrity test exceeds uncompressed size limit')
    # Validate CRC of EVERY member, including files deliberately not extracted.
    bad_member = bundle.testzip()
    if bad_member is not None:
        raise ValueError('ZIP integrity failure: ' + bad_member)
    if sum(entry.file_size for entry, _ in selected) > MAX_EXTRACTED:
        raise ValueError('Map extraction exceeds size limit')
    extracted = []
    for entry, relative in selected:
        dest = output / relative
        dest.parent.mkdir(parents=True, exist_ok=True)
        with bundle.open(entry) as source, dest.open('xb') as target:
            count = 0
            while chunk := source.read(1024 * 1024):
                count += len(chunk)
                if count > entry.file_size:
                    raise ValueError('ZIP member exceeds declared size')
                target.write(chunk)
        if count != entry.file_size:
            raise ValueError('Truncated ZIP member')
        dest.chmod(0o644)
        extracted.append({'path': relative.as_posix(), 'bytes': count, 'sha256': sha(dest)})
receipt = {'repo': repo, 'tag': release['tag_name'], 'release_id': release.get('id'), 'archive': archive_name,
           'archive_sha256': archive_hash, 'archive_checksum_present': archive_name in checksums,
           'release_json_sha256': sha(Path('/input/release.json')), 'zip_integrity_tested': True,
           'asset_verification': [{'name': name, 'github_digest_verified': assets[name].get('digest') is not None,
                                   'manifest_verified': name in checksums} for name in downloads],
           'files': extracted}
(output / 'map-release.json').write_text(json.dumps(receipt, indent=2) + '\n')
print(json.dumps(receipt, indent=2))
shutil.rmtree(work)
MAP_FETCH
FROM scratch AS map-artifacts
COPY --from=map-download /output/ /
FROM node:24-bookworm-slim@sha256:c86ecead9e55a5210ac1727de38a4ca840bf03dcaf34dc9749addbf383b1920c AS backend-build
WORKDIR /app
COPY backend/package.json backend/package-lock.json ./
RUN npm ci --no-audit --no-fund
COPY backend/tsconfig.json ./
COPY backend/src ./src
RUN npm test && npm run build && npm prune --omit=dev
FROM node:24-bookworm-slim@sha256:c86ecead9e55a5210ac1727de38a4ca840bf03dcaf34dc9749addbf383b1920c AS backend
ENV NODE_ENV=production DATA_DIR=/data PORT=3000
WORKDIR /app
COPY --from=backend-build --chown=node:node /app/node_modules ./node_modules
COPY --from=backend-build --chown=node:node /app/dist ./dist
COPY --chown=node:node config/prices.json ./config/prices.json
RUN mkdir /data && chown node:node /data && chmod 700 /data
USER node
VOLUME ["/data"]
EXPOSE 3000
CMD ["node","dist/server.js"]
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 CMD node -e 'fetch("http://127.0.0.1:"+(process.env.PORT||3000)+"/health",{headers:{authorization:"Bearer "+process.env.BRIDGE_TOKEN}}).then(r=>{if(!r.ok)process.exit(1)}).catch(()=>process.exit(1))'

# Selecting disabled creates no dependency on deltaops-build.
FROM scratch AS deltaops-disabled
COPY <<'EMPTY' /optional/.disabled
DeltaOps excluded
EMPTY
FROM scratch AS deltaops-enabled
COPY --from=deltaops-build /work/artifacts/build-1/DeltaOps.dll /work/artifacts/build-1/DeltaOps.deps.json /optional/
FROM deltaops-${INCLUDE_DELTAOPS} AS deltaops-selected

FROM ubuntu:24.04@sha256:fe81560990fae86dc4743202b2cb4d3df98d0bcf949173cecef15ce1127ef852 AS server
ENV DEBIAN_FRONTEND=noninteractive HOME=/home/steam
RUN dpkg --add-architecture i386 \
 && apt-get update && apt-get install -y --no-install-recommends \
 ca-certificates curl tar python3 tini lib32gcc-s1 lib32stdc++6 \
 libicu74 libssl3t64 libunwind8 zlib1g libstdc++6 libgcc-s1 \
 libsdl2-2.0-0 libcurl4t64 libatomic1 libtinfo6 libcurl3t64-gnutls \
 && rm -rf /var/lib/apt/lists/* \
 && useradd --create-home --uid 1001 --shell /bin/bash steam \
 && mkdir -p /opt/steamcmd /home/steam/cs2 \
 && chown -R steam:steam /opt/steamcmd /home/steam
# SteamCMD bootstraps/updates itself on first start, not during image build.
RUN curl -fL --retry 3 https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz -o /tmp/steamcmd.tar.gz \
 && echo "cebf0046bfd08cf45da6bc094ae47aa39ebf4155e5ede41373b579b8f1071e7c  /tmp/steamcmd.tar.gz" | sha256sum -c - \
 && tar -xzf /tmp/steamcmd.tar.gz -C /opt/steamcmd && rm /tmp/steamcmd.tar.gz
RUN chown -R steam:steam /opt/steamcmd
COPY --from=dependencies /deps/css/ /opt/deltaops/css/
COPY --from=server-dependencies /deps/metamod/ /opt/deltaops/metamod/
COPY --from=raid-build /work/artifacts/build-1/RaidPlugin.dll /work/artifacts/build-1/RaidPlugin.deps.json /opt/deltaops/plugins/RaidPlugin/
COPY --from=deltaops-selected /optional/ /opt/deltaops/optional/
COPY --from=debug-build /work/artifacts/plugin/Debug.dll /work/artifacts/plugin/Debug.deps.json /opt/deltaops/plugins/Debug/
COPY config/ /opt/deltaops/config/
COPY <<'ENTRY' /opt/deltaops/entrypoint
#!/bin/bash
set -euo pipefail
umask 077
root=/home/steam/cs2
if [[ ! -x "$root/game/bin/linuxsteamrt64/cs2" && "${INSTALL_ON_START:-0}" != 1 ]]; then
  echo "CS2 is absent. Set INSTALL_ON_START=1 to opt into the mutable Steam download." >&2
  exit 1
fi
if [[ "${UPDATE_ON_START:-0}" == 1 || ! -x "$root/game/bin/linuxsteamrt64/cs2" ]]; then
  args=(+force_install_dir "$root" +login anonymous +app_update 730)
  [[ "${VALIDATE:-0}" != 1 ]] || args+=(validate)
  /opt/steamcmd/steamcmd.sh "${args[@]}" +quit
fi
test -x "$root/game/bin/linuxsteamrt64/cs2"
mkdir -p "$HOME/.steam/sdk64"
ln -sf /opt/steamcmd/linux64/steamclient.so "$HOME/.steam/sdk64/steamclient.so"
# Refresh map and framework binaries even when the CS2 volume predates this image.
cp -a /opt/deltaops/map/game/. "$root/game/"
# Keep volume configs/logs.
cp -a /opt/deltaops/metamod/. "$root/game/csgo/"
css="$root/game/csgo/addons/counterstrikesharp"
mkdir -p "$css/configs"
if [[ -d /opt/deltaops/css/addons/counterstrikesharp/configs ]]; then
  cp -an /opt/deltaops/css/addons/counterstrikesharp/configs/. "$css/configs/"
fi
tar -C /opt/deltaops/css --exclude='./addons/counterstrikesharp/configs' -cf - . | tar -C "$root/game/csgo" -xf -
plugin="$root/game/csgo/addons/counterstrikesharp/plugins/RaidPlugin"
mkdir -p "$plugin"
cp /opt/deltaops/plugins/RaidPlugin/* "$plugin/"
cp /opt/deltaops/config/prices.json "$plugin/prices.json"
[[ -f "$plugin/gameplay.json" ]] || cp /opt/deltaops/config/gameplay.example.json "$plugin/gameplay.json"
operator="$root/game/csgo/addons/counterstrikesharp/plugins/DeltaOps"
mkdir -p "$operator"
if [[ -f /opt/deltaops/optional/DeltaOps.dll ]]; then
  cp /opt/deltaops/optional/DeltaOps.* "$operator/"
else
  rm -f "$operator/DeltaOps.dll" "$operator/DeltaOps.deps.json"
fi
debug="$root/game/csgo/addons/counterstrikesharp/plugins/Debug"
mkdir -p "$debug"
cp /opt/deltaops/plugins/Debug/* "$debug/"
python3 /opt/deltaops/configure "$root"
export LD_LIBRARY_PATH="$root/game/bin/linuxsteamrt64:$root/game/csgo/bin/linuxsteamrt64:${LD_LIBRARY_PATH:-}"
cd "$root/game"
args=(-dedicated -console -condebug -usercon -port 27015 -maxplayers "7"
      +game_type 0 +game_mode 1 +map "${MAP:?set an installed map name}" +exec deltaops-mode.cfg)
token="${GSLT:-}"
[[ -z "$token" ]] || args+=(+sv_setsteamaccount "$token")
exec ./bin/linuxsteamrt64/cs2 "${args[@]}"
ENTRY
COPY <<'CONFIGURE' /opt/deltaops/configure
"""Prepare only DeltaOps-owned files. Fail closed on unexpected gameinfo layout."""
import json
import os
from pathlib import Path
import re
import secrets
import sys

def prepare(root, defaults=Path("/opt/deltaops/config")):
    root = Path(root)
    game = root / "game/csgo"
    info = game / "gameinfo.gi"
    text = info.read_text()
    pattern = r'(?m)^\s*Game\s+csgo/addons/metamod\s*(?://[^\n]*)?$'
    if not re.search(pattern, text):
        text, n = re.subn(r'(?m)^(\s*Game_LowViolence\s+csgo_lv[^\n]*\n)',
                          r'\1\t\t\tGame csgo/addons/metamod\n', text, count=1)
        if not n:
            raise RuntimeError("gameinfo.gi SearchPaths anchor missing; refusing to guess")
        info.write_text(text)

    access = root / "server-access.json"
    saved = json.loads(access.read_text()) if access.exists() else {}
    for key in ("SERVER_PASSWORD", "RCON_PASSWORD"):
        value = os.environ.get(key) or saved.get(key) or secrets.token_hex(18)
        # Console cfg syntax has no safe general-purpose quoting across builds.
        if not re.fullmatch(r"[A-Za-z0-9_!@%+=.,:/?-]{1,128}", value):
            raise ValueError(key + " must be 1-128 safe ASCII characters (no spaces, quotes, $, backslash or semicolon)")
        saved[key] = value
    access.write_text(json.dumps(saved, indent=2) + "\n")
    access.chmod(0o600)

    cfgdir = game / "cfg"
    cfgdir.mkdir(parents=True, exist_ok=True)
    cfg = cfgdir / "deltaops-mode.cfg"
    mode = cfg.read_text() if cfg.exists() else (defaults / cfg.name).read_text()
    for cvar, key in (("sv_password", "SERVER_PASSWORD"), ("rcon_password", "RCON_PASSWORD")):
        mode, count = re.subn(r'(?m)^\s*' + cvar + r'\s+[^\n]*$', cvar + ' "' + saved[key] + '"', mode)
        if not count:
            mode += '\n' + cvar + ' "' + saved[key] + '"\n'
    cfg.write_text(mode)
    cfg.chmod(0o600)

    if not re.fullmatch(r"[A-Za-z0-9_/-]+", os.environ.get("MAP", "")):
        raise ValueError("Invalid MAP")
    players = int(os.environ.get("MAXPLAYERS", "7"))
    if players != 7:
        raise ValueError("MAXPLAYERS must be 7")
    token = os.environ.get("GSLT", "")
    if token and not re.fullmatch(r"[A-Za-z0-9]+", token):
        raise ValueError("Invalid GSLT")

if __name__ == "__main__":
    prepare(sys.argv[1])
CONFIGURE
COPY <<'HEALTH' /opt/deltaops/healthcheck
"""A2S_INFO query, including the optional challenge. Does not prove plugin load."""
import socket
import sys
query = b"\xff\xff\xff\xffTSource Engine Query\x00"
try:
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as sock:
        sock.settimeout(3)
        sock.connect(("127.0.0.1", 27015))
        sock.send(query)
        data = sock.recv(65535)
        if data.startswith(b"\xff\xff\xff\xffA") and len(data) >= 9:
            sock.send(query + data[5:9])
            data = sock.recv(65535)
        sys.exit(0 if data.startswith(b"\xff\xff\xff\xffI") else 1)
except OSError:
    sys.exit(1)
HEALTH
RUN chmod 755 /opt/deltaops/entrypoint \
 && mkdir -p /data/map-reports && chown -R steam:steam /data/map-reports
VOLUME ["/data/map-reports"]
USER steam
WORKDIR /home/steam/cs2
RUN /opt/steamcmd/steamcmd.sh +force_install_dir /home/steam/cs2 +login anonymous +app_update 730 +quit
COPY --from=map-download /output/game/ /opt/deltaops/map/game/
COPY --from=map-download --chown=steam:steam /output/game/ /home/steam/cs2/game/
RUN <<'INSTALL'
set -eu
root=/home/steam/cs2
cp -a /opt/deltaops/metamod/. "$root/game/csgo/"
cp -a /opt/deltaops/css/. "$root/game/csgo/"
for name in RaidPlugin Debug; do
  mkdir -p "$root/game/csgo/addons/counterstrikesharp/plugins/$name"
  cp /opt/deltaops/plugins/$name/* "$root/game/csgo/addons/counterstrikesharp/plugins/$name/"
done
if [ -f /opt/deltaops/optional/DeltaOps.dll ]; then
  mkdir -p "$root/game/csgo/addons/counterstrikesharp/plugins/DeltaOps"
  cp /opt/deltaops/optional/DeltaOps.* "$root/game/csgo/addons/counterstrikesharp/plugins/DeltaOps/"
fi
INSTALL
EXPOSE 27015/udp 27015/tcp
STOPSIGNAL SIGINT
HEALTHCHECK --interval=60s --timeout=10s --start-period=90m --retries=3 CMD ["python3", "/opt/deltaops/healthcheck"]
ENTRYPOINT ["/usr/bin/tini", "-g", "--", "/opt/deltaops/entrypoint"]

