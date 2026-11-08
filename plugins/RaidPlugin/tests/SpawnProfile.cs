using Cs2Raid;
using System.Text.Json;
public static class SpawnProfileTests
{
    public static void Run(){
        int checks=0;void Check(bool ok){checks++;if(!ok)throw new Exception("spawn profile assertion "+checks);}
        void Reject(string json){try{SpawnProfile.Parse(json);}catch(Exception ex)when(ex is InvalidOperationException or JsonException){checks++;return;}throw new Exception("accepted invalid spawn profile");}
        var json=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"spawn-profile.json"));
        var p=SpawnProfile.Parse(json);
        double[][] expected=[
            [7114.962402,8056.473633,-153.348633,-90],
            [10003.610352,2420.411133,-136.504883,180],
            [6658.519531,-3788.969727,-153.133789,135],
            [-9901.139648,-1260.527832,-25.809937,0],
            [-10078.707031,5226.147949,119.095291,0],
            [3431.539795,3946.397217,73.000488,0],
            [-1797.990479,7724.342773,223.606323,0]];
        Check(p.Spawns.Length==7);Check(!p.NativeValidated);
        for(int i=0;i<7;i++){
            var s=p.Spawns[i];var e=expected[i];
            Check(s.X==e[0]);Check(s.Y==e[1]);Check(s.Z==e[2]+100);Check(s.Yaw==e[3]);
            Check(s.Pitch==0&&s.Roll==0);Check(s.Valid);Check(s.Yaw%45==0);
            var native=s.NativePosition();Check(native.X==(float)e[0]&&native.Y==(float)e[1]&&native.Z==(float)(e[2]+100)&&native.Yaw==(float)e[3]);
        }
        Check(p.Spawns[0].X>0&&p.Spawns[0].Y>0&&p.Spawns[0].Z<0);
        Check(p.Spawns[3].X<0&&p.Spawns[3].Y<0); // No reflection, swap or handedness conversion.
        Check(p.Matches(SpawnProfile.Selection,p.MapId,p.Release));
        Check(p.Select(SpawnProfile.Selection,p.MapId,p.Release).Length==7);
        Check(p.Select(SpawnProfile.Selection,"de_dust2",p.Release).Length==0);
        Check(p.Select(SpawnProfile.Selection,p.MapId,"map-v1").Length==0);
        Check(p.Select(SpawnProfile.Selection,p.MapId,"").Length==0);
        Check(p.Select("other",p.MapId,p.Release).Length==0);
        Check(p.Select(SpawnProfile.Selection,p.MapId.ToUpperInvariant(),p.Release).Length==0);
        var roundtrip=SpawnProfile.Parse(JsonSerializer.Serialize(p));
        Check(roundtrip.Spawns.SequenceEqual(p.Spawns));
        Reject(json.Replace("default-20261105","map-v1"));Reject(json.Replace("df_spaceport_player_scale_raytrace","de_dust2"));
        Reject(json.Replace("7114.962402","1e400"));Reject(json.Replace("7114.962402","3.5e38"));
        Reject(json.Replace("\"pitch\": 0","\"pitch\": 1"));Reject(json.Replace("\"roll\": 0","\"roll\": 1"));
        Reject(json.Replace("\"yaw\": -90","\"yaw\": -91"));Reject(json.Replace("\"nativeValidated\": false","\"nativeValidated\": true"));
        Reject(json.Replace("USER_PROVIDED_SETPOS","TRANSFORMED"));Reject(json.Replace("native-cs2-setpos","df"));
        Reject(json.Replace("\"spawns\": [","\"spawns\": [null,"));
        var duo=new GameplayState();duo.Join("a");duo.Join("b");duo.ShootRequest("a","b");duo.ShootRequest("b","a");
        Check(!SpawnProfile.LayoutReady(duo.Players.Values));
        var solo=new GameplayState();solo.Join("a");solo.Solo("a");Check(SpawnProfile.LayoutReady(solo.Players.Values));
        var zone=p.ExtractionZone!;
        Check(zone.Valid&&zone.UnboundedZ);
        Check(zone.Vertices.Length==4&&p.WarningZone!.Vertices.Length==4);
        Check(zone.MinX==715.278076&&zone.MaxX==1484.720825&&zone.MinY==-6369.889648&&zone.MaxY==-5576.641113);
        var center=new NativeZoneVertex(1089.1912371274484,-5973.454242877722);
        // Translation-stable shoelace centroid, deliberately not the vertex mean.
        var origin=zone.Vertices[0];double twiceArea=0,cxNumerator=0,cyNumerator=0;
        for(var i=0;i<4;i++){
            var a=zone.Vertices[i];var b=zone.Vertices[(i+1)%4];
            var ax=a.X-origin.X;var ay=a.Y-origin.Y;var bx=b.X-origin.X;var by=b.Y-origin.Y;
            var cross=ax*by-bx*ay;
            twiceArea+=cross;cxNumerator+=(ax+bx)*cross;cyNumerator+=(ay+by)*cross;
        }
        Check(Math.Abs(origin.X+cxNumerator/(3*twiceArea)-center.X)<1e-9);
        Check(Math.Abs(origin.Y+cyNumerator/(3*twiceArea)-center.Y)<1e-9);
        Check(Math.Abs(zone.Vertices.Average(v=>v.X)-center.X)>1);
        for(var i=0;i<4;i++){
            var a=zone.Vertices[i];var b=zone.Vertices[(i+1)%4];var red=p.WarningZone!.Vertices[i];
            var mx=(a.X+b.X)/2;var my=(a.Y+b.Y)/2;
            var dx=b.X-a.X;var dy=b.Y-a.Y;var length=Math.Sqrt(dx*dx+dy*dy);
            Check(zone.Contains(mx-dy/length*.01,my+dx/length*.01,0));
            Check(!zone.Contains(mx+dy/length*.01,my-dx/length*.01,0));
            Check(Math.Abs(red.X-(4*a.X-3*center.X))<1e-9&&Math.Abs(red.Y-(4*a.Y-3*center.Y))<1e-9);
            foreach(var z in new[]{double.MinValue,-1e12,0,1e12,double.MaxValue}){
                Check(p.Zone(a.X,a.Y,z)==2);
                Check(p.Zone((a.X+b.X)/2,(a.Y+b.Y)/2,z)==2);
                Check(p.Zone(center.X,center.Y,z)==2);
                Check(p.Zone(red.X,red.Y,z)==1);
                var next=p.WarningZone.Vertices[(i+1)%4];
                Check(p.Zone((red.X+next.X)/2,(red.Y+next.Y)/2,z)==1);
            }
        }
        // Inside bounding boxes does not imply inside the rotated polygon.
        Check(!zone.Contains(720,-6360,0));Check(!zone.Contains(1480,-5580,0));
        Check(p.Zone(720,-6360,0)==1);Check(p.Zone(-400,-7550,0)==0);
        Check(p.Zone(500,-6000,0)==1);Check(p.Zone(0,0,0)==0);
        foreach(var bad in new[]{double.NaN,double.PositiveInfinity,double.NegativeInfinity}){
            Check(p.Zone(bad,center.Y,0)==0);Check(p.Zone(center.X,bad,0)==0);Check(p.Zone(center.X,center.Y,bad)==0);
        }
        Check(SpawnProfile.Parse(JsonSerializer.Serialize(p)).ExtractionZone!.Vertices.SequenceEqual(zone.Vertices));
        // Bbox metadata is not consulted for membership.
        Check((zone with {MinX=0,MaxX=0,MinY=0,MaxY=0}).Contains(center.X,center.Y,0));
        Check((zone with {Vertices=zone.Vertices.Reverse().ToArray()}).Contains(center.X,center.Y,0));
        Check(!(zone with {Vertices=[]}).Valid);
        Check(!(zone with {Vertices=[zone.Vertices[0],zone.Vertices[2],zone.Vertices[1],zone.Vertices[3]]}).Valid);
        Reject(json.Replace("\"unboundedZ\": true","\"unboundedZ\": false"));
        Reject(json.Replace("715.278076","1e400"));
        Reject(json.Replace("\"vertices\": [","\"vertices\": [null,"));
        // Legacy bounded radius and vertical distance are unchanged.
        var legacy=new GameplayConfig{ExtractionCenter=new(0,0,0),ExtractionRadius=10};
        Check(new Position(0,0,10).Within(legacy.ExtractionCenter,legacy.ExtractionRadius));
        Check(!new Position(0,0,11).Within(legacy.ExtractionCenter,legacy.ExtractionRadius));Check(legacy.Spawns.Length==0&&legacy.SpawnProfile==""&&legacy.MapRelease=="");
        Check(!legacy.Enabled&&!legacy.NativeFlowVerified&&!legacy.CoordinatesConfirmed&&!legacy.FinancialNativeVerified);
        Check(!legacy.Ready(1));
        Console.WriteLine($"{checks} qualified spawn profile checks passed");
    }
}
