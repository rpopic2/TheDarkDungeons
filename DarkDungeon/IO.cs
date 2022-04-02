public static class IO
{
    public const ConsoleKey CANCELKEY = ConsoleKey.Q;
    public const ConsoleKey OKKEY = ConsoleKey.Spacebar;
    private const string Emphasis = "=> ";
    private const string delString = "                                                                                       ";
    private static Player player { get => Player.instance; }
    public static int printCount;

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object print, bool emphasis = false, bool newline = false)
    {
        pr(print, emphasis, newline);
        ConsoleKeyInfo info = rk();
        del();
        return info;
    }
    public static void seln(Array value, out int index, out bool cancel, out ConsoleModifiers mod)
    {
        _seln(value, out index, out cancel, out mod, out _, value.Length);
    }
    public static void seln(Array value, out int index, out ConsoleKeyInfo keyInfo)
    {
        _seln(value, out index, out _, out _, out keyInfo, value.Length);
    }
    private static void _seln(object print, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo, int max)
    {
        bool found;
        do
        {
            keyInfo = rk(print);
            mod = keyInfo.Modifiers;
            cancel = keyInfo.Key == CANCELKEY;
            bool ok = keyInfo.Key == OKKEY;
            found = chkn(keyInfo.KeyChar, max, out index);
            if (cancel || ok) return;
        } while (!found);
    }
    public static bool chkn(Char i, int max, out int index)
    {
        index = (int)Char.GetNumericValue(i);
        if (index == 0) index = 10;
        if (index != -1) index--;
        return index != -1 && index <= max - 1;
    }
    ///<summary>Select from hand</summary>
    public static void seln_h(out int result, out bool cancel, out ConsoleKeyInfo keyInfo) =>
        _seln(player.Hand, out result, out cancel, out _, out keyInfo, player.Hand.Cap);
    ///<summary>Select from inventory</summary>
    public static void seln_i(out int result, out bool cancel, out ConsoleModifiers mod) =>
    _seln(player.Inven, out result, out cancel, out mod, out _, player.Inven.Cap);
    public static void seln_t(out int result, out bool cancel, out ConsoleModifiers mod) =>
    _seln(player.tokens, out result, out cancel, out mod, out _, player.Hand.Cap);

    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x, bool emphasis = false, bool newline = false)
    {
        if (x is Array array)
        {
            _prfo(array); return;
        }
        if (emphasis) x = Emphasis + x;
        if (newline) x = "\n" + x;
        Console.WriteLine(x);
        printCount++;
    }
    public static void prb(object text, bool emphasis = false, bool newline = false)
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        Console.CursorTop = x + Console.WindowHeight - 1;
        pr(text, emphasis, newline);
        Console.SetCursorPosition(x, y);
    }

    ///<summary>Print in Formated Options</summary>
    private static void _prfo(Array options, string comment = "Select :")
    {
        string printResult = comment + " /";
        foreach (var item in options)
        {
            printResult += $" {item} /";
        }
        pr(printResult);
    }

    public static void del()
    {
        if (Console.CursorTop == 0) return;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr(delString);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
    public static void del(int lines)
    {
        for (int i = 0; i < lines; i++)
        {
            del();
        }
    }
}