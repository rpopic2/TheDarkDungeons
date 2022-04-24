public record StatInfo(int sol, int lun, int con, int cap, int killExp, int Sight);
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