using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private sealed record BuySnapshot(string Steam,int Tick,long Balance,string Item);
    private readonly Dictionary<int,BuySnapshot> buySnapshots=[];
    private readonly Dictionary<nint,(CCSWeaponBaseVData Data,int Original)> priceOriginals=[];
    private static string BuyName(string name) => name.Replace("weapon_", "").Replace("item_", "") switch {
        "kevlar"=>"vest", "assaultsuit"=>"vesthelm", var n=>n
    };
    private ShopItem? PriceItem(string name) => catalog.FirstOrDefault(x=>BuyName(x.Entity)==BuyName(name));
    private void RegisterPricing()
    {
        AddCommandListener("buy",(p,c)=>{
            if(!Human(p))return HookResult.Continue;
            buySnapshots.Remove(p.Slot);
            var item=c.ArgCount==2?PriceItem(c.GetArg(1)):null;
            if(!sessionLoaded || game.Phase!=GamePhase.Active || !game.Living(Id(p)) || item==null || item.PriceDollars<0) {
                p.PrintToChat("Purchase unavailable.");return HookResult.Handled;
            }
            var balance=game.Players[Id(p)].Cash;
            if(balance<item.PriceDollars){p.PrintToChat($"{item.Id}: ${item.PriceDollars}, insufficient funds.");return HookResult.Handled;}
            buySnapshots[p.Slot]=new(Id(p),Server.TickCount,balance,BuyName(item.Entity));
            SetMoney(p,balance);
            return HookResult.Continue;
        },HookMode.Pre);
        RegisterEventHandler<EventItemPurchase>((e,i)=>{
            var p=e.Userid;
            if(!Human(p))return HookResult.Continue;
            var item=PriceItem(e.Weapon);
            if(item==null || !buySnapshots.Remove(p.Slot,out var snap) || snap.Steam!=Id(p) || snap.Tick!=Server.TickCount || snap.Item!=BuyName(e.Weapon)) {
                // Never invent a refund or award cash for an uncorrelated engine event.
                p.PrintToChat("Purchase not verified; no custom charge applied.");return HookResult.Continue;
            }
            if(!game.TryPurchase(Id(p),item.PriceDollars))return HookResult.Continue;
            SetMoney(p,game.Players[Id(p)].Cash);
            p.PrintToChat($"{(item.Id=="ak47" || item.Entity=="weapon_ak47"?"AK-47":item.Id)}: ${item.PriceDollars}");
            var steam=Id(p);var generation=mapGeneration;
            Server.NextFrame(()=>{if(generation==mapGeneration && sessionLoaded && game.Players.TryGetValue(steam,out var m)) {var current=Find(steam);if(current!=null)SetMoney(current,m.Cash);}});
            return HookResult.Continue;
        });
        RegisterListener<Listeners.OnEntitySpawned>(e=>{
            if(!e.DesignerName.StartsWith("weapon_",StringComparison.Ordinal))return;
            var handle=e.EntityHandle.Raw;var generation=mapGeneration;
            Server.NextFrame(()=>{if(generation!=mapGeneration)return;var entity=new CHandle<CCSWeaponBase>(handle).Value;if(entity is {IsValid:true})PatchPrice(entity);});
        });
        RegisterListener<Listeners.OnMapStart>(_=>{priceOriginals.Clear();buySnapshots.Clear();});
        Server.NextFrame(()=>{foreach(var item in catalog.Where(x=>x.Entity.StartsWith("weapon_",StringComparison.Ordinal)))foreach(var weapon in Utilities.FindAllEntitiesByDesignerName<CCSWeaponBase>(item.Entity))if(weapon.IsValid)PatchPrice(weapon);});
    }
    private void PatchPrice(CCSWeaponBase weapon)
    {
        var item=PriceItem(weapon.DesignerName);var data=weapon.VData;
        if(item==null || data==null || data.Handle==nint.Zero)return;
        priceOriginals.TryAdd(data.Handle,(data,data.Price));data.Price=item.PriceDollars;
    }
    private void ClearPricing()
    {
        foreach(var entry in priceOriginals.Values)entry.Data.Price=entry.Original;
        priceOriginals.Clear();buySnapshots.Clear();
    }
}
