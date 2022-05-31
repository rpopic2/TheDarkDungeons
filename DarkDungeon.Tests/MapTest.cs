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
        _map = new Map(5, false, new RandomPortal());
        Map.Current = _map;
        _player = Player._instance = new Player("TestPlayer");
        map.UpdateFightable(player);
    }
    public void Dispose()
    {
        _map = null;
        Map.ResetMapTurn();
        _player = null;
    }
    [Fact]
    public void CreateMapWithPit()
    {
        _map = new Map(5, false, new Pit());
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
    [Theory]
    [InlineData(50)]
    [InlineData(80)]
    public void PitAlwaysOnTurn50(int turn)
    {
        for (int i = 0; i < turn; i++) Program.ElaspeTurn();
        _map = new(5, false, new RandomPortal());
        bool containsPortal = Array.Exists(map.Steppables, (s) => s is IPortal);
        Assert.Equal(turn, Map.Turn);
        Assert.True(containsPortal);
    }
    [Fact]
    public void PitChanceZeroOnNewGame()
    {
        Assert.Equal(0, Map.TurnInCurrentDepth);
        Assert.Equal(-1, map.PitChanceMax);
        bool containsPit = Array.Exists(map.Steppables, (s) => s is Pit);
        Assert.False(containsPit);
    }
    [Fact]
    public void TurnInCurrentDepthResetsOnNewDepth()
    {
        Assert.Equal(Map.START_DEPTH, Map.Depth);
        Assert.Equal(0, Map.TurnInCurrentDepth);
        Program.ElaspeTurn();
        Assert.Equal(1, Map.TurnInCurrentDepth);
        _map = new(5, false, new Pit());
        Assert.Equal(1, Map.TurnInCurrentDepth);
        Program.ElaspeTurn();
        Assert.Equal(2, Map.TurnInCurrentDepth);
        PlayerInteractPit();
        Assert.Equal(Map.START_DEPTH + 1, Map.Depth);
        Assert.Equal(0, Map.TurnInCurrentDepth);
    }
    [Fact]
    public void ResetPitChanceOnNewMap()
    {
        for (int i = 0; i < 50; i++) Program.ElaspeTurn();
        _map = new(5, false, new RandomPortal());
        PlayerInteractPit();
        Assert.Equal(Map.START_DEPTH + 1, Map.Depth);
        Assert.Equal(52, Map.Turn);
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
        Assert.NotNull(map.GetCreatureAt(0));
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void GetElementAtTestInvalidIndex(int index)
    {
        Assert.Null(map.GetCreatureAt(index));
    }
    private void PlayerInteractPit()
    {
        int portalIndex = Array.FindIndex(map.Steppables, (p) => p is IPortal);
        Assert.NotEqual(-1, portalIndex);
        player.SetAction(Creature.basicActions, 0, portalIndex, (int)Facing.Right);
        Program.ElaspeTurn();

        Assert.True(player.UnderFoot is IPortal);
        player.SetAction(Creature.basicActions, 2);
        Program.ElaspeTurn();
    }
    [Fact]
    public void PlayerInteractPitTest()
    {
        _map = new(3, false, new Pit());
        Assert.Equal(Map.START_DEPTH, Map.Depth);
        PlayerInteractPit();
        Program.ElaspeTurn();

        Assert.Equal(Map.START_DEPTH + 1, Map.Depth);
    }
    [Fact]
    public void PlayerInteractDoorTest()
    {
        _map = new(3, false, new Door());
        Map current = Map.Current;
        Assert.Equal(Map.START_DEPTH, Map.Depth);
        int portalIndex = Array.FindIndex(map.Steppables, (p) => p is Door);

        player.SetAction(Creature.basicActions, 0, portalIndex, (int)Facing.Right);
        Program.ElaspeTurn();

        Assert.True(player.UnderFoot is Door);
        player.SetAction(Creature.basicActions, 2);
        Program.ElaspeTurn();

        Assert.NotEqual(current, Map.Current);
        Assert.Equal(Map.START_DEPTH, Map.Depth);
    }
}
