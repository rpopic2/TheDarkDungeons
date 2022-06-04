using System;
using Xunit;
public class ItemTest: IDisposable
{
    Map? _map;
    Player? _player;
    Map map => _map!;
    Player player => _player!;
    public ItemTest()
    {    
        _map = new(3, false, null);
        _player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        _map = null;
        _player = Player._instance = null;
        Map.ResetMapTurn();
        Program.OnTurn = null;
    }
    [Fact]
    public void ShadowBowTest()
    {
    
    }
}
