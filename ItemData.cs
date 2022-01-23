public readonly record struct ItemData (string abv, int amount)
{
    public static readonly ItemData HpPot = new("HPPOT", 3);
    public static readonly ItemData AmuletOfLa = new("AMULA", 30 );

    public override string ToString()
    {
        return $"[{abv}]";
    }
}