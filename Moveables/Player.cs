public class Player : Inventoriable
{
    public static Player instance = new Player("Michael", ClassName.Assassin, 1, 2, 2, 2, 5, 3);
    public Exp exp;
    public int torch = 0;
    public int sight = 1;

    public Player(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, lv, sol, lun, con, maxHp, cap)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
        for (int i = 0; i < cap; i++)
        {
            Hand[i] = Draw();
        }
    }
    public void StartItem()
    {
        PickupItem(new Torch(this, stat));
        PickupItem(new Torch(this, stat));
        switch (ClassName)
        {
            case ClassName.Warrior:
                PickupItem(Inventoriable.SkillDb.Charge);
                PickupItem(Inventoriable.SkillDb.Berserk);
                stat.sol += 2;
                break;
            case ClassName.Assassin:
                PickupItem(Inventoriable.SkillDb.ShadowAttack);
                PickupItem(Inventoriable.SkillDb.Backstep);
                stat.lun += 2;
                break;
            case ClassName.Mage:
                PickupItem(Torch.torch);
                PickupItem(Torch.torch);
                stat.con += 2;
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
                stat.sol += 2;
                break;
            case 1:
                stat.lun += 2;
                break;
            case 2:
                stat.con += 2;
                break;
        }
        Hand.Cap = new Mul(3, 0.4f, Mul.lv);
        Hp.Max = new Mul(5, Mul.n, Mul.lv);
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
    public void PickupItem(IItemData data)
    {
        if(data is ItemData item) PickupItem(item);
        else if(data is EquipData equip) PickupItem(equip);
    }
    public void PickupItem(ItemData data) => PickupItem(new Item(data, stat));
    public void PickupItem(EquipData data) => PickupItem(new Equip(this, stat, data));
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
        bool success = Move(x, out char obj);
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
    public override string ToString() => base.ToString() + $"\nExp : {exp}\tTorch : {torch}";
    public override char ToChar() => MapSymb.player;
}
