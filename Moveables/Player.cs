public class Player : Moveable
{
    private const char PlayerChar = '@';
    public static Player instance = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
    public Exp exp;
    public int torch = 0;
    public int sight = 1;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
        Hp.OnHeal += new EventHandler<HealArgs>(OnHeal);
        for (int i = 0; i < cap; i++)
        {
            Hand[Hand.Count] = Draw();
        }
    }
    public void StartItem()
    {
        switch (ClassName)
        {
            case ClassName.Warrior:
                Pickup(Fightable.ItemData.Charge);
                Pickup(Fightable.ItemData.Berserk);
                Sol += 2;
                break;
            case ClassName.Assassin:
                Pickup(Fightable.ItemData.ShadowAttack);
                Pickup(Fightable.ItemData.Backstep);
                Lun += 2;
                break;
            case ClassName.Mage:
                Pickup(Fightable.ItemData.Torch);
                Pickup(Fightable.ItemData.SNIPE);
                Con += 2;
                break;
        }
    }

    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        Level++;
        exp.UpdateMax();
        IO.pr("Level up! : " + Level, true);
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
        Hand.Cap = Rules.capBasic + Level.FloorMult(Rules.capByLevel);
        Hp.Max += Level.FloorMult(Rules.hpByLevel);
        Hp += Hp.Max;
    }
    public override void Pickup(Card card)
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
        Hand[index] = card;
        IO.del(2);
    }
    public void Pickup(Item item)
    {
        IO.pr("\nFound an item." + item);
        IO.seln_i(out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel)
        {
            Pickup(Draw().Exile());
            IO.del(2);
            return;
        }
        if (Inven[index] is Item oldItem && oldItem.itemType == ItemType.Equip)
            oldItem.onExile?.Invoke(this);
        Inven[index] = item;
        if (item.itemType == ItemType.Equip) item.onPickup?.Invoke(this);
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
        Hand[index] = Hand[index]?.Exile();
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
            if (!cancel && mod == ConsoleModifiers.Alt) Hand[index] = Hand[index]?.StanceShift();
            IO.del();
        } while (!cancel);
    }
    public override Card? PickCard()
    {
        do
        {
            IO.seln_h(out int index, out bool cancel, out ConsoleModifiers mod);
            if (cancel) return null;
            return Hand[index];
        } while (true);
    }
    public void UseInven()
    {
        do
        {
            IO.seln_i(out int index, out bool cancel, out ConsoleModifiers mod);
            if (cancel) return;
            UseInven(index);
            return;
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
        base.ToString() + $"\nExp : {exp}\tTorch : {torch}";
    public override char ToChar() => PlayerChar;
}
