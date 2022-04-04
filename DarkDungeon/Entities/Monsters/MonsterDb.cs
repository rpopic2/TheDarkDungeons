public static class MonsterDb
{
    public static List<MonsterData> data = new();
    public static int Count => data.Count;
    private static int lv => Map.level;
    private static int t => Game.Turn;
    private const int n = 1;
    private static DropList lunDropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.FieryRing, 15),
        (It.LunarRing, 15));
    private static DropList snakeDropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.ShadowAttack, 20),
        (It.Scouter, 5));
    private static StatMul lunaticMul = new(sol: new(1, 0.6f, lv), lun: new(1, n, n), con: new(3, n, n), hp: new(3, 0.6f, lv), cap: new(4, n, n), killExp: new(4, 0.3f, lv));
    public static MonsterData lunatic = new(1, "Lunatic", '>', '<', ClassName.Warrior, lunaticMul, Monster.lunaticBehav, lunDropList);
    private static StatMul snakeMul = new(sol: new(2, 0.6f, lv), lun: new(1, n, n), con: new(2, n, n), hp: new(2, 0.3f, lv), cap: new(2, 0.16f, lv), killExp: new(5, 0.3f, lv));
    public static MonsterData snake = new(2, "Snake", 'S', '2', ClassName.Warrior, snakeMul, Monster.snakeBehav, snakeDropList);
}
public record MonsterData(string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, Action<Monster> behaviour, DropList dropList)
{
    public MonsterData(int i, string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, Action<Monster> behaviour, DropList dropList) : this(name, fowardChar, backwardChar, className, stat, behaviour, dropList)
    {
        MonsterDb.data.Add(this);
    }
}
public record StatMul(Mul sol, Mul lun, Mul con, Mul hp, Mul cap, Mul killExp);
public record DropList
{
    public readonly (It dataIndex, int outof)[] list;
    public DropList()
    {
        throw new ArgumentOutOfRangeException("Droplist cannot be empty");
    }
    public DropList(params (It dataIndex, int outof)[] list)
    {
        this.list = list;
    }
}
//sol lun con hp cap killexp
public record Mul
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