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
        IO.pr("created an entity");
        Hand = new Hand(3);
        Hp = new Hp(this, 5);
    }
    public virtual Card Draw()
        => new Card(rnd.Next(sol), rnd.Next(lun));
    public void OnDeath()
    {
        IO.pr($"{Name} died.");
    }
    public string Stats
    {
        get => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Lv}\nHp : {Hp.Max}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
    }
}