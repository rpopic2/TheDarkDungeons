public class Entity : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; private set; }
    public Hp Hp { get; private set; }
    public virtual int lv { get; protected set; }
    private Random rnd = new Random();

    public Entity(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        Cap = cap;
        Hand = new Hand(this);
        Hp = new Hp(this, maxHp, () => OnDeath());
        this.sol = sol;
        this.lun = lun;
        this.con = con;
        this.lv = lv;
    }
    public override Card Draw()
        => new Card(rnd.Next(1, sol + 1), rnd.Next(1, lun + 1));
    protected virtual void OnDeath()
    {
        IO.pr($"{Name} died.");
        Player.instance.curTarget = null;
    }

    public virtual void Attack(Card card, Entity target)
    {
        IO.pr($"{Name} attacks {target.Name} with {card.sol} damage.");
        target.Hp.TakeDamage(card.sol);
    }
    public string Stats
    {
        get => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {lv}\nHp : {Hp.point}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
    }
}