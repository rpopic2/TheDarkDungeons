public class Player : Moveable
{
    private const char PlayerChar = '@';
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
        Sol += 1;
        Hp.RestoreFull();
    }
    public void Pickup(Card card)
    {
        IO.pr("Found a card." + card);
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
    public void Loot(int expGain, Card card)
    {
        exp.Gain(expGain);
        Pickup(card);
    }
    public override void Rest()
    {
        base.Rest();
        Pickup(Draw());
    }
    public override void Move(int x)
    {
        bool success = _Move(x, out char obj);
        if (!success) return;
        if (obj == MapSymb.portal)
        {
            Map.NewMap();
            Pos = new Position();
        }
        IO.del(2);
        Program.instance.ElaspeTurn();
    }
    public new string Stats
    {
        get => base.Stats + $"\nExp : {exp}";
    }
    public override char ToChar()
    {
        return PlayerChar;
    }
}
