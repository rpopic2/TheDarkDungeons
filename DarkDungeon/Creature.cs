public abstract partial class Creature
{
    public readonly string Name;
    public int Level => Stat.Level;
    public readonly Status Stat;
    public int Sight => Stat.Sight;
    public bool IsAlive { get; private set; } = true;
    public Position Pos { get; set; }

    private CurrentAction CurAction { get; init; }
    public Energy Energy { get => CurAction.Energy; }
    public IBehaviour? CurrentBehaviour => CurAction.CurrentBehav;

    public Inventory Inven { get; private set; }
    private Creature? _lastHit { get; set; }
    protected Creature? _lastAttacker { get; set; }
    public Action<Creature> passives = (p) => { };
    protected Action _turnPre = delegate { };
    protected Action _turnEnd = delegate { };
    public Action OnTurnAction = delegate { };

    public bool DidMoveLastTurn { get; private set; }

    public Creature(string name, int level, Status stat, int energy, Position pos)
    {
        Name = name;
        Stat = stat;
        Stat.Level = level;
        Pos = pos;
        Inven = new(this, " .(휴식)");
        CurAction = new(this, energy);
        GetHp().OnOverflow += new(OnDeath);
        GetHp().OnIncrease += new(OnHeal);
        GetHp().OnDecrease += new(OnDamaged);
        _turnPre = () => OnTurnPre();
        _turnEnd = () => OnTurnEnd();
        if (Map.Current is null) return;
        _currentMap.AddToOnTurnPre(_turnPre);
        _currentMap.AddToOnTurnEnd(_turnEnd);

    }
    public GamePoint GetHp() => Stat.Hp;
    public int CurrentHp => Stat.Hp.Cur;
    public int MaxHp => Stat.Hp.Max;
    protected Map _currentMap => Map.Current;
    public virtual Creature? CreatureAtFront => _currentMap.GetCreatureAt(Pos.Front(1));
    public abstract void LetSelectBehaviour();
    public void SetAction(ItemOld item, int skillIndex, int amount = 0, int amount2 = 0) => CurAction.Set(item, item.skills[skillIndex], amount, amount2);
    protected void SelectBehaviour(ItemOld item, int index)
    {
        if (CurAction.CurrentBehav != null) throw new Exception("스탠스가 None이 아닌데 새 동작을 선택했습니다. 한 턴에 두 동작을 할 수 없습니다.");
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Passive || behaviour is WearEffect)
        {
            IO.rk(behaviour.OnUseOutput);
            IO.Redraw();
        }
        else SetSkill(item, behaviour);
    }
    private void SetSkill(ItemOld item, IBehaviour behaviour)
    {
        int amount = 0;
        if (behaviour is IEnergyConsume energyConsume)
        {
            int mastery = Inven.GetMeta(item)!.Mastery;
            amount = Stat.GetRandom(energyConsume.StatDepend, mastery);
        }
        if (behaviour is Consume) Inven.Consume(item);
        CurAction.Set(item, behaviour, amount);
        PrintOnUseOutput();
    }
    //requirements : stance 정하기, rk로 프린트하기.
    ///<summary>[0] : 이동(Fightable f, int amout, int(Facing) facing) | [1] : 숨고르기(Fightable f, int(TokenType) tokenType, int discardIndex) | [2] : 상호작용
    ///</summary>
    protected void SelectBasicBehaviour(int index, int x, int y)
    {
        IBehaviour behaviour = basicActions.skills[index];
        if (behaviour is NonTokenSkill nonToken)
        {
            string output = behaviour.OnUseOutput;
            if (output != string.Empty) IO.rk(Name + output);
            CurAction.Set(basicActions, nonToken, x, y);
        }
    }
    private void PrintOnUseOutput()
    {
        string useOutput = $"{Name} {CurAction.CurrentBehav?.OnUseOutput} ({CurAction.Amount})";
        int mcharge = Inven.GetMeta(CurAction.CurrentItem!)!.magicCharge;
        if (mcharge > 0) useOutput += ($"+^b({mcharge})^/");
        IO.rk(useOutput);
    }

    private void Attack(int range)
    {
        DamageType damageType = default;
        if (CurAction.CurrentBehav is SkillOld skill) damageType = skill.damageType;
        Creature? mov = _currentMap.RayCast(Pos, range);
        ItemMetaData metaData = Inven.GetMeta(CurAction.CurrentItem!)!;
        if (mov is Creature hit)
        {
            _lastHit = hit;
            AttackMagicCharge(metaData);
            hit.Dodge(CurAction.Amount, damageType, this, metaData);
            metaData.GainExp();
        }
        metaData.magicCharge = 0;

        void AttackMagicCharge(ItemMetaData metaData)
        {
            int magicCharge = metaData.magicCharge;
            if (magicCharge > 0)
            {
                hit.Dodge(magicCharge, DamageType.Magic, this, metaData);
                Inven.GetMeta(CurAction.CurrentItem!)!.magicCharge = 0;
            }
        }
    }
    private void Throw(int range, ItemOld item, bool itemPreservesOnEnemy = false)
    {
        if (Inven.Contains(item))
        {
            IO.pr($"{item.Name}이 바람을 가르며 날아갔다.");
            Attack(range);
            ItemMetaData metaData = Inven.GetMeta(item)!;
            if (itemPreservesOnEnemy) _lastHit?.Inven.Add(item, metaData);
            Inven.Consume(item);
        }
        else
        {
            CurAction.Reset();
            IO.rk($"{item.Name}이 없다!");
        }
    }
    private void Dodge(int damage, DamageType damageType, Creature attacker, ItemMetaData metaData)
    {
        if (damage < 0) throw new Exception("데미지는 0보다 작을 수 없습니다.");
        _lastAttacker = attacker;
        StanceName? stance = CurAction.CurrentBehav?.Stance;
        if (stance == StanceName.Defence) CalcDamageType(ref damage, damageType, attacker);
        else if (stance == StanceName.Charge)
        {
            IO.pr($"{Name}은 약점이 드러나 있었다! ({damage})x{Rules.vulMulp}");
            damage = damage.ToVul();
        }

        if (damage <= 0) IO.rk($"{Name}은 아무런 피해도 받지 않았다.");
        else if (metaData.poison-- > 0) CurAction.SetPoison(2);
        Stat.Damage(damage);
    }
    private void CalcDamageType(ref int damage, DamageType damageType, Creature attacker)
    {
        DamageType defenceType = default;
        if (CurAction.CurrentBehav is SkillOld skill) defenceType = skill.damageType;
        if (damageType == defenceType) EffectiveDefence(ref damage);
        else if (damageType == DamageType.Slash && defenceType == DamageType.Magic) UneffectiveDefence(ref damage);
        else if (damageType == DamageType.Thrust && defenceType == DamageType.Slash) UneffectiveDefence(ref damage);
        else if (damageType == DamageType.Magic && defenceType == DamageType.Thrust) UneffectiveDefence(ref damage);

        damage -= CurAction.Amount;
        void EffectiveDefence(ref int damage)
        {
            IO.pr($"{Name}의 {CurAction.CurrentBehav?.Name}은 적의 공격을 효과적으로 막아냈다. 원래 피해 : {damage}");
            damage = damage.ToUnVul();
            if (damageType != DamageType.Thrust && damage <= CurAction.Amount)
            {
                IO.pr($"그리고 패리로 적을 스턴 상태에 빠뜨렸다!");
                attacker.CurAction.SetStun(1);
            }
        }
        void UneffectiveDefence(ref int damage)
        {
            IO.pr($"{Name}의 {CurAction.CurrentBehav?.Name}은 별로 효과적인 막기가 아니었다! 원래 피해 : {damage}");
            damage = damage.ToVul();
        }
    }
    protected virtual void Charge(ItemOld? item = null)
    {
        if (item is null) Inven.GetMeta(CurAction.CurrentItem!)!.magicCharge += CurAction.Amount;
        else Inven.GetMeta(item)!.magicCharge += CurAction.Amount;
        IO.rk($"{item}에 마법부여를 하였다.");
    }
    protected virtual void PoisonItem(ItemOld? item = null)
    {
        if (item is null) Inven.GetMeta(CurAction.CurrentItem!)!.poison++;
        else Inven.GetMeta(item)!.poison++;
        IO.rk($"{item}은 독으로 젖어 있다.");
    }
    protected void Move(Position value)
    {
        if (CanMove(value))
        {
            Pos += value;
            _currentMap.UpdateFightable(this);
            if (Energy.Cur != Energy.Max && DidMoveLastTurn)
            {
                Energy.Gain();
            }
            DidMoveLastTurn = true;
        }
    }
    protected void Dash(Position value)
    {
        Position movingDir = new(1, value.facing);
        for (int i = 0; i < value.x; i++)
        {
            Move(movingDir);
        }
    }
    public bool CanMove(Position value)
    {
        Position temp = Pos + value;
        bool canGo = true;
        if (temp.x != Pos.x)
        {
            bool existsTile = _currentMap.Tiles.TryGet(temp.x, out _);
            bool obstructed = _currentMap.GetCreatureAt(temp.x) is not null;
            canGo = existsTile && !obstructed;
        }
        return canGo;
    }
    protected virtual void Interact() { }
    private void OnTurnPre()
    {
        if (CurAction.Stun > 0) CurAction.ProcessStun();
        else LetSelectBehaviour();
    }
    public void OnTurn()
    {
        if (CurAction.CurrentBehav is not IBehaviour behav) throw new Exception($"턴이 흘렀는데도 {Name}이 아무 행동도 선택하지 않았습니다.");
        if (behav is NonTokenSkill nonTokenSkill) nonTokenSkill.NonTokenBehav.Invoke(this, CurAction.Amount, CurAction.Amount2);
        else behav.Behaviour.Invoke(this);
        OnTurnAction.Invoke();
    }
    protected virtual void OnTurnEnd()
    {
        passives.Invoke(this);
        if (CurAction.Poison > 0)
        {
            IO.pr($"{Name}은 중독 상태이다!", __.emphasis);
            CurAction.ProcessPoison();
        }
        CurAction.Reset();
    }
    protected virtual void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        IsAlive = false;
        IO.pr($"{Name}가 죽었다.", __.newline);
        Map.Current.UpdateFightable(this);
#pragma warning disable CS8601
        _currentMap.RemoveFromOnTurnPre(_turnPre);
        _currentMap.RemoveFromOnTurnEnd(_turnEnd);
    }
    private void OnHeal(object? sender, PointArgs e) => IO.rk($"{Name}은 {e.Amount}의 hp를 회복했다. {GetHp()}", __.emphasis);
    private void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.rk($"{Name}은 {e.Amount}의 피해를 입었다. {GetHp()}", __.emphasis);
    }
    public virtual char ToChar() => Name.ToLower()[0];
    public override string ToString() => $"이름 : {Name}\t레벨 : {Level}\nHp : {GetHp()}\t기력 : {CurAction.Energy}\t{Stat}";
}
