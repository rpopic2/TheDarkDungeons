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
        point.OnIncrease += new EventHandler<PointArgs>(OnGain);
        UpdateMax();
    }
    private void OnGain(object? sender, PointArgs e) => IO.pr($"{e.Amount}xp를 얻었다. {point}");
    public void UpdateMax() => point.Max = CalcMax();
    private int CalcMax() => (int)MathF.Floor(owner.Level * lvIncrement);

    public override string ToString()
    {
        return point.ToString();
    }
}