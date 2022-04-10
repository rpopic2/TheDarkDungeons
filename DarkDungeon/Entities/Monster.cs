namespace Entities;
public partial class Monster : Fightable
{
    private int killExp;
    protected static Player player { get => Player.instance; }
    protected DropList dropList;
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    private Dictionary<string, int> metaData = new();
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap, spawnPoint)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
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
            Toks.Add((TokenType)token);
        }
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        foreach (var item in Toks.Content)
        {
            if (item is not null) player.PickupToken((TokenType)item);
        }
        // foreach (var item in dropList.list)
        // {
        //     if (DropOutOf(stat.rnd, item.outOf)) player.PickupItem(item.item);
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



public record struct DropList(params (Item item, int outOf)[] list);