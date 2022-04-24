namespace Entities;
public partial class Fightable
{
    public readonly string Name;
    public int Level { get; protected set; }
    public GamePoint Energy { get; protected set; }
    public readonly Stat Stat;
    public int Sight { get; protected set; } = 1;
    public Position Pos { get; protected set; }

    public Inventory Inven { get; private set; }

    public FightableStatus Status { get; init; }
    public virtual Fightable? FrontFightable => Map.Current.GetFightableAt(Pos.Front(1));
    private Fightable? lastHit { get; set; }
    public Action<Fightable> passives = (p) => { };
    public Fightable(string name, int level, int sol, int lun, int con, int cap, Position pos)
    {
        Pos = pos;
        this.Level = level;
        Name = name;
        Stat = new(sol, lun, con);
        Inven = new((Fightable)this, " .(휴식)");
        Energy = new(cap, GamePointOption.Reserving);
        Status = new(this);
        GetHp().OnOverflow += new EventHandler(OnDeath);
        GetHp().OnIncrease += new EventHandler<PointArgs>(OnHeal);
        GetHp().OnDecrease += new EventHandler<PointArgs>(OnDamaged);
    }
    public GamePoint GetHp() => Stat.Hp;
    public bool IsAlive { get; private set; } = true;
    protected Map _currentMap => Map.Current;
    public void PickupToken(int amount)
    {
        Energy += amount;
    }
    public virtual void SelectAction() { }
    protected void SelectBehaviour(Item item, int index)
    {
        if (Status.CurrentBehav != null) throw new Exception("스탠스가 None이 아닌데 새 동작을 선택했습니다. 한 턴에 두 동작을 할 수 없습니다.");
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Skill skill) SelectSkill(item, skill);
        else if (behaviour is Charge charge) SelectCharge(item, charge);
        else if (behaviour is Consume consume) SelectConsume(item, consume);
        else if (behaviour is Passive || behaviour is WearEffect)
        {
            IO.rk(behaviour.OnUseOutput);
            IO.Redraw();
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
            if (output != string.Empty) IO.rk(Name + output);
            Status.Set(basicActions, nonToken, x, y);
        }
    }
    private void SelectSkill(Item item, Skill selected)
    {
        if (Energy.Cur > 0)
        {
            Energy -= 1;
            int amount = Stat.GetRandom(selected.statName);
            Status.Set(item, selected, amount);
            string useOutput = $"{Name} {selected.OnUseOutput} ({amount})";
            int mcharge = Inven.GetMeta(item).magicCharge;
            if (mcharge > 0) useOutput += ($"+^b({mcharge})^/");
            IO.rk(useOutput);
        }
        else IO.rk("기력이 없습니다.");
    }
    private void SelectCharge(Item item, Charge charge)
    {
        if (Energy.Cur > 0)
        {
            Energy -= 1;
            int amount = Stat.GetRandom(StatName.Con);
            Status.Set(item, charge, amount);
            IO.rk($"{Name}{charge.OnUseOutput} ^b({Status.Amount})^/");
        }
        else IO.rk("기력이 없습니다.");
    }
    private void SelectConsume(Item item, Consume consume)
    {
        Status.Set(item, consume);
        IO.rk($"{Name} {consume.OnUseOutput}");
        Inven.Consume(item);
    }
    public void OnTurn()
    {
        if (Status.CurrentBehav is not IBehaviour behav) throw new Exception($"턴이 흘렀는데도 {Name}이 아무 행동도 선택하지 않았습니다.");
        if (behav is NonTokenSkill nonTokenSkill) nonTokenSkill.NonTokenBehav.Invoke(this, Status.Amount, Status.Amount2);
        else behav.Behaviour.Invoke(this);
    }
    private void Attack(int range)
    {
        DamageType damageType = default;
        if (Status.CurrentBehav is Skill skill) damageType = skill.damageType;
        Fightable? mov = _currentMap.RayCast(Pos, range);
        if (mov is Fightable hit)
        {
            ItemMetaData metaData = Inven.GetMeta(Status.CurrentItem!);
            lastHit = hit;
            AttackMagicCharge(metaData);
            hit.Dodge(Status.Amount, damageType, this, metaData);
        }

        void AttackMagicCharge(ItemMetaData metaData)
        {
            int magicCharge = metaData.magicCharge;
            if (magicCharge > 0)
            {
                hit.Dodge(magicCharge, DamageType.Magic, this, metaData);
                Inven.GetMeta(Status.CurrentItem!).magicCharge = 0;
            }
        }
    }
    private void Throw(int range, Item item)
    {
        if (Inven.Contains(item))
        {
            IO.pr($"{item.Name}이 바람을 가르며 날아갔다.");
            Attack(range);
            Inven.Consume(item);
            lastHit?.Inven.Add(item!);
        }
        else
        {
            Status.Reset();
            IO.rk($"{item.Name}이 없다!");
        }
    }
    private void Dodge(int damage, DamageType damageType, Fightable attacker, ItemMetaData metaData)
    {
        if (damage < 0) throw new Exception("데미지는 0보다 작을 수 없습니다.");
        StanceName? stance = Status.CurrentBehav?.Stance;
        if (stance == StanceName.Defence) CalcDamageType(ref damage, damageType, attacker);
        else if (stance == StanceName.Charge)
        {
            IO.pr($"{Name}은 약점이 드러나 있었다! ({damage})x{Rules.vulMulp}");
            damage = damage.ToVul();
        }

        if (damage <= 0) IO.rk($"{Name}은 아무런 피해도 받지 않았다.");
        else if (metaData.isPoisoned) Status.SetPoison(2);
        Stat.Damage(damage);
    }
    private void Charge()
    {
        IO.pr("마법부여할 대상을 선택해 주십시오.");
        IO.sel(Inven, 0, out int index, out _, out _, out _);
        IO.del();
        if (Inven[index] is Item item) Charge(item);
    }
    private void Charge(Item item)
    {
        Inven.GetMeta(item).magicCharge += Status.Amount;
        IO.rk($"{item}에 마법부여를 하였다.");
    }
    private void PoisonItem(Item item)
    {
        Inven.GetMeta(item).isPoisoned = true;
        IO.rk($"{item}은 독으로 젖어 있다.");
    }
    private void CalcDamageType(ref int damage, DamageType damageType, Fightable attacker)
    {
        DamageType defenceType = default;
        if (Status.CurrentBehav is Skill skill) defenceType = skill.damageType;
        if (damageType == defenceType) EffectiveDefence(ref damage);
        else if ((damageType == DamageType.Slash && defenceType == DamageType.Magic) ||
            (damageType == DamageType.Thrust && defenceType == DamageType.Slash) ||
            (damageType == DamageType.Magic && defenceType == DamageType.Thrust)) UneffectiveDefence(ref damage);

        damage -= Status.Amount;
        void EffectiveDefence(ref int damage)
        {
            IO.pr($"{Name}의 {Status.CurrentBehav?.Name}은 적의 공격을 효과적으로 막아냈다. 원래 피해 : {damage}");
            damage = damage.ToUnVul();
            if (damageType != DamageType.Thrust && damage <= Status.Amount)
            {
                IO.pr($"그리고 패리로 적을 스턴 상태에 빠뜨렸다!");
                attacker.Status.SetStun(1);
            }
        }
        void UneffectiveDefence(ref int damage)
        {
            IO.pr($"{Name}의 {Status.CurrentBehav?.Name}은 별로 효과적인 막기가 아니었다! 원래 피해 : {damage}");
            damage = damage.ToVul();
        }
    }
    protected void Move(Position value)
    {
        if (Energy.IsMin) { IO.rk($"{Name}은 움직일 기력도 없다!"); return; }
        bool canGo = CanMove(value);
        if (canGo)
        {
            Pos += value;
            _currentMap.UpdateFightable(this);
        }
    }
    public bool CanMove(Position value)
    {
        Position temp = Pos + value;
        bool canGo = true;
        if (temp.x != Pos.x)
        {
            bool existsTile = _currentMap.Tiles.TryGet(temp.x, out _);
            bool obstructed = _currentMap.GetFightableAt(temp.x) is not null;
            canGo = existsTile && !obstructed;
        }
        return canGo;
    }
    protected virtual void Interact() { }
    public void OnBeforeTurn()
    {
        if (Status.Stun > 0) Status.ProcessStun();
        else SelectAction();
    }
    public virtual void OnTurnEnd()
    {
        passives.Invoke(this);
        if (Status.Poison > 0)
        {
            IO.pr($"{Name}은 중독 상태이다!", __.emphasis);
            Status.ProcessPoison();
        }
        Status.Reset();
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        IsAlive = false;
        IO.pr($"{Name}가 죽었다.", __.newline);
        Map.Current.UpdateFightable(this);
    }
    protected void OnHeal(object? sender, PointArgs e) => IO.rk($"{Name}은 {e.Amount}의 hp를 회복했다. {GetHp()}", __.emphasis);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.rk($"{Name}은 {e.Amount}의 피해를 입었다. {GetHp()}", __.emphasis);
    }
    public virtual char ToChar() => Name.ToLower()[0];
    public override string ToString() => $"이름 : {Name}\t레벨 : {Level}\nHp : {GetHp()}\t기력 : {Energy}\t{Stat}";
}
