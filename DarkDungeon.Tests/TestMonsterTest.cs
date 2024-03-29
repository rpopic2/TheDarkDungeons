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
        testMon.GiveItemOld(Creature.sword);
        Assert.Equal(Creature.sword, testMon!.Inven[0]);
    }
    [Fact]
    public void TestMonSetAction()
    {
        testMon.GiveItemOld(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Assert.Equal(Creature.sword.skills[0], testMon.CurrentBehaviour);
    }
    [Fact]
    public void TestMonPreformSetAction()
    {
        testMon.GiveItemOld(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Program.OnTurn?.Invoke();
        Assert.Equal(testMon.Energy.Max - 1, testMon.Energy.Cur);
    }
    [Fact]
    public void TestMonPositionTest()
    {
        Assert.Equal(testMon, map!.GetCreatureAt(1));
    }
    [Theory]
    [InlineData(10)]
    // [InlineData(0)]
    // [InlineData(-1)]
    public void SetHpTest(int v)
    {
       testMon.SetHp(v);
       Assert.Equal(v, testMon.GetHp().Cur); 
    }
    [Theory]
    [InlineData(10)]
    public void SetConTest(int con)
    {
        testMon.SetStat(StatName.Con, con);
        Assert.Equal(con, testMon.Stat[StatName.Con]);
    }
    [Fact]
    public void TestLvUp()
    {
        Assert.Equal(Map.Depth, testMon.Level);
        testMon.LevelUp();
        Assert.Equal(Map.Depth + 1, testMon.Level);
    }
    
}
