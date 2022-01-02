public class Entity : IMass
{
    public string entityName = "unnamed";
    public ClassName entityClass;

    private int lv = 0;
    protected int cap = 3;
    private Hand hand;
    public Hand Hand { get => hand; }
    private Hp hp;
    public Hp Hp { get => hp; }
    public int sol { get; set; }
    public int lun { get; set; }
    public int con { get; set; }
    public int Lv { get => lv; protected set => lv = value; }
    protected Random rnd = new Random();

    public Entity()
    {
        IO.pr("created an entity");
        hand = new Hand(cap);
        hp = new Hp(this, 5);
    }
    public virtual Card Draw()
        => new Card(rnd.Next(sol), rnd.Next(lun));
    public void OnDeath()
    {
        IO.pr($"{entityName} died.");
    }

    public string Stats
    {
        get => $"Name : {entityName}\tClass : {entityClass.ToString()}\tLevel : {Lv}\nHp : {Hp.Max}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
    }

}