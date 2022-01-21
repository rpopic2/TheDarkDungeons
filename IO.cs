using System.Linq;

public static class IO
{
    public static readonly char[] NUMERICKEYS = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    private const char CANCELKEY = 'q';
    private const string Emphasis = "=> ";
    private const string delString = "                                                                 ";
    private static readonly Player player = Player.instance;

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk()
    {
        ConsoleKeyInfo info = Console.ReadKey(true);
        return info;
    }
    public static void newSelh(int max, out int index, out ConsoleModifiers mod, out bool cancel)
    {
        bool found;
        do
        {
            ConsoleKeyInfo keyInfo = rk();
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
    public static void newSelh(int max, out int result, out bool cancel)
    {
        newSelh(max, out result, out ConsoleModifiers mod, out cancel);
    }
    public static void newSelh(out int result, out bool cancel)
    {
        newSelh(Player.instance.Cap, out result, out ConsoleModifiers mod, out cancel);
    }
    public static void newSelh(out int result, out ConsoleModifiers mod, out bool cancel)
    {
        newSelh(Player.instance.Cap, out result, out mod, out cancel);
    }


    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x, bool emphasis = false, bool newline = false)
    {
        if (emphasis) x = Emphasis + x;
        if (newline) x = "\n" + x;
        Console.WriteLine(x);
    }
    ///<summary>Print in Formated Options</summary>
    public static void prfo(string[] options, string comment = "Select :")
        => _prfo(options, comment);
    ///<summary>Print in Formated Options</summary>
    public static void prfo(char[] options, string comment = "Select :")
        => _prfo(options, comment);
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