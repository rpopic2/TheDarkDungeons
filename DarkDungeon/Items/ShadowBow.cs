namespace Item;
public class ShadowBow : ItemBase
{
    public ShadowBow()
    {
        Skills = new() { Throw };
    }

    public override string Name => "그림자 활";
    public override List<Action> Skills { get; init; }

    public void Throw()
    {
        ConsumeEnergy();
        ConsumeItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
