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

// Shared by the actual adapter and injected failure tests. Faults remain latched
// after cleanup until an explicit reset successfully opens a fresh generation.
public sealed class RaidBuyOwnership
{
    public bool Blocked { get; private set; }
    public bool Owned { get; private set; }
    public int Generation { get; private set; }
    public bool Pending { get; private set; }
    public bool BeginReset(Action restore,Func<bool> pending,Action<Exception> report){
        Blocked=true;Owned=false;Generation++;
        Cleanup(restore,pending,report);
        if(Pending)return false;
        Blocked=false;return true;
    }
    public bool Apply(Action setup,Action restore,Func<bool> pending,Action<Exception> report,bool ownsBuy){
        if(Blocked)return false;
        Owned=false; // admission closes before any native lookup, capture or write
        try{setup();Owned=ownsBuy;return true;}
        catch(Exception ex){
            Blocked=true;Generation++;SafeReport(report,ex);
            Cleanup(restore,pending,report);return false;
        }
    }
    public void Cleanup(Action restore,Func<bool> pending,Action<Exception> report){
        Owned=false;
        try{restore();}catch(Exception ex){SafeReport(report,ex);}
        Pending=pending();
    }
    public void Stop(Action restore,Func<bool> pending,Action<Exception> report){
        Blocked=true;Owned=false;Generation++;Cleanup(restore,pending,report);
    }
    public bool Allows(int generation,bool window) => !Blocked&&Owned&&generation==Generation&&window;
    private static void SafeReport(Action<Exception> report,Exception ex){try{report(ex);}catch{/* keep fence closed */}}
}
