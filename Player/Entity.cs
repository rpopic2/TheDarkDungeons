public class Entity : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; private set; }
    public Hp Hp { get; private set; }
    public virtual int Lv { get; protected set; }
    protected Random rnd = new Random();
    public virtual Entity? Target { get; protected set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Star { get; private set; }
    public bool IsResting { get; set; }

    public Entity(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        if (cap <= 0) throw new ArgumentException("cap cannot be equal or less than 0.");
        Cap = cap;
        Hand = new Hand(this);
        Hp = new Hp(this, maxHp, () => OnDeath());
        Sol = sol;
        Lun = lun;
        Con = con;
        Lv = lv;
    }
    public override Card Draw()
        => new Card(GetRandomStat(Sol), GetRandomStat(Lun), GetRandomStat(Con), Stance.Attack);
    protected virtual void OnDeath()
    {
        IO.pr($"\n{Name} died. {Hp}");
    }
    public void UseCard(int index, out bool elaspeTurn)
    {
        elaspeTurn = true;
        Card card = Hand[index] ?? throw new ArgumentNullException(nameof(card), "Cannot use card in null index");
        if (Target is null)
        {
            //IO.pr("No target to use card");
            elaspeTurn = false;
            return;
        }
        if (card.Stance == Stance.Star) elaspeTurn = false;
        _UseCard(card);
    }
    protected void _UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case Stance.Attack:
                Atk += card.Sol;
                break;
            case Stance.Defence:
                Def += card.Lun;
                break;
            case Stance.Star:
                Star = card.Con;
                break;
        }
    }
    public virtual void Rest()
    {
        if(Map.Current.IsVisible((Moveable)this)) IO.pr($"{Name} is resting a turn.");
        IsResting = true;
    }
    private int PopAttack()
    {
        if (Atk <= 0) return 0;
        int dmg = Atk;
        IO.pr($"{Name} attacks with {dmg} damage.");
        Atk = 0;
        return dmg + PopStar();
    }
    private int PopDefence()
    {
        if (Def <= 0) return 0;
        int block = Def;
        IO.pr($"{Name} defences {Def} damage.");
        Def = 0;
        return block + PopStar();
    }
    private bool PopResting()
    {
        if (!IsResting) return false;
        IsResting = false;
        return true;
    }
    private int PopStar()
    {
        if (Star <= 0) return 0;
        int star = Star;
        IO.pr($"...and {Star} more damage!");
        Star = 0;
        return star;
    }
    public void TryBattle()
    {
        if (Target is null) return;
        int dmg = PopAttack();
        int targetBlock = Target.PopDefence();
        bool targetResting = Target.PopResting();
        if (dmg > 0)
        {
            if (targetResting)
            {
                dmg = (int)MathF.Round(dmg * Rules.vulMulp);
                IO.pr($"{Target.Name} is resting vulnerable, takes {dmg}x damage!");
            }
            Target.TakeDamage(dmg - targetBlock);
        }
        if (dmg <= 0 && targetBlock > 0) IO.pr($"But {Name} did not attack...");
    }
    private void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        Hp.TakeDamage(damage);
        if (IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp.point}");
    }

    public string Stats
        => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Lv}\nHp : {Hp.point}\tStrength : {Sol}\tDexterity : {Lun}\tWisdom : {Con}";
    private int GetRandomStat(int stat)
        => rnd.Next(1, stat + 1);
    public bool IsAlive
        => Hp.IsAlive;
    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        return MapSymb.invisible;
    }
}