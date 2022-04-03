public class StanceInfo
{
    public Stance stance;
    public int amount;

    public StanceInfo(Stance stance, int amount)
    {
        Set(stance, amount);
    }
    public void Set(Stance stance, int amount)
    {
        this.stance = stance;
        this.amount = amount;
    }
    public void Reset()
    {
        this.stance = default;
        this.amount = default;
    }
    public override string ToString()
    {
        return $"{stance}, {amount}";
    }
}