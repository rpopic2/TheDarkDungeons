public class Hp
{
    private Entity owner;
    public GamePoint point;
    private int def;
    public Hp(Entity owner, int max, Action onDeath)
    {
        this.owner = owner;
        point = new GamePoint(max, GamePointOption.Reserving, onDeath);
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
        point -= x;
        if (IsAlive) IO.pr($"{owner.Name} takes {x} damage. {point}");
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
        => point.Cur > 0;
}