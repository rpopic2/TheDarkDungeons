public class Stat
{
    private int[] data = new int[3];
    public int this[StatName index]
    {
        get => data[(int)index];
        set => data[(int)index] = value;
    }
    public void ModifyStat(StatName stats, int amount, bool isWearing) => this[stats] += isWearing ? amount : -amount;
}