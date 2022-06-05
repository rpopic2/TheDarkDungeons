public class ShadowDagger : IItem
{
    private int range = 1;
    private static Player s_player => Player.instance;
    public void Pierce() => Program.OnTurnAction += _Pierce;
    private void _Pierce()
    {
        s_player.Energy.Consume();
        int damage = s_player.Stat.GetRandom(StatName.Sol);
        Creature? hit = Map.Current.RayCast(s_player.Pos, range);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(damage);
        }
    }
}