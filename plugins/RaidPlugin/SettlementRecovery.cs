using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
namespace Cs2Raid;

public enum RecoveryPhase { Issued, ProofRecorded, ConsumptionStarted, Consumed, CleanupComplete, Quarantined, ManualTerminal }
public sealed record RecoveryRecord(string Key,string Operation,string Fingerprint,string RequestJson,RaidSession Session,string NativeSnapshot,long AmountCents,RecoveryPhase Phase,SettlementProof? Proof=null,string? Reason=null);

// Single-owner fail-closed journal. No method executes economic effects or replays consumption.
public sealed class SettlementRecovery : IDisposable
{
    private sealed record Line(long Sequence,string Previous,string Payload,string Checksum);
    private readonly Dictionary<string,RecoveryRecord> records=new(StringComparer.Ordinal);
    private readonly FileStream ownership;
    private FileStream? journal;
    private readonly Action<RecoveryRecord>? beforeWrite;
    private long sequence;
    private string previous="";
    private bool failed,disposed;
    public IReadOnlyDictionary<string,RecoveryRecord> Records {get;}
    public bool IsAvailable=>!failed&&!disposed;
    public SettlementRecovery(string path,Action<RecoveryRecord>? beforeWrite=null)
    {
        this.beforeWrite=beforeWrite;
        Records=new ReadOnlyDictionary<string,RecoveryRecord>(records);
        path=Path.GetFullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        ownership=new FileStream(path+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
        try {
            journal=new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.Read,4096,FileOptions.WriteThrough);
            using(var reader=new StreamReader(journal,new UTF8Encoding(false,true),false,4096,true)) {
                string text=reader.ReadToEnd();
                if(text.Length>0 && !text.EndsWith('\n'))throw new InvalidDataException("Torn journal record");
                foreach(var raw in text.Length==0 ? Array.Empty<string>() : text[..^1].Split('\n')) {
                    var line=JsonSerializer.Deserialize<Line>(raw,SettlementProtocol.Json)??throw new InvalidDataException("Null journal line");
                    if(line.Sequence!=sequence+1 || line.Previous!=previous || line.Checksum!=Digest(line.Sequence,line.Previous,line.Payload))
                        throw new InvalidDataException("Journal checksum/sequence");
                    var record=JsonSerializer.Deserialize<RecoveryRecord>(line.Payload,SettlementProtocol.Json)??throw new InvalidDataException("Null record");
                    Validate(record);ValidateTransition(record);
                    records[record.Key]=record;sequence=line.Sequence;previous=line.Checksum;
                }
            }
            journal.Seek(0,SeekOrigin.End);
            foreach(var r in records.Values.ToArray())
                if(r.Phase is not (RecoveryPhase.CleanupComplete or RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal))
                    Append(r with {Phase=RecoveryPhase.Quarantined,Reason="Restart with unfinished settlement; never replay native consumption"});
        }catch {journal?.Dispose();ownership.Dispose();throw;}
    }
    private static string Digest(long seq,string prev,string payload)=>Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(seq.ToString(System.Globalization.CultureInfo.InvariantCulture)+"\n"+prev+"\n"+payload)));
    private static void Require(bool condition,string reason){if(!condition)throw new InvalidOperationException(reason);}
    private static void Validate(RecoveryRecord r)
    {
        Require(!string.IsNullOrWhiteSpace(r.Key)&&!string.IsNullOrWhiteSpace(r.NativeSnapshot),"Missing identity/snapshot");
        Require(r.AmountCents>=0 && r.AmountCents<=SettlementProtocol.MaxSafeCents,"Invalid amount");
        Require(r.Session!=null && !string.IsNullOrWhiteSpace(r.Session.SteamId64)&&!string.IsNullOrWhiteSpace(r.Session.ConnectionId)&&!string.IsNullOrWhiteSpace(r.Session.PawnGeneration)&&!string.IsNullOrWhiteSpace(r.Session.MapGeneration),"Session required");
        string fp;
        if(r.Operation=="cashin") {
            var q=JsonSerializer.Deserialize<CashInRequest>(r.RequestJson,SettlementProtocol.Json)??throw new InvalidOperationException("Request");
            Require(q.Session==r.Session&&q.AmountCents==r.AmountCents,"Request mismatch");fp=SettlementProtocol.Fingerprint(q);
        }else {
            Require(r.Operation=="extract","Operation");
            var q=JsonSerializer.Deserialize<ExtractRequest>(r.RequestJson,SettlementProtocol.Json)??throw new InvalidOperationException("Request");
            Require(q.Session==r.Session && q.Inventory!=null && q.CashCents>=0 && q.CashCents<=r.AmountCents,"Request mismatch");
            fp=SettlementProtocol.Fingerprint(q);
        }
        Require(r.Key.StartsWith("cs2.raid.v1."+r.Operation+".",StringComparison.Ordinal)&&r.Key.Length==("cs2.raid.v1."+r.Operation+".").Length+64 && r.Key[^64..].All(c=>c is >= '0' and <= '9' or >= 'a' and <= 'f'),"Operation key");
        Require(fp==r.Fingerprint,"Fingerprint mismatch");
        Require(Enum.IsDefined(r.Phase),"Unknown phase");
        if(r.Proof!=null)Require(SettlementProtocol.ProofMatches(r.Proof,r.Key,r.Operation,r.Fingerprint,r.AmountCents),"Invalid structured proof");
        if(r.Phase is RecoveryPhase.ProofRecorded or RecoveryPhase.ConsumptionStarted or RecoveryPhase.Consumed or RecoveryPhase.CleanupComplete)Require(r.Proof!=null,"Proof required");
        if(r.Phase is RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal)Require(!string.IsNullOrWhiteSpace(r.Reason),"Audit reason required");
    }
    private void ValidateTransition(RecoveryRecord r)
    {
        if(!records.TryGetValue(r.Key,out var old)){Require(r.Phase==RecoveryPhase.Issued&&r.Proof==null&&r.Reason==null,"First phase must be issued");return;}
        Require(old with{Phase=r.Phase,Proof=r.Proof,Reason=r.Reason}==r,"Immutable operation conflict");
        Require(old.Proof==null || old.Proof==r.Proof,"Original proof immutable");
        Require(old.Proof!=null || r.Proof==null || r.Phase==RecoveryPhase.ProofRecorded,"Proof can only be recorded in proof phase");
        Require(r.Phase is RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal || r.Reason==old.Reason,"Unexpected audit mutation");
        bool ok=(old.Phase,r.Phase) switch {
            (RecoveryPhase.Issued,RecoveryPhase.ProofRecorded)=>true,
            (RecoveryPhase.ProofRecorded,RecoveryPhase.ConsumptionStarted)=>true,
            (RecoveryPhase.ConsumptionStarted,RecoveryPhase.Consumed)=>true,
            (RecoveryPhase.Consumed,RecoveryPhase.CleanupComplete)=>true,
            (RecoveryPhase.Quarantined,RecoveryPhase.ManualTerminal)=>true,
            (_,RecoveryPhase.Quarantined)=>old.Phase is not (RecoveryPhase.ManualTerminal or RecoveryPhase.Quarantined),
            _=>false
        };
        Require(ok,"Invalid recovery phase transition");
    }
    private RecoveryRecord Append(RecoveryRecord r)
    {
        Require(!failed&&!disposed,"Journal unavailable");
        Validate(r);ValidateTransition(r);
        try {
            beforeWrite?.Invoke(r);
            string payload=JsonSerializer.Serialize(r,SettlementProtocol.Json);
            var line=new Line(sequence+1,previous,payload,Digest(sequence+1,previous,payload));
            var bytes=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(line,SettlementProtocol.Json)+"\n");
            journal!.Write(bytes);journal.Flush(true);
            sequence=line.Sequence;previous=line.Checksum;records[r.Key]=r;return r;
        }catch{failed=true;throw;}
    }
    private RecoveryRecord Get(string key){Require(!failed&&!disposed,"Journal unavailable");return records.TryGetValue(key,out var r)?r:throw new InvalidOperationException("Unknown settlement");}
    public RecoveryRecord Issue(RecoveryRecord record) {
        Require(!failed&&!disposed,"Journal unavailable");
        if(records.TryGetValue(record.Key,out var old)){Require(old==record,"Immutable duplicate issue");return old;}
        return Append(record);
    }
    public RecoveryRecord RecordProof(string key,SettlementProof proof) {
        var r=Get(key);
        if(r.Proof!=null){Require(r.Proof==proof,"Conflicting proof");return r;}
        return Append(r with{Phase=RecoveryPhase.ProofRecorded,Proof=proof});
    }
    public RecoveryRecord StartConsumption(string key)=>Append(Get(key) with{Phase=RecoveryPhase.ConsumptionStarted});
    public RecoveryRecord MarkConsumed(string key)=>Append(Get(key) with{Phase=RecoveryPhase.Consumed});
    public RecoveryRecord MarkCleanupComplete(string key)=>Append(Get(key) with{Phase=RecoveryPhase.CleanupComplete});
    public RecoveryRecord Quarantine(string key,string reason) {
        var r=Get(key);if(r.Phase==RecoveryPhase.Quarantined)return r;
        return Append(r with{Phase=RecoveryPhase.Quarantined,Reason=reason});
    }
    // Caller must authenticate/authorize the operator; evidence is audit data, never a replay permission.
    public RecoveryRecord MarkManualTerminal(string key,string isolationEvidence,string operatorAudit) {
        Require(!string.IsNullOrWhiteSpace(isolationEvidence)&&!string.IsNullOrWhiteSpace(operatorAudit),"Explicit isolation evidence and operator audit required");
        return Append(Get(key) with{Phase=RecoveryPhase.ManualTerminal,Reason=JsonSerializer.Serialize(new{isolationEvidence,operatorAudit},SettlementProtocol.Json)});
    }
    public void Dispose(){if(disposed)return;disposed=true;journal?.Dispose();ownership.Dispose();}
}
