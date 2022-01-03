public static class IO
{
    public static readonly char[] NUMERICKEYS = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
    ///<summary>Select from provided keys.</summary>
    public static void sel(char[] keys, out int resultIndex)
    {
        char key = rkc();
        int index = Array.IndexOf(keys, key);
        if (index == -1) sel(keys, out resultIndex);
        else resultIndex = index;
    }
    ///<summary>Select a card and run</summary>
    public static void selc(out Card card)
    {
        sel(Hand.PlayerCur, out int index);
        if (Hand.Player[index] == null) selc(out card);
        else card = Hand.Player[index];
    }

    ///<summary>Select from string array.</summary>
    public static void selsa(string[] options, out int resultIndex)
    {
        char[] keys = options.ParseKeys2();
        sel(keys, out resultIndex);
    }

    ///<summary>Select and run</summary>
    public static void cmdsel(CmdTuple cmd)
    {
        char key = rkc();
        int index = Array.IndexOf(cmd.Keys.ToArray(), key);
        if (index == -1) cmdsel(cmd);
        else cmd.Invoke(key);
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

}