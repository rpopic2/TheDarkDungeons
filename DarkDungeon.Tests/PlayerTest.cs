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
        player.SetAction(Creature.spiritStaff, 1, 10);
        Program.ElaspeTurn();
        Assert.Equal(10, player.Inven.GetMeta(Creature.spiritStaff!)!.magicCharge);
    }
}
