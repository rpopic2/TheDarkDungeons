public interface IItemData
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Func<Inventoriable, bool>? onUse { get; init; }

    string ToString();
}
public interface IItemEntity : IItemData
{
    int stack { get; set; }
}