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
        public bool Tombstoned,Applying,Submitted;
    }
    private readonly SettlementFence settlementFence=new();
    private readonly Dictionary<string,NativeSettlement> nativeSettlements=[];
    private readonly Dictionary<string,string> connectionIds=[];
    private readonly Dictionary<uint,string[]> itemInstances=[];
    private bool settlementPollBusy;
    private bool extractionAdmissionClosed;
    private bool nativeBridgeStopped=true;
    private string nativeConsumer=Guid.NewGuid().ToString("N");
    private static bool FinancialEnvironment => Environment.GetEnvironmentVariable("RAID_FINANCIAL_ENABLED")=="1";
    private bool FinancialReady => sessionLoaded && gameplayConfig.FinancialNativeVerified && FinancialEnvironment && !nativeBridgeStopped;
    private bool PendingSettlement => nativeSettlements.Values.Any(s=>settlementFence.Find(s.Key)?.Completed==null);
    private bool FinancialFrozen(string steam)=>nativeSettlements.Values.Any(s=>s.Steam==steam && (s.Tombstoned || settlementFence.Find(s.Key)?.Completed==null));
    private RaidSession CurrentSession(CCSPlayerController p)
    {
        var steam=Id(p);
        if(!connectionIds.TryGetValue(steam,out var connection))connectionIds[steam]=connection=Guid.NewGuid().ToString("N");
        return new(steam,connection,p.PlayerPawn.Raw.ToString(System.Globalization.CultureInfo.InvariantCulture),mapGeneration.ToString("N"));
    }
    private void RegisterSettlementBridge()
    {
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
    private void InvalidateFinancialPlayer(string steam)
    {
        foreach(var s in nativeSettlements.Values.Where(s=>s.Steam==steam && settlementFence.Find(s.Key)?.Completed==null)) {
            s.Tombstoned=true;
            // A pending paid inventory must never become a loot source in this match.
            foreach(var item in s.Inventory) {
                try{var entity=new CHandle<CBasePlayerWeapon>(item.Handle).Value;if(entity is {IsValid:true})entity.Remove();}catch{ /* Cleanup failure remains quarantined; never abort lifecycle invalidation. */ }
            }
            if(game.Players.TryGetValue(steam,out var m)){m.Cash=0;m.Alive=false;}
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
        }catch{p.PrintToChat("Extraction snapshot unavailable. No new payout request issued.");}
    }
    private static string ServerIdentity()=>Environment.GetEnvironmentVariable("SERVER_ID")??"raid-friends";
    private void IssueSettlement(CCSPlayerController p,string key,string operation,object request,long cents,string fingerprint,ExtractionIntent? intent,NativeInventory[]? inventory=null)
    {
        var snapshot=inventory??SnapshotInventory(p);
        settlementFence.Issue(key,operation,fingerprint,cents,NativeSnapshot(snapshot));
        var s=new NativeSettlement(key,operation,request,Id(p),CurrentSession(p),game.Players[Id(p)].Cash,snapshot,intent);
        nativeSettlements.Add(key,s);HoldFinancialInventory(p);
        SubmitSettlement(s);
    }
    private void SubmitSettlement(NativeSettlement s)
    {
        if(s.Submitted || s.Tombstoned || settlementFence.Find(s.Key)?.Completed!=null)return;
        s.Submitted=true;_ = SendSettlement(s.Key,s.Operation,s.Request);
    }
    private HttpRequestMessage BridgeRequest(string route,object body)
    {
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
        if(entry.Completed!=null)return true; // ack-loss replay, no native mutations
        var current=Find(s.Steam);
        if(current==null || s.Applying || !NativeSnapshotMatches(s,current))return false;
        s.Applying=true;
        try {
            return entry.Complete(proof,()=>{
                if(s.Operation=="cashin") {
                    if(!game.ApplyCashIn(s.Steam,SettlementProtocol.Dollars(proof.AmountCents)))return false;
                    SetMoney(current,game.Players[s.Steam].Cash);
                    ReleaseFinancialInventory(s);SetPresentation(current,false,false);return true;
                }
                // Mark consumed first on single game thread; any native failure below
                // quarantines remaining inventory and never returns actor to gameplay.
                if(s.Intent==null || !game.ConfirmVerifiedExtraction(s.Intent))return false;
                foreach(var item in s.Inventory){var w=new CHandle<CBasePlayerWeapon>(item.Handle).Value;if(w is not {IsValid:true})throw new InvalidOperationException("Missing frozen item");w.Remove();}
                current.RemoveWeapons();SetMoney(current,0);
                current.ChangeTeam(CounterStrikeSharp.API.Modules.Utils.CsTeam.Spectator);
                try{ShowRaidResult(current,true);}catch{ /* rendering is not financial acceptance */ }
                return true;
            });
        }catch{InvalidateFinancialPlayer(s.Steam);return false;}
        finally{s.Applying=false;}
    }
}
