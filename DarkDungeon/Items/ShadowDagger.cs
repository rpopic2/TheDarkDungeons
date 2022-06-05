public class ShadowDagger : ItemNew
{
    public void Pierce()
    {
        AttackRange(1, StatName.Sol);
    }
    public void Throw()
    {
        AttackRange(3, StatName.Lun);
    }
    public void Shadowstep()
    {
        Creature? hit = Map.Current.RayCast(s_player.Pos, 3);
        if (hit is Creature hitCreature)
        {
            Position targetPos = hit.Pos;
            s_player.Pos = new(targetPos.Back(2), targetPos.facing);
        }
    }
}