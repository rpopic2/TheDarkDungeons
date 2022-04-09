public class Inventory
{
    public Inventoriable owner;
    public const int INVENSIZE = 5;
    private List<Item?> content;
    private Dictionary<Item, ItemMetaData> metaDatas;
    public readonly string name;
    public ref readonly List<Item?> Content => ref content;

    public Inventory(Inventoriable owner, string name)
    {
        this.owner = owner;
        content = new(INVENSIZE);
        metaDatas = new(INVENSIZE);
        this.name = name;
    }
    public Item? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);
    public void Add(Item item)
    {
        var passives = (from p in item.skills where p is Passive select p).ToArray();
        foreach (var pass in passives)
        {
            ((Passive)pass).wear.Invoke(owner);
        }
        if (item.itemType == ItemType.Consume && content.IndexOf(item) != -1)
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
        var passives = (from p in item.skills where p is Passive select p).ToArray();
        foreach (var pass in passives)
        {
            ((Passive)pass).takeOff.Invoke(owner);
        }
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
                if (item.itemType == ItemType.Consume) itemName = itemName.Insert(1, $"{GetMeta(item).stack}x");
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