public struct Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 10;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Player owner;
    public GamePoint point;
    public Exp(Player owner)
    {
        this.owner = owner;
        point = new GamePoint(1, GamePointOption.Stacking);
        UpdateMax();
    }
    public void UpdateMax()
        => point.Max = GetMax();
    private int GetMax()
    => (int)MathF.Floor(owner.Level * lvIncrement);

    public void Gain(int amount)
    {
        point += amount;
        IO.pr($"Gained {amount} xp. {point}");
    }
    public override string ToString()
    {
        return point.ToString();
    }
}