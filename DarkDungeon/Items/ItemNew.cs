public abstract class ItemNew : IItem
{
    protected static Player s_player => Player.instance;
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