"""Offline structural guards, not native-client or engine integration tests."""
from pathlib import Path
source = (Path(__file__).resolve().parents[1] / "DebugPlugin.cs").read_text()
handler = source[source.index("    private void SimpleBannerCommand"):source.index("    private static bool TryPose")]
assert 'c.ArgCount != 2 || !BannerInput.TryParse(c.ArgCount - 1, c.GetArg(1), out var result)' in handler
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
assert 'ModuleVersion => "0.1.6"' in source
print("PASS private invoker delivery, allocation cleanup, parser wiring, native reason and no round termination/broadcast structural guards")

reset = source[source.index("    private void BannerResetCommand"):source.index("    private void SimpleBannerCommand")]
assert 'AddCommand("css_bannerreset", "Reset your private native banner preview", BannerResetCommand);' in source
assert 'if (!Ready(p, c)) return;' in reset
assert 'if (c.ArgCount != 1)' in reset
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
assert handler == old
print("PASS reset human/active/arity/rate gates, invoker-only delivery, finally-free, cleanup, no shared writes; previous preview handlers identical")
