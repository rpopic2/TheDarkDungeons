public record StatInfo(Stat stat, int energy, int killExp, int Sight)
{
    public int SolDifficulty => ApplyDifficulty(stat[StatName.Sol]);
    public int LunDifficulty => ApplyDifficulty(stat[StatName.Lun]);
    public int ConDifficulty => ApplyDifficulty(stat[StatName.Con]);
    private static int ApplyDifficulty(int stat) => (int)(stat + MathF.Floor(Rules.LEVEL_DIFFICULTY * Map.Depth));
}
public record struct Mul(int @base, float multiplier, int scaler)
{
    public static int lv => Map.Depth;
    public static int t => Program.Turn;
    public const int n = 1;
    public static implicit operator int(Mul m)
    {
        return (int)(m.@base + MathF.Floor(m.multiplier * m.scaler));
    }
}