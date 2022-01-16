public readonly struct Position
{
    public readonly int val;
    public readonly Facing facing;

    public Position(int val = default, Facing facing = default)
    {
        this.val = val;
        this.facing = facing;
    }
    public int FrontVal
      => facing == Facing.Front ? val + 1 : val - 1;

}