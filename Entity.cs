public class Entity : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; private set; }
    public Hp Hp { get; private set; }
    public virtual int lv { get; protected set; }
    private Random rnd = new Random();
    public Entity? target;
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
        Player.instance.target = null;
    }
    public void UseCard(int index)
    {
        Card card = Hand[index] ?? throw new ArgumentNullException(nameof(card), "Cannot use card in null index");
        if (target is null) return;
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
    public void DoBattleAction()
    {
        if (target is null) return;
        int dmg = PopAttack();
        int targetBlock = target.PopDefence();
        if (dmg > 0) target.TakeDamage(dmg - targetBlock);
        if (dmg <= 0 && targetBlock > 0) IO.pr($"But {Name} did not attack...");
    }
    private void TakeDamage(int damage)
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
    public int GetRandomStat(int stat)
     => rnd.Next(1, stat + 1);
}