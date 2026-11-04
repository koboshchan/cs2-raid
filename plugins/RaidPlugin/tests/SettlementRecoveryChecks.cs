using Cs2Raid;
using System.Text.Json;
internal static class SettlementRecoveryChecks
{
    public static void Run()
    {
        int n=0;
        void Check(bool b){n++;if(!b)throw new Exception("Recovery check "+n);}
        void Reject(Action a){bool rejected=false;try{a();}catch(Exception e) when(e is InvalidOperationException or IOException or InvalidDataException or JsonException){rejected=true;}Check(rejected);}
        string dir=Path.Combine(Path.GetTempPath(),"raid-recovery-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(dir);
        var session=new RaidSession("76561198000000001","connection","pawn","map");
        var q=new CashInRequest(Guid.NewGuid().ToString("N"),session,10000);
        var key=SettlementProtocol.Key("raid-friends","cashin",q.RaidId,session.SteamId64);
        var r=new RecoveryRecord(key,"cashin",SettlementProtocol.Fingerprint(q),JsonSerializer.Serialize(q,SettlementProtocol.Json),session,"native-inventory-and-cash",10000,RecoveryPhase.Issued);
        var proof=new SettlementProof(key,r.Operation,r.Fingerprint,10000,Guid.NewGuid().ToString("D"),Guid.NewGuid().ToString("D"));
        try {
            for(int crash=0;crash<=4;crash++) {
                string path=Path.Combine(dir,"crash"+crash);
                int mutations=0;
                using(var j=new SettlementRecovery(path)) {
                    Check(j.Issue(r)==r);Check(j.Issue(r)==r);
                    Reject(()=>new SettlementRecovery(path).Dispose());
                    Reject(()=>j.Issue(r with{NativeSnapshot="changed"}));
                    Reject(()=>j.Issue(r with{RequestJson=r.RequestJson+" "}));
                    Reject(()=>j.Issue(r with{AmountCents=10100}));
                    Reject(()=>j.Issue(r with{Session=session with{MapGeneration="different"}}));
                    if(crash>=1) {
                        j.RecordProof(key,proof);Check(j.RecordProof(key,proof).Proof==proof);
                        Reject(()=>j.RecordProof(key,proof with{TransactionId=Guid.NewGuid().ToString("D")}));
                    }
                    if(crash>=2){j.StartConsumption(key);mutations++;}
                    if(crash>=3)j.MarkConsumed(key);
                    if(crash>=4)j.MarkCleanupComplete(key);
                }
                using(var j=new SettlementRecovery(path)) {
                    Check(j.Records[key].Phase==(crash==4?RecoveryPhase.CleanupComplete:RecoveryPhase.Quarantined));
                    Check(j.Records[key].Proof==(crash>=1?proof:null));
                    Reject(()=>j.StartConsumption(key));Reject(()=>j.MarkConsumed(key));
                    Check(mutations==(crash>=2?1:0)); // Reload provides no economic callback/reexecution.
                    if(crash!=4) {
                        Reject(()=>j.MarkManualTerminal(key,"","operator"));
                        j.MarkManualTerminal(key,"isolated inventory; no replay","authorized operator audit");
                        Check(j.Records[key].Phase==RecoveryPhase.ManualTerminal);
                    }
                }
            }
            foreach(var phase in new[]{RecoveryPhase.Issued,RecoveryPhase.ProofRecorded,RecoveryPhase.ConsumptionStarted,RecoveryPhase.Consumed,RecoveryPhase.CleanupComplete}) {
                string path=Path.Combine(dir,"fail"+phase);
                bool armed=false;
                using(var j=new SettlementRecovery(path,entry=>{if(armed&&entry.Phase==phase)throw new IOException("injected write failure");})) {
                    if(phase!=RecoveryPhase.Issued)j.Issue(r);
                    if(phase>RecoveryPhase.ProofRecorded)j.RecordProof(key,proof);
                    if(phase>RecoveryPhase.ConsumptionStarted)j.StartConsumption(key);
                    if(phase>RecoveryPhase.Consumed)j.MarkConsumed(key);
                    armed=true;
                    Reject(()=> {
                        switch(phase) {
                            case RecoveryPhase.Issued:j.Issue(r);break;
                            case RecoveryPhase.ProofRecorded:j.RecordProof(key,proof);break;
                            case RecoveryPhase.ConsumptionStarted:j.StartConsumption(key);break;
                            case RecoveryPhase.Consumed:j.MarkConsumed(key);break;
                            case RecoveryPhase.CleanupComplete:j.MarkCleanupComplete(key);break;
                        }
                    });
                    Check(j.Records.Count==(phase==RecoveryPhase.Issued?0:1)); // inspectable even poisoned
                    Reject(()=>j.Issue(r));
                }
                using var loaded=new SettlementRecovery(path);
                Check(loaded.Records.Count==(phase==RecoveryPhase.Issued?0:1));
                if(loaded.Records.Count>0)Check(loaded.Records[key].Phase==RecoveryPhase.Quarantined);
            }
            foreach(string fault in new[]{"torn","checksum","blank","sequence"}) {
                string path=Path.Combine(dir,fault);
                using(var j=new SettlementRecovery(path)){j.Issue(r);j.RecordProof(key,proof);}
                string text=File.ReadAllText(path);
                File.WriteAllText(path,fault switch {
                    "torn"=>text[..^1],
                    "checksum"=>text.Replace("native-inventory-and-cash","tampered"),
                    "blank"=>text+"\n",
                    _=>string.Join("\n",text.Split('\n').Take(1).Concat(text.Split('\n').Take(1)))+"\n"
                });
                Reject(()=>new SettlementRecovery(path).Dispose());
            }
            using(var j=new SettlementRecovery(Path.Combine(dir,"validation"))) {
                Reject(()=>j.Issue(r with{Fingerprint="bad"}));
                Reject(()=>j.Issue(r with{Session=session with{ConnectionId="other"}}));
                Reject(()=>j.Issue(r with{Phase=RecoveryPhase.Consumed}));
                j.Issue(r);
                Reject(()=>j.RecordProof(key,proof with{TransactionId="arbitrary receipt"}));
                Reject(()=>j.MarkCleanupComplete(key));
                // Model-only lifecycle faults, not native runtime verification.
                j.Quarantine(key,"model disconnect/map-generation invalidation");
                Reject(()=>j.RecordProof(key,proof));Reject(()=>j.StartConsumption(key));
            }
        }finally{Directory.Delete(dir,true);}
        Console.WriteLine(n+" durable settlement recovery checks passed (model/filesystem only, not native runtime QA)");
    }
}
