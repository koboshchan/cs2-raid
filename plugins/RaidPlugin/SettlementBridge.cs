using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private sealed record NativeInventory(uint Handle,string Entity,int Clip,int[] Reserve,ushort[] Ammo,SettlementItem[] Items);
    private sealed class NativeSettlement(string key,string operation,object request,string steam,RaidSession session,long cash,NativeInventory[] inventory,ExtractionIntent? intent)
    {
        public string Key=key,Operation=operation,Steam=steam;
        public object Request=request;
        public RaidSession Session=session;
        public long Cash=cash;
        public NativeInventory[] Inventory=inventory;
        public ExtractionIntent? Intent=intent;
        public bool Tombstoned,Applying,Submitted,BackendReady,JournalBusy,Isolated;
        public DateTimeOffset IssuedAt=DateTimeOffset.UtcNow;
    }
    private SettlementRecovery? settlementRecovery;
    private bool recoveryUnavailable;
    private bool manualRecoveryPollBusy,cleanupEvidenceBusy;
    private int manualRecoveryCursor;
    private readonly HashSet<string> cleanupEvidenceAcknowledged=[];
    private readonly HashSet<string> blockedFinancialActors=[];
    private readonly SettlementFence settlementFence=new();
    private readonly Dictionary<string,NativeSettlement> nativeSettlements=[];
    private readonly Dictionary<string,string> connectionIds=[];
    private readonly Dictionary<uint,string[]> itemInstances=[];
    private bool settlementPollBusy;
    private bool extractionAdmissionClosed;
    private bool nativeBridgeStopped=true;
    private string nativeConsumer=Guid.NewGuid().ToString("N");
    private static bool FinancialEnvironment => Environment.GetEnvironmentVariable("RAID_FINANCIAL_ENABLED")=="1";
    private bool FinancialReady => !TestServer && sessionLoaded && gameplayConfig.FinancialNativeVerified && FinancialEnvironment && !nativeBridgeStopped && !recoveryUnavailable && settlementRecovery!=null;
    private bool PendingSettlement => nativeSettlements.Values.Any(s=>settlementFence.Find(s.Key)?.Completed==null);
    private bool FinancialFrozen(string steam)=>recoveryUnavailable || blockedFinancialActors.Contains(steam) || nativeSettlements.Values.Any(s=>s.Steam==steam && (s.Tombstoned || settlementFence.Find(s.Key)?.Completed==null));
    private RaidSession CurrentSession(CCSPlayerController p)
    {
        var steam=Id(p);
        if(!connectionIds.TryGetValue(steam,out var connection))connectionIds[steam]=connection=Guid.NewGuid().ToString("N");
        return new(steam,connection,p.PlayerPawn.Raw.ToString(System.Globalization.CultureInfo.InvariantCulture),mapGeneration.ToString("N"));
    }
    private void RegisterSettlementBridge()
    {
        try {
            settlementRecovery=new SettlementRecovery(Path.Combine(ModuleDirectory,"settlement-recovery.jsonl"));
            foreach(var r in settlementRecovery.Records.Values.Where(r=>r.Phase is RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal))blockedFinancialActors.Add(r.Session.SteamId64);
        }catch{recoveryUnavailable=true;}
        nativeBridgeStopped=false;
        foreach(var command in new[]{"drop","kill","explode","jointeam","joinclass","buy","autobuy","rebuy","+attack","+attack2","+reload"})
            AddCommandListener(command,(p,c)=>Human(p)&&FinancialFrozen(Id(p))?HookResult.Handled:HookResult.Continue);
        RegisterEventHandler<EventPlayerDisconnect>((e,i)=>{
            if(Human(e.Userid)){InvalidateFinancialPlayer(Id(e.Userid));connectionIds.Remove(Id(e.Userid));}
            return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventPlayerDeath>((e,i)=>{
            if(Human(e.Userid)&&FinancialFrozen(Id(e.Userid)))InvalidateFinancialPlayer(Id(e.Userid));
            return HookResult.Continue;
        },HookMode.Pre);
        AddTimer(5f,()=>{if(FinancialReady)foreach(var s in nativeSettlements.Values.ToArray())SubmitSettlement(s);},TimerFlags.REPEAT);
        AddTimer(0.25f,()=>{if(FinancialReady && !settlementPollBusy){settlementPollBusy=true;_ = PollNativeCallback();}},TimerFlags.REPEAT);
    }
    private void HoldFinancialInventory(CCSPlayerController p)
    {
        SetPresentation(p,true,false);
        if(p.PlayerPawn.Value?.WeaponServices is not {} ws)return;
        foreach(var h in ws.MyWeapons)if(h.Value is {IsValid:true} w) {
            w.NextPrimaryAttackTick=int.MaxValue;w.NextSecondaryAttackTick=int.MaxValue;
        }
    }
    private void ReleaseFinancialInventory(NativeSettlement s)
    {
        foreach(var item in s.Inventory)if(new CHandle<CBasePlayerWeapon>(item.Handle).Value is {IsValid:true} w) {
            w.NextPrimaryAttackTick=Server.TickCount+1;w.NextSecondaryAttackTick=Server.TickCount+1;
        }
    }
    private bool IsFinancialActorBlocked(string steam)=>recoveryUnavailable || blockedFinancialActors.Contains(steam);
    private bool PendingExtractionSettlement(string windowId)=>nativeSettlements.Values.Any(s=>s.Operation=="extract" && s.Request is ExtractRequest r && r.ExtractionWindowId==windowId && !s.Tombstoned && settlementFence.Find(s.Key)?.Completed==null);
    private bool TryIsolateFinancialActor(CCSPlayerController p)
    {
        try {
            p.RemoveWeapons();SetMoney(p,0);p.ChangeTeam(CsTeam.Spectator);
            return p.TeamNum==(int)CsTeam.Spectator && !p.PawnIsAlive && p.InGameMoneyServices is {} money && money.Account==0 && p.PlayerPawn.Value?.WeaponServices is {} ws && ws.MyWeapons.Count==0;
        }catch{return false;}
    }
    private bool IsQuarantinedHandle(uint handle)
    {
        if(recoveryUnavailable)return true;
        return settlementRecovery!=null && settlementRecovery.Records.Values.Any(r=>r.Session.MapGeneration==mapGeneration.ToString("N") && r.Phase is RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal && (JsonSerializer.Deserialize<NativeInventory[]>(r.NativeSnapshot,SettlementProtocol.Json)??[]).Any(i=>i.Handle==handle));
    }
    private bool OriginalHandlesAbsent(string snapshot,bool remove)
    {
        try {
            foreach(var item in JsonSerializer.Deserialize<NativeInventory[]>(snapshot,SettlementProtocol.Json)??throw new InvalidOperationException("Snapshot")) {
                if(new CHandle<CBasePlayerWeapon>(item.Handle).Value is not {IsValid:true} w)continue;
                if(remove)w.Remove();
                if(new CHandle<CBasePlayerWeapon>(item.Handle).Value is {IsValid:true})return false;
            }
            return true;
        }catch{return false;}
    }
    private void TickSettlementRecovery()
    {
        // Retained original evidence enforces handle-specific quarantine even after disconnect/reset.
        if(settlementRecovery!=null)foreach(var r in settlementRecovery.Records.Values.Where(r=>r.Phase is RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal))if(r.Session.MapGeneration==mapGeneration.ToString("N"))OriginalHandlesAbsent(r.NativeSnapshot,true);
        if(settlementRecovery!=null && !recoveryUnavailable && !manualRecoveryPollBusy) {
            var unresolved=settlementRecovery.Records.Values.Where(r=>r.Phase!=RecoveryPhase.ManualTerminal).ToArray();
            if(unresolved.Length>0){var r=unresolved[(int)((uint)manualRecoveryCursor++%(uint)unresolved.Length)];manualRecoveryPollBusy=true;_ = ObserveManualTerminal(r);}
        }
        if(settlementRecovery!=null && !recoveryUnavailable && !cleanupEvidenceBusy) {
            var done=settlementRecovery.Records.Values.FirstOrDefault(r=>r.Phase==RecoveryPhase.CleanupComplete && !cleanupEvidenceAcknowledged.Contains(r.Key));
            if(done?.Proof!=null){cleanupEvidenceBusy=true;_ = PersistCleanupEvidence(done);}
        }
        foreach(var s in nativeSettlements.Values.ToArray()) {
            if(s.Tombstoned){var p=Find(s.Steam);if(p!=null)s.Isolated=TryIsolateFinancialActor(p);continue;}
            if(settlementFence.Find(s.Key)?.Completed!=null)continue;
            if(DateTimeOffset.UtcNow-s.IssuedAt>TimeSpan.FromSeconds(20) || (s.Operation=="cashin" && game.Phase!=GamePhase.Buy))InvalidateFinancialPlayer(s.Steam);
        }
    }
    private void InvalidateFinancialPlayer(string steam)
    {
        foreach(var s in nativeSettlements.Values.Where(s=>s.Steam==steam && settlementFence.Find(s.Key)?.Completed==null)) {
            blockedFinancialActors.Add(steam);s.Tombstoned=true;
            try{settlementRecovery?.Quarantine(s.Key,"Actor invalidated; preserve original key, receipt and value; no refund or regrant");}catch{recoveryUnavailable=true;}
            foreach(var item in s.Inventory)try{var entity=new CHandle<CBasePlayerWeapon>(item.Handle).Value;if(entity is {IsValid:true})entity.Remove();}catch{}
            if(game.Players.TryGetValue(steam,out var m)){m.Cash=0;m.Alive=false;}
            if(Find(steam) is {} p)s.Isolated=TryIsolateFinancialActor(p);
        }
    }
    private void InvalidateFinancialMatch()
    {
        try{foreach(var steam in nativeSettlements.Values.Select(s=>s.Steam).Distinct().ToArray())InvalidateFinancialPlayer(steam);}
        finally{nativeSettlements.Clear();settlementFence.Clear();connectionIds.Clear();itemInstances.Clear();nativeConsumer=Guid.NewGuid().ToString("N");extractionAdmissionClosed=false;settlementPollBusy=false;}
    }
    private NativeInventory[] SnapshotInventory(CCSPlayerController p)
    {
        var ws=p.PlayerPawn.Value?.WeaponServices ?? throw new InvalidOperationException("No inventory");
        var result=new List<NativeInventory>();
        foreach(var handle in ws.MyWeapons) {
            var w=handle.Value ?? throw new InvalidOperationException("Invalid weapon");
            if(!w.IsValid || w.OwnerEntity.Raw!=p.PlayerPawn.Raw)throw new InvalidOperationException("Inventory ownership");
            if(new CCSWeaponBase(w.Handle).InReload)throw new InvalidOperationException("Reload in progress");
            if(w.DesignerName.StartsWith("weapon_knife",StringComparison.Ordinal)||w.DesignerName=="weapon_c4")continue;
            var item=catalog.SingleOrDefault(x=>x.Entity==w.DesignerName) ?? throw new InvalidOperationException("Unknown inventory");
            bool utility=GameplayConfig.Utility(item.Entity);
            if(!utility && !item.IsGun)throw new InvalidOperationException("Non-refundable inventory");
            var ammo=ws.Ammo.ToArray();
            int count=1;
            if(utility) {
                var data=new CCSWeaponBase(w.Handle).VData ?? throw new InvalidOperationException("No ammo data");
                if(data.PrimaryAmmoType>=ammo.Length)throw new InvalidOperationException("Invalid ammo type");
                count=ammo[data.PrimaryAmmoType];
                var grenade=new CBaseCSGrenade(w.Handle);
                if(count is <1 or >2 || grenade.PinPulled || grenade.ThrowTime>0 || grenade.ThrowAnimating || grenade.JustPulledPin)
                    throw new InvalidOperationException("Utility not unused");
                // Per-unit GUIDs follow a stable entity lifetime; count changes after first
                // observation retire consumed unit IDs; snapshots remain exact during freeze.
            }
            if(!itemInstances.TryGetValue(handle.Raw,out var ids))itemInstances[handle.Raw]=ids=Enumerable.Range(0,count).Select(_=>Guid.NewGuid().ToString("D")).ToArray();
            if(ids.Length!=count) {
                ids=count<ids.Length?ids.Take(count).ToArray():ids.Concat(Enumerable.Range(ids.Length,count-ids.Length).Select(_=>Guid.NewGuid().ToString("D"))).ToArray();
                itemInstances[handle.Raw]=ids; // Retire consumed units, never reuse their IDs.
            }
            var items=ids.Select(id=>new SettlementItem(id,item.Id,utility?"unused_utility":"gun")).ToArray();
            result.Add(new(handle.Raw,w.DesignerName,w.Clip1,w.ReserveAmmo.ToArray(),ammo,items));
        }
        if(result.Sum(x=>x.Items.Length)>128)throw new InvalidOperationException("Too much inventory");
        return result.OrderBy(x=>x.Handle).ToArray();
    }
    private static string NativeSnapshot(NativeInventory[] items)=>JsonSerializer.Serialize(items,SettlementProtocol.Json);
    private bool NativeSnapshotMatches(NativeSettlement s,CCSPlayerController p)
    {
        if(s.Tombstoned || !p.PawnIsAlive || CurrentSession(p)!=s.Session || game.MatchId!=(s.Request is CashInRequest c?c.RaidId:((ExtractRequest)s.Request).RaidId) ||
           !game.Living(s.Steam) || game.Players[s.Steam].Cash!=s.Cash)return false;
        if(s.Operation=="cashin" ? game.Phase!=GamePhase.Buy : game.Phase!=GamePhase.Active || s.Intent==null || !game.IsIssuedExtraction(s.Intent))return false;
        try{return NativeSnapshot(SnapshotInventory(p))==NativeSnapshot(s.Inventory);}catch{return false;}
    }
    private void BeginCashIn(CCSPlayerController? p,string amount)
    {
        if(!Human(p))return;
        if(!FinancialReady){p.PrintToChat("Cash-in disabled until native financial QA and backend policy are configured. No funds moved.");return;}
        if(!long.TryParse(amount,out var dollars)||dollars is <1 or >15000 || game.Phase!=GamePhase.Buy || !game.Living(Id(p))) {
            p.PrintToChat("Use !cashout <whole dollars>, from 1 to 15000 during the buy phase.");return;
        }
        try {
            var steam=Id(p);var key=SettlementProtocol.Key(ServerIdentity(),"cashin",game.MatchId,steam);
            if(nativeSettlements.TryGetValue(key,out var old)){if(old.Request is CashInRequest r && r.AmountCents==SettlementProtocol.Cents(dollars))SubmitSettlement(old);else p.PrintToChat("Cash-in amount is immutable for this raid.");return;}
            ObserveNativeSpend(p);
            if(game.Players[steam].Cash!=0)throw new InvalidOperationException("Entry balance must be zero");
            var rnew=new CashInRequest(game.MatchId,CurrentSession(p),SettlementProtocol.Cents(dollars));
            IssueSettlement(p,key,"cashin",rnew,rnew.AmountCents,SettlementProtocol.Fingerprint(rnew),null);
        }catch{p.PrintToChat("Cash-in snapshot unavailable. No new money request issued.");}
    }
    private void BeginExtraction(CCSPlayerController p,ExtractionIntent intent)
    {
        if(!FinancialReady || !Human(p))return;
        try {
            ObserveNativeSpend(p);
            var inventory=SnapshotInventory(p);
            var request=new ExtractRequest(game.MatchId,CurrentSession(p),intent.Id,game.ExtractionWindowId,SettlementProtocol.Cents(intent.Cash),inventory.SelectMany(x=>x.Items).ToArray());
            long cents=request.CashCents;
            foreach(var i in request.Inventory)cents=checked(cents+SettlementProtocol.Cents(catalog.Single(x=>x.Id==i.CatalogId).PriceDollars));
            if(cents>SettlementProtocol.MaxSafeCents)throw new InvalidOperationException("Precision");
            IssueSettlement(p,SettlementProtocol.Key(ServerIdentity(),"extract",game.MatchId,Id(p)),"extract",request,cents,SettlementProtocol.Fingerprint(request),intent,inventory);
        }catch{p.PrintToChat("Extraction snapshot unavailable. No payout this window; no economic consumption requested. Retry at the next extraction window.");}
    }
    private static string ServerIdentity()=>Environment.GetEnvironmentVariable("SERVER_ID")??"raid-friends";
    private void IssueSettlement(CCSPlayerController p,string key,string operation,object request,long cents,string fingerprint,ExtractionIntent? intent,NativeInventory[]? inventory=null)
    {
        if(TestServer)return;
        var snapshot=inventory??SnapshotInventory(p);
        if(settlementRecovery==null || recoveryUnavailable)throw new InvalidOperationException("Durable journal unavailable");
        try{settlementRecovery.Issue(new RecoveryRecord(key,operation,fingerprint,JsonSerializer.Serialize(request,request.GetType(),SettlementProtocol.Json),CurrentSession(p),NativeSnapshot(snapshot),cents,RecoveryPhase.Issued));}
        catch{recoveryUnavailable=true;throw;}
        settlementFence.Issue(key,operation,fingerprint,cents,NativeSnapshot(snapshot));
        var s=new NativeSettlement(key,operation,request,Id(p),CurrentSession(p),game.Players[Id(p)].Cash,snapshot,intent);
        nativeSettlements.Add(key,s);HoldFinancialInventory(p);
        SubmitSettlement(s);
    }
    private void SubmitSettlement(NativeSettlement s)
    {
        if(TestServer || s.Submitted || s.Tombstoned || settlementFence.Find(s.Key)?.Completed!=null)return;
        s.Submitted=true;_ = SendSettlement(s.Key,s.Operation,s.Request);
    }
    private HttpRequestMessage BridgeRequest(string route,object body)
    {
        if(TestServer)throw new InvalidOperationException("MONEY HARD OFF");
        if(Environment.GetEnvironmentVariable("RAID_BACKEND_URL")!="http://backend:3000" || (Environment.GetEnvironmentVariable("BRIDGE_TOKEN")??"").Length<32)
            throw new InvalidOperationException("Bridge configuration");
        var req=new HttpRequestMessage(HttpMethod.Post,"http://backend:3000"+route);
        req.Headers.Authorization=new AuthenticationHeaderValue("Bearer",Environment.GetEnvironmentVariable("BRIDGE_TOKEN"));
        req.Content=JsonContent.Create(body,options:SettlementProtocol.Json);return req;
    }
    private async Task SendSettlement(string key,string operation,object request)
    {
        try {
            using var req=BridgeRequest(operation=="cashin"?"/cashin":"/extract",request);
            using var response=await http.SendAsync(req);
            // Completion is exclusively through verified callback, never HTTP status/body.
        }catch { /* Ambiguous outcome remains frozen; identical request can reconcile. */ }
        Server.NextFrame(()=>{if(nativeSettlements.TryGetValue(key,out var s))s.Submitted=false;});
    }
    private async Task PollNativeCallback()
    {
        var epoch=nativeConsumer;
        try {
            using var req=BridgeRequest("/native/next",new {});
            using var response=await http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            using var doc=JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if(doc.RootElement.TryGetProperty("job",out var job) && job.ValueKind==JsonValueKind.Object) {
                var copy=job.Clone();
                Server.NextFrame(()=>{
                    var accepted=false;
                    try{if(epoch==nativeConsumer && FinancialReady)accepted=ApplyNativeCallback(copy);}catch{ /* refuse malformed or uncertain callbacks */ }
                    _ = ReplyNativeCallback(copy.GetProperty("id").GetString()??"",epoch,accepted);
                });
            }
        }catch { /* Polling never releases a financial freeze. */ }
        finally{Server.NextFrame(()=>{if(epoch==nativeConsumer)settlementPollBusy=false;});}
    }
    private async Task ReplyNativeCallback(string id,string consumer,bool accepted)
    {
        try{using var req=BridgeRequest("/native/reply",new{id,accepted});using var response=await http.SendAsync(req);}catch{ /* retry same durable operation; native completion is idempotent */ }
    }
    private bool ApplyNativeCallback(JsonElement job)
    {
        if(TestServer)return false;
        var key=job.GetProperty("operationKey").GetString()??"";
        if(!nativeSettlements.TryGetValue(key,out var s) || job.GetProperty("operation").GetString()!=s.Operation)return false;
        var entry=settlementFence.Find(key);if(entry==null)return false;
        var request=job.GetProperty("request");
        string fingerprint=s.Operation=="cashin"?SettlementProtocol.Fingerprint(request.Deserialize<CashInRequest>(SettlementProtocol.Json)??throw new InvalidOperationException()):
            SettlementProtocol.Fingerprint(request.Deserialize<ExtractRequest>(SettlementProtocol.Json)??throw new InvalidOperationException());
        if(fingerprint!=entry.Fingerprint)return false;
        var method=job.GetProperty("method").GetString();
        if(method=="freeze")return entry.Completed==null && Find(s.Steam) is {} p && NativeSnapshotMatches(s,p);
        if(method!="finalize")return false;
        var proof=job.GetProperty("proof").Deserialize<SettlementProof>(SettlementProtocol.Json);if(proof==null||!entry.Accepted(proof))return false;
        if(entry.Completed!=null)return true; // ack loss never repeats economics
        if(s.Tombstoned || settlementRecovery==null || recoveryUnavailable)return false;
        var current=Find(s.Steam);
        if(current==null || s.Applying)return false;
        if(CurrentSession(current)!=s.Session || game.MatchId!=(s.Request is CashInRequest cashRequest?cashRequest.RaidId:((ExtractRequest)s.Request).RaidId)){InvalidateFinancialPlayer(s.Steam);return false;}
        var recovery=settlementRecovery.Records[key];
        if(recovery.Phase==RecoveryPhase.Issued) {
            if(!NativeSnapshotMatches(s,current))return false;
            try{settlementRecovery.RecordProof(key,proof);}catch{recoveryUnavailable=true;InvalidateFinancialPlayer(s.Steam);return false;}
        }
        if(!s.BackendReady){if(!s.JournalBusy){s.JournalBusy=true;_ = PersistConsumptionBarrier(s,proof,nativeConsumer);}return false;}
        s.Applying=true;
        try {
            recovery=settlementRecovery.Records[key];
            if(recovery.Phase==RecoveryPhase.ProofRecorded) {
                if(!NativeSnapshotMatches(s,current)){InvalidateFinancialPlayer(s.Steam);return false;}
                settlementRecovery.StartConsumption(key); // fsync BEFORE economic mutation, never replay after restart
                var consumed=s.Operation=="cashin" ? game.ApplyCashIn(s.Steam,SettlementProtocol.Dollars(proof.AmountCents)) : s.Intent!=null && game.ConfirmVerifiedExtraction(s.Intent);
                if(!consumed){InvalidateFinancialPlayer(s.Steam);return false;}
                settlementRecovery.MarkConsumed(key); // economic commit is separate from fallible presentation
            }
            if(settlementRecovery.Records[key].Phase!=RecoveryPhase.Consumed)return false;
            // Cleanup is idempotent. No debit, mint, model grant or consumption is repeated here.
            if(s.Operation=="cashin") {
                SetMoney(current,game.Players[s.Steam].Cash);
                if(current.InGameMoneyServices is not {} money || money.Account!=TonightRules.Hud(game.Players[s.Steam].Cash))throw new InvalidOperationException("Projection not verified");
                ReleaseFinancialInventory(s);SetPresentation(current,false,false);
            } else {
                foreach(var item in s.Inventory)if(new CHandle<CBasePlayerWeapon>(item.Handle).Value is {IsValid:true} w)w.Remove();
                if(!OriginalHandlesAbsent(recovery.NativeSnapshot,true) || !TryIsolateFinancialActor(current))throw new InvalidOperationException("Extraction physical isolation not verified");
            }
            settlementRecovery.MarkCleanupComplete(key);
            var complete=entry.Complete(proof,()=>true);
            // Durable outbox in TickSettlementRecovery mirrors cleanup until acknowledged, including after restart.
            if(s.Operation=="extract")try{ShowRaidResult(current,true);}catch{}
            return complete;
        }catch {
            // A failed fsync at the consumption boundary is ambiguous. Never replay it.
            try{HoldFinancialInventory(current);}catch{TryIsolateFinancialActor(current);}
            if(!settlementRecovery.IsAvailable){recoveryUnavailable=true;InvalidateFinancialPlayer(s.Steam);}
            else if(settlementRecovery.Records[key].Phase==RecoveryPhase.ConsumptionStarted)InvalidateFinancialPlayer(s.Steam);
            return false; // consumed cleanup is retried by identical proof, not reapplied economically
        }
        finally{s.Applying=false;}
    }
    private async Task ObserveManualTerminal(RecoveryRecord record)
    {
        JsonElement? evidence=null;
        try {
            using var req=BridgeRequest("/native/journal/read",new{operationKey=record.Key,requestFingerprint=record.Fingerprint});
            using var response=await http.SendAsync(req);response.EnsureSuccessStatusCode();
            using var doc=JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            evidence=doc.RootElement.Clone();
        }catch{}
        Server.NextFrame(()=>{
            try {
                if(evidence is not {} result || settlementRecovery==null)return;
                if(!result.TryGetProperty("journal",out var journal) || journal.ValueKind!=JsonValueKind.Object || !journal.TryGetProperty("terminal",out var terminal))return;
                var status=terminal.GetString();
                if(status is not ("quarantine_requested" or "quarantined"))return;
                // The revocation gate runs on exactly the same game thread as every finalize.
                // Persist tombstone before fallible physical isolation or sending backend acknowledgement.
                if(nativeSettlements.TryGetValue(record.Key,out var live))live.Tombstoned=true;
                blockedFinancialActors.Add(record.Session.SteamId64);
                if(settlementRecovery.Records[record.Key].Phase is not (RecoveryPhase.Quarantined or RecoveryPhase.ManualTerminal))
                    settlementRecovery.Quarantine(record.Key,"Authenticated backend revocation requested; no later native consumption permitted");
                var actor=Find(record.Session.SteamId64);
                // Persisted raw handles are generation-local, never remove unrelated new-map entities.
                // Unknown old-world isolation remains unresolved for a verified operator workflow.
                if(record.Session.MapGeneration!=mapGeneration.ToString("N"))return;
                if(!OriginalHandlesAbsent(record.NativeSnapshot,true))return;
                if(actor!=null && !TryIsolateFinancialActor(actor))return;
                if(status=="quarantine_requested") {
                    var epoch=journal.GetProperty("revocationEpoch").GetString()??throw new InvalidOperationException("Revocation epoch");
                    _ = AcknowledgeNativeRevocation(record,epoch);
                    return;
                }
                if(settlementRecovery.Records[record.Key].Phase==RecoveryPhase.Quarantined)
                    settlementRecovery.MarkManualTerminal(record.Key,"Original handles absent; connected actor verified spectator without weapons",result.GetRawText());
                // Actor remains blocked across reconnect/reset. Terminal audit never grants or refunds.
            }catch{if(settlementRecovery?.IsAvailable!=true)recoveryUnavailable=true; /* Missing isolation remains unresolved, never ack. */ }
            finally{manualRecoveryPollBusy=false;}
        });
    }
    private async Task AcknowledgeNativeRevocation(RecoveryRecord record,string epoch)
    {
        try {
            using var req=BridgeRequest("/native/quarantine/ack",new{operationKey=record.Key,requestFingerprint=record.Fingerprint,revocationEpoch=epoch,session=record.Session,nativePhase="quarantined",isolationEvidence="Local fsynced tombstone on sole game thread; original handles absent; connected actor verified isolated; no economic replay"});
            using var response=await http.SendAsync(req);response.EnsureSuccessStatusCode();
        }catch{ /* Retained durable tombstone polls original epoch again; no terminal-by-absence. */ }
    }
    private async Task PersistConsumptionBarrier(NativeSettlement s,SettlementProof proof,string epoch)
    {
        bool accepted=false;
        try {
            foreach(var phase in new[]{"snapshot","consumption_intent"}) {
                var payload=new Dictionary<string,object>{["operationKey"]=s.Key,["requestFingerprint"]=proof.RequestFingerprint,["nativeSnapshot"]=NativeSnapshot(s.Inventory),["phase"]=phase,["evidence"]="Original native generation frozen; local durable proof recorded"};
                if(phase!="snapshot")payload["proof"]=proof;
                using var req=BridgeRequest("/native/recovery",payload);
                using var response=await http.SendAsync(req);response.EnsureSuccessStatusCode();
            }
            accepted=true;
        }catch{}
        Server.NextFrame(()=>{if(epoch!=nativeConsumer || s.Tombstoned)return;s.JournalBusy=false;s.BackendReady=accepted;});
    }
    private async Task PersistCleanupEvidence(RecoveryRecord record)
    {
        bool accepted=false;
        try {
            foreach(var phase in new[]{"consumed","cleanup_complete"}) {
                using var req=BridgeRequest("/native/recovery",new{operationKey=record.Key,requestFingerprint=record.Fingerprint,nativeSnapshot=record.NativeSnapshot,phase,proof=record.Proof,evidence="Local fsynced native journal phase; no economic replay"});
                using var response=await http.SendAsync(req);response.EnsureSuccessStatusCode();
            }
            accepted=true;
        }catch{ /* Retain durable outbox and retry evidence only, never economic consumption. */ }
        Server.NextFrame(()=>{cleanupEvidenceBusy=false;if(accepted)cleanupEvidenceAcknowledged.Add(record.Key);});
    }
}
