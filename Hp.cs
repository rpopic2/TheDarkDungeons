public class Hp
{
    private Entity owner;
    private int cur;
    public Action OnDeath = () => {};

    public int Max { get; private set; }
    public int Cur
    {
        get => cur;
        private set
        {
            if (value > Max)
            {
                cur = Max;
                return;
            }
            if (value <= 0)
            {
                cur = 0;
                OnDeath();
                return;
            }
            int damage = cur - value;
            cur = value;
        }
    }
    private int def;
    public Hp(Entity owner, int max)
    {
        this.owner = owner;
        Max = this.cur = max;
        EventListener.OnTurnEnd += () => OnTurnEnd();
    }
    public void TakeDamage(int x)
    {
        if (def != 0)
        {
            x -= def;
            IO.pr($"{owner.Name} defences {def} damage.");
        }
        if (x < 0) x = 0;
        Cur -= x;
        if (IsAlive) IO.pr($"{owner.Name} takes {x} damage. {Cur}/{Max}");
    }

    private void OnTurnEnd()
    {
        def = 0;
    }

    internal void Defence(int x)
    {
        def = x;
    }

    public bool IsAlive
        => Cur > 0;
}