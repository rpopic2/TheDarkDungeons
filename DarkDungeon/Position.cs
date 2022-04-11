public struct Position
{
    public int v;
    public Facing facing;
    public Position(int x, Facing facing = default)
    {
        this.v = x;
        this.facing = facing;
    }
    public int GetFrontIndex(int value) => facing == Facing.Front ? v + value : v - value;
    public bool isFacingFront => facing == Facing.Front;

    public static Position operator +(Position a, Position b)
    {
        if (a.facing != b.facing) b.v--;
        int bv = b.facing == Facing.Front ? b.v : -b.v;
        return new(a.v + bv, b.facing);
    }
    public override string ToString() => $"{v} facing {facing.ToString()}";
}