namespace Entities;
public class Monster : Inventoriable
{
    protected static int lv => Map.level;
    protected static int t => Game.Turn;
    protected const int n = 1;
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
    internal void LunaticBehav()
    {
        if (tokens.Count > 0)
        {
            if (Target is null)
            {
                int moveX = stat.rnd.Next(3) - 1;
                int direction = Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                else Move(moveX, out char obj);
            }
            else
            {
                if (tempCharge > 0 && tokens.Contains(TokenType.Offence)) SelectSkill(Inven[0]!, 0);
                else if (tokens.Contains(TokenType.Charge)) SelectSkill(Inven[0]!, 1);
            }
        }
        else Rest(TokenType.Offence);
    }
    internal void SnakeBehav()
    {
        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                Map.Current.MoveablePositions.TryGet(Pos.x + 2, out Moveable? target);
                if (target is not null) Target = target;
                else
                {
                    int moveX = stat.rnd.Next(2) == 1 ? 1 : -1;
                    int direction = Pos.facing == Facing.Front ? -1 : 1;
                    if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                    else Move(moveX, out char obj);
                }
            }
            if (Target is not null)
            {
                _UseCard((Card)Hand.GetFirst()!);
            }
        }
        else Rest(TokenType.Offence);
    }
    private static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? fowardChar : backwardChar;
}