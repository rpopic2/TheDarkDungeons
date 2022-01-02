public static class IO
{
    public static readonly string[] handOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    public static readonly char[] numericKeys = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

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
        if (!cmd.InvokeIfHasKey(key)) selr(cmd);
    }
    public static Card selcard(List<char> keys, Hand hand)
    {
        char key = rkc();
        if (keys.IndexOf(key) == -1) return selcard(keys, hand);
        else return hand.GetAt(keys.IndexOf(key));
    }

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);

}