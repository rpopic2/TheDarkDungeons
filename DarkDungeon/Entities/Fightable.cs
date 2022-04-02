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
                stance.stance = Stance.Offence;
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
    public void SelectSkillAndUse(NewItem item, int index)
    {
        

        Skill? selected = item.skills[index];
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = SetStance(token, selected.stats);
            IO.rk(selected.OnUseOutput + $"({amount})");
        }
        else
        {
            IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
        }
    }
    public int SetStance(TokenType token, Stats stats)
    {
        stance.stance = token.ToStance();
        int amount = rnd.Next(1, stat[stats]);
        stance.amount += amount;
        return amount;
    }
    public void TryAttack()
    {
        if (!(Target is Fightable fight)) return;
        if (stance.stance == Stance.Offence)
        {
            // string tempString = $"{Name}은 주먹으로 상대를 힘껏 때렸다. ({stance.amount})";
            // TryUseStar();
            // IO.rk(tempString);
            fight.TryDodge(stance.amount);
        }
        else if (fight.stance.stance == Stance.Defence) fight.TryDodge(0);
    }
    private void TryDodge(int damage)
    {
        if (stance.stance == Stance.Defence)
        {
            //string tempStr = $"{Name}는 굴러서 적의 공격을 피했다. ({stance.amount}).";
            //TryUseStar();
            // if (damage <= 0)
            // {
            //     IO.rk(tempStr += "..but oppenent did not attack...");
            //     return;
            // }
            // IO.rk(tempStr);
            damage -= stance.amount;
        }
        else if (damage > 0 && IsResting)
        {
            IO.rk($"{Name}은 무방비 상태로 쉬고 있었다! {Rules.vulMulp}x({damage})");
            damage = (int)MathF.Round(damage * Rules.vulMulp);
        }
        Hp -= damage;
        if (damage <= 0) IO.rk($"{Name} completely dodges. {Hp}");
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
        stance.stance = default;
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        IO.pr($"{Name}가 죽었다.", false, true);
        Map.Current.UpdateMoveable(this);
    }
    protected void OnHeal(object? sender, PointArgs e) => IO.pr($"{Name} restored {e.Amount} hp. {Hp}", true);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.rk($"{Name}은 {e.Amount}의 피해를 입었다. {Hp}", true);
    }

    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {stat[Stats.Sol]}\tLun : {stat[Stats.Lun]}\tCon : {stat[Stats.Con]}";
    public override char ToChar()
    {
        if (IsAlive) return base.ToChar();
        else return 'x';
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
