public class Stat
{
    private int[] data = new int[3];
    public int this[Stats index]
    {
        get => data[(int)index];
        set => data[(int)index] = value;
    }
    public void ModifyStat(Stats stats, int amount, bool isWearing) => this[stats] += isWearing ? amount : -amount;
}
public enum Stats
{
    Sol, Lun, Con
}