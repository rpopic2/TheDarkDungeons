namespace Items;

public readonly record struct EquipData(string abv, params (Stats stats, int amount)[] mods) : IItemData
{
    public EquipData(int index, string abv, params (Stats stats, int amount)[] mods) : this(abv, mods)
    {
        Inventoriable.RegisterItem(index, this);
    }
    public ItemType itemType { get; init; } = ItemType.Equip;
    public Func<Inventoriable, bool>? onUse { get; init; } = null;

    public IItem Instantiate(Inventoriable owner, Stat ownerStat) => new Equip(owner, ownerStat, this);
}