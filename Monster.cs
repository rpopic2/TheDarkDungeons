public class Monster : Entity
{
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
    protected override void OnDeath(object sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        Player.exp.Gain(3);
        Player.hand.Pickup(Draw());
    }
}