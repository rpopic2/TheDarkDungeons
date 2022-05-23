using System;
using Xunit;
public class InventoryTest : IDisposable
{
    Map? map;
    Player? player;
    public InventoryTest()
    {
        map = new(5, false);
        player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        player = Player._instance = null;
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
        Assert.NotEqual(0, inven.GetMeta(Creature.dagger).CurExp);
    }
    [Fact]
    public void PickupCorpseItemNonInteractive()
    {
        player!.Inven.Add(Creature.dagger);
        Shaman shaman = new(new(1, Facing.Right));
        player!.CurAction.Set(Creature.dagger, Creature.dagger.skills[0], 5);
        Program.OnTurn?.Invoke();

        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Corpse corpse = (Corpse)player.UnderFoot!;
        corpse.GetItemAndMeta(0, out Item? item, out ItemMetaData? metaData);
        Assert.False(player.Inven.Contains(item));
        player.PickupItem(item, metaData);
        Assert.True(player.Inven.Contains(item));
    }
}
