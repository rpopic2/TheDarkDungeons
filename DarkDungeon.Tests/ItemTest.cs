using System;
using Xunit;
public class ItemTest : IDisposable
{
    private static Action? OnTurn { get => Program.OnTurnAction; set => Program.OnTurnAction = value; }
    Map? _map;
    Player? _player;
    Map map => _map!;
    Player player => _player!;
    public ItemTest()
    {
        _map = new(4, false, null);
        _player = Player._instance = new Player("test");
        _map.UpdateFightable(_player);
    }
    public void Dispose()
    {
        _map = null;
        _player = Player._instance = null;
        Map.ResetMapTurn();
        Program.OnTurn = null;
    }
    [Fact]
    public void GiveItemTest()
    {
        Assert.False(player.HasItem<ShadowBow>());
        player.GiveItem(new ShadowBow());
        bool hasShadowBow = player.HasItem<ShadowBow>();
        Assert.True(hasShadowBow);
    }
    [Fact]
    public void GiveMultipleItem()
    {
        player.GiveItem(new Bolt(), 2);
        Assert.Equal(2, player.GetStackOfItem<Bolt>());
    }
    [Fact]
    public void HasMultipleItems()
    {
        Assert.False(player.HasItem<ShadowBow>());
        player.GiveItem(new ShadowBow());
        Assert.False(player.HasItem<Bolt>());
        player.GiveItem(new Bolt());
        Assert.True(player.HasItem<ShadowBow>());
        Assert.True(player.HasItem<Bolt>());
    }
    [Fact]
    public void ItemStacks()
    {
        player.GiveItem(new Bolt());
        player.GiveItem(new Bolt());
        player.GiveItem(new ShadowBow());
        int stackOfBolt = player.GetStackOfItem<Bolt>();
        Assert.Equal(2, stackOfBolt);
        Assert.Equal(1, player.GetStackOfItem<ShadowBow>());
    }
    [Fact]
    public void RemoveItem()
    {
        player.GiveItem(new ShadowBow());
        player.RemoveItem<ShadowBow>();
        Assert.False(player.HasItem<ShadowBow>());
    }
    [Fact]
    public void RemoveItemDoesNotThrowError()
    {
        player.RemoveItem<Bolt>();//trying to remove when bolt does not exist in inventory.
    }
    [Fact]
    public void ShootShadowBow()
    {
        TestMonster testMon = new(new(2));
        player.GiveItem(new ShadowBow());
        player.GiveItem(new Bolt());
        player.GiveItem(new Bolt());
        OnTurn += player.GetItem<ShadowBow>().Throw;
        Assert.Equal(testMon.MaxHp, testMon.CurrentHp);

        Program.ElaspeTurn();
        Assert.Equal(1, player.GetStackOfItem<Bolt>());//consumes bolt
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTest()
    {
        TestMonster testMon = new(new(1));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Pierce;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTestOutOfRange()
    {
        TestMonster testMon = new(new(2));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Pierce;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.Equal(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTestThrow()
    {
        TestMonster testMon = new(new(3));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Throw;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
        int tempCur = testMon.CurrentHp;
        Program.ElaspeTurn();
        Assert.Equal(tempCur, testMon.CurrentHp);
    }
    // [Fact]
    // public void ShadowDaggerTestBackstep()
    // {
    //     TestMonster testMon = new(new(3));
    //     player.GiveItem(new ShadowDagger());
    //     player.GetItem<ShadowDagger>().Magic();
    //     Program.ElaspeTurn();
    //     Assert.Equal(new Position(4, Facing.Left), player.Pos);
    // }
    [Fact]
    public void TestItemUseSkill()
    {
        player.GiveItem(new ShadowDagger());
        // OnTurn += player.GetItem<ShadowDagger>().Throw;
    }
}