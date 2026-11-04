using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
namespace Cs2Raid;
public sealed partial class RaidPlugin
{
    private DateTimeOffset testBannerUntil;
    private readonly Dictionary<uint,(IntPtr Services,int Cash)> testCash=[];
    private void RestoreTestCash(uint? only=null){
        var entries=only is {} raw?testCash.Where(x=>x.Key==raw).ToDictionary():testCash;
        TestModeState.RestoreEntries(entries,(raw,saved)=>{
            var p=new CHandle<CCSPlayerController>(raw).Value;
            if(p is {IsValid:true}&&p.InGameMoneyServices is {} money&&money.Handle==saved.Services){
                money.Account=saved.Cash;Utilities.SetStateChanged(p,"CCSPlayerController","m_pInGameMoneyServices");
            }
            testCash.Remove(raw);
        },TestCleanupError);
    }
    private bool QueueLobbyWeaponCleanup(CCSPlayerController p){
        try{
        // Snapshot only this pawn's tracked fixture entities BEFORE engine death-drop.
        var owned=p.PlayerPawn.Value?.WeaponServices?.MyWeapons.Select(h=>h.Raw).Where(testWeapons.Contains).ToArray()??[];
        var generation=testMode.Generation;
        Server.NextFrame(()=>{
            try{
                if(!testMode.CallbackAllowed(generation,testUnloaded)||!TestScope())return;
                foreach(var raw in owned){
                    if(!testWeapons.Contains(raw))continue;
                    var gun=new CHandle<CCSWeaponBase>(raw).Value;
                    if(gun is {IsValid:true})gun.Remove();
                    testWeapons.Remove(raw);
                }
            }catch(Exception ex){FailTestNative(ex);}
        });
        return true;
        }catch(Exception ex){FailTestNative(ex);return false;}
    }
    private static string FixturePrompt(GamePhase phase)=>phase switch{
        GamePhase.Lobby=>"Choose a partner or !team single",
        GamePhase.CharacterSelect=>"Select your operator: !characterselect 1..6",
        GamePhase.Buy=>"BUY PHASE | finite fixture credits",
        GamePhase.Countdown=>"Prepare for insertion",
        GamePhase.Freeze=>"Insertion grace period",
        GamePhase.Active=>"RAID ACTIVE",
        _=>"RAID CLOSED"
    };
    private void RegisterTestPresentation(){
        if(!TestServer)return;
        AddCommand("css_knife","Fixture stock knife: !knife default|t|ct",(p,c)=>{
            if(!TestPlayerCommand(p,c)||!p!.PawnIsAlive)return;
            var choice=c.ArgCount==2?c.GetArg(1):"default";
            if(!NativeBuyPolicy.Knives.Contains(choice)){c.ReplyToCommand("Only default, t, ct are supported; no arbitrary item ids.");return;}
            try{
                var knife=FindFixtureKnife(p);
                // Existing untracked native knives are never destroyed by a cosmetic request.
                if(knife!=null&&!testWeapons.Contains(knife.EntityHandle.Raw)){c.ReplyToCommand("Existing native knife retained.");return;}
                if(knife!=null){var raw=knife.EntityHandle.Raw;knife.Remove();testWeapons.Remove(raw);}
                GiveFixtureKnife(p,choice);
            }catch(Exception ex){FailTestNative(ex);}
        });
        RegisterEventHandler<EventRoundEnd>((e,i)=>{
            if(testMode.Enabled)testBannerUntil=DateTimeOffset.UtcNow.AddSeconds(8);
            return HookResult.Continue;
        });
    }
    private static CCSWeaponBase? FindFixtureKnife(CCSPlayerController p)=>
        p.PlayerPawn.Value?.WeaponServices?.MyWeapons.Select(h=>h.Value).OfType<CCSWeaponBase>()
            .FirstOrDefault(w=>w.IsValid&&(w.DesignerName=="weapon_knife"||w.DesignerName=="weapon_knife_t"));
    private void GiveFixtureKnife(CCSPlayerController p,string choice="default"){
        var name=choice=="t"||choice=="default"&&p.TeamNum==(int)CsTeam.Terrorist?"weapon_knife_t":"weapon_knife";
        var item=p.GiveNamedItem<CCSWeaponBase>(name);
        if(item is not {IsValid:true})throw new InvalidOperationException("Stock fixture knife grant failed.");
        testWeapons.Add(item.EntityHandle.Raw); // Fixture-only, never redeemable inventory.
    }
    private void TestPresentationSecond(){
        if(!testMode.Enabled||!TestScope())return;
        var now=DateTimeOffset.UtcNow;
        var deadline=testFlow.Teams.StartsAt??testFlow.Teams.EndsAt;
        var remaining=deadline is {} at?Math.Max(0,(int)Math.Ceiling((at-now).TotalSeconds)):1800;
        if(testMode.NativeActive)UpdateTestNativeClock(remaining);
        foreach(var p in Utilities.GetPlayers().Where(x=>TestParticipant(x)||TestBot(x))){
            if(testMode.NativeActive&&p.PawnIsAlive&&FindFixtureKnife(p)==null)GiveFixtureKnife(p);
            if(!TestParticipant(p))continue;
            // Wallet is authoritative. Capped native cash is presentation ONLY.
            var balance=testFlow.Wallet.GetValueOrDefault(Id(p));
            if(p.InGameMoneyServices is {} money){
                var raw=p.EntityHandle.Raw;
                // Never project onto a replacement service under the old baseline.
                if(testCash.TryGetValue(raw,out var saved)&&saved.Services!=money.Handle)
                    throw new InvalidOperationException("Fixture cash service identity changed; projection denied.");
                testCash.TryAdd(raw,(money.Handle,money.Account));
                money.Account=TonightRules.Hud(balance);
                Utilities.SetStateChanged(p,"CCSPlayerController","m_pInGameMoneyServices");
            }
            // Native center alert is not a guaranteed exact bottom-center coordinate.
            // Single composed message avoids internal phase/balance writers competing.
            // Yield to round banners; no centerHTML spam fighting their display.
            if(now>=testBannerUntil)p.PrintToCenterAlert(
                $"TEST $"+balance.ToString("N0",System.Globalization.CultureInfo.InvariantCulture)+
                " | "+FixturePrompt(testFlow.Teams.Phase)+(deadline!=null?$" | {remaining/60}:{remaining%60:00}":""));
        }
    }
    private void UpdateTestNativeClock(int seconds){
        var proxy=Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault(x=>x.IsValid);
        var rules=proxy?.GameRules;
        if(proxy==null||rules==null||rules.Handle==IntPtr.Zero)throw new InvalidOperationException("Fixture clock GameRules unavailable.");
        if(!testCvars.ContainsKey("fixture-native-clock")){
            var raw=proxy.EntityHandle.Raw;var handle=rules.Handle;var time=rules.RoundTime;var start=rules.RoundStartTime;
            testCvars.Add("fixture-native-clock",()=>{
                var current=new CHandle<CCSGameRulesProxy>(raw).Value;
                if(current is not {IsValid:true}||current.GameRules is not {} r||r.Handle!=handle)return;
                r.RoundTime=time;r.RoundStartTime=start;
                Utilities.SetStateChanged(current,"CCSGameRulesProxy","m_pGameRules");
            });
        }
        // Never use warmup or round termination as a clock. Plugin phase owns expiry.
        rules.RoundTime=Math.Clamp(seconds,0,1800);rules.RoundStartTime=Server.CurrentTime;
        Utilities.SetStateChanged(proxy,"CCSGameRulesProxy","m_pGameRules");
    }
}
