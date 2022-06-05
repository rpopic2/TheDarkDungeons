public class ShadowBow : ItemNew
{
    public void Throw()
    {
        s_player.Energy.Consume();
        s_player.RemoveItem<Bolt>();
        AttackRange(3, StatName.Lun);
    }
}
