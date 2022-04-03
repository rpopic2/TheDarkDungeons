namespace Entities;
public class Bat : Monster
{
    public static MonsterData data = new(0, "Bat", 'b', 'd', ClassName.Assassin, mul, Monster.batBehav, dropList);
    private static StatMul mul = new(new(1, 0.6f, lv), new(3, n, n), new(2, n, n), new(2, 0.4f, lv), new(1, 0.16f, lv), new(3, 0.3f, lv));
    private new static DropList dropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.FieryRing, 15),
        (It.LunarRing, 15));

    public Bat(Position spawnPoint) : base(data, spawnPoint)
    {

    }
    public override void DoTurn()
    {
        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                int moveX = stat.rnd.Next(2) == 1 ? 1 : -1;
                int direction = Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                else Move(moveX, out char obj);
            }
            else if (Hand.GetFirst() is Card card)
            {
                _UseCard(card);
            }
        }
        else Rest(TokenType.Offence);
    }
}