public class ExperiencePoint : GamePoint
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 7;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Status owner;
    public ExperiencePoint(Status owner) : base(1, GamePointOption.Stacking)
    {
        this.owner = owner;
        OnIncrease += new EventHandler<PointArgs>(OnGain);
        UpdateMax();
    }
    private void OnGain(object? sender, PointArgs e) => IO.pr($"{e.Amount}xp를 얻었다. {ToString()}");
    public void UpdateMax() => Max = CalcMax();
    private int CalcMax() => (int)MathF.Floor(owner.Level * lvIncrement);

    public static implicit operator int(ExperiencePoint exp) => exp.Cur;
}