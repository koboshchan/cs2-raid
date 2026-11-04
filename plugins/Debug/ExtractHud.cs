using System.Globalization;
namespace Cs2Debug;
public sealed record PreviewArgs(int Seconds,string Style,bool Stop)
{
    public static PreviewArgs? Parse(string[] args){
        if(args.Length==1&&args[0]=="stop")return new(0,"css",true);
        int seconds=300;string style="css";bool number=false,variant=false;
        foreach(var a in args){
            if(int.TryParse(a,NumberStyles.None,CultureInfo.InvariantCulture,out var n)&&!number&&n>=1&&n<=3600){seconds=n;number=true;}
            else if((a=="css"||a=="png")&&!variant){style=a;variant=true;}
            else return null;
        }
        return new(seconds,style,false);
    }
}
public sealed class PreviewClock
{
    public long End {get;}
    public bool Owned {get;private set;}
    private long next;
    private int color;
    public PreviewClock(long now,int seconds){End=now+seconds*1000L;}
    public static int Zone(double x,double y,double z){
        if(!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))return 0;
        if(x>=1051.424683&&x<=1266.525269&&y>=-6398.375488&&y<=-5561.906738)return 2;
        return x>=997.650&&x<=1320.300&&y>=-6607.493&&y<=-5352.790?1:0;
    }
    public int Remaining(long now)=>(int)Math.Clamp((End-now+999)/1000,0,3600);
    public string? Clear(bool channel){var had=Owned;Owned=false;return had&&channel?"":null;}
    public string? Tick(long now,int zone,bool valid,bool channel,string style){
        if(!valid||!channel||zone==0||now>=End)return Clear(channel);
        if(Owned&&now<next&&zone==color)return null;
        Owned=true;next=now+250;color=zone;
        return Markup(Remaining(now),zone==1,style);
    }
    public static string Markup(int n,bool red,string style){
        var c=red?"#FF3030":"#00FFAA";
        if(n<=10)return "<font color='"+c+"' size='48'>[ "+n.ToString(CultureInfo.InvariantCulture)+" ]</font><br><font color='"+c+"' size='20'>Extracting</font>";
        var text="<font color='"+c+"' size='20'>Extracting</font><br><font color='"+c+"' size='48'>"+(n/60).ToString("00",CultureInfo.InvariantCulture)+":"+(n%60).ToString("00",CultureInfo.InvariantCulture)+"</font>";
        // CSS is an explicit capability PROBE, not a promise of native support.
        if(style=="css")return "<div style='border:1px solid "+c+";background-color:rgba("+(red?"80,10,10":"10,50,45")+",0.7);text-align:center'>"+text+"</div>";
        // Fixed inline alpha PNG probe, no network/file path or user-controlled URL.
        var asset=red?"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR4nGMI4OJaAAACFwEFt0Mz/QAAAABJRU5ErkJggg==":"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR4nGPgMtJdAAABvQEKvwgQ/QAAAABJRU5ErkJggg==";
        return "<img src='data:image/png;base64,"+asset+"' width='240' height='100'><br>"+text;
    }
}

public static class PreviewActivation
{
    public static bool HudOnly(bool enabled,string target,int port,bool lan) =>
        enabled&&target=="192.168.1.162:27016"&&port==27016&&lan;
    public static bool Legacy(bool enabled,bool hudOnly)=>enabled&&!hudOnly;
}

public static class RaidHudGuard
{
    public static bool InactiveJson(string json){
        try{
            using var doc=System.Text.Json.JsonDocument.Parse(json);
            if(doc.RootElement.ValueKind!=System.Text.Json.JsonValueKind.Object)return false;
            var enabled=doc.RootElement.EnumerateObject().Where(p=>p.Name.Equals("enabled",StringComparison.OrdinalIgnoreCase)).ToArray();
            return enabled.Length==1&&enabled[0].Value.ValueKind==System.Text.Json.JsonValueKind.False;
        }catch{return false;}
    }
    public static bool ReadInactive(Func<string> read,bool verifiedNoRaid){
        try{return InactiveJson(read());}
        catch(FileNotFoundException){return verifiedNoRaid;}
        catch(DirectoryNotFoundException){return verifiedNoRaid;}
        catch{return false;}
    }
}

public sealed class PreviewNativeLifecycle
{
    public bool Available {get;private set;}
    public void OnTick()=>Available=true;
    public void Stop()=>Available=false;
    public void Clear(Action nativeClear,Action managedClear){
        var available=Available;
        managedClear(); // Cancellation precedes any potentially failing native access.
        if(available){
            try{nativeClear();}catch{Stop();throw;}
        }
    }
}

public static class PreviewCleanup
{
    public static void Each<T>(IEnumerable<T> detached,Action<T> clear){
        List<Exception>? errors=null;
        foreach(var item in detached){
            try{clear(item);}catch(Exception e){(errors??=[]).Add(e);}
        }
        if(errors!=null)throw new AggregateException("Owned preview cleanup failed after managed cancellation",errors);
    }
}
