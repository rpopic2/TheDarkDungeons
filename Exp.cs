public class Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 10;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Player owner;
    public GamePoint point;

    public Exp(Player owner, Action overflow)
    {
        this.owner = owner;
        point = new GamePoint(GetMax(owner.lv), GamePointOption.Stacking, overflow);
    }

    public static int GetMax(int lv)
    => (int)MathF.Floor(lv * lvIncrement);

    public void Gain(int amount)
    {
        point += amount;
        IO.pr($"You gained {amount} xp. {ToString()}");
    }

    public override string ToString()
    {
        return point.ToString();
    }
    public void SetExp(GamePoint newPoint)
    {
        point = newPoint;
    }
}