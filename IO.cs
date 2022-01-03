public static class IO
{
    public static readonly string[] handOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    public static readonly char[] NUMERICKEYS = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x)
    {
        Console.WriteLine(x);
    }
    ///<summary>Select from options.</summary>
    public static int sel(string[] options)
    {
        prfo(options);
        List<char> keys = options.ParseKeys();

        char key = rkc();
        int indexOf = keys.IndexOf(key);
        if (indexOf == -1) return sel(options);
        return indexOf;
    }

    ///<summary>Select from hand</summary>
    public static int selh(this Hand hand)
    {
        pr(hand.ToString());
        int cap = hand.Cap;
        string[] curOptions = new string[cap];
        for (int i = 0; i < cap; i++)
        {
            curOptions[i] = handOptions[i];
        }
        return sel(curOptions);
    }
    public static void prh(this Hand hand)
    {
        pr(hand.ToString());
        int cap = hand.Cap;
        string[] curOptions = new string[cap];
        for (int i = 0; i < cap; i++)
        {
            curOptions[i] = handOptions[i];
        }
        prfo(curOptions);
    }
    ///<summary>Print Formated Options</summary>
    public static void prfo(string[] options)
    {
        string printResult = "Select...\n/";
        foreach (string item in options)
        {
            printResult += $" {item} /";
        }
        pr(printResult);
    }
    ///<summary>Print Formated Options</summary>
    public static void prfo(List<string> options)
    {
        string printResult = "Select...\n/";
        foreach (string item in options)
        {
            printResult += $" {item} /";
        }
        pr(printResult);
    }
    ///<summary>Select and run</summary>
    public static void selr(CmdTuple cmd)
    {
        char key = rkc();
        if (cmd.HasKey(key)) cmd.Invoke(key);
        else selr(cmd);
    }
    public static void selcard(Action<Card> act)
    {
        char key = rkc();
        int index = Array.IndexOf(NUMERICKEYS, key);
        Card target = Program.player.Hand[index];
        if (index == -1 || target == null) selcard(act);
        else act(target);
    }

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);

}