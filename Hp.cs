public class Hp
{
    private Entity owner;
    private int cur;

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
                OnRaiseDeathEvent(new EventArgs());
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
    public event EventHandler<EventArgs>? RaiseDeathEvent;
    protected virtual void OnRaiseDeathEvent(EventArgs e)
    {
        EventHandler<EventArgs> deathEvent = RaiseDeathEvent!;
        if (deathEvent != null) deathEvent(this, e);
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