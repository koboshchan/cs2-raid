namespace Cs2Raid;

public enum RaidPhase { Shop, Active, Closed }
public sealed record ShopItem(string Id, string Entity, int PriceDollars, string Kind)
{
    public int PriceCents => checked(PriceDollars * 100);
    public bool IsGun => Kind is "pistol" or "smg" or "shotgun" or "rifle" or "lmg" or "sniper";
}
public sealed record LootGun(Guid Instance, string ItemId, string OriginalOwner, int PurchaseCents);
public sealed class Raider(string steamId)
{
    public string SteamId { get; } = steamId;
    public bool Alive { get; internal set; } = true;
    public bool Extracted { get; internal set; }
    // Logical backpack, not an engine inventory with a weapon-slot cap.
    public List<LootGun> Backpack { get; } = [];
}
public sealed record Extraction(string SteamId, IReadOnlyList<LootGun> Kept, IReadOnlyList<LootGun> Sold, long SellbackCents);

// Engine-independent rule skeleton. Only trusted server adapters may call inventory/lever methods.
// No credits or delivery are inferred from this model. Persist receipts before wiring settlement.
public sealed class RaidState
{
    public const int MaxPlayers = 7;
    public const int RaidSeconds = 30 * 60;
    public const int ExtractionDelaySeconds = 5 * 60;
    public RaidPhase Phase { get; private set; } = RaidPhase.Shop;
    public DateTimeOffset? EndsAt { get; private set; }
    public DateTimeOffset? ExtractionAt { get; private set; }
    public Dictionary<string, Raider> Players { get; } = [];
    public Dictionary<string, List<LootGun>> Corpses { get; } = [];
    private readonly HashSet<int> levers = [];
    public Raider Join(string id)
    {
        if (Phase != RaidPhase.Shop) throw new InvalidOperationException("Roster locked during raid");
        if (Players.ContainsKey(id)) return Players[id];
        if (Players.Count >= MaxPlayers) throw new InvalidOperationException("Raid full (7 players)");
        return Players[id] = new Raider(id);
    }
    public void Start(DateTimeOffset now)
    {
        if (Phase != RaidPhase.Shop || Players.Count == 0) throw new InvalidOperationException("No ready roster");
        Phase = RaidPhase.Active; EndsAt = now.AddSeconds(RaidSeconds);
    }
    public void Tick(DateTimeOffset now)
    {
        if (Phase != RaidPhase.Active || now < EndsAt!.Value) return;
        foreach (var p in Players.Values.Where(p => !p.Extracted)) { p.Alive = false; p.Backpack.Clear(); }
        Corpses.Clear(); Phase = RaidPhase.Closed;
    }
    public bool ExtractionOpen(DateTimeOffset now) => Phase == RaidPhase.Active && now < EndsAt!.Value && ExtractionAt is { } at && now >= at;
    public void PullLever(int lever, DateTimeOffset now)
    {
        Tick(now);
        if (Phase != RaidPhase.Active || lever is < 1 or > 2) throw new InvalidOperationException("Invalid lever/phase");
        levers.Add(lever);
        if (levers.Count == 2 && ExtractionAt == null) ExtractionAt = now.AddSeconds(ExtractionDelaySeconds);
    }
    public void RecordPurchasedGun(string id, ShopItem item)
    {
        if (Phase != RaidPhase.Shop || !item.IsGun || item.PriceDollars <= 0) throw new InvalidOperationException("Pre-raid guns only");
        Alive(id).Backpack.Add(new LootGun(Guid.NewGuid(), item.Id, id, item.PriceCents));
    }
    private Raider Alive(string id)
    {
        if (!Players.TryGetValue(id, out var p) || !p.Alive || p.Extracted) throw new InvalidOperationException("Player not eligible");
        return p;
    }
    public void Die(string id)
    {
        var p = Alive(id);
        if (Phase != RaidPhase.Active) return;
        Corpses[id] = [..p.Backpack]; p.Backpack.Clear(); p.Alive = false;
        // Native adapter must remove armor, grenades and engine weapons. No kill bonus.
    }
    public void Loot(string looterId, string corpseId)
    {
        if (Phase != RaidPhase.Active) throw new InvalidOperationException("No active raid");
        var looter = Alive(looterId);
        if (!Corpses.Remove(corpseId, out var guns)) throw new InvalidOperationException("No corpse loot");
        looter.Backpack.AddRange(guns); // Removal is atomic on the game thread, no duplicate loot.
    }
    public Extraction Extract(string id, DateTimeOffset now)
    {
        Tick(now);
        if (!ExtractionOpen(now)) throw new InvalidOperationException("Extraction not open");
        var p = Alive(id);
        var kept = p.Backpack.Where(g => g.OriginalOwner == id).ToArray();
        var sold = p.Backpack.Where(g => g.OriginalOwner != id).ToArray();
        // Guns are sold once; the extraction receipt records them, not reusable inventory.
        long cents = sold.Sum(g => (long)g.PurchaseCents * 90 / 100);
        p.Backpack.Clear(); p.Backpack.AddRange(kept); p.Extracted = true;
        return new Extraction(id, kept, sold, cents);
    }
}
