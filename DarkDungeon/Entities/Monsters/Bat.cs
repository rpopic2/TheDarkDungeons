namespace Entities;
public class Bat : Monster
{
    public Bat(MonsterData data, Position spawnPoint) : base(data, spawnPoint)
    {
        IO.pr("Bat spawned");
        if (Hand[0] is Card card)
        {
            Hand[0] = card.StanceShift();
        }
        dropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.FieryRing, 15),
        (It.LunarRing, 15));
    }
    public override void DoTurn()
    {
        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                int moveX = rnd.Next(2) == 1 ? 1 : -1;
                int direction = Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                else Move(moveX, out char obj);
            }
            else if (Hand.GetFirst() is Card card)
            {
                _UseCard(card);
            }
        }
        else Rest();
    }
}