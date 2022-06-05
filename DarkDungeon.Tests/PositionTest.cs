using Xunit;
public class PositionTest
{
    [Fact]
    public void TestName()
    {
        Position pos = new(3, Facing.Left);
        Position expected = Shadowstep(pos);
        Assert.Equal(new(4, Facing.Left), expected);
        pos = new(3, Facing.Right);
        expected = Shadowstep(pos);
        Assert.Equal(new(2, Facing.Right), expected);
    }
    private Position Shadowstep(Position targetPos)=> new(targetPos.Back(2), targetPos.facing);
}