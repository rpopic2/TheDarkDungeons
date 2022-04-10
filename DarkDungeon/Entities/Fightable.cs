namespace Entities;
public partial class Fightable
{
    protected Stat stat;
    public Inventory Inven { get; private set; }
    public readonly Tokens tokens;
    public GamePoint Hp { get; set; }
    public virtual Fightable? Target { get; protected set; }
    public bool IsAlive => !Hp.IsMin;
    public int sight = 1;
    public Action<Fightable>? currentBehav;
    public Item? currentItem;
    public Fightable? lastHit { get; private set; }
    public Action<Fightable> passives = (p) => { };
    public int Level { get; protected set; }
    public readonly string Name;
    protected StanceInfo stance = new(default, default);

    public Position Pos { get; set; }

    public StanceInfo Stance => stance;
    protected virtual void Move(int x) => Move(x, out char obj);
    public virtual char ToChar() => Name.ToLower()[0];
    public Fightable(string name, int level, int sol, int lun, int con, int maxHp, int cap, Position pos)
    {
        Pos = pos;
        this.Level = level;
        Name = name;
        stat = new();
        stat[StatName.Sol] = sol;
        stat[StatName.Lun] = lun;
        stat[StatName.Con] = con;
        Inven = new((Fightable)this, "(맨손)");
        tokens = new(cap);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
    }
    protected virtual bool Move(int x, out char obj)
    {
        Map current = Map.Current;
        Position newPos = Pos + x;
        bool existsTile = current.Tiles.TryGet(newPos.x, out obj);
        if (Pos.facing != newPos.facing)
        {
            Pos = !Pos;
            return true;
        }
        if (existsTile && current.steppables[newPos.x] is ISteppable step) obj = step.ToChar();
        bool obstructed = current.FightablePositions.TryGet(newPos.x, out Fightable? mov);
        bool canGo = existsTile && !obstructed;
        if (canGo)
        {
            Pos = newPos;
            current.UpdateFightable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                Pos = !Pos;
                return true;
            }
            else
            {
                stance.Set(StanceName.None, default);
            }
        }
        return canGo;
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
            currentBehav.Invoke((Fightable)this);
            currentBehav = null;
            return;
        }
        if (Target is not Fightable tar) return;
        if (tar.stance.Stance == StanceName.Defence) tar.Dodge(0);
    }
    public void Throw(int range)
    {
        for (int i = 0; i < range; i++)
        {
            Map.Current.FightablePositions.TryGet(Pos.GetFrontIndex(i + 1), out Fightable? mov);
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
            currentBehav.Invoke((Fightable)this);
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
    public void OnTurnEnd()
    {
        UpdateTarget();
        stance.Reset();
        currentBehav = null;
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        IO.pr($"{Name}가 죽었다.", __.newline);
        Map.Current.UpdateFightable(this);
    }
    protected void OnHeal(object? sender, PointArgs e) => IO.rk($"{Name}은 {e.Amount}의 hp를 회복했다. {Hp}", __.emphasis);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.rk($"{Name}은 {e.Amount}의 피해를 입었다. {Hp}", __.emphasis);
    }

    public override string ToString() =>
        $"Name : {Name}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {stat[StatName.Sol]}\tLun : {stat[StatName.Lun]}\tCon : {stat[StatName.Con]}";
    public void UpdateTarget()
    {
        Map.Current.FightablePositions.TryGet(Pos.GetFrontIndex(1), out Fightable? mov);
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
    public void SelectBehaviour(Item item, int index)
    {
        if (Stance.Stance != StanceName.None) throw new Exception("스탠스가 None이 아닌데 새 동작을 선택했습니다. 한 턴에 두 동작을 할 수 없습니다.");
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Skill skill) SelectSkill(item, skill);
        else if (behaviour is Consume consume) SelectConsume(item, consume);
        else if (behaviour is Passive || behaviour is WearEffect) IO.rk(behaviour.OnUseOutput);
        else throw new Exception("등록되지 않은 행동 종류입니다.");
    }
    //requirements : stance 정하기, rk로 프린트하기.
    public void SelectBasicBehaviour(int index, int x, int y)
    {
        IBehaviour behaviour = basicActions.skills[index];
        if (behaviour is NonTokenSkill nonToken)
        {
            string output = behaviour.OnUseOutput;
            if (output != string.Empty) IO.pr(Name + output);
            stance.Set(StanceName.Charge, default);
            nonToken.behaviour.Invoke(this, x, y);
        }
    }
    private void SelectSkill(Item item, Skill selected)
    {
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = SetStance(token, selected.statName);
            string s = $"{Name} {selected.OnUseOutput} ({amount})";
            int mcharge = Inven.GetMeta(item).magicCharge;
            if (mcharge > 0) s += ($"+({mcharge})");
            if (selected.statName == StatName.Con) Inven.GetMeta(item).magicCharge += amount;
            currentBehav = selected.behaviour;
            currentItem = item;
            IO.rk(s);
        }
        else
        {
            IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
        }
    }
    private void SelectConsume(Item item, Consume consume)
    {
        SetStance(TokenType.Charge, default);
        IO.rk($"{Name} {consume.OnUseOutput}");
        consume.behaviour.Invoke(this);
        Inven.Consume(item);
    }
}
