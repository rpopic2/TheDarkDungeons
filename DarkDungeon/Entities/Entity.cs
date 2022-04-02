namespace Entities;
public class Entity
{
    public int Level { get; protected set; }
    protected Stat stat;
    public readonly string Name;
    protected Random rnd = new Random();

    public Entity(int level, int sol, int lun, int con, string name)
    {
        this.Level = level;
        stat = new();
        stat[StatName.Sol] = sol;
        stat[StatName.Lun] = lun;
        stat[StatName.Con] = con;
        Name = name;
    }

    public Card Draw(StatName stats, bool isOffence = true) => new(GetRandomStat(stat[stats]), stats, isOffence);
    private int GetRandomStat(int stat) => rnd.Next(1, stat + 1);
}