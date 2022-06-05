public partial class Creature
{
    private List<IItem> _items = new();
    public bool HasItem<T>() where T : IItem
    {
        return _items.OfType<T>().Any();
    }
    public int GetStackOfItem<T>() where T : IItem
    {
        return _items.OfType<T>().Count();
    }
    public T? GetItemOrDefault<T>() where T : IItem
    {
        return _items.OfType<T>().FirstOrDefault();
    }
    public T GetItem<T>() where T : IItem
    {
        return _items.OfType<T>().FirstOrDefault()!;
    }
    public void GiveItem(ItemNew item, int stack = 1)
    {
        for (int i = 0; i < stack; i++)
        {
            item.owner = this;
            _items.Add(item);
        }
    }
    public void RemoveItem<T>() where T : IItem
    {
        IItem? targetItem = GetItem<T>();
        if (targetItem is not null) _items.Remove(targetItem);
    }
    public string InvenToString
    {
        get
        {
            return _items.ToFString();
        }
    }
}