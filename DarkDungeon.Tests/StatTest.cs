using System;
using Xunit;
public class StatTest : IDisposable
{
    Map? map;
    Player? player;
    public StatTest()
    {    
        map = new(3, false, null);
        player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        player = Player._instance = null;
        Program.OnTurn = null;
    }
    [Fact]
    public void HpScalesWithLevel()
    {
        TestMonster testMon = new(new(1));
        testMon.LevelUp();
        Assert.Equal(Status.LevelToBaseHp(testMon.Level), testMon.CurrentHp);
    }
}
