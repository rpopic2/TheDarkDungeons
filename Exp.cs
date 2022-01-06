public struct Exp
{
    private const float lvCurve = 1.25f;
    private const int lvMultiplier = 10;
    const float lvIncrement = lvCurve * lvMultiplier;
    private Charactor owner;
    private int cur = 0;
    public int Cur
    {
        get => cur;
        private set
        {
            cur = value;
            if (cur >= Max)
            {
                cur -= Max;
                owner.LvUp();
                Max = UpdateMax();
            }
        }
    }
    public int Max { get; private set; }

    public Exp(Charactor owner)
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
    }
    
    public override string ToString()
    {
        return $"{Cur}/{Max}";
    }
}