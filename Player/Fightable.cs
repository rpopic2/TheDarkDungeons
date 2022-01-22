public class Fightable : Mass
{
    public string Name { get; private set; }
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public int Cap { get; protected set; }
    public GamePoint Hp { get; protected set; }
    protected Random rnd = new Random();
    public virtual Fightable? Target { get; protected set; }
    public bool IsResting => stance.stance == Stance.Rest;
    protected (Stance stance, int amount) stance = (default, default);
    public (Stance stance, int amount) TurnStance { get => stance; }
    private int star;

    public Fightable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con)
    {
        Name = name;
        ClassName = className;
        if (cap <= 0 || cap > 10) throw new ArgumentException("cap is out of index");
        Cap = cap;
        Hand = new Hand(this);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Sol = sol;
        Lun = lun;
        Con = con;
        level = lv;
    }
    public override Card Draw()
        => new Card(GetRandomStat(Sol), GetRandomStat(Lun), GetRandomStat(Con), CardStance.Attack);
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        IO.pr($"{Name} died. {Hp}", true, true);
    }
    public virtual void UseCard(int index)
    {
        Card card = Hand[index] ?? throw new ArgumentNullException(nameof(card), "Cannot use card in null index");
        if (Target is null) return;
        if (card.Stance == CardStance.Star && star <= 0)
        {
            IO.pr("Next move will be reinforced.");
        }
        _UseCard(card);
    }
    protected void _UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case CardStance.Attack:
                stance = (Stance.Attack, card.Sol);
                break;
            case CardStance.Dodge:
                stance = (Stance.Dodge, card.Sol);
                break;
            case CardStance.Star:
                star = card.Con;
                break;
        }
    }
    public void TryAttack()
    {
        if (!IsAlive) return;
        if (Target is null) return;
        if (stance.stance == Stance.Attack)
        {
            string atkString = $"{Name} attacks with {stance.amount} damage.";
            if (star > 0)
            {
                stance.amount += star;
                atkString += $"..and {star} more damage! (total {stance.amount})";
                star = 0;
            }
            IO.pr(atkString);
            Target.TryDodge(stance.amount);
        }
        else if (Target.stance.stance == Stance.Dodge) Target.TryDodge(0);
    }
    private void TryDodge(int damage)
    {

        if (stance.stance == Stance.Dodge)
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
            damage -= stance.amount;
        }
        else if (damage > 0 && IsResting)
        {
            damage = (int)MathF.Round(damage * Rules.vulMulp);
            IO.pr($"{Name} is resting vulnerable, takes {Rules.vulMulp}x damage!");
        }
        Hp -= damage;
        if (damage <= 0) IO.pr($"{Name} completely dodges. {Hp}", true);
        else if (IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp}", true);
    }
    public virtual void Rest()
    {
        /*if(Map.Current.IsVisible((Moveable)this))*/
        IO.pr($"{Name} is resting a turn.");
        stance = (Stance.Rest, default);
    }

    public virtual void OnTurnEnd()
    {
        stance = (default, default);
    }
    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {level}\nHp : {Hp}\tSol : {Sol}\tLun : {Lun}\tCon : {Con}";
    private int GetRandomStat(int stat) =>
        rnd.Next(1, stat + 1);
    public bool IsAlive => !Hp.IsMin;
    public bool DidPrint => TurnStance.stance != Stance.Move && TurnStance.stance != Stance.None;

    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        return MapSymb.invisible;
    }
}