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
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, data.className, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        Pos = spawnPoint;
        if (data.name == "Lunatic")
        {
            Inven[0] = Item.sword;
            tokens.Add(TokenType.Offence);
            tokens.Add(TokenType.Defence);
            tokens.Add(TokenType.Offence);
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
    public readonly static Action<Monster> batBehav = (m) =>
    {
        throw new NotImplementedException();
    };
    internal static readonly Action<Monster> lunaticBehav = (m) =>
    {
        if (m.tokens.Count > 0)
        {
            if (m.Target is null)
            {
                int moveX = m.stat.rnd.Next(3) - 1;
                int direction = m.Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(m.Pos.x)) m.Move(direction, out char obj);
                else m.Move(moveX, out char obj);
            }
            else
            {
                TokenType? token = m.tokens.TryUse(0);
                if (token is TokenType token1) m.SelectSkill(m.Inven[0]!, 0);
            }
        }
        else m.Rest();
    };
    internal static readonly Action<Monster> snakeBehav = (m) =>
    {
        if (m.Hand.Count > 0)
        {
            if (m.Target is null)
            {
                Map.Current.MoveablePositions.TryGet(m.Pos.x + 2, out Moveable? target);
                if (target is not null) m.Target = target;
                else
                {
                    int moveX = m.stat.rnd.Next(2) == 1 ? 1 : -1;
                    int direction = m.Pos.facing == Facing.Front ? -1 : 1;
                    if (Map.Current.IsAtEnd(m.Pos.x)) m.Move(direction, out char obj);
                    else m.Move(moveX, out char obj);
                }
            }
            if (m.Target is not null)
            {
                m._UseCard((Card)m.Hand.GetFirst()!);
            }
        }
        else m.Rest();
    };

    public void Rest()
    {
        base.Rest(TokenType.Offence);
    }
    private static bool DropOutOf(Random rnd, int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? fowardChar : backwardChar;
}