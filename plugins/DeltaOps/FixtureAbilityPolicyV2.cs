namespace DeltaOps;

// Native-free server-thread bookkeeping. A policy reservation is NEVER proof of delivery.
// The native adapter must prove ownership/revocation before using this ledger.
public sealed class FixtureAbilityPolicyV2
{
    public long Generation { get; private set; }
    private readonly Dictionary<uint,(uint Pawn,int Character)> kits=[];
    private readonly HashSet<uint> quarantined=[];
    private readonly HashSet<uint> completed=[];
    public bool CommitKit(long generation,uint controller,uint pawn,int character,bool human,bool selection,
        bool nativeAdapterAccepted,Func<bool> deliverAndProve)
    {
        if(!nativeAdapterAccepted)return false; // No environment variable can waive native acceptance.
        if(Fence(generation,controller,pawn))return completed.Contains(controller)&&!quarantined.Contains(controller)
            && kits[controller].Character==character;
        if(!ReserveKit(generation,controller,pawn,character,human,selection))return false;
        try {
            if(deliverAndProve()){completed.Add(controller);return true;}
        }catch{ /* An uncertain side effect is quarantined, never retried/minted again. */ }
        quarantined.Add(controller);return false;
    }
    public bool Quarantined(uint controller)=>quarantined.Contains(controller);
    private readonly Dictionary<uint,(uint Owner,uint Pawn,bool Consumed)> utility=[];
    public bool Begin(long generation)
    {
        if(generation<=Generation || quarantined.Count!=0 || utility.Values.Any(g=>!g.Consumed))return false; // Duplicate reset cannot mint kits/reset cooldowns.
        Generation=generation;kits.Clear();utility.Clear();quarantined.Clear();completed.Clear();return true;
    }
    public bool ReserveKit(long generation,uint controller,uint pawn,int character,bool human,bool selection)
    {
        if(generation!=Generation || generation<=0 || !Valid(controller)||!Valid(pawn)
            ||!human||!selection||character is <2 or >6)return false;
        // Same actor cannot reselect to heal, clear cooldowns or receive a second kit.
        return kits.TryAdd(controller,(pawn,character));
    }
    public bool Fence(long generation,uint controller,uint pawn) =>
        generation>0 && generation==Generation && kits.TryGetValue(controller,out var kit)&&kit.Pawn==pawn;
    public bool RecordUtility(long generation,uint controller,uint pawn,uint weapon,bool ownershipProven)
    {
        if(!ownershipProven || !Valid(weapon)||!Fence(generation,controller,pawn))return false;
        return utility.TryAdd(weapon,(controller,pawn,false));
    }
    public bool Consume(long generation,uint controller,uint pawn,uint weapon)
    {
        if(!Fence(generation,controller,pawn)||!utility.TryGetValue(weapon,out var grant)
            ||grant.Owner!=controller||grant.Pawn!=pawn||grant.Consumed)return false;
        utility[weapon]=(controller,pawn,true);return true;
    }
    public bool Redeemable(uint weapon)=>false; // No sale, loot, stash, extraction or SSG value.
    public bool BuyUtilityAllowed=>false; // Kit utility is a separate, explicitly owned source.
    public bool Revoke(long generation,uint controller,uint pawn,Func<uint,bool> revokeOwned)
    {
        if(!Fence(generation,controller,pawn))return false;
        var ok=true;
        foreach(var pair in utility.Where(p=>p.Value.Owner==controller&&p.Value.Pawn==pawn&&!p.Value.Consumed).ToArray())
        {
            try { if(revokeOwned(pair.Key))utility[pair.Key]=(controller,pawn,true);else ok=false; }
            catch{ok=false;}
        }
        // Keep kit and consumed tombstones: revocation/death/reconnect never allows regrant.
        return ok;
    }
    private static bool Valid(uint raw)=>raw!=0 && raw!=uint.MaxValue;
}
