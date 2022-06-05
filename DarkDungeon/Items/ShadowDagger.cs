public class ShadowDagger : ItemNew
{
    private const int _RANGE = 1;
    protected override void _Pierce()
    {
        AttackRange(_RANGE, StatName.Sol);
    }
}