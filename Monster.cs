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

    public void TakeDamge(int x)
    {
        Hp.Cur -= x;
        IO.pr($"{Name} takes {x} damage. {Hp.Cur}/{Hp.Max}");
    }
    public override void OnDeath()
    {
        base.OnDeath();
        Program.player.exp.Gain(3);
    }
}