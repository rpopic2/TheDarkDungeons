using System;
using Xunit;
public class TestMonsterTest : IDisposable
{
    Map? map;
    TestMonster? _testMon;
    TestMonster testMon => _testMon!;
    public TestMonsterTest()
    {
        map = new(3, false);
        _testMon = new(new(1, Facing.Right));
    }
    public void Dispose()
    {
        map = null;
        _testMon = null;
        Program.OnTurn = null;
    }
    [Fact]
    public void TestMonsterGiveItem()
    {
        testMon.GiveItem(Creature.sword);
        Assert.Equal(Creature.sword, testMon!.Inven[0]);
    }
    [Fact]
    public void TestMonSetAction()
    {
        TestMonster testMon = new(new(1, Facing.Right));
        testMon.GiveItem(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Assert.Equal(Creature.sword.skills[0], testMon.CurAction.CurrentBehav);
    }
    [Fact]
    public void TestMonPreformSetAction()
    {
        TestMonster testMon = new(new(1, Facing.Right));
        testMon.GiveItem(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Program.OnTurn?.Invoke();
        Assert.Equal(testMon.Energy.Max - 1, testMon.Energy.Cur);
    }
    [Fact]
    public void TestMonPositionTest()
    {
        TestMonster testMon = new(new(1, Facing.Right));
        Assert.Equal(testMon, map!.GetCreatureAt(1));
    }
}
