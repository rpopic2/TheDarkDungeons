namespace Entities;
public class Fightable : Moveable
{
    public ClassName ClassName { get; private set; }
    public Inventory<Card?> Hand { get; private set; }
    public Tokens tokens;
    public GamePoint Hp { get; set; }
    public virtual Moveable? Target { get; protected set; }
    private int star;
    public bool IsResting => stance.stance == Stance.Rest;
    public bool IsAlive => !Hp.IsMin;
    public Fightable(string name, ClassName className, int level, int sol, int lun, int con, int maxHp, int cap) : base(level, sol, lun, con, name)
    {
        ClassName = className;
        Hand = new Inventory<Card?>(cap, "Hand");
        tokens = new(cap);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
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
            //if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced.");
            _UseCard(card);
        }
    }
    public void UseCard(Card card)
    {
        if (Target is null) return;
        //if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced by ." + card.Con);
        _UseCard(card);
    }
    protected void _UseCard(Card card)
    {
        if (card.isOffence)
        {
            if (card.stat == Stats.Sol)
            {
                stance.stance = Stance.Attack;
                stance.amount += card.value;
            }
            else return;
        }
        if (!card.isOffence)
        {
            if (card.stat == Stats.Sol || card.stat == Stats.Lun)
            {
                stance.stance = Stance.Defence;
                stance.amount += card.value;
            }
            else return;
        }
        Hand.Delete(card);
    }
    public void UseToken(TokenType token)
    {
        if (token == TokenType.Offence)
        {
            stance.stance = Stance.Attack;
            stance.amount += rnd.Next(1, stat[Stats.Sol]);
        }
        else if (token == TokenType.Defence)
        {
            stance.stance = Stance.Defence;
            stance.amount += rnd.Next(1, stat[Stats.Sol]);
        }
        tokens.Remove(token);
    }
    public void TryAttack()
    {
        if (!(Target is Fightable fight)) return;
        if (stance.stance == Stance.Attack)
        {
            string tempString = $"{Name}은 주먹으로 상대를 힘껏 떄렸다. ({stance.amount})";
            TryUseStar();
            IO.pr(tempString);
            fight.TryDodge(stance.amount);
        }
        else if (fight.stance.stance == Stance.Defence) fight.TryDodge(0);
    }
    private void TryDodge(int damage)
    {
        if (stance.stance == Stance.Defence)
        {
            string tempStr = $"{Name}는 굴러서 적의 공격을 피 {stance.amount} damage.";
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
        if (Map.Current.IsVisible(this)) IO.pr($"{Name}은 잠시 숨을 골랐다.");
        stance = new(Stance.Rest, default);
    }
    public virtual void OnBeforeTurnEnd()
    {
        TryAttack();
    }
    public void OnTurnEnd()
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
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {stat[Stats.Sol]}\tLun : {stat[Stats.Lun]}\tCon : {stat[Stats.Con]}";
    public override char ToChar()
    {
        if (IsAlive) return base.ToChar();
        else return 'x';
    }
    public string GetHandPrivate()
    {
        string result = $"Hand : ";
        foreach (Card? item in Hand.Content)
        {
            if (item is not Card card) result += Item.Empty;
            else result += card.ToStringPrivate();
        }
        return result;
    }
    public void UpdateTarget()
    {
        Map.Current.Moveables.TryGet(Pos.FrontIndex, out Moveable? mov);
        if (mov is Fightable f && isEnemy(this, f)) Target = mov;
        else Target = null;
    }
    private bool isEnemy(Fightable p1, Fightable p2)
    {
        if (p1 is Player && p2 is Monster) return true;
        if (p1 is Monster && p2 is Player) return true;
        return false;
    }

    public static bool IsFirst(Fightable p1, Fightable p2)
    => p1.stat[Stats.Lun] >= p2.stat[Stats.Lun];
}