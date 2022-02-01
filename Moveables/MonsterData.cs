public readonly record struct MonsterData(string name, char fowardChar, char backwardChar, ClassName className, StatMul stat, Action<Monster> behaviour, DropList dropList);
public readonly record struct StatMul(Mul sol, Mul lun, Mul con, Mul hp, Mul cap, Mul killExp);
public readonly record struct DropList(params (IItemData data, int outof)[] list);
//sol lun con hp cap killexp
public readonly record struct Mul
{
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
        return (int)(m.@base + MathF.Round(m.multiplier * m.scaler));
    }
}
public static class MonsterDb
{
    private static int lv => Map.level;
    private static int t => Program.turn;
    private const int n = 1;
    private static DropList batDropList = new(
        (Inventoriable.ConsumeDb.HpPot, 10),
            (Inventoriable.ConsumeDb.Bag, 11),
            (Torch.torch, 5),
            (EquipDb.FieryRing, 15),
            (EquipDb.LunarRing, 15));
    private static StatMul batMul = new(new(1, 0.6f, lv), new(3, n, n), new(2, n, n), new(2, 0.03f, lv), new(1, 0.16f, lv), new(3, 0.3f, lv));
    public static MonsterData bat = new("Bat", 'b', 'd', ClassName.Assassin, batMul, Monster.batBehav, batDropList);
    private static StatMul lunaticMul = new(new(2, 0.6f, lv), new(1, n, n), new(2, n, n), new(3, 0.03f, lv), new(1, 0.16f, lv), new(4, 0.3f, lv));
    public static MonsterData lunatic = new("Lunatic", '>', '<', ClassName.Warrior, lunaticMul, Monster.lunaticBehav, batDropList);


}