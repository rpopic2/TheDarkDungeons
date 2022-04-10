namespace Entities;
public record MonsterData(string name, char fowardChar, char backwardChar, StatMul stat, Action<Monster> behaviour, Item[] startItem, int[] startToken, DropList dropList);
public partial class Monster
{
    private static DropList lunDropList = new(
        (Item.holySword, 10));
    private static DropList snakeDropList = new(
        (Item.holySword, 10));
    private static DropList batDropList = new(
        (Item.bat, 10));
    private static StatMul lunaticMul = new(sol: 1, lun: 1, con: 3, hp: 3, cap: 4, killExp: 4);
    public static MonsterData lunatic = new("광신도", '>', '<', lunaticMul, (m) => m.LunaticBehav(), new Item[] { Item.holySword, Item.tearOfLun }, new int[] { 2, 0, 2, 0 }, lunDropList);
    private static StatMul snakeMul = new(sol: 2, lun: 1, con: 2, hp: 2, cap: 2, killExp: 5);
    public static MonsterData snake = new("뱀", 'S', '2', snakeMul, (m) => m.SnakeBehav(), new Item[] { Item.bareHand }, new int[] { 2, 0, 0 }, snakeDropList);
    public static StatMul batMul = new(sol: 1, lun: 3, con: 2, hp: 2, cap: 3, killExp: 3);
    public static MonsterData bat = new("박쥐", 'b', 'd', batMul, (m) => m.BatBehav(), new Item[] { Item.bat }, new int[] { 1, 1, 0 }, batDropList);
    public static List<MonsterData> data = new() { lunatic, bat };
    public static int Count => data.Count;
    private void BasicMovement()
    {
        int moveX = stat.rnd.Next(2) == 1 ? 1 : -1;
        int direction = Pos.facing == Facing.Front ? -1 : 1;
        if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
        else Move(moveX, out char obj);
    }
    private void _SelectSkill(int item, int skill)
    {
        SelectBehaviour(Inven[item]!, skill);
    }
    public void BatBehav()
    {
        if (tokens.Count > 0)
        {
            if (Target is null) BasicMovement();
            else
            {
                if (tokens.Count < player.tokens.Count && tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
                else if (tokens.Contains(TokenType.Defence)) _SelectSkill(0, 1);
                else if (tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
            }
        }
        else Rest(TokenType.Offence);
    }
    internal void LunaticBehav()
    {
        if(Hp.Cur != Hp.Max && Inven.Content.Contains(Item.tearOfLun)) _SelectSkill(1, 0);
        else if (tokens.Count > 0)
        {
            if (Target is null) BasicMovement();
            else
            {
                if (Inven.GetMeta(Item.holySword).magicCharge > 0 && tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
                else if (tokens.Contains(TokenType.Charge)) _SelectSkill(0, 1);
            }
        }
        else Rest(TokenType.Offence);
    }
    internal void SnakeBehav()
    {
        if (tokens.Count > 0)
        {
            if (Target is null) BasicMovement();
            else
            {

            }
        }
        else Rest(TokenType.Offence);
    }
}
