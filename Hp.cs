public class Hp
{
    private int cur;

    public int Max { get; private set;}
    public int Cur
    {
        get => cur;
        set
        {
            if (cur > Max) cur = Max;
            if (cur < 0) cur = 0;
            else cur = value;
        }
    }

    public Hp(Entity owner, int max)
    {
        Max = this.cur = max;
    }
}