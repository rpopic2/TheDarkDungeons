public readonly struct Position
{
    public readonly int x;
    public readonly Facing facing;

    public Position(int val, Facing facing = default)
    {
        this.x = val;
        this.facing = facing;
    }
    public int Front
      => facing == Facing.Front ? x + 1 : x - 1;

    public static Position operator +(Position original, int newX)
    {
        return new Position(original.x + newX);
    } 

}