public record LunarRing : ItemEntity
{
    public static readonly ItemData data = new("LUNRIN", ItemType.Equip, null);
    public const int LunInc = 3;
    public ref int RefLun() => ref ownerStat.RefLun();


    public LunarRing(Inventoriable owner, Stat ownerStat) : base(data, owner, ownerStat)
    {
        onUse = (f) =>
        {
            RefLun() += LunInc;
        };
        onExile = (f) =>
        {
            RefLun() -= LunInc;
        };
    }
    public override string ToString() => base.ToString();
}