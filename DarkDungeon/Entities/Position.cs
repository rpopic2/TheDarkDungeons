public struct Position
{
    public int oldX;
    public int x;
    public Facing facing;

    public Position(int x, int oldX = default, Facing facing = default)
    {
        this.x = x;
        this.oldX = oldX;
        this.facing = facing;
    }
    public int FrontIndex => facing == Facing.Front ? x + 1 : x - 1;
    public int FrontMul => facing == Facing.Front ? 1 : -1;
    public int BackMul => facing == Facing.Front ? -1 : 1;
    public bool isFacingFront => facing == Facing.Front;

    public override string ToString() => $"{x} facing {facing.ToString()}";
    public static Position operator +(Position original, int value)
    {
        original.oldX = original.x;
        original.x += value;
        original.facing = value < 0 ? Facing.Back : Facing.Front;
        return original;
    }
    public static Position operator !(Position original)
    {
        original.facing = original.facing.Flip();
        original.oldX = original.x;
        return original;
    }
}