using System;
using Xunit;
public class MapTest
{
    [Fact]
    public void CreateMapWithPortal()
    {
        Map map = new Map(5, false, true);     
        ISteppable?[] steppables = map.Steppables;
        Assert.NotEqual(-1, Array.LastIndexOf<ISteppable?>(steppables, new Portal()));
    }
    [Fact]
    public void CreatePortallessMap()
    {
        Map map = new Map(5, false, false);     
        ISteppable?[] steppables = map.Steppables;
        int portalIndex = Array.LastIndexOf<ISteppable?>(steppables, new Portal());
        Assert.Equal(-1, portalIndex);
    }
    [Fact]
    public void NonInteractiveTurnElapse()
    {
        Map map = new Map(5, true);
        Player player = Player._instance = new Player("TestPlayer");
        Assert.Equal(0, Map.Turn);

        Program.OnTurn?.Invoke();
        Assert.Equal(1, Map.Turn);
    }
}
