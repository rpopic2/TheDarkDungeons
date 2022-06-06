public partial class Creature
{
    private List<ItemNew> _items = new();
    public bool HasItem<T>() where T : ItemNew
    {
        return _items.OfType<T>().Any();
    }
    public int GetStackOfItem<T>() where T : ItemNew
    {
        return _items.OfType<T>().Count();
    }
    public int GetStackOfItem(ItemNew item)
    {
        var query = from i in _items
                    where i.Name == item.Name
                    select i;
        return query.Count();
    }

    public T? GetItemOrDefault<T>() where T : ItemNew
    {
        return _items.OfType<T>().FirstOrDefault();
    }
    public T GetItem<T>() where T : ItemNew
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