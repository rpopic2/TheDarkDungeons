public class Entity : IMass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public Hp Hp { get; private set; }
    public int sol { get; set; }
    public int lun { get; set; }
    public int con { get; set; }
    public int Lv { get; protected set; }
    private Random rnd = new Random();

    public Entity(string name, ClassName className)
    {
        Name = name;
        ClassName = className;
        Hand = new Hand(3);
        Hp = new Hp(this, 5);
        sol = 1;
        lun = 1;
        con = 1;
        Lv = 0;
    }
    public virtual Card Draw()
        => new Card(rnd.Next(sol, 1), rnd.Next(lun, 1));
    public void OnDeath()
    {
        IO.pr($"{Name} died.");
    }
    public string Stats
    {
        get => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Lv}\nHp : {Hp.Max}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
    }
}