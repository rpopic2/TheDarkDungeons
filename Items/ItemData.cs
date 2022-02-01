public readonly record struct ItemData(string abv, ItemType itemType, Action<Inventoriable>? onUse, Action<Inventoriable>? onExile = null) : IItem;
public readonly record struct EquipData(string abv, params (Equip.RefInt del, int amount)[] mods);
public readonly record struct EquipData2(string abv, params (Func<Inventoriable, Equip.RefInt> stat, int amount)[] mods);
