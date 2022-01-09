public class Monster : Entity
{
    private static readonly Player player = Player.instance;
    public Monster(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
    }

    public override Card Draw()
    {
        return base.Draw();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        player.exp.Gain(3);
        player.Hand.PlayerPickup(Draw());
    }
}