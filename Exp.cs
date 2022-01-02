public struct Exp
{
    const float lvCurve = 1.25f;
    const int lvMultiplier = 10;
    const float lvIncrement = lvCurve * lvMultiplier;
    Charactor owner;
    private int cur = 0;
    private int max = 1;
    public int Cur
    {
        get => cur;
        private set
        {
            cur = value;
            if (cur >= max)
            {
                cur -= max;
                owner.OnLvUp();
                max = UpdateMax();
            }
        }
    }
    public int Max { get => max; }

    public Exp(Charactor owner)
    {
        this.owner = owner;
        max = UpdateMax();
    }
    
    private int UpdateMax()
    => (int)MathF.Floor(owner.lv * lvIncrement);

    public void Gain(int amount)
    {
        Cur += amount;
    }
    
    public override string ToString()
    {
        return $"{cur}/{max}";
    }
}