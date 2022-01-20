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
    public void TryAttack()
    {
        if (!IsAlive) return;
        if (Target is null) return;

        if (Atk > 0)
        {
            string atkString = $"{Name} attacks with {Atk} damage.";
            if (Star > 0)
            {
                int tempStar = Star;
                Atk += tempStar;
                atkString += $"..and {tempStar} more damage! (total {Atk})";
            }
            IO.pr(atkString);
        }

        if (Atk > 0)
        {
            Target.TakeDamage(Atk);
        }
        if (Atk <= 0 && Target.Def > 0) IO.pr($"But {Target.Name} did not attack...");
    }
    private void TakeDamage(int damage)
    {
        if (IsResting)
        {
            damage = (int)MathF.Round(damage * Rules.vulMulp);
            IO.pr($"{Name} is resting vulnerable, takes {Rules.vulMulp}x damage!");
        }
        else if (Def > 0)
        {
            string tempStr = $"{Name} defences {Def} damage.";
            if (Star > 0)
            {
                int tempStar = Star;
                Def += tempStar;
                tempStr += $"..and {tempStar} more damage! (total {Def})";
            }
            IO.pr(tempStr);
        }
        damage -= Def;

        Hp.Take(damage);
        if (IsAlive) IO.pr($"=> {Name} takes {damage} damage. {Hp.point}");
    }
    public virtual void Rest()
    {
        /*if(Map.Current.IsVisible((Moveable)this))*/
        IO.pr($"{Name} is resting a turn.");
        IsResting = true;
    }

    public virtual void OnTurnEnd()
    {
        TryAttack();
        IsResting = false;
        Atk = 0;
        Def = 0;
        Star = 0;
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

}