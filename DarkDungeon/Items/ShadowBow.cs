public class ShadowBow : ItemNew
{
    public void Throw()
    {
        ConsumeEnergy();
        ConsumeItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
