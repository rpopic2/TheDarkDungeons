using System.Collections;

public class Inventory : ICollection<ItemOld?>
{
    public Creature owner;
    public const int INVENSIZE = 5;
    private List<ItemOld?> content;
    public ref readonly List<ItemOld?> Content => ref content;
    private Dictionary<ItemOld, ItemMetaData> metaDatas;
    private ItemMetaData bareHandMetaData = new();
    public readonly string name;

    public Inventory(Creature owner, string name)
    {
        this.owner = owner;
        this.name = name;
        content = new(INVENSIZE);
        metaDatas = new(INVENSIZE);
    }
    public ItemOld? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);

    public bool IsReadOnly => ((ICollection<ItemOld?>)content).IsReadOnly;

    public void Add(ItemOld? value, ItemMetaData metaData, out bool added)
    {
        if (value is not ItemOld item) throw new Exception("Trying to put non-Item objext into Inventory.");
        if (item.itemType == ItemType.Consume && content.IndexOf(item) != -1)
        {
            GetMeta(item)!.stack += metaData.stack;
            added = true;
        }
        else added = Store(item, metaData);
    }
    public void Add(ItemOld item, ItemMetaData metaData)
    {
        Store(item, metaData);
    }
    public void Add(ItemOld? value) => Add(value, new(), out _);
    private bool Store(ItemOld item, ItemMetaData metaData)
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
            metaDatas.Add(item, metaData);
            Wear(item);
            RegisterPassives(item);
        }
        return true;
    }
    private void Wear(ItemOld item)
    {
        item.ForEach<WearEffect>((w) => w.Behaviour.Invoke(owner));
    }
    private void TakeOff(ItemOld item)
    {
        item.ForEach<WearEffect>((w) => w.OnTakeOff.Invoke(owner));
    }
    private void RegisterPassives(ItemOld item)
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
    private void UnregisterPassives(ItemOld item)
    {
#pragma warning disable
        item.ForEach<Passive>(p => owner.passives -= p.Behaviour);
    }
    public void Remove(ItemOld item)
    {
        TakeOff(item);
        UnregisterPassives(item);

        content.Remove(item);
        metaDatas.Remove(item);
    }
    public ItemOld? this[int index]
    {
        get => content[index];
    }
    public ItemMetaData? GetMeta(ItemOld item)
    {
        if (item == Creature.bareHand) return bareHandMetaData;
        if(metaDatas.ContainsKey(item)) return metaDatas[item];
        else return null;
    }
    public int GetStack(ItemOld item) => GetMeta(item).stack;
    public int GetMagicCharge(ItemOld item) => GetMeta(item).magicCharge;
    public void Consume(ItemOld item)
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
            else if (content[i] is ItemOld item)
            {
                itemName = item.ToString();
                int stack = GetMeta(item)?.stack ?? 0;
                if (stack > 1) itemName = itemName.Insert(1, $"{stack}x");
            }
            result += itemName.AppendKeyName(i);
        }
        return result + "|" + name;
    }

    public void Clear()
    {
        ((ICollection<ItemOld?>)content).Clear();
    }

    public bool Contains(ItemOld? item)
    { 
        return ((ICollection<ItemOld?>)content).Contains(item);
    }

    public void CopyTo(ItemOld?[] array, int arrayIndex)
    {
        ((ICollection<ItemOld?>)content).CopyTo(array, arrayIndex);
    }

    bool ICollection<ItemOld?>.Remove(ItemOld? item)
    {
        return ((ICollection<ItemOld?>)content).Remove(item);
    }

    public IEnumerator<ItemOld?> GetEnumerator()
    {
        return ((IEnumerable<ItemOld?>)content).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)content).GetEnumerator();
    }
}