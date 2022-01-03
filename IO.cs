public static class IO
{
    public static readonly char[] NUMERICKEYS = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x)
    {
        Console.WriteLine(x);
    }
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
        ///<summary>Select from provided keys.</summary>
    public static void sel(char[] keys, out int resultIndex)
    {
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1); char key = rkc();
        int index = Array.IndexOf(keys, key);
        if (index == -1) sel(keys, out resultIndex);
        else resultIndex = index;
    }
    ///<summary>Select from string array.</summary>
    public static void selsa(string[] options, out int resultIndex)
    {
        prfo(options);
        char[] keys = options.ParseKeys2();
        sel(keys, out resultIndex);
    }
    ///<summary>Select a card index.</summary>
    public static void selci(out int resultIndex)
    {
        sel(NUMERICKEYS, out resultIndex);
    }
    ///<summary>Select and run</summary>
    public static void selr(CmdTuple cmd)
    {
        char key = rkc();
        if (cmd.HasKey(key)) cmd.Invoke(key);
        else selr(cmd);
    }
    ///<summary>Select a card and run</summary>
    public static void selcr(Action<Card> act)
    {
        char key = rkc();
        int index = Array.IndexOf(NUMERICKEYS, key);
        Card target = Program.player.Hand[index];
        if (index == -1 || target == null) selcr(act);
        else act(target);
    }

    ///<summary>Print hand</summary>
    public static void prh(this Hand hand)
    {
        pr(hand.ToString());
        int cap = hand.Cap;
        string[] curOptions = new string[cap];
        for (int i = 0; i < cap; i++)
        {
            curOptions[i] = NUMERICKEYS[i].ToString();
        }
        prfo(curOptions, "Select Index :");
    }
    ///<summary>Print Formated Options</summary>
    public static void prfo(string[] options, string comment = "Select :")
    {
        string printResult = comment + " /";
        foreach (string item in options)
        {
            printResult += $" {item} /";
        }
        pr(printResult);
    }
    ///<summary>Print Formated Options</summary>
    public static void prfo(List<string> options, string comment = "Select :")
    {
        prfo(options.ToArray(), comment);
    }
    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    

}