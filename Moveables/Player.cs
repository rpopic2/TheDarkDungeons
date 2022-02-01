public class Player : Inventoriable
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
        for (int i = 0; i < cap; i++)
        {
            Hand[Hand.Count] = Draw();
        }
    }
    public void StartItem()
    {
        EquipData data = new("LUNRIN", (stat.RefLun, 3));
        PickupItem(new Equip(stat, data));
        switch (ClassName)
        {
            case ClassName.Warrior:
                PickupItem(Inventoriable.Data.Charge);
                PickupItem(Inventoriable.Data.Berserk);
                stat.Sol += 2;
                break;
            case ClassName.Assassin:
                PickupItem(Inventoriable.Data.ShadowAttack);
                PickupItem(Inventoriable.Data.Backstep);
                stat.Lun += 2;
                break;
            case ClassName.Mage:
                PickupItem(Torch.data);
                PickupItem(Torch.data);
                stat.Con += 2;
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
                stat.Sol += 2;
                break;
            case 1:
                stat.Lun += 2;
                break;
            case 2:
                stat.Con += 2;
                break;
        }
        Hand.Cap = Rules.capBasic + Level.FloorMult(Rules.capByLevel);
        Hp.Max += Level.FloorMult(Rules.hpByLevel);
        Hp += Hp.Max;
    }
    public void PickupCard(Card card)
    {
        IO.pr("\nFound a card." + card);
        IO.seln_h(out int index, out bool cancel, out ConsoleModifiers mod);
        if (mod == ConsoleModifiers.Alt) card.StanceShift();
        if (cancel)
        {
            if (card.Stance != CardStance.Star)
            {
                PickupCard(card.Exile());
            }
            IO.del(2);
            return;
        }
        PickupCard(card, index);
        IO.del(2);
    }
    public void PickupItem(ItemData item) => PickupItem(new ItemEntity(item, stat));
    public void PickupItem(IItemEntity item)
    {
        IO.pr("\nFound an item." + item.abv);
        IO.seln_i(out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel)
        {
            PickupCard(Draw().Exile());
            IO.del(2);
            return;
        }
        PickupItem(item, index);
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
        stance = new(Stance.Exile, default);
    }
    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw());
        bool cancel = false;
        do
        {
            IO.pr("Review your hand\tq : Exit | Alt + num : Stanceshift");
            IO.seln_h(out int index, out cancel, out ConsoleModifiers mod);
            if (!cancel && mod == ConsoleModifiers.Alt) Hand[index] = Hand[index]?.StanceShift();
            IO.del();
        } while (!cancel);
    }
    public override Card? SelectCard()
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
