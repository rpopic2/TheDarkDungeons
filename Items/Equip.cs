public record Equip : ItemEntity
{
    public delegate ref int RefInt();
    private (Func<Inventoriable, Equip.RefInt> stat, int amount)[] mods;
    public Equip(Inventoriable owner, Stat ownerStat, EquipData2 data) : base(data.abv, ItemType.Equip, ownerStat)
    {
        this.mods = data.mods;
        onUse = (f) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                var o = mods[i].stat(owner);
                ref int stat = ref o();
                stat += mods[i].amount;
            }

        };
        onExile = (f) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                var o = mods[i].stat(owner);
                ref int stat = ref o();
                stat -= mods[i].amount;
            }
        };
    }
    public override string ToString() => base.ToString();
}