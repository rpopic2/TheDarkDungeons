using Xunit;
public class MovementTest
{
    [Fact]
    public Bat SpawnBat()
    {
        Bat testBat = new Bat(default);
        Map map = new(2, null, false);
        map.UpdateFightable(testBat);
        Assert.Equal(map.GetCreature(0), testBat);
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
        Map map = new(2, null, false);
        bool existsTile = map.Tiles.TryGet(0, out _);
        Assert.True(existsTile);
        existsTile = map.Tiles.TryGet(-1, out _);
        Assert.False(existsTile);
    }
    [Fact]
    public void TestObstructedOnEmptyTile()
    {
        Map map = new(2,null,false);
        bool isObstructed = map.GetCreature(0) is null;
        Assert.True(isObstructed);
    }
    [Fact]
    public void BlockMoveIfAtEnd()
    {
        Bat bat = SpawnBat();
        bool canMove = bat.CanMove(new(2, Facing.Left));
        Assert.False(canMove);

        canMove = bat.CanMove(new(2, Facing.Right));
        Assert.False(canMove);
    }
    [Fact]
    public void TestPlayerMovement()
    {

        Player player = Player._instance = new Player("testPlayer");
        Map map = new(2,null,false);
        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(1, player.Pos.x);
    }
    [Fact]
    public void CheckDidPlayerMoveLastTurn()
    {
        Player player = Player._instance = new Player("testPlayer");
        Map map = new(2,null,false);
        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Assert.Equal(false, player.DidMoveLastTurn);
        Program.OnTurn?.Invoke();
        Assert.Equal(true, player.DidMoveLastTurn);
    }

}
