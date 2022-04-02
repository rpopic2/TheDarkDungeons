namespace Entities;

public class Player : Inventoriable
{
    public const int skillMax = 2;
    public const int basicCap = 3;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp;
    public int torch = 0;
    public int sight = 1;
    public Player(string name, ClassName className) : base(name, className, level: 1, sol: 2, lun: 2, con: 2, maxHp: 3, cap: basicCap)
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
    }
    public void StartItem()
    {
        switch (ClassName)
        {
            case ClassName.Warrior:
                PickupItemData(Inventoriable.SkillDb.Charge);
                //PickupItemData(Inventoriable.SkillDb.Berserk);
                stat[Stats.Sol] += 1;
                break;
            case ClassName.Assassin:
                PickupItemData(Inventoriable.SkillDb.ShadowAttack);
                //PickupItemData(Inventoriable.SkillDb.Backstep);
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
            IO.seln(Program.stats, out index, out cancel, out ConsoleModifiers mod);
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
    public void PickupItemData(IItemData data) => PickupItem(data.Instantiate(this, stat));
    private void PickupItem(IItem item)
    {
        IO.pr("\nFound an item." + item.abv);
        IO.seln_i(out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel)
        {
            IO.del(2);
            return;
        }
        PickupItem(item, index);
        IO.del(2);
    }
    public void Exile()
    {
        throw new NotImplementedException();

        // int index;
        // Card card;
        // do
        // {
        //     IO.seln_h(out index, out bool cancel, out ConsoleModifiers mod);
        //     card = Hand[index] ?? throw new Exception();
        //     if (cancel) return;
        // } while (card.Stance == CardStance.Star);
        // IO.pr("Exiled a card.");
        // Hand[index] = Hand[index]?.Exile();
        // stance = new(Stance.Exile, default);
    }
    public override void Rest()
    {
        base.Rest();
        IO.pr("토큰 종류를 선택해 주십시오.");
        IO.seln(Tokens.TokenPromptNames, out int index, out _);
        IO.del();

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

        var skills = from s in Inven.Content where s is not null && s.itemType == ItemType.Skill select s;
        foreach (var item in skills)
        {
            item.stack = item.level;
        }
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
            if(tokens[index] is byte result) return(TokenType)result;
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
