public abstract class ItemNew
{
    public abstract string Name { get; }
    public Creature? owner { get; set; }
    protected void AttackRange(int range, StatName statDepend)
    {
        if (owner is null) return;
        int damage = owner.Stat.GetRandom(statDepend);
        Creature? hit = RayCast(range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
    protected Creature? RayCast(int range)
    {
        if (owner is null) return null;
        return Map.Current.RayCast(owner.Pos, range);
    }
    protected void ConsumeEnergy() => owner?.Energy.Consume();
    protected void ConsumeItem<T>() where T : IStackable => owner?.RemoveItemStack<T>(1);

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType()) return false;
        return Equals((ItemNew)obj);
    }
    public bool Equals(ItemNew item) => item.Name == Name;
    public override int GetHashCode() => HashCode.Combine(Name);
    public override string ToString()
    {
        string name = Name;
        if (this is IStackable stackable) name += $"x{(int)stackable.Stack}";
        return name;
    }
}