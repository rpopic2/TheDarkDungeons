namespace Entities;
public class Player : Fightable
{
    public const int BASICCAP = 3;
    public const int BASICSTAT = 0;
    private static readonly string[] _STATPROMPT = new string[] { "^r(q) 힘/체력^/", "^g(w) 정밀/민첩^/", "^b(e) 마력/지능^/" };
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp { get; init; }
    public Player(string name) : base(name, level: 1, sol: BASICSTAT, lun: BASICSTAT, con: BASICSTAT, cap: BASICCAP, pos: new(0))
    {
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
    }
    public ISteppable? UnderFoot => Map.Current.Steppables[Pos.x];
    public override void SelectAction()
    {
        do
        {
            Selection();
        } while (Stance.CurrentBehav is null);

        void Selection()
        {
            ConsoleKeyInfo info = IO.rk();
            ConsoleKey key = info.Key;
            switch (key)
            {
                case ConsoleKey.RightArrow:
                case ConsoleKey.L:
                    if (info.Modifiers == ConsoleModifiers.Control) IO.Redraw();
                    else if (CanMove(Position.MOVERIGHT)) SelectBasicBehaviour(0, Position.MOVERIGHT.x, (int)Position.MOVERIGHT.facing);
                    break;
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    IO.Redraw();
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.H:
                    if (CanMove(Position.MOVELEFT)) SelectBasicBehaviour(0, Position.MOVELEFT.x, (int)Position.MOVELEFT.facing);
                    break;
                case ConsoleKey.B:
                case ConsoleKey.Insert:
                    SelectBehaviour(Fightable.bareHand);
                    break;
                case ConsoleKey.N:
                case ConsoleKey.OemPeriod:
                case ConsoleKey.Delete: //Rest
                    SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                    break;
                case ConsoleKey.Spacebar: //상호작용
                case ConsoleKey.Enter: //상호작용
                    if (UnderFoot is not null) SelectBasicBehaviour(2, 0, 0);
                    break;
                default:
                    DefaultSwitch(info);
                    break;
            }
        }
        void DefaultSwitch(ConsoleKeyInfo key)
        {
            bool found = IO.chk(key.KeyChar, Inven.Count, out int i);
            if (found && Inven[i] is Item item)
            {
                SelectBehaviour(item);
                return;
            }
            bool found2 = IO.chkp(key.Key, Inven.Count, out int i2);
            if (found2 && Inven[i2] is Item item2)
            {
                SelectBehaviour(item2);
                return;
            }

            switch (key.KeyChar)
            {
                case 'i':
                case '*':
                    IO.DrawInventory();
                    break;
                case '/':
                case 'm':
                    IO.ShowStats();
                    break;
                case '?':
                case '5':
                    IO.ShowHelp();
                    break;
            }
        }
    }
    public void SelectBehaviour(Item item)
    {
        IO.del(__.bottom);
        IO.sel(item.skills, __.bottom, out int index, out bool cancel, out _, out _, $"{item.Name} : ");
        if (cancel)
        {
            IO.Redraw();
            return;
        }
        SelectBehaviour(item, index);
    }
    public void SelectPickupStat(int times)
    {
        for (int i = 0; i < times; i++)
        {
            IO.pr($"남은 능력치 포인트 : {times - i}");
            SelectPickupStat();
            IO.del();
        }
    }
    public void SelectPickupStat()
    {
        int index;
        do
        {
            IO.pr($"현재 : {Stat}\n");
            IO.sel(_STATPROMPT, 0, out index, out _, out _, out _);
            IO.del(2);
        } while (index == -1);
        Stat[(StatName)index] += 1;
    }
    public void PickupItem(Item item)
    {
    Select:
        IO.pr($"\n아이템을 얻었다. {item.Name}");
        if (Inven.Count != Inventory.INVENSIZE)
        {
            Inven.Add(item);
            return;
        }
        IO.sel(Inven, __.fullinven, out int index, out bool cancel, out _, out _);
        IO.del();
        if (cancel) return;

        if (index < Inven.Count && Inven[index] is Item old)
        {
            do
            {
                ConsoleKeyInfo keyInfo = IO.rk($"{old.Name}이 버려집니다. 계속하시겠습니까?");
                if (keyInfo.Key.IsOK())
                {
                    Inven.Remove(old);
                    break;
                }
                else if (keyInfo.Key.IsCancel())
                {
                    IO.del();
                    goto Select;
                }
            } while (true);
        }
        Inven.Add(item);
    }
    protected override void Interact()
    {
        if (UnderFoot is null) return;
        else if (UnderFoot is Corpse corpse) PickupCorpse(corpse);
        else if (UnderFoot is Portal portal)
        {
            IO.pr($"{Name}은 구멍 속으로 떨어졌다!");
            Map.Current.LoadNewMap = true;
        }
    }
    private void PickupCorpse(Corpse corpse)
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
            Map.Current.Steppables[Pos.x] = null;
        }
    }
    public void SelectStartItem()
    {
        Corpse corpse = new("누군가", new() { torch, sword, shield, dagger, bow, arrow, staff, magicBook });
        while (Inven.Count < 3)
        {
            IO.sel(corpse.droplist.ToArray(), 0, out int index, out bool cancel, out _, out _);
            if (cancel) break;
            if (corpse.droplist[index] is Item item)
            {
                PickupItem(item);
                corpse.droplist.Remove(item);
            }
        }
    }
    private void OnLvUp(object? sender, EventArgs e)
    {
        //1레벨마다 1솔씩, 5레벨마다 1캡씩, 1레벨마다 1체력씩
        Level++;
        exp.UpdateMax();
        IO.pr($"{Level}레벨이 되었다.", __.emphasis);
        SelectPickupStat();
        Stat.Heal(GetHp().Max / 2);
    }
    public override string ToString() => base.ToString() + $"\nExp : {exp}";
    public override char ToChar() => IsAlive ? MapSymb.player : MapSymb.corpse;
}
