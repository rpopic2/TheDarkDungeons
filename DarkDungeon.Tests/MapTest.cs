using System;
using Xunit;
public class MapTest : IDisposable
{
    Map? _map;
    Map map => _map!;
    public MapTest()
    {
        _map = new Map(5, false, new Portal());
    }
    public void Dispose()
    {
        _map = null;
    }
    [Fact]
    public void CreateMapWithPortal()
    {
        ISteppable?[] steppables = map.Steppables;
        Assert.NotEqual(-1, Array.LastIndexOf<ISteppable?>(steppables, new Portal()));
    }
    [Fact]
    public void CreatePortallessMap()
    {
        _map = new Map(5, false, null);
        ISteppable?[] steppables = map.Steppables;
        int portalIndex = Array.LastIndexOf<ISteppable?>(steppables, new Portal());
        Assert.Equal(-1, portalIndex);
    }
    [Fact]
    public void NonInteractiveTurnElapse()
    {
        Player player = Player._instance = new Player("TestPlayer");
        Assert.Equal(0, Map.Turn);

        Program.OnTurn?.Invoke();
        Assert.Equal(1, Map.Turn);
    }
    [Fact]
    public void GetElementAtTest()
    {
        Player player = Player._instance = new Player("TestPlayer");
        map.UpdateFightable(player);
        Assert.NotNull(map.GetCreatureAt(0));
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void GetElementAtTestInvalidIndex(int index)
    {
        Player player = Player._instance = new Player("TestPlayer");
        map.UpdateFightable(player);
        Assert.Null(map.GetCreatureAt(index));
    }
    [Fact]
    public void SpawnPortal()
    {
       _map = new(5, false, new Door()); 
       bool containsDoor = Array.Exists(map.Steppables, (s)=>s is Door);
       Assert.True(containsDoor);
    }
    [Fact]
    public void SpawnDoors()
    {
       _map = new(5, false, new Door()); 
       bool containsDoor = Array.Exists(map.Steppables, (s)=>s is Door);
       Assert.True(containsDoor);
    }
}
