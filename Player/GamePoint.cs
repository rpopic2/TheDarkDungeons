public class GamePoint
{
    public const int Min = 0;
    private int cur = 0;
    private int Cur
    {
        get => cur;
        set
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
                if (Option == GamePointOption.Reserving)
                    OnOverflow?.Invoke(this, EventArgs.Empty);
            }
            else cur = value;
        }
    }
    public int Max { get; set; }
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