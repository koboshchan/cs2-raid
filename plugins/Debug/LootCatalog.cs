using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
namespace Cs2Debug;
public sealed record LootVisual(string Id,string Asset,string? Model,double X,double Y,double Z);
public static class LootCatalog
{
    public const string Map="df_spaceport_player_scale_raytrace",Release="default-20261105";
    public static readonly IReadOnlyList<LootVisual> Entries=Load();
    private static IReadOnlyList<LootVisual> Load(){
        var a=Assembly.GetExecutingAssembly();using var raw=a.GetManifestResourceStream("LootPlacements")!;
        using var bytes=new MemoryStream();raw.CopyTo(bytes);var data=bytes.ToArray();
        using var resources=JsonDocument.Parse(a.GetManifestResourceStream("LootResources")!);
        var r=resources.RootElement;
        if(r.GetProperty("sourceCommit").GetString()!="03ac1cc3e88b4815c5c561929b97121abf4565d8"||r.GetProperty("sourceSha256").GetString()!=Convert.ToHexStringLower(SHA256.HashData(data))||r.GetProperty("map").GetString()!=Map||r.GetProperty("release").GetString()!=Release)throw new InvalidDataException("Loot provenance");
        var mapping=r.GetProperty("models").EnumerateArray().ToDictionary(x=>x.GetProperty("asset").GetString()!,x=>x.GetProperty("resourcePath").GetString());
        using var doc=JsonDocument.Parse(data);var d=doc.RootElement;
        if(d.GetProperty("loot").GetArrayLength()!=247||d.GetProperty("excluded_loot").GetArrayLength()!=88||d.GetProperty("cs2_transform").GetProperty("status").GetString()!="OCEAN_FULL40_EXACT_CELL_TRANSFORM")throw new InvalidDataException("Loot schema/count");
        var result=new List<LootVisual>();var ids=new HashSet<string>();
        foreach(var e in d.GetProperty("loot").EnumerateArray()){
            var id=e.GetProperty("id").GetString()!;var asset=e.GetProperty("asset").GetString()!;
            var p=e.GetProperty("cs2_position");if(p.GetArrayLength()!=3||!ids.Add(id)||!mapping.ContainsKey(asset))throw new InvalidDataException("Loot identity");
            var xyz=p.EnumerateArray().Select(x=>x.GetDouble()).ToArray();if(xyz.Any(x=>!double.IsFinite(x)||Math.Abs(x)>float.MaxValue))throw new InvalidDataException("Loot coordinate");
            if(e.GetProperty("rotation").ValueKind!=JsonValueKind.Null)throw new InvalidDataException("Unexpected assigned rotation");
            var model=mapping[asset];if(model!=null&&!SafeModel(model))throw new InvalidDataException("Unsafe model");
            result.Add(new(id,asset,model,xyz[0],xyz[1],xyz[2]));
        }
        return result.AsReadOnly();
    }
    public static bool SafeModel(string model)=>model.StartsWith("models/",StringComparison.Ordinal)&&model.EndsWith(".vmdl",StringComparison.Ordinal)&&!model.Contains("..")&&!model.Contains('\\')&&!model.Contains(':')&&!model.Any(char.IsControl);
    public static bool MapAllowed(string map,string release)=>map==Map&&release==Release;
}
