public readonly record struct ItemData(string abv, ItemType itemType, Action<Inventoriable>? onUse, Action<Inventoriable>? onExile = null) : IItem;
