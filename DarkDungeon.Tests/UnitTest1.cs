using Xunit;

namespace DarkDungeon.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        GamePoint point = new(5, GamePointOption.Reserving);
        point -= 1;
        Equals(4 == point.Cur);
    }
    [Fact]
    public void Test2()
    {
        GamePoint point = new(0, GamePointOption.Stacking);
        point += 1;
        Equals(1 == point.Cur);
    }
    [Fact]
    public void Test3()
    {
        Program prg = new();
        Assert.True(prg.GetType() == typeof(Program));
    }
}