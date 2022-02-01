public record LunarRing : ItemEntity
{
    public static readonly ItemData data = new("LUNRIN", ItemType.Equip, null);
    public const int LunInc = 3;

    public LunarRing(Inventoriable owner) : base(data, owner)
    {
        // onUse = (f) =>
        // {
        //     owner.Lun += LunInc;
        // };
        // onExile = (f) =>
        // {
        //     owner.Lun -= LunInc;
        // };
    }
}