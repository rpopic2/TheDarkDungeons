using Xunit;
public class EnergyTest
{
    [Fact]
    public void EnergyRestoreWhileWalking()
    {
        Map map = new Map(5, null, false);
        Player player = Player._instance = new Player("TestPlayer");
        player.CurAction.Set(Creature.sword, Creature.sword.skills[0]);
        Assert.True(player.CurAction.CurrentBehav is not null);
        Program.OnTurn?.Invoke();
        Assert.NotEqual(player.Energy.Max, player.Energy.Cur);
    }
}
