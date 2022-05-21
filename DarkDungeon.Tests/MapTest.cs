using Xunit;
public class MapTest
{
    [Fact]
    public void NonInteractiveTurnElapse()
    {
        Map map = new Map(5, null, false);
        Player player = Player._instance = new Player("TestPlayer");
        Assert.Equal(0, Map.Turn);

        Program.OnTurn?.Invoke();
        Assert.Equal(1, Map.Turn);
    }
}
