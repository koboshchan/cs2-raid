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
            Check(s.X==e[0]);Check(s.Y==e[1]);Check(s.Z==e[2]);Check(s.Yaw==e[3]);
            Check(s.Pitch==0&&s.Roll==0);Check(s.Valid);Check(s.Yaw%45==0);
            var native=s.NativePosition();Check(native.X==(float)e[0]&&native.Y==(float)e[1]&&native.Z==(float)e[2]&&native.Yaw==(float)e[3]);
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
        var legacy=new GameplayConfig();Check(legacy.Spawns.Length==0&&legacy.SpawnProfile==""&&legacy.MapRelease=="");
        Check(!legacy.Enabled&&!legacy.NativeFlowVerified&&!legacy.CoordinatesConfirmed&&!legacy.FinancialNativeVerified);
        Check(!legacy.Ready(1));
        Console.WriteLine($"{checks} qualified spawn profile checks passed");
    }
}
