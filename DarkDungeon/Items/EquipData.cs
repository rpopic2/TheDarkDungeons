public readonly record struct EquipData(string abv, params (Stats stats, int amount)[] mods) : IItemData
{
    public ItemType itemType { get; init; } = ItemType.Equip;
    public Func<Inventoriable, bool>? onUse { get; init; } = null;

    public IItem Instantiate(Inventoriable owner, Stat ownerStat) => new Equip(owner, ownerStat, this);
}

public static class EquipDb
{
    public static readonly EquipData LunarRing = new("LUNRIN", (Stats.Lun, 3));
    public static readonly EquipData AmuletOfLa = new("AMULLA", (Stats.Sol, 20));
    public static readonly EquipData FieryRing = new("FIRING", (Stats.Sol, 3));
}