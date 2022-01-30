public class GamePoint
{
    public const int Min = 0;
    private int cur = 0;
    public event EventHandler<PointArgs>? OnHeal;
    public event EventHandler<PointArgs>? OnDamage;
    public event EventHandler? OnOverflow;

    public int Cur
    {
        get => cur;
        private set
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
        x.OnHeal?.Invoke(x, new PointArgs(amount));
        return x;
    }
    public static GamePoint operator -(GamePoint x, int amount)
    {
        x.Cur -= amount;
        x.OnDamage?.Invoke(x, new PointArgs(amount));
        if (x.Cur == Min && x.Option == GamePointOption.Reserving)
            x.OnOverflow?.Invoke(x, EventArgs.Empty);
        return x;
    }
    public bool IsMin => Cur == Min;
    public override string ToString()
    {
        return $"{Cur}/{Max}";
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