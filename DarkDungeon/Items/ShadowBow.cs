public class ShadowBow : ItemNew
{
    private const int _RANGE = 3;
    protected override void _Throw()
    {
        s_player.RemoveItem<Bolt>();
        AttackRange(_RANGE);
    }
}
