using System.Globalization;
namespace Cs2Debug;
public sealed record NativeZoneVertex(double X,double Y);
// Ordered simple polygon is authoritative; Min/Max fields are compatibility metadata only.
// Z has no height bound, but all position components must be finite.
public sealed record NativeExtractionZone(double MinX,double MaxX,double MinY,double MaxY,bool UnboundedZ)
{
    public NativeZoneVertex[] Vertices {get;init;}=[];
    private static double Cross(NativeZoneVertex a,NativeZoneVertex b,double x,double y) =>
        (b.X-a.X)*(y-a.Y)-(b.Y-a.Y)*(x-a.X);
    public bool Valid {
        get {
            if(!UnboundedZ||Vertices is not {Length:4}||
                Vertices.Any(v=>v==null||!double.IsFinite(v.X)||!double.IsFinite(v.Y)||Math.Abs(v.X)>float.MaxValue||Math.Abs(v.Y)>float.MaxValue)||
                Vertices.Distinct().Count()!=4)return false;
            var origin=Vertices[0];
            var area=0d;
            for(var i=0;i<4;i++)area+=Cross(origin,Vertices[i],Vertices[(i+1)%4].X,Vertices[(i+1)%4].Y);
            if(!double.IsFinite(area)||area==0)return false;
            // Opposite edge intersections reject a self-crossing quad.
            for(var i=0;i<2;i++){
                var a=Vertices[i];var b=Vertices[i+1];var c=Vertices[i+2];var d=Vertices[(i+3)%4];
                var abC=Cross(a,b,c.X,c.Y);var abD=Cross(a,b,d.X,d.Y);
                var cdA=Cross(c,d,a.X,a.Y);var cdB=Cross(c,d,b.X,b.Y);
                if(Math.Sign(abC)!=Math.Sign(abD)&&Math.Sign(cdA)!=Math.Sign(cdB))return false;
            }
            return true;
        }
    }
    public bool Contains(double x,double y,double z){
        if(!Valid||!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))return false;
        var inside=false;
        for(var i=0;i<Vertices.Length;i++){
            var a=Vertices[i];var b=Vertices[(i+1)%Vertices.Length];
            var left=(b.X-a.X)*(y-a.Y);var right=(b.Y-a.Y)*(x-a.X);
            // Only arithmetic roundoff allowance, not a gameplay-sized zone expansion.
            var error=8*2.220446049250313e-16*(Math.Abs(left)+Math.Abs(right));
            if(Math.Abs(left-right)<=error&&x>=Math.Min(a.X,b.X)&&x<=Math.Max(a.X,b.X)&&
                y>=Math.Min(a.Y,b.Y)&&y<=Math.Max(a.Y,b.Y))return true;
            if((a.Y>y)!=(b.Y>y)&&x<(b.X-a.X)*(y-a.Y)/(b.Y-a.Y)+a.X)inside=!inside;
        }
        return inside;
    }
}
public sealed record PreviewArgs(int Seconds,bool Stop)
{
    // Only "[seconds]" (1..3600, default 300) or "stop". Anything else is rejected.
    public static PreviewArgs? Parse(string[] args){
        if(args.Length==0)return new(300,false);
        if(args.Length!=1)return null;
        if(args[0]=="stop")return new(0,true);
        return int.TryParse(args[0],NumberStyles.None,CultureInfo.InvariantCulture,out var n)&&n>=1&&n<=3600?new(n,false):null;
    }
}
public sealed class PreviewClock
{
    public long End {get;}
    public bool Owned {get;private set;}
    private string? lastMarkup;
    public PreviewClock(long now,int seconds){End=now+seconds*1000L;}
    // Same embedded release-qualified JSON authority as Raid, no independently rounded HUD bounds.
    private static readonly System.Text.Json.JsonDocument ZoneProfile=System.Text.Json.JsonDocument.Parse(
        typeof(PreviewClock).Assembly.GetManifestResourceStream("SpaceportZoneProfile")
        ??throw new InvalidOperationException("Missing release zone authority"));
    private static readonly NativeExtractionZone? Green=ReadZone("extractionZone");
    private static readonly NativeExtractionZone? Red=ReadZone("warningZone");
    private static NativeExtractionZone? ReadZone(string key) => System.Text.Json.JsonSerializer.Deserialize<NativeExtractionZone>(
        ZoneProfile.RootElement.GetProperty(key).GetRawText(),new System.Text.Json.JsonSerializerOptions{PropertyNameCaseInsensitive=true});
    public static int Zone(double x,double y,double z){
        if(!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))return 0;
        var root=ZoneProfile.RootElement;
        if(root.GetProperty("mapId").GetString()!="df_spaceport_player_scale_raytrace"||
           root.GetProperty("release").GetString()!="default-20261105")return 0;
        return Green?.Contains(x,y,z)==true?2:Red?.Contains(x,y,z)==true?1:0;
    }

    public int Remaining(long now)=>(int)Math.Clamp((End-now+999)/1000,0,3600);
    public string? Clear(bool channel){var had=Owned;Owned=false;lastMarkup=null;return had&&channel?"":null;}
    // Returns alert text only when the displayed text changes (about once per second); "" means owned clear, never sent.
    public string? Tick(long now,int zone,bool valid,bool channel){
        if(!valid||!channel||zone==0||now>=End)return Clear(channel);
        var markup=Markup(Remaining(now),zone==1);
        if(Owned&&lastMarkup==markup)return null;
        Owned=true;lastMarkup=markup;
        return markup;
    }
    public static string Markup(int n,bool red){
        var clock=(n/60).ToString("00",CultureInfo.InvariantCulture)+":"+(n%60).ToString("00",CultureInfo.InvariantCulture);
        return red?"WARNING "+clock:clock+" Extracting";
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
