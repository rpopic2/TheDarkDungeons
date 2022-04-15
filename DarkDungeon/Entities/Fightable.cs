namespace Entities;
public partial class Fightable
{
    public readonly string Name;
    public int Level { get; protected set; }
    public GamePoint Hp { get; protected set; }
    public Tokens tokens { get; private set; }
    protected readonly Stat Stat;
    public int Sight { get; private set; } = 1;
    public Position Pos { get; protected set; }

    public Inventory Inven { get; private set; }

    public StanceInfo Stance { get; protected set; } = new();
    public virtual Fightable? FrontFightable => Map.Current.GetFightableAt(Pos.Front(1));
    private Fightable? lastHit { get; set; }
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
        tokens = new(cap);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnIncrease += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDecrease += new EventHandler<PointArgs>(OnDamaged);
    }
    public bool IsAlive => !Hp.IsMin;
    protected Map _currentMap => Map.Current;
    protected void PickupToken(TokenType tokenType, int discardIndex = -1)
    {
        if (tokens.IsFull)
        {
            if (discardIndex == -1) discardIndex = tokens.Count - 1;
            tokens.RemoveAt(discardIndex);
        }
        tokens.Add(tokenType);
    }
    protected void SelectBehaviour(Item item, int index)
    {
        if (Stance.CurrentBehav != null) throw new Exception("스탠스가 None이 아닌데 새 동작을 선택했습니다. 한 턴에 두 동작을 할 수 없습니다.");
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Skill skill) SelectSkill(item, skill);
        else if (behaviour is Consume consume) SelectConsume(item, consume);
        else if (behaviour is Passive || behaviour is WearEffect)
        {
            IO.rk(behaviour.OnUseOutput);
            IO.DrawScreen();
        }
        else throw new Exception("등록되지 않은 행동 종류입니다.");
    }
    //requirements : stance 정하기, rk로 프린트하기.
    ///<summary>[0] : 이동(Fightable f, int amout, int(Facing) facing) | [1] : 숨고르기(Fightable f, int(TokenType) tokenType, int discardIndex) | [2] : 상호작용
    ///</summary>
    public void SelectBasicBehaviour(int index, int x, int y)
    {
        IBehaviour behaviour = basicActions.skills[index];
        if (behaviour is NonTokenSkill nonToken)
        {
            string output = behaviour.OnUseOutput;
            if (output != string.Empty) IO.pr(Name + output);
            Stance.Set(basicActions, nonToken, x, y);
        }
    }
    private void SelectSkill(Item item, Skill selected)
    {
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = Stat.GetRandom(selected.statName);
            Stance.Set(item, selected, amount);
            string useOutput = $"{Name} {selected.OnUseOutput} ({amount})";
            int mcharge = Inven.GetMeta(item).magicCharge;
            if (mcharge > 0) useOutput += ($"+({mcharge})");
            if (selected.statName == StatName.Con) Inven.GetMeta(item).magicCharge += amount;
            IO.rk(useOutput);
        }
        else IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
    }
    private void SelectConsume(Item item, Consume consume)
    {
        Stance.Set(item, consume);
        IO.rk($"{Name} {consume.OnUseOutput}");
        Inven.Consume(item);
    }
    public void InvokeBehaviour()
    {
        if (Stance.CurrentBehav is not IBehaviour behav) return;
        if (behav is NonTokenSkill nonTokenSkill) nonTokenSkill.NonTokenBehav.Invoke(this, Stance.Amount, Stance.Amount2);
        else behav.Behaviour.Invoke(this);
    }
    private void Throw(int range)
    {
        DamageType damageType = default;
        if (Stance.CurrentBehav is Skill skill) damageType = skill.damageType;
        Fightable? mov = _currentMap.RayCast(Pos, range);
        if (mov is Fightable hit)
        {
            int magicCharge = Inven.GetMeta(Stance.CurrentItem!).magicCharge;
            if (magicCharge > 0)
            {
                Stance.AddAmount(magicCharge);
                Inven.GetMeta(Stance.CurrentItem!).magicCharge = 0;
            }
            lastHit = hit;
            hit.Dodge(Stance.Amount, damageType);
        }
    }
    private void Dodge(int damage, DamageType damageType)
    {
        bool isDefending = Stance.CurrentBehav?.Stance == StanceName.Defence;
        if (isDefending)
        {
            DamageType defenceType = default;
            if (Stance.CurrentBehav is Skill skill) defenceType = skill.damageType;
            if (defenceType == DamageType.Slash && damageType == DamageType.Slash)
            {
                IO.pr($"{Name}은 적의 공격을 효과적으로 막아냈다. 원래 피해 : {damage}");
                damage = damage.ToUnVul();
            }
            damage -= Stance.Amount;
        }
        else if (damage > 0 && Stance.CurrentBehav?.Stance == StanceName.Charge)
        {
            IO.pr($"{Name}은 약점이 드러나 있었다! ({damage})x{Rules.vulMulp}");
            damage = damage.ToVul();
        }
        if(damage <= 0) IO.rk($"{Name}은 공격을 완전히 막아냈다!");
        Hp -= damage;
    }
    protected void Move(Position x)
    {
        Position newPos = Pos + x;
        Map currentMap = Map.Current;
        bool canGo = true;
        if (newPos.x != Pos.x)
        {
            bool existsTile = currentMap.Tiles.TryGet(newPos.x, out _);
            bool obstructed = currentMap.FightablePositions.TryGet(newPos.x, out _);
            canGo = existsTile && !obstructed;
        }
        if (canGo)
        {
            Pos = newPos;
            currentMap.UpdateFightable(this);
        }
    }
    protected virtual void Interact() { }
    public virtual void OnTurnEnd()
    {
        Stance.Reset();
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
    public virtual char ToChar() => Name.ToLower()[0];
    public override string ToString() => $"Name : {Name}\tLevel : {Level}\nHp : {Hp}\t{tokens}\tSol : {Stat[StatName.Sol]}\tLun : {Stat[StatName.Lun]}\tCon : {Stat[StatName.Con]}";
}
