using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
namespace Cs2Raid;

public sealed partial class RaidPlugin
{
    private sealed record PartnerVisual(string Owner,uint Pawn,uint Glow,uint Tag);
    private readonly Dictionary<string,PartnerVisual> partnerVisuals=[];
    private void RegisterPartnerVisuals()
    {
        RegisterListener<Listeners.CheckTransmit>(infos=>{
            foreach(var (info,viewer) in infos)
            foreach(var visual in partnerVisuals.Values) {
                if(viewer!=null && game.CanSeePartner(Id(viewer),visual.Owner))continue;
                var glow=new CHandle<CBaseEntity>(visual.Glow).Value;
                var tag=new CHandle<CBaseEntity>(visual.Tag).Value;
                if(glow is {IsValid:true}) info.TransmitEntities.Remove(glow);
                if(tag is {IsValid:true}) info.TransmitEntities.Remove(tag);
            }
        });
    }
    private void UpdatePartnerVisuals()
    {
        foreach(var id in partnerVisuals.Keys.ToArray()) {
            var p=Find(id);
            if(p==null || !game.Living(id) || !game.Players.Values.Any(m=>m.Id!=id&&game.CanSeePartner(m.Id,id)) || p.PlayerPawn.Raw!=partnerVisuals[id].Pawn) RemoveVisual(id);
        }
        foreach(var m in game.Players.Values) {
            if(!m.Alive || !game.Players.Values.Any(x=>x.Id!=m.Id&&game.CanSeePartner(x.Id,m.Id)))continue;
            var p=Find(m.Id);var pawn=p?.PlayerPawn.Value;
            if(pawn is not {IsValid:true} || pawn.AbsOrigin is not {} pos)continue;
            if(!partnerVisuals.TryGetValue(m.Id,out var visual)) {
                var model=pawn.CBodyComponent?.SceneNode?.GetSkeletonInstance().ModelState.ModelName;
                if(string.IsNullOrWhiteSpace(model))continue;
                var glow=Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
                var tag=Utilities.CreateEntityByName<CPointWorldText>("point_worldtext");
                if(glow==null||tag==null) {glow?.Remove();tag?.Remove();continue;}
                // Register ownership BEFORE spawn, default-deny every non-partner in CheckTransmit.
                visual=new(m.Id,pawn.EntityHandle.Raw,glow.EntityHandle.Raw,tag.EntityHandle.Raw);partnerVisuals.Add(m.Id,visual);
                glow.SetModel(model);glow.Collision.SolidType=SolidType_t.SOLID_NONE;glow.TakesDamage=false;
                glow.Render=Color.FromArgb(0,255,255,255);glow.RenderMode=RenderMode_t.kRenderTransAlpha;
                glow.Glow.GlowColorOverride=Color.Lime;glow.Glow.GlowType=3;glow.Glow.GlowRange=10000;glow.Glow.Glowing=true;
                glow.DispatchSpawn();glow.AcceptInput("SetParent",pawn,pawn,"!activator");glow.Effects|=1; // EF_BONEMERGE
                tag.Enabled=true;tag.Fullbright=true;tag.FontSize=28;tag.WorldUnitsPerPx=0.15f;tag.Color=Color.Lime;tag.FontName="Arial";tag.DispatchSpawn();
            }
            var text=new CHandle<CPointWorldText>(visual.Tag).Value;
            if(text is not {IsValid:true})continue;
            var message=$"{SafeName(p!.PlayerName)}\n{pawn.Health} HP";
            if(text.MessageText!=message){text.MessageText=message;Utilities.SetStateChanged(text,"CPointWorldText","m_messageText");}
            var mate=game.Players.Values.First(x=>x.Id!=m.Id&&game.CanSeePartner(x.Id,m.Id));
            var eye=Find(mate.Id)?.PlayerPawn.Value?.AbsOrigin;
            var yaw=eye==null?0:(float)(Math.Atan2(eye.Y-pos.Y,eye.X-pos.X)*180/Math.PI)+90;
            text.Teleport(new Vector(pos.X,pos.Y,pos.Z+84),new QAngle(0,yaw,90),new Vector(0,0,0));
        }
    }
    private void RemoveVisual(string id)
    {
        if(!partnerVisuals.Remove(id,out var v))return;
        foreach(var raw in new[]{v.Glow,v.Tag}) {var e=new CHandle<CBaseEntity>(raw).Value;if(e is {IsValid:true})e.Remove();}
    }
    private void ClearPartnerVisuals(){foreach(var id in partnerVisuals.Keys.ToArray())RemoveVisual(id);}
}
