public record Equip : ItemEntity
{
    public static readonly ItemData data = new("LUNRIN", ItemType.Equip, null);
    public delegate ref int Del();
    private (Del del, int amount)[] mods;
    public Equip(Inventoriable owner, Stat ownerStat, params (Del del, int amount)[] mods) : base(data, owner, ownerStat)
    {
        this.mods = mods;
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