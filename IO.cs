using System.Linq;

public static class IO
{
    private const char CANCELKEY = 'q';
    private const string Emphasis = "=> ";
    private const string delString = "                                                                 ";
    private static readonly Player player = Player.instance;

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object obj)
    {
        pr(obj);
        ConsoleKeyInfo info = rk();
        del();
        return info;
    }
    public static void seln(object obj, out int index, out bool cancel, out ConsoleModifiers mod, int max)
    {
        bool found;
        do
        {
            ConsoleKeyInfo keyInfo = rk(obj);
            mod = keyInfo.Modifiers;
            Char key = keyInfo.KeyChar;
            index = (int)Char.GetNumericValue(key);
            if (index == 0) index = 10;
            if (index != -1) index--;
            cancel = key == CANCELKEY;
            if (cancel) return;
            found = index != -1 && index <= max - 1;
        } while (!found);
    }
    public static void selh(out int result, out bool cancel, out ConsoleModifiers mod)
    {
        seln(player.Hand, out result, out cancel, out mod, player.Hand.Cap);
    }

    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x, bool emphasis = false, bool newline = false)
    {
        if (x is string[])
        {
            _prfo(x); return;
        }
        if (emphasis) x = Emphasis + x;
        if (newline) x = "\n" + x;
        Console.WriteLine(x);
    }

    ///<summary>Print in Formated Options</summary>
    private static void _prfo(object options, string comment = "Select :")
    {
        string printResult = comment + " /";
        foreach (var item in (Array)options)
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