public readonly struct ItemData
{
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