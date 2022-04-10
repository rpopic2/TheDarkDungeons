namespace Entities;
public class Fightable : Moveable
{
    protected Stat stat;
    public Inventory Inven { get; private set; }
    public readonly Tokens tokens;
    public GamePoint Hp { get; set; }
    public virtual Moveable? Target { get; protected set; }
    public bool IsAlive => !Hp.IsMin;
    public int sight = 1;
    public Action<Inventoriable>? currentBehav;
    public Item? currentItem;
    public Fightable(string name, int level, int sol, int lun, int con, int maxHp, int cap, Position pos) : base(level, name, pos)
    {
        stat = new();
        stat[StatName.Sol] = sol;
        stat[StatName.Lun] = lun;
        stat[StatName.Con] = con;
        Inven = new((Inventoriable)this, "(맨손)");
        tokens = new(cap);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
    }
    public int GetStat(StatName statName)
    {
        return stat[statName];
    }
    public int SetStance(TokenType token, StatName statName)
    {
        int amount = stat.GetRandom(statName);
        stance.Set(token.ToStance(), amount);
        return amount;
    }
    public void TryAttack()
    {
        if (stance.Stance == StanceName.Offence && currentBehav is not null)
        {
            currentBehav.Invoke((Inventoriable)this);
            currentBehav = null;
            return;
        }
        if (Target is not Fightable tar) return;
        if (tar.stance.Stance == StanceName.Defence) tar.Dodge(0);
    }
    public Fightable lastHit { get; private set; }
    public void Throw(int range)
    {
        for (int i = 0; i < range; i++)
        {
            Map.Current.MoveablePositions.TryGet(Pos.GetFrontIndex(i + 1), out Moveable? mov);
            if (mov is Fightable hit)
            {
                int magicCharge = Inven.GetMeta(currentItem!).magicCharge;
                if (magicCharge > 0)
                {
                    stance.AddAmount(magicCharge);
                    Inven.GetMeta(currentItem!).magicCharge = 0;
                }
                lastHit = hit;
                hit.Dodge(stance.Amount);
                break;
            }
        }
    }

    public void TryDefence()
    {
        if (stance.Stance == StanceName.Defence && currentBehav is not null)
        {
            currentBehav.Invoke((Inventoriable)this);
            currentBehav = null;
            return;
        }
    }
    public void Dodge() => Dodge(0);
    public void Dodge(int damage)
    {
        if (stance.Stance == StanceName.Defence)
        {
            damage -= stance.Amount;
        }
        else if (damage > 0 && stance.Stance == StanceName.Charge)
        {
            IO.pr($"{Name}은 약점이 드러나 있었다! ({damage})x{Rules.vulMulp}");
            damage = GetVulDmg(damage);
        }
        Hp -= damage;
    }
    public static int GetVulDmg(int damage)
    {
        int result = (int)MathF.Round(damage * Rules.vulMulp);
        return result == damage ? ++result : result;
    }
    protected void _PickupToken(TokenType tokenType, int discardIndex = -1)
    {
        if (tokens.IsFull)
        {
            if (discardIndex != -1) tokens.RemoveAt(discardIndex);
            else tokens.RemoveAt(tokens.Count - 1);
        }
        tokens.Add(tokenType);
    }
    public virtual void OnBeforeFight()
    {
        //TryAttack();
    }
    public void OnTurnEnd()
    {
        UpdateTarget();
        stance.Reset();
        currentBehav = null;
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        IO.pr($"{Name}가 죽었다.", __.newline);
        Map.Current.UpdateMoveable(this);
    }
    protected void OnHeal(object? sender, PointArgs e) => IO.rk($"{Name}은 {e.Amount}의 hp를 회복했다. {Hp}", __.emphasis);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.rk($"{Name}은 {e.Amount}의 피해를 입었다. {Hp}", __.emphasis);
    }

    public override string ToString() =>
        $"Name : {Name}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {stat[StatName.Sol]}\tLun : {stat[StatName.Lun]}\tCon : {stat[StatName.Con]}";
    public override char ToChar()
    {
        if (IsAlive) return base.ToChar();
        else return 'x';
    }
    public void UpdateTarget()
    {
        Map.Current.MoveablePositions.TryGet(Pos.GetFrontIndex(1), out Moveable? mov);
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
