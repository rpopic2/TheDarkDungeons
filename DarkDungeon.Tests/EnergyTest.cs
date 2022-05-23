using Xunit;
public class EnergyTest
{
    private Map map = new Map(5, false);
    private Player player = Player._instance = new Player("TestPlayer");
    [Fact]
    public void ConsumeEnergyTest()
    {
        player.CurAction.Set(Creature.sword, Creature.sword.skills[0]);
        Program.OnTurn?.Invoke();
        Assert.NotEqual(player.Energy.Max, player.Energy.Cur);
    }
    [Fact]
    public void DontConsumeIfIsNotIEnergyConsume()
    {
        Assert.Equal(player.Energy.Max, player.Energy.Cur);
        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(player.Energy.Max, player.Energy.Cur);
    }

    [Fact]
    public void RegenerateEnergyByWalking()
    {
        ConsumeEnergyTest();
        Assert.Equal(2, player.Energy.Cur);
        
        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(2, player.Energy.Cur);

        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 1, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.Equal(3, player.Energy.Cur);
    }
}
