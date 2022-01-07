public class Player : Entity
{
    public Exp exp;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this);
    }

    public new string Stats
    {
        get => base.Stats + $"\tExp : {exp}";
    }
    public void LvUp()
    {
        lv++;
        Console.WriteLine("\nLevel up!");
        //ClassSwitch(() => { sol += 2; }, () => { lun += 2; }, () => { con += 2; });
    }
    public override Card Draw()
    {
        Card card = base.Draw();
        return card;
    }

    // public void ClassSwitch(Action warrior, Action assassin, Action mage)
    // {
    //     switch (charClass)
    //     {
    //         case ClassName.Warrrior:
    //             warrior();
    //             break;
    //         case ClassName.Assassin:
    //             assassin();
    //             break;
    //         case ClassName.Mage:
    //             mage();
    //             break;
    //     }
    // }
}
