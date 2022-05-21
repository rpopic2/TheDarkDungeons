using Xunit;
public class EnergyTest
{
    Map map = new Map(5, null, false);
    Player player = new Player("TestPlayer");

    [Fact]
    public void EnergyRestoreWhileWalking()
    {
    
       map.OnTurnElapse();
    }
}
