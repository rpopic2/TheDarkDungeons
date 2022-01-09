public class Player : Entity
{
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
    public Exp exp;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this, () => OnLvUp());
    }
    private void OnLvUp()
    {
        lv++;
        exp.UpdateMax();
        Console.WriteLine("Level up!");
    }

    public new string Stats
    {
        get => base.Stats + $"\nExp : {exp}";
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
