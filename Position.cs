public readonly struct Position
{
    public readonly int oldX;
    public readonly int x;
    public readonly Facing facing;

    public Position(int x, int oldX = default, Facing facing = default)
    {
        this.x = x;
        this.oldX = oldX;
        this.facing = facing;
    }
    public int Front
      => facing == Facing.Front ? x + 1 : x - 1;

    public static Position operator +(Position original, int val)
    {
        int originalX = original.x;
        Facing newFacing = val < 0 ? Facing.Back : Facing.Front;
        Position newPos = new Position(originalX + val, originalX, newFacing);
        return newPos;
    } 

}