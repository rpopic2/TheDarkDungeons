public class GamePoint
{
    private int cur = 0;
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
                    OnOverflow();
                    return;
                }
                else value = Max;
            }
            if (value <= Min)
            {
                if (Option == GamePointOption.Reserving) OnOverflow();
                value = Min;
            }
            cur = value;
        }
    }
    public int Max { get; set; }
    public const int Min = 0;
    public Action OnOverflow = () => { };
    public readonly GamePointOption Option;

    public GamePoint(int max, GamePointOption option, Action overFlow)
    {
        Max = max;
        this.OnOverflow = overFlow;
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
    public override string ToString()
    {
        return $"{Cur}/{Max}";
    }
}