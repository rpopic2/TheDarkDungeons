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