public class ShadowBow : ItemNew
{
    public void Throw()
    {
        ConsumeEnergy();
        owner.RemoveItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
