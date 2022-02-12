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
}