namespace Entities;
public class Player : Fightable
{
    public const int BASICCAP = 3;
    public const int BASICSTAT = 1;
    private static readonly string[] _STATPROMPT = new string[] { "^r (힘/체력)^/", "^g (정밀/민첩)^/", "^b (마력/지능)^/" };
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp { get; init; }
    public Player(string name) : base(name, level: 1, new(BASICSTAT, BASICSTAT, default), energy: BASICCAP, pos: new(0))
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
        } while (Status.CurrentBehav is null);

        void Selection()
        {
            ConsoleKeyInfo info = IO.rk();
            ConsoleKey key = info.Key;
            if (key.IsCancel())
            {
                DiscardItem();
                IO.Redraw();
                return;
            }
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
                case ConsoleKey.N:
                case ConsoleKey.OemPeriod:
                case ConsoleKey.Delete: //Rest
                    SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                    break;
                case ConsoleKey.Z: //상호작용
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
            bool found = IO.chk(key.KeyChar, Inventory.INVENSIZE, out int i);
            if (!found) IO.chkp(key.Key, Inventory.INVENSIZE, out i);
            if (found)
            {
                if (i >= Inven.Count)
                {
                    SelectBehaviour(bareHand);
                    return;
                }
                else if (Inven[i] is Item item)
                {
                    SelectBehaviour(item);
                    return;
                }
            }

            switch (key.KeyChar)
            {
                case '.':
                    SelectBasicBehaviour(1, 0, -1);
                    break;
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
        IO.selOnce(item.skills, __.bottom, out int index, out bool cancel, out _, out _, $"{item.Name}으로 무엇을 할까 : ");
        if (index == -1 || cancel)
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
        string[] selection = new string[3];
        do
        {
            for (int i = 0; i < _STATPROMPT.Length; i++)
            {
                selection[i] = $"{_STATPROMPT[i]} : {Stat[(StatName)i]} / ";
            }
            IO.sel(selection, 0, out index, out _, out _, out _);
        } while (index == -1);
        Stat[(StatName)index] += 1;
    }
    public bool PickupItem(Item item)
    {
        IO.pr($"\n아이템을 얻었다. {item.Name}");
        Inven.Add(item, out bool success);
        IO.Redraw();
        return success;
    }
    public int SelectIndexOfItem(string message)
    {
        IO.sel(Inven, out int index, 0, message);
        return index;
    }
    public bool DiscardItem()
    {
    Start:
        int index = SelectIndexOfItem("버릴 아이템을 선택해 주십시오");
        if (index <= -1) return false;
        Item selected = Inven[index]!;

    Confirm:
        ConsoleKey key = IO.rk($"{selected.Name}이 버려집니다. 계속하시겠습니까?").Key;
        if (key.IsOK())
        {
            Inven.Remove(selected);
            return true;
        }
        else if (key.IsCancel()) goto Start;
        else goto Confirm;
    }
    protected override void Charge(Item? item = null)
    {
        IO.pr("마법부여할 대상을 선택해 주십시오.");
        IO.sel(Inven, 0, out int index, out _, out _, out _);
        IO.del();
        if (Inven[index] is Item item2) Charge(item2);
    }
    protected override void PoisonItem(Item? item = null)
    {
        IO.pr("독을 바를 대상을 선택해 주십시오.");
        IO.sel(Inven, 0, out int index, out _, out _, out _);
        IO.del();
        if (index == -1) return;
        if (Inven[index] is Item item2) base.PoisonItem(item2);
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
            PickAnItemFromCorpse(corpse);
        }
        if (corpse.droplist.Count() <= 0)
        {
            Map.Current.Steppables[Pos.x] = null;
        }
    }
    private void PickAnItemFromCorpse(Corpse corpse)
    {
            IO.sel(corpse.droplist.ToArray(), 0, out int index, out bool cancel, out _, out _);
            if (cancel) return;
            if (corpse.droplist[index] is Item item)
            {
                bool pickedUp = PickupItem(item);
                if (pickedUp) corpse.droplist.Remove(item);
            }
    }
    public void SelectStartItem()
    {
        Corpse corpse = new("누군가", new() { torch, sword, shield, dagger, bow, arrow, staff, magicBook });
        while (Inven.Count < 3)
        {
            PickAnItemFromCorpse(corpse);
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
    public override char ToChar() => IsAlive ? MapSymb.player : MapSymb.playerCorpse;
}
