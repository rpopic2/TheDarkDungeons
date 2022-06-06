using System;
public class TestTemplate : IDisposable
{
    protected static Action? OnTurn { get => Program.OnTurnAction; set => Program.OnTurnAction = value; }
    protected Map? _map;
    protected Player? _player;
    protected Map map => _map!;
    protected Player player => _player!;
    public TestTemplate()
    {
        _map = new(4, false, null);
        _player = Player._instance = new Player("test");
        _map.UpdateFightable(_player);
    }
    public void Dispose()
    {
        _map = null;
        _player = Player._instance = null;
        Map.ResetMapTurn();
        Program.OnTurn = null;
    }
    protected void ElapseTurn() => Program.ElaspeTurn();
}