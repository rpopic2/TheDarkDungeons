namespace Entities;
public record MonsterData(string name, char fowardChar, char backwardChar, StatInfo stat, Item[] startItem);
public abstract class Monster : Fightable
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    private char fowardChar, backwardChar;
    protected Fightable? _followTarget;
    protected Dictionary<string, int> metaData = new();
    public Monster(MonsterData data, Position spawnPoint)
    : base(name: data.name, level: Map.Depth, stat: new(data.stat.SolDifficulty, data.stat.LunDifficulty, data.stat.ConDifficulty),
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
    public override void SelectAction()
    {
        if (!IsAlive) return;
        if(Energy.Cur <= 0) OnEnergyDeplete();
        else if(_followTarget is not null) OnFollowTarget();
        else OnNothing();
    }
    protected abstract void OnEnergyDeplete();
    protected abstract void OnFollowTarget();
    protected abstract void OnNothing();
    protected void BasicMovement()
    {
        int randomFace = Stat.rnd.Next(2);
        if (!Map.Current.IsAtEnd(Pos.x)) SelectBasicBehaviour(0, 1, randomFace);
        else SelectBasicBehaviour(0, 1, 1);// = Move(new(1, Facing.Left));
    }
    protected void _SelectSkill(int item, int skill)
    {
        SelectBehaviour(Inven[item]!, skill);
    }
    public override void OnTurnEnd()
    {
        UpdateTarget();
        base.OnTurnEnd();
    }
    private void UpdateTarget()
    {
        if (_lastAttacker is not null)
        {
            _followTarget = _lastAttacker;
        }
        Fightable? target = _currentMap.RayCast(Pos, Sight);
        if (target is not Fightable || !this.IsEnemy(target)) this._followTarget = null;
        else this._followTarget = target;
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        base.OnDeath(sender, e);
        player.exp.point += killExp;
    }
    public static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Right ? fowardChar : backwardChar;
}



public record DropList(params (Item item, int outOf)[] list);