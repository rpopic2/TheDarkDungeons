public abstract class ItemNew : IItem
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
    protected void ConsumeItem<T>() where T : IItem => owner?.RemoveItem<T>();
    public override string ToString()
    {
        return $"{Name}";
    }
}