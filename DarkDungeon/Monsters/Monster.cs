public record MonsterData(string name, char fowardChar, char backwardChar, StatInfo stat, Item[] startItem);
public abstract class Monster : Creature
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    private char fowardChar, backwardChar;
    protected Creature? _target;
    public Monster(MonsterData data, Position spawnPoint)
    : base(name: data.name, level: Map.Depth, stat: data.stat.stat.GetDifficultyStat(),
    energy: data.stat.energy, pos: spawnPoint)
    {
        killExp = data.stat.KillExpDifficulty;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        foreach (var newItem in data.startItem)
        {
            Inven.Add(newItem);
        }
    }
    protected int DistanceToTarget => _target?.Pos.Distance(Pos) ?? 0;

    public override void LetSelectBehaviour()
    {
        if (!IsAlive) return;
        if (Energy.Cur <= 0) OnEnergyDeplete();
        else if (_target is not null) OnTarget();
        else OnNothing();
    }
    protected abstract void OnEnergyDeplete();
    protected abstract void OnTarget();
    protected abstract void OnNothing();
    protected void BasicMovement()
    {
        int randomFace = Stat.rnd.Next(2);
        if (!Map.Current.IsEnd(Pos.x)) SelectBasicBehaviour(0, 1, randomFace);
        else SelectBasicBehaviour(0, 1, 1);// = Move(new(1, Facing.Left));
    }
    protected void _SelectSkill(int item, int skill)
    {
        SelectBehaviour(Inven[item]!, skill);
    }
    protected override void OnTurnEnd()
    {
        UpdateTarget();
        base.OnTurnEnd();
    }
    private void UpdateTarget()
    {
        if (_lastAttacker is not null)
        {
            _target = _lastAttacker;
            return;
        }
        Creature? target = _currentMap.RayCast(Pos, Sight);
        if (target is not Creature || !this.IsEnemy(target)) this._target = null;
        else this._target = target;
    }
    protected void FollowTarget()
    {
        if (_target is null) return;
        Facing newFacing = Pos.LookAt(_target.Pos.x);
        SelectBasicBehaviour(0, 1, (int)newFacing); //move
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        base.OnDeath(sender, e);
        player.Stat.GainExp(killExp);
    }
    public static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Right ? fowardChar : backwardChar;
}