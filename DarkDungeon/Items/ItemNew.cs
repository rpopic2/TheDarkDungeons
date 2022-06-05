public abstract class ItemNew : IItem
{
    public Creature? owner { get; set; }
    protected void AttackRange(int range, StatName statDepend)
    {
        if(owner is null)return;
        int damage = owner.Stat.GetRandom(statDepend);
        Creature? hit = Map.Current.RayCast(owner.Pos, range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
    protected void ConsumeEnergy() => owner?.Energy.Consume();
}