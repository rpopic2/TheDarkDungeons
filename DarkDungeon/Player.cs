public partial class Player : Creature {
    public const int BASICCAP = 3;
    public const int BASICSTAT = 1;
    private static readonly string[] _STATPROMPT = new string[] {
        "^r (힘/체력)^/", "^g (정밀/민첩)^/", "^b (마력/지능)^/"
    };
    public static Player? _instance;
    public static Player Me {
        get => _instance ?? throw new Exception("Player was not initialised");
        set => _instance = value;
    }
    public ExperiencePoint exp => Stat.Exp;
    public int ReqExp => Stat.Exp.Max;

    public Player(string name)
        : base(name, level: 1, Status.BasicStatus, energy: BASICCAP, pos: new(0)) {
        Map.OnNewMap += ()
            => OnNewMap();
        exp.OnOverflow += new EventHandler(OnLvUp);
    }

    public ISteppable? UnderFoot
        => _currentMap.GetSteppable(Pos.x);

    private void OnNewMap() {
        _currentMap.AddToOnTurnPre(_turnPre);
        _currentMap.AddToOnTurnEnd(_turnEnd);
    }


    private void OnLvUp(object? sender, EventArgs e) {
        SelectPickupStat();
    }

    public void SelectBehaviour(ItemOld item) {
        object? skills = "";
        if (IO.IIO is GameSocket gs) {
            gs.pr_skill(item.skills);
        } else {
            skills = item.skills;
        }
        var text = $"{item.Name}으로 무엇을 할까? (x로 취소) ";
        IO.selOnce(skills, out int index, __.bottom, true, text);
        if (index == -1) {
            IO.Redraw();
            return;
        }
        SelectBehaviour(item, index);
    }

    public void SelectPickupStat(int times) {
        for (int i = 0; i < times; i++) {
            IO.pr($"남은 능력치 포인트 : {times - i}");
            SelectPickupStat();
            IO.del(1);
        }
    }

    public void SelectPickupStat() {
        int index;
        string[] selection = new string[3];
        do {
            for (int i = 0; i < _STATPROMPT.Length; i++) {
                selection[i] = $"{_STATPROMPT[i]} : {Stat[(StatName)i]} / ";
            }
            IO.sel(selection, out index, 0, true, "레벨업!: ");
        } while (index == -1);
        Stat[(StatName)index] += 1;
        Energy = new(3 + Level / 5);
    }

    public bool PickupItem(ItemOld item, ItemMetaData? metaData = null) {
        if (metaData is null)
            metaData = new();
        IO.pr($"\n아이템을 얻었다. {item.Name}");
        Inven.Add(item, metaData, out bool success);
        IO.Redraw();
        return success;
    }

    private int SelectIndexOfItem(string message) {
        IO.sel(Inven, out int index, 0, true, message);
        return index;
    }

    public bool DiscardItem() {
    Start:
        int index = SelectIndexOfItem("버릴 아이템을 선택해 주십시오 (x로 취소) : ");
        if (index <= -1)
            return false;
        ItemOld selected = Inven[index]!;
    Confirm:
        ConsoleKey key = IO.rk($"{selected.Name}이 버려집니다. 계속하시겠습니까?").Key;
        if (key.IsOK()) {
            Inven.Remove(selected);
            return true;
        }
        else if (key.IsCancel()) {
            goto Start;
        }
        else {
            goto Confirm;
        }
    }

    protected override void Charge(ItemOld? item = null) {
        if (item is not null) {
            base.Charge(item);
            return;
        }
        IO.pr("마법부여할 대상을 선택해 주십시오.");
        IO.sel(Inven, out int index);
        IO.del();
        if (Inven[index] is ItemOld item2)
            Charge(item2);
    }

    protected override void PoisonItem(ItemOld? item = null) {
        IO.pr("독을 바를 대상을 선택해 주십시오.");
        IO.sel(Inven, out int index);
        IO.del();
        if (index == -1)
            return;
        if (Inven[index] is ItemOld item2)
            base.PoisonItem(item2);
    }

    protected override void Interact() {
        if (UnderFoot is null) {
            return;
        } else if (UnderFoot is Corpse corpse) {
            PickupCorpse(corpse);
        } else if (UnderFoot is Pit portal) {
            IO.rk($"{Name}은 구멍 속으로 떨어졌다!");
            _currentMap.DoLoadNewMap = true;
        } else if (UnderFoot is Door door) {
            IO.rk($"{Name}은 이쪽 길로 가기로 했다.");
            _currentMap.DoLoadNewMap = true;
        }
    }

    private void PickupCorpse(Corpse corpse) {
        while (corpse.droplist.Count > 0) {
            PickAnItemFromCorpse(corpse, out bool cancel);
            if (cancel)
                return;
        }
        _currentMap.OnCorpsePickUp(corpse);
    }

    private void PickAnItemFromCorpse(Corpse corpse, out bool cancel) {
        if (IO.IIO is GameSocket gs) {
            gs.pr_loot(corpse.droplist);
        }
        IO.sel(corpse.droplist, out int index, 0, true, "주울 아이템 선택 (x로 취소) : ");
        cancel = index == -1;
        if (cancel) {
            IO.del();
            return;
        }
        corpse.GetItemAndMeta(index, out ItemOld? item, out ItemMetaData? metaData);
        if (item is ItemOld) {
            bool pickedUp = PickupItem(item, metaData!);
            if (pickedUp)
                corpse.droplist.Remove(item);
        }
    }

    public override string ToString() => base.ToString() + $"\nExp : {exp}";
    public override char ToChar() => IsAlive ? MapSymb.player : MapSymb.playerCorpse;
}

