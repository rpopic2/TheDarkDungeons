using System;
using Xunit;
public class PlayerTest : IDisposable
{
    Map? map;
    Player? _player;
    Player player => _player!;
    public PlayerTest()
    {
        map = new(3, false);
        _player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
    }
    [Fact]
    public void TestPlayerCharge()
    {
        player.Inven.Add(Creature.spiritStaff);
        player.CurAction.Set(Creature.spiritStaff, 1, 10);
        Program.ElaspeTurn();
        Assert.Equal(10, player.Inven.GetMeta(Creature.spiritStaff!)!.magicCharge);
    }
    [Fact]
    public void PlayerLevelUpTest()
    {
        Assert.Equal(1, player.Level);
        player.exp.point += player.exp.point.Max;
        Assert.Equal(2, player.Level);
    }
    [Fact]
    public void HpScalesWithLevelPlayer()
    {
        Assert.Equal(1, player.Level);
        player.exp.point += player.exp.point.Max;
        Assert.Equal(2, player.Level);
        int expectedHp = Status.LevelToBaseHp(player.Level) + player.Stat[StatName.Sol];
        Assert.Equal(expectedHp, player.GetHp().Cur);
    }
}
