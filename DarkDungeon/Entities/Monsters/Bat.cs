namespace Entities;
public class Bat : Monster
{
    private static StatMul mul = new(1, 3, 2, 2, 1, 3);
    public static MonsterData data = new("Bat", 'b', 'd', mul, Monster.batBehav, dropList);
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