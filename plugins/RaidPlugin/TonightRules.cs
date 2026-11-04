namespace Cs2Raid;

// Pure rules kept independent of native handles for offline coverage.
public static class TonightRules
{
    // Engine round clock and native buy expiry share this one fixed anchor.
    public static float BuyRoundStart(float engineNow,DateTimeOffset now,DateTimeOffset end){
        if(!float.IsFinite(engineNow))throw new ArgumentOutOfRangeException(nameof(engineNow));
        var elapsed=(now-end.AddSeconds(-GameplayState.BuySeconds)).TotalSeconds;
        if(elapsed<0||elapsed>GameplayState.BuySeconds)throw new InvalidOperationException("Buy clock outside current window.");
        return engineNow-(float)elapsed;
    }
    public const int SpawnHealth = 200;
    public const int HudLimit = 65535;
    public static int Hud(long balance) => (int)Math.Clamp(balance, 0L, HudLimit);
    public static bool ChooseNativeTeam(GamePhase phase, int nativeTeam) =>
        phase is GamePhase.Lobby or GamePhase.CharacterSelect && nativeTeam is 2 or 3;
    public static float Half(float baseline) => baseline * 0.5f;
    public readonly record struct BannerFields(int Winner,int Reason,string Message) { public int PanelFinalEvent => Reason; }
    public static BannerFields Banner(int nativeTeam,bool won)
    {
        if(nativeTeam is not (2 or 3))throw new ArgumentOutOfRangeException(nameof(nativeTeam));
        return (won ? nativeTeam : 5-nativeTeam) == 2
            ? new(2,9,"#SFUI_Notice_Terrorists_Win") : new(3,8,"#SFUI_Notice_CTs_Win");
    }
}
