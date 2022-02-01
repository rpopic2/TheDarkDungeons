public readonly record struct ItemData(string abv, ItemType itemType, Action<Inventoriable>? onUse, Action<Inventoriable>? onExile = null);
public class ItemEntity : IItem
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
public interface IItem
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Action<Inventoriable>? onUse { get; init; }
    Action<Inventoriable>? onExile { get; init; }
    int stack { get; set; }

    string ToString();
}