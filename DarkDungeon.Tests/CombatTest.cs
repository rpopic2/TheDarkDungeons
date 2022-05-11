using Xunit;
public class CombatTest
{
    private Map map = new(5, null, false);
    private Player player = Player._instance = new("TestPlayer");
    private Monster monster = new Bat(new(1, Facing.Left));
    [Fact]
    public void TestSelectAction()
    {
        player.SelectAction();
        Assert.NotNull(player.CurAction.CurrentBehav);
    }
}

