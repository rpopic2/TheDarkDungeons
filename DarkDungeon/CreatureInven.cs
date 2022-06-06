public partial class Creature
{
    private List<ItemBase> _items = new();
    public readonly Item.BareHand BareHand = new();
    public bool HasItem<T>() where T : ItemBase
    {
        return _items.OfType<T>().Any();
    }
    public int GetStack<T>() where T : ItemBase
    {
        ItemBase item = GetItem<T>();
        if (item is IStackable stackable) return stackable.Stack;
        else if (item is not null) return 1;
        else return 0;
    }
    public T? GetItemOrDefault<T>() where T : ItemBase
    {
        return _items.OfType<T>().FirstOrDefault();
    }
    public T GetItem<T>() where T : ItemBase
    {
        return _items.OfType<T>().FirstOrDefault()!;
    }
    public ItemBase GetItemAt(int index)
    {
        if (index >= _items.Count) return BareHand;
        return _items[index];
    }
    public void GiveItem(ItemBase item)
    {
        int indexOfItem = _items.IndexOf(item);
        if (item is IStackable && indexOfItem != -1)
        {
            ((IStackable)_items[indexOfItem]).Stack++;
        }
        else
        {
            item.Owner = this;
            _items.Add(item);
        }
    }
    public void RemoveItemStack<T>(int stackToRemove) where T : IStackable
    {
        IStackable? targetItem = _items.OfType<IStackable>().FirstOrDefault()!;
        if (targetItem is not null) targetItem.Stack -= stackToRemove;
    }
    public void RemoveItem<T>() where T : ItemBase
    {
        ItemBase? targetItem = GetItem<T>();
        if (targetItem is not null)
        {
            _items.Remove(targetItem);
        }
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
        string? name = _items.ElementAtOrDefault(index)?.ToString();
        if (name is null) name = BareHand.Name;
        return $"({IO.ITEMKEYS1[index]}|{name})";
    }
}