public static class IO
{
    public const ConsoleKey CANCELKEY = ConsoleKey.Backspace;
    private const string Emphasis = "=> ";
    public const string ItemKeys1 = "qwert";
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
    private static void _seli(object print, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo, int max)
    {
        bool found;
        do
        {
            keyInfo = rk(print);
            mod = keyInfo.Modifiers;
            cancel = keyInfo.Key == CANCELKEY;
            found = chki(keyInfo.KeyChar, max, out index);
            if (cancel) return;
        } while (!found);
    }
    public static void seli(Array print, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo)
    {
        bool found;
        do
        {
            keyInfo = rk(print);
            mod = keyInfo.Modifiers;
            cancel = keyInfo.Key == ConsoleKey.Escape;
            found = chki(keyInfo.KeyChar, print.Length, out index);
            if (cancel) return;
        } while (!found);
    }
    public static void seli_i(out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo)
    {
        bool found;
        do
        {
            keyInfo = rk(player.Inven);
            mod = keyInfo.Modifiers;
            cancel = keyInfo.Key == ConsoleKey.Escape;
            found = chki_i(keyInfo.KeyChar, out index);
            if (cancel) return;
        } while (!found);
    }
    public static bool chkn(Char i, int max, out int index)
    {
        index = (int)Char.GetNumericValue(i);
        if (index == 0) index = 10;
        if (index != -1) index--;
        return index != -1 && index <= max - 1;
    }
    public static bool chki(Char i, int max, out int index)
    {
        index = ItemKeys1.IndexOf(i);
        return index != -1 && index <= max -1;
    }
    public static bool chki_i(Char i, out int index)
    {
        index = ItemKeys1.IndexOf(i);
        return index != -1 && index <= player.Inven.Cap -1;
    }
    public static void seli_t(out int result, out bool cancel, out ConsoleModifiers mod) =>
    _seli(player.tokens, out result, out cancel, out mod, out _, player.tokens.Count);

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
    ///<summary>Print on the bottom</summary>
    public static void prb(object text, bool emphasis = false, bool newline = false)
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        Console.CursorTop = x + Console.WindowHeight - 1;
        pr(text, emphasis, newline);
        Console.SetCursorPosition(x, y);
    }

    ///<summary>Print in Formated Options</summary>
    private static void _prfo(Array options, string comment = "선택 :")
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