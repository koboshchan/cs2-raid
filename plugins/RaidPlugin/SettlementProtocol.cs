using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
namespace Cs2Raid;

public sealed record RaidSession(string SteamId64,string ConnectionId,string PawnGeneration,string MapGeneration);
public sealed record SettlementItem(string InstanceId,string CatalogId,string Kind);
public sealed record CashInRequest(string RaidId,RaidSession Session,long AmountCents);
public sealed record ExtractRequest(string RaidId,RaidSession Session,string ExtractionIntentId,string ExtractionWindowId,long CashCents,SettlementItem[] Inventory);
public sealed record SettlementProof(string OperationKey,string Operation,string RequestFingerprint,long AmountCents,string? TransactionId,string AppUserId);
public static class SettlementProtocol
{
    public const long MaxSafeCents=9007199254740991;
    public static readonly JsonSerializerOptions Json=new() {PropertyNamingPolicy=JsonNamingPolicy.CamelCase,Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping};
    public static long Cents(long dollars) => dollars>=0 && dollars<=MaxSafeCents/100 ? checked(dollars*100) : throw new InvalidOperationException("Currency precision");
    public static long Dollars(long cents) => cents>=0 && cents<=MaxSafeCents && cents%100==0 ? cents/100 : throw new InvalidOperationException("Fractional game money");
    public static string Hash(object value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value,Json))));
    public static string NormalizeRaid(string raid) => Guid.TryParseExact(raid,"N",out var n)||Guid.TryParseExact(raid,"D",out n) ? n.ToString("N") : throw new InvalidOperationException("Raid identity");
    public static string Key(string server,string operation,string raid,string steam)
    {
        if(!Regex.IsMatch(server,@"\A[a-zA-Z0-9_-]{1,32}\z") || operation is not ("cashin" or "extract") || !Regex.IsMatch(steam,@"\A[0-9]{17}\z"))throw new InvalidOperationException("Operation identity");
        return "cs2.raid.v1."+operation+"."+Hash(new[]{server,NormalizeRaid(raid),steam});
    }
    public static string Fingerprint(CashInRequest r)=>Hash(new object[]{"cs2.raid.cashin.v1",NormalizeRaid(r.RaidId),r.Session,r.AmountCents});
    public static string Fingerprint(ExtractRequest r)=>Hash(new object[]{"cs2.raid.extract.v1",NormalizeRaid(r.RaidId),r.Session,r.ExtractionIntentId,r.ExtractionWindowId,r.CashCents,r.Inventory.OrderBy(x=>x.InstanceId,StringComparer.Ordinal).ToArray()});
    public static bool ProofMatches(SettlementProof p,string key,string operation,string fingerprint,long cents) =>
        p.OperationKey==key && p.Operation==operation && p.RequestFingerprint==fingerprint && p.AmountCents==cents && cents>=0 && cents<=MaxSafeCents &&
        Guid.TryParseExact(p.AppUserId,"D",out _) && (cents==0 ? operation=="extract" && p.TransactionId==null : p.TransactionId!=null && Guid.TryParseExact(p.TransactionId,"D",out _));
}
// Game-thread-owned allowlist. Backend callbacks cannot create requests or change snapshots.
public sealed class SettlementFence
{
    public sealed class Entry(string key,string operation,string fingerprint,long cents,string nativeSnapshot)
    {
        public string Key {get;}=key; public string Operation {get;}=operation; public string Fingerprint {get;}=fingerprint;
        public long Cents {get;}=cents; public string NativeSnapshot {get;}=nativeSnapshot;
        public SettlementProof? Completed {get;private set;}
        public bool Accepted(SettlementProof proof) => SettlementProtocol.ProofMatches(proof,Key,Operation,Fingerprint,Cents) && (Completed==null || Completed==proof);
        public bool Complete(SettlementProof proof,Func<bool> apply)
        {
            if(!Accepted(proof))return false;
            if(Completed!=null)return true;
            if(!apply())return false; Completed=proof;return true;
        }
    }
    private readonly Dictionary<string,Entry> entries=[];
    public Entry? Find(string key)=>entries.GetValueOrDefault(key);
    public Entry Issue(string key,string operation,string fingerprint,long cents,string nativeSnapshot)
    {
        if(entries.TryGetValue(key,out var old)) {
            if(old.Operation!=operation || old.Fingerprint!=fingerprint || old.Cents!=cents || old.NativeSnapshot!=nativeSnapshot)throw new InvalidOperationException("Immutable operation conflict");
            return old;
        }
        if(cents<0||cents>SettlementProtocol.MaxSafeCents)throw new InvalidOperationException("Currency precision");
        var entry=new Entry(key,operation,fingerprint,cents,nativeSnapshot);entries.Add(key,entry);return entry;
    }
    public void Clear()=>entries.Clear();
}
