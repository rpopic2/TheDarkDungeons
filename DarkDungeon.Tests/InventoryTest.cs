using Xunit;
public class InventoryTest
{
    private Player player = Player._instance = new("TestPlayer");
    private Inventory inven = Player._instance.Inven;
    [Fact]
    public void TestAddEquip()
    {
        player.Inven.Add(Creature.bow);
        Assert.Equal(Creature.bow, player.Inven[0]);
    }
    [Fact]
    public void TestAddConsumeable()
    {
        player.Inven.Add(Creature.arrow);
        Assert.Equal(Creature.arrow, player.Inven[0]);
        player.Inven.Add(Creature.arrow);

        Assert.Equal(Creature.arrow, player.Inven[0]);
        Assert.Equal(2, inven.GetMeta(Creature.arrow).stack);
    }
    [Fact]
    public void TestAddEquipOnFull()
    {
        inven.Add(Creature.torch);
        inven.Add(Creature.sword);
        inven.Add(Creature.tearOfLun);
        inven.Add(Creature.shield);
        inven.Add(Creature.spiritStaff);
//todo change this to an automated test
        inven.Add(Creature.dagger, 1, out bool added);
        Assert.Equal(5, inven.Count);
        if (added) Assert.Contains(Creature.dagger, inven);
        else Assert.DoesNotContain(Creature.dagger, inven);
    }
    [Fact]
    public void TestRemove()
    {
        player.Inven.Add(Creature.arrow);
        player.Inven.Remove(Creature.arrow);
        Assert.Empty(player.Inven);
    }
    [Fact]
    public void TestRemoveStacked()
    {
        inven.Add(Creature.arrow);
        player.Inven.Add(Creature.arrow);
        player.Inven.Remove(Creature.arrow);
        Assert.Empty(player.Inven);
    }
    [Fact]
    public void TestConsume()
    {
        inven.Add(Creature.arrow);
        inven.Consume(Creature.arrow);
        Assert.Empty(inven);
    }
    [Fact]
    public void TestConsumeStacked()
    {
        inven.Add(Creature.arrow);
        inven.Add(Creature.arrow);
        inven.Consume(Creature.arrow);
        Assert.Equal(1, inven.GetMeta(Creature.arrow).stack);
    }

}
