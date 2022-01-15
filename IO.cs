public static class IO
{
    public static readonly char[] NUMERICKEYS = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    private const char CANCELKEY = 'q';
    private const string delString = "                                                                 ";
    private static readonly Player player = Player.instance;
    private static readonly Hand playerHand = player.Hand;

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static KeyArrow rarw()
    {
        string result = Console.ReadKey(true).Key.ToString();
        if(result.ToLower()[0] == CANCELKEY) return KeyArrow.Cancel;
        return (KeyArrow)Enum.Parse(typeof(KeyArrow), result);
    }
    ///<summary>Select from provided keys. Returns cancel.</summary>
    public static void sel(char[] keys, out int index, out char key, out bool cancel)
    {
        do
        {
            key = rkc();
            index = Array.IndexOf(keys, key);
            if (key == CANCELKEY) goto Cancel;
        } while (index == -1);
        del();
        cancel = false;
        return;
    Cancel:
        del();
        cancel = true;
    }
    ///<summary>Select a card.</summary>
    public static void selc(out Card card, out int index, out bool cancel)
    {
        do
        {
            sel(playerHand.Cur, out index, out char key, out cancel);
            if (cancel) goto Cancel;
        } while (playerHand[index] == null);
        card = playerHand[index] ?? throw new Exception();
        return;
    Cancel:
        card = new Card();
    }
    ///<summary>Select a card index.</summary>
    public static void selci(out int index, out bool cancel)
    {
        do
        {
            sel(playerHand.Cur, out index, out char key, out cancel);
            if (cancel) goto Cancel;
        } while (playerHand[index] == null);
    Cancel:
        return;
    }

    ///<summary>Select from string array.</summary>
    public static void selsa(string[] options, out int resultIndex, out bool cancel)
    {
        char[] keys = options.ParseKeys();
        sel(keys, out int index, out char key, out cancel);
        if (!cancel) resultIndex = index;
        else resultIndex = -1;
    }

    ///<summary>Select and run</summary>
    public static void selcmd(CmdTuple cmd, out bool cancel)
    {
        sel(cmd.Keys.ToArray(), out int index, out char key, out cancel);
        if (!cancel) cmd.Invoke(key);
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
    public static void prh(Hand hand)
    {
        pr(hand.ToString());
        prfo(hand.Cur, "Select Index :");
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
    public static void SelectCardIndex(out int index, out bool cancel)
    {
        prh(playerHand);
        selci(out index, out cancel);
        del();
    }
    public static void SelectCardUse()
    {
        prh(playerHand);
    }
    public static void Prompt(CmdTuple cmd, out bool cancel)
    {
        IO.prfo(cmd.Names.ToArray());
        IO.selcmd(cmd, out cancel);
    }
}