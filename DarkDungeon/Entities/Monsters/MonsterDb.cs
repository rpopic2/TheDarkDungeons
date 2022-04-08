public partial class Monster
{
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

    private static DropList batDropList = new(
            (It.HpPot, 10),
            (It.Bag, 11),
            (It.Torch, 5),
            (It.FieryRing, 15),
            (It.LunarRing, 15));
    private static StatMul lunaticMul = new(sol: 1, lun: 1, con: 3, hp: 3, cap: 4, killExp: 4);
    public static MonsterData lunatic = new("Lunatic", '>', '<', lunaticMul, (m) => m.LunaticBehav(), Item.holySword, new int[] { 2, 0, 2, 0 }, lunDropList);
    private static StatMul snakeMul = new(sol: 2, lun: 1, con: 2, hp: 2, cap: 2, killExp: 5);
    public static MonsterData snake = new("Snake", 'S', '2', snakeMul, (m) => m.SnakeBehav(), Item.bareHand, new int[] { 2, 0, 0 }, snakeDropList);
    public static StatMul batMul = new(sol:1, lun:3, con:2, hp:2, cap:3, killExp:3);
    public static MonsterData bat = new("Bat", 'b', 'd', batMul, (m) => m.BatBehav(), Item.bareHand, new int[] { 1, 1, 0 }, batDropList);
    public static List<MonsterData> data = new() { lunatic, snake, bat };
}
public record MonsterData(string name, char fowardChar, char backwardChar, StatMul stat, Action<Monster> behaviour, Item startItem, int[] startToken, DropList dropList);

public record StatMul(int sol, int lun, int con, int hp, int cap, int killExp);
public record struct DropList
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
public record struct Mul
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