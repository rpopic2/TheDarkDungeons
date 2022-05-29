public class Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 7;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Status owner;
    public GamePoint point;
    public Exp(Status owner)
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
    public static implicit operator int(Exp exp) => exp.point.Cur;
}