public interface IItem
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Action<Inventoriable>? onUse { get; init; }
    Action<Inventoriable>? onExile { get; init; }

    string ToString();
}
public interface IItemEntity : IItem
{
    int stack { get; set; }
}