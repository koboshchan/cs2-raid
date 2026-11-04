using System.Text.RegularExpressions;

// Deliberately source-contract checks, not native CS2 gameplay tests.
var source = File.ReadAllText(args[0]);
var checks = 0;
void Check(bool condition, string label)
{
    if (!condition) throw new InvalidOperationException(label);
    ++checks;
    Console.WriteLine("PASS " + label);
}
string Section(string start, string end) =>
    source[source.IndexOf(start, StringComparison.Ordinal)..source.IndexOf(end, source.IndexOf(start, StringComparison.Ordinal), StringComparison.Ordinal)];
Check(source.Contains("[ConsoleCommand(\"css_characterselect\""), "characterselect registered");
Check(source.Contains("[ConsoleCommand(\"css_op\""), "legacy op alias retained");
var select = Section("public void OnOperatorCommand(", "private void ResetOperatorForSwitch(");
Check(select.Contains("selectedIndex < 1 || selectedIndex > _operators.Count"), "selection range validated");
Check(select.Contains("!int.TryParse("), "invalid numeric selection rejected");
Check(select.Contains("state.Operator.HasValue && !IsWarmupPeriod()"), "live selection lock retained");
Check(select.Contains("!IsHumanController(player)"), "player command guard retained");
Check(select.Contains("_operators[selectedIndex - 1]"), "one-based selection maps to roster");
var roster = Section("private readonly List<OperatorDefinition>", "private readonly Dictionary<string, PlayerState>");
var names = new[] { "D-Wolf", "Stinger", "Luna", "Vyron", "Hackclaw", "Shepherd" };
Check(Regex.Matches(roster, @"new\(").Count == 6, "exactly six roster entries");
var last = -1;
foreach (var name in names)
{
    var index = roster.IndexOf("\"" + name + "\"", StringComparison.Ordinal);
    Check(index > last, "numbered roster order " + name);
    last = index;
}
Check(!source.Contains("CenterHtmlMenu") && !source.Contains("MenuManager"), "display box removed");
Check(!Regex.IsMatch(source, @"player\.ExecuteClientCommand(?:FromServer)?\(\s*""bind"), "no remote bind calls");
Check(!source.Contains("ScheduleSkillKeyBinding"), "retry binding timers removed");
var help = Section("private void ShowSkillBindingHelp(", "private void OnClientDisconnect(");
Check(help.Contains("state.BindingHelpShown") && help.Contains("bool force = false"), "one-time help and force option");
Check(help.Contains("!v / !x") && help.Contains("css_v") && help.Contains("css_x"), "usable chat and local binding instructions");
Check(source.Contains("ShowSkillBindingHelp(player, force: true)"), "bind command always shows help");
var tick = Section("private void TickSelectionTimeout(", "private void PrintRoleList(");
Check(!tick.Contains("PrintRoleList("), "timeout does not spam full roster each second");
Check(tick.Contains("30 or 15 or 10 or 5"), "bounded countdown warnings retained");
Check(tick.Contains("Random.Shared.Next(_operators.Count)"), "timeout retains six-character random fallback");
Check(tick.Contains("state.SelectionTimer?.Kill()"), "timeout cancels timer");
var list = Section("private void PrintRoleList(", "private void SelectOperator(");
Check(list.Contains("i < _operators.Count") && list.Contains("{i + 1}."), "chat list prints numbered full roster");
Check(list.Contains("role.EnglishName"), "chat list includes English character names");
foreach (var slot in new[] { "V", "X" })
{
    var skill = Section("private bool TryUseOperator" + slot + "Skill(", slot == "V" ? "private bool TryUseOperatorXSkill(" : "private bool UseStingerHeal(");
    Check(Regex.Matches(skill, "case OperatorId.").Count == 6, "all six " + slot + " skill routes retained");
}
Check(source.Contains("DeltaOps disabled in Raid gameplay mode"), "unsafe raid integration stays disabled");
Console.WriteLine($"{checks} DeltaOps source-contract checks passed (not gameplay QA).");
