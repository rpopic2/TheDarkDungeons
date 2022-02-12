using System.Diagnostics;
namespace Entities;

public class Player : Inventoriable
{
    public const int skillMax = 2;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp;
    public int torch = 0;
    public int sight = 1;

    public Player(string name, ClassName className, int cap, int maxHp, int sol, int lun, int con) : base(name, className, 1, sol, lun, con, maxHp, cap)
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
        switch (ClassName)
        {
            case ClassName.Warrior:
                PickupItemData(Inventoriable.SkillDb.Charge);
                PickupItemData(Inventoriable.SkillDb.Berserk);
                stat[Stats.Sol] += 1;
                break;
            case ClassName.Assassin:
                PickupItemData(Inventoriable.SkillDb.ShadowAttack);
                PickupItemData(Inventoriable.SkillDb.Backstep);
                stat[Stats.Lun] += 1;
                break;
            case ClassName.Mage:
                PickupItemData(TorchData.data);
                PickupItemData(TorchData.data);
                stat[Stats.Con] += 1;
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
                stat[Stats.Sol] += 1;
                break;
            case 1:
                stat[Stats.Lun] += 1;
                break;
            case 2:
                stat[Stats.Con] += 1;
                break;
        }
        Hand.Cap = new Mul(3, 0.3f, Mul.lv);
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
    public void PickupItemData(IItemData data) => PickupItem(data.Instantiate(this, stat));
    private void PickupItem(IItem item)
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
        var skills = from s in Inven.Content where s is not null && s.itemType == ItemType.Skill select s;
        foreach (var item in skills)
        {
            item.stack = skillMax;
        }
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
    public override string ToString() => base.ToString() + $"\nExp : {exp}\tTorch : {torch}\tMem : {Process.GetCurrentProcess().PrivateMemorySize64}";
    public override char ToChar() => MapSymb.player;
}
