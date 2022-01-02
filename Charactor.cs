public class Charactor : Entity
{
    public Exp exp;
    public Charactor(string name, ClassName className) : base(name, className)
    {
        exp = new Exp(this);
    }
    public new string Stats
    {
        get => base.Stats + $"\tExp : {exp}";
    }
    public void LvUp()
    {
        Lv++;
        Console.WriteLine("\nLevel up!");
        //ClassSwitch(() => { sol += 2; }, () => { lun += 2; }, () => { con += 2; });
    }
    public override Card Draw()
    {
        Card card = base.Draw();
        IO.pr("You've found a card." + card);
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
