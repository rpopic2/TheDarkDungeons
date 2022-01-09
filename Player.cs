public class Player : Entity
{
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
    public Exp exp;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this, () => OnLvUp());
        for (int i = 0; i < cap; i++)
        {
            Hand.SetAt(Hand.Count, Draw());
        }
    }
    private void OnLvUp()
    {
        Lv++;
        exp.UpdateMax();
        Console.WriteLine("Level up!");
    }
    public void Pickup(Card card)
    {
        IO.pr("You've found a card." + card);
        IO.pr(Hand);
        IO.sel(Hand.Cur, out int index, out char key, out bool cancel);
        if (cancel)
        {
            IO.del();
            return;
        }
        Hand.SetAt(index, card);
        IO.del();
    }

    public new string Stats
    {
        get => base.Stats + $"\nExp : {exp}";
    }
}
