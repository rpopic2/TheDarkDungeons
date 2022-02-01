public readonly record struct EquipData(string abv, params (Equip.RefInt del, int amount)[] mods);

public record Equip : ItemEntity
{
    public delegate ref int RefInt();
    private (RefInt del, int amount)[] mods;
    public Equip(Stat ownerStat, EquipData data) : base(data.abv, ItemType.Equip, ownerStat)
    {
        this.mods = data.mods;
        onUse = (f) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                ref int stat = ref mods[i].del();
                stat += mods[i].amount;
            }

        };
        onExile = (f) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                ref int stat = ref mods[i].del();
                stat -= mods[i].amount;
            }
        };
    }
    public override string ToString() => base.ToString();
}