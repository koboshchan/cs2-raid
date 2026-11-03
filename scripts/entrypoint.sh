#!/bin/bash
set -euo pipefail
umask 077
root=/home/steam/cs2
if [[ "${UPDATE_ON_START:-1}" == 1 || ! -x "$root/game/bin/linuxsteamrt64/cs2" ]]; then
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
cp /opt/deltaops/plugin/* "$plugin/"
cp /opt/deltaops/config/prices.json "$plugin/prices.json"
python3 /opt/deltaops/scripts/configure.py "$root"
export LD_LIBRARY_PATH="$root/game/bin/linuxsteamrt64:$root/game/csgo/bin/linuxsteamrt64:${LD_LIBRARY_PATH:-}"
cd "$root/game"
args=(-dedicated -console -condebug -usercon -port 27015 -maxplayers "7"
      +game_type 0 +game_mode 1 +map "${MAP:?set an installed map name}" +exec deltaops-mode.cfg)
token="${GSLT:-}"
[[ -z "$token" ]] || args+=(+sv_setsteamaccount "$token")
exec ./bin/linuxsteamrt64/cs2 "${args[@]}"
