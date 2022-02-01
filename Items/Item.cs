public record Item : IItemEntity
{
    public Item(ItemData data, Stat ownerStat)
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
    public Stat stat { get; init; }
    public int stack { get; set; } = 1;
    public string abv { get; init; }
    public ItemType itemType { get; init; }
    public Func<Inventoriable, bool>? onUse { get; init; }

    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"[{abv}{stack}]";
    }
}