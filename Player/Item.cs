public struct Item
{
    public readonly string abv;

    public Item(string abv)
    {
        this.abv = abv;
    }

    public override string ToString()
    {
        return $"[{abv}]";
    }
}