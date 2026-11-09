namespace Cs2Raid.Abilities;

public interface IAbilityClock { TimeSpan Elapsed { get; } }
public readonly record struct AbilityActor(ulong PlayerId, uint PawnHandle, long Generation);
public interface IExactAbilityAdapter
{
    // Game-server-thread only. Implementations must be transactional: on throw/false they
    // must leave no side effects. No native adapter is registered by this scaffold.
    bool TryActivate(AbilityActor actor, AbilityDefinition skill, out IDisposable? ownedEffect);
}
public sealed class ExactAbilityAdapters
{
    private readonly Dictionary<string, IExactAbilityAdapter> adapters = new(StringComparer.Ordinal);
    public void Register(string key, IExactAbilityAdapter adapter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key); ArgumentNullException.ThrowIfNull(adapter);
        if (!adapters.TryAdd(key, adapter)) throw new InvalidOperationException("Duplicate adapter key");
    }
    internal bool Find(string key, out IExactAbilityAdapter adapter) => adapters.TryGetValue(key, out adapter!);
}

// Intentionally disconnected from RaidPlugin.Load, native callbacks, capabilities, keys and money.
// Single-threaded state machine; caller owns admission, lifecycle delivery and monotonic clock.
public sealed class ExactAbilityRuntime
{
    private sealed class SkillState { public int? Remaining; public TimeSpan Ready; public TimeSpan End; public IDisposable? Effect; public bool Active; public bool Disposing; }
    private sealed class PlayerState
    {
        public required AbilityActor Actor; public required string Operator;
        public bool Quarantined; public bool Busy;
        public readonly Dictionary<string, SkillState> Skills = new(StringComparer.Ordinal);
    }
    private readonly ExactAbilityCatalog catalog;
    private readonly ExactAbilityAdapters adapters;
    private readonly IAbilityClock clock;
    private readonly Func<AbilityActor, bool> admitted;
    private readonly Dictionary<ulong, PlayerState> players = [];
    private readonly HashSet<ulong> spentSelections = []; // Tombstones prevent death/reconnect resource minting.
    private TimeSpan last;
    private long generation = 1;
    public long Generation => generation;
    public ExactAbilityRuntime(ExactAbilityCatalog catalog, ExactAbilityAdapters adapters, IAbilityClock clock, Func<AbilityActor, bool> admitted)
    {
        this.catalog = catalog; this.adapters = adapters; this.clock = clock; this.admitted = admitted;
        last = clock.Elapsed;
        if (last < TimeSpan.Zero) throw new ArgumentException("Negative clock");
    }
    public bool Select(AbilityActor actor, string operatorId)
    {
        if (!Fence(actor) || !catalog.HasOperator(operatorId) || spentSelections.Contains(actor.PlayerId) || !Allowed(actor)) return false;
        var state = new PlayerState { Actor = actor, Operator = operatorId };
        foreach (var id in catalog.Operator(operatorId).SkillIds) {
            var definition = catalog.Skill(id);
            state.Skills.Add(id, new() { Remaining = definition.ResourceMode.Value == "finite" &&
                definition.Charges.Value is >= 1 and <= int.MaxValue && decimal.Truncate(definition.Charges.Value.Value) == definition.Charges.Value
                ? (int)definition.Charges.Value.Value : null });
        }
        spentSelections.Add(actor.PlayerId); players.Add(actor.PlayerId, state); return true;
    }
    public string? Activate(AbilityActor actor, string skillId)
    {
        if (!Fence(actor) || !players.TryGetValue(actor.PlayerId, out var player) || player.Actor != actor ||
            player.Quarantined || player.Busy || !Allowed(actor)) return "actor unavailable";
        if (!player.Skills.TryGetValue(skillId, out var state)) return "skill not selected";
        var blocker = catalog.ActivationBlocker(skillId); if (blocker != null) return blocker;
        if (!Now(out var now)) return "clock regressed";
        player.Busy = true;
        try {
        if (!Expire(player, state, now)) return "cleanup failed";
        if (player.Quarantined || !players.TryGetValue(actor.PlayerId, out var afterExpiry) || afterExpiry != player) return "actor revoked during expiry";
        if (state.Active || now < state.Ready || state.Remaining == 0) return "skill unavailable";
        var skill = catalog.Skill(skillId);
        if (!adapters.Find(skill.AdapterKey!, out var adapter)) return "adapter unavailable";
        TimeSpan ready, end;
        try { ready = now + TimeSpan.FromTicks(checked((long)(skill.Cooldown.Value!.Value * TimeSpan.TicksPerSecond))); end = now + TimeSpan.FromTicks(checked((long)(skill.Duration.Value!.Value * TimeSpan.TicksPerSecond))); }
        catch (OverflowException) { return "timing overflow"; }
        try {
            if (!adapter.TryActivate(actor, skill, out var effect)) {
                // A rejected adapter must not return ownership; clean it if it violates the contract.
                if (effect != null) { state.Effect = effect; player.Quarantined = true; Cleanup(player, state); }
                return "adapter rejected";
            }
            state.Effect = effect;
            if (!Fence(actor) || !players.TryGetValue(actor.PlayerId, out var current) || current != player || player.Quarantined || !Allowed(actor)) {
                Cleanup(player, state); return "actor revoked during activation";
            }
            state.Active = true; state.Ready = ready; state.End = end;
            if (state.Remaining != null) state.Remaining--;
            return null;
        } catch { player.Quarantined = true; Cleanup(player, state); return "adapter failed; actor quarantined"; }
        } finally { player.Busy = false; }
    }
    public void Tick()
    {
        if (!Now(out var now)) return;
        foreach (var player in players.Values.ToArray())
            foreach (var state in player.Skills.Values.ToArray()) Expire(player, state, now);
    }
    // All three invalidate the pawn session without allowing same-generation reselection.
    public void Death(AbilityActor actor) => Revoke(actor);
    public void Disconnect(AbilityActor actor) => Revoke(actor);
    public void Revoke(AbilityActor actor)
    {
        if (players.TryGetValue(actor.PlayerId, out var p) && p.Actor == actor) {
            p.Quarantined = true;
            foreach (var state in p.Skills.Values) Cleanup(p, state);
            // Retain failed cleanup ownership for retry, never discard it.
            if (p.Skills.Values.All(s => s.Effect == null) && !p.Busy) players.Remove(actor.PlayerId);
        }
    }
    public bool MapEnd()
    {
        foreach (var p in players.Values.ToArray()) Revoke(p.Actor);
        if (players.Count != 0) return false;
        checked { generation++; }
        spentSelections.Clear(); return true;
    }
    private bool Allowed(AbilityActor actor) { try { return admitted(actor); } catch { return false; } }
    private bool Fence(AbilityActor actor) => actor.Generation == generation && actor.PlayerId != 0 && actor.PawnHandle is not (0 or uint.MaxValue);
    private bool Now(out TimeSpan now)
    {
        now = clock.Elapsed;
        if (now < last) {
            foreach (var p in players.Values.ToArray()) Revoke(p.Actor);
            return false;
        }
        last = now; return true;
    }
    private static bool Expire(PlayerState p, SkillState s, TimeSpan now) => !s.Active || now < s.End || Cleanup(p, s);
    private static bool Cleanup(PlayerState p, SkillState s)
    {
        s.Active = false;
        if (s.Disposing) return false; // Reentrant cleanup never disposes the same handle twice.
        if (s.Effect == null) return true;
        s.Disposing = true;
        try { s.Effect.Dispose(); s.Effect = null; return true; }
        catch { p.Quarantined = true; return false; }
        finally { s.Disposing = false; }
    }
}
