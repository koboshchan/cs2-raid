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
assert 'ModuleVersion => "0.1.5"' in source
print("PASS private invoker delivery, allocation cleanup, parser wiring, native reason and no round termination/broadcast structural guards")
