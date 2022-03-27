public class Snake : Monster
{
    private static DropList snakeDropList = new(
        (It.HpPot, 10),
        (It.Bag, 11),
        (It.Torch, 5),
        (It.ShadowAttack, 20),
        (It.Scouter, 5));
    private static StatMul snakeMul = new(sol: new(2, 0.6f, lv), lun: new(1, n, n), con: new(2, n, n), hp: new(2, 0.3f, lv), cap: new(2, 0.16f, lv), killExp: new(5, 0.3f, lv));
    public static MonsterData data = new(2, "Snake", 'S', '2', ClassName.Warrior, snakeMul, snakeDropList);

    public Snake(Position pos) : base(data, pos)
    {

    }

    public override void DoTurn()
    {
        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                Map.Current.Moveables.TryGet(Pos.x + 2, out Moveable? target);
                if (target is not null) Target = target;
                else
                {
                    int moveX = rnd.Next(2) == 1 ? 1 : -1;
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
        else Rest();
    }
    
}