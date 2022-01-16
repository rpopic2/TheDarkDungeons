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
    public int Front
      => facing == Facing.Front ? x + 1 : x - 1;

    public static Position operator +(Position target, int val)
    {
        target.oldX = target.x;
        target.x += val;
        target.facing = val < 0 ? Facing.Back : Facing.Front;
        return target;
    } 
    public static Position operator !(Position original)
    {
        original.facing = original.facing.Flip();
        return original;
    }
}