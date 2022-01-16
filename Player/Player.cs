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
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        Level++;
        exp.UpdateMax();
        Console.WriteLine("Level up! : " + Level);
        Sol += 1;
        Cap = 3 + Level.FloorMult(0.2f);
        Hp = new Hp(this, Hp.point.Max + 1, () => OnDeath());
        Hp.RestoreFull();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        IO.pr(Stats);
    }
    public void Pickup(Card card)
    {
        IO.pr("\nFound a card." + card);
        IO.pr(Hand);
        IO.sel(Hand.Cur, out int index, out char key, out bool cancel);
        if (cancel)
        {
            if(card.Stance != Stance.Star)
            {
                Pickup(card.Exile());
            }
            IO.del(2);
            return;
        }
        Hand.SetAt(index, card);
        IO.del(2);
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
