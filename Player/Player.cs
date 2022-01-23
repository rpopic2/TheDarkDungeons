public class Player : Moveable
{
    private const char PlayerChar = '@';
    private readonly ItemData HpPot = new ItemData("HPPOT", 3);
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
    public Exp exp;
    public Inventory Inven { get; private set; }

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
        Hp.OnHeal += new EventHandler<HealArgs>(OnHeal);
        for (int i = 0; i < cap; i++)
        {
            Hand.SetAt(Hand.Count, Draw());
        }
        Inven = new Inventory(3);
        Inven[0] = new Item(HpPot);
    }

    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        level++;
        exp.UpdateMax();
        IO.pr("Level up! : " + level, true);
        bool cancel;
        int index;
        do
        {
            IO.seln(Program.stats, out index, out cancel, out ConsoleModifiers mod, Program.stats.Count());
        } while (cancel);
        switch (index)
        {
            case 0:
                Sol += 2;
                break;
            case 1:
                Lun += 2;
                break;
            case 2:
                Con += 2;
                break;
        }
        Hand.UpdateHandCap(Rules.capBasic + level.FloorMult(Rules.capByLevel));
        Hp.Max += level.FloorMult(Rules.hpByLevel);
        Hp += Hp.Max;
    }
    public void Pickup(Card card)
    {
        IO.pr("\nFound a card." + card);
        IO.seln_h(out int index, out bool cancel, out ConsoleModifiers mod);
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
            IO.seln_h(out index, out bool cancel, out ConsoleModifiers mod);
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
            IO.seln_h(out int index, out cancel, out ConsoleModifiers mod);
            if (!cancel && mod == ConsoleModifiers.Alt) Hand.StanceShift(index);
            IO.del();
        } while (!cancel);
    }
    public void UseCard()
    {
        do
        {
            IO.seln_h(out int index, out bool cancel, out ConsoleModifiers mod);
            if (cancel) return;
            UseCard(index);
            return;
        } while (true);
    }
    public void UseEquip()
    {
        do
        {
            IO.seln(Inven, out int index, out bool cancel, out ConsoleModifiers mod, Inven.Cap);
            if (cancel) return;
            UseEquip(index);
            return;
        } while (true);
    }
    public void UseEquip(int index)
    {
        if (Inven[index] is Item item)
        {
            stance = (Stance.Item, default);
            Hp += item.data.amount;
            Inven[index] = null;
        }
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
