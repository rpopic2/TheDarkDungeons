namespace Entities;
public class Entity
{
    public int Level { get; protected set; }
    protected Stat stat;
    public readonly string Name;
    public Entity(int level, int sol, int lun, int con, string name)
    {
        this.Level = level;
        stat = new();
        stat[StatName.Sol] = sol;
        stat[StatName.Lun] = lun;
        stat[StatName.Con] = con;
        Name = name;
    }
    public int GetStat(StatName statName)
    {
        return stat[statName];
    }
    public Card Draw(StatName stats, bool isOffence = true) => new(stat.GetRandom(stats), stats, isOffence);
}