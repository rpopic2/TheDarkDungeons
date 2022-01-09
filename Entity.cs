public class Entity : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; private set; }
    public Hp Hp { get; private set; }
    public virtual int lv { get; protected set; }
    private Random rnd = new Random();
    public Entity? curTarget;
    public int atk { get; private set; }
    public int def { get; private set; }

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
        => new Card(GetRandomStat(sol), GetRandomStat(lun), GetRandomStat(con), Stance.Attack);
    protected virtual void OnDeath()
    {
        IO.pr($"{Name} died.");
        Player.instance.curTarget = null;
    }
    public void UseCard(int index)
    {
        Card card = Hand[index] ?? throw new ArgumentNullException(nameof(card), "Cannot use card in null index");
        if (curTarget is null) return;
        UseCard(card);
    }
    protected void UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case Stance.Attack:
                atk += card.sol;
                break;
            case Stance.Defence:
                def += card.lun;
                break;
        }
    }
    public int Attack(Entity target)
    {
        int dmg = atk;
        atk = 0;
        IO.pr($"{Name} attacks with {dmg} damage.");
        return dmg;
    }
    public int PopAttack()
    {
        if (!Attacking) return 0;
        int dmg = atk;
        IO.pr($"{Name} attacks with {dmg} damage.");
        atk = 0;
        return dmg;
    }
    public int PopDefence()
    {
        if (!Defending) return 0;
        int block = def;
        IO.pr($"{Name} defences {def} damage.");
        def = 0;
        return block;
    }
    public void PrDefend()
    {
        IO.pr($"{Name} defences {def} damage.");
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) throw new Exception($"Cannot inflict {damage} damage. target : {Name}");
        if (Defending)
        {
            damage -= def;
            PrDefend();
        }
        if (damage < 0) damage = 0;
        Hp.TakeDamage(damage);
        if (Hp.IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp.point}");
    }
    private void NewTakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        Hp.TakeDamage(damage);
        if (Hp.IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp.point}");
    }
    public string Stats
        => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {lv}\nHp : {Hp.point}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";

    public bool Attacking
        => atk > 0;
    public bool Defending
        => def > 0;
    public static void Battle(Entity t1, Entity t2)
    {
        int t1dmg = t1.PopAttack();
        int t2block = t2.PopDefence();
        if (t1dmg > 0) t2.NewTakeDamage(t1dmg - t2block);

        int t2dmg = t2.PopAttack();
        int t1block = t1.PopDefence();
        if (t2dmg > 0) t1.NewTakeDamage(t1dmg - t2block);
    }
    public int GetRandomStat(int stat)
     => rnd.Next(1, stat + 1);
}