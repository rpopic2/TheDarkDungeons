public static class IO
{
    private const string EMPHASIS = "=> ";
    public const string ITEMKEYS1 = "qwert";
    public static readonly ConsoleKey[] ITEMKEYS_PAD = new ConsoleKey[] { ConsoleKey.End, ConsoleKey.PageDown, ConsoleKey.Home, ConsoleKey.PageUp, ConsoleKey.OemPlus };
    private static readonly string DELSTRING = " ";
    private static Player s_player { get => Player.instance; }
    static IO()
    {
        DELSTRING = new String(' ', Console.WindowWidth - 1);
    }
    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object value, __ flag = 0, string title = "선택 : ")
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        string stringValue;
        if (value is Array arrayValue) stringValue = arrayValue.ToFString(title);
        else stringValue = value.ToString() ?? string.Empty;
        if (flag.HasFlag(__.emphasis)) stringValue = EMPHASIS + stringValue;
        if (flag.HasFlag(__.newline)) stringValue += "\n";
        if (flag.HasFlag(__.bottom)) Console.CursorTop = x + Console.WindowHeight - 1;
        if (stringValue.Contains("^"))
        {
            pr_Color(stringValue, flag);
            if (!flag.HasFlag(__.bottom)) Console.WriteLine();
            else Console.SetCursorPosition(x, y);
            return;
        }
        else if (flag.HasFlag(__.bottom))
        {
            Console.Write(stringValue);
            Console.SetCursorPosition(x, y);
        }
        else Console.WriteLine(stringValue);
    }
    private static void pr_Color(string value, __ flags)
    {
        string[] splits = value.Split('^', StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in splits)
        {
            if (item.StartsWith("b")) Console.ForegroundColor = ConsoleColor.Blue;
            else if (item.StartsWith("g")) Console.ForegroundColor = ConsoleColor.Green;
            else if (item.StartsWith("r")) Console.ForegroundColor = ConsoleColor.Red;
            else if (item.StartsWith("/")) Console.ResetColor();
            else if (!item.StartsWith('c'))
            {
                Console.Write(item);
                continue;
            }
            Console.Write(item.Substring(1));
        }
        Console.ResetColor();
    }
    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object print, __ flags = 0, string title = "선택 : ")
    {
        pr(print, flags, title);
        ConsoleKeyInfo info = Console.ReadKey(true); ;
        del(flags);
        return info;
    }
    public static void sel(object value, __ flags, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo, string title = "선택 : ")
    {
        bool found;
        int max = Inventory.INVENSIZE;
        if (!flags.HasFlag(__.fullinven))
        {
            if (value is Array a) max = a.Length;
            else if (value is Tokens t) max = t.Count;
            else if (value is Inventory inv) max = inv.Count;
        }
        do
        {
            keyInfo = rk(value, flags, title);
            mod = keyInfo.Modifiers;
            cancel = keyInfo.Key.IsCancel();
            found = chk(keyInfo.KeyChar, max, out index);
            if (index == -1) found = IO.chkp(keyInfo.Key, max, out index);
            if (cancel) return;
        } while (!found);
    }
    public static bool chk(Char i, int max, out int index)
    {
        index = ITEMKEYS1.IndexOf(i);
        return index != -1 && index <= max - 1;
    }
    public static bool chkp(ConsoleKey i, int max, out int index)
    {
        index = Array.IndexOf(ITEMKEYS_PAD, i);
        return index != -1 && index <= max - 1;
    }
    public static void del(__ flags = 0)
    {
        if (flags.HasFlag(__.bottom))
        {
            s_del_bottom();
            return;
        }
        if (Console.CursorTop == 0) return;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr(DELSTRING, flags);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
    private static void s_del_bottom()
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        Console.CursorTop = x + Console.WindowHeight - 1;
        Console.Write(DELSTRING);
        Console.SetCursorPosition(x, y);
    }
    public static void del(int lines)
    {
        for (int i = 0; i < lines; i++) del();
    }
    public static void Redraw()
    {
        Console.Clear();
        //pr("History");
        pr($"턴 : {Program.Turn}  깊이 : {Map.Depth}\t레벨 : {s_player.Level} ({s_player.exp})  Hp : {s_player.GetHp()}", __.bottom | __.newline);
        pr($"기력 : {s_player.Tokens}\t 상대 : {s_player.FrontFightable?.Tokens}", __.bottom | __.newline);
        pr(s_player.Inven, __.bottom);
        pr(Map.Current);
        if (s_player.UnderFoot is ISteppable step) pr(step.name + " 위에 서 있다. (spacebar)");
    }
    public static void ShowHelp()
    {
        Console.Clear();
        pr("키 도움말\n");
        pr("\n 좌우 화살표 = 이동");
        pr("\n q = 1번 아이템(혹은 선택지), w = 2번 아이템, e = 3번 아이템, r = 4번 아이템, t = 5번 아이템 / i : 인벤토리");
        pr("\n , = 맨손    . = 휴식");
        pr("\n / = 내 정보 보기, ? : 이 도움말 보기");
        pr("\n x = 취소, spacebar = Ok 상호작용");
        pr("\n ctrl + L = 새로고침 (맵이 이상하면 누르기)");

        pr("여기에서 m : 모바일 단축키 보기, 5 : 숫자패드 단축키 보기", __.bottom);
        ConsoleKeyInfo consoleKeyInfo = rk();
        if (consoleKeyInfo.KeyChar == 'm') ShowMobileHelp();
        else if (consoleKeyInfo.KeyChar == 'n') ShowNumpadHelp();
        else Redraw();
    }

    private static void ShowNumpadHelp()
    {
        Console.Clear();
        pr("숫자패드 키 도움말 (Num Lock 끄고 플레이)\n");
        pr("\n 좌우 화살표(4, 6) = 좌우 이동");
        pr("\n End(1) = 1번 아이템(혹은 선택지), PgDn(2) = 2번 아이템, Home(7) = 3번 아이템, PgUp(9) = 4번 아이템, + = 5번 아이템 / * : 인벤토리");
        pr("\n Ins(0) = 맨손, DEL = 휴식");
        pr("\n / = 내 정보 보기, 5(NumLock 켜고) : 이 도움말 보기");
        pr("\n - = 취소, Enter = Ok 상호작용");
        pr("\n 0(NumLock 켜고) = 새로고침 (맵이 이상하면 누르기)");
        rk();
        Redraw();
    }

    public static void ShowMobileHelp()
    {
        Console.Clear();
        pr("모바일 키 도움말\n");
        pr("\n h, l = 좌우 이동");
        pr("\n q = 1번 아이템(혹은 선택지), w = 2번 아이템, e = 3번 아이템, r = 4번 아이템, t = 5번 아이템 / i : 인벤토리");
        pr("\n b = 맨손, n = 휴식");
        pr("\n m = 내 정보 보기, ? : 이 도움말 보기");
        pr("\n x = 취소, spacebar = Ok 상호작용");
        pr("\n 0 = 새로고침 (맵이 이상하면 누르기)");

        rk();
        Redraw();
    }
    public static void ShowStats()
    {
        rk(s_player.ToString());
        Redraw();
    }
    public static void DrawInventory()
    {
        Console.Clear();
        foreach (Item item in s_player.Inven.OfType<Item>())
        {
            pr($"{item.Name} | 종류 : {item.itemType} ");
            foreach (IBehaviour behav in item.skills)
            {
                pr($"\t{behav.ToString()} {behav.Stance}");
            }
            pr("\n");
        }
        rk();
        Redraw();
    }
}