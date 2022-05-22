using Xunit;
public class InventoryTest
{
    [Fact]
    public void AddWithMetaData()
    {
        Player player = new("test");
        Inventory inven = new(player, "testInven");
        ItemMetaData metaData = new();
        metaData.GainExp();
        Assert.NotEqual(0, metaData.CurExp);
        inven.Add(Creature.dagger, metaData);
        Assert.NotEqual(0, inven.GetMeta(Creature.dagger).CurExp);
    }
}
