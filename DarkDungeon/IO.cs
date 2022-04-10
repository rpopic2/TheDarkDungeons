public static class IO
{
    public const ConsoleKey OKKEY = ConsoleKey.Z;
    public const ConsoleKey CANCELKEY = ConsoleKey.X;
    private const string EMPHASIS = "=> ";
    public const string ITEMKEYS1 = "qwert";
    private const string DELSTRING = "                                                                                       ";
    private static Player player { get => Player.instance; }

    ///<summary>Console.ReadKey. Intercept is true.</summary>
    public static ConsoleKeyInfo rk() => Console.ReadKey(true);

    public static ConsoleKeyInfo rk(object print, __ flags = 0)
    {
        pr(print, flags);
        ConsoleKeyInfo info = rk();
        del();
        return info;
    }
    public static void sel(object value, __ flags, out int index, out bool cancel, out ConsoleModifiers mod, out ConsoleKeyInfo keyInfo)
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
            keyInfo = rk(value);
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
    public static void pr(object value, __ flag = 0)
    {
        if (value is Array array)
        {
            value = array.ToFString();
        }
        if (flag.HasFlag(__.emphasis)) value = EMPHASIS + value;
        if (flag.HasFlag(__.newline)) value = "\n" + value;
        if (flag.HasFlag(__.bottom))
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = x + Console.WindowHeight - 1;
            Console.WriteLine(value);
            Console.SetCursorPosition(x, y);
            return;
        }
        Console.WriteLine(value);
    }
    ///<summary>Print in Formated Options</summary>


    public static void del()
    {
        if (Console.CursorTop == 0) return;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr(DELSTRING);
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