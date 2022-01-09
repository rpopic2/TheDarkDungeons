public class Hp
{
    private Entity owner;
    public GamePoint point;
    public Hp(Entity owner, int max, Action onDeath)
    {
        this.owner = owner;
        point = new GamePoint(max, GamePointOption.Reserving, onDeath);
    }
    public void TakeDamage(int x, int defence)
    {
        if (defence != 0)
        {
            x -= defence;
            IO.pr($"{owner.Name} defences {defence} damage.");
        }
        if (x < 0) x = 0;
        point -= x;
        if (IsAlive) IO.pr($"{owner.Name} takes {x} damage. {point}");
    }

    public bool IsAlive
        => point.Cur > 0;
}