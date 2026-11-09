using System.Collections.Frozen;
namespace DeltaOps;
public sealed record InterpretedSkill(int Operator, int Id, string Name, int CdId, int Initial, int Capacity, double RechargeSeconds, bool Passive);
public static class InterpretedR2 {
public static readonly FrozenDictionary<(int Operator,int Skill),InterpretedSkill> Skills = new InterpretedSkill[] {
new(1,2008002,"烟幕",2002,1,1,60.0,false),
new(1,2008005,"蜂巢科技烟雾弹",2001,1,1,40.0,false),
new(1,4008007,"拔箭",0,1,1,1.0,false),
new(1,20010101,"激素枪",2,0,3,30.0,false),
new(1,20010201,"高效救援",0,1,1,1.0,true),
new(2,2002017,"侦察箭矢",14,0,1,75.0,false),
new(2,3008002,"破片手雷",92,2,2,55.0,false),
new(2,4008002,"电击箭矢",13,1,1,50.0,false),
new(2,4008007,"拔箭",0,1,1,1.0,false),
new(2,40010201,"敌情追踪",0,1,1,1.0,true),
new(3,1009001,"动力推进",18,0,1,20.0,false),
new(3,1009003,"虎蹲炮",17,0,1,70.0,false),
new(3,1009004,"动能辅助系统",0,1,1,1.0,true),
new(3,1009005,"磁吸炸弹",16,0,1,55.0,false),
new(3,4008007,"拔箭",0,1,1,1.0,false),
new(3,10010301,"敏捷射手",0,1,1,1.0,true),
new(4,1002016,"信号破译器",22,0,1,90.0,false),
new(4,4008007,"拔箭",0,1,1,1.0,false),
new(4,4009002,"数据飞刀",20,0,1,40.0,false),
new(4,4009003,"闪光巡飞器",21,2,2,45.0,false),
new(4,40010202,"202被动-静匿消声",0,1,1,1.0,true),
new(5,3008001,"声波陷阱",11,2,2,40.0,false),
new(5,3008008,"破片手雷",15,2,2,45.0,false),
new(5,4008007,"拔箭",0,1,1,1.0,false),
new(5,30010101,"声波震慑",12,0,1,90.0,false),
new(5,30010201,"共振防御",0,1,1,1.0,true),
}.ToFrozenDictionary(s=>(s.Operator,s.Id));
}
