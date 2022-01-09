public struct Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 10;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Player owner;
    public GamePoint point;

    public Exp(Player owner, Action overflow)
    {
        this.owner = owner;
        point = new GamePoint(1, GamePointOption.Stacking, overflow);
        UpdateMax();
    }
    public void UpdateMax()
        => point.Max = GetMax();
    private int GetMax()
    => (int)MathF.Floor(owner.Lv * lvIncrement);

    public void Gain(int amount)
    {
        point += amount;
        IO.pr($"You gained {amount} xp. {ToString()}");
    }

    public override string ToString()
    {
        return point.ToString();
    }
}