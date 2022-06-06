public partial class Creature
{
    private List<ItemNew> _items = new();
    public bool HasItem<T>() where T : ItemNew
    {
        return _items.OfType<T>().Any();
    }
    public int GetStack<T>() where T : ItemNew
    {
        ItemNew item = GetItem<T>();
        if (item is IStackable stackable) return stackable.stack;
        else if (item is not null) return 1;
        else return 0;
    }
    public T? GetItemOrDefault<T>() where T : ItemNew
    {
        return _items.OfType<T>().FirstOrDefault();
    }
    public T GetItem<T>() where T : ItemNew
    {
        return _items.OfType<T>().FirstOrDefault()!;
    }
    public void GiveItem(ItemNew item)
    {
        int indexOfItem = _items.IndexOf(item);
        if (item is IStackable && indexOfItem != -1)
        {
            ((IStackable)_items[indexOfItem]).stack++;
        }
        else
        {
            item.owner = this;
            _items.Add(item);
        }
    }
    public void RemoveItemStack<T>(int stackToRemove) where T : IStackable
    {
        IStackable? targetItem = _items.OfType<IStackable>().FirstOrDefault()!;
        if (targetItem is not null) targetItem.stack -= stackToRemove;
    }
    public void RemoveItem<T>() where T : ItemNew
    {
        ItemNew? targetItem = GetItem<T>();
        if (targetItem is not null) _items.Remove(targetItem);
    }
    public string InvenToString
    {
        get
        {
            string result = string.Empty;
            for (int i = 0; i < _items.Count + 1; i++) result += IndexToString(i);
            return result;
        }
    }
    private string IndexToString(int index)
    {
        string? name;
        ItemNew? item = _items.ElementAtOrDefault(index);
        if (item is not null)
        {
            name = item.ToString();
        }
        else name = BareHand.BareHandName;
        return $"({IO.ITEMKEYS1[index]}|{name})";
    }
}