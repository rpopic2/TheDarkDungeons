public class Stat
{
    private int[] data = new int[3];
    public int this[Stats index]
    {
        get => data[(int)index];
        set => data[(int)index] = value;
    }
}
public enum Stats
{
    Sol, Lun, Con
}