public record StatInfo(Status stat, int energy, int killExp)
{
    public int KillExpDifficulty => ApplyDifficulty(killExp);
    private static int ApplyDifficulty(int stat) => (int)(stat + MathF.Floor(Rules.LEVEL_DIFFICULTY * Map.Depth));
}