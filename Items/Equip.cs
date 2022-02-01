public record Equip : Item
{
    public delegate ref int RefInt();
    private (Func<Stat, RefInt> refGetter, int amount)[] mods;
    public new Action<bool> onUse { get; init; }
    public Equip(Inventoriable owner, Stat ownerStat, EquipData data) : base(data.abv, ItemType.Equip, ownerStat)
    {
        this.mods = data.mods;
        onUse = (b) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                RefInt o = mods[i].refGetter(ownerStat);
                ref int stat = ref o();
                if (b) stat += mods[i].amount;
                else stat -= mods[i].amount;
            }
        };
    }
    public override string ToString() => base.ToString();
}