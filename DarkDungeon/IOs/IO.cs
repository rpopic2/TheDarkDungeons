namespace DarkDungeon;

public static class IO
{
// public:

    public const string ITEMKEYS1 = "qwert";
    public static readonly ConsoleKey[] ITEMKEYS_PAD = new ConsoleKey[] {
        ConsoleKey.End, ConsoleKey.PageDown, ConsoleKey.Home, ConsoleKey.PageUp, ConsoleKey.OemPlus
    };

    public static bool IsInteractive = CSIO.IsInteractive;
    public static IIO IIO { get => s_io; set => s_io = value; }
    public static GameSocket? ServerSocket;

    public static async Task<Result> InitIO(bool isServer) {
        if (!isServer) {
            IO.IIO = new CSIO();
            IO.clr();
        }
        else if (isServer) {
            IO.ServerSocket = await GameSocket.New();
            IO.IIO = IO.ServerSocket;
            IO.ServerSocket.pr("<press_any_key>");
        }
        Console.CancelKeyPress += (_, _) => { Environment.Exit(1); };

        return CheckInteractive();
    }


#nullable disable
    static IO() {
        foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if (assembly.FullName?.StartsWith("xunit") ?? false) {
                IO.IsInteractive = false;
                return;
            }
        }
    }

#nullable restore
    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object value, __ flag = 0, string title = "", bool newline = true) {
        if (value is Array arrayValue)
            value = arrayValue.ToFString();

        if (s_io is CSIO)
            CSIO.pr(value, flag, title, newline);
        else if (s_io is GameSocket)
            s_io.pr(title + value.ToString(), newline);
    }

    public static void mes(object value, __ flag = 0, string title = "", bool newline = true) {
        var s = value.ToString();
        if (s is null)
            return;
        if (newline)
            _history.Add(s);
        else
            _history.Append(s);
    }

    static Result CheckInteractive() {
        try {
            IO.pr("The Dungeon of the Mine " + Program.VERSION);
            IO.pr("Press any key to start...");
            ConsoleKeyInfo info = IO.rk(false);
            if (info.Modifiers == ConsoleModifiers.Control && info.Key == ConsoleKey.D) {
                IO.IsInteractive = false;
            }
        } catch (InvalidOperationException) {
            IO.IsInteractive = false;
        }
        if (IO.IsInteractive)
            return Result.Success;
        else
            return Result.Failure;
    }

    ///<summary>read line.</summary>
    public static string rl() {
        return s_io.rl();
    }

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    //[Obsolete("Use rk() instead.")]
    public static ConsoleKeyInfo rk(object print, __ flags = 0, string title = "") {
        pr(print, flags, title);
        ConsoleKeyInfo info = rk();
        del();
        return info;
    }

    public static ConsoleKeyInfo rk(bool printMore = true) {
        if (printMore)
            pr("-더보기-");
        var result = s_io.rk();
        if (printMore)
            del();
        return result;
    }

    /// <summary>
    /// -1 is not found or canceled.
    /// </summary>
    public static void sel(object value, out int index, __ flags = 0, bool dopr = true, string title = "선택 : ")
    => sel(value, flags, out index, out _, out _, out _, dopr, title);

    public static void sel(object value, __ flags, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo, bool dopr = true, string title = "선택 : ") {
        bool found;
        do {
            found = selOnce(value, flags, out index, out cancel, out mod, out keyInfo, dopr, title);
            if (cancel) return;
        } while (!found);
    }

    public static bool selOnce(object value, out int index, __ flags = 0, bool dopr = true, string title = "선택 :")
        => selOnce(value, flags, out index, out _, out _, out _, dopr, title);

    public static bool selOnce(object value, __ flags, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo, bool dopr = true, string title = "선택 : ") {
        bool found;
        int max = Inventory.INVENSIZE;
        if (!flags.HasFlag(__.fullinven)) {
            if (value is Array a)
                max = a.Length;
            else if (value is Inventory inv)
                max = inv.Count;
        }
        if (dopr)
            pr(value, flags, title);
        keyInfo = rk(false);
        mod = keyInfo.Modifiers;
        cancel = keyInfo.Key.IsCancel() || keyInfo.KeyChar == 'x';
        found = chk(keyInfo.KeyChar, max, out index);
        if (index == -1)
            found = IO.chkp(keyInfo.Key, max, out index);
        if (cancel)
            return false;
        return found;
    }

    public static bool chk(Char i, int max, out int index) {
        index = ITEMKEYS1.IndexOf(i);
        bool found = index != -1 && index <= max - 1;
        if (!found)
            index = -1;
        return found;
    }

    public static bool chkp(ConsoleKey i, int max, out int index) {
        index = Array.IndexOf(ITEMKEYS_PAD, i);
        return index != -1 && index <= max - 1;
    }

    public static void del(int lines = 1) {
        for (int i = 0; i < lines; ++i)
            s_io.del();
    }

    public static void clr()
        => s_io.clr();

    public static void OnTurnEnd() {
        _history.Clear();
    }

    public static void Redraw() {
        if (!IsInteractive)
            return;
        clr();
        string? energyTip = default;
        if (s_player.Energy.IsMin)
            energyTip = "기력이 떨어진 상태다. 휴식하는게 좋겠다";
        if (energyTip != default)
            pr(energyTip, __.bottom | __.newline);
        s_io.pr_depth_lv();
        Creature? frontCreature = s_player.CreatureAtFront;
        s_io.pr_monster_hp_energy(frontCreature);
        s_io.pr_hp_energy();
        s_io.pr_inventory();
        s_io.pr_map();
        if (s_player.UnderFoot is ISteppable step) {
            pr(step.name + " 위에 서 있다. (z를 눌러 상호작용)");
            if (ServerSocket is not null && step is Corpse c) {
                ServerSocket.pr_underfoot(c.was);
            }
        }
    }

    public static void PrintTurnHistory() {
        var count = _history.Count;
        if (count <= 0) 
            return;

        for (int i = 0; i < count; i++) {
            if (i != 0 && i % _messageBoxHeight == 0) {
                rk(true);
                del(_messageBoxHeight);
            }
            pr(_history[i]);
        }
    }

    public static void ShowStats() {
        rk(s_player.ToString());
        Redraw();
    }

    public static void DrawInventory() {
        clr();
        Inventory Inven = s_player.Inven;
        pr("^" + s_player.Stat.ToString(), 0);
        pr("\n");
        foreach (ItemOld item in Inven.OfType<ItemOld>()) {
            ItemMetaData metaData = Inven.GetMeta(item)!;
            pr($"{item.ToString()} | 종류 : {metaData} ");
            /*foreach (IBehaviour behav in item.skills)
            {
                pr($"\t{behav.ToString()} {behav.Stance}");
            }*/
        }
        rk();
        Redraw();
    }

// private:

    const int _messageBoxHeight = 8;

    static Player s_player { get => Player.Me; }
    static IIO s_io;
    static List<string> _history = new();

    static void pr(string value, bool newline = true) {
        s_io.pr(value, newline);
    }
}

