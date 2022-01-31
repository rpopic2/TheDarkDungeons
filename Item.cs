public readonly record struct Item(string abv, ItemType itemType, Action<Fightable>? onUse, Action<Fightable>? onExile = null) : IItem
{
    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"[{abv}]";
    }
}
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
        return $"[{abv}]";
    }
}
// public readonly record struct Equip : IItem
// {
//     private readonly (int stat, int amount)[] mods;
//     public string Abv { get; init; }
//     public ItemType itemType { get; init; } = ItemType.Equip;
//     public Action<Fightable>? onUse { get; init; } = null;
//     public readonly Action<Fightable> onPickup;
//     public readonly Action<Fightable> onExile;

//     public Equip(string abv, params (int stat, int amount)[] mods)
//     {
//         Abv = abv;
//         this.mods = mods;
//         Abv = abv;
//         this.itemType = itemType;
//         this.onUse = onUse;
//         onPickup = f =>
//    {
//        for (int i = 0; i < mods.Length; i++)
//        {
//            mods[i].stat += mods[i].amount;
//        }
//    };
//         onExile = f =>
//    {
//        for (int i = 0; i < mods.Length; i++)
//        {
//            mods[i].stat -= mods[i].amount;
//        }
//    };
//     }

//     public override string ToString()
//     {
//         if (Abv is null) return "[EMPTY]";
//         return $"<{Abv}>";
//     }
// }

public interface IItem
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Action<Fightable>? onUse { get; init; }
    Action<Fightable>? onExile { get; init; }

    string ToString();
}