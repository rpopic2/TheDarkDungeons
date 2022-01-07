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
            if (value > Max)
            {
                cur = Max;
                return;
            }
            if (value <= 0)
            {
                cur = 0;
                owner.OnDeath();
                return;
            }
            int damage = cur - value;
            cur = value;
            IO.pr($"{owner.Name} takes {damage} damage. {Cur}/{Max}");
        }
    }
    public Hp(Entity owner, int max)
    {
        this.owner = owner;
        Max = this.cur = max;
    }
}