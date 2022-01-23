public class Mass : IMass
{
    public int level { get; protected set; }
    public int Sol { get; protected set; }
    public int Lun { get; protected set; }
    public int Con { get; protected set; }
    protected Random rnd = new Random();
    public Card Draw() => new Card(GetRandomStat(Sol), GetRandomStat(Lun), GetRandomStat(Con), CardStance.Attack);
    private int GetRandomStat(int stat) => rnd.Next(1, stat + 1);
}