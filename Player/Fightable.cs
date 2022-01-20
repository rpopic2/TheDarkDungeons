public class Fightable : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; protected set; }
    public Hp Hp { get; protected set; }
    protected Random rnd = new Random();
    public virtual Fightable? Target { get; protected set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Star { get; private set; }
    public bool IsResting { get; set; }

    public Fightable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        if (cap <= 0 || cap > 10) throw new ArgumentException("cap is out of index");
        Cap = cap;
        Hand = new Hand(this);
        Hp = new Hp(this, maxHp, () => OnDeath());
        Sol = sol;
        Lun = lun;
        Con = con;
        level = lv;
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
        if (card.Stance == Stance.Star)
        {
            elaspeTurn = false;
            IO.pr("Next move will be reinforced.");
        }
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
        /*if(Map.Current.IsVisible((Moveable)this))*/
        IO.pr($"{Name} is resting a turn.");
        IsResting = true;
    }
    public void UnRest()
    {
        IsResting = false;
    }
    private int PopAttack()
    {
        if (Atk <= 0) return 0;
        int dmg = Atk;
        IO.pr($"{Name} attacks with {dmg} damage.");
        Atk = 0;
        int poppedStar = PopStar();
        if(poppedStar > 0) IO.pr($"...and {poppedStar} more damage! (total {dmg + poppedStar})");
        return dmg + poppedStar;
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
        Star = 0;
        return star;
    }
    public void TryBattle()
    {
        if (!IsAlive) return;
        if (Target is null) return;
        int dmg = PopAttack();
        int targetBlock = Target.PopDefence();
        bool targetResting = Target.PopResting();
        if (dmg > 0)
        {
            if (targetResting)
            {
                dmg = (int)MathF.Round(dmg * Rules.vulMulp);
                IO.pr($"{Target.Name} is resting vulnerable, takes {Rules.vulMulp}x ({dmg}) damage!");
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
        => $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {level}\nHp : {Hp.point}\tStrength : {Sol}\tDexterity : {Lun}\tWisdom : {Con}";
    private int GetRandomStat(int stat)
        => rnd.Next(1, stat + 1);
    public bool IsAlive
        => Hp.IsAlive;
    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        return MapSymb.invisible;
    }
    public virtual void OnTurnEnd()
    {
        UnRest();
    }
}