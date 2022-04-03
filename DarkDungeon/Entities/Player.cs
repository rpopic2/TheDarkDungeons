namespace Entities;

public class Player : Inventoriable
{
    public const int skillMax = 2;
    public const int basicCap = 3;
    public const int BASICSTAT = 2;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp;
    public int torch = 0;
    public int sight = 1;
    public Player(string name, ClassName className) : base(name, className, level: 1, sol: BASICSTAT, lun: BASICSTAT, con: BASICSTAT, maxHp: 3, cap: basicCap)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
    }
    public void StartItem()
    {
        switch (ClassName)
        {
            case ClassName.Warrior:
                PickupItem(Item.sword);
                stat[StatName.Sol] += 1;
                break;
            case ClassName.Assassin:
                stat[StatName.Lun] += 1;
                break;
            case ClassName.Mage:
                stat[StatName.Con] += 1;
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
            IO.seln(Program.stats, out index, out cancel, out ConsoleModifiers mod);
        } while (cancel);
        stat[(StatName)index] += 1;
        Hand.Cap = new Mul(3, 0.4f, Level);
        Hp.Max = new Mul(3, Mul.n, Level);
        Hp += Level;
    }
    public void PickupCard(Card card)
    {
    Show:
        IO.pr("\nFound a card." + card);
        IO.seln_h(out int index, out bool cancel, out ConsoleKeyInfo keyInfo);
        if (keyInfo.Key == IO.OKKEY)
        {
            card = Card.StanceShift(card);
            IO.del(2);
            goto Show;
        }

        if (cancel)
        {
            IO.del(2);
            return;
        }
        PickupCard(card, index);
        IO.del(2);
    }
    private void PickupItem(Item item)
    {
        IO.pr($"\n아이템을 얻었다. {item.name}");
        IO.seli(out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;
        NewPickupItem(item, index);
    }
    public new void Rest()
    {
        base.Rest();
        IO.pr("토큰 종류를 선택해 주십시오.");
        IO.del();
        IO.seln(Tokens.TokenPromptNames, out int index, out _);

        if (tokens.IsFull)
        {
            IO.pr("손패가 꽉 찼습니다. 버릴 토큰을 고르십시오.");
            IO.seln_t(out int index2, out _, out _);
            IO.del();
            tokens.RemoveAt(index2);
        }
        tokens.Add((byte)index);
        IO.pr($"{Tokens.TokenSymbols[index]} 토큰을 얻었습니다.");
        IO.rk();
        IO.del();
    }
    public override Card? SelectCard()
    {
        do
        {
            IO.seln_h(out int index, out bool cancel, out _);
            if (cancel) return null;
            return Hand[index];
        } while (true);
    }
    public TokenType? SelectToken()
    {
        do
        {
            IO.seln_t(out int index, out bool cancel, out _);
            if (cancel) return null;
            if (tokens[index] is byte result) return (TokenType)result;
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
