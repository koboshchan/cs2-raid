using System.Drawing;
using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.UserMessages;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using CssTimer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace DeltaOps;

public sealed partial class DeltaOpsPlugin : BasePlugin, IPluginConfig<DeltaOpsConfig>
{
    private enum OperatorId
    {
        RedWolf,
        Stinger,
        Luna,
        Vyron,
        Hackclaw,
        Shepherd
    }

    private enum CookedGrenadeKind
    {
        Flashbang,
        HeGrenade
    }

    private sealed class LandmineState
    {
        public required string OwnerKey { get; init; }
        public required string OwnerName { get; init; }
        public required int OwnerTeam { get; init; }
        public required Vector Position { get; init; }
        public required int EntityIndex { get; init; }
        public CssTimer? Timer { get; set; }
    }

    private sealed record OperatorDefinition(
        OperatorId Id,
        string Name,
        string EnglishName,
        string Role,
        string VName,
        string VDescription,
        string XName,
        string XDescription,
        string PassiveName,
        string PassiveDescription,
        float BaseSpeed = 1.0f,
        int BonusHealth = 0,
        int BonusArmor = 0);

    private sealed class PlayerState
    {
        public OperatorId? Operator { get; set; }
        public long VCooldownUntilMs { get; set; }
        public long XCooldownUntilMs { get; set; }
        public CssTimer? VReadyTimer { get; set; }
        public CssTimer? XReadyTimer { get; set; }
        public string LastKnownName { get; set; } = string.Empty;
        public CssTimer? SelectionTimer { get; set; }
        public int SelectionSecondsRemaining { get; set; }
        public bool BindingHelpShown { get; set; }
        public int LastLunaFlashRound { get; set; } = -1;
        public string? LastLunaDamagedTargetKey { get; set; }
        public string? LastLunaAttackerKey { get; set; }
        public string? LunaDotTargetKey { get; set; }
        public CssTimer? LunaDotTimer { get; set; }
        public HashSet<string> LunaPassiveSuppressedVictims { get; } =
            new(StringComparer.Ordinal);
        public CookedGrenadeKind? ArmedGrenade { get; set; }
        public long GrenadePinPulledAtMs { get; set; }
        public long GrenadeSequence { get; set; }
        public long LastGrenadeHudUpdateMs { get; set; }
        public CssTimer? GrenadeCancelTimer { get; set; }
        public uint? GrantedLunaFlashHandle { get; set; }
        public int PassiveArmorAwarded { get; set; }
    }

    private readonly List<OperatorDefinition> _operators =
    [
        new(
            OperatorId.RedWolf,
            "红狼",
            "D-Wolf",
            "突击",
            "爆发外骨骼",
            "4 秒内移动速度提升 35%。",
            "动力跃迁",
            "沿 WASD 输入方向跃迁并恢复 15 生命；无输入时向前。",
            "猎杀本能",
            "击杀敌人后恢复 50 生命。"),
        new(
            OperatorId.Stinger,
            "蜂医",
            "Stinger",
            "支援",
            "激素治疗",
            "治疗 18 米内所有存活队友 60 生命，也治疗自己。",
            "蜂巢烟雾",
            "沿瞄准方向释放一条由 5 段真实烟雾组成的长烟。",
            "战地体质",
            "最大生命提高 20。",
            BonusHealth: 20),
        new(
            OperatorId.Luna,
            "露娜",
            "Luna",
            "侦察",
            "侦测箭",
            "扫描 46 米内敌人，并向全队报告姓名与距离。",
            "电流箭",
            "对最近伤害你的敌人持续造成 5 点伤害、共 5 次，并显形 5 秒。",
            "侦测标记",
            "对敌人造成伤害或闪光命中时，使其向全队显形 2.5 秒。",
            BaseSpeed: 1.0f),
        new(
            OperatorId.Vyron,
            "威龙",
            "Vyron",
            "突击",
            "喷气推进",
            "沿 WASD 输入方向高速喷气推进；无输入时向前。",
            "震荡手雷",
            "获得一枚标准高爆手雷；爆炸范围内敌人缴械并定身 4 秒。",
            "突击姿态",
            "常驻移动速度提升 6%。",
            BaseSpeed: 1.06f),
        new(
            OperatorId.Hackclaw,
            "骇爪",
            "Hackclaw",
            "侦察",
            "信号解码器",
            "扫描范围内敌人、向全队标记并提示被扫描者。",
            "闪光无人机",
            "获得一枚额外闪光弹。按住原生左键拉栓时显示 3 秒引信，取消拉栓会停止计时。",
            "潜行机动",
            "常驻移动速度提升 4%。",
            BaseSpeed: 1.04f),
        new(
            OperatorId.Shepherd,
            "牧羊人",
            "Shepherd",
            "工程",
            "感应地雷",
            "在脚下放置隐雷，敌人进入触发半径后自动爆炸。",
            "GE2 手雷",
            "获得一枚强化高爆手雷。原生拉栓会启动 3 秒引信，取消拉栓会停止计时。",
            "工程防护",
            "每回合出生时至少获得 15 护甲。",
            BonusArmor: 15)
    ];

    private readonly Dictionary<string, PlayerState> _states = new(StringComparer.Ordinal);
    private readonly Dictionary<string, CssTimer> _temporarySpeedTimers = new(StringComparer.Ordinal);
    private readonly Dictionary<string, CssTimer> _outlineTimers = new(StringComparer.Ordinal);
    private readonly Dictionary<string, CssTimer> _controlTimers = new(StringComparer.Ordinal);
    private readonly Dictionary<string, MoveType_t> _controlPreviousMoveTypes = new(StringComparer.Ordinal);
    private readonly List<LandmineState> _landmines = [];
    private int _roundNumber;
    private long _nextGrenadePollMs;

    public DeltaOpsConfig Config { get; set; } = new();

    public override string ModuleName => "Delta Ops";
    public override string ModuleVersion => "1.3.2";
    public override string ModuleAuthor => "Local server";
    public override string ModuleDescription => "CS gameplay with selectable Delta Force inspired operator abilities.";

    public void OnConfigParsed(DeltaOpsConfig config)
    {
        config.SelectionSeconds = Math.Clamp(config.SelectionSeconds, 10, 120);
        config.VCooldownSeconds = Math.Clamp(config.VCooldownSeconds, 1, 120);
        config.XCooldownSeconds = Math.Clamp(config.XCooldownSeconds, 1, 120);
        config.CookMaxFuseSeconds = Math.Clamp(config.CookMaxFuseSeconds, 1f, 5f);
        config.HackclawMarkSeconds = Math.Clamp(config.HackclawMarkSeconds, 1f, 12f);
        config.VyronGrenadeRadius = Math.Clamp(config.VyronGrenadeRadius, 200f, 800f);
        config.VyronControlSeconds = Math.Clamp(config.VyronControlSeconds, 1f, 8f);
        config.ShepherdGrenadeRadius = Math.Clamp(config.ShepherdGrenadeRadius, 200f, 800f);
        config.ShepherdGrenadeDamage = Math.Clamp(config.ShepherdGrenadeDamage, 1, 300);
        config.ShepherdMineTriggerRadius = Math.Clamp(config.ShepherdMineTriggerRadius, 80f, 500f);
        config.ShepherdMineDamageRadius = Math.Clamp(config.ShepherdMineDamageRadius, 100f, 800f);
        config.ShepherdMineMaxDamage = Math.Clamp(config.ShepherdMineMaxDamage, 1, 300);
        config.ShepherdMineLifetimeSeconds = Math.Clamp(config.ShepherdMineLifetimeSeconds, 10f, 180f);
        config.RedWolfKillHeal = Math.Clamp(config.RedWolfKillHeal, 0, 100);
        config.RedWolfSpeedMultiplier = Math.Clamp(config.RedWolfSpeedMultiplier, 1.0f, 2.0f);
        config.RedWolfDurationSeconds = Math.Clamp(config.RedWolfDurationSeconds, 1.0f, 15.0f);
        config.StingerHealAmount = Math.Clamp(config.StingerHealAmount, 1, 200);
        config.StingerHealRadius = Math.Clamp(config.StingerHealRadius, 100f, 2000f);
        config.StingerBonusHealth = Math.Clamp(config.StingerBonusHealth, 0, 200);
        config.StingerLongSmokeClouds = Math.Clamp(config.StingerLongSmokeClouds, 1, 8);
        config.StingerLongSmokeSpacing = Math.Clamp(config.StingerLongSmokeSpacing, 80f, 320f);
        config.StingerLongSmokeStartDistance = Math.Clamp(config.StingerLongSmokeStartDistance, 0f, 400f);
        config.StingerLongSmokeRange = Math.Clamp(config.StingerLongSmokeRange, 300f, 1800f);
        config.ScanRange = Math.Clamp(config.ScanRange, 300f, 5000f);
        config.HackclawMaxTargets = Math.Clamp(config.HackclawMaxTargets, 1, 12);
        config.VyronRadius = Math.Clamp(config.VyronRadius, 100f, 1500f);
        config.VyronSlowFactor = Math.Clamp(config.VyronSlowFactor, 0.1f, 1.0f);
        config.VyronSlowSeconds = Math.Clamp(config.VyronSlowSeconds, 0.5f, 10f);
        config.ShepherdRadius = Math.Clamp(config.ShepherdRadius, 100f, 2000f);
        config.ShepherdSlowFactor = Math.Clamp(config.ShepherdSlowFactor, 0.1f, 1.0f);
        config.ShepherdSlowSeconds = Math.Clamp(config.ShepherdSlowSeconds, 0.5f, 10f);
        config.ShepherdBonusArmor = Math.Clamp(config.ShepherdBonusArmor, 0, 100);
        config.LunaOutlineSeconds = Math.Clamp(config.LunaOutlineSeconds, 0.5f, 10f);
        config.LunaPunishDamage = Math.Clamp(config.LunaPunishDamage, 1, 50);
        config.LunaPunishTicks = Math.Clamp(config.LunaPunishTicks, 1, 12);
        config.LunaPunishIntervalSeconds = Math.Clamp(config.LunaPunishIntervalSeconds, 0.25f, 5f);
        config.LunaPunishOutlineSeconds = Math.Clamp(config.LunaPunishOutlineSeconds, 1f, 12f);
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        ConfigureRaidBridge();
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        RegisterEventHandler<EventPlayerTeam>(OnPlayerTeam);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        RegisterEventHandler<EventHegrenadeDetonate>(OnHegrenadeDetonate);
        RegisterEventHandler<EventPlayerBlind>(OnPlayerBlind);
        RegisterEventHandler<EventGrenadeThrown>(OnGrenadeThrown);
        RegisterEventHandler<EventGameEnd>(OnMatchEnded);
        RegisterEventHandler<EventCsWinPanelMatch>(OnMatchEnded);
        RegisterEventHandler<EventCsMatchEndRestart>(OnMatchEnded);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
        RegisterListener<Listeners.OnClientDisconnect>(OnClientDisconnect);
        RegisterListener<Listeners.OnTick>(OnTick);

        Logger.LogInformation(
            "DeltaOps loaded. Operators={OperatorCount}, selectionTimeout={Timeout}s, " +
            "cooldowns=V:{VCooldown}s/X:{XCooldown}s, bindingHelp={BindingHelp}.",
            _operators.Count, Config.SelectionSeconds, Config.VCooldownSeconds,
            Config.XCooldownSeconds, Config.AutoBindSkillKeys);
    }

    public override void Unload(bool hotReload)
    {
        _bridgeLoaded = false;
        ResetAllPlayersAndState("插件卸载", broadcast: false);
    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !IsHumanController(player))
        {
            return HookResult.Continue;
        }

        var key = GetPlayerKey(player);
        Server.NextFrame(() =>
        {
            var current = FindPlayerByKey(key);
            if (current == null)
            {
                return;
            }

            ShowSkillBindingHelp(current);
            var state = GetOrCreateState(current, key);
            if (state.Operator == null)
            {
                ShowOperatorSelection(current, key);
            }
            else
            {
                ApplyPassive(current, state.Operator.Value);
            }
        });

