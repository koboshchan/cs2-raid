using System.Globalization;
namespace Cs2Debug;
public sealed record PreviewArgs(int Seconds,string Style,bool Stop)
{
    public static PreviewArgs? Parse(string[] args){
        if(args.Length==1&&args[0]=="stop")return new(0,"css",true);
        int seconds=300;string style="css";bool number=false,variant=false;
        foreach(var a in args){
            if(int.TryParse(a,NumberStyles.None,CultureInfo.InvariantCulture,out var n)&&!number&&n>=1&&n<=3600){seconds=n;number=true;}
            else if((a=="css"||a=="png"||a=="plain"||a=="tags"||a=="classes")&&!variant){style=a;variant=true;}
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
    private string? lastMarkup;
    public PreviewClock(long now,int seconds){End=now+seconds*1000L;}
    // Same embedded release-qualified JSON authority as Raid, no independently rounded HUD bounds.
    private static readonly System.Text.Json.JsonDocument ZoneProfile=System.Text.Json.JsonDocument.Parse(
        typeof(PreviewClock).Assembly.GetManifestResourceStream("SpaceportZoneProfile")
        ??throw new InvalidOperationException("Missing release zone authority"));
    public static int Zone(double x,double y,double z){
        if(!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))return 0;
        var root=ZoneProfile.RootElement;
        if(root.GetProperty("mapId").GetString()!="df_spaceport_player_scale_raytrace"||
           root.GetProperty("release").GetString()!="default-20261105")return 0;
        bool Contains(string key){var r=root.GetProperty(key);return r.GetProperty("unboundedZ").GetBoolean()&&
            x>=r.GetProperty("minX").GetDouble()&&x<=r.GetProperty("maxX").GetDouble()&&
            y>=r.GetProperty("minY").GetDouble()&&y<=r.GetProperty("maxY").GetDouble();}
        return Contains("extractionZone")?2:Contains("warningZone")?1:0;
    }
    public int Remaining(long now)=>(int)Math.Clamp((End-now+999)/1000,0,3600);
    public string? Clear(bool channel){var had=Owned;Owned=false;lastMarkup=null;return had&&channel?"":null;}
    public string? Tick(long now,int zone,bool valid,bool channel,string style){
        if(!valid||!channel||zone==0||now>=End)return Clear(channel);
        var markup=Markup(Remaining(now),zone==1,style);
        // Content changes are immediate; unchanged content renews only at Duration2 minus0.5s.
        if(Owned&&lastMarkup==markup&&now<next)return null;
        Owned=true;next=now+1500;color=zone;lastMarkup=markup;
        return markup;
    }
    public static string Markup(int n,bool red,string style){
        var c=red?"#FF0000":"#00FF00";
        var clock=n<=10?"[ "+n.ToString(CultureInfo.InvariantCulture)+" ]":(n/60).ToString("00",CultureInfo.InvariantCulture)+":"+(n%60).ToString("00",CultureInfo.InvariantCulture);
        if(style=="plain")return clock+"<br>Extracting";
        if(style=="tags")return "<font color='"+c+"'><b>"+clock+"</b><br>Extracting</font>";
        if(style=="classes")return "<font color='"+c+"' class='fontSize-xxl fontWeight-Bold'>"+clock+"</font><br><font color='"+c+"' class='fontSize-s'>Extracting</font>";
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
