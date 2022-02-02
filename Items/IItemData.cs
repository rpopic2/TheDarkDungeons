public interface IItemData
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Func<Inventoriable, bool>? onUse { get; init; }

    string ToString();
}
public interface IItem : IItemData
{
    int stack { get; set; }
    //IItem Instantiate();
}