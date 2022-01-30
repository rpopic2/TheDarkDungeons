public readonly record struct Item(string abv, ItemType itemType, Action<Fightable>? onPickup, Action<Fightable>? onUse) : IItem
{
    public string Abv { get; init; } = abv;
    public ItemType itemType { get; init; } = itemType;
    public Action<Fightable>? onUse { get; init; } = onUse;

    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"[{abv}]";
    }
}
public readonly record struct Equip(string abv, params (int stat, int amount)[] mods) : IItem
{
    public string Abv { get; init; } = abv;
    public ItemType itemType { get; init; } = ItemType.Equip;
    public Action<Fightable>? onUse { get; init; } = null;
    public readonly Action<Fightable> onPickup = f =>
    {
        for (int i = 0; i < mods.Length; i++)
        {
            mods[i].stat += mods[i].amount;
        }
    };
    public readonly Action<Fightable> onExile = f =>
    {
        for (int i = 0; i < mods.Length; i++)
        {
            mods[i].stat -= mods[i].amount;
        }
    };

    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"<{abv}>";
    }
}

public interface IItem
{
    string Abv { get; init; }
    ItemType itemType { get; init; }
    Action<Fightable>? onUse { get; init; }

    string ToString();
}