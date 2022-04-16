public static class IO
{
    public const ConsoleKey OKKEY = ConsoleKey.Spacebar;
    public const ConsoleKey CANCELKEY = ConsoleKey.X;
    private const string EMPHASIS = "=> ";
    public const string ITEMKEYS1 = "qwert";
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
        if (value is Array array)
        {
            value = array.ToFString(title);
        }
        if (flag.HasFlag(__.emphasis)) value = EMPHASIS + value;
        if (flag.HasFlag(__.newline)) value += "\n";
        if (flag.HasFlag(__.bottom))
        {
            Console.CursorTop = x + Console.WindowHeight - 1;
        }
        if (flag.HasFlag(__.color_on))
        {
            string v = (string)value;
            string[] splits = v.Split('^', StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in splits)
            {
                if (item.StartsWith("b")) Console.ForegroundColor = ConsoleColor.Blue;
                else if (item.StartsWith("g")) Console.ForegroundColor = ConsoleColor.Green;
                else if (item.StartsWith("r")) Console.ForegroundColor = ConsoleColor.Red;
                else if (item.StartsWith("/")) Console.ResetColor();
                else
                {
                    Console.Write(item);
                    continue;
                }
                Console.Write(item.Substring(1));
            }
            Console.ResetColor();
            if (!flag.HasFlag(__.bottom)) Console.WriteLine();
            else Console.SetCursorPosition(x, y);
        }
        else
        {
            if (flag.HasFlag(__.bottom))
            {
                Console.Write(value);
                Console.SetCursorPosition(x, y);
            }
            else Console.WriteLine(value);
        }
    }
    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object print, __ flags = 0, string title = "선택 : ")
    {
        pr(print, flags, title);
        ConsoleKeyInfo info = rk();
        del();
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
            cancel = keyInfo.Key == CANCELKEY;
            found = chk(keyInfo.KeyChar, max, out index);
            if (cancel) return;
        } while (!found);
    }
    public static bool chk(Char i, int max, out int index)
    {
        index = ITEMKEYS1.IndexOf(i);
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
        for (int i = 0; i < lines; i++)
        {
            del();
        }
    }

    public static void DrawScreen()
    {
        Console.Clear();
        //IO.pr("History");
        GamePoint hp = s_player.Hp;
        string tempHp = hp.Cur <= hp.Max / 2 ? $"^r{hp.ToString()}^/" : hp.ToString();
        IO.pr($"턴 : {Game.Turn}  깊이 : {Map.level}\tHP : {tempHp}  Level : {s_player.Level} ({s_player.exp})", __.bottom | __.newline | __.color_on);
        IO.pr($"{s_player.tokens}\t 상대 : {s_player.FrontFightable?.tokens}", __.bottom | __.newline);
        IO.pr(s_player.Inven, __.bottom);
        IO.pr(Map.Current);
        if (s_player.UnderFoot is ISteppable step) IO.pr(step.name + " 위에 서 있다. (spacebar)");
    }
}