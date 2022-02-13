namespace Items;

public record Item : IItem
{
    public const string Empty = "{EMPTY}";
    public Item(IItemData data, Stat ownerStat)
    {
        this.abv = data.abv;
        this.itemType = data.itemType;
        this.onUse = data.onUse;
        this.stat = ownerStat;
    }
    public Item(string abv, ItemType itemType, Stat ownerStat)
    {
        this.abv = abv;
        this.itemType = itemType;
        this.stat = ownerStat;
    }
    public static IItem Instantiate(Inventoriable owner, Stat ownerStat, IItemData data) => new Item((ItemData)data, ownerStat);
    public Stat stat { get; init; }
    public int stack { get; set; } = 1;
    public int level { get; set; } = 1;
    public string abv { get; init; }
    public ItemType itemType { get; init; }
    public Func<Inventoriable, bool>? onUse { get; init; }

    public override string ToString()
    {
        if (abv is null) return Item.Empty;
        return $"[{abv}{stack}]";
    }
}
public readonly record struct ItemData(string abv, ItemType itemType, Func<Inventoriable, bool>? onUse) : IItemData
{
    public ItemData(int index, string abv, ItemType itemType, Func<Inventoriable, bool>? onUse) : this(abv, itemType, onUse)
    {
        Inventoriable.RegisterItem(index, this);
    }
    public IItem Instantiate(Inventoriable owner, Stat ownerStat) => new Item(this, ownerStat);
}