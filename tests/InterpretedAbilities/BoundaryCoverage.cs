using DeltaOps;
static class BoundaryCoverage
{
 const string Root="https://github.com/roflmuffin/CounterStrikeSharp/blob/f58e383b0553be16c0f8c6dd68a8194de17691e9/";
 static string[] Urls(params string[] paths)=>paths.Select(p=>Root+p).ToArray();
 public static string[] Sources(int id)=>id switch {
  20010101=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs#L65-L71","managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs#L109-L114","managed/CounterStrikeSharp.API/Utilities.cs#L231-L254"),
  1009001=>Urls("managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L15-L59","managed/CounterStrikeSharp.API/Generated/Schema/Classes/CBaseEntity.g.cs"),
  2002017 or 1002016=>Urls("managed/CounterStrikeSharp.API/Utilities.cs#L155-L170","managed/CounterStrikeSharp.API/Core/Model/CCSPlayerController.cs#L49-L54","managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L77-L80","managed/CounterStrikeSharp.API/Modules/Utils/CHandle.cs","managed/CounterStrikeSharp.API/Core/Capabilities/PluginCapability.cs"),
  4008002 or 30010101 or 4009002 or 3008001=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CCSPlayerPawn.g.cs#L317-L319","managed/CounterStrikeSharp.API/Modules/Memory/Schema.cs#L109-L114","managed/CounterStrikeSharp.API/Utilities.cs#L231-L254","managed/CounterStrikeSharp.API/Core/BasePlugin.cs#L684-L730"),
  30010201 or 1009004=>Urls("managed/CounterStrikeSharp.API/Generated/Schema/Classes/CTakeDamageInfo.g.cs#L49-L59","managed/CounterStrikeSharp.API/Generated/Schema/Enums/DamageTypes_t.g.cs#L17-L18","managed/CounterStrikeSharp.API/Core/Listeners.g.cs#L40-L78","src/core/managers/entity_manager.cpp#L248-L325","src/core/detours.h#L14-L37"),
  40010201=>Urls("managed/CounterStrikeSharp.API/Generated/GameEvents/EventPlayerHurt.g.cs","managed/CounterStrikeSharp.API/Core/Model/CCSPlayerController.cs#L49-L54"),
  2008002 or 2008005 or 3008002 or 3008008 or 4009003=>Urls("managed/CounterStrikeSharp.API/Utilities.cs#L59-L62","managed/CounterStrikeSharp.API/Core/Model/CBaseEntity.cs#L63-L75","managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L35-L40","managed/CounterStrikeSharp.API/Core/Model/CEntityInstance.cs#L89-L118"),
  _=>[]
 };
 public static object[] Tuning(int id)=>id switch {
  20010101=>[new{Field="HealHp",Default=25d,Unit="HP",Authority="CS2 assumption, not extracted DF magnitude"}],
  2002017 or 1002016=>[new{Field="ReconRadiusSourceUnits",Default=1200d,Unit="Source units",Authority="CS2 assumption, no DF distance conversion claimed"}],
  1009001=>[new{Field="RushSpeedSourceUnits",Default=400d,Unit="Source units per second",Authority="CS2 horizontal impulse assumption; yaw degrees; existing Z velocity retained"}],
  4008002 or 30010101 or 4009002=>[
   new{Field="PulseRadiusSourceUnits",Default=240d,Unit="Source units",Authority="CS2 assumption"},
   new{Field="PulseSeconds",Default=2d,Unit="seconds",Authority="CS2 assumption; runtime converts to monotonic milliseconds"},
   new{Field="PulseSlowModifier",Default=.65d,Unit="dimensionless",Authority="CS2 speed scalar, not DF fire-rate suppression"}],
  3008001=>[
   new{Field="TrapRadiusSourceUnits",Default=160d,Unit="Source units",Authority="CS2 assumption"},
   new{Field="TrapLifetimeSeconds",Default=30d,Unit="seconds",Authority="CS2 assumption"},
   new{Field="TrapSlowSeconds",Default=2d,Unit="seconds",Authority="CS2 assumption"},
   new{Field="TrapVelocityModifier",Default=.5d,Unit="dimensionless",Authority="CS2 assumption"}],
  30010201=>[new{Field="DamgeCoifficient[0].fModifyRateExplodeDamage",Default=.7d,Unit="dimensionless",Authority="DFPROVEN literal; raw assault1.0 and LimitPlayMode[] retained; DMG_BLAST mapping CS2 assumption"}],
  1009004=>[new{Field="VyronFallDamageMultiplier",Default=.5d,Unit="dimensionless",Authority="CS2 assumption; DF buff magnitudes not decoded"}],
  _=>[]
 };
}
