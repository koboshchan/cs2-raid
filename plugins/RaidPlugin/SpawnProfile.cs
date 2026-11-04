using System.Text.Json;
namespace Cs2Raid;
public sealed record NativeSpawn(double X,double Y,double Z,double Yaw,double Pitch,double Roll)
{
    public bool Valid => new[]{X,Y,Z,Yaw,Pitch,Roll}.All(double.IsFinite)&&
        new[]{X,Y,Z}.All(v=>Math.Abs(v)<=float.MaxValue)&&Pitch==0&&Roll==0&&Yaw%45==0;
    // Engine vectors are float32. Preserve source doubles until this native boundary.
    public Position NativePosition() => new((float)X,(float)Y,(float)Z,(float)Yaw);
}
// Inclusive native XY rectangle. Z must be finite input, but has no height limit.
// No yaw field: facing is irrelevant to eligibility.
public sealed record NativeExtractionZone(double MinX,double MaxX,double MinY,double MaxY,bool UnboundedZ)
{
    public bool Valid => UnboundedZ && new[]{MinX,MaxX,MinY,MaxY}.All(double.IsFinite)&&MinX<=MaxX&&MinY<=MaxY;
    public bool Contains(double x,double y,double z) => Valid&&double.IsFinite(x)&&double.IsFinite(y)&&double.IsFinite(z)&&
        x>=MinX&&x<=MaxX&&y>=MinY&&y<=MaxY;
    public static NativeExtractionZone FromCorners(double x1,double y1,double x2,double y2) =>
        new(Math.Min(x1,x2),Math.Max(x1,x2),Math.Min(y1,y2),Math.Max(y1,y2),true);
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
    public static SpawnProfile Parse(string json) {
        var p=JsonSerializer.Deserialize<SpawnProfile>(json,new JsonSerializerOptions{PropertyNameCaseInsensitive=true})
            ??throw new InvalidOperationException("Missing spawn profile");
        if(p.MapId!="df_spaceport_player_scale_raytrace"||p.Release!="default-20261105"||
            p.Provenance!="USER_PROVIDED_SETPOS"||p.CoordinateSpace!="native-cs2-setpos"||p.NativeValidated||
            p.ExtractionZone is not {Valid:true}||p.Spawns==null||p.Spawns.Length!=7||p.Spawns.Any(s=>s==null||!s.Valid))
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
