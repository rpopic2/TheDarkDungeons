public record LunarRing : ItemEntity
{
    public static readonly ItemData data = new("LUNRIN", ItemType.Equip, null);
    public const int LunInc = 3;

    public LunarRing(Inventoriable owner, Stat ownerStat) : base(data, owner, ownerStat)
    {
        onUse = (f) =>
        {
            ownerStat.Lun += LunInc;
        };
        onExile = (f) =>
        {
            ownerStat.Lun -= LunInc;
        };
    }
}