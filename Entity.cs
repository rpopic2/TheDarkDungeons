public class Entity : IMass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public Hp Hp { get; private set; }
    public int sol { get; set; }
    public int lun { get; set; }
    public int con { get; set; }
    public int lv { get; protected set; }
    private Random rnd = new Random();

    public Entity(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        Hand = new Hand(cap);
        Hp = new Hp(this, maxHp);
        Hp.RaiseDeathEvent += OnDeath!;
        this.sol = sol;
        this.lun = lun;
        this.con = con;
        this.lv = lv;
    }
    public virtual Card Draw()
        => new Card(rnd.Next(1, sol + 1), rnd.Next(1, lun + 1));
    protected virtual void OnDeath(object sender, EventArgs e)
    {
        IO.pr($"{Name} died.");
    }
    public void TakeDamage(int x)
    {
        Hp.TakeDamage(x);
        if (Hp.Cur > 0) IO.pr($"{Name} takes {x} damage. {Hp.Cur}/{Hp.Max}");
    }
    public string Stats
    {
        get => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {lv}\nHp : {Hp.Max}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
    }
}