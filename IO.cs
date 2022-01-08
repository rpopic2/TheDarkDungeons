public static class IO
{
    public static readonly char[] NUMERICKEYS = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
    ///<summary>Select from provided keys. Returns cancel.</summary>
    public static void sel(char[] keys, out int index, out char key, out bool cancel)
    {
        do
        {
            key = rkc();
            index = Array.IndexOf(keys, key);
            if(key == 'q') goto Cancel;
        } while (index == -1);
        del();
        cancel = false;
        return;
        Cancel :
            del();
            cancel = true;
    }
    ///<summary>Select a card. Returns cancel.</summary>
    public static bool selc(out Card? card, out int index)
    {
        do
        {
            sel(Player.hand.Cur, out index, out char key, out bool cancel);
            if(cancel) goto Cancel;
        } while (Player.hand[index] == null);
        card = Player.hand[index]!;
        return false;
        Cancel :
            card = null;
            return true;
    }

    ///<summary>Select from string array.</summary>
    public static bool selsa(string[] options, out int resultIndex)
    {
        char[] keys = options.ParseKeys();
        sel(keys, out int index, out char key, out bool cancel);
        if(!cancel) resultIndex = index;
        else resultIndex = -1;
        return cancel;
    }

    ///<summary>Select and run</summary>
    public static bool selcmd(CmdTuple cmd)
    {
        sel(cmd.Keys.ToArray(), out int index, out char key, out bool cancel);
        if(!cancel) cmd.Invoke(key);
        return cancel;
    }


    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x)
    {
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
    ///<summary>Print hand</summary>
    public static void prh(this Hand hand)
    {
        pr(hand.ToString());
        prfo(hand.Cur, "Select Index :");
    }

    public static void del()
    {
        if(Console.CursorTop == 0) return;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr("                                                  ");
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