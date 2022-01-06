public class Hp
{
    private Entity owner;
    private int cur;

    public int Max { get; private set; }
    public int Cur
    {
        get => cur;
        set
        {
            if (cur > Max) cur = Max;
            if (cur <= 0)
            {
                cur = 0;
                owner.OnDeath();
            }
            else cur = value;
        }
    }

    public Hp(Entity owner, int max)
    {
        this.owner = owner;
        Max = this.cur = max;
    }
}