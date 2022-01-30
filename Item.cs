public readonly record struct Item(string abv, ItemType itemType, Action<Fightable>? onUse, Action<Fightable>? onExile = null)
{
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

// public interface IItem
// {
//     string Abv { get; init; }
//     ItemType itemType { get; init; }
//     Action<Fightable>? onUse { get; init; }

//     string ToString();
// }