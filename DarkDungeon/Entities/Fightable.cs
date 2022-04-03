namespace Entities;
public class Fightable : Moveable
{
    public ClassName ClassName { get; private set; }
    public Inventory<Card?> Hand { get; private set; }
    public readonly Tokens tokens;
    public GamePoint Hp { get; set; }
    public virtual Moveable? Target { get; protected set; }
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
            if (card.stat == StatName.Sol)
            {
                stance.stance = global::Stance.Offence;
                stance.amount += card.value;
            }
            else return;
        }
        if (!card.isOffence)
        {
            if (card.stat == StatName.Sol || card.stat == StatName.Lun)
            {
                stance.stance = global::Stance.Defence;
                stance.amount += card.value;
            }
            else return;
        }
        Hand.Delete(card);
    }
    public int tempCharge;
    public void SelectSkill(Item item, int index)
    {
        Skill? selected = item.skills[index];
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = SetStance(token, selected.statName);
            if (selected.statName == StatName.Con) tempCharge += amount;
            if(tempCharge > 0) IO.rk($"{Name}은 {selected.OnUseOutput} ({amount}+{tempCharge})");
            else IO.rk($"{Name}은 {selected.OnUseOutput} ({amount})");
        }
        else
        {
            IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
        }
    }
    public int SetStance(TokenType token, StatName statName)
    {
        stance.stance = token.ToStance();
        int amount = stat.GetRandom(statName);
        stance.amount += amount;
        return amount;
    }
    public void TryAttack()
    {
        if (!(Target is Fightable fight)) return;
        if (stance.stance == global::Stance.Offence)
        {
            if (tempCharge > 0)
            {
                stance.amount += tempCharge;
                tempCharge = 0;
            }
            fight.TryDodge(stance.amount);
        }
        else if (fight.stance.stance == global::Stance.Defence) fight.TryDodge(0);
    }
    private void TryDodge(int damage)
    {
        if (stance.stance == global::Stance.Defence)
        {
            damage -= stance.amount;
        }
        else if (damage > 0 && stance.stance == global::Stance.Charge)
        {
            IO.pr($"{Name}은 무방비 상태로 쉬고 있었다! ({damage})x{Rules.vulMulp}");
            damage = GetVulDmg(damage);
        }
        Hp -= damage;
    }
    public static int GetVulDmg(int damage)
    {
        return (int)MathF.Round(damage * Rules.vulMulp);
    }

    public void Rest(TokenType tokenType, int discardIndex = -1)
    {
        IO.pr($"{Name}은 잠시 숨을 골랐다.");
        if (tokens.IsFull)
        {
            if (discardIndex != -1) tokens.RemoveAt(discardIndex);
            else tokens.RemoveAt(tokens.Count - 1);
        }
        stance.Set(global::Stance.Charge, default);
        tokens.Add(tokenType);
    }
    public virtual void OnBeforeTurnEnd()
    {
        TryAttack();
    }
    public void OnTurnEnd()
    {
        UpdateTarget();
        stance.Reset();
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
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {stat[StatName.Sol]}\tLun : {stat[StatName.Lun]}\tCon : {stat[StatName.Con]}";
    public override char ToChar()
    {
        if (IsAlive) return base.ToChar();
        else return 'x';
    }
    public void UpdateTarget()
    {
        Map.Current.MoveablePositions.TryGet(Pos.FrontIndex, out Moveable? mov);
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
    => p1.stat[StatName.Lun] >= p2.stat[StatName.Lun];
}
