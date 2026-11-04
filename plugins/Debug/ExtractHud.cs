using System.Globalization;
namespace Cs2Debug;
public sealed record PreviewArgs(int Seconds,string Style,bool Stop)
{
    public static PreviewArgs? Parse(string[] args){
        if(args.Length==1&&args[0]=="stop")return new(0,"css",true);
        int seconds=300;string style="css";bool number=false,variant=false;
        foreach(var a in args){
            if(int.TryParse(a,NumberStyles.None,CultureInfo.InvariantCulture,out var n)&&!number&&n>=1&&n<=3600){seconds=n;number=true;}
            else if((a=="css"||a=="png"||a=="plain"||a=="tags"||a=="classes"||a=="alert"||a=="world")&&!variant){style=a;variant=true;}
            else return null;
        }
        return new(seconds,style,false);
    }
}
public sealed class PreviewClock
{
    public long End {get;}
    public bool Owned {get;private set;}
    private long keepaliveDeadline;
    private long lastDispatch=long.MinValue/2;
    private readonly int htmlKeepaliveMs;
    private int color;
    private string? lastMarkup;
    public PreviewClock(long now,int seconds,int htmlKeepaliveMs=200){End=now+seconds*1000L;this.htmlKeepaliveMs=Math.Clamp(htmlKeepaliveMs,100,500);}
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
        // Requested native Duration2 is not evidence of actual client lifetime.
        // Separate changed content from empirical HTML renewal; cap sends to20/s PER PREVIEW (unchanged default HTML renews at5/s).
        var html=style!="alert"&&style!="world";
        var changed=!Owned||lastMarkup!=markup;
        var renew=html&&now>=keepaliveDeadline;
        if(!changed&&!renew)return null;
        if(html&&now-lastDispatch<50)return null; // No catch-up burst; next tick uses current content.
        Owned=true;lastDispatch=now;keepaliveDeadline=now+htmlKeepaliveMs;color=zone;lastMarkup=markup;
        return markup;
    }
    public static string Markup(int n,bool red,string style){
        if(style=="alert")return (red?"WARNING ":"EXTRACT ")+(n<=10?"[ "+n.ToString(CultureInfo.InvariantCulture)+" ]":(n/60).ToString("00",CultureInfo.InvariantCulture)+":"+(n%60).ToString("00",CultureInfo.InvariantCulture))+" Extracting";
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

public static class PreviewDismiss
{
    // These are experiments, NOT claimed full-dismiss semantics.
    public static int? ProbeDuration(string mode)=>mode=="clearblank"?1:mode=="clearttl"?0:null;
    public static bool ShouldRender(string? text)=>!string.IsNullOrEmpty(text);
}

public static class PreviewDispatch
{
    public static bool IsHtml(string style)=>style!="alert";
    public static void Send(string style,string? message,Action<string> html,Action<string> alert){
        if(!PreviewDismiss.ShouldRender(message))return;
        if(IsHtml(style))html(message!);else alert(message!);
    }
}

// Pure camera-relative placement, intended upper view, client appearance is unverified.
public static class WorldProbeGeometry
{
    public static (float X,float Y,float Z) Place(float x,float y,float z,float pitch,float yaw){
        if(!float.IsFinite(x)||!float.IsFinite(y)||!float.IsFinite(z)||!float.IsFinite(pitch)||!float.IsFinite(yaw))throw new ArgumentException("Nonfinite eyes");
        var p=pitch*Math.PI/180;var a=yaw*Math.PI/180;
        return ((float)(x+48*Math.Cos(p)*Math.Cos(a)+12*Math.Sin(p)*Math.Cos(a)),
                (float)(y+48*Math.Cos(p)*Math.Sin(a)+12*Math.Sin(p)*Math.Sin(a)),
                (float)(z-48*Math.Sin(p)+12*Math.Cos(p)));
    }
    public static string Text(int n,bool red)=>(n<=10?"[ "+n.ToString(CultureInfo.InvariantCulture)+" ]":(n/60).ToString("00",CultureInfo.InvariantCulture)+":"+(n%60).ToString("00",CultureInfo.InvariantCulture))+"\n"+(red?"WARNING":"Extracting");
}
