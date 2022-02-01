public class Monster : Fightable
{
    private int killExp;
    private static Player player { get => Player.instance; }
    private DropList dropList;
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, data.className, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        Pos = spawnPoint;
        if (data.name == "Bat") PickupCard(Draw().StanceShift(), Hand.Count);
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        player.PickupCard(Draw());
        foreach (var item in dropList.list)
        {
            if (DropOutOf(item.outof)) player.PickupItem(item.data);
        }
    }
    public void DoTurn()
    {
        if (!IsAlive) return;
        behaviour(this);
    }
    public readonly static Action<Monster> batBehav = (m) =>
    {
        if (m.Hand.Count > 0)
        {
            if (m.Target is null)
            {
                int moveX = m.rnd.Next(2) == 1 ? 1 : -1;
                int direction = m.Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(m.Pos.x)) m.Move(direction, out char obj);
                else m.Move(moveX, out char obj);
            }
            else m._UseCard((Card)m.Hand.GetFirst()!);
        }
        else m.Rest();
    };
    internal static readonly Action<Monster> lunaticBehav = (m) =>
    {
        if (m.Hand.Count > 0)
        {
            if (m.Target is null)
            {
                int moveX = m.rnd.Next(3) - 1;
                int direction = m.Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(m.Pos.x)) m.Move(direction, out char obj);
                else m.Move(moveX, out char obj);
            }
            else m._UseCard((Card)m.Hand.GetFirst()!);
        }
        else m.Rest();
    };

    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw(), Hand.Count);
    }
    private bool DropOutOf(int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? fowardChar : backwardChar;
}