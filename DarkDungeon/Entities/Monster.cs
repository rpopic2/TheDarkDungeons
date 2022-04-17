namespace Entities;
public partial class Monster : Fightable
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    private Dictionary<string, int> metaData = new();
    public Monster(MonsterData data, Position spawnPoint)
    : base(name: data.name, level: Map.Depth,
    sol: new Mul(data.stat.sol, 1.5f, Map.Depth), lun: new Mul(data.stat.lun, 1.5f, Map.Depth), con: new Mul(data.stat.con, 1.5f, Map.Depth),
    cap: data.stat.cap, pos: spawnPoint)
    {
        Sight = data.stat.Sight;
        killExp = new Mul(data.stat.killExp, 1.5f, Map.Depth);
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        OnSpawn(data.startItem, data.startToken);
    }
    public void OnSpawn(Item[] items, int[] startTokens)
    {
        foreach (var newItem in items)
        {
            Inven.Add(newItem);
        }
        foreach (var token in startTokens)
        {
            tokens.Add((TokenType)token);
        }
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        if (!IsAlive) return;
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        //player.PickupToken(tokens);
        // foreach (var item in dropList.list)
        // {
        //     if (DropOutOf(stat.rnd, item.outOf)) player.PickupItem(item.item);
        // }
    }
    public override void SelectAction()
    {
        if (!IsAlive) return;
        behaviour(this);
    }
    private static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Right ? fowardChar : backwardChar;
}



public record DropList(params (Item item, int outOf)[] list);