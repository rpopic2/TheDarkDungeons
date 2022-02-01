public readonly record struct EquipData(string abv, params (Func<Stat, Equip.RefInt> refGetter, int amount)[] mods);

public static class EquipDb
{
    public static readonly EquipData LunarRing = new("LUNRIN", (s => s.RefLun, 3));
    public static readonly EquipData AmuletOfLa = new("AMULLA", (s => s.RefSol, 20));
    public static readonly EquipData FieryRing = new("FIRING", (s => s.RefSol, 3));
}