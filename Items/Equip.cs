public readonly record struct EquipData
{
    public readonly string abv;
    public delegate ref int Del();
    public readonly (Del del, int amount)[] mods;
    public EquipData(string abv, params (Del del, int amount)[] mods)
    {
        this.abv = abv;
        this.mods = mods;
    }
}

public record Equip : ItemEntity
{
    //public static readonly ItemData data = new("LUNRIN", ItemType.Equip, null);
    private (EquipData.Del del, int amount)[] mods;
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