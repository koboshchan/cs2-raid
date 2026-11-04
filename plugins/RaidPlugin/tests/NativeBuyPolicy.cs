using Cs2Raid;
static class NativeBuyPolicyTests
{
    public static void Run(){
        var n=0;void C(bool pass){n++;if(!pass)throw new Exception("native-buy-policy "+n);}
        NativeBuyPolicy.Actor A()=>new(7,123,true,true,true,true,true,true);
        NativeBuyPolicy L(long b=15000)=>new(7,123,b);
        var actor=A();
        foreach(var wrong in new[]{actor with{Generation=8},actor with{Controller=124},actor with{Dedicated=false},
            actor with{Scope=false},actor with{Human=false},actor with{Enrolled=false},actor with{Buy=false},actor with{Alive=false}}){
            var l=L();C(l.Reserve("bad",wrong,"ak47",2700)==null&&l.Balance==15000);
        }
        foreach(var item in new[]{"smokegrenade","flashbang","hegrenade","molotov","incgrenade","decoy","taser","defuser","weapon_ak47","knife","ak47;quit",""}){
            var l=L();C(l.Reserve("utility",actor,item,200)==null&&l.Balance==15000);
        }
        C(NativeBuyPolicy.Catalog.Contains("vest")&&NativeBuyPolicy.Catalog.Contains("vesthelm"));
        foreach(var item in NativeBuyPolicy.Catalog){var l=L();C(l.Reserve(item,actor,item,200)!=null&&l.Balance==14800);}
        var book=L();C(book.Reserve("one",actor,"ak47",2700)!=null&&book.Balance==12300);
        C(book.Reserve("one",actor,"ak47",2700)==null&&book.Reserve("two",actor,"awp",4750)==null);
        C(book.Begin("one",actor));C(!book.Begin("one",actor)&&!book.Cancel("one",actor));
        C(!book.Delivered("one",actor,9,false)&&!book.Delivered("one",actor with{Controller=2},9,true));
        C(book.Delivered("one",actor,9,true));C(!book.Delivered("one",actor,9,true)&&book.Balance==12300);
        C(book.Reserve("two",actor,"deagle",700)!=null&&book.Begin("two",actor));
        C(!book.Delivered("two",actor,9,true));C(book.Unknown("two")&&!book.Cancel("two",actor));
        C(book.Reserve("three",actor,"p250",300)==null&&book.Balance==11600);
        var cancel=L();C(cancel.Reserve("cancel",actor,"awp",4750)!=null&&cancel.Cancel("cancel",actor));
        C(!cancel.Cancel("cancel",actor)&&!cancel.Begin("cancel",actor)&&cancel.Balance==15000);
        var pending=L();C(pending.Reserve("reserved",actor,"ak47",2700)!=null);pending.Invalidate();
        C(pending.Balance==15000&&pending.Reserve("late",actor,"p250",300)==null&&!pending.Begin("reserved",actor));
        var attempted=L();C(attempted.Reserve("attempted",actor,"ak47",2700)!=null&&attempted.Begin("attempted",actor));
        attempted.Invalidate();C(attempted.Balance==12300&&attempted.Receipts.Single().State==NativeBuyPolicy.Outcome.Unknown);
        C(!attempted.Delivered("attempted",actor,10,true)&&!attempted.Cancel("attempted",actor));
        foreach(var balance in new long[]{0,1,65534,65535,65536,90000,long.MaxValue}){
            var l=L(balance);C(l.HudProjection==(int)Math.Min(balance,65535)&&l.Balance==balance);
        }
        C(L(2699).Reserve("short",actor,"ak47",2700)==null);
        C(L().Reserve("zero",actor,"ak47",0)==null&&L().Reserve("negative",actor,"ak47",-1)==null);
        C(NativeBuyPolicy.MagazinePrice(700)==35&&NativeBuyPolicy.MagazinePrice(5000)==250);
        C(NativeBuyPolicy.MagazinePrice(209)==10&&NativeBuyPolicy.MagazinePrice(210)==11);
        C(NativeBuyPolicy.MagazinePrice(1)==1&&NativeBuyPolicy.MagazinePrice(int.MaxValue)>0);
        for(int b=0;b<50;b++){
            C(NativeBuyPolicy.ReloadAffordable(b,700,0,0,7)==(b>=35));
            C(!NativeBuyPolicy.ReloadAffordable(b,700,1,0,7));
        }
        C(!NativeBuyPolicy.ReloadAffordable(1000,700,0,7,7));
        C(NativeBuyPolicy.Knives.SetEquals(new[]{"default","t","ct"}));
        C(NativeBuyPolicy.NeedsKnife(true,true,true,true,0));
        C(!NativeBuyPolicy.NeedsKnife(true,true,true,true,1)&&!NativeBuyPolicy.NeedsKnife(false,true,true,true,0));
        C(!NativeBuyPolicy.NeedsKnife(true,false,true,true,0)&&!NativeBuyPolicy.NeedsKnife(true,true,false,true,0));
        C(!NativeBuyPolicy.NeedsKnife(true,true,true,false,0));
        Console.WriteLine($"{n} stage2 inactive native-buy policy checks passed");
    }
}
