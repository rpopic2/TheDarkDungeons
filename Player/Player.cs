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
        level++;
        exp.UpdateMax();
        Console.WriteLine("Level up! : " + level);
        Sol += level.FloorMult(Rules.solByLevel);
        Cap = Rules.capBasic + level.FloorMult(Rules.capByLevel);
        Hp.Max += level.FloorMult(Rules.hpByLevel);
        Hp += Hp.Max;
    }
    public void Pickup(Card card)
    {
        IO.pr("\nFound a card." + card);
        IO.pr(Hand);
        IO.seln(out int index, out bool cancel, out ConsoleModifiers mod);
        IO.del();
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
            IO.pr(Hand);
            IO.seln(out index, out bool cancel);
            card = Hand[index] ?? throw new Exception();
            IO.del();
            if (cancel) return;
        } while (card.Stance == CardStance.Star);
        IO.pr("Exiled a card.");
        Hand.Exile(index);
        OnAction();
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
            IO.pr(Hand);
            IO.seln(out int index, out cancel, out ConsoleModifiers mod);
            if (!cancel && mod == ConsoleModifiers.Alt) Hand.StanceShift(index);
            IO.del(2);
        } while (!cancel);
        OnAction();
    }
    public void UseCard()
    {
        IO.pr(Hand);
        IO.seln(out int index, out bool cancel);
        if (cancel)
        {
            IO.del();
            return;
        }
        Card? card = Hand[index];
        if (card is null)
        {
            IO.del();
            return;
        }
        IO.del();
        UseCard(index, out bool elaspe);
        if (elaspe) OnAction();
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
        OnAction();
    }
    private void OnAction()
    {
        //Program.instance.ElaspeTurn();
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
