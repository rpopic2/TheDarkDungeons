public class Player : Entity
{
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 0, 2, 2, 2);
    public static Hand hand;
    public static Exp exp = new Exp(instance);

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        Player.exp = new Exp(this);
        Player.hand = base.Hand;
        exp.OnLvUp = () =>
        {
            lv++;
            Console.WriteLine("\nLevel up!");
        };
    }

    public new string Stats
    {
        get => base.Stats + $"\tExp : {exp}";
    }
    public override Card Draw()
    {
        Card card = base.Draw();
        return card;
    }
    public override void Attack(Card card, Entity target)
    {
        hand.Delete(card);
        base.Attack(card, target);
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
