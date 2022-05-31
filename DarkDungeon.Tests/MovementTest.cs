using System;
using Xunit;
public class MovementTest: IDisposable 
{
    private Map? _map;
    private Player? _player;
    private Player player => _player!;
    private Map map => _map!;

    public void Dispose()
    {
        _map = null;
        _player = null;
    }
    public MovementTest()
    {
        _player = Player._instance = new Player("TestPlayer");
        _map = new Map(5, false);
    }
    [Fact]
    public Bat SpawnBat()
    {
        Bat testBat = new Bat(default);
        map.UpdateFightable(testBat);
        Assert.Equal(map.GetCreatureAt(0), testBat);
        return testBat;
    }
    [Fact]
    public void TestCanMove()
    {
        Bat bat = SpawnBat();
        bool moveOneRight = bat.CanMove(new(1, Facing.Right));
        Assert.True(moveOneRight);
    }
    [Fact]
    public void TestTileExists()
    {
        bool existsTile = map.Tiles.TryGet(0, out _);
        Assert.True(existsTile);
        existsTile = map.Tiles.TryGet(-1, out _);
        Assert.False(existsTile);
    }
    [Fact]
    public void TestObstructedOnEmptyTile()
    {
        bool isObstructed = map.GetCreatureAt(0) is null;
        Assert.True(isObstructed);
    }
    [Fact]
    public void BlockMoveIfAtEnd()
    {
        Bat bat = SpawnBat();
        bool canMove = bat.CanMove(new(2, Facing.Left));
        Assert.False(canMove);

        canMove = bat.CanMove(new(6, Facing.Right));
        Assert.False(canMove);
    }
    [Fact]
    public void TestPlayerMovement()
    {
        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(1, player.Pos.x);
    }
    [Fact]
    public void CheckDidPlayerMoveLastTurn()
    {
        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Assert.False(player.DidMoveLastTurn);
        Program.OnTurn?.Invoke();
        Assert.True(player.DidMoveLastTurn);
    }

}
