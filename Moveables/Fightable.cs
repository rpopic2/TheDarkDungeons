public class Fightable : Moveable
{
    public ClassName ClassName { get; private set; }
    public Inventory<Card?> Hand { get; private set; }
    public GamePoint Hp { get; set; }
    public virtual Moveable? Target { get; protected set; }
    private int star;
    public bool IsResting => stance.stance == Stance.Rest;
    public bool IsAlive => !Hp.IsMin;
    public Fightable(string name, ClassName className, int level, int sol, int lun, int con, int maxHp, int cap) : base(level, sol, lun, con, name)
    {
        ClassName = className;
        Hand = new Inventory<Card?>(cap, "Hand");
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
        Program.OnTurnEnd += new EventHandler(OnTurnEnd);
    }
    public virtual Card? SelectCard() => Hand.GetFirst();
    public virtual void PickupCard(Card card, int index)
    {
        Hand[index] = card;
    }
    public void UseCard(int index)
    {
        if (Target is null) return;
        if (Hand[index] is Card card)
        {
            if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced.");
            _UseCard(card);
        }
    }
    public void UseCard(Card card)
    {
        if (Target is null) return;
        if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced by ." + card.Con);
        _UseCard(card);
    }
    protected void _UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case CardStance.Attack:
                stance.stance = Stance.Attack;
                stance.amount += card.Sol;
                break;
            case CardStance.Dodge:
                stance.stance = Stance.Dodge;
                stance.amount += card.Lun;
                break;
            case CardStance.Star:
                star = card.Con;
                break;
        }
    }
    public void TryAttack()
    {
        if (!(Target is Fightable fight)) return;
        if (stance.stance == Stance.Attack)
        {
            string tempString = $"{Name} attacks with {stance.amount} damage.";
            TryUseStar();
            IO.pr(tempString);
            fight.TryDodge(stance.amount);
        }
        else if (fight.stance.stance == Stance.Dodge) fight.TryDodge(0);
    }
    private void TryDodge(int damage)
    {
        if (stance.stance == Stance.Dodge)
        {
            string tempStr = $"{Name} dodges {stance.amount} damage.";
            TryUseStar();
            if (damage <= 0)
            {
                IO.pr(tempStr += "..but oppenent did not attack...");
                return;
            }
            IO.pr(tempStr);
            damage -= stance.amount;
        }
        else if (damage > 0 && IsResting)
        {
            damage = (int)MathF.Round(damage * Rules.vulMulp);
            IO.pr($"{Name} is resting vulnerable, takes {Rules.vulMulp}x damage! (total {damage})");
        }
        Hp -= damage;
        if (damage <= 0) IO.pr($"{Name} completely dodges. {Hp}", true);
    }
    private void TryUseStar()
    {
        if (star <= 0) return;
        stance.amount += star;
        IO.pr($"{star} more damage... (total {stance.amount})");
        star = 0;
    }
    public virtual void Rest()
    {
        if (this is Player)
            IO.pr($"{Name} is resting a turn.");
        stance = new(Stance.Rest, default);
    }

    public virtual void OnTurnEnd(object? sender, EventArgs e)
    {
        UpdateTarget();
        stance = new(default, default);
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        IO.pr($"{Name} died.", false, true);
        Map.Current.UpdateMoveable(this);
    }
    protected void OnHeal(object? sender, PointArgs e) => IO.pr($"{Name} restored {e.Amount} hp. {Hp}", true);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.pr($"{Name} takes {e.Amount} damage. {Hp}", true);
    }

    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\tCap : {Hand.Cap}\tSol : {stat.sol}\tLun : {stat.lun}\tCon : {stat.con}";
    public override char ToChar()
    {
        if (IsAlive) return base.ToChar();
        else return MapSymb.Empty;
    }
    public void UpdateTarget()
    {
        Map.Current.Moveables.TryGet(Pos.FrontIndex, out Moveable? mov);
        Target = mov;
    }

    public static bool IsFirst(Fightable p1, Fightable p2)
    => p1.stat.lun >= p2.stat.lun;
}