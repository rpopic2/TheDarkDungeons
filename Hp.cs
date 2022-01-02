public struct Hp
{
    private int max;
    private int cur;

    public int Max { get => max; }
    public int Cur
    {
        get
        {
            return cur;
        }
        set
        {
            if (cur > max) cur = max;
            if (cur < 0) cur = 0;
            else cur = value;
        }
    }

    public Hp(Entity owner, int max)
    {
        this.max = this.cur = max;
    }
}