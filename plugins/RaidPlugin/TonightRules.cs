namespace Cs2Raid;

// Pure rules kept independent of native handles for offline coverage.
public static class TonightRules
{
    public const int SpawnHealth = 200;
    public const int HudLimit = 65535;
    public static int Hud(long balance) => (int)Math.Clamp(balance, 0L, HudLimit);
    public static bool ChooseNativeTeam(GamePhase phase, int nativeTeam) =>
        phase is GamePhase.Lobby or GamePhase.CharacterSelect && nativeTeam is 2 or 3;
    public static float Half(float baseline) => baseline * 0.5f;
    public static (int Winner,int Reason,string Message) Banner(int nativeTeam,bool won)
    {
        if(nativeTeam is not (2 or 3))throw new ArgumentOutOfRangeException(nameof(nativeTeam));
        return (won ? nativeTeam : 5-nativeTeam) == 2
            ? (2,9,"#SFUI_Notice_Terrorists_Win") : (3,8,"#SFUI_Notice_CTs_Win");
    }
}
