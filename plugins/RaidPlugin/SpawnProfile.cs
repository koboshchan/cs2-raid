using System.Text.Json;
namespace Cs2Raid;
public sealed record NativeSpawn(double X,double Y,double Z,double Yaw,double Pitch,double Roll)
{
    public bool Valid => new[]{X,Y,Z,Yaw,Pitch,Roll}.All(double.IsFinite)&&
        new[]{X,Y,Z}.All(v=>Math.Abs(v)<=float.MaxValue)&&Pitch==0&&Roll==0&&Yaw%45==0;
    // Engine vectors are float32. Preserve source doubles until this native boundary.
    public Position NativePosition() => new((float)X,(float)Y,(float)Z,(float)Yaw);
}
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
public sealed class SpawnProfile
{
    public const string Selection="spaceport-default-20261105";
    public string MapId {get;set;}="";
    public string Release {get;set;}="";
    public string Provenance {get;set;}="";
    public string CoordinateSpace {get;set;}="";
    public bool NativeValidated {get;set;}
    public NativeSpawn[] Spawns {get;set;}=[];
    public NativeExtractionZone? ExtractionZone {get;set;}
    public NativeExtractionZone? WarningZone {get;set;}
    // Warning must never grant extraction; overlap, including green boundary, stays green.
    public int Zone(double x,double y,double z) =>
        ExtractionZone?.Contains(x,y,z)==true?2:WarningZone?.Contains(x,y,z)==true?1:0;
    public static SpawnProfile Parse(string json) {
        var p=JsonSerializer.Deserialize<SpawnProfile>(json,new JsonSerializerOptions{PropertyNameCaseInsensitive=true})
            ??throw new InvalidOperationException("Missing spawn profile");
        if(p.MapId!="df_spaceport_player_scale_raytrace"||p.Release!="default-20261105"||
            p.Provenance!="USER_PROVIDED_SETPOS"||p.CoordinateSpace!="native-cs2-setpos"||p.NativeValidated||
            p.ExtractionZone is not {Valid:true}||p.WarningZone is not {Valid:true}||p.Spawns==null||p.Spawns.Length!=7||p.Spawns.Any(s=>s==null||!s.Valid))
            throw new InvalidOperationException("Invalid unverified native spawn profile");
        return p;
    }
    // No user-provided duo offsets exist. Do not stack or invent positions for duos.
    public static bool LayoutReady(IEnumerable<Member> players) =>
        players.Where(p=>p.Team!=null).GroupBy(p=>p.Team).All(group=>group.Count()==1);
    public bool Matches(string selection,string actualMap,string configuredRelease) =>
        selection==Selection&&actualMap==MapId&&configuredRelease==Release;
    public Position[] Select(string selection,string actualMap,string configuredRelease) =>
        Matches(selection,actualMap,configuredRelease)?Spawns.Select(s=>s.NativePosition()).ToArray():[];
}
