public class Inventory
{
    public const int INVENSIZE = 5;
    private List<Item?> content;
    private Dictionary<Item, ItemMetaData> metaDatas;
    public readonly string name;
    public ref readonly List<Item?> Content => ref content;

    public Inventory(string name)
    {
        content = new(INVENSIZE);
        metaDatas = new(INVENSIZE);
        this.name = name;
    }
    public Item? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);
    public void Add(Item item)
    {
        if (content.IndexOf(item) != -1)
        {
            GetMeta(item).stack++;
        }
        else
        {
            content.Add(item);
            metaDatas.Add(item, new());
        }
    }
    public void Remove(Item item)
    {
        content.Remove(item);
        metaDatas.Remove(item);
    }
    public Item? this[int index]
    {
        get => content[index];
    }
    public ItemMetaData GetMeta(Item item)
    {
        return metaDatas[item];
    }
    public override string ToString()
    {
        string result = $"{name}|";
        foreach (Item? item in content)
        {
            if (item == null) result += "{EMPTY}";
            else
            {
                string itemName = item.ToString();
                if (item.itemtType == ItemType.Consume) itemName = itemName.Insert(1, $"{GetMeta(item).stack}x");
                result += itemName;
            }
        }
        return result;
    }

    public void Consume(Item item)
    {
        if(--GetMeta(item).stack <= 0) Remove(item);
    }
}