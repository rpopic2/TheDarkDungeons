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

    public static Position operator +(Position original, int val)
    {
        int newX = original.x + val;
        Facing newFacing = val < 0 ? Facing.Back : Facing.Front;
        Position newPos = new Position(newX, newFacing);
        return newPos;
    } 

}