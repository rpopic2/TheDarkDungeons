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
        int newHp = testMon.Stat.Level.RoundMult(Rules.LEVEL_TO_BASE_HP_RATE) + testMon.Stat[StatName.Sol];
        Assert.Equal(newHp, testMon.CurrentHp);
    }
    [Fact]
    public void StatusGetLevel()
    {
        Assert.Equal(1, player.Stat.Level);
    }
    [Fact]
    public void StatusGetExp()
    {
        Assert.Equal(0, player.Stat.Exp);
    }
    [Fact]
    public void TestOnLvUp()
    {
        Assert.Equal(1, player.Level);
        player.GainExp(player.ReqExp);
        Assert.Equal(2, player.Level);
        int newHp = player.Level.RoundMult(Rules.LEVEL_TO_BASE_HP_RATE) + player.Stat[StatName.Sol];
        Assert.Equal(newHp, player.GetHp().Max);
    }
}
