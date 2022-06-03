using System;
using Xunit;
public class EnergyTest : IDisposable
{
    private Map? map;
    private Player? _player;
    private Player player => _player!;

    public void Dispose()
    {
        map = null;
        _player = null;
    }
    public EnergyTest()
    {
        _player = Player._instance = new Player("TestPlayer");
        map = new Map(5, false);
    }
    [Fact]
    public void ConsumeEnergyTest()
    {
        player.Inven.Add(Creature.sword);
        player.SetAction(Creature.sword, 0);
        Program.OnTurn?.Invoke();
        Assert.NotEqual(player.Energy.Max, player.Energy.Cur);
    }

    [Fact]
    public void DontConsumeIfIsNotIEnergyConsume()
    {
        Assert.Equal(player.Energy.Max, player.Energy.Cur);
        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(player.Energy.Max, player.Energy.Cur);
    }

    [Fact]
    public void RegenerateEnergyByWalking()
    {
        ConsumeEnergyTest();
        Assert.Equal(2, player.Energy.Cur);

        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(2, player.Energy.Cur);

        player.SetAction(Creature.basicActions, 0, 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(3, player.Energy.Cur);
    }
}
