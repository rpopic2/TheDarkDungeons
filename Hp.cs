public struct Hp
{
    private Entity owner;
    public GamePoint point;
    public Hp(Entity owner, int max, Action onDeath)
    {
        this.owner = owner;
        point = new GamePoint(max, GamePointOption.Reserving, onDeath);
    }
    public void TakeDamage(int x)
    {
        point -= x;
    }

    public bool IsAlive
        => point.Cur > 0;
    
}