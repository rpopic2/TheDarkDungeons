public record Equip : ItemEntity
{
    public delegate ref int RefInt();
    private (Func<Stat, RefInt> stat, int amount)[] mods;
    public new Action onUse { get; init; }
    public new Action onExile { get; init; }
    public Equip(Inventoriable owner, Stat ownerStat, EquipData data) : base(data.abv, ItemType.Equip, ownerStat)
    {
        this.mods = data.mods;
        Action<bool> Invoker = (b) =>
        {
            for (int i = 0; i < mods.Length; i++)
            {
                RefInt o = mods[i].stat(ownerStat);
                ref int stat = ref o();
                if (b) stat += mods[i].amount;
                else stat -= mods[i].amount;
            }
        };
        onUse = () => Invoker(true);
        onExile = () => Invoker(false);
    }
    public override string ToString() => base.ToString();
}