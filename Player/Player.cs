public class Player : Moveable
{
    private const char PlayerChar = '@';
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
    public Exp exp;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
        for (int i = 0; i < cap; i++)
        {
            Hand.SetAt(Hand.Count, Draw());
        }
    }
    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        level++;
        exp.UpdateMax();
        IO.pr("Level up! : " + level, true);
        Sol += level.FloorMult(Rules.solByLevel);
        Cap = Rules.capBasic + level.FloorMult(Rules.capByLevel);
        Hp.Max += level.FloorMult(Rules.hpByLevel);
        Hp += Hp.Max;
    }
    public void Pickup(Card card)
    {
        IO.pr("\nFound a card." + card);
        IO.selh(out int index, out bool cancel, out ConsoleModifiers mod);
        if (mod == ConsoleModifiers.Alt) card.StanceShift();
        if (cancel)
        {
            if (card.Stance != CardStance.Star)
            {
                Pickup(card.Exile());
            }
            IO.del(2);
            return;
        }
        Hand.SetAt(index, card);
        IO.del(2);
    }
    public void Exile()
    {
        int index;
        Card card;
        do
        {
            IO.selh(out index, out bool cancel, out ConsoleModifiers mod);
            card = Hand[index] ?? throw new Exception();
            if (cancel) return;
        } while (card.Stance == CardStance.Star);
        IO.pr("Exiled a card.");
        Hand.Exile(index);
        stance = (Stance.Exile, default);
    }
    public override void Rest()
    {
        base.Rest();
        Pickup(Draw());
        bool cancel = false;
        do
        {
            IO.pr("Review your hand\tq : Exit | Alt + num : Stanceshift");
            IO.selh(out int index, out cancel, out ConsoleModifiers mod);
            if (!cancel && mod == ConsoleModifiers.Alt) Hand.StanceShift(index);
            IO.del();
        } while (!cancel);
    }
    public void UseCard()
    {
        do
        {
            IO.selh(out int index, out bool cancel, out ConsoleModifiers mod);
            if (cancel) return;
            Card? card = Hand[index];
            if (card is not null)
            {
                UseCard(index);
                return;
            }
        } while (true);

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
    }
    public void ShowStats()
    {
        IO.pr(this);
        IO.rk();
        IO.del(3);
    }
    public override string ToString() =>
        base.ToString() + $"\nExp : {exp}";
    public override char ToChar() => PlayerChar;
}
