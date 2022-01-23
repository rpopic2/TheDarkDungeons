public struct Item
{
    public readonly ItemData data;

    public Item(ItemData data)
    {
        this.data = data;
    }

    public override string ToString()
    {
        return $"[{data.abv}]";
    }
}