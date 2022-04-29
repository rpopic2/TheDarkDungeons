namespace Entities;
public record MonsterData(string name, char fowardChar, char backwardChar, StatInfo stat, Action<Monster> behaviour, Item[] startItem);
public partial class Monster : Fightable
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    private Fightable? _followTarget;
    private Dictionary<string, int> metaData = new();
    public Monster(MonsterData data, Position spawnPoint)
    : base(name: data.name, level: Map.Depth,
    sol: new Mul(data.stat.sol, Rules.LEVEL_DIFFICULTY, Map.Depth), lun: new Mul(data.stat.lun, Rules.LEVEL_DIFFICULTY, Map.Depth), con: new Mul(data.stat.con, Rules.LEVEL_DIFFICULTY, Map.Depth),
    cap: data.stat.energy, pos: spawnPoint)
    {
        Sight = data.stat.Sight;
        killExp = new Mul(data.stat.killExp, Rules.LEVEL_DIFFICULTY, Map.Depth);
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        foreach (var newItem in data.startItem)
        {
            Inven.Add(newItem);
        }
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        base.OnDeath(sender, e);
        player.exp.point += killExp;
    }
    public override void SelectAction()
    {
        if (!IsAlive) return;
        behaviour(this);
    }
    private void BasicMovement()
    {
        int randomFace = Stat.rnd.Next(2);
        if (!Map.Current.IsAtEnd(Pos.x)) SelectBasicBehaviour(0, 1, randomFace);
        else SelectBasicBehaviour(0, 1, 1);// = Move(new(1, Facing.Left));
    }
    private void _SelectSkill(int item, int skill)
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
    public static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Right ? fowardChar : backwardChar;
}



public record DropList(params (Item item, int outOf)[] list);