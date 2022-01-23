public class Entity : IMass
{
    public int Level { get; protected set; }
    public int Sol { get; protected set; }
    public int Lun { get; protected set; }
    public int Con { get; protected set; }
    public string Name { get; private set; }
    protected Random rnd = new Random();

    public Entity(int level, int sol, int lun, int con, string name)
    {
        this.Level = level;
        Sol = sol;
        Lun = lun;
        Con = con;
        Name = name;
    }

    public Card Draw() => new Card(GetRandomStat(Sol), GetRandomStat(Lun), GetRandomStat(Con), CardStance.Attack);
    private int GetRandomStat(int stat) => rnd.Next(1, stat + 1);
}