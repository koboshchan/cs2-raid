# syntax=docker/dockerfile:1
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
COPY --chown=1000:1000 plugins/DeltaOps/DeltaOps.csproj plugins/DeltaOps/DeltaOpsConfig.cs plugins/DeltaOps/DeltaOpsPlugin.cs plugins/DeltaOps/config.example.json plugins/DeltaOps/source.sha256 /input/
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
  cp /input/DeltaOps.csproj /input/DeltaOpsConfig.cs /input/DeltaOpsPlugin.cs /work/run$n/src/
  dotnet build /work/run$n/src/DeltaOps.csproj -c Release --configfile /work/NuGet.Config \
    -o /work/artifacts/build-$n -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api \
    -p:Deterministic=true -p:ContinuousIntegrationBuild=true -p:PathMap=/work/run$n/src=/_/deltaops \
    -p:UseSharedCompilation=false -p:TreatWarningsAsErrors=true > /work/artifacts/build-$n.log 2>&1
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
COPY --chown=1000:1000 plugins/RaidPlugin/RaidPlugin.csproj plugins/RaidPlugin/RaidPlugin.cs plugins/RaidPlugin/RaidState.cs plugins/RaidPlugin/source.sha256 /input/
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
  cp /input/RaidPlugin.csproj /input/RaidPlugin.cs /input/RaidState.cs /work/run$n/src/; cp -R /input/tests /work/run$n/src/
  dotnet build /work/run$n/src/RaidPlugin.csproj -c Release --configfile /work/NuGet.Config \
    -o /work/artifacts/build-$n -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api \
    -p:Deterministic=true -p:ContinuousIntegrationBuild=true -p:PathMap=/work/run$n/src=/_/raidplugin \
    -p:UseSharedCompilation=false -p:TreatWarningsAsErrors=true > /work/artifacts/build-$n.log 2>&1
  cat /work/artifacts/build-$n.log
  grep -q '0 Warning(s)' /work/artifacts/build-$n.log
  grep -q '0 Error(s)' /work/artifacts/build-$n.log
  dotnet restore /work/run$n/src/tests/RaidState.Tests.csproj --configfile /work/NuGet.Config >> /work/artifacts/build-$n.log 2>&1
  dotnet build /work/run$n/src/tests/RaidState.Tests.csproj -c Release --no-restore -p:TreatWarningsAsErrors=true >> /work/artifacts/build-$n.log 2>&1
  dotnet /work/run$n/src/tests/bin/Release/net10.0/RaidState.Tests.dll >> /work/artifacts/build-$n.log 2>&1
  grep '19 raid rule checks passed' /work/artifacts/build-$n.log
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

FROM dependencies AS server-dependencies
RUN curl -fL --retry 3 https://mms.alliedmods.net/mmsdrop/2.0/mmsource-2.0.0-git1483-linux.tar.gz -o /deps/metamod.tar.gz \
 && echo "0fdcccaa369937ed7f521c78462314b5266f39c1d65a84e4c2d3847324cd7776  /deps/metamod.tar.gz" | sha256sum -c - \
 && mkdir /deps/metamod && tar -xzf /deps/metamod.tar.gz -C /deps/metamod && rm /deps/metamod.tar.gz

