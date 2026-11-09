using Cs2Raid;
internal static class SettlementTests
{
    public static void Run()
    {
        int n=0;void Check(bool value){n++;if(!value)throw new Exception("Settlement check "+n);}
        void Reject(Action a){bool failed=false;try{a();}catch(InvalidOperationException){failed=true;}Check(failed);}
        var session=new RaidSession("76561198000000001","conn-1","42","map-1");
        var raid="550e8400-e29b-41d4-a716-446655440000";
        var cash=new CashInRequest(raid,session,1500000);
        var key=SettlementProtocol.Key("raid-friends","cashin",raid,session.SteamId64);
        var hash=SettlementProtocol.Fingerprint(cash);
        Check(key=="cs2.raid.v1.cashin.a394a5ff1165066fc2db20a48b7d1a0ebafbed3aad3d4a95cc144114bbeea69f");
        Check(hash=="9f91ad151a1a1a8458bf7d5f98b975eefad11e021044a93374f65e8c86963816");
        var extract=new ExtractRequest(raid,session,"intent-1","window-1",12300,[
            new("00000000-0000-4000-8000-000000000002","flashbang","unused_utility"),
            new("00000000-0000-4000-8000-000000000001","ak47","gun")]);
        Check(SettlementProtocol.Fingerprint(extract)=="0b7942be4339d156513528f4bf1e1f7dd41309b5a225346afa5504d28b529a57");
        Check(SettlementProtocol.Fingerprint(extract)==SettlementProtocol.Fingerprint(extract with{Inventory=extract.Inventory.Reverse().ToArray()}));
        Check(SettlementProtocol.Fingerprint(cash with{RaidId=raid.Replace("-","").ToUpperInvariant()})==hash);
        Check(SettlementProtocol.Cents(15000)==1500000);Check(SettlementProtocol.Dollars(1500000)==15000);
        Reject(()=>SettlementProtocol.Cents(long.MaxValue));Reject(()=>SettlementProtocol.Cents(-1));Reject(()=>SettlementProtocol.Dollars(1));Reject(()=>SettlementProtocol.Dollars(-100));
        Check(SettlementProtocol.Cents(SettlementProtocol.MaxSafeCents/100)<=SettlementProtocol.MaxSafeCents);
        Reject(()=>SettlementProtocol.Cents(SettlementProtocol.MaxSafeCents/100+1));
        Reject(()=>SettlementProtocol.Key("invalid.","cashin",raid,session.SteamId64));
        var proof=new SettlementProof(key,"cashin",hash,1500000,"10000000-0000-4000-8000-000000000001","20000000-0000-4000-8000-000000000001");
        var fence=new SettlementFence();Check(fence.Find(key)==null);
        var entry=fence.Issue(key,"cashin",hash,1500000,"inventory");
        Check(ReferenceEquals(entry,fence.Issue(key,"cashin",hash,1500000,"inventory")));
        Reject(()=>fence.Issue(key,"cashin","changed",1500000,"inventory"));
        Reject(()=>fence.Issue(key,"cashin",hash,1500001,"inventory"));
        Reject(()=>fence.Issue(key,"cashin",hash,1500000,"changed"));
        foreach(var wrong in new[]{proof with{OperationKey="unknown"},proof with{Operation="extract"},proof with{RequestFingerprint="changed"},proof with{AmountCents=1},proof with{TransactionId=null},proof with{TransactionId="receipt-text"},proof with{AppUserId="unknown"}})Check(!entry.Accepted(wrong));
        int applies=0;Check(entry.Complete(proof,()=>{applies++;return true;}));Check(applies==1);
        Check(entry.Complete(proof,()=>{applies++;return true;}));Check(applies==1);
        Check(!entry.Complete(proof with{TransactionId="10000000-0000-4000-8000-000000000002"},()=>{applies++;return true;}));Check(applies==1);
        var zero=proof with{Operation="extract",AmountCents=0,TransactionId=null};
        Check(SettlementProtocol.ProofMatches(zero,key,"extract",hash,0));Check(!SettlementProtocol.ProofMatches(zero with{Operation="cashin"},key,"cashin",hash,0));
        var failed=fence.Issue("failed","extract",hash,0,"frozen");
        Check(!failed.Complete(zero with{OperationKey="failed"},()=>false));Check(failed.Completed==null);
        var game=new GameplayState();game.Join("a");game.Solo("a");var t=DateTimeOffset.UtcNow;game.Tick(t,true);game.SelectCharacter("a",2);game.Tick(t,true);
        Check(game.ApplyCashIn("a",15000));Check(game.Players["a"].Cash==15000);Check(!game.ApplyCashIn("a",15000));
        Check(!new GameplayConfig().FinancialNativeVerified);
        fence.Clear();Check(fence.Find(key)==null);
        Console.WriteLine(n+" settlement protocol checks passed");
    }
}
