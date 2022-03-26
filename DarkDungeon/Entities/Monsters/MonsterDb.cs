public static class MonsterDb
{
    public static List<MonsterData> data = new List<MonsterData>{Bat.bat, Lunatic.lunatic, Snake.snake};
    public static int Count => data.Count;
    private static int lv => Map.level;
    private static int t => Game.Turn;
    private const int n = 1;

    
}
public readonly record struct MonsterData(string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, DropList dropList)
{
    public MonsterData(int i, string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, DropList dropList) : this(name, fowardChar, backwardChar, className, stat, dropList)
    {
        //MonsterDb.data.Add(this);
    }
}
public readonly record struct StatMul(Mul sol, Mul lun, Mul con, Mul hp, Mul cap, Mul killExp);
public readonly record struct DropList
{
    public readonly (It dataIndex, int outof)[] list;
    public DropList()
    {
        throw new ArgumentOutOfRangeException("Droplist needs cannot be empty");
    }
    public DropList(params (It dataIndex, int outof)[] list)
    {
        this.list = list;
    }
}
//sol lun con hp cap killexp
public readonly record struct Mul
{
    public static int lv => Map.level;
    public static int t => Game.Turn;
    public const int n = 1;
    public readonly int @base;
    public readonly float multiplier;
    public readonly int scaler;
    public Mul(int @base, float multiplier, int scaler)
    {
        this.@base = @base;
        this.multiplier = multiplier;
        this.scaler = scaler;
    }
    public static implicit operator int(Mul m)
    {
        return (int)(m.@base + MathF.Floor(m.multiplier * m.scaler));
    }
}