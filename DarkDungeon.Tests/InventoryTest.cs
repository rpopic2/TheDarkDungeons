using System;
using Xunit;
public class InventoryTest : IDisposable
{
    Map? map;
    Player? _player;
    Player player => _player!;
    public InventoryTest()
    {
        map = new(5, false);
        _player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
        Program.OnTurn = null;
    }
    [Fact]
    public void AddWithMetaData()
    {
        Inventory inven = player!.Inven;
        ItemMetaData metaData = new();
        metaData.GainExp();
        Assert.NotEqual(0, metaData.CurExp);
        inven.Add(Creature.dagger, metaData);
        Assert.NotEqual(0, inven.GetMeta(Creature.dagger)!.CurExp);
    }
    [Fact]
    public void PickupCorpseItemNonInteractive()
    {
        player!.Inven.Add(Creature.dagger);
        player!.SetAction(Creature.dagger, 0, 5);
        TestMonster testMon = new(new(1, Facing.Right));
        testMon.GiveItemOld(Creature.arrow);
        Program.OnTurn?.Invoke();

        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();

        Corpse corpse = (Corpse)player.UnderFoot!;
        corpse.GetItemAndMeta(0, out Item? item, out ItemMetaData? metaData);
        Assert.DoesNotContain(item, player.Inven);
        if (item is null || metaData is null) throw new NullReferenceException();
        player.PickupItem(item, metaData);
        Assert.Contains(item, player.Inven);
    }
    [Fact]
    public void GetMetaTest()
    {
        ItemMetaData metaData = new();
        metaData.GainExp();
        player.Inven.Add(Creature.shield, metaData);

        Assert.Equal(metaData, player.Inven.GetMeta(Creature.shield));
        Assert.Equal(1, player.Inven.GetMeta(Creature.shield)!.CurExp);
    }
    [Fact]
    public void GetInvalidMetaData()
    {
        Assert.Null(player.Inven.GetMeta(Creature.shield));
    }
    [Fact]
    public void GetStackOfItem()
    {
        ItemMetaData metaData = new();
        metaData.stack = 2;
        player.Inven.Add(Creature.arrow, metaData);
        int stack = player.Inven.GetStack(Creature.arrow);
        Assert.Equal(2, stack);
    }
}
