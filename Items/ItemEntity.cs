public record ItemEntity : IItemEntity
{
    public ItemEntity(ItemData data, Inventoriable owner)
    {
        this.abv = data.abv;
        this.itemType = data.itemType;
        this.onUse = data.onUse;
        this.onExile = data.onExile;
        this.owner = owner;
    }
    public Inventoriable owner { get; init; }
    public int stack { get; set; } = 1;
    public string abv { get; init; }
    public ItemType itemType { get; init; }
    public Action<Inventoriable>? onUse { get; init; }
    public Action<Inventoriable>? onExile { get; init; }

    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"[{abv}{stack}]";
    }
}