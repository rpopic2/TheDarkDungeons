public interface IIO
{
    public ConsoleKeyInfo rk();
    public void pr(string value, bool newline = true);
    public void pr_inventory();
    public void pr_map();
    public void pr_depth_lv();
    public void pr_hp_energy();
    public void pr_monster_hp_energy(Creature? frontCreature);
    public string rl();
    public void clr();
    public void del();
}
