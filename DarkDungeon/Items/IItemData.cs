namespace Items;

public interface IItemBase
{
    string abv { get; init; }
    ItemType itemType { get; init; }
    Func<Inventoriable, bool>? onUse { get; init; }
}
public interface IItemData : IItemBase
{
    IItem Instantiate(Inventoriable owner, Stat ownerStat);
}
public interface IItem : IItemBase
{
    int stack { get; set; }
}