namespace Entities;

public class Player : Inventoriable
{
    public const int skillMax = 2;
    public const int BASICCAP = 3;
    public const int BASICSTAT = 3;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public ISteppable? underFoot => Map.Current.steppables[Pos.x];
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
    Select:
        IO.pr($"\n아이템을 얻었다. {item.name}");
        IO.seli_if(out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;

        if (index < Inven.Count && Inven[index] is Item old)
        {
            ConsoleKeyInfo keyInfo = IO.rk($"{old.name}이 버려집니다. 계속하시겠습니까?");
            if (keyInfo.Key == IO.OKKEY) Inven.Remove(old);
            else goto Select;
        }
        Inven.Add(item);
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

        //IO.pr($"{Tokens.ToString(token)} 토큰을 얻었습니다.");
        //IO.rk();
        //IO.del();
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
        IO.pr("토큰을 획득하였습니다.");
        IO.seli(Tokens.TokenPromptNames, out int index, out bool cancel, out _, out _);
        IO.del(2);
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
    }
    public void InteractUnderFoot()
    {
        if (underFoot is null) return;
        else if (underFoot is Corpse corpse) PickupCorpse(corpse);
        else if (underFoot is Portal portal)
        {
            Map.NewMap();
            Pos = new Position();
            Map.Current.UpdateMoveable(this);
        }

        Stance.Set(StanceName.Charge, 0);
    }
    public void PickupCorpse(Corpse corpse)
    {
        while (corpse.droplist.Count > 0)
        {
            IO.seli(corpse.droplist.ToArray(), out int index, out bool cancel, out _, out _);
            if (cancel) break;
            if (corpse.droplist[index] is Item item)
            {
                PickupItem(item);
                corpse.droplist.Remove(item);
            }
        }
        if (corpse.droplist.Count() <= 0)
        {
            Map.Current.steppables[Pos.x] = null;
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
