using Cs2Raid;
using PrivateRaidSafety;

static class PrivateRehearsalSafetyTests {
public static void Run(){
    var n=0;void C(bool ok){n++;if(!ok)throw new Exception("Private safety check "+n);}
    void Reject(Action f){var rejected=false;try{f();}catch(InvalidOperationException){rejected=true;}C(rejected);}
    var token=new HandoffToken(PrivateRaidHandoff.Map,Guid.NewGuid().ToString("N"));
    C(PrivateRaidHandoff.Matches(token,token));
    C(!PrivateRaidHandoff.Matches(token,token with{Nonce=Guid.NewGuid().ToString("N")}));
    C(!PrivateRaidHandoff.Matches(token,token with{Map="other"}));
    C(!PrivateRaidHandoff.Matches(null,token));
    C(!PrivateRaidHandoff.Matches(token with{Nonce="map-name"},token with{Nonce="map-name"}));
    var now=DateTime.UtcNow;
    C(PrivateRaidHandoff.Fresh(now,now.AddSeconds(-10),10));
    C(!PrivateRaidHandoff.Fresh(now,now.AddSeconds(-10.001),10));
    C(!PrivateRaidHandoff.Fresh(now,now.AddSeconds(1),10));
    var restored=0;var fail=true;var attempts=0;var errors=0;
    Dictionary<string,Action> entries=new(){
        ["ok"]=()=>restored++,
        ["retry"]=()=>{attempts++;if(fail)throw new InvalidOperationException();}
    };
    C(!PrivateRaidHandoff.RestorePending(entries,_=>{errors++;throw new Exception("logger");}));
    C(entries.Count==1&&entries.ContainsKey("retry")&&restored==1&&errors==1);
    fail=false;C(PrivateRaidHandoff.RestorePending(entries,_=>{}));C(attempts==2&&restored==1);
    var all=0;TestModeState.FinalizeAll(_=>throw new Exception(),()=>throw new Exception(),()=>all++,()=>throw new Exception(),()=>all++);
    C(all==2);
    var points=Enumerable.Range(0,16).SelectMany(i=>new[]{new Position(i*2000,0,0,37+i),new Position(i*2000+64,0,0,137+i)}).ToArray();
    PrivateRehearsalPolicy.ValidateSpawns(points);C(true);
    var teams=PrivateRehearsalPolicy.Allocate(points,new[]{2,2,2,1},new Random(17));
    C(teams.SelectMany(x=>x).Distinct().Count()==7);
    foreach(var t in teams){C(t.All(points.Contains));C(t.All(p=>p.Within(t[0],512)));C(t.All(p=>p.Yaw!=0));}
    Reject(()=>PrivateRehearsalPolicy.ValidateSpawns(points[..31]));
    var copy=points.ToArray();copy[1]=copy[0];Reject(()=>PrivateRehearsalPolicy.ValidateSpawns(copy));
    copy=points.ToArray();copy[0]=copy[0] with{Yaw=float.NaN};Reject(()=>PrivateRehearsalPolicy.ValidateSpawns(copy));
    copy=points.ToArray();copy[0]=copy[0] with{Z=float.PositiveInfinity};Reject(()=>PrivateRehearsalPolicy.ValidateSpawns(copy));
    var vertical=Enumerable.Range(0,32).Select(i=>new Position(0,0,i*1000,90)).ToArray();
    Reject(()=>PrivateRehearsalPolicy.Allocate(vertical,new[]{2},new Random(1)));
    Reject(()=>PrivateRehearsalPolicy.Allocate(points,new[]{3},new Random(1)));
    for(var seed=0;seed<32;seed++){
        var chosen=PrivateRehearsalPolicy.Allocate(points,new[]{2,2,2,1},new Random(seed)).SelectMany(x=>x).ToArray();
        C(chosen.Distinct().Count()==7);
    }
    C(PrivateRehearsalPolicy.GlockBullet(true,"weapon_glock"));
    C(!PrivateRehearsalPolicy.GlockBullet(false,"weapon_glock"));
    C(!PrivateRehearsalPolicy.GlockBullet(true,"weapon_m4a1"));
    C(!PrivateRehearsalPolicy.GlockBullet(true,null));
    foreach(var phase in Enum.GetValues<GamePhase>()){
        C(PrivateRehearsalPolicy.UnexpectedSpawn(phase,true,true)==(phase!=GamePhase.Lobby));
        C(!PrivateRehearsalPolicy.UnexpectedSpawn(phase,false,true));
        C(!PrivateRehearsalPolicy.UnexpectedSpawn(phase,true,false));
    }
    // Isolated file protocol, never touch the real /tmp marker paths.
    var dir=Path.Combine(Path.GetTempPath(),"raid-handoff-tests-"+Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(dir);
    try{
        var file=Path.Combine(dir,"echo");
        PrivateRaidHandoff.Write(file,"old");PrivateRaidHandoff.Write(file,"new");
        C(File.ReadAllText(file)=="new");C(Directory.GetFiles(dir).Length==1);
    }finally{Directory.Delete(dir,true);}
    Console.WriteLine($"{n} private rehearsal safety checks passed (pure policy/protocol, not native QA)");
}}
