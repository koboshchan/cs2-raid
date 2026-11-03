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
