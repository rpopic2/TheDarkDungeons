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
        if (item.itemType == ItemType.Consume && content.IndexOf(item) != -1)
        {
            GetMeta(item).stack++;
        }
        else
        {
            if (content.IndexOf(item) == -1)
            {
                content.Add(item);
                metaDatas.Add(item, new());
            }
        }

        var wears = from p in item.skills where p is WearEffect select p;
        if (wears is not null)
        {
            foreach (WearEffect wear in wears)
            {
                wear.wear.Invoke(owner);
            }
        }
        var passives = from p in item.skills where p is Passive select p as Passive;
        foreach (var pass in passives)
        {
            owner.passives += pass.actionEveryTurn;
        }
    }
    public void Remove(Item item)
    {
        var wears = from p in item.skills where p is WearEffect select p;
        if (wears is not null)
        {
            foreach (WearEffect wear in wears)
            {
                wear.takeOff.Invoke(owner);
            }
        }
        var passives = from p in item.skills where p is Passive select p as Passive;
        foreach (var p in passives)
        {
            if (owner.passives is not null) owner.passives -= p.actionEveryTurn;
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
    public void Consume(Item item)
    {
        if (--GetMeta(item).stack <= 0) Remove(item);
    }
    public override string ToString()
    {
        string result = $"{name}|";
        for (int i = 0; i < INVENSIZE; i++)
        {
            if (i >= Count) result += $"({IO.ItemKeys1[i]})";
            else if(content[i] is Item item)
            {
                string itemName = item.ToString();
                if (item.itemType == ItemType.Consume) itemName = itemName.Insert(1, $"{GetMeta(item).stack}x");
                result += itemName;
            }
        }
        foreach (Item? item in content)
        {

        }
        return result;
    }
}