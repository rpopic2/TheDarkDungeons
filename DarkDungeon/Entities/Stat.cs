public class Stat
{
    public const int MIN = 1;
    private int[] _data = new int[3];
    private Random _rnd = new Random();
    public int this[StatName index]
    {
        get => _data[(int)index];
        set => _data[(int)index] = value;
    }
    public void ModifyStat(StatName stats, int amount, bool isWearing) => this[stats] += isWearing ? amount : -amount;
    public int GetRandom(StatName stat) => _rnd.Next(MIN, this[stat]);
}
public enum StatName
{
    Sol, Lun, Con
}