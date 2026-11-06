"""Offline structural guards, not native-client or engine integration tests."""
from pathlib import Path
source = (Path(__file__).resolve().parents[1] / "DebugPlugin.cs").read_text()
handler = source[source.index("    private void SimpleBannerCommand"):source.index("    private static bool TryPose")]
assert 'if (c.ArgCount != 2)' in handler
assert handler.index('if (c.ArgCount != 2)') < handler.index('var action = c.GetArg(1);')
assert 'if (action.Equals("reset", StringComparison.OrdinalIgnoreCase))' in handler
assert '{ ResetBanner(p!, c); return; }' in handler
assert 'BannerInput.TryParse(c.ArgCount - 1, action, out var result)' in handler
assert handler.count('if (!Ready(p, c)) return;') == 2
assert 'RoundBanner.TryCreate(p!.TeamNum, result, out var banner)' in handler
assert 'bannerLimiter.Allow(p.SteamID' in handler
assert handler.count('.FireEventToClient(p);') == 2
assert handler.count('finally {') == 2
assert 'finally { end.Free(); }' in handler and 'finally { panel.Free(); }' in handler
assert 'panel.FinalEvent = banner.PanelFinalEvent;' in handler
assert 'end.Winner = banner.Winner;' in handler and 'end.Reason = banner.Reason;' in handler
# Ignore comments when excluding actual calls and shared-state writes.
code = '\n'.join(line.split('//')[0] for line in handler.splitlines())
for forbidden in ('FireEvent(', 'TerminateRound(', 'PrintToChatAll(', 'Server.', 'Utilities.', 'TeamNum =', 'GameRules', 'requestedColor'):
    assert forbidden not in code, forbidden
assert 'ModuleVersion => "0.1.25"' in source
print("PASS private invoker delivery, allocation cleanup, parser wiring, native reason and no round termination/broadcast structural guards")

reset = source[source.index("    private void BannerResetCommand"):source.index("    private void SimpleBannerCommand")]
assert 'AddCommand("css_bannerreset", "Reset your private native banner preview", BannerResetCommand);' in source
assert 'if (!Ready(p, c)) return;' in reset
assert 'if (c.ArgCount != 1)' in reset
assert reset.index('if (c.ArgCount != 1)') < reset.index('ResetBanner(p!, c);')
assert 'private void ResetBanner(CCSPlayerController p, CommandInfo c)' in reset
assert 'bannerResetLimiter.Allow(p!.SteamID, Environment.TickCount64, 1000)' in reset
assert 'var reset = new EventRoundStart(true);' in reset
assert 'try { reset.FireEventToClient(p); }' in reset
assert 'finally { reset.Free(); }' in reset
assert reset.count('FireEventToClient(') == 1
code = '\n'.join(line.split('//')[0] for line in reset.splitlines())
for forbidden in ('FireEvent(', 'TerminateRound(', 'PrintToChatAll(', 'Server.', 'Utilities.', 'GameRules', 'GetArg(', '.Set', 'TeamNum ='):
    assert forbidden not in code, forbidden
assert 'bannerResetLimiter.Clear();' in source and 'bannerResetLimiter.Remove(id);' in source
# Existing preview implementation must remain byte-identical to reviewed 0.1.5.
import subprocess
before = subprocess.check_output(['git', 'show', '34fa384e67cdfc7d0e01a922fbd585e6b0587ccb:plugins/Debug/DebugPlugin.cs'], cwd=Path(__file__).resolve().parents[3]).decode()
old = before[before.index('    private void SimpleBannerCommand'):before.index('    private static bool TryPose')]
assert handler[handler.index('    private void BannerCommand'):] == old[old.index('    private void BannerCommand'):]
print("PASS reset human/active/arity/rate gates, invoker-only delivery, finally-free, cleanup, no shared writes; canonical reset and optional alias share private reset; existing preview sender identical")

# Native event routing is structurally checked, not a substitute for two-client QA.
mvp = source[source.index("    private void MvpCommand"):source.index("    private void BannerResetCommand")]
assert 'AddCommand("css_mvp", "Preview your private native round MVP panel", MvpCommand);' in source
assert 'if (!Ready(p, c)) return;' in mvp and 'if (c.ArgCount != 1)' in mvp
assert mvp.index('if (c.ArgCount != 1)') < mvp.index('RoundBanner.TryCreate') < mvp.index('mvpLimiter.Allow') < mvp.index('new EventRoundEnd')
assert 'BannerResult.Win' in mvp
assert mvp.count('.FireEventToClient(p);') == 3
for name in ('end', 'mvp', 'panel'):
    assert 'finally { ' + name + '.Free(); }' in mvp
assert mvp.index('end.FireEventToClient') < mvp.index('mvp.FireEventToClient') < mvp.index('panel.FireEventToClient')
for assignment in ('mvp.Userid = p;', 'mvp.Reason = kills > 0 ? 1 : 7;', 'mvp.Value = kills;', 'mvp.Musickitid = musicId;', 'mvp.Musickitmvps = musicMvps;', 'mvp.Nomusic = noMusic;'):
    assert assignment in mvp
for read in ('p.ActionTrackingServices?.NumRoundKills ?? 0', 'p.MusicKitID', 'p.MusicKitMVPs', 'p.MvpNoMusic'):
    assert read in mvp
code = '\n'.join(line.split('//')[0] for line in mvp.splitlines())
for forbidden in ('FireEvent(', 'TerminateRound(', 'PrintToChatAll(', 'Server.', 'Utilities.', 'GameRules', 'p.MVPs =', 'p.MusicKitID =', 'p.MusicKitMVPs =', 'p.MvpNoMusic =', 'NumRoundKills =', 'InventoryServices', 'ExecuteClientCommand', 'SetStateChanged'):
    assert forbidden not in code, forbidden
assert 'mvpLimiter.Clear();' in source and 'mvpLimiter.Remove(id);' in source
assert 'banner/MVP reset notification sent' in reset
print('PASS MVP enabled/human/arity/team/rate gates, real-stat reads, three private sends/finally-free, no shared stats/music/round writes; reset shares existing private cleanup')
