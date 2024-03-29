using System;
using Xunit;
public class MonsterTestTest : IDisposable
{
    Map? _map;
    Player? _player;
    Map map => _map!;
    Player player => _player!;
    public MonsterTestTest()
    {
        _map = new(3, false, null);
        _player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        _map = null;
        _player = Player._instance = null;
        Map.ResetMapTurn();
        Program.OnTurn = null;
    }
    [Fact]
    public void HolySwordTest()
    {
        TestMonster testMon = new(new());
        testMon.GiveItemOld(Creature.holySword);
        testMon.SetAction(Creature.holySword, 1, 5);
        Program.ElaspeTurn();
        Assert.NotEqual(0, testMon.Inven.GetMagicCharge(Creature.holySword));
    }
    // [Fact]
    // public void MarksmanTest()
    // {
    //     Marksman marksman = new(new(2, Facing.Left));
    //     map.UpdateFightable(player);
    //     map.UpdateFightable(marksman);
    //     player.Inven.Add(Creature.torch);
    //     Assert.Equal(5, marksman.Inven.GetStack(Creature.arrow));
    //     Assert.Contains(Creature.arrow, marksman.Inven);
    //     Program.ElaspeTurn();
    //     Program.ElaspeTurn();
    //     Assert.Equal(4, marksman.Inven.GetStack(Creature.arrow));
    //     Assert.NotEqual(player.GetHp().Max, player.GetHp().Cur);
    // }
}
