using System.Collections;

public class Inventory : ICollection<Item?>
{
    public Creature owner;
    public const int INVENSIZE = 5;
    private List<Item?> content;
    public ref readonly List<Item?> Content => ref content;
    private Dictionary<Item, ItemMetaData> metaDatas;
    private ItemMetaData bareHandMetaData = new();
    public readonly string name;

    public Inventory(Creature owner, string name)
    {
        this.owner = owner;
        this.name = name;
        content = new(INVENSIZE);
        metaDatas = new(INVENSIZE);
    }
    public Item? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);

    public bool IsReadOnly => ((ICollection<Item?>)content).IsReadOnly;

    public void Add(Item? value, int stack, out bool added)
    {
        if (value is not Item item) throw new Exception("Trying to put non-Item objext into Inventory.");
        if (item.itemType == ItemType.Consume && content.IndexOf(item) != -1)
        {
            GetMeta(item).stack += stack;
            added = true;
        }
        else
        {
            added = Store(item, stack);
        }
    }
    public void Add(Item item, ItemMetaData metaData)
    {
        content.Add(item);
        metaDatas.Add(item, metaData);
    }
    public void Add(Item? value) => Add(value, 1, out _);
    private bool Store(Item item, int stack)
    {
        if (Count >= INVENSIZE && owner is Player p)
        {
            IO.pr("인벤토리가 꽉 찼습니다");
            p.DiscardItem();
            if (Count >= INVENSIZE) return false;
        }
        if (content.IndexOf(item) == -1)
        {
            content.Add(item);
            metaDatas.Add(item, new());
            if (item.itemType == ItemType.Consume) GetMeta(item).stack = stack;
            Wear(item);
            RegisterPassives(item);
        }
        return true;
    }
    private void Wear(Item item)
    {
        item.ForEach<WearEffect>((w) => w.Behaviour.Invoke(owner));
    }
    private void TakeOff(Item item)
    {
        item.ForEach<WearEffect>((w) => w.OnTakeOff.Invoke(owner));
    }
    private void RegisterPassives(Item item)
    {
        item.ForEach<Passive>((p) =>
        {
            if (!OwnerHasPassive(p)) owner.passives += p.Behaviour;
        });

        bool OwnerHasPassive(Passive passive)
        {
            var invocationList = owner.passives.GetInvocationList();
            return invocationList.Contains(passive.Behaviour);
        }
    }
    private void UnregisterPassives(Item item)
    {
#pragma warning disable
        item.ForEach<Passive>(p => owner.passives -= p.Behaviour);
    }
    public void Remove(Item item)
    {
        TakeOff(item);
        UnregisterPassives(item);

        content.Remove(item);
        metaDatas.Remove(item);
    }
    public Item? this[int index]
    {
        get => content[index];
    }
    public ItemMetaData GetMeta(Item item)
    {
        if (item == Creature.bareHand) return bareHandMetaData;
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
            string itemName = string.Empty;
            if (i >= Count) itemName = "(맨손)";
            else if (content[i] is Item item)
            {
                itemName = item.ToString();
                int stack = GetMeta(item).stack;
                if (stack > 1) itemName = itemName.Insert(1, $"{stack}x");
            }
            result += itemName.AppendKeyName(i);
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