public partial class Player : Creature
{
    public const int BASICCAP = 3;
    public const int BASICSTAT = 1;
    private static readonly string[] _STATPROMPT = new string[] { "^r (힘/체력)^/", "^g (정밀/민첩)^/", "^b (마력/지능)^/" };
    public static Player? _instance;
    public static Player instance { get => _instance ?? throw new Exception("Player was not initialised"); }
    public Exp exp { get; init; }
    public Player(string name, int classIndex) : base(name, level: 1, Status.BasicStatus, energy: BASICCAP, pos: new(0))
    {
        Map.OnNewMap += () => OnNewMap();
        exp = new Exp(this);
        exp.point.OnOverflow += new EventHandler(OnLvUp);
        if (classIndex == -1) SelectPickupStat(3);
        else Stat[(StatName)classIndex] += 3;
    }
    public ISteppable? UnderFoot => _currentMap.GetSteppable(Pos.x);

    private void OnNewMap()
    {
        _currentMap.AddToOnTurnPre(_turnPre);
        _currentMap.AddToOnTurnEnd(_turnEnd);
    }
    public void SelectBehaviour(Item item)
    {
        IO.del(__.bottom);
        IO.selOnce(item.skills, out int index, __.bottom, $"{item.Name}으로 무엇을 할까 : ");
        if (index == -1)
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
            IO.del(1);
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
            IO.sel(selection, out index);
        } while (index == -1);
        Stat[(StatName)index] += 1;
    }
    public bool PickupItem(Item item, ItemMetaData metaData)
    {
        IO.pr($"\n아이템을 얻었다. {item.Name}");
        Inven.Add(item, metaData, out bool success);
        IO.Redraw();
        return success;
    }
    public bool PickupItem(Item item, int stack = 1, bool redraw = true)
    {
        IO.pr($"\n아이템을 얻었다. {item.Name}");
        ItemMetaData metaData = new();
        metaData.stack = stack;
        Inven.Add(item, metaData, out bool success);
        if (redraw) IO.Redraw();
        return success;
    }
    private int SelectIndexOfItem(string message)
    {
        IO.sel(Inven, out int index, 0, message);
        return index;
    }
    public bool DiscardItem()
    {
    Start:
        int index = SelectIndexOfItem("버릴 아이템을 선택해 주십시오 : ");
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
        if (item is not null)
        {
            base.Charge(item);
            return;
        }
        IO.pr("마법부여할 대상을 선택해 주십시오.");
        IO.sel(Inven, out int index);
        IO.del();
        if (Inven[index] is Item item2) Charge(item2);
    }
    protected override void PoisonItem(Item? item = null)
    {
        IO.pr("독을 바를 대상을 선택해 주십시오.");
        IO.sel(Inven, out int index);
        IO.del();
        if (index == -1) return;
        if (Inven[index] is Item item2) base.PoisonItem(item2);
    }
    protected override void Interact()
    {
        if (UnderFoot is null) return;
        else if (UnderFoot is Corpse corpse) PickupCorpse(corpse);
        else if (UnderFoot is Pit portal)
        {
            IO.pr($"{Name}은 구멍 속으로 떨어졌다!");
            _currentMap.DoLoadNewMap = true;
        }
        else if (UnderFoot is Door door)
        {
            IO.pr($"{Name}은 이쪽 길로 가기로 했다.");
            _currentMap.DoLoadNewMap = true;
        }

    }
    private void PickupCorpse(Corpse corpse)
    {
        while (corpse.droplist.Count > 0)
        {
            PickAnItemFromCorpse(corpse, out bool cancel);
            if (cancel) return;
        }
        _currentMap.OnCorpsePickUp(corpse);
    }
    private void PickAnItemFromCorpse(Corpse corpse, out bool cancel)
    {
        IO.sel(corpse.droplist, out int index, 0, "주울 아이템 선택 : ");
        cancel = index == -1;
        if (cancel) return;
        if (corpse.droplist[index] is Item item)
        {
            int stack = corpse.droplist.GetMeta(item)?.stack ?? 1;
            bool pickedUp = PickupItem(item, stack);
            if (pickedUp) corpse.droplist.Remove(item);
        }
    }
    public void SelectStartItem()
    {
        Corpse corpse = new("누군가", new(Player.instance, "시체"));
        corpse.droplist.Content.Add(torch);
        corpse.droplist.Content.Add(sword);
        corpse.droplist.Content.Add(shield);
        corpse.droplist.Content.Add(dagger);
        corpse.droplist.Content.Add(bow);
        ItemMetaData arrowMeta = new();
        arrowMeta.stack = 3;
        corpse.droplist.Content.Add(arrow);
        corpse.droplist.Content.Add(staff);
        corpse.droplist.Content.Add(magicBook);
        while (Inven.Count < 3)
        {
            IO.sel(corpse.droplist, out int index, 0, "아이템 선택 : ");
            if (corpse.droplist[index] is Item item)
            {
                Inven.Add(item);
                corpse.droplist.Remove(item);
            }
            PickAnItemFromCorpse(corpse, out bool cancel);
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
