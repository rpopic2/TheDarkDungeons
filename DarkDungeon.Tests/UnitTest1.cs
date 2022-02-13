using Xunit;
namespace DarkDungeon.Tests;

using Entities;
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
        Player._instance = new("Micheal", ClassName.Warrior, cap: 3, maxHp: 3, sol: 2, lun: 2, con: 2);
        Assert.True(Player.instance.IsAlive);
        //Game.NewTurn();

    }
}