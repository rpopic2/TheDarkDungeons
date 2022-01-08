public class Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 10;
    private const float lvIncrement = lvCurve * lvMultiplier;
    private Player owner;
    public Action OnLvUp = () => { };
    private int cur = 0;
    public int Cur
    {
        get => cur;
        private set
        {
            if (value >= Max)
            {
                value -= Max;
                OnLvUp();
                Max = UpdateMax();
            }
            cur = value;
        }
    }
    public int Max { get; private set; }

    public Exp(Player owner)
    {
        this.owner = owner;
        Max = 0;
        Max = UpdateMax();
    }

    private int UpdateMax()
    => (int)MathF.Floor(owner.lv * lvIncrement);

    public void Gain(int amount)
    {
        Cur += amount;
        IO.pr($"You gained {amount} xp. {ToString()}");
    }

    public override string ToString()
    {
        return $"{Cur}/{Max}";
    }
}