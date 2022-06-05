public class ShadowBow : IItem
{
    private int range = 3;
    private static Player s_player => Player.instance;
    public void Throw() => Program.OnTurnAction += _Throw;
    private void _Throw()
    {
        s_player.RemoveItem<Bolt>();
        s_player.Energy.Consume();
        int damage = s_player.Stat.GetRandom(StatName.Lun);
        Creature? hit = Map.Current.RayCast(s_player.Pos, range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
}
