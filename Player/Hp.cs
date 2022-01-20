public struct Hp
{
    private Fightable owner;
    public GamePoint point;
    public Hp(Fightable owner, int max, Action onDeath)
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

    public override string ToString()
    {
        return point.ToString();
    }

    public void RestoreFull()
    {
        point += point.Max;
    }
}