public readonly struct ItemData
{
    public static readonly ItemData HpPot = new("HPPOT", 3);
    public static readonly ItemData AmuletOfLa = new("AMULA", 30);

    public readonly string abv;
    public readonly int amount;

    public ItemData(string abv, int amount)
    {
        this.abv = abv;
        this.amount = amount;
    }
    public override string ToString()
    {
        return $"[{abv}]";
    }
}