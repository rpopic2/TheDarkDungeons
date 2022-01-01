public static class IO
{
    private static readonly string[] handOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    ///<summary>Print.
    ///Equals to Console.WriteLine(x);</summary>
    public static void pr(object x)
    {
        Console.WriteLine(x);
    }
    ///<summary>Select from options.</summary>
    public static int sel(string[] options)
    {
        string printResult = "/";
        char[] keys = new char[options.Length];

        for (int i = 0; i < options.Length; i++)
        {
            int beforeIndex = options[i].IndexOf('(');
            keys[i] = Char.ToLower(options[i][beforeIndex + 1]);
        }

        foreach (string item in options)
        {
            string tempItem = item;
            printResult += $" {tempItem} /";
        }
        pr("Select From :");
        pr(printResult);
        char key = Console.ReadKey(true).KeyChar;
        int indexOf = (Array.IndexOf<char>(keys, Char.ToLower(key)));
        if (indexOf != -1)
        {
            return indexOf;
        }
        return sel(options);
    }

    ///<summary>Select from hand caps</summary>
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
    ///<summary>Select a cards from hand</summary>
    public static int selc(this Hand hand)
    {
        pr(hand.ToString());
        int cardCount = hand.Count;
        string[] curOptions = new string[cardCount];
        for (int i = 0; i < cardCount; i++)
        {
            curOptions[i] = handOptions[i];
        }
        return sel(curOptions);
    }
}