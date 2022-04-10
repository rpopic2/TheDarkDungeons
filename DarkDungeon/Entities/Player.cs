namespace Entities;

public class Player : Inventoriable
{
    public const int skillMax = 2;
    public const int BASICCAP = 3;
    public const int BASICSTAT = 3;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public string underFoot = "";
    public Exp exp;
    public int torch = 0;
    public Player(string name) : base(name, level: 1, sol: BASICSTAT, lun: BASICSTAT, con: BASICSTAT, maxHp: 3, cap: BASICCAP, pos: new(0))
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
    }
    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        Level++;
        exp.UpdateMax();
        IO.pr($"{Level}레벨이 되었다.", true);
        SelectPickupStat();
        Hp.Max = new Mul(3, Mul.n, Level);
        Hp += Level;
    }
    public void SelectPickupStat()
    {
        bool cancel;
        int index;
        do
        {
            IO.seli(Program.stats, out index, out cancel, out ConsoleModifiers mod, out _);
        } while (cancel);
        stat[(StatName)index] += 1;
    }
    public void PickupItem(Item item)
    {
        IO.pr($"\n아이템을 얻었다. {item.name}");
        IO.seli_i(out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;
        PickupItem(item, index);
    }
    public void PickupToken(TokenType token)
    {
        int discard = -1;
        if (tokens.IsFull)
        {
            IO.pr("손패가 꽉 찼습니다. 버릴 토큰을 고르십시오. " + Tokens.ToString(token));
            IO.seli_t(out discard, out bool cancel2, out _);
            IO.del();
            if (cancel2) return;
        }
        _PickupToken(token, discard);

        IO.pr($"{Tokens.ToString(token)} 토큰을 얻었습니다.");
        IO.rk();
        IO.del();
    }
    public void SelectPickupToken()
    {
        IO.seli(Tokens.TokenPromptNames, out int index, out bool cancel, out _, out _);
        if (cancel) return;
        PickupToken((TokenType)index);
    }
    public void Rest()
    {
        _Rest();
        IO.seli(Tokens.TokenPromptNames, out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;
        PickupToken((TokenType)index);
    }
    public TokenType? SelectToken()
    {
        do
        {
            IO.seli_t(out int index, out bool cancel, out _);
            if (cancel) return null;
            if (tokens[index] is byte result) return (TokenType)result;
        } while (true);
    }
    public override void Move(int x)
    {
        bool success = Move(x, out char obj);
        if (!success) return;
        if (obj == MapSymb.corpse)
        {
            underFoot = "발밑 : 시체";
        }
        else
        {
            underFoot = "";
        }
        if (obj == MapSymb.portal)
        {
            Map.NewMap();
            Pos = new Position();
        }
    }
    public void PickUpCorpse()
    {
        if (underFoot == "") return;
        PickupItem(Item.torch);
        Map.Current.corpses[Pos.x] = null;
        underFoot = "";
        Stance.Set(StanceName.Charge, 0);
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
