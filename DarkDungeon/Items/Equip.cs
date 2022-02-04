public record Equip : Item
{
    private (Stats stats, int amount)[] mods;
    public new Action<bool> onUse { get; init; }
    public Equip(Inventoriable owner, Stat ownerStat, EquipData data) : base(data.abv, ItemType.Equip, ownerStat)
    {
        this.mods = data.mods;
        onUse = (b) => Array.ForEach(mods, m => ownerStat.ModifyStat(m.stats, m.amount, b));
    }
    public override string ToString() => base.ToString();
    public static new IItem Instantiate(Inventoriable owner, Stat ownerStat, IItemData data) => new Equip(owner, ownerStat, (EquipData)data);
}