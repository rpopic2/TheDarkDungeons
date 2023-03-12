public class GamePoint
{
    public const int Min = 0;
    private int cur = 0;
    public event EventHandler<PointArgs>? OnIncrease;
    public event EventHandler<PointArgs>? OnDecrease;
    public event EventHandler? OnOverflow;

    public int Cur
    {
        get => cur;
        protected set
        {
            if (value >= Max)
            {
                if (Option == GamePointOption.Stacking)
                {
                    cur = value - Max;
                    OnOverflow?.Invoke(this, EventArgs.Empty);
                }
                else cur = Max;
            }
            else if (value <= Min)
            {
                cur = Min;
            }
            else cur = value;
        }
    }
    public int Max { get; set; }
    public readonly GamePointOption Option;

    public GamePoint(int max, GamePointOption option)
    {
        Max = max;
        Option = option;
        if (option == GamePointOption.Reserving) cur = Max;
    }
    public static GamePoint operator +(GamePoint x, int amount)
    {
        x.Cur += amount;
        x.OnIncrease?.Invoke(x, new PointArgs(amount));
        return x;
    }
    public static GamePoint operator -(GamePoint x, int amount)
    {
        x.Cur -= amount;
        x.OnDecrease?.Invoke(x, new PointArgs(amount));
        if (x.Cur == Min && x.Option == GamePointOption.Reserving)
            x.OnOverflow?.Invoke(x, EventArgs.Empty);
        return x;
    }
    public void IncreaseMax(int value)
    {
        Max += value;
        Cur += value;
    }
    public void DecreaseMax(int value)
    {
        Max -= value;
        if (Cur > Max) Cur = Max;
    }
    public bool IsMin => Cur == Min;
    public bool IsMax => Cur == Max;
    public bool IsInjured =>  Cur <= Max / 3;
    public override string ToString()
    {
        string temp = $"{Cur}/{Max}";
        //if (Option == GamePointOption.Reserving && IsInjured) temp = $"^r{Cur}/{Max}^/";
        return temp;
    }
}

public class PointArgs : EventArgs
{
    public PointArgs(int amount)
    {
        Amount = amount;
    }

    public int Amount { get; set; }
}
