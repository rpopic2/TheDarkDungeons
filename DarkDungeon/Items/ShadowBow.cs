public class ShadowBow : ItemNew
{
    public override string Name => "그림자 활";

    public void Throw()
    {
        ConsumeEnergy();
        ConsumeItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
