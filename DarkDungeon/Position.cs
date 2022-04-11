public struct Position
{
    public int x { get; private set; }
    public Facing facing { get; private set; }
    public Position(int x, Facing facing = default)
    {
        this.x = x;
        this.facing = facing;
    }
    public int Front(int value) => (this + new Position(value, facing)).x;
    public bool isFacingRight => facing == Facing.Right;

    public static Position operator +(Position a, Position b)
    {
        if (a.facing != b.facing) b.x--;
        int bv = b.facing == Facing.Right ? b.x : -b.x;
        return new(a.x + bv, b.facing);
    }
    public override string ToString() => $"{x} facing {facing.ToString()}";
}