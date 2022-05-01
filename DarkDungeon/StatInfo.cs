public record StatInfo(Stat stat, int energy, int killExp, int Sight)
{
    public int SolDifficulty => ApplyDifficulty(stat[StatName.Sol]);
    public int LunDifficulty => ApplyDifficulty(stat[StatName.Lun]);
    public int ConDifficulty => ApplyDifficulty(stat[StatName.Con]);
    public int KillExpDifficulty => ApplyDifficulty(killExp);
    private static int ApplyDifficulty(int stat) => (int)(stat + MathF.Floor(Rules.LEVEL_DIFFICULTY * Map.Depth));
}