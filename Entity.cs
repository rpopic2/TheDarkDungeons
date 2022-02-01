public class Entity
{
    public int Level { get; protected set; }
    protected StatData stat;
    public readonly string Name;
    protected Random rnd = new Random();

    public Entity(int level, int sol, int lun, int con, string name)
    {
        this.Level = level;
        stat = new StatData(sol, lun, con);
        Name = name;
    }

    public Card Draw() => new Card(GetRandomStat(stat.Sol), GetRandomStat(stat.Lun), GetRandomStat(stat.Con), CardStance.Attack);
    private int GetRandomStat(int stat) => rnd.Next(1, stat + 1);
}