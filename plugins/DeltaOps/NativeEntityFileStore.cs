using System.Text.Json;
namespace DeltaOps;

// A single server-thread writer owns this path. File contents are identity records,
// not commands. Loading a corrupt/inaccessible file fails closed. Rename is atomic
// on the local filesystem; power-loss directory-entry durability is OS-dependent.
public sealed class NativeEntityFileStore(string path)
{
    private sealed record Snapshot(int Schema,NativeEntityTicket[] Tickets);
    public IReadOnlyList<NativeEntityTicket> Read()
    {
        if(!File.Exists(path))return Array.Empty<NativeEntityTicket>();
        var data=JsonSerializer.Deserialize<Snapshot>(File.ReadAllBytes(path))
            ??throw new InvalidDataException("Empty entity escrow");
        if(data.Schema!=1||data.Tickets==null)throw new InvalidDataException("Unsupported entity escrow");
        return Array.AsReadOnly(data.Tickets);
    }
    public void Write(IReadOnlyList<NativeEntityTicket> tickets)
    {
        var directory=Path.GetDirectoryName(Path.GetFullPath(path))!;
        Directory.CreateDirectory(directory);
        var temp=Path.Combine(directory,".deltaops-escrow-"+Guid.NewGuid().ToString("N")+".tmp");
        try{
            using(var stream=new FileStream(temp,FileMode.CreateNew,FileAccess.Write,FileShare.None)){
                JsonSerializer.Serialize(stream,new Snapshot(1,tickets.ToArray()));
                stream.Flush(flushToDisk:true);
            }
            File.Move(temp,path,overwrite:true);
        }finally{
            // Only our unique temporary name; never delete the prior escrow on failure.
            if(File.Exists(temp))File.Delete(temp);
        }
    }
}
