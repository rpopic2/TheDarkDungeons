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
    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object value, __ flag = 0, string title = "선택 : ")
    {
        if (value is Array array)
        {
            value = array.ToFString(title);
        }
        if (flag.HasFlag(__.emphasis)) value = EMPHASIS + value;
        if (flag.HasFlag(__.newline)) value = "\n" + value;
        if (flag.HasFlag(__.bottom))
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = x + Console.WindowHeight - 1;
            Console.Write(value);
            Console.SetCursorPosition(x, y);
            return;
        }
        if (flag.HasFlag(__.write)) Console.Write(value);
        else Console.WriteLine(value);
    }
    ///<summary>Print in Formated Options</summary>


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
        IO.pr($"턴 : {Game.Turn}  깊이 : {Map.level}\tHP : {s_player.Hp}  Level : {s_player.Level} ({s_player.exp})", __.bottom | __.newline);
        IO.pr($"{s_player.tokens}\t 상대 : {s_player.FrontFightable?.tokens}", __.bottom | __.newline);
        IO.pr(s_player.Inven, __.bottom | __.newline);
        IO.pr(Map.Current);
        if (s_player.UnderFoot is ISteppable step) IO.pr(step.name + " 위에 서 있다. (spacebar)");
    }
}