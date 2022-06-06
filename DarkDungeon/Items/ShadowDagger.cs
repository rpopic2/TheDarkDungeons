namespace Item;
public class ShadowDagger : ItemBase
{
    public override string Name => "그림자 단검";
    public override List<Action> Skills { get; init; }

    public ShadowDagger()
    {
        Skills = new() { Pierce, Throw };
    }

    public void Pierce()
    {
        ConsumeEnergy();
        AttackRange(1, StatName.Sol);
    }
    public void Throw()
    {
        ConsumeEnergy();
        RemoveItem<ShadowDagger>();
        Creature? hit = AttackRange(3, StatName.Lun);
        hit?.GiveItem(this);
    }
    public void Shadowstep()
    {
        if (Owner is null) return;
        Creature? hit = RayCast(3);
        if (hit is Creature hitCreature)
        {
            ConsumeEnergy();
            Position targetPos = hit.Pos;
            Owner.Pos = new(targetPos.Back(2), targetPos.facing);
        }
    }

    public IEnumerable<char> ToSkillString()
    {
        return $"{Name} | (q|찌르기)(w|던지기)";
    }

}