namespace Entities;

public class Player : Fightable
{
    public const int BASICCAP = 3;
    public const int BASICSTAT = 2;
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp;
    public Player(string name) : base(name, level: 1, sol: BASICSTAT, lun: BASICSTAT, con: BASICSTAT, maxHp: 3, cap: BASICCAP, pos: new(0))
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
    }
    public ISteppable? UnderFoot => Map.Current.steppables[Pos.x];
    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        Level++;
        exp.UpdateMax();
        IO.pr($"{Level}레벨이 되었다.", __.emphasis);
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
            IO.sel(Program.stats, 0, out index, out cancel, out ConsoleModifiers mod, out _);
        } while (cancel);
        Stat[(StatName)index] += 1;
    }
    public void PickupItem(Item item)
    {
    Select:
        IO.pr($"\n아이템을 얻었다. {item.name}");
        IO.sel(Inven, __.fullinven, out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;

        if (index < Inven.Count && Inven[index] is Item old)
        {
            do
            {
                ConsoleKeyInfo keyInfo = IO.rk($"{old.name}이 버려집니다. 계속하시겠습니까?");
                if (keyInfo.Key == IO.OKKEY)
                {
                    Inven.Remove(old);
                    break;
                }
                else if (keyInfo.Key == IO.CANCELKEY)
                {
                    IO.del();
                    goto Select;
                }
            } while (true);
        }
        Inven.Add(item);
    }
    public void PickupToken(int amount)
    {
        for (int i = amount; i > 0; i--)
        {
            IO.pr($"토큰을 선택하십시오. ({i})");
            IO.sel(Tokens.TokenPromptNames, 0, out int index, out bool cancel, out _, out _);
            IO.del();
            if (cancel) return;
            PickupToken((TokenType)index);
        }
    }
    public void PickupToken(TokenType token)
    {
        int discard = -1;
        if (Toks.IsFull)
        {
            IO.pr("손패가 꽉 찼습니다. 버릴 토큰을 고르십시오. " + Tokens.ToString(token));
            IO.sel(Toks, 0, out discard, out bool cancel2, out _, out _);
            IO.del();
            if (cancel2) return;
        }
        PickupToken(token, discard);
    }
    public void InteractUnderFoot()
    {
        if (UnderFoot is null) return;
        else if (UnderFoot is Corpse corpse) PickupCorpse(corpse);
        else if (UnderFoot is Portal portal)
        {
            Map.NewMap();
            Pos = new Position();
            Map.Current.UpdateFightable(this);
        }

        Stance.Set(StanceName.Charge, 0);
    }
    public void PickupCorpse(Corpse corpse)
    {
        while (corpse.droplist.Count > 0)
        {
            IO.sel(corpse.droplist.ToArray(), 0, out int index, out bool cancel, out _, out _);
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
    public override string ToString() => base.ToString() + $"\nExp : {exp}";
    public override char ToChar() => MapSymb.player;
}