        return HookResult.Continue;
    }

    private HookResult OnPlayerTeam(EventPlayerTeam @event, GameEventInfo info)
    {
        if (@event.Disconnect || @event.Team is not (2 or 3) ||
            @event.Userid == null || !IsHumanController(@event.Userid))
        {
            return HookResult.Continue;
        }

        var key = GetPlayerKey(@event.Userid);
        Server.NextFrame(() =>
        {
            var current = FindPlayerByKey(key);
            if (current == null)
            {
                return;
            }

            var state = GetOrCreateState(current, key);
            if (state.Operator == null)
            {
                ShowOperatorSelection(current, key);
            }
        });

        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        _roundNumber++;
        ClearTemporarySpeedEffects();
        ClearAllLunaDots();
        ClearAllArmedGrenades();
        ClearAllLandmines();
        ClearAllControlTimers();

        foreach (var player in Utilities.GetPlayers().ToList())
        {
            if (!IsHumanInMatch(player))
            {
                continue;
            }

            var state = GetOrCreateState(player, GetPlayerKey(player));
            ResetSkillCooldowns(state);
            state.LastLunaDamagedTargetKey = null;
            state.LastLunaAttackerKey = null;
            if (state.Operator.HasValue)
            {
                ApplyPassive(player, state.Operator.Value);
                player.PrintToCenter(
                    $"干员 {GetOperator(state.Operator.Value).Name}：V / X 冷却已重置");
            }
        }

        return HookResult.Continue;
    }

    private HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        ClearTemporarySpeedEffects();
        ClearAllOutlines();
        ClearAllLunaDots();
        ClearAllArmedGrenades();
        ClearAllLandmines();
        ClearAllControlTimers();
        return HookResult.Continue;
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var victim = @event.Userid;
        var attacker = @event.Attacker;
        if (IsCombatController(victim))
        {
            var victimKey = GetPlayerKey(victim!);
            ClearOutline(victimKey);
            CancelArmedGrenade(victimKey);
            ClearControl(victimKey);
            var victimState = GetExistingState(victim!);
            if (victimState?.Operator == OperatorId.Luna)
            {
                victimState.LastLunaDamagedTargetKey = null;
                victimState.LastLunaAttackerKey = null;
                StopLunaDot(victimKey);
            }
        }

        if (IsHumanController(attacker) && IsCombatController(victim) &&
            attacker!.Handle != victim!.Handle && IsEnemy(attacker, victim))
        {
            var attackerState = GetExistingState(attacker);
            if (attackerState?.Operator == OperatorId.RedWolf)
            {
                var healed = HealPlayer(attacker, Config.RedWolfKillHeal);
                if (healed > 0)
                {
                    attacker.PrintToChat($" [红狼] 猎杀本能触发，恢复 {healed} 生命。");
                }
            }
        }

        return HookResult.Continue;
    }

    private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var victim = @event.Userid;
        if (attacker == null || victim == null ||
            !IsHumanController(attacker) || !IsCombatController(victim) ||
            attacker.Handle == victim.Handle ||
            !IsEnemy(attacker, victim) ||
            @event.DmgHealth <= 0)
        {
            return HookResult.Continue;
        }

        var attackerState = GetExistingState(attacker);
        if (attackerState?.Operator == OperatorId.Luna)
        {
            var victimKey = GetPlayerKey(victim);
            attackerState.LastLunaDamagedTargetKey = victimKey;
            if (!attackerState.LunaPassiveSuppressedVictims.Contains(victimKey))
            {
                ShowOutline(victim, Config.LunaOutlineSeconds);
            }
        }

        var victimState = GetExistingState(victim);
        if (victimState?.Operator == OperatorId.Luna)
        {
            victimState.LastLunaAttackerKey = GetPlayerKey(attacker);
        }

        return HookResult.Continue;
    }

    private HookResult OnHegrenadeDetonate(EventHegrenadeDetonate @event, GameEventInfo info)
    {
        var owner = @event.Userid;
        if (owner == null || !IsHumanController(owner))
        {
            return HookResult.Continue;
        }

        var state = GetExistingState(owner);
        if (state?.Operator != OperatorId.Vyron)
        {
            return HookResult.Continue;
        }

        var explosion = new Vector(@event.X, @event.Y, @event.Z);
        foreach (var target in GetEnemies(owner).ToList())
        {
            if (DistanceToPoint(target, explosion) <= Config.VyronGrenadeRadius)
            {
                ApplyControlDebuff(target, Config.VyronControlSeconds);
                target.PrintToCenter("震荡手雷命中：缴械并定身");
                target.PrintToChat(
                    $" [威龙] 你被震荡手雷命中，缴械并定身 " +
                    $"{Config.VyronControlSeconds:0.#} 秒。");
            }
        }

        return HookResult.Continue;
    }

    private HookResult OnPlayerBlind(EventPlayerBlind @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var victim = @event.Userid;
        if (!IsHumanController(attacker) || !IsCombatController(victim) ||
            attacker!.Handle == victim!.Handle || !IsEnemy(attacker, victim) ||
            @event.BlindDuration < 0.1f)
        {
            return HookResult.Continue;
        }

        var attackerState = GetExistingState(attacker);
        if (attackerState?.Operator == OperatorId.Luna)
        {
            ShowOutline(victim);
            attacker.PrintToChat($" [露娜] 特殊闪光命中 {victim.PlayerName}，轮廓标记 {Config.LunaOutlineSeconds:0.#} 秒。");
        }

        return HookResult.Continue;
    }

    private void OnTick()
    {
        var now = Environment.TickCount64;
        if (now < _nextGrenadePollMs)
        {
            return;
        }

        _nextGrenadePollMs = now + 50L;
        foreach (var player in Utilities.GetPlayers().ToList())
        {
            if (!IsHumanInMatch(player) || !player.PawnIsAlive)
            {
                continue;
            }

            var state = GetExistingState(player);
            if (state?.Operator is not (OperatorId.Hackclaw or OperatorId.Shepherd))
            {
                continue;
            }

            var expectedKind = state.Operator == OperatorId.Hackclaw
                ? CookedGrenadeKind.Flashbang
                : CookedGrenadeKind.HeGrenade;
            var grenade = GetActiveGrenade(player, expectedKind);
            if (grenade == null || !IsValidGrenade(grenade))
            {
                ScheduleGrenadeCancel(player, state);
                continue;
            }

            if (!grenade.PinPulled)
            {
                ScheduleGrenadeCancel(player, state);
                continue;
            }

            var wasCancelling = state.GrenadeCancelTimer != null;
            state.GrenadeCancelTimer?.Kill();
            state.GrenadeCancelTimer = null;
            if (state.ArmedGrenade != expectedKind || wasCancelling)
            {
                state.ArmedGrenade = expectedKind;
                state.GrenadePinPulledAtMs = now;
                state.GrenadeSequence++;
                state.LastGrenadeHudUpdateMs = 0;
                RenderGrenadePinHud(player, Config.CookMaxFuseSeconds, expectedKind);
            }

            var remaining = GetGrenadePinRemainingSeconds(state);
            if (now - state.LastGrenadeHudUpdateMs >= 100L)
            {
                state.LastGrenadeHudUpdateMs = now;
                RenderGrenadePinHud(player, remaining, expectedKind);
            }

            if (remaining <= 0f)
            {
                DetonateHeldGrenade(player, state, grenade, expectedKind);
            }
        }
    }

    private HookResult OnGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !IsHumanController(player))
        {
            return HookResult.Continue;
        }

        var state = GetExistingState(player);
        if (state?.Operator == null ||
            !TryGetGrenadeKindFromEventName(@event.Weapon, out var kind))
        {
            return HookResult.Continue;
        }

        var expectedKind = state.Operator == OperatorId.Hackclaw
            ? CookedGrenadeKind.Flashbang
            : state.Operator == OperatorId.Shepherd
                ? CookedGrenadeKind.HeGrenade
                : (CookedGrenadeKind?)null;
        if (expectedKind != kind)
        {
            return HookResult.Continue;
        }

        float? remaining = null;
        if (state.ArmedGrenade == kind)
        {
            remaining = Math.Max(0.05f, GetGrenadePinRemainingSeconds(state));
            ResetArmedGrenade(state, player, clearHud: true);
        }

        if (!ApplyThrownGrenadeEffect(player, kind, remaining))
        {
            var key = GetPlayerKey(player);
            AddTimer(0.02f, () =>
            {
                var current = FindPlayerByKey(key);
                if (current != null &&
                    !ApplyThrownGrenadeEffect(current, kind, remaining))
                {
                    Logger.LogWarning(
                        "Player '{Player}' threw {Grenade}, but no matching projectile was found.",
                        current.PlayerName,
                        kind);
                }
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }

        if (remaining.HasValue)
        {
            player.PrintToCenter($"拉栓投掷：剩余引信 {remaining.Value:0.0} 秒");
            Logger.LogInformation(
                "Player '{Player}' threw native {Grenade} with {Fuse:0.00}s remaining.",
                player.PlayerName,
                kind,
                remaining.Value);
        }

        return HookResult.Continue;
    }

    [ConsoleCommand("css_ops_test_outline", "服务器控制台：测试露娜轮廓效果")]
    public void OnTestOutlineCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行轮廓测试。");
            return;
        }

        var luna = Utilities.GetPlayers().FirstOrDefault(candidate =>
            IsHumanInMatch(candidate) &&
            GetExistingState(candidate)?.Operator == OperatorId.Luna);
        if (luna == null)
        {
            command.ReplyToCommand("[三角洲模式] 当前没有已选择露娜的真人玩家。");
            return;
        }

        var target = GetEnemies(luna).FirstOrDefault();
        if (target == null)
        {
            command.ReplyToCommand("[三角洲模式] 当前没有可作为目标的敌人。");
            return;
        }

        ShowOutline(target);
        command.ReplyToCommand($"[三角洲模式] 已为 {target.PlayerName} 显示轮廓。");
    }

    private HookResult OnMatchEnded(EventGameEnd @event, GameEventInfo info)
    {
        ResetAllPlayersAndState("比赛结束", broadcast: true);
        return HookResult.Continue;
    }

    private HookResult OnMatchEnded(EventCsWinPanelMatch @event, GameEventInfo info)
    {
        ResetAllPlayersAndState("比赛结束", broadcast: true);
        return HookResult.Continue;
    }

    private HookResult OnMatchEnded(EventCsMatchEndRestart @event, GameEventInfo info)
    {
        ResetAllPlayersAndState("比赛结束", broadcast: true);
        return HookResult.Continue;
    }

    private void OnMapStart(string mapName)
    {
        ResetAllPlayersAndState("地图开始", broadcast: false);
    }

    private void OnMapEnd()
    {
        ResetAllPlayersAndState("地图结束", broadcast: false);
    }

    private void OnClientPutInServer(int slot)
    {
        Server.NextFrame(() =>
        {
            var player = Utilities.GetPlayerFromSlot(slot);
            if (player != null && IsHumanController(player))
            {
                ShowSkillBindingHelp(player);
            }
        });
    }

    private void ShowSkillBindingHelp(CCSPlayerController player, bool force = false)
    {
        if (!IsHumanController(player))
        {
            return;
        }

        var state = GetOrCreateState(player, GetPlayerKey(player));
        if (!force && (!Config.AutoBindSkillKeys || state.BindingHelpShown))
        {
            return;
        }

        state.BindingHelpShown = true;
        // Bind is local input configuration, not a server command. Never send it
        // through ExecuteClientCommand[FromServer], and never claim it succeeded.
        player.PrintToChat(" [DeltaOps] 技能输入 !v / !x；按键需自己在游戏控制台绑定：");
        player.PrintToChat(" bind \"v\" \"css_v\"; bind \"x\" \"css_x\"");
    }

    private void OnClientDisconnect(int slot)
    {
        var player = Utilities.GetPlayerFromSlot(slot);
        if (player == null || !IsHumanController(player))
        {
            return;
        }

        var key = GetPlayerKey(player);
        ClearTemporarySpeed(key, applyBaseSpeed: false);
        StopLunaDot(key);
        CancelArmedGrenade(key);
        ClearControl(key);
        ClearLandminesForOwner(key);
    }

    [ConsoleCommand("css_characterselect", "选择干员：!characterselect <1-6>；无参数查看列表")]
    [ConsoleCommand("css_op", "选择或重新查看干员：!op 1")]
    public void OnOperatorCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (_raidMode) return; // Raid owns selection and confirms only after capability success.
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        if (command.ArgCount < 2)
        {
            ShowOperatorSelection(player, GetPlayerKey(player));
            return;
        }

        if (!int.TryParse(command.GetArg(1), out var selectedIndex) ||
            selectedIndex < 1 || selectedIndex > _operators.Count)
        {
            player.PrintToChat($" [三角洲模式] 干员编号应为 1-{_operators.Count}。");
            return;
        }

        var key = GetPlayerKey(player);
        var state = GetOrCreateState(player, key);
        if (state.Operator.HasValue && !IsWarmupPeriod())
        {
            player.PrintToChat(" [三角洲模式] 正式比赛开始后干员已锁定。输入 !ops 查看当前能力。");
            return;
        }

        SelectOperator(player, key, _operators[selectedIndex - 1], automatic: false);
    }

    private void ResetOperatorForSwitch(
        CCSPlayerController player,
        string key,
        PlayerState state)
    {
        state.SelectionTimer?.Kill();
        state.SelectionTimer = null;
        CancelArmedGrenade(key);
        StopLunaDot(key);
        ClearControl(key);
        ClearOutline(key);
        ClearLandminesForOwner(key);
        ClearTemporarySpeed(key, applyBaseSpeed: false);
        RemoveGrantedLunaFlash(player, state);
        RemovePassiveArmor(player, state);
        state.LastLunaDamagedTargetKey = null;
        state.LastLunaAttackerKey = null;
        state.Operator = null;
        ResetSkillCooldowns(state);

        var pawn = player.PlayerPawn.Value;
        if (pawn != null && pawn.IsValid)
        {
            pawn.MaxHealth = _raidMode ? 200 : 100;
            pawn.Health = Math.Min(pawn.Health, pawn.MaxHealth);
            pawn.VelocityModifier = 1.0f;
            Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
            Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
            Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
        }

        Logger.LogInformation(
            "Player '{Player}' requested a live operator switch.",
            player.PlayerName);
    }

    [ConsoleCommand("css_ops", "查看当前干员和能力")]
    public void OnOperatorStatusCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        if (_raidMode && !RaidActive(player.SteamID)) return;
        var state = GetExistingState(player);
        if (state?.Operator == null)
        {
            ShowOperatorSelection(player, GetPlayerKey(player));
            return;
        }

        var definition = GetOperator(state.Operator.Value);
        player.PrintToChat($" [三角洲模式] {definition.Name} / {definition.EnglishName} | {definition.Role}");
        player.PrintToChat($" [V] {definition.VName}: {definition.VDescription}");
        player.PrintToChat($" [X] {definition.XName}: {definition.XDescription}");
        player.PrintToChat($" [被动] {definition.PassiveName}: {definition.PassiveDescription}");
        PrintCooldownStatus(player, state);
    }

    [ConsoleCommand("css_cd", "查看 V/X 技能冷却")]
    public void OnCooldownCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        if (_raidMode && !RaidActive(player.SteamID)) return;
        var state = GetExistingState(player);
        if (state?.Operator == null)
        {
            player.PrintToChat(" [三角洲模式] 请先选择干员。");
            return;
        }

        PrintCooldownStatus(player, state);
    }

    [ConsoleCommand("css_ops_use", "服务器控制台：css_ops_use <slot> <v|x>")]
    public void OnServerUseSkillCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行技能测试。");
            return;
        }

        if (command.ArgCount < 3 ||
            !int.TryParse(command.GetArg(1), out var slot) ||
            slot < 0)
        {
            command.ReplyToCommand("用法：css_ops_use <slot> <v|x>");
            return;
        }

        var slotName = command.GetArg(2).Trim().ToLowerInvariant();
        if (slotName is not ("v" or "x"))
        {
            command.ReplyToCommand("技能栏位只能是 v 或 x。");
            return;
        }

        var target = Utilities.GetPlayerFromSlot(slot);
        if (target == null || !IsHumanController(target))
        {
            command.ReplyToCommand($"槽位 {slot} 没有真人玩家。");
            return;
        }

        UseSkillSlot(target, command, useV: slotName == "v");
    }

    [ConsoleCommand("css_ops_assign", "服务器控制台：css_ops_assign <slot> <1-6>")]
    public void OnServerAssignOperatorCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行干员测试。");
            return;
        }

        if (command.ArgCount < 3 ||
            !int.TryParse(command.GetArg(1), out var slot) ||
            !int.TryParse(command.GetArg(2), out var operatorIndex) ||
            invalidArguments(slot, operatorIndex))
        {
            command.ReplyToCommand("用法：css_ops_assign <slot> <1-6>");
            return;
        }

        var target = Utilities.GetPlayerFromSlot(slot);
        if (target == null || !IsHumanController(target))
        {
            command.ReplyToCommand($"槽位 {slot} 没有真人玩家。");
            return;
        }

        var definition = _operators[operatorIndex - 1];
        SelectOperator(target, GetPlayerKey(target), definition, automatic: false);
        command.ReplyToCommand($"已将槽位 {slot} 设为 {definition.Name}。");

        static bool invalidArguments(int slot, int operatorIndex) =>
            slot < 0 || operatorIndex < 1 || operatorIndex > 6;
    }

    [ConsoleCommand("css_ops_test_smoke", "服务器控制台：测试蜂医长烟")]
    public void OnServerTestSmokeCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行长烟测试。");
            return;
        }

        if (command.ArgCount < 2 || !int.TryParse(command.GetArg(1), out var slot))
        {
            command.ReplyToCommand("用法：css_ops_test_smoke <slot>");
            return;
        }

        var target = Utilities.GetPlayerFromSlot(slot);
        if (target == null || !IsHumanController(target) || !target.PawnIsAlive)
        {
            command.ReplyToCommand($"槽位 {slot} 没有存活真人玩家。");
            return;
        }

        var spawned = SpawnLongSmoke(target);
        command.ReplyToCommand($"长烟测试完成：生成 {spawned} 段烟雾。");
    }

    [ConsoleCommand("css_ops_test_luna_dot", "服务器控制台：测试露娜持续伤害")]
    public void OnServerTestLunaDotCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行露娜测试。");
            return;
        }

        if (command.ArgCount < 3 ||
            !int.TryParse(command.GetArg(1), out var lunaSlot) ||
            !int.TryParse(command.GetArg(2), out var targetSlot))
        {
            command.ReplyToCommand("用法：css_ops_test_luna_dot <lunaSlot> <targetSlot>");
            return;
        }

        var luna = Utilities.GetPlayerFromSlot(lunaSlot);
        var target = Utilities.GetPlayerFromSlot(targetSlot);
        if (luna == null || target == null ||
            !IsHumanController(luna) || !IsCombatController(target) ||
            !IsEnemy(luna, target))
        {
            command.ReplyToCommand("需要两名不同队伍的存活玩家。");
            return;
        }

        var state = GetOrCreateState(luna, GetPlayerKey(luna));
        state.Operator = OperatorId.Luna;
        state.LastLunaDamagedTargetKey = GetPlayerKey(target);
        ShowOutline(target, Config.LunaPunishOutlineSeconds);
        StartLunaDamageOverTime(luna, target);
        command.ReplyToCommand($"露娜持续伤害测试已启动：{luna.PlayerName} -> {target.PlayerName}");
    }

    [ConsoleCommand("css_ops_test_damage", "服务器控制台：测试直接伤害")]
    public void OnServerTestDamageCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行伤害测试。");
            return;
        }

        if (command.ArgCount < 4 ||
            !int.TryParse(command.GetArg(1), out var attackerSlot) ||
            !int.TryParse(command.GetArg(2), out var victimSlot) ||
            !int.TryParse(command.GetArg(3), out var damage) ||
            damage < 1)
        {
            command.ReplyToCommand("用法：css_ops_test_damage <attackerSlot> <victimSlot> <damage>");
            return;
        }

        var attacker = Utilities.GetPlayerFromSlot(attackerSlot);
        var victim = Utilities.GetPlayerFromSlot(victimSlot);
        if (attacker == null || victim == null ||
            !IsHumanController(attacker) || !IsCombatController(victim) ||
            !attacker.PawnIsAlive || !victim.PawnIsAlive ||
            !IsEnemy(attacker, victim))
        {
            command.ReplyToCommand("需要两名不同队伍的存活玩家。");
            return;
        }

        DealDamage(attacker, victim, Math.Min(damage, 500));
        command.ReplyToCommand(
            $"伤害测试已执行：{attacker.PlayerName} -> {victim.PlayerName}，{damage}。");
    }

    [ConsoleCommand("css_roles", "查看全部干员说明")]
    public void OnRolesCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        PrintRoleList(player, includeDescriptions: true);
    }

    [ConsoleCommand("css_v", "使用 V 技能")]
    [ConsoleCommand("css_skill1", "使用 V 技能")]
    [ConsoleCommand("css_skill", "使用 V 技能")]
    public void OnVSkillCommand(CCSPlayerController? player, CommandInfo command)
    {
        UseSkillSlot(player, command, useV: true);
    }

    [ConsoleCommand("css_x", "使用 X 技能")]
    [ConsoleCommand("css_skill2", "使用 X 技能")]
    [ConsoleCommand("css_q", "使用 X 技能")]
    public void OnXSkillCommand(CCSPlayerController? player, CommandInfo command)
    {
        UseSkillSlot(player, command, useV: false);
    }

    [ConsoleCommand("css_bind", "绑定 V/X 技能键")]
    [ConsoleCommand("css_binds", "绑定 V/X 技能键")]
    public void OnBindCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        ShowSkillBindingHelp(player, force: true);
    }

    private void UseSkillSlot(CCSPlayerController? player, CommandInfo command, bool useV)
    {
        if (player == null || !IsHumanController(player))
        {
            command.ReplyToCommand("[三角洲模式] 该命令需要在游戏内使用。");
            return;
        }

        if (_raidMode && !RaidActive(player.SteamID)) return;
        var state = GetExistingState(player);
        if (state?.Operator == null)
        {
            player.PrintToChat(" [三角洲模式] 请先输入 !characterselect <1-6> 选择干员。");
            return;
        }

        if (!player.PawnIsAlive)
        {
            player.PrintToChat(" [三角洲模式] 存活时才能使用主动技能。");
            return;
        }

        var remainingSeconds = GetCooldownRemainingSeconds(state, useV);
        if (remainingSeconds > 0)
        {
            var slot = useV ? "V" : "X";
            player.PrintToChat($" [三角洲模式] {slot} 冷却中，还需 {remainingSeconds} 秒。");
            player.PrintToCenter($"{slot} 冷却中：{remainingSeconds} 秒");
            Logger.LogInformation(
                "Player '{Player}' blocked {Slot} skill by cooldown, remaining={Remaining}s.",
                player.PlayerName, slot, remainingSeconds);
            return;
        }

        if (!(useV
                ? TryUseOperatorVSkill(player, state.Operator.Value)
                : TryUseOperatorXSkill(player, state.Operator.Value)))
        {
            return;
        }

        var definition = GetOperator(state.Operator.Value);
        var skillName = useV ? definition.VName : definition.XName;
        var cooldownSeconds = useV ? Config.VCooldownSeconds : Config.XCooldownSeconds;
        StartSkillCooldown(player, state, useV);

        player.PrintToCenter(
            $"已使用 {(useV ? "V" : "X")} 技能：{skillName} | CD {cooldownSeconds} 秒");
        player.PrintToChat(
            $" [冷却] {(useV ? "V" : "X")} 进入 {cooldownSeconds} 秒冷却；" +
            "输入 !cd 查看剩余时间。");
        Logger.LogInformation("Player '{Player}' used {Operator} {Slot} skill {Skill}.",
            player.PlayerName, definition.Name, useV ? "V" : "X", skillName);
    }

    [ConsoleCommand("css_ops_reset", "服务器控制台：清空全部干员状态")]
    public void OnResetCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null)
        {
            player.PrintToChat(" [三角洲模式] 仅服务器控制台可执行模式重置。");
            return;
        }

        ResetAllPlayersAndState("管理员重置", broadcast: true);
        command.ReplyToCommand("[三角洲模式] 已清空全部干员状态。");
    }

    private void ShowOperatorSelection(CCSPlayerController player, string key)
    {
        if (_raidMode) return;
        var state = GetOrCreateState(player, key);
        if (state.Operator.HasValue)
        {
            return;
        }

        if (state.SelectionTimer == null)
        {
            state.SelectionSecondsRemaining = Config.SelectionSeconds;
            state.SelectionTimer = AddTimer(1.0f, () => TickSelectionTimeout(key),
                TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            player.PrintToChat(
                $" [三角洲模式] 选角已开始，{Config.SelectionSeconds} 秒内输入 " +
                "!characterselect <1-6> 选择。");
        }

        PrintRoleList(player, includeDescriptions: false);
    }

    private void TickSelectionTimeout(string key)
    {
        if (!_states.TryGetValue(key, out var state) || state.Operator.HasValue)
        {
            return;
        }

        state.SelectionSecondsRemaining--;
        if (state.SelectionSecondsRemaining <= 0)
        {
            state.SelectionTimer?.Kill();
            state.SelectionTimer = null;
            var current = FindPlayerByKey(key);
            var selected = _operators[Random.Shared.Next(_operators.Count)];
            if (current != null)
            {
                SelectOperator(current, key, selected, automatic: true);
            }
            else
            {
                state.Operator = selected.Id;
                Logger.LogInformation("Offline player '{Player}' auto-selected {Operator}.",
                    state.LastKnownName, selected.Name);
            }
            return;
        }

        if (state.SelectionSecondsRemaining is 30 or 15 or 10 or 5)
        {
            var warningPlayer = FindPlayerByKey(key);
            warningPlayer?.PrintToChat(
                $" [三角洲模式] 选角剩余 {state.SelectionSecondsRemaining} 秒，" +
                "请尽快输入 !characterselect <1-6>。");
        }
    }

    private void PrintRoleList(CCSPlayerController player, bool includeDescriptions)
    {
        player.PrintToChat(" [三角洲模式] 选择干员：!characterselect <1-6>");
        for (var i = 0; i < _operators.Count; i++)
        {
            var role = _operators[i];
            if (includeDescriptions)
            {
                player.PrintToChat(
                    $" {i + 1}. {role.Name}({role.EnglishName}) [{role.Role}] " +
                    $"V:{role.VName} | X:{role.XName} | 被动:{role.PassiveName}");
            }
            else
            {
                player.PrintToChat($" {i + 1}. {role.Name} / {role.EnglishName} [{role.Role}] V:{role.VName} X:{role.XName}");
            }
        }
    }

    private void SelectOperator(
        CCSPlayerController player,
        string key,
        OperatorDefinition definition,
        bool automatic)
    {
        if (_raidMode && !_bridgeAssigning) return;
        var state = GetOrCreateState(player, key);
        if (state.Operator.HasValue && state.Operator.Value != definition.Id)
        {
            ResetOperatorForSwitch(player, key, state);
        }
        state.SelectionTimer?.Kill();
        state.SelectionTimer = null;
        CancelArmedGrenade(key);
        state.Operator = definition.Id;
        ResetSkillCooldowns(state);
        ApplyPassive(player, definition.Id);
        ShowSkillBindingHelp(player);

        player.PrintToChat(
            $" [三角洲模式] {(automatic ? "超时自动选择" : "已选择")} {definition.Name} " +
            $"({definition.EnglishName})。");
        player.PrintToChat($" [V] {definition.VName}: {definition.VDescription}");
        player.PrintToChat($" [X] {definition.XName}: {definition.XDescription}");
        player.PrintToChat($" [被动] {definition.PassiveName}: {definition.PassiveDescription}");
        player.PrintToChat(
            $" [操作] V/X 为独立技能，冷却分别为 {Config.VCooldownSeconds} / " +
            $"{Config.XCooldownSeconds} 秒；输入 !cd 查看冷却。");
        Logger.LogInformation("Player '{Player}' selected operator {Operator}, automatic={Automatic}.",
            player.PlayerName, definition.Name, automatic);
    }

    private bool TryUseOperatorVSkill(CCSPlayerController player, OperatorId operatorId)
    {
        switch (operatorId)
        {
            case OperatorId.RedWolf:
                ApplyTemporarySpeed(player, Config.RedWolfSpeedMultiplier, Config.RedWolfDurationSeconds);
                player.PrintToChat(" [红狼] 动力外骨骼启动，移动速度大幅提升。");
                return true;

            case OperatorId.Stinger:
                return UseStingerHeal(player);

            case OperatorId.Luna:
                return UseScan(player, Config.ScanRange, maxTargets: null, "侦测箭", revealToTeam: true);

            case OperatorId.Vyron:
                if (!ApplyDirectionalBoost(player, horizontal: 700f, vertical: 300f))
                {
                    return false;
                }
                player.PrintToChat(" [威龙] 按输入方向启动喷气推进。");
                return true;

            case OperatorId.Hackclaw:
                return UseScan(player, Config.ScanRange, maxTargets: null, "信号解码器", revealToTeam: true);

            case OperatorId.Shepherd:
                return PlaceLandmine(player);

            default:
                return false;
        }
    }

    private bool TryUseOperatorXSkill(CCSPlayerController player, OperatorId operatorId)
    {
        switch (operatorId)
        {
            case OperatorId.RedWolf:
                return UseRedWolfLeap(player);

            case OperatorId.Stinger:
                return UseStingerSmoke(player);

            case OperatorId.Luna:
                return UseLunaVoltArrow(player);

            case OperatorId.Vyron:
                return UseVyronGrenade(player);

            case OperatorId.Hackclaw:
                return UseHackclawFlashbang(player);

            case OperatorId.Shepherd:
                return UseShepherdGrenade(player);

            default:
                return false;
        }
    }

    private bool UseStingerHeal(CCSPlayerController player)
    {
        var healed = 0;
        foreach (var target in Utilities.GetPlayers().ToList())
        {
            if (!IsCombatController(target) || !target.PawnIsAlive ||
                !IsAlly(player, target) ||
                Distance(player, target) > Config.StingerHealRadius)
            {
                continue;
            }

            HealPlayer(target, Config.StingerHealAmount);
            healed++;
            if (target.Handle != player.Handle)
            {
                target.PrintToCenter($"蜂医治疗 +{Config.StingerHealAmount}");
            }
        }

        if (healed == 0)
        {
            player.PrintToChat(" [蜂医] 范围内没有可治疗目标。");
            return false;
        }

        player.PrintToChat($" [蜂医] 激素治疗完成，共治疗 {healed} 名队友。");
        return true;
    }

    private bool UseRedWolfLeap(CCSPlayerController player)
    {
        if (!ApplyDirectionalBoost(player, horizontal: 500f, vertical: 280f))
        {
            return false;
        }

        HealPlayer(player, 15);
        player.PrintToChat(" [红狼] 按输入方向动力跃迁，恢复 15 生命。");
        return true;
    }

    private bool UseStingerSmoke(CCSPlayerController player)
    {
        var spawned = SpawnLongSmoke(player);
        if (spawned <= 0)
        {
            player.PrintToChat(" [蜂医] 长烟生成失败，请对准可落烟的地面后重试。");
            return false;
        }

        player.PrintToChat($" [蜂医] 蜂巢长烟已释放，生成 {spawned} 段连续烟雾。");
        Logger.LogInformation(
            "Player '{Player}' spawned {CloudCount} long-smoke segments.",
            player.PlayerName,
            spawned);
        return true;
    }

    private int SpawnLongSmoke(CCSPlayerController player)
    {
        var pawn = player.PlayerPawn.Value;
        var origin = pawn?.AbsOrigin;
        if (pawn == null || !pawn.IsValid || origin == null)
        {
            return 0;
        }

        var forward = new Vector();
        NativeAPI.AngleVectors(pawn.EyeAngles.Handle, forward.Handle, IntPtr.Zero, IntPtr.Zero);

        var traceStart = new Vector(origin.X, origin.Y, origin.Z + 48f);
        var forwardTrace = Trace.TraceShape(traceStart, pawn.EyeAngles, pawn);
        var maxDistance = Config.StingerLongSmokeRange;
        if (forwardTrace.DidHit())
        {
            maxDistance = Math.Min(maxDistance, forwardTrace.Fraction * 8192f);
        }

        var spawned = 0;
        for (var i = 0; i < Config.StingerLongSmokeClouds; i++)
        {
            var distance = Config.StingerLongSmokeStartDistance +
                           i * Config.StingerLongSmokeSpacing;
            if (distance > maxDistance)
            {
                break;
            }

            var pathPoint = new Vector(
                origin.X + forward.X * distance,
                origin.Y + forward.Y * distance,
                origin.Z + forward.Z * distance);
            var downStart = new Vector(pathPoint.X, pathPoint.Y, pathPoint.Z + 96f);
            var downEnd = new Vector(pathPoint.X, pathPoint.Y, pathPoint.Z - 512f);
            var floorTrace = Trace.TraceEndShape(downStart, downEnd, pawn);
            var detonationPoint = floorTrace.DidHit()
                ? floorTrace.HitPoint
                : pathPoint;
            detonationPoint.Z += 2f;

            if (CreateSmokeCloud(player, pawn, detonationPoint, 0.08f + i * 0.08f))
            {
                spawned++;
            }
        }

        return spawned;
    }

    private bool CreateSmokeCloud(
        CCSPlayerController player,
        CCSPlayerPawn pawn,
        Vector position,
        float detonationDelay)
    {
        var projectile = Utilities.CreateEntityByName<CSmokeGrenadeProjectile>(
            "smokegrenade_projectile");
        if (projectile == null || !projectile.IsValid)
        {
            return false;
        }

        projectile.Teleport(position, null, new Vector(IntPtr.Zero));
        projectile.DispatchSpawn();
        projectile.IsLive = true;
        projectile.IsSmokeGrenade = true;
        projectile.SpawnTime = Server.CurrentTime;
        projectile.DetonateTime = Server.CurrentTime + detonationDelay;
        projectile.InitialPosition.X = position.X;
        projectile.InitialPosition.Y = position.Y;
        projectile.InitialPosition.Z = position.Z;
        projectile.InitialVelocity.X = 0f;
        projectile.InitialVelocity.Y = 0f;
        projectile.InitialVelocity.Z = 0f;
        projectile.OriginalSpawnLocation.X = position.X;
        projectile.OriginalSpawnLocation.Y = position.Y;
        projectile.OriginalSpawnLocation.Z = position.Z;
        projectile.DidSmokeEffect = false;
        projectile.SmokeEffectTickBegin = 0;
        projectile.SmokeDetonationPos.X = position.X;
        projectile.SmokeDetonationPos.Y = position.Y;
        projectile.SmokeDetonationPos.Z = position.Z;
        projectile.Thrower.Raw = pawn.EntityHandle.Raw;
        projectile.OriginalThrower.Raw = pawn.EntityHandle.Raw;
        projectile.OwnerEntity.Raw = pawn.EntityHandle.Raw;
        AddTimer(detonationDelay, () =>
        {
            if (projectile == null || !projectile.IsValid)
            {
                return;
            }

            projectile.SmokeDetonationPos.X = position.X;
            projectile.SmokeDetonationPos.Y = position.Y;
            projectile.SmokeDetonationPos.Z = position.Z;
            projectile.SmokeEffectTickBegin = Server.TickCount;
            projectile.AcceptInput("Detonate");
            Logger.LogInformation(
                "Detonated long-smoke segment at {X},{Y},{Z}.",
                position.X,
                position.Y,
                position.Z);
        }, TimerFlags.STOP_ON_MAPCHANGE);
        return true;
    }

    private bool UseLunaVoltArrow(CCSPlayerController player)
    {
        var state = GetExistingState(player);
        if (state?.LastLunaDamagedTargetKey == null &&
            state?.LastLunaAttackerKey == null)
        {
            player.PrintToChat(" [露娜] 还没有记录到被你伤害的敌人。");
            return false;
        }

        var targetKey = state.LastLunaDamagedTargetKey ??
                        state.LastLunaAttackerKey;
        var target = targetKey == null ? null : FindPlayerByKey(targetKey);
        if (target == null || !target.PawnIsAlive || !IsEnemy(player, target))
        {
            player.PrintToChat(" [露娜] 最近被你伤害的敌人已不在场或已经死亡。");
            return false;
        }

        ShowOutline(target, Config.LunaPunishOutlineSeconds);
        StartLunaDamageOverTime(player, target);
        target.PrintToCenter("被露娜锁定：持续受伤并显形");
        player.PrintToChat(
            $" [露娜] 已锁定最近被你伤害的敌人 {target.PlayerName}：" +
            $"{Config.LunaPunishDamage}×{Config.LunaPunishTicks} 持续伤害，显形 " +
            $"{Config.LunaPunishOutlineSeconds:0.#} 秒。");
        return true;
    }

    private void StartLunaDamageOverTime(
        CCSPlayerController luna,
        CCSPlayerController target)
    {
        var lunaKey = GetPlayerKey(luna);
        var targetKey = GetPlayerKey(target);
        var state = GetExistingState(luna);
        if (state == null || state.Operator != OperatorId.Luna)
        {
            return;
        }

        StopLunaDot(lunaKey);
        state.LunaDotTargetKey = targetKey;
        state.LunaPassiveSuppressedVictims.Add(targetKey);
        var ticks = 0;
        CssTimer? timer = null;
        timer = AddTimer(Config.LunaPunishIntervalSeconds, () =>
        {
            if (!_states.TryGetValue(lunaKey, out var currentState) ||
                currentState.Operator != OperatorId.Luna ||
                currentState.LunaDotTimer != timer ||
                currentState.LunaDotTargetKey != targetKey)
            {
                return;
            }

            var currentLuna = FindPlayerByKey(lunaKey);
            var currentTarget = FindPlayerByKey(targetKey);
            if (currentLuna == null || currentTarget == null ||
                !currentLuna.PawnIsAlive || !currentTarget.PawnIsAlive ||
                !IsEnemy(currentLuna, currentTarget))
            {
                StopLunaDot(lunaKey);
                return;
            }

            DealDamage(currentLuna, currentTarget, Config.LunaPunishDamage);
            ticks++;
            Logger.LogInformation(
                "Luna DoT tick {Tick}/{Ticks}: '{Luna}' -> '{Target}' for {Damage}.",
                ticks,
                Config.LunaPunishTicks,
                currentLuna.PlayerName,
                currentTarget.PlayerName,
                Config.LunaPunishDamage);
            if (ticks >= Config.LunaPunishTicks)
            {
                StopLunaDot(lunaKey);
            }
        }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        state.LunaDotTimer = timer;
    }

    private void StopLunaDot(string lunaKey)
    {
        if (!_states.TryGetValue(lunaKey, out var state))
        {
            return;
        }

        state.LunaDotTimer?.Kill();
        state.LunaDotTimer = null;
        if (state.LunaDotTargetKey != null)
        {
            state.LunaPassiveSuppressedVictims.Remove(state.LunaDotTargetKey);
            state.LunaDotTargetKey = null;
        }
    }

    private void ClearAllLunaDots()
    {
        foreach (var state in _states.Values)
        {
            if (state.LunaDotTimer != null)
            {
                state.LunaDotTimer.Kill();
                state.LunaDotTimer = null;
            }

            state.LunaDotTargetKey = null;
            state.LunaPassiveSuppressedVictims.Clear();
        }
    }

    private void CancelArmedGrenade(string playerKey)
    {
        if (!_states.TryGetValue(playerKey, out var state))
        {
            return;
        }

        var player = FindPlayerByKey(playerKey);
        ResetArmedGrenade(state, player, clearHud: true);
    }

    private void ClearAllArmedGrenades()
    {
        foreach (var entry in _states)
        {
            var player = FindPlayerByKey(entry.Key);
            ResetArmedGrenade(entry.Value, player, clearHud: true);
        }
    }

    private void ScheduleGrenadeCancel(
        CCSPlayerController player,
        PlayerState state)
    {
        if (!state.ArmedGrenade.HasValue || state.GrenadeCancelTimer != null)
        {
            return;
        }

        player.PrintToCenter(" ");
        var key = GetPlayerKey(player);
        var armedSequence = state.GrenadeSequence;
        state.GrenadeCancelTimer = AddTimer(0.15f, () =>
        {
            if (!_states.TryGetValue(key, out var currentState) ||
                currentState.GrenadeSequence != armedSequence ||
                !currentState.ArmedGrenade.HasValue)
            {
                return;
            }

            var currentPlayer = FindPlayerByKey(key);
            if (currentPlayer != null)
            {
                var grenade = GetActiveGrenade(
                    currentPlayer,
                    currentState.ArmedGrenade.Value);
                if (grenade != null && IsValidGrenade(grenade) && grenade.PinPulled)
                {
                    return;
                }

                currentPlayer.PrintToCenter("拉栓已取消，引信计时停止");
            }

            ResetArmedGrenade(
                currentState,
                currentPlayer,
                clearHud: true);
        }, TimerFlags.STOP_ON_MAPCHANGE);
    }

    private static void ResetArmedGrenade(
        PlayerState state,
        CCSPlayerController? player = null,
        bool clearHud = false)
    {
        state.GrenadeCancelTimer?.Kill();
        state.GrenadeCancelTimer = null;
        state.ArmedGrenade = null;
        state.GrenadePinPulledAtMs = 0;
        state.LastGrenadeHudUpdateMs = 0;
        state.GrenadeSequence++;

        if (clearHud && player != null && IsHumanController(player))
        {
            player.PrintToCenter(" ");
        }
    }

    private void ClearControl(string playerKey)
    {
        if (_controlTimers.Remove(playerKey, out var timer))
        {
            timer.Kill();
        }

        _controlPreviousMoveTypes.Remove(playerKey);
        var player = FindPlayerByKey(playerKey);
        var pawn = player?.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        pawn.MoveType = MoveType_t.MOVETYPE_WALK;
        pawn.ActualMoveType = MoveType_t.MOVETYPE_WALK;
        var state = GetExistingState(player!);
        pawn.VelocityModifier = state?.Operator is { } operatorId
            ? GetOperator(operatorId).BaseSpeed
            : 1.0f;
        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
    }

    private void ClearAllControlTimers()
    {
        foreach (var key in _controlTimers.Keys.ToList())
        {
            ClearControl(key);
        }
    }

    private bool UseHackclawFlashbang(CCSPlayerController player)
    {
        if (GiveNamedItemToHand(player, "weapon_flashbang") == null)
        {
            player.PrintToChat(" [骇爪] 额外闪光弹发放失败。");
            return false;
        }

        player.PrintToChat(
            " [骇爪] 获得一枚额外闪光弹。按住原生左键拉栓开始 3 秒计时；" +
            "松开或取消拉栓会停止计时。");
        return true;
    }

    private bool UseShepherdGrenade(CCSPlayerController player)
    {
        if (CountNamedWeapons(player, "weapon_hegrenade") >= 3)
        {
            player.PrintToChat(" [牧羊人] 你已经携带 3 枚高爆手雷，请先投掷后再使用 X。");
            return false;
        }

        if (GiveNamedItemToHand(player, "weapon_hegrenade") == null)
        {
            player.PrintToChat(" [牧羊人] 强化高爆手雷发放失败。");
            return false;
        }

        player.PrintToChat(
            " [牧羊人] 获得一枚强化高爆手雷。原生拉栓会启动 3 秒计时；" +
            $"投掷后范围为 {Config.ShepherdGrenadeRadius:0}，伤害为 " +
            $"{Config.ShepherdGrenadeDamage}。");
        return true;
    }

    private bool UseVyronGrenade(CCSPlayerController player)
    {
        if (CountNamedWeapons(player, "weapon_hegrenade") >= 3)
        {
            player.PrintToChat(" [威龙] 你已经携带 3 枚高爆手雷，请先投掷后再使用 X。");
            return false;
        }

        if (GiveNamedItemToHand(player, "weapon_hegrenade") == null)
        {
            player.PrintToChat(" [威龙] 高爆手雷发放失败。");
            return false;
        }
        player.PrintToChat(
            " [威龙] 获得标准高爆手雷；按正常方式投掷，爆炸范围内敌人" +
            $"缴械并定身 {Config.VyronControlSeconds:0.#} 秒。");
        return true;
    }

    private static int CountNamedWeapons(
        CCSPlayerController player,
        string designerName)
    {
        var weapons = player.PlayerPawn.Value?.WeaponServices?.MyWeapons;
        return weapons?.Count(handle =>
            handle.Value is { IsValid: true } weapon &&
            weapon.DesignerName.Equals(designerName, StringComparison.OrdinalIgnoreCase)) ?? 0;
    }

    private static CBasePlayerWeapon? GiveNamedItemToHand(
        CCSPlayerController player,
        string designerName)
    {
        var weapon = player.GiveNamedItem<CBasePlayerWeapon>(designerName);
        if (weapon == null || !weapon.IsValid)
        {
            return null;
        }

        var weaponServices = player.PlayerPawn.Value?.WeaponServices;
        if (weaponServices != null)
        {
            weaponServices.ActiveWeapon.Raw = weapon.EntityHandle.Raw;
        }

        player.ExecuteClientCommandFromServer($"use {designerName}");
        return weapon;
    }

    private bool PlaceLandmine(CCSPlayerController player)
    {
        var pawn = player.PlayerPawn.Value;
        var origin = pawn?.AbsOrigin;
        if (pawn == null || !pawn.IsValid || origin == null)
        {
            return false;
        }

        var mineEntity = Utilities.CreateEntityByName<CHEGrenadeProjectile>(
            "hegrenade_projectile");
        if (mineEntity == null || !mineEntity.IsValid)
        {
            player.PrintToChat(" [牧羊人] 感应地雷布置失败。");
            return false;
        }
        var entityIndex = mineEntity.Index;

        var position = new Vector(origin.X, origin.Y, origin.Z + 2f);
        mineEntity.Teleport(position, null, new Vector(IntPtr.Zero));
        mineEntity.DispatchSpawn();
        mineEntity.IsLive = true;
        mineEntity.IsSmokeGrenade = false;
        mineEntity.Damage = 0f;
        mineEntity.DmgRadius = 1f;
        mineEntity.DetonateTime =
            Server.CurrentTime + Config.ShepherdMineLifetimeSeconds + 10f;
        mineEntity.Thrower.Raw = pawn.EntityHandle.Raw;
        mineEntity.OriginalThrower.Raw = pawn.EntityHandle.Raw;
        mineEntity.OwnerEntity.Raw = pawn.EntityHandle.Raw;

        var ownerKey = GetPlayerKey(player);
        var mine = new LandmineState
        {
            OwnerKey = ownerKey,
            OwnerName = player.PlayerName,
            OwnerTeam = player.TeamNum,
            Position = position,
            EntityIndex = (int)entityIndex
        };
        mine.Timer = AddTimer(0.2f, () => TickLandmine(mine),
            TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        _landmines.Add(mine);
        player.PrintToChat(
            $" [牧羊人] 感应地雷已布置，触发半径 " +
            $"{Config.ShepherdMineTriggerRadius:0}，伤害半径 " +
            $"{Config.ShepherdMineDamageRadius:0}。");
        return true;
    }

    private void TickLandmine(LandmineState mine)
    {
        if (!_landmines.Contains(mine))
        {
            return;
        }

        var owner = FindPlayerByKey(mine.OwnerKey);
        if (owner == null)
        {
            RemoveLandmine(mine);
            return;
        }

        foreach (var target in Utilities.GetPlayers().ToList())
        {
            if (!IsCombatController(target) || !target.PawnIsAlive ||
                !IsEnemy(owner, target) ||
                DistanceToPoint(target, mine.Position) > Config.ShepherdMineTriggerRadius)
            {
                continue;
            }

            DetonateLandmine(mine, owner);
            return;
        }
    }

    private void DetonateLandmine(
        LandmineState mine,
        CCSPlayerController owner)
    {
        if (!_landmines.Remove(mine))
        {
            return;
        }

        mine.Timer?.Kill();
        var visual = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>(
            mine.EntityIndex);
        if (visual != null && visual.IsValid)
        {
            visual.Damage = 0f;
            visual.DmgRadius = 1f;
            visual.AcceptInput("Detonate");
        }

        foreach (var target in Utilities.GetPlayers().ToList())
        {
            if (!IsCombatController(target) || !target.PawnIsAlive ||
                !IsEnemy(owner, target))
            {
                continue;
            }

            var distance = DistanceToPoint(target, mine.Position);
            if (distance > Config.ShepherdMineDamageRadius)
            {
                continue;
            }

            var ratio = Math.Clamp(
                distance / Config.ShepherdMineDamageRadius,
                0f,
                1f);
            var damage = Math.Max(
                1,
                (int)(Config.ShepherdMineMaxDamage * (0.35f + 0.65f * (1f - ratio))));
            DealDamage(owner, target, damage);
            target.PrintToCenter($"感应地雷命中：-{damage} HP");
            target.PrintToChat($" [牧羊人] 你触发了感应地雷，受到 {damage} 伤害。");
        }

        owner.PrintToChat($" [牧羊人] 感应地雷已引爆。");
        owner.PrintToCenter("感应地雷已引爆");
    }

    private void RemoveLandmine(LandmineState mine)
    {
        if (!_landmines.Remove(mine))
        {
            return;
        }

        mine.Timer?.Kill();
        var visual = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>(
            mine.EntityIndex);
        if (visual != null && visual.IsValid)
        {
            visual.Remove();
        }
    }

    private void ClearLandminesForOwner(string ownerKey)
    {
        foreach (var mine in _landmines
                     .Where(item => item.OwnerKey == ownerKey)
                     .ToList())
        {
            RemoveLandmine(mine);
        }
    }

    private void ClearAllLandmines()
    {
        foreach (var mine in _landmines.ToList())
        {
            RemoveLandmine(mine);
        }
    }

    private void ApplyControlDebuff(CCSPlayerController player, float duration)
    {
        var key = GetPlayerKey(player);
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid || !player.PawnIsAlive)
        {
            return;
        }

        if (_controlTimers.Remove(key, out var oldTimer))
        {
            oldTimer.Kill();
        }
        else
        {
            _controlPreviousMoveTypes[key] = pawn.MoveType;
        }

        pawn.MoveType = MoveType_t.MOVETYPE_NONE;
        pawn.ActualMoveType = MoveType_t.MOVETYPE_NONE;
        pawn.VelocityModifier = 0f;
        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");

        var weaponServices = pawn.WeaponServices?.As<CCSPlayer_WeaponServices>();
        if (weaponServices != null)
        {
            weaponServices.NextAttack = Server.CurrentTime + duration;
            foreach (var weaponHandle in weaponServices.MyWeapons)
            {
                var weapon = weaponHandle.Value;
                if (weapon == null || !weapon.IsValid)
                {
                    continue;
                }

                weapon.NextPrimaryAttackTick =
                    Server.TickCount + (int)(duration / Server.TickInterval);
                weapon.NextSecondaryAttackTick =
                    Server.TickCount + (int)(duration / Server.TickInterval);
            }
        }

        player.PrintToChat(
            $" [控制] 你被缴械并定身 {duration:0.#} 秒。");
        CssTimer? timer = null;
        timer = AddTimer(duration, () =>
        {
            if (!_controlTimers.TryGetValue(key, out var current) ||
                current != timer)
            {
                return;
            }

            _controlTimers.Remove(key);
            var currentPlayer = FindPlayerByKey(key);
            var currentPawn = currentPlayer?.PlayerPawn.Value;
            if (currentPawn != null && currentPawn.IsValid)
            {
                currentPawn.MoveType = _controlPreviousMoveTypes.Remove(
                    key,
                    out var previousMoveType)
                    ? previousMoveType
                    : MoveType_t.MOVETYPE_WALK;
                currentPawn.ActualMoveType = currentPawn.MoveType;
                var state = currentPlayer == null ? null : GetExistingState(currentPlayer);
                currentPawn.VelocityModifier = state?.Operator is { } operatorId
                    ? GetOperator(operatorId).BaseSpeed
                    : 1.0f;
                Utilities.SetStateChanged(currentPawn, "CBaseEntity", "m_MoveType");
                Utilities.SetStateChanged(currentPawn, "CCSPlayerPawn", "m_flVelocityModifier");
            }
        }, TimerFlags.STOP_ON_MAPCHANGE);
        _controlTimers[key] = timer;
    }

    private static float DistanceToPoint(
        CCSPlayerController player,
        Vector point)
    {
        var origin = player.PlayerPawn.Value?.AbsOrigin;
        if (origin == null)
        {
            return float.MaxValue;
        }

        var dx = origin.X - point.X;
        var dy = origin.Y - point.Y;
        var dz = origin.Z - point.Z;
        return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    private static CBaseCSGrenade? GetActiveGrenade(
        CCSPlayerController player,
        CookedGrenadeKind kind)
    {
        var weapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;
        if (weapon == null || !weapon.IsValid)
        {
            return null;
        }

        var expectedName = kind == CookedGrenadeKind.Flashbang
            ? "weapon_flashbang"
            : "weapon_hegrenade";
        if (!weapon.DesignerName.Equals(expectedName, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return weapon.As<CBaseCSGrenade>();
    }

    private static bool IsValidGrenade(CBaseCSGrenade? grenade)
    {
        return grenade != null && grenade.IsValid;
    }

    private static void RenderGrenadePinHud(
        CCSPlayerController player,
        float remaining,
        CookedGrenadeKind kind)
    {
        var displayTenths = Math.Max(1, (int)Math.Ceiling(remaining * 10f));
        var displaySeconds = displayTenths / 10f;
        var label = kind == CookedGrenadeKind.Flashbang
            ? "闪光弹"
            : "高爆手雷";
        player.PrintToCenter(
            remaining <= 1f
                ? $"警告：{label}引信 {displaySeconds:0.0} 秒！"
                : $"{label}引信 {displaySeconds:0.0} 秒 | 松开左键投掷");
    }

    private float GetGrenadePinRemainingSeconds(PlayerState state)
    {
        if (!state.ArmedGrenade.HasValue)
        {
            return 0f;
        }

        return Math.Max(
            0f,
            Config.CookMaxFuseSeconds -
            (Environment.TickCount64 - state.GrenadePinPulledAtMs) / 1000f);
    }

    private static bool TryGetGrenadeKindFromEventName(
        string eventWeapon,
        out CookedGrenadeKind kind)
    {
        if (eventWeapon.Contains("flashbang", StringComparison.OrdinalIgnoreCase))
        {
            kind = CookedGrenadeKind.Flashbang;
            return true;
        }

        if (eventWeapon.Contains("hegrenade", StringComparison.OrdinalIgnoreCase))
        {
            kind = CookedGrenadeKind.HeGrenade;
            return true;
        }

        kind = default;
        return false;
    }

    private static CBaseCSGrenadeProjectile? FindLatestThrownGrenade(
        CCSPlayerController player,
        CookedGrenadeKind kind)
    {
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return null;
        }

        var designerName = kind == CookedGrenadeKind.Flashbang
            ? "flashbang_projectile"
            : "hegrenade_projectile";
        return Utilities.FindAllEntitiesByDesignerName<CBaseCSGrenadeProjectile>(
                designerName)
            .Where(projectile =>
                projectile.IsValid &&
                projectile.Thrower.Raw == pawn.EntityHandle.Raw)
            .OrderByDescending(projectile => projectile.CreateTime)
            .FirstOrDefault();
    }

    private bool ApplyThrownGrenadeEffect(
        CCSPlayerController player,
        CookedGrenadeKind kind,
        float? remainingFuse)
    {
        var projectile = FindLatestThrownGrenade(player, kind);
        if (projectile == null)
        {
            return false;
        }

        if (kind == CookedGrenadeKind.HeGrenade)
        {
            projectile.Damage = Config.ShepherdGrenadeDamage;
            projectile.DmgRadius = Config.ShepherdGrenadeRadius;
        }

        if (!remainingFuse.HasValue)
        {
            return true;
        }

        var fuse = Math.Max(0.05f, remainingFuse.Value);
        projectile.DetonateTime = Server.CurrentTime + fuse;
        if (projectile is CFlashbangProjectile flashbang)
        {
            flashbang.TimeToDetonate = fuse;
        }

        return true;
    }

    private void DetonateHeldGrenade(
        CCSPlayerController player,
        PlayerState state,
        CBaseCSGrenade grenade,
        CookedGrenadeKind kind)
    {
        var radius = kind == CookedGrenadeKind.HeGrenade
            ? Config.ShepherdGrenadeRadius
            : 0f;
        var damage = kind == CookedGrenadeKind.HeGrenade
            ? Config.ShepherdGrenadeDamage
            : 0;
        ResetArmedGrenade(state, player, clearHud: false);
        if (!SpawnGrenadeProjectile(
                player,
                kind,
                0.05f,
                radius,
                damage,
                detonateAtFeet: true))
        {
            Logger.LogError(
                "Player '{Player}' failed to detonate armed {Grenade} at their feet.",
                player.PlayerName,
                kind);
            player.PrintToChat(" [手雷] 脚下引爆生成失败，手雷未被消耗。");
            return;
        }

        ConsumeOneGrenade(player, grenade);
        player.PrintToCenter("引信燃尽，手雷在手中原地爆炸");
        player.PrintToChat(" [手雷] 拉栓计时耗尽，手雷已在手中原地引爆。");
        Logger.LogInformation(
            "Player '{Player}' held native {Grenade} until detonation.",
            player.PlayerName,
            kind);
    }

    private static void ConsumeOneGrenade(
        CCSPlayerController player,
        CBaseCSGrenade grenade)
    {
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        var reserveCount = grenade.ReserveAmmo.Length > 0
            ? grenade.ReserveAmmo[0]
            : 0;
        grenade.PinPulled = false;
        grenade.ThrowAnimating = false;
        if (reserveCount > 1)
        {
            grenade.ReserveAmmo[0] = reserveCount - 1;
            Utilities.SetStateChanged(
                grenade,
                "CBaseCSGrenade",
                "m_bPinPulled");
            Utilities.SetStateChanged(
                grenade,
                "CBasePlayerWeapon",
                "m_pReserveAmmo");
            player.ExecuteClientCommandFromServer("lastinv");
            return;
        }

        pawn.RemovePlayerItem(grenade);
    }

    private static bool SpawnGrenadeProjectile(
        CCSPlayerController player,
        CookedGrenadeKind kind,
        float fuseSeconds,
        float radius,
        int damage,
        bool detonateAtFeet = false)
    {
        var pawn = player.PlayerPawn.Value;
        var origin = pawn?.AbsOrigin;
        if (pawn == null || !pawn.IsValid || origin == null)
        {
            return false;
        }

        var forward = new Vector();
        NativeAPI.AngleVectors(
            pawn.EyeAngles.Handle,
            forward.Handle,
            IntPtr.Zero,
            IntPtr.Zero);

        CBaseCSGrenadeProjectile? projectile;
        if (kind == CookedGrenadeKind.Flashbang)
        {
            projectile = Utilities.CreateEntityByName<CFlashbangProjectile>(
                "flashbang_projectile");
        }
        else
        {
            projectile = Utilities.CreateEntityByName<CHEGrenadeProjectile>(
                "hegrenade_projectile");
        }
        if (projectile == null || !projectile.IsValid)
        {
            return false;
        }

        var start = detonateAtFeet
            ? new Vector(origin.X, origin.Y, origin.Z + 8f)
            : new Vector(
                origin.X + forward.X * 36f,
                origin.Y + forward.Y * 36f,
                origin.Z + 52f);
        var velocity = detonateAtFeet
            ? new Vector()
            : new Vector(
                forward.X * 680f,
                forward.Y * 680f,
                forward.Z * 680f + 100f);
        projectile.Teleport(start, pawn.EyeAngles, velocity);
        projectile.DispatchSpawn();
        projectile.IsLive = true;
        projectile.IsSmokeGrenade = false;
        projectile.DetonateTime = Server.CurrentTime + Math.Max(0.05f, fuseSeconds);
        projectile.Thrower.Raw = pawn.EntityHandle.Raw;
        projectile.OriginalThrower.Raw = pawn.EntityHandle.Raw;
        projectile.OwnerEntity.Raw = pawn.EntityHandle.Raw;
        if (kind == CookedGrenadeKind.HeGrenade)
        {
            projectile.Damage = damage;
            projectile.DmgRadius = radius;
        }
        if (projectile is CFlashbangProjectile flashbang)
        {
            flashbang.TimeToDetonate = Math.Max(0.05f, fuseSeconds);
        }

        projectile.AddEntityIOEvent(
            "Detonate",
            player,
            projectile,
            string.Empty,
            Math.Max(0.05f, fuseSeconds));
        return true;
    }

    private bool UseScan(
        CCSPlayerController player,
        float range,
        int? maxTargets,
        string skillName,
        bool revealToTeam)
    {
        var targets = GetEnemies(player)
            .Where(enemy => Distance(player, enemy) <= range)
            .OrderBy(enemy => Distance(player, enemy))
            .Take(maxTargets ?? int.MaxValue)
            .ToList();

        if (targets.Count == 0)
        {
            player.PrintToChat($" [{skillName}] 扫描范围内没有敌人。");
            return false;
        }

        var report = string.Join(" | ", targets.Select(enemy =>
            $"{enemy.PlayerName} {DistanceMeters(player, enemy):0}m"));
        player.PrintToCenter($"{skillName}: 共 {targets.Count} 名敌人");
        player.PrintToChat($" [{skillName}] {report}");
        if (GetExistingState(player)?.Operator == OperatorId.Hackclaw)
        {
            foreach (var target in targets)
            {
                ShowOutline(target, Config.HackclawMarkSeconds);
                target.PrintToCenter("你已被骇爪扫描标记");
                target.PrintToChat(" [骇爪] 你已被扫描并标记。");
            }
        }

        if (revealToTeam)
        {
            foreach (var teammate in Utilities.GetPlayers().ToList())
            {
                if (IsHumanInController(teammate) && IsAlly(player, teammate) &&
                    teammate.Handle != player.Handle)
                {
                    teammate.PrintToCenter($"{skillName}: 共 {targets.Count} 名敌人");
                    teammate.PrintToChat($" [{skillName}] {report}");
                }
            }
        }

        return true;
    }

    private bool UseVyronSuppression(CCSPlayerController player)
    {
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return false;
        }

        var forward = new Vector();
        NativeAPI.AngleVectors(pawn.EyeAngles.Handle, forward.Handle, IntPtr.Zero, IntPtr.Zero);
        var affected = 0;

        foreach (var enemy in GetEnemies(player))
        {
            if (Distance(player, enemy) > Config.VyronRadius)
            {
                continue;
            }

            var enemyPawn = enemy.PlayerPawn.Value;
            if (enemyPawn?.AbsOrigin == null || pawn.AbsOrigin == null)
            {
                continue;
            }

            var dx = enemyPawn.AbsOrigin.X - pawn.AbsOrigin.X;
            var dy = enemyPawn.AbsOrigin.Y - pawn.AbsOrigin.Y;
            var length = MathF.Sqrt(dx * dx + dy * dy);
            if (length < 1f)
            {
                continue;
            }

            var dot = (dx / length) * forward.X + (dy / length) * forward.Y;
            if (dot < 0.15f)
            {
                continue;
            }

            ApplyTemporarySpeed(enemy, Config.VyronSlowFactor, Config.VyronSlowSeconds);
            SendShake(enemy, 1.2f, 9f, 14f);
            enemy.PrintToCenter("被虎蹲炮压制");
            affected++;
        }

        if (affected == 0)
        {
            player.PrintToChat(" [威龙] 前方范围内没有可压制目标。");
            return false;
        }

        player.PrintToChat($" [威龙] 虎蹲炮压制了 {affected} 名敌人。");
        return true;
    }

    private bool UseShepherdSuppression(CCSPlayerController player)
    {
        var affected = 0;
        foreach (var enemy in GetEnemies(player))
        {
            if (Distance(player, enemy) > Config.ShepherdRadius)
            {
                continue;
            }

            ApplyTemporarySpeed(enemy, Config.ShepherdSlowFactor, Config.ShepherdSlowSeconds);
            SendShake(enemy, 2f, 6f, 12f);
            enemy.PrintToCenter("声波震慑：移动受压制");
            affected++;
        }

        if (affected == 0)
        {
            player.PrintToChat(" [牧羊人] 声波范围内没有敌人。");
            return false;
        }

        player.PrintToChat($" [牧羊人] 声波无人机压制了 {affected} 名敌人。");
        return true;
    }

    private void ApplyPassive(CCSPlayerController player, OperatorId operatorId)
    {
        var pawn = player.PlayerPawn.Value;
        if (!IsHumanController(player) || !player.PawnIsAlive ||
            pawn == null || !pawn.IsValid)
        {
            return;
        }

        var definition = GetOperator(operatorId);
        var state = GetExistingState(player);
        pawn.MaxHealth = _raidMode ? 200 : 100 + definition.BonusHealth;
        pawn.Health = pawn.MaxHealth;
        pawn.VelocityModifier = definition.BaseSpeed;
        if (state != null)
        {
            state.PassiveArmorAwarded = 0;
        }
        if (definition.BonusArmor > 0)
        {
            var armorBefore = pawn.ArmorValue;
            pawn.ArmorValue = Math.Max(pawn.ArmorValue, definition.BonusArmor);
            if (state != null)
            {
                state.PassiveArmorAwarded = pawn.ArmorValue - armorBefore;
            }
        }

        if (operatorId == OperatorId.Luna && state != null &&
            state.LastLunaFlashRound != _roundNumber)
        {
            var flashbang = GiveNamedItemToHand(player, "weapon_flashbang");
            if (flashbang != null)
            {
                state.GrantedLunaFlashHandle = flashbang.EntityHandle.Raw;
                state.LastLunaFlashRound = _roundNumber;
                player.PrintToChat(
                    $" [露娜] 获得特殊闪光弹：被闪中的敌人将显示 " +
                    $"{Config.LunaOutlineSeconds:0.#} 秒轮廓。");
                Logger.LogInformation("Player '{Player}' received Luna special flashbang for round {Round}.",
                    player.PlayerName, _roundNumber);
            }
        }

        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_ArmorValue");
    }

    private static void RemoveGrantedLunaFlash(
        CCSPlayerController player,
        PlayerState state)
    {
        if (state.GrantedLunaFlashHandle is not { } rawHandle)
        {
            return;
        }

        var weapon = new CHandle<CBasePlayerWeapon>(rawHandle).Value;
        if (weapon != null && weapon.IsValid)
        {
            weapon.Remove();
        }

        state.GrantedLunaFlashHandle = null;
    }

    private static void RemovePassiveArmor(
        CCSPlayerController player,
        PlayerState state)
    {
        var pawn = player.PlayerPawn.Value;
        if (state.PassiveArmorAwarded <= 0 ||
            pawn == null || !pawn.IsValid)
        {
            state.PassiveArmorAwarded = 0;
            return;
        }

        pawn.ArmorValue = Math.Max(
            0,
            pawn.ArmorValue - state.PassiveArmorAwarded);
        state.PassiveArmorAwarded = 0;
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_ArmorValue");
    }

    private void ApplyTemporarySpeed(CCSPlayerController player, float activeMultiplier, float duration)
    {
        var pawn = player.PlayerPawn.Value;
        var state = GetExistingState(player);
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        var key = GetPlayerKey(player);
        ClearTemporarySpeed(key, applyBaseSpeed: false);
        var baseSpeed = state?.Operator is { } operatorId
            ? GetOperator(operatorId).BaseSpeed
            : 1.0f;
        pawn.VelocityModifier = baseSpeed * activeMultiplier;
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");

        CssTimer? timer = null;
        timer = AddTimer(duration, () =>
        {
            if (_temporarySpeedTimers.TryGetValue(key, out var existing) && existing == timer)
            {
                ClearTemporarySpeed(key, applyBaseSpeed: true);
            }
        }, TimerFlags.STOP_ON_MAPCHANGE);
        _temporarySpeedTimers[key] = timer;
    }

    private static bool ApplyDirectionalBoost(
        CCSPlayerController player,
        float horizontal,
        float vertical)
    {
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid || pawn.MovementServices == null)
        {
            return false;
        }

        var forward = new Vector();
        var right = new Vector();
        NativeAPI.AngleVectors(
            pawn.EyeAngles.Handle,
            forward.Handle,
            right.Handle,
            IntPtr.Zero);

        var buttons = player.Buttons;
        var forwardInput =
            (buttons & PlayerButtons.Forward) != 0 ? 1f : 0f;
        forwardInput -=
            (buttons & PlayerButtons.Back) != 0 ? 1f : 0f;
        var rightInput =
            (buttons & PlayerButtons.Moveright) != 0 ? 1f : 0f;
        rightInput -=
            (buttons & PlayerButtons.Moveleft) != 0 ? 1f : 0f;

        var directionX = forward.X * forwardInput + right.X * rightInput;
        var directionY = forward.Y * forwardInput + right.Y * rightInput;
        var length = MathF.Sqrt(directionX * directionX + directionY * directionY);
        if (length < 0.001f)
        {
            directionX = forward.X;
            directionY = forward.Y;
            length = MathF.Sqrt(directionX * directionX + directionY * directionY);
            if (length < 0.001f)
            {
                return false;
            }
        }

        directionX /= length;
        directionY /= length;
        pawn.AbsVelocity.X = directionX * horizontal;
        pawn.AbsVelocity.Y = directionY * horizontal;
        pawn.AbsVelocity.Z = Math.Max(pawn.AbsVelocity.Z, vertical);
        return true;
    }

    private void ClearTemporarySpeed(string key, bool applyBaseSpeed)
    {
        if (_temporarySpeedTimers.Remove(key, out var timer))
        {
            timer.Kill();
        }

        if (!applyBaseSpeed)
        {
            return;
        }

        var player = FindPlayerByKey(key);
        var state = _states.GetValueOrDefault(key);
        var pawn = player?.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        var baseSpeed = state?.Operator is { } operatorId
            ? GetOperator(operatorId).BaseSpeed
            : 1.0f;
        pawn.VelocityModifier = baseSpeed;
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
    }

    private void ClearTemporarySpeedEffects()
    {
        foreach (var key in _temporarySpeedTimers.Keys.ToList())
        {
            ClearTemporarySpeed(key, applyBaseSpeed: true);
        }
    }

    private int HealPlayer(CCSPlayerController player, int amount)
    {
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid || !player.PawnIsAlive)
        {
            return 0;
        }

        var before = pawn.Health;
        pawn.Health = Math.Min(pawn.MaxHealth, before + amount);
        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
        return Math.Max(0, pawn.Health - before);
    }

    private void DealDamage(
        CCSPlayerController attacker,
        CCSPlayerController victim,
        int damage)
    {
        if (!IsEnemy(attacker, victim)) return;
        var attackerPawn = attacker.PlayerPawn.Value;
        var victimPawn = victim.PlayerPawn.Value;
        if (attackerPawn == null || victimPawn == null ||
            !attackerPawn.IsValid || !victimPawn.IsValid ||
            !attacker.PawnIsAlive || !victim.PawnIsAlive)
        {
            return;
        }

        var damageInfoSize = Schema.GetClassSize("CTakeDamageInfo");
        var damageInfoPtr = Marshal.AllocHGlobal(damageInfoSize);
        var damageResultSize = Schema.GetClassSize("CTakeDamageResult");
        var damageResultPtr = Marshal.AllocHGlobal(damageResultSize);
        try
        {
            for (var i = 0; i < damageInfoSize; i++)
            {
                Marshal.WriteByte(damageInfoPtr, i, 0);
            }
            for (var i = 0; i < damageResultSize; i++)
            {
                Marshal.WriteByte(damageResultPtr, i, 0);
            }

            var damageInfo = new CTakeDamageInfo(damageInfoPtr);
            var attackerInfo = new AttackerInfo_t
            {
                AttackerPawn = attacker.Pawn.Raw,
                AttackerPlayerSlot = attacker.Slot,
                IsPawn = true,
                NeedInit = true
            };
            Marshal.StructureToPtr(
                attackerInfo,
                new IntPtr(damageInfoPtr.ToInt64() + 0x88),
                false);

            Schema.SetSchemaValue(
                damageInfo.Handle,
                "CTakeDamageInfo",
                "m_hInflictor",
                attackerPawn.EntityHandle.Raw);
            Schema.SetSchemaValue(
                damageInfo.Handle,
                "CTakeDamageInfo",
                "m_hAttacker",
                attackerPawn.EntityHandle.Raw);

            damageInfo.Damage = damage;
            damageInfo.OriginalDamage = damage;
            damageInfo.TotalledDamage = damage;
            damageInfo.BitsDamageType = DamageTypes_t.DMG_ENERGYBEAM;
            damageInfo.DamageFlags = TakeDamageFlags_t.DFLAG_IGNORE_ARMOR;

            var damageResult = new CTakeDamageResult(damageResultPtr);
            Schema.SetSchemaValue(
                damageResult.Handle,
                "CTakeDamageResult",
                "m_pOriginatingInfo",
                damageInfo.Handle);
            damageResult.HealthBefore = victimPawn.Health;
            damageResult.HealthLost = damage;
            damageResult.DamageDealt = damage;
            damageResult.PreModifiedDamage = damage;
            damageResult.TotalledHealthLost = damage;
            damageResult.TotalledDamageDealt = damage;
            damageResult.WasDamageSuppressed = false;

#pragma warning disable CS0618
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Invoke(
                victimPawn,
                damageInfo,
                damageResult);
#pragma warning restore CS0618
        }
        finally
        {
            Marshal.FreeHGlobal(damageInfoPtr);
            Marshal.FreeHGlobal(damageResultPtr);
        }
    }

    private void ShowOutline(CCSPlayerController player, float? duration = null)
    {
        if (_raidMode) return; // Legacy global glow leaks enemy positions to other squads.
        if (!IsCombatController(player))
        {
            return;
        }

        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        var key = GetPlayerKey(player);
        ClearOutline(key);
        var outlineSeconds = duration ?? Config.LunaOutlineSeconds;

        pawn.Glow.GlowType = 3;
        pawn.Glow.GlowRange = 5000;
        pawn.Glow.GlowRangeMin = 0;
        pawn.Glow.GlowColorOverride = Color.FromArgb(255, 255, 40, 40);
        pawn.Glow.Flashing = false;
        pawn.Glow.GlowTime = outlineSeconds;
        pawn.Glow.GlowStartTime = Server.CurrentTime;
        pawn.Glow.EligibleForScreenHighlight = true;
        pawn.Glow.Glowing = true;
        Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_Glow");

        _outlineTimers[key] = AddTimer(outlineSeconds, () => ClearOutline(key),
            TimerFlags.STOP_ON_MAPCHANGE);
    }

    private void ClearOutline(string key)
    {
        if (_outlineTimers.Remove(key, out var timer))
        {
            timer.Kill();
        }

        var player = FindPlayerByKey(key);
        var pawn = player?.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid)
        {
            return;
        }

        pawn.Glow.GlowType = 0;
        pawn.Glow.GlowRange = 0;
        pawn.Glow.GlowRangeMin = 0;
        pawn.Glow.GlowColorOverride = Color.Transparent;
        pawn.Glow.Flashing = false;
        pawn.Glow.EligibleForScreenHighlight = false;
        pawn.Glow.Glowing = false;
        Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_Glow");
    }

    private void ClearAllOutlines()
    {
        foreach (var key in _outlineTimers.Keys.ToList())
        {
            ClearOutline(key);
        }
    }

    private static void SendShake(CCSPlayerController player, float duration, float amplitude, float frequency)
    {
        if (!IsHumanController(player))
        {
            return;
        }

        using var message = UserMessage.FromPartialName("Shake");
        message.SetFloat("duration", duration);
        message.SetFloat("amplitude", amplitude);
        message.SetFloat("frequency", frequency);
        message.SetInt("command", 0);
        message.Send(player);
    }

    private void StartSkillCooldown(
        CCSPlayerController player,
        PlayerState state,
        bool useV)
    {
        var cooldownSeconds = useV ? Config.VCooldownSeconds : Config.XCooldownSeconds;
        var cooldownUntil = Environment.TickCount64 + cooldownSeconds * 1000L;
        var key = GetPlayerKey(player);
        var priorTimer = useV ? state.VReadyTimer : state.XReadyTimer;
        priorTimer?.Kill();

        if (useV)
        {
            state.VCooldownUntilMs = cooldownUntil;
        }
        else
        {
            state.XCooldownUntilMs = cooldownUntil;
        }

        var timer = AddTimer(cooldownSeconds + 0.05f, () =>
        {
            if (!_states.TryGetValue(key, out var currentState) ||
                currentState.Operator == null)
            {
                return;
            }

            var activeUntil = useV
                ? currentState.VCooldownUntilMs
                : currentState.XCooldownUntilMs;
            if (activeUntil != cooldownUntil)
            {
                return;
            }

            if (useV)
            {
                currentState.VReadyTimer = null;
            }
            else
            {
                currentState.XReadyTimer = null;
            }

            var currentPlayer = FindPlayerByKey(key);
            if (currentPlayer == null)
            {
                return;
            }

            var definition = GetOperator(currentState.Operator.Value);
            var slot = useV ? "V" : "X";
            var skillName = useV ? definition.VName : definition.XName;
            currentPlayer.PrintToCenterAlert($"{slot} 技能已就绪：{skillName}");
        }, TimerFlags.STOP_ON_MAPCHANGE);

        if (useV)
        {
            state.VReadyTimer = timer;
        }
        else
        {
            state.XReadyTimer = timer;
        }
    }

    private void ResetSkillCooldowns(PlayerState state)
    {
        state.VReadyTimer?.Kill();
        state.VReadyTimer = null;
        state.XReadyTimer?.Kill();
        state.XReadyTimer = null;
        state.VCooldownUntilMs = 0;
        state.XCooldownUntilMs = 0;
    }

    private static int GetCooldownRemainingSeconds(PlayerState state, bool useV)
    {
        var cooldownUntil = useV ? state.VCooldownUntilMs : state.XCooldownUntilMs;
        var remainingMs = cooldownUntil - Environment.TickCount64;
        return remainingMs > 0 ? (int)Math.Ceiling(remainingMs / 1000.0) : 0;
    }

    private string GetCooldownText(PlayerState state, bool useV)
    {
        var remaining = GetCooldownRemainingSeconds(state, useV);
        if (remaining > 0)
        {
            return $"{remaining} 秒";
        }

        var configured = useV ? Config.VCooldownSeconds : Config.XCooldownSeconds;
        return $"就绪（CD {configured} 秒）";
    }

    private void PrintCooldownStatus(CCSPlayerController player, PlayerState state)
    {
        player.PrintToChat(
            $" [冷却] V {GetCooldownText(state, useV: true)} | " +
            $"X {GetCooldownText(state, useV: false)}。");
    }

    private void ResetAllPlayersAndState(string reason, bool broadcast)
    {
        ClearAllOutlines();
        ClearAllLunaDots();
        ClearAllArmedGrenades();
        ClearAllLandmines();
        ClearAllControlTimers();
        foreach (var key in _temporarySpeedTimers.Keys.ToList())
        {
            ClearTemporarySpeed(key, applyBaseSpeed: false);
        }

        foreach (var state in _states.Values)
        {
            state.SelectionTimer?.Kill();
            state.SelectionTimer = null;
            ResetSkillCooldowns(state);
        }

        foreach (var player in Utilities.GetPlayers().ToList())
        {
            if (!IsHumanController(player))
            {
                continue;
            }

            var key = GetPlayerKey(player);
            _states.TryGetValue(key, out var state);
            if (state != null)
            {
                RemoveGrantedLunaFlash(player, state);
                RemovePassiveArmor(player, state);
            }

            var pawn = player.PlayerPawn.Value;
            if (pawn != null && pawn.IsValid)
            {
                pawn.MaxHealth = _raidMode ? 200 : 100;
                pawn.Health = Math.Min(pawn.Health, pawn.MaxHealth);
                pawn.VelocityModifier = 1.0f;
                Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
                Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
                Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
            }

            if (broadcast)
            {
                player.PrintToChat($" [三角洲模式] {reason}，干员和技能状态已清空。");
            }
        }

        _states.Clear();
    }

    private PlayerState GetOrCreateState(CCSPlayerController player, string key)
    {
        var state = GetOrCreateState(key);
        state.LastKnownName = player.PlayerName;
        return state;
    }

    private PlayerState GetOrCreateState(string key)
    {
        if (!_states.TryGetValue(key, out var state))
        {
            state = new PlayerState();
            _states[key] = state;
        }

        return state;
    }

    private PlayerState? GetExistingState(CCSPlayerController player)
    {
        return IsHumanController(player) ? _states.GetValueOrDefault(GetPlayerKey(player)) : null;
    }

    private IEnumerable<CCSPlayerController> GetEnemies(CCSPlayerController player)
    {
        return Utilities.GetPlayers().Where(candidate =>
            IsCombatController(candidate) &&
            candidate.PawnIsAlive &&
            IsEnemy(player, candidate) &&
            candidate.TeamNum is 2 or 3);
    }

    private CCSPlayerController? FindPlayerByKey(string key)
    {
        return Utilities.GetPlayers().FirstOrDefault(player =>
            IsCombatController(player) && GetPlayerKey(player) == key);
    }

    private static float Distance(CCSPlayerController first, CCSPlayerController second)
    {
        var a = first.PlayerPawn.Value?.AbsOrigin;
        var b = second.PlayerPawn.Value?.AbsOrigin;
        if (a == null || b == null)
        {
            return float.MaxValue;
        }

        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        var dz = a.Z - b.Z;
        return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    private static float DistanceMeters(CCSPlayerController first, CCSPlayerController second)
    {
        return Distance(first, second) / 39f;
    }

    private OperatorDefinition GetOperator(OperatorId id)
    {
        return _operators.First(definition => definition.Id == id);
    }

    private static bool IsWarmupPeriod()
    {
        var proxy = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules")
            .FirstOrDefault();
        return proxy?.GameRules?.WarmupPeriod ?? false;
    }

    private static string GetPlayerKey(CCSPlayerController player)
    {
        var steamId = player.AuthorizedSteamID;
        if (steamId != null && steamId.IsValid())
        {
            return $"steam:{steamId.SteamId64}";
        }

        return $"slot:{player.Slot}:{player.UserId?.ToString() ?? "unknown"}";
    }

    private static bool IsHumanController(CCSPlayerController? player)
    {
        return IsHumanInController(player) && !player!.IsBot;
    }

    private static bool IsCombatController(CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.TeamNum is 2 or 3;
    }

    private static bool IsHumanInController(CCSPlayerController? player)
    {
        return player != null && player.IsValid && !player.IsBot;
    }

    private static bool IsHumanInMatch(CCSPlayerController? player)
    {
        return IsHumanController(player) && player!.TeamNum is 2 or 3;
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct AttackerInfo_t
{
    public bool NeedInit;
    public bool IsPawn;
    public bool IsWorld;
    public uint AttackerPawn;
    public int AttackerPlayerSlot;
    public int TeamChecked;
    public int Team;
}
