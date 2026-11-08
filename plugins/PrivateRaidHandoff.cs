using System.Text.Json;

namespace PrivateRaidSafety;

// The legacy map-only lease remains compatible with the already-live respawn helper.
// Only an exact fresh nonce echo grants economy ownership to Raid.
public sealed record HandoffToken(string Map, string Nonce);
public static class PrivateRaidHandoff
{
    public const string Lease = "/tmp/kat-private-raid-flow-active";
    public const string Request = "/tmp/kat-private-raid-flow-request";
    public const string Ready = "/tmp/kat-private-raid-flow-debug-ready";
    public const string Map = "df_spaceport_v261";
    public static bool Fresh(DateTime now, DateTime modified, double seconds) =>
        now >= modified && (now - modified).TotalSeconds <= seconds;
    public static bool Matches(HandoffToken? expected,HandoffToken? actual) =>
        expected is not null && actual == expected && expected.Map == Map &&
        Guid.TryParseExact(expected.Nonce,"N",out _);
    public static void Write(string path,string text)
    {
        var temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";
        try { File.WriteAllText(temp,text);File.Move(temp,path,true); }
        finally { if(File.Exists(temp))File.Delete(temp); }
    }
    public static void Publish(HandoffToken token)
    {
        Write(Request,JsonSerializer.Serialize(token));
        Write(Lease,token.Map);
    }
    public static HandoffToken? ReadRequest(string map)
    {
        try {
            var now=DateTime.UtcNow;
            if(map!=Map || !Fresh(now,File.GetLastWriteTimeUtc(Lease),15) ||
               !Fresh(now,File.GetLastWriteTimeUtc(Request),15) || File.ReadAllText(Lease)!=map)return null;
            var token=JsonSerializer.Deserialize<HandoffToken>(File.ReadAllText(Request));
            return Matches(token,token)?token:null;
        } catch { return null; }
    }
    public static bool Acknowledged(HandoffToken token)
    {
        try { return Fresh(DateTime.UtcNow,File.GetLastWriteTimeUtc(Ready),10) &&
            Matches(token,JsonSerializer.Deserialize<HandoffToken>(File.ReadAllText(Ready))); }
        catch { return false; }
    }
    public static void Acknowledge(HandoffToken token)=>Write(Ready,JsonSerializer.Serialize(token));
    public static bool RestorePending(Dictionary<string,Action> entries,Action<Exception> report)
    {
        foreach(var (key,restore) in entries.ToArray()) {
            try { restore();entries.Remove(key); }
            catch(Exception e) { try { report(e); }catch {} }
        }
        return entries.Count==0;
    }
}
