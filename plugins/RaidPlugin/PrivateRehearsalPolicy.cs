namespace Cs2Raid;

public static class PrivateRehearsalPolicy
{
    public static void ValidateSpawns(IReadOnlyList<Position> spawns) {
        if(spawns.Count!=32 || spawns.Any(p=>!p.Valid) ||
           spawns.Select(p=>(p.X,p.Y,p.Z)).Distinct().Count()!=spawns.Count)
            throw new InvalidOperationException("Require 32 finite, unique native spawn points with authored yaw.");
    }
    public static Position[][] Allocate(IReadOnlyList<Position> spawns,int[] sizes,Random random) {
        ValidateSpawns(spawns);
        if(sizes.Any(n=>n is <1 or >2))throw new InvalidOperationException("Invalid raid team size.");
        var remaining=spawns.OrderBy(_=>random.Next()).ToList();
        var result=new List<Position[]>();
        foreach(var size in sizes) {
            // A team must fit within a real 512-unit 3D spawn cluster. Never invent offsets.
            var first=remaining.FirstOrDefault(p=>remaining.Count(q=>q.Within(p,512))>=size)
                ??throw new InvalidOperationException("Insufficient real same-cluster spawn points.");
            var selected=remaining.Where(p=>p.Within(first,512))
                .OrderBy(p=>Math.Pow(p.X-first.X,2)+Math.Pow(p.Y-first.Y,2)+Math.Pow(p.Z-first.Z,2)).Take(size).ToArray();
            result.Add(selected);
            remaining.RemoveAll(p=>selected.Contains(p)||p.Within(first,512));
        }
        return result.ToArray();
    }
    public static bool UnexpectedSpawn(GamePhase phase,bool living,bool pawnAlive)=>phase!=GamePhase.Lobby&&living&&pawnAlive;
    public static bool GlockBullet(bool bullet,string? weapon)=>bullet && weapon=="weapon_glock";
}
