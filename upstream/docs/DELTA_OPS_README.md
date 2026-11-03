# CS2 Delta Ops 模式 v1.3.0

已在本机 CS2 专用服务器实际加载并测试。

核心规则仍然是 CS：

- 标准购买、枪械、经济和炸弹/回合规则
- 开局选择干员
- 每名干员有一个常驻被动，以及独立的 V、X 两个主动技能
- 死亡、复活、换边和断线重连保留干员
- V、X 技能分别拥有独立冷却，默认 V 18 秒、X 24 秒
- 冷却只会在技能成功释放后开始，新回合统一重置
- 死亡、复活、换边和断线重连不会白送冷却
- 比赛结束或换图清空干员
- 高爆手雷携带上限为 3
- 技能发放的道具会直接切到手中，不会只留在背包
- 换角会撤销上一角色发放的闪光弹和额外护甲，避免被动串到新角色
- 不需要客户端 Mod，不使用三角洲模型或素材

## 当前六名干员

| 干员 | V 技能 | X 技能 | 被动 |
|---|---|---|---|
| 红狼 / D-Wolf | 4 秒速度 +35% | 向前跃迁并恢复 15 HP | 击杀敌人恢复 50 HP |
| 蜂医 / Stinger | 治疗附近队友 60 HP | 沿瞄准方向生成 5 段真实长烟 | 最大生命 +20 |
| 露娜 / Luna | 扫描 46 米敌人 | 对最近被露娜伤害的敌人造成 5×5 持续伤害并显形 5 秒 | 造成伤害或闪光命中时，全队显形目标 2.5 秒 |
| 威龙 / Vyron | 向前喷气推进 | 获得标准高爆手雷；爆炸命中敌人缴械并定身 4 秒 | 常驻速度 +6% |
| 骇爪 / Hackclaw | 扫描范围内敌人并标记，目标收到提示 | 两次 X 掐秒投掷闪光弹 | 常驻速度 +4% |
| 牧羊人 / Shepherd | 原地放置感应地雷，敌人进入半径自动爆炸 | 两次 X 掐秒投掷增强高爆手雷 | 出生至少 15 护甲 |

## 游戏内命令

聊天框：

```text
!op 1
!op 2
!op 3
!op 4
!op 5
!op 6
V
X
!v
!x
!bind
!cd
!ops
!switch
!switch 1-6
!roles
```

`!op` 选择的编号与上方表格顺序一致。

插件会在进入服务器、出生和完成选角后多次尝试下发键位绑定。部分 CS2
客户端仍会拦截服务器发起的 `bind`，此时只需在游戏控制台执行一次：

```text
bind "v" "css_v"
bind "x" "css_x"
host_writeconfig
```

之后 V 使用技能 1，X 使用技能 2。`!v`、`!x` 永远可作为聊天回退。

`!cd` 显示 V/X 当前剩余冷却；`!ops` 同时显示干员能力与冷却状态。

## 当前安装

- 插件：`C:\cs2server\game\csgo\addons\counterstrikesharp\plugins\DeltaOps\DeltaOps.dll`
- 配置：`C:\cs2server\game\csgo\addons\counterstrikesharp\configs\plugins\DeltaOps\DeltaOps.json`
- 原卡牌插件已保留在 `plugins\disabled\CardBuffs`
- 服务端地址和密码沿用：

```text
192.168.1.118:27015
CB-147e0dc886f989a9c0
```

启动：

```cmd
powershell.exe -NoProfile -ExecutionPolicy Bypass -File C:\cs2server\start-deltaops.ps1
```

停止：

```cmd
powershell.exe -NoProfile -ExecutionPolicy Bypass -File C:\cs2server\stop-deltaops.ps1
```

## 实测结果

- DeltaOps 插件零警告构建
- `DeltaOps loaded. Operators=6, cooldowns=V:18s/X:24s, autoBind=true`
- 服务器已加载 `Delta Ops v1.3.0`
- 服务器已实际重启并加载 `Delta Ops v1.3.0`
- 中心选角菜单已在客户端实机显示
- 选角默认延长到 120 秒，加入 CT/T 后立即显示菜单
- 最后 30、15、10、5 秒会发送选择提醒
- 每个角色在中心菜单使用单行简表，完整技能说明由 `!roles` 查看，避免小屏幕截断
- `!op 1` 手动选择红狼成功
- `!v` 使用红狼爆发外骨骼成功
- `!x` 使用红狼动力跃迁成功
- 超时自动选角成功
- `!op 2` 选择蜂医成功，生命上限显示 120
- 蜂医主动治疗成功
- `!op 3` 选择露娜成功
- 露娜扫描机器人成功，报告 `Squad 36m`
- 露娜伤害与闪光都会触发表情目标的 2.5 秒轮廓
- 服务器诊断命令成功为机器人应用红色轮廓并按计时清理
- 独立冷却数学、过期恢复和新回合清零测试通过
- `css_cd`、`css_v`、`css_x` 命令已确认注册
- 实机触发骇爪 X 后进入冷却，1 秒内再次触发被拒绝并记录 `remaining=23s`
- 一键启动参数成功写入 V/X bind，实机按 X 释放蜂医技能
- 再次按 X 被冷却拒绝并记录 `remaining=17s`
- 蜂医长烟测试实际创建 5 段 CS2 烟雾投射物
- 露娜持续伤害实测完成 5 次日志，每次 5 点伤害

一键启动脚本使用 Steam 的 `-console -condebug +bind` 启动参数，在客户端启动
阶段直接绑定 V/X，已通过实机按键验证。服务器运行中仍不能强制下发 `bind`，
CS2 会以 `missing required FCVAR flag` 拒绝该路径；从 Steam 菜单普通启动或
朋友首次加入时，仍需执行上面的本机控制台命令。聊天回退为 `!v`、`!x`。

## 下次启动

双击：

```text
C:\cs2server\play-deltaops.bat
```

该脚本会先启动 Steam/CS2 客户端，再启动专用服务器，最后打开连接地址。完整
步骤和故障处理见 `PLAY_MANUAL.md`。

威龙和牧羊人的范围压制已编译并接入相同速度/震屏路径，但尚未用第二名真人完成
完整视觉与命中范围验收。

## 研究来源

官网干员页：
<https://www.playdeltaforce.com/zh-tw/universe/>

当前官网干员列表为：牧羊人、蜂医、红狼、露娜、威龙、骇爪、乌鲁鲁、蛊、深蓝、无名。
首版选择其中六名机制容易在 CS2 服务端稳定复现的干员。乌鲁鲁、蛊、深蓝和无名
暂未加入，后续可扩展。
