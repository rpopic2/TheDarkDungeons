public class GamePoint
{
    private int cur = 0;
    private int Cur
    {
        get => cur;
        set
        {
            if (value >= Max)
            {
                if (Option == GamePointOption.Reserving)
                {
                    cur = Max;
                    return;
                }
                cur = value - Max;
                OnOverflow?.Invoke(this, EventArgs.Empty);
            }
            else if (value <= Min)
            {
                cur = Min;
                if (Option == GamePointOption.Stacking) return;
                OnOverflow?.Invoke(this, EventArgs.Empty);
            }
            else cur = value;
        }
    }
    public int Max { get; set; }
    public const int Min = 0;
    public event EventHandler? OnOverflow;
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
        return x;
    }
    public static GamePoint operator -(GamePoint x, int amount)
    {
        x.Cur -= amount;
        return x;
    }
    public bool IsMin => Cur == Min;
    public override string ToString()
    {
        return $"{Cur}/{Max}";
    }
}