public class Fightable : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; protected set; }
    public GamePoint Hp { get; protected set; }
    protected Random rnd = new Random();
    public virtual Fightable? Target { get; protected set; }
    public bool IsResting => stance.stance == FightStance.Rest;
    public (FightStance stance, int amount) stance = (default, default);
    public int star;

    public Fightable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        if (cap <= 0 || cap > 10) throw new ArgumentException("cap is out of index");
        Cap = cap;
        Hand = new Hand(this);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving, () => OnDeath());
        Sol = sol;
        Lun = lun;
        Con = con;
        level = lv;
    }
    public override Card Draw()
        => new Card(GetRandomStat(Sol), GetRandomStat(Lun), GetRandomStat(Con), Stance.Attack);
    protected virtual void OnDeath()
    {
        IO.pr($"{Name} died. {Hp}", true, true);
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
        if (card.Stance == Stance.Star && star <= 0)
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
                stance = (FightStance.Attack, card.Sol);
                break;
            case Stance.Dodge:
                stance = (FightStance.Dodge, card.Sol);
                break;
            case Stance.Star:
                star = card.Con;
                break;
        }
    }
    public void TryAttack()
    {
        if (!IsAlive) return;
        if (Target is null) return;
        if (stance.stance == FightStance.Attack)
        {
            string atkString = $"{Name} attacks with {stance.amount} damage.";
            if (star > 0)
            {
                stance.amount += star;
                atkString += $"..and {star} more damage! (total {stance.amount})";
                star = 0;
            }
            IO.pr(atkString);
        }

        Target.TryDefence(stance.stance == FightStance.Attack ? stance.amount : 0);
        //if (tempAtk <= 0 && Target.tempDef > 0) IO.pr($"But {Target.Name} did not attack...");
    }
    private void TryDefence(int damage)
    {

        if (stance.stance == FightStance.Dodge)
        {
            string tempStr = $"{Name} dodges {stance.amount} damage.";
            if (star > 0)
            {
                stance.amount += star;
                tempStr += $"..and {star} more damage! (total {stance.amount})";
                star = 0;
            }
            if (damage <= 0) tempStr += "..but oppenent did not attack...";
            IO.pr(tempStr);
        }
        else if (damage > 0 && IsResting)
        {
            damage = (int)MathF.Round(damage * Rules.vulMulp);
            IO.pr($"{Name} is resting vulnerable, takes {Rules.vulMulp}x damage!");
        }
        if (damage > 0) TakeDamage(damage - stance.amount);
    }
    private void TakeDamage(int damage)
    {
        Hp -= damage;
        if (IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp}", true);
    }
    public virtual void Rest()
    {
        /*if(Map.Current.IsVisible((Moveable)this))*/
        IO.pr($"{Name} is resting a turn.");
        stance = (FightStance.Rest, 0);
    }

    public virtual void OnTurnEnd()
    {
        stance = (default, default);
    }
    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {level}\nHp : {Hp}\tStrength : {Sol}\tDexterity : {Lun}\tWisdom : {Con}";
    private int GetRandomStat(int stat) =>
        rnd.Next(1, stat + 1);
    public bool IsAlive => !Hp.IsMin;

    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        return MapSymb.invisible;
    }

}