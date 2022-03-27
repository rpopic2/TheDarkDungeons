namespace Entities;
public class Monster : Fightable
{
    protected static int lv => Map.level;
    protected static int t => Game.Turn;
    protected const int n = 1;
    private int killExp;
    protected static Player player { get => Player.instance; }
    protected DropList dropList;
    private char fowardChar, backwardChar;
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, data.className, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        Pos = spawnPoint;
        PickupCard(Draw(), Hand.Count);
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        player.PickupCard(Draw());
        foreach (var item in dropList.list)
        {
            IItemData iitem = Inventoriable.Items[(int)item.dataIndex];
            if (DropOutOf(rnd, item.outof)) player.PickupItemData(iitem);
        }
    }
    public virtual void DoTurn()
    {
        if (!IsAlive) return;
    }
    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw(), Hand.Count);
    }
    private static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? fowardChar : backwardChar;
}