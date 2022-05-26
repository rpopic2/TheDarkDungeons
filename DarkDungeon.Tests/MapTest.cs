using System;
using Xunit;
public class MapTest : IDisposable
{
    Map? _map;
    Map map => _map!;
    Player? _player;
    Player player => _player!;
    public MapTest()
    {
        _map = new Map(5, false, new Pit());
        _player = Player._instance = new Player("TestPlayer");
    }
    public void Dispose()
    {
        _map = null;
        _player = null;
    }
    [Fact]
    public void CreateMapWithPit()
    {
        ISteppable?[] steppables = map.Steppables;
        bool containsPit = Array.Exists(map.Steppables, (s) => s is Pit);
        Assert.True(containsPit);
    }
    [Fact]
    public void CreateMapWithDoor()
    {
        _map = new(5, false, new Door());
        bool containsDoor = Array.Exists(map.Steppables, (s) => s is Door);
        Assert.True(containsDoor);
    }
    [Fact]
    public void CreateNextPortalRandomly()
    {
        _map = new(5, false, new RandomPortal());
        bool containsPortal = Array.Exists(map.Steppables, (s) => s is IPortal);
        Assert.True(containsPortal);
    }
    [Fact]
    public void CreatePortallessMap()
    {
        _map = new Map(5, false, null);
        ISteppable?[] steppables = map.Steppables;
        bool containsPortal = Array.Exists(map.Steppables, (s) => s is IPortal);
        Assert.False(containsPortal);
    }
    [Fact]
    public void NonInteractiveTurnElapse()
    {
        Assert.Equal(0, Map.Turn);

        Program.OnTurn?.Invoke();
        Assert.Equal(1, Map.Turn);
    }
    [Fact]
    public void GetElementAtTest()
    {
        map.UpdateFightable(player);
        Assert.NotNull(map.GetCreatureAt(0));
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void GetElementAtTestInvalidIndex(int index)
    {
        map.UpdateFightable(player);
        Assert.Null(map.GetCreatureAt(index));
    }
    [Fact]
    public void PlayerInteractPit()
    {
        Assert.Equal(0, Map.Depth);
        map.UpdateFightable(player);
        int portalIndex = Array.FindIndex(map.Steppables, (p) => p is Pit);
        Assert.NotEqual(-1, portalIndex);
        player.CurAction.Set(Creature.basicActions, 0, portalIndex, (int)Facing.Right);
        Program.ElaspeTurn();

        Assert.True(player.UnderFoot is Pit);
        player.CurAction.Set(Creature.basicActions, 2);
        Program.ElaspeTurn();

        Assert.Equal(1, Map.Depth);
    }
}
