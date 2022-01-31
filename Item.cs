public readonly record struct Item(string abv, ItemType itemType, Action<Fightable>? onUse, Action<Fightable>? onExile = null) : IItem;
public class ItemEntity : IItem
{
    public ItemEntity(Item data, Fightable owner)
    {
        this.abv = data.abv;
        this.itemType = data.itemType;
        this.onUse = data.onUse;
        this.onExile = data.onExile;
        this.owner = owner;
    }
    public Fightable owner { get; init; }
    public int stack = 1;
    public string abv { get; init; }
    public ItemType itemType { get; init; }
    public Action<Fightable>? onUse { get; init; }
    public Action<Fightable>? onExile { get; init; }

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
    Action<Fightable>? onUse { get; init; }
    Action<Fightable>? onExile { get; init; }

    string ToString();
}