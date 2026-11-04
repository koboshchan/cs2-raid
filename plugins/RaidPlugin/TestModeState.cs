using System.Net;
namespace Cs2Raid;

// No GameplayState, wallets, inventory values, native acceptance, or raid sessions here.
public sealed class TestModeState
{
    public enum Stage { Lobby, Selection, Buy, Countdown, Freeze, Active, Closed }
    public bool Enabled { get; private set; }
    public Stage Phase { get; private set; }
    public int Generation { get; private set; }
    public Dictionary<string,string?> Participants { get; } = [];
    public Dictionary<string,int> Characters { get; } = [];
    public static bool Scope(string? dedicated, bool lan, int port, string? bind)
    {
        if(dedicated!="1" || !lan || port!=27016 || !IPAddress.TryParse(bind,out var ip))return false;
        var b=ip.GetAddressBytes();
        return b.Length==4 && (b[0]==10 || b[0]==127 || b[0]==192&&b[1]==168 || b[0]==172&&b[1]>=16&&b[1]<=31);
    }
    public void Sync(Stage stage){if(Enabled)Phase=stage;}
    public bool NativeActive => Enabled && Phase!=Stage.Closed;
    public bool CallbackAllowed(int generation,bool unloaded) => !unloaded && Generation==generation;
    public bool CanRespawn(bool enrolled) => NativeActive && Phase==Stage.Lobby && enrolled;
    // Failed entries remain available for a later OFF retry. One failure never skips siblings.
    public static void RestoreEntries<K,V>(Dictionary<K,V> entries,Action<K,V> restore,Action<Exception> report) where K:notnull
    {
        foreach(var (key,value) in entries.ToArray()) {
            try { restore(key,value);entries.Remove(key); }
            catch(Exception ex){try{report(ex);}catch{/* A failing logger must not abort teardown. */}}
        }
    }
    // Capture before mutation, with identical generic type for read, write and restoration.
    // A failed setter retains the original restoration action for OFF cleanup.
    public static void SetCaptured<T>(Dictionary<string,Action> saved,string name,Func<T> read,Action<T> write,T value){
        if(!saved.ContainsKey(name)){var original=read();saved.Add(name,()=>write(original));}
        write(value);
    }
    // Expiration abandons opaque native pointers without invoking any restore/dereference callback.
    public static int ExpireEntries<K,V>(Dictionary<K,V> entries) where K:notnull {var count=entries.Count;entries.Clear();return count;}
    public static void FinalizeAll(Action<Exception> report,params Action[] actions)
    {
        foreach(var action in actions)try{action();}catch(Exception ex){try{report(ex);}catch{/* A failing logger must not abort teardown. */}}
    }
    public void Start(bool scope)
    {
        if(!scope || Enabled)throw new InvalidOperationException("Requires dedicated private LAN27016; already ON is rejected.");
        Generation++;Enabled=true;Phase=Stage.Lobby;
    }
    public void Stop(){Generation++;Enabled=false;Phase=Stage.Lobby;Participants.Clear();Characters.Clear();}
    public void Join(string id,bool authenticated)
    {
        if(!Enabled || Phase!=Stage.Lobby || !authenticated || string.IsNullOrEmpty(id))throw new InvalidOperationException("Authenticated lobby participants only.");
        Participants.TryAdd(id,null);
    }
    public void Duo(string a,string b)
    {
        if(!Enabled || Phase!=Stage.Lobby || a==b || !Participants.ContainsKey(a)||!Participants.ContainsKey(b) || Participants[a]!=null || Participants[b]!=null)
            throw new InvalidOperationException("Two unpaired enrolled lobby participants required.");
        Participants[a]=b;Participants[b]=a;
    }
    public void Leave(string id){if(Participants.Remove(id,out var mate)&&mate!=null&&Participants.ContainsKey(mate))Participants[mate]=null;Characters.Remove(id);}
    public void Select(string id,int number)
    {
        if(!Enabled || Phase!=Stage.Selection || !Participants.ContainsKey(id)||number<1||number>6)throw new InvalidOperationException("Selection fixture requires enrolled participant and label 1-6.");
        Characters[id]=number;
    }
    public void Advance()
    {
        if(!Enabled || Participants.Count==0 || Phase==Stage.Closed || Phase==Stage.Selection&&Participants.Keys.Any(id=>!Characters.ContainsKey(id)))
            throw new InvalidOperationException("Enroll participants / select kit-stub labels before advancing.");
        Phase++;
    }
}
