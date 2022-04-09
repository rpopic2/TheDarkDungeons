namespace Entities;
public partial class Monster
{
    private static DropList lunDropList = new(
        (Item.holySword, 10));
    private static DropList snakeDropList = new(
        (Item.holySword, 10));
    private static DropList batDropList = new(
        (Item.holySword, 10));
    private static StatMul lunaticMul = new(sol: 1, lun: 1, con: 3, hp: 3, cap: 4, killExp: 4);
    public static MonsterData lunatic = new("Lunatic", '>', '<', lunaticMul, (m) => m.LunaticBehav(), Item.holySword, new int[] { 2, 0, 2, 0 }, lunDropList);
    private static StatMul snakeMul = new(sol: 2, lun: 1, con: 2, hp: 2, cap: 2, killExp: 5);
    public static MonsterData snake = new("Snake", 'S', '2', snakeMul, (m) => m.SnakeBehav(), Item.bareHand, new int[] { 2, 0, 0 }, snakeDropList);
    public static StatMul batMul = new(sol: 1, lun: 3, con: 2, hp: 2, cap: 3, killExp: 3);
    public static MonsterData bat = new("Bat", 'b', 'd', batMul, (m) => m.BatBehav(), Item.bareHand, new int[] { 1, 1, 0 }, batDropList);
    public static List<MonsterData> data = new() { lunatic, snake, bat };
    public static int Count => data.Count;
    private void BasicMovement()
    {
        int moveX = stat.rnd.Next(2) == 1 ? 1 : -1;
        int direction = Pos.facing == Facing.Front ? -1 : 1;
        if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
        else Move(moveX, out char obj);
    }
    public void BatBehav()
    {
        if (tokens.Count > 0)
        {
            if (Target is null) BasicMovement();
            else
            {
                if (tokens.Count < 3 && player.Stance.Last == global::StanceName.Charge && tokens.Contains(TokenType.Offence)) SelectSkill(Item.bat, 0);
                else if (tokens.Contains(TokenType.Defence)) SelectSkill(Item.bat, 1);
                else if (tokens.Contains(TokenType.Offence)) SelectSkill(Item.bat, 0);
            }
        }
        else Rest(TokenType.Offence);
    }
    internal void LunaticBehav()
    {
        if (tokens.Count > 0)
        {
            if (Target is null) BasicMovement();
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
