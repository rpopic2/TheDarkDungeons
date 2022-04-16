public record StatMul(int sol, int lun, int con, int hp, int cap, int killExp);
public record struct Mul(int @base, float multiplier, int scaler)
{
    public static int lv => Map.depth;
    public static int t => Program.Turn;
    public const int n = 1;
    public static implicit operator int(Mul m)
    {
        return (int)(m.@base + MathF.Floor(m.multiplier * m.scaler));
    }
}