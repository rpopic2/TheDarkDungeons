public static class IO
{
    public static readonly char[] NUMERICKEYS = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
    ///<summary>Select from provided keys.</summary>
    public static void sel(char[] keys, out int index, out char key, bool doDel = true)
    {
        do
        {
            key = rkc();
            index = Array.IndexOf(keys, key);
        } while (index == -1);
        if(doDel) del();
    }
    ///<summary>Select a card</summary>
    public static void selc(out Card card, out int index)
    {
        do
        {
            sel(Hand.PlayerCur, out index, out char key, false);
        } while (Hand.Player[index] == null);
        card = Hand.Player[index]!;
        del();
    }

    ///<summary>Select from string array.</summary>
    public static void selsa(string[] options, out int resultIndex)
    {
        char[] keys = options.ParseKeys();
        sel(keys, out int index, out char key);
        resultIndex = index;
    }

    ///<summary>Select and run</summary>
    public static void selcmd(CmdTuple cmd)
    {
        sel(cmd.Keys.ToArray(), out int index, out char key);
        cmd.Invoke(key);
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
        prfo(hand.CurOption, "Select Index :");
    }

    public static void del()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr("                                                  ");
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }

}