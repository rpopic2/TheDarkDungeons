global using Items;
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
        stat[Stats.Sol] = sol;
        stat[Stats.Lun] = lun;
        stat[Stats.Con] = con;
        Name = name;
    }

    public Card Draw() => new Card(GetRandomStat(stat[Stats.Sol]), GetRandomStat(stat[Stats.Lun]), GetRandomStat(stat[Stats.Con]), CardStance.Attack);
    private int GetRandomStat(int stat) => rnd.Next(1, stat + 1);
}