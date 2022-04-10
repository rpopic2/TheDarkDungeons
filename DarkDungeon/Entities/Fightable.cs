namespace Entities;
public partial class Fightable
{
    public readonly string Name;
    public int Level { get; protected set; }
    public GamePoint Hp { get; protected set; }
    public Tokens Toks { get; private set; }
    protected readonly Stat Stat;
    public int Sight { get; private set; } = 1;
    public Position Pos { get; protected set; }

    public Inventory Inven { get; private set; }

    public StanceInfo Stance { get; protected set; } = new(default, default);
    public virtual Fightable? Target { get; protected set; }

    public Action<Fightable>? currentBehav;
    public Item? currentItem;
    public Fightable? lastHit { get; private set; }
    public Action<Fightable> passives = (p) => { };
    public Fightable(string name, int level, int sol, int lun, int con, int maxHp, int cap, Position pos)
    {
        Pos = pos;
        this.Level = level;
        Name = name;
        Stat = new();
        Stat[StatName.Sol] = sol;
        Stat[StatName.Lun] = lun;
        Stat[StatName.Con] = con;
        Inven = new((Fightable)this, "(맨손)");
        Toks = new(cap);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
    }
    public bool IsAlive => !Hp.IsMin;
    protected bool Move(int x, out char obj)
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
                Stance.Set(StanceName.None, default);
            }
        }
        return canGo;
    }
    public void TryAttack()
    {
        if (Stance.Stance == StanceName.Offence && currentBehav is not null)
        {
            currentBehav.Invoke((Fightable)this);
            currentBehav = null;
            return;
        }
        if (Target is not Fightable tar) return;
        if (tar.Stance.Stance == StanceName.Defence) tar.Dodge(0);
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
                    Stance.AddAmount(magicCharge);
                    Inven.GetMeta(currentItem!).magicCharge = 0;
                }
                lastHit = hit;
                hit.Dodge(Stance.Amount);
                break;
            }
        }
    }

    public void TryDefence()
    {
        if (Stance.Stance == StanceName.Defence && currentBehav is not null)
        {
            currentBehav.Invoke((Fightable)this);
            currentBehav = null;
            return;
        }
    }
    public void Dodge() => Dodge(0);
    public void Dodge(int damage)
    {
        if (Stance.Stance == StanceName.Defence)
        {
            damage -= Stance.Amount;
        }
        else if (damage > 0 && Stance.Stance == StanceName.Charge)
        {
            IO.pr($"{Name}은 약점이 드러나 있었다! ({damage})x{Rules.vulMulp}");
            damage = damage.ToVul();
        }
        Hp -= damage;
    }
    public void OnTurnEnd()
    {
        UpdateTarget();
        Stance.Reset();
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
    public void UpdateTarget()
    {
        Map.Current.FightablePositions.TryGet(Pos.GetFrontIndex(1), out Fightable? mov);
        if (mov is Fightable f && this.IsEnemy(f)) Target = mov;
        else Target = null;
    }
    protected void PickupToken(TokenType tokenType, int discardIndex = -1)
    {
        if (Toks.IsFull)
        {
            if (discardIndex != -1) Toks.RemoveAt(discardIndex);
            else Toks.RemoveAt(Toks.Count - 1);
        }
        Toks.Add(tokenType);
    }
    public int SetStance(TokenType token, StatName statName)
    {
        int amount = Stat.GetRandom(statName);
        Stance.Set(token.ToStance(), amount);
        return amount;
    }
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
            Stance.Set(StanceName.Charge, default);
            nonToken.behaviour.Invoke(this, x, y);
        }
    }
    private void SelectSkill(Item item, Skill selected)
    {
        TokenType? tokenTry = Toks.TryUse(selected.TokenType);
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
    public virtual char ToChar() => Name.ToLower()[0];
    public override string ToString() => $"Name : {Name}\tLevel : {Level}\nHp : {Hp}\t{Toks}\tSol : {Stat[StatName.Sol]}\tLun : {Stat[StatName.Lun]}\tCon : {Stat[StatName.Con]}";
}
