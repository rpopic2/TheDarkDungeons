using Xunit;
public class EnergyTest
{
    [Fact]
    public void ConsumeEnergyTest()
    {
        Map map = new Map(5, null, false);
        Player player = Player._instance = new Player("TestPlayer");
        player.CurAction.Set(Creature.sword, Creature.sword.skills[0]);

        Program.OnTurn?.Invoke();
        Assert.NotEqual(player.Energy.Max, player.Energy.Cur);
    }
}
