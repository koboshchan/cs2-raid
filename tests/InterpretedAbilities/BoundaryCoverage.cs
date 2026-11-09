using DeltaOps;
static class BoundaryCoverage
{
 const string Root="https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/";
 static string[] Urls(params string[] paths)=>paths.Select(p=>Root+p).ToArray();
 public static string[] Sources(int id)=>id switch {
  10010301=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L265-L267","managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L317-L319","managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs#L109-L114","managed/CounterStrikeSharp.API/Utilities.cs#L231-L254"),
  20010101=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs#L65-L71","managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs#L109-L114","managed/CounterStrikeSharp.API/Utilities.cs#L231-L254"),
  1009001=>Urls("managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L15-L59","managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs"),
  2002017 or 1002016=>Urls("managed/CounterStrikeSharp.API/Utilities.cs#L155-L170","managed/CounterStrikeSharp.API/Core/Model/CCSPlayerController.cs#L49-L54","managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L77-L80","managed/CounterStrikeSharp.API/Modules/Utils/CHandle.cs","managed/CounterStrikeSharp.API/Core/Capabilities/PluginCapability.cs"),
  4008002 or 30010101 or 4009002 or 3008001=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L317-L319","managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs#L109-L114","managed/CounterStrikeSharp.API/Utilities.cs#L231-L254","managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L684-L730"),
  30010201 or 1009004=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CTakeDamageInfo.g.cs#L49-L59","managed/CounterStrikeSharp.API/Generated/Schema/Enums/DamageTypes_t.g.cs#L17-L18","managed/CounterStrikeSharp.API/Core/Listeners.g.cs#L40-L78","src/core/managers/entity_manager.cpp#L248-L325","src/core/detours.h#L14-L37"),
  40010201=>Urls("managed/CounterStrikeSharp.API/Generated/GameEvents/EventPlayerHurt.g.cs","managed/CounterStrikeSharp.API/Core/Model/CCSPlayerController.cs#L49-L54"),
  2008002 or 2008005 or 3008002 or 3008008 or 4009003=>Urls("managed/CounterStrikeSharp.API/Utilities.cs#L59-L62","managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L63-L75","managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L35-L40","managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L89-L118"),
  _=>[]
 };
 public static object[] Tuning(int id)=>id==30010201
 ? [new{Field="SkillDataConfig SkillID30010201.DamgeCoifficient[0].fModifyRateExplodeDamage",Value=.7d,Unit="dimensionless",Authority="Extracted DF literal; assault1.0 and LimitPlayMode[] retained; native mapping activation held"}]
 : [new{Status="UNRESOLVED",Authority="No permitted behavior default; old CS2 tuning withdrawn"}];
}
