public static class MonsterDb
{
    public static List<MonsterData> data = new();
    public static int Count => data.Count;
    private static int lv => Map.level;
    private static int t => Game.Turn;
    private const int n = 1;
    private static DropList batDropList = new();
    private static DropList snakeDropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.ShadowAttack, 20),
        (It.Scouter, 5));
    private static StatMul batMul = new(new(1, 0.6f, lv), new(3, n, n), new(2, n, n), new(2, 0.4f, lv), new(1, 0.16f, lv), new(3, 0.3f, lv));
    public static MonsterData bat = new(0, "Bat", 'b', 'd', ClassName.Assassin, batMul, Monster.batBehav, batDropList);
    private static StatMul lunaticMul = new(new(3, 0.6f, lv), new(1, n, n), new(2, n, n), new(3, 0.6f, lv), new(1, 0.16f, lv), new(4, 0.3f, lv));
    public static MonsterData lunatic = new(1, "Lunatic", '>', '<', ClassName.Warrior, lunaticMul, Monster.lunaticBehav, batDropList);
    private static StatMul snakeMul = new(sol:new(2, 0.6f, lv), lun:new(1, n, n), con:new(2, n, n), hp:new(2, 0.3f, lv), cap:new(2, 0.16f, lv), killExp:new(5, 0.3f, lv));
    public static MonsterData snake = new(2, "Snake", 'S', 'Ƨ', ClassName.Warrior, snakeMul, Monster.snakeBehav, snakeDropList);
}
public readonly record struct MonsterData(string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, Action<Monster> behaviour, DropList dropList)
{
    public MonsterData(int i, string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, Action<Monster> behaviour, DropList dropList) : this(name, fowardChar, backwardChar, className, stat, behaviour, dropList)
    {
        MonsterDb.data.Add(this);
    }
}
public readonly record struct StatMul(Mul sol, Mul lun, Mul con, Mul hp, Mul cap, Mul killExp);
public readonly record struct DropList(params (It dataIndex, int outof)[] list);
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