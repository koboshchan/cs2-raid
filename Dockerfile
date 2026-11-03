# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS builder
RUN apt-get update && apt-get install -y --no-install-recommends curl unzip ca-certificates && rm -rf /var/lib/apt/lists/*
WORKDIR /deps
RUN curl -fL --retry 3 https://github.com/roflmuffin/CounterStrikeSharp/releases/download/v1.0.379/counterstrikesharp-with-runtime-linux-1.0.379.zip -o css.zip \
 && echo "bd7e52265346a66a3106934c1f1085b642ec5560b7f070a9b10be510341d583a  css.zip" | sha256sum -c - \
 && unzip -q css.zip -d css && rm css.zip
RUN curl -fL --retry 3 https://mms.alliedmods.net/mmsdrop/2.0/mmsource-2.0.0-git1483-linux.tar.gz -o metamod.tar.gz \
 && echo "0fdcccaa369937ed7f521c78462314b5266f39c1d65a84e4c2d3847324cd7776  metamod.tar.gz" | sha256sum -c - \
 && mkdir metamod && tar -xzf metamod.tar.gz -C metamod && rm metamod.tar.gz
WORKDIR /src
COPY plugin/ ./
RUN dotnet build RaidPlugin.csproj -c Release -p:CSS_API_DIR=/deps/css/addons/counterstrikesharp/api -o /out

COPY tests/ /tests/
COPY plugin/RaidState.cs /plugin/RaidState.cs
RUN dotnet run --project /tests/RaidState.Tests.csproj -c Release

FROM ubuntu:24.04 AS runner
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
RUN curl -fL --retry 3 https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz \
 | tar -xz -C /opt/steamcmd
RUN chown -R steam:steam /opt/steamcmd
COPY --from=builder /deps/css/ /opt/deltaops/css/
COPY --from=builder /deps/metamod/ /opt/deltaops/metamod/
COPY --from=builder /out/RaidPlugin.dll /out/RaidPlugin.deps.json /opt/deltaops/plugin/
COPY config/ /opt/deltaops/config/
COPY scripts/ /opt/deltaops/scripts/
RUN chmod 755 /opt/deltaops/scripts/entrypoint.sh
USER steam
WORKDIR /home/steam/cs2
EXPOSE 27015/udp 27015/tcp
STOPSIGNAL SIGINT
ENTRYPOINT ["/usr/bin/tini", "-g", "--", "/opt/deltaops/scripts/entrypoint.sh"]
