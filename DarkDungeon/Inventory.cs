using System.Collections;

public class Inventory : ICollection<Item?>
{
    public Fightable owner;
    public const int INVENSIZE = 5;
    private List<Item?> content;
    public ref readonly List<Item?> Content => ref content;
    private Dictionary<Item, ItemMetaData> metaDatas;
    private ItemMetaData bareHandMetaData = new();
    public readonly string name;

    public Inventory(Fightable owner, string name)
    {
        this.owner = owner;
        content = new(INVENSIZE);
        metaDatas = new(INVENSIZE);
        this.name = name;
    }
    public Item? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);

    public bool IsReadOnly => ((ICollection<Item?>)content).IsReadOnly;

    public void Add(Item? value)
    {
        if (value is not Item item) throw new Exception("Cannot add null into inventory.");
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
                wear.Behaviour.Invoke(owner);
            }
        }
        var passives = item.skills.OfType<Passive>();
        var invocationList = owner.passives.GetInvocationList();
        foreach (var pass in passives)
        {
            if(!invocationList.Contains(pass.Behaviour)) owner.passives += pass.Behaviour;
        }
    }
    public void Remove(Item item)
    {
        var wears = from p in item.skills where p is WearEffect select p;
        if (wears is not null)
        {
            foreach (WearEffect wear in wears)
            {
                wear.OnTakeOff.Invoke(owner);
            }
        }
        var passives = from p in item.skills where p is Passive select p as Passive;
        foreach (var p in passives)
        {
#pragma warning disable CS8601
            if (owner.passives is not null) owner.passives -= p.Behaviour;
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
        if (item == Fightable.bareHand) return bareHandMetaData;
        return metaDatas[item];
    }
    public void Consume(Item item)
    {
        if (--GetMeta(item).stack <= 0)
        {
            Remove(item);
            IO.rk($"{owner.Name}은 {item.Name}을 모두 사용했다.");
        }
    }
    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < INVENSIZE; i++)
        {
            if (i >= Count) result += $"({IO.ITEMKEYS1[i]})";
            else if (content[i] is Item item)
            {
                string itemName = item.ToString();
                int stack = GetMeta(item).stack;
                if (stack > 1) itemName = itemName.Insert(1, $"{stack}x");
                result += itemName;
            }
        }
        return result + "|" + name;
    }

    public void Clear()
    {
        ((ICollection<Item?>)content).Clear();
    }

    public bool Contains(Item? item)
    {
        return ((ICollection<Item?>)content).Contains(item);
    }

    public void CopyTo(Item?[] array, int arrayIndex)
    {
        ((ICollection<Item?>)content).CopyTo(array, arrayIndex);
    }

    bool ICollection<Item?>.Remove(Item? item)
    {
        return ((ICollection<Item?>)content).Remove(item);
    }

    public IEnumerator<Item?> GetEnumerator()
    {
        return ((IEnumerable<Item?>)content).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)content).GetEnumerator();
    }
}