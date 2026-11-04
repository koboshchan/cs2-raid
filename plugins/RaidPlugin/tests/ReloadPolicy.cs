using Cs2Raid;
static class ReloadPolicyTests
{
    public static void Run(){
        int n=0;void C(bool ok){n++;if(!ok)throw new Exception("reload policy "+n);}
        var a=new ReloadPolicy.Actor(7,123,456,true,true,true,true,true);
        ReloadPolicy L(long b=15000)=>new(a,b);
        foreach(var other in new[]{a with{Generation=8},a with{Controller=124},a with{Weapon=457},
            a with{Dedicated=false},a with{Scope=false},a with{Human=false},a with{Enrolled=false},a with{Alive=false}}){
            var l=L();C(!l.Reserve("x",other,2700,10,30,0)&&l.Balance==15000);
        }
        var book=L();C(book.Reserve("one",a,2700,10,30,0)&&book.Balance==14865);
        C(!book.Reserve("two",a,2700,10,30,0));C(book.PlannedReserve(a)==0);
        C(book.Begin(a)&&book.PlannedReserve(a)==30);C(!book.Begin(a)&&!book.CancelBeforeNative(a));
        C(!book.ConfirmConsumption(a,false,true)&&!book.ConfirmConsumption(a,true,false));
        C(book.ConfirmConsumption(a,true,true)&&book.PlannedReserve(a)==0);
        C(!book.ConfirmConsumption(a,true,true)&&!book.Reserve("one",a,2700,10,30,0));
        C(book.Reserve("two",a,700,6,7,0)&&book.Balance==14830);
        C(book.CancelBeforeNative(a)&&book.Balance==14865&&!book.CancelBeforeNative(a));
        var pending=L();C(pending.Reserve("x",a,700,6,7,0));pending.Invalidate();
        C(pending.Balance==15000&&!pending.Begin(a)&&pending.PlannedReserve(a)==0);
        var unknown=L();C(unknown.Reserve("x",a,700,6,7,0)&&unknown.Begin(a));unknown.Invalidate();
        C(unknown.Balance==14965&&unknown.Current!.Status==ReloadPolicy.State.Unknown&&unknown.PlannedReserve(a)==0);
        C(!unknown.CancelBeforeNative(a)&&!unknown.ConfirmConsumption(a,true,true));
        C(!L(34).Reserve("poor",a,700,6,7,0));
        foreach(var values in new[]{(-1,30,0),(30,30,0),(0,0,0),(10,30,1)}){
            var l=L();C(!l.Reserve("bad",a,2700,values.Item1,values.Item2,values.Item3)&&l.Balance==15000);
        }
        C(NativeBuyPolicy.MagazinePrice(709)==35&&NativeBuyPolicy.MagazinePrice(710)==36);
        Console.WriteLine($"{n} inactive reload policy checks passed");
    }
}
