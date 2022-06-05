public abstract class ItemNew : IItem
{
    protected static Player s_player => Player.instance;
    protected virtual void _Pierce() {}
    protected virtual void _Throw() {}
    public void Pierce() => Program.OnTurnAction += _Pierce;
    public void Throw() => Program.OnTurnAction += _Throw;
    protected void AttackRange(int range, StatName statDepend)
    {
        s_player.Energy.Consume();
        int damage = s_player.Stat.GetRandom(statDepend);
        Creature? hit = Map.Current.RayCast(s_player.Pos, range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
}