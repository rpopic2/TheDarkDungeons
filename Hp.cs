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
                OnRaiseDeathEvent(new DeathEventArgs());
                return;
            }
            int damage = cur - value;
            cur = value;
        }
    }
    public Hp(Entity owner, int max)
    {
        this.owner = owner;
        Max = this.cur = max;
    }
    public void TakeDamage(int x)
    {
        Cur -= x;
    }
    public event EventHandler<DeathEventArgs>? RaiseDeathEvent;
    protected virtual void OnRaiseDeathEvent(DeathEventArgs e)
    {
        EventHandler<DeathEventArgs> deathEvent = RaiseDeathEvent!;
        if(deathEvent != null) deathEvent(this, e);
    }
}

public class DeathEventArgs : EventArgs
{
    public DeathEventArgs()
    {

    }
}