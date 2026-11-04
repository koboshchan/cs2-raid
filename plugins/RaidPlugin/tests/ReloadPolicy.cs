using Cs2Raid;
static class ReloadPolicyTests
{
    sealed class Catalog(int price):ReloadPolicy.IVanillaCatalog{
        public ReloadPolicy.VanillaQuote? Resolve(string item)=>item=="unknown"?null:new(item,price,"fixture-engine-build","fixture-catalog-digest");
    }
    sealed class Adapter(bool accept):ReloadPolicy.IVerifiedReloadAdapter{
        public bool Verify(ReloadPolicy.CommitProof proof,ReloadPolicy.Entry entry)=>accept;
    }
    public static void Run(){
        int n=0;void C(bool ok){n++;if(!ok)throw new Exception("reload policy "+n);}
        var a=new ReloadPolicy.Actor(7,123,456,true,true,true,true,true);
        ReloadPolicy L(long b=15000)=>new(a,b);
        foreach(var other in new[]{a with{Generation=8},a with{Controller=124},a with{Weapon=457},
            a with{Dedicated=false},a with{Scope=false},a with{Human=false},a with{Enrolled=false},a with{Alive=false}}){
            var l=L();C(!l.ReserveFromCatalog("x",other,"ak47",new Catalog(2700),10,30,0)&&l.Balance==15000);
        }
        var book=L();C(book.ReserveFromCatalog("one",a,"ak47",new Catalog(2700),10,30,0)&&book.Balance==14865);
        C(!book.ReserveFromCatalog("two",a,"ak47",new Catalog(2700),10,30,0));C(book.PlannedReserve(a)==0);
        C(book.Begin(a)&&book.PlannedReserve(a)==30);C(!book.Begin(a)&&!book.CancelBeforeNative(a));
        var proof=new ReloadPolicy.CommitProof("one",7,123,456,10,30,20,"engine-op-1");
        C(!book.ConfirmConsumption(a,proof,new Adapter(false)));
        C(!book.ConfirmConsumption(a,proof with{Weapon=457},new Adapter(true)));
        C(!book.ConfirmConsumption(a,proof with{ReserveConsumed=30},new Adapter(true)));
        C(book.ConfirmConsumption(a,proof,new Adapter(true))&&book.PlannedReserve(a)==0);
        C(!book.ConfirmConsumption(a,proof,new Adapter(true))&&!book.ReserveFromCatalog("one",a,"ak47",new Catalog(2700),10,30,0));
        C(book.ReserveFromCatalog("two",a,"ak47",new Catalog(700),6,7,0)&&book.Balance==14830);
        C(book.CancelBeforeNative(a)&&book.Balance==14865&&!book.CancelBeforeNative(a));
        var pending=L();C(pending.ReserveFromCatalog("x",a,"ak47",new Catalog(700),6,7,0));pending.Invalidate();
        C(pending.Balance==15000&&!pending.Begin(a)&&pending.PlannedReserve(a)==0);
        var unknown=L();C(unknown.ReserveFromCatalog("x",a,"ak47",new Catalog(700),6,7,0)&&unknown.Begin(a));unknown.Invalidate();
        C(unknown.Balance==14965&&unknown.Current!.Status==ReloadPolicy.State.Unknown&&unknown.PlannedReserve(a)==0);
        C(!unknown.CancelBeforeNative(a)&&!unknown.ConfirmConsumption(a,proof,new Adapter(true)));
        C(!L(34).ReserveFromCatalog("poor",a,"ak47",new Catalog(700),6,7,0));
        foreach(var values in new[]{(-1,30,0),(30,30,0),(0,0,0),(10,30,1)}){
            var l=L();C(!l.ReserveFromCatalog("bad",a,"ak47",new Catalog(2700),values.Item1,values.Item2,values.Item3)&&l.Balance==15000);
        }
        C(!L().ReserveFromCatalog("unknown",a,"unknown",new Catalog(700),0,30,0));
        C(!L().ReserveFromCatalog("free",a,"ak47",new Catalog(0),0,30,0));
        C(!L().ReserveFromCatalog("utility",a,"smokegrenade",new Catalog(300),0,30,0));
        var partial=L();C(partial.ReserveFromCatalog("partial",a,"ak47",new Catalog(2700),25,30,0)&&partial.Begin(a));
        var pp=new ReloadPolicy.CommitProof("partial",7,123,456,25,30,5,"engine-op-partial");
        C(partial.ConfirmConsumption(a,pp,new Adapter(true))&&partial.Balance==14865);
        var concurrent=L();int wins=0;
        Parallel.For(0,64,i=>{if(concurrent.ReserveFromCatalog("race"+i,a,"ak47",new Catalog(2700),10,30,0))Interlocked.Increment(ref wins);});
        C(wins==1&&concurrent.Balance==14865);
        C(concurrent.Begin(a));int commits=0;
        var cp=new ReloadPolicy.CommitProof(concurrent.Current!.Input,7,123,456,10,30,20,"op-race");
        Parallel.For(0,64,i=>{if(concurrent.ConfirmConsumption(a,cp,new Adapter(true)))Interlocked.Increment(ref commits);});
        C(commits==1&&concurrent.Balance==14865);
        C(NativeBuyPolicy.MagazinePrice(709)==35&&NativeBuyPolicy.MagazinePrice(710)==36);
        Console.WriteLine($"{n} inactive reload policy checks passed");
    }
}
