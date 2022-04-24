public struct Position
{
    public static readonly Position MOVELEFT = new(1, Facing.Left);
    public static readonly Position MOVERIGHT = new(1, Facing.Right);
    public int x { get; private set; }
    public Facing facing { get; private set; }
    public Position(int x, Facing facing = default)
    {
        this.x = x;
        this.facing = facing;
    }
    public int Front(int value) => (this + new Position(value, facing)).x;
    public int Back(int value) => (this + new Position(value, facing.Flip())).x;
    public static Position operator +(Position a, Position b)
    {
        if (a.facing != b.facing) b.x--;
        int bv = b.facing == Facing.Right ? b.x : -b.x;
        return new(a.x + bv, b.facing);
    }
    public override string ToString() => $"{x} facing {facing.ToString()}";

    public Facing LookAt(int v)
    {
        if (x > v) return Facing.Left;
        else if (x < v) return Facing.Right;
        else return facing;
    }
}