public class ShadowBow : IItem
{
    private static Player s_player => Player.instance;
    public void Throw()
    {
        s_player.RemoveItem<Bolt>();
        Creature? hit = Map.Current.RayCast(s_player.Pos, 3);
        if (hit is Creature hitCreature)
        {
            hitCreature.Stat.Damage(1);
        }
    }
}
