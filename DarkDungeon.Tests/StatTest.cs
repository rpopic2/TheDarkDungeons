using System;
using Xunit;
public class StatTest : IDisposable
{
    Map? map;
    Player? _player;
    Player player => _player!;
    public StatTest()
    {
        map = new(3, false, null);
        _player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
        Program.OnTurn = null;
    }
    [Fact]
    public void HpScalesWithLevelTestMon()
    {
        TestMonster testMon = new(new(1));
        testMon.LevelUp();
        Assert.Equal(Status.LevelToBaseHp(testMon.Level), testMon.CurrentHp);
    }
}
