using System;
using Xunit;
public class DamageTypeTest : IDisposable
{
    Map? map;
    Player? _player;
    TestMonster? _testMon;
    Player player => _player!;
    TestMonster testMon => _testMon!;
    public DamageTypeTest()
    {
        map = new(3, false);
        _player = Player._instance = new Player("test");
        _testMon = new(new(2));
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
        Program.OnTurn = null;
        _testMon = null;
    }
    [Fact]
    public void DamageType()
    {
        player.Inven.Add(Creature.sword);
        testMon.GiveItem(Creature.sword);
        player.CurAction.Set(Creature.sword, 1, 10);
        testMon.SetAction(Creature.sword, 1);

    }
}
