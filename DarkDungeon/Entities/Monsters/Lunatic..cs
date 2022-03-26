public class Lunatic : Monster
{
    public static MonsterData lunatic = new(1, "Lunatic", '>', '<', ClassName.Warrior, lunaticMul, lunDropList);
    public override MonsterData data { get => lunatic; }
    private static StatMul lunaticMul = new(new(3, 0.6f, lv), new(1, n, n), new(2, n, n), new(3, 0.6f, lv), new(1, 0.16f, lv), new(4, 0.3f, lv));
    private static DropList lunDropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.FieryRing, 15),
        (It.LunarRing, 15));

    public Lunatic(Position pos) : base(lunatic, pos)
    {

    }

    public override void DoTurn()
    {
        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                int moveX = rnd.Next(3) - 1;
                int direction = Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                else Move(moveX, out char obj);
            }
            else _UseCard((Card)Hand.GetFirst()!);
        }
        else Rest();
    }

}