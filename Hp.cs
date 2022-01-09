public class Hp
{
    private Entity owner;
    public GamePoint point;
    public Hp(Entity owner, int max, Action onDeath)
    {
        this.owner = owner;
        point = new GamePoint(max, GamePointOption.Reserving, onDeath);
    }

    public bool IsAlive
        => point.Cur > 0;
}