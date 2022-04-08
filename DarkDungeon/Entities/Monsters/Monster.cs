namespace Entities;
public partial class Monster : Inventoriable
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    protected DropList dropList;
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        Pos = spawnPoint;
        OnSpawn(data.startItem, data.startToken);
    }
    public void OnSpawn(Item item, int[] startTokens)
    {
        Inven[0] = item;
        foreach (var token in startTokens)
        {
            tokens.Add((TokenType)token);
        }
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        player.PickupCard(Draw(StatName.Sol, true));
        // foreach (var item in dropList.list)
        // {
        //     IItemData iitem = Inventoriable.Items[(int)item.dataIndex];
        //     if (DropOutOf(rnd, item.outof)) player.PickupItemData(iitem);
        // }
    }
    public virtual void DoTurn()
    {
        if (!IsAlive) return;
        behaviour(this);
    }
    private static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? fowardChar : backwardChar;
}

public record MonsterData(string name, char fowardChar, char backwardChar, StatMul stat, Action<Monster> behaviour, Item startItem, int[] startToken, DropList dropList);


public record struct DropList
{
    public readonly (It dataIndex, int outof)[] list;
    public DropList()
    {
        throw new ArgumentOutOfRangeException("Droplist cannot be empty");
    }
    public DropList(params (It dataIndex, int outof)[] list)
    {
        this.list = list;
    }
}