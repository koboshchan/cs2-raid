using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace PriceTest;
public sealed class PriceTestPlugin : BasePlugin
{
    public override string ModuleName => "PriceTest";
    public override string ModuleVersion => "0.1.0";
    public override string ModuleAuthor => "koboshchan";
    public override string ModuleDescription => "Experimental sequential buy prices; server VData and purchase correction";
    public static readonly string[] Items = ("ak47 aug awp bizon cz75a deagle decoy defuser elite famas fiveseven flashbang g3sg1 galilar glock healthshot hegrenade hkp2000 incgrenade m249 m4a1 m4a1_silencer mac10 mag7 molotov mp5sd mp7 mp9 negev nova p250 p90 revolver sawedoff scar20 sg556 smokegrenade ssg08 taser tec9 ump45 usp_silencer vest vesthelm xm1014").Split(' ').Order(StringComparer.Ordinal).ToArray();
    private readonly Dictionary<string,int> prices = Items.Select((s,i)=>(s,i)).ToDictionary(x=>x.s,x=>x.i+1);
    private readonly Dictionary<nint,(CCSWeaponBaseVData Data,int Old)> originals = [];
    private readonly Dictionary<int,(CCSPlayerController Player,int Before,int Total,int Tick)> pending = [];
    private readonly Dictionary<int,(int Money,int Tick)> beforeBuy = [];
    private static string Normalize(string s) => s.Replace("weapon_","").Replace("item_","") switch { "kevlar"=>"vest", "assaultsuit"=>"vesthelm", var n=>n };
    public override void Load(bool hotReload)
    {
        AddCommand("css_prices","Show PriceTest mapping", (p,c) => {
            foreach(var x in prices) c.ReplyToCommand($"{x.Key} = ${x.Value}");
            c.ReplyToCommand("Server VData only. Client menu display must be checked by a real client.");
        });
        AddCommandListener("buy", (p,c) => {
            if(p is {IsValid:true,InGameMoneyServices:not null}) beforeBuy[p.Slot]=(p.InGameMoneyServices.Account,Server.TickCount);
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterEventHandler<EventItemPurchase>((e,i) => {
            var p=e.Userid;
            if(p is not {IsValid:true,InGameMoneyServices:not null} || !prices.TryGetValue(Normalize(e.Weapon),out var target)) return HookResult.Continue;
            if(!beforeBuy.Remove(p.Slot,out var pre) || pre.Tick!=Server.TickCount) {
                Logger.LogWarning("PriceTest purchase {Item} has no same-tick buy snapshot; no speculative refund",e.Weapon);
                return HookResult.Continue;
            }
            if(pending.TryGetValue(p.Slot,out var batch) && batch.Tick==Server.TickCount)
                pending[p.Slot]=(p,batch.Before,batch.Total+target,batch.Tick);
            else {
                pending[p.Slot]=(p,pre.Money,target,Server.TickCount);
                var slot=p.Slot;
                Server.NextFrame(()=> {
                    if(!pending.Remove(slot,out var b) || !b.Player.IsValid || b.Player!=p || p.InGameMoneyServices==null) return;
                    var actual=p.InGameMoneyServices.Account;
                    var desired=Math.Max(0,b.Before-b.Total);
                    p.InGameMoneyServices.Account=desired;
                    Utilities.SetStateChanged(p,"CCSPlayerController","m_pInGameMoneyServices");
                    Logger.LogInformation("PriceTest charge slot={Slot} before={Before} target={Target} nativeAfter={Actual} correctedAfter={Desired}",slot,b.Before,b.Total,actual,desired);
                });
            }
            return HookResult.Continue;
        });
        RegisterListener<Listeners.OnEntitySpawned>(entity=> {
            if(entity.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))
                Server.NextFrame(()=> { if(entity.IsValid) Patch(entity.As<CCSWeaponBase>()); });
        });
        RegisterListener<Listeners.OnMapStart>(map=> { originals.Clear(); pending.Clear(); beforeBuy.Clear(); AddTimer(2f,Initialize); });
        RegisterListener<Listeners.OnMapEnd>(()=> { originals.Clear(); pending.Clear(); beforeBuy.Clear(); });
        if(hotReload) Server.NextFrame(Initialize);
    }
    private void Patch(CCSWeaponBase weapon)
    {
        var name=Normalize(weapon.DesignerName);
        if(!prices.TryGetValue(name,out var value)) return;
        var data=weapon.VData;
        if(data==null || data.Handle==nint.Zero) return;
        if(!originals.ContainsKey(data.Handle)) originals[data.Handle]=(data,data.Price);
        data.Price=value;
        Logger.LogInformation("PriceTest VData {Name} price={Price} readback={Readback}",name,value,data.Price);
    }
    private void Initialize()
    {
        foreach(var x in prices) {
            Logger.LogInformation("PriceTest MAP {Name} = ${Price}",x.Key,x.Value);
            if(x.Key is "vest" or "vesthelm" or "defuser") continue;
            CCSWeaponBase? entity=null;
            try {
                entity=Utilities.CreateEntityByName<CCSWeaponBase>("weapon_"+x.Key);
                if(entity==null || !entity.IsValid) continue;
                entity.DispatchSpawn();
                Patch(entity);
            } catch(Exception ex) { Logger.LogWarning(ex,"PriceTest VData unavailable for {Item}; purchase fallback only",x.Key); }
            finally { if(entity is {IsValid:true}) entity.Remove(); }
        }
        Logger.LogInformation("PriceTest ready. {Count} stable prices. Server VData writes do NOT prove client buy menu display.",prices.Count);
    }
    public override void Unload(bool hotReload)
    {
        foreach(var v in originals.Values) v.Data.Price=v.Old;
        originals.Clear(); pending.Clear(); beforeBuy.Clear();
    }
}
