public class ShadowDagger : ItemNew
{
    protected override void _Pierce()
    {
        AttackRange(1, StatName.Sol);
    }
    protected override void _Throw()
    {
        AttackRange(3, StatName.Lun);
    }
}