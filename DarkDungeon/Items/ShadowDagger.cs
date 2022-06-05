public class ShadowDagger : ItemNew
{
    public void Pierce()
    {
        ConsumeEnergy();
        AttackRange(1, StatName.Sol);
    }
    public void Throw()
    {
        ConsumeEnergy();
        AttackRange(3, StatName.Lun);
    }
    public void Shadowstep()
    {
        if(owner is null) return;
        Creature? hit = Map.Current.RayCast(owner.Pos, 3);
        if (hit is Creature hitCreature)
        {
            ConsumeEnergy();
            Position targetPos = hit.Pos;
            owner.Pos = new(targetPos.Back(2), targetPos.facing);
        }
    }
}