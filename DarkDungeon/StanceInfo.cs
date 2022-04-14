public class StanceInfo
{
    private StanceName stance;
    private StanceName last;
    private int amount;
    private int amount2;

    public StanceName Stance { get => stance; }
    public int Amount { get => amount; }
    public int Amount2 { get => amount2; }
    public StanceName Last { get => last;}

    public StanceInfo(StanceName stance, int amount, int amount2 = default)
    {
        Set(stance, amount);
    }
    public void Set(StanceName stance, int amount, int amount2 = default)
    {
        last = stance;
        this.stance = stance;
        this.amount = amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void Reset()
    {
        stance = default;
        amount = default;
    }
    public override string ToString()
    {
        return $"{Stance}, {Amount}";
    }
}