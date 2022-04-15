public class StanceInfo
{
    private StanceName stance;
    private int amount;
    private int amount2;

    public StanceName Stance { get => stance; }
    public int Amount { get => amount; }
    public int Amount2 { get => amount2; }

    public StanceInfo(StanceName stance, int amount, int amount2 = default)
    {
        Set(stance, amount, amount2);
    }
    public void Set(StanceName stance, int amount, int amount2 = default)
    {
        this.stance = stance;
        this.amount = amount;
        this.amount2 = amount2;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void Reset()
    {
        stance = default;
        amount = default;
        amount2 = default;
    }
    public override string ToString()
    {
        return $"{Stance}, {Amount}, {Amount2}";
    }
}