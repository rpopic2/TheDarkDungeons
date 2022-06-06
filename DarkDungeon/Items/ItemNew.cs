public abstract class ItemBase
{
    public abstract string Name { get; }
    public Creature? Owner { get; set; }
    public abstract List<Action> Skills { get; init; }
    protected void AttackRange(int range, StatName statDepend)
    {
        if (Owner is null) return;
        int damage = Owner.Stat.GetRandom(statDepend);
        Creature? hit = RayCast(range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
    protected Creature? RayCast(int range)
    {
        if (Owner is null) return null;
        return Map.Current.RayCast(Owner.Pos, range);
    }
    protected void ConsumeEnergy() => Owner?.Energy.Consume();
    protected void ConsumeItem<T>() where T : IStackable => Owner?.RemoveItemStack<T>(1);
    protected void RemoveItem<T>() where T : ItemBase => Owner?.RemoveItem<T>();

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType()) return false;
        return Equals((ItemBase)obj);
    }
    public bool Equals(ItemBase item) => item.Name == Name;
    public override int GetHashCode() => HashCode.Combine(Name);
    public override string ToString()
    {
        string name = Name;
        if (this is IStackable stackable) name += $"x{(int)stackable.Stack}";
        return name;
    }
    public Action GetSkillAt(int index)
    {
        return Skills[index];
    }
}