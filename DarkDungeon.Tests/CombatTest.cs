//using Xunit;
public class CombatTest
{
    private Player player = Player._instance = new("TestPlayer");
    private Map map = new(5, null, false);
    private Monster monster = new Bat(new(1, Facing.Left));
}

