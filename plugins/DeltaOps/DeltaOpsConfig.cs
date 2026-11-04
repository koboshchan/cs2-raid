using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace DeltaOps;

public sealed class DeltaOpsConfig : BasePluginConfig
{
    [JsonPropertyName("SelectionSeconds")]
    public int SelectionSeconds { get; set; } = 120;

    [JsonPropertyName("AutoBindSkillKeys")]
    public bool AutoBindSkillKeys { get; set; } = true;

    [JsonPropertyName("VCooldownSeconds")]
    public int VCooldownSeconds { get; set; } = 18;

    [JsonPropertyName("XCooldownSeconds")]
    public int XCooldownSeconds { get; set; } = 24;

    [JsonPropertyName("CookMaxFuseSeconds")]
    public float CookMaxFuseSeconds { get; set; } = 3.0f;

    [JsonPropertyName("HackclawMarkSeconds")]
    public float HackclawMarkSeconds { get; set; } = 5.0f;

    [JsonPropertyName("VyronGrenadeRadius")]
    public float VyronGrenadeRadius { get; set; } = 380f;

    [JsonPropertyName("VyronControlSeconds")]
    public float VyronControlSeconds { get; set; } = 4.0f;

    [JsonPropertyName("ShepherdGrenadeRadius")]
    public float ShepherdGrenadeRadius { get; set; } = 400f;

    [JsonPropertyName("ShepherdGrenadeDamage")]
    public int ShepherdGrenadeDamage { get; set; } = 150;

    [JsonPropertyName("ShepherdMineTriggerRadius")]
    public float ShepherdMineTriggerRadius { get; set; } = 220f;

    [JsonPropertyName("ShepherdMineDamageRadius")]
    public float ShepherdMineDamageRadius { get; set; } = 420f;

    [JsonPropertyName("ShepherdMineMaxDamage")]
    public int ShepherdMineMaxDamage { get; set; } = 120;

    [JsonPropertyName("ShepherdMineLifetimeSeconds")]
    public float ShepherdMineLifetimeSeconds { get; set; } = 60f;

    [JsonPropertyName("RedWolfKillHeal")]
    public int RedWolfKillHeal { get; set; } = 50;

    [JsonPropertyName("RedWolfSpeedMultiplier")]
    public float RedWolfSpeedMultiplier { get; set; } = 1.35f;

    [JsonPropertyName("RedWolfDurationSeconds")]
    public float RedWolfDurationSeconds { get; set; } = 4.0f;

    [JsonPropertyName("StingerHealAmount")]
    public int StingerHealAmount { get; set; } = 60;

    [JsonPropertyName("StingerHealRadius")]
    public float StingerHealRadius { get; set; } = 700f;

    [JsonPropertyName("StingerBonusHealth")]
    public int StingerBonusHealth { get; set; } = 20;

    [JsonPropertyName("StingerLongSmokeClouds")]
    public int StingerLongSmokeClouds { get; set; } = 5;

    [JsonPropertyName("StingerLongSmokeSpacing")]
    public float StingerLongSmokeSpacing { get; set; } = 160f;

    [JsonPropertyName("StingerLongSmokeStartDistance")]
    public float StingerLongSmokeStartDistance { get; set; } = 120f;

    [JsonPropertyName("StingerLongSmokeRange")]
    public float StingerLongSmokeRange { get; set; } = 860f;

    [JsonPropertyName("ScanRange")]
    public float ScanRange { get; set; } = 1800f;

    [JsonPropertyName("HackclawMaxTargets")]
    public int HackclawMaxTargets { get; set; } = 6;

    [JsonPropertyName("VyronRadius")]
    public float VyronRadius { get; set; } = 420f;

    [JsonPropertyName("VyronSlowFactor")]
    public float VyronSlowFactor { get; set; } = 0.45f;

    [JsonPropertyName("VyronSlowSeconds")]
    public float VyronSlowSeconds { get; set; } = 2.0f;

    [JsonPropertyName("ShepherdRadius")]
    public float ShepherdRadius { get; set; } = 600f;

    [JsonPropertyName("ShepherdSlowFactor")]
    public float ShepherdSlowFactor { get; set; } = 0.65f;

    [JsonPropertyName("ShepherdSlowSeconds")]
    public float ShepherdSlowSeconds { get; set; } = 3.0f;

    [JsonPropertyName("ShepherdBonusArmor")]
    public int ShepherdBonusArmor { get; set; } = 15;

    [JsonPropertyName("LunaOutlineSeconds")]
    public float LunaOutlineSeconds { get; set; } = 2.5f;

    [JsonPropertyName("LunaPunishDamage")]
    public int LunaPunishDamage { get; set; } = 5;

    [JsonPropertyName("LunaPunishTicks")]
    public int LunaPunishTicks { get; set; } = 5;

    [JsonPropertyName("LunaPunishIntervalSeconds")]
    public float LunaPunishIntervalSeconds { get; set; } = 1.0f;

    [JsonPropertyName("LunaPunishOutlineSeconds")]
    public float LunaPunishOutlineSeconds { get; set; } = 5.0f;
}
