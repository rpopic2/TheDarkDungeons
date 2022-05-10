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
        Assert.Equal(2, player.Inven[0]?.stack);
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
        Assert.Equal(1, inven[0]!.stack);
        //Assert.Empty(inven);
    }
    
}
