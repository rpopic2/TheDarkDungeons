public class ShadowBow : ItemNew
{
    public void Throw()
    {
        ConsumeEnergy();
        s_player.RemoveItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