FROM mcr.microsoft.com/dotnet/sdk:10.0.403-noble@sha256:de4fe90a98845d75910b9769662465c84de1a934665c431279d1382b2bddc02b AS map-tools
COPY config/map-v1.lock.json /input/map-v1.lock.json
COPY <<'MAP_FETCH' /input/fetch-map.ps1
param([string]$Archive = "")
$ErrorActionPreference = "Stop"
$lock = Get-Content /input/map-v1.lock.json -Raw | ConvertFrom-Json
New-Item -ItemType Directory -Force /work, /output | Out-Null
if (-not $Archive) {
    $assets = @{}
    $token = ""
    if (Test-Path /run/secrets/github_token) {
        $token = (Get-Content /run/secrets/github_token -Raw).Trim()
        $release = Invoke-RestMethod -Uri "https://api.github.com/repos/koboshchan/cs2-raid/releases/tags/map-v1" -Headers @{ Authorization = "Bearer $token"; Accept = "application/vnd.github+json" }
        foreach ($item in $release.assets) { $assets[$item.name] = $item.url }
    }
    $parts = @()
    foreach ($item in $lock.assets) {
        if ($item.name -notlike "*.zip.part*") { continue }
        $dest = "/work/" + $item.name
        $url = $lock.url.Replace("/tag/", "/download/") + "/" + $item.name
        $args = @("-fL", "--retry", "3", "--silent", "--show-error")
        if ($token) {
            $url = $assets[$item.name]
            if (-not $url.StartsWith("https://api.github.com/")) { throw "Unexpected authenticated asset host" }
            $args += @("-H", "Authorization: Bearer $token", "-H", "Accept: application/octet-stream")
        }
        # curl drops Authorization on cross-host redirects unless --location-trusted is set. Never set it.
        & curl @args $url -o $dest
        if ($LASTEXITCODE -ne 0) { throw "Map part download failed" }
        if ((Get-Item $dest).Length -ne $item.size) { throw "Map part size mismatch" }
        if ((Get-FileHash $dest -Algorithm SHA256).Hash.ToLower() -ne $item.sha256.Replace("sha256:", "")) { throw "Map part hash mismatch" }
        $parts += $dest
    }
    $Archive = "/work/map.zip"
    $out = [System.IO.File]::Create($Archive)
    try {
        foreach ($part in $parts) {
            $src = [System.IO.File]::OpenRead($part)
            try { $src.CopyTo($out) } finally { $src.Dispose() }
        }
    } finally { $out.Dispose() }
}
if ((Get-Item $Archive).Length -ne $lock.archive_bytes) { throw "Archive size mismatch" }
if ((Get-FileHash $Archive -Algorithm SHA256).Hash.ToLower() -ne $lock.archive_sha256) { throw "Archive hash mismatch" }
$names = @("game/csgo/cfg/df_spaceport_server.cfg")
foreach ($suffix in @("", "_000", "_001", "_002")) {
    $names += "game/csgo/maps/df_spaceport_player_scale_raytrace" + $suffix + ".vpk"
}
$zip = [System.IO.Compression.ZipFile]::OpenRead($Archive)
try {
    foreach ($name in $names) {
        $entry = $zip.GetEntry("df_spaceport_server_multipart/" + $name)
        if ($null -eq $entry) { throw "Missing expected map member" }
        $dest = "/output/" + $name
        New-Item -ItemType Directory -Force (Split-Path $dest) | Out-Null
        $src = $entry.Open()
        $out = [System.IO.File]::Create($dest)
        try { $src.CopyTo($out) } finally { $src.Dispose(); $out.Dispose() }
    }
} finally { $zip.Dispose() }
Copy-Item /input/map-v1.lock.json /output/map-v1.lock.json
Write-Output "Map archive and all parts verified, five allowlisted files staged. No server started."
MAP_FETCH
FROM map-tools AS map-download
RUN --mount=type=secret,id=github_token pwsh -NoLogo -NoProfile -File /input/fetch-map.ps1
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
COPY --from=deltaops-build /work/artifacts/build-1/DeltaOps.dll /work/artifacts/build-1/DeltaOps.deps.json /opt/deltaops/plugins/DeltaOps/
COPY plugins/DeltaOps/config.example.json /opt/deltaops/config/DeltaOps.example.json
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
# Refresh framework binaries from the image, without removing volume configs/logs.
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
operator="$root/game/csgo/addons/counterstrikesharp/plugins/DeltaOps"
mkdir -p "$operator"
cp /opt/deltaops/plugins/DeltaOps/* "$operator/"
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
RUN chmod 755 /opt/deltaops/entrypoint
USER steam
WORKDIR /home/steam/cs2
RUN /opt/steamcmd/steamcmd.sh +force_install_dir /home/steam/cs2 +login anonymous +app_update 730 +quit
COPY --from=map-download --chown=steam:steam /output/game/ /home/steam/cs2/game/
RUN <<'INSTALL'
set -eu
root=/home/steam/cs2
cp -a /opt/deltaops/metamod/. "$root/game/csgo/"
cp -a /opt/deltaops/css/. "$root/game/csgo/"
for name in RaidPlugin DeltaOps; do
  mkdir -p "$root/game/csgo/addons/counterstrikesharp/plugins/$name"
  cp /opt/deltaops/plugins/$name/* "$root/game/csgo/addons/counterstrikesharp/plugins/$name/"
done
INSTALL
EXPOSE 27015/udp 27015/tcp
STOPSIGNAL SIGINT
HEALTHCHECK --interval=60s --timeout=10s --start-period=90m --retries=3 CMD ["python3", "/opt/deltaops/healthcheck"]
ENTRYPOINT ["/usr/bin/tini", "-g", "--", "/opt/deltaops/entrypoint"]

