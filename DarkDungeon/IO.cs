public static class IO
{
    private const char CANCELKEY = 'q';
    private const string Emphasis = "=> ";
    private const string delString = "                                                                                       ";
    private static Player player {get => Player.instance;}
    public static int printCount;

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object print)
    {
        pr(print);
        ConsoleKeyInfo info = rk();
        del();
        return info;
    }
    public static void seln(Array value, out int index, out bool cancel, out ConsoleModifiers mod)
    {
        _seln(value, out index, out cancel, out mod, value.Length);
    }
    private static void _seln(object print, out int index, out bool cancel, out ConsoleModifiers mod, int max)
    {
        bool found;
        do
        {
            ConsoleKeyInfo keyInfo = rk(print);
            mod = keyInfo.Modifiers;
            Char key = keyInfo.KeyChar;
            cancel = key == CANCELKEY;
            found = chkn(key, max, out index);
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
    ///<summary>Select from hand</summary>
    public static void seln_h(out int result, out bool cancel, out ConsoleModifiers mod) =>
        _seln(player.Hand, out result, out cancel, out mod, player.Hand.Cap);
    ///<summary>Select from inventory</summary>
    public static void seln_i(out int result, out bool cancel, out ConsoleModifiers mod) =>
    _seln(player.Inven, out result, out cancel, out mod, player.Inven.Cap);

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