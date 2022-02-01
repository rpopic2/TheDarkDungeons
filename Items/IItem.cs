public interface IItemData
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Action<Inventoriable>? onUse { get; init; }
    Action<Inventoriable>? onExile { get; init; }

    string ToString();
}
public interface IItemEntity : IItemData
{
    int stack { get; set; }
}