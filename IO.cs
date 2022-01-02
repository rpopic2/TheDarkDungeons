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
        prfo(options);
        List<char> keys = ParseKeys(options);

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
    public static List<char> ParseKeys(this string[] options)
    {
        List<char> result = new List<char>();
        foreach (string item in options)
        {
            result.Add(item.ParseKey());
        }
        return result;
    }
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);

    ///<summary>ReadKey as lowercase char. Intercept is true.</summary>
    public static char rkc()
       => Char.ToLower(Console.ReadKey(true).KeyChar);
    // ///<summary>Select a cards from hand</summary>
    // public static int selc(this Hand hand)
    // {
    //     pr(hand.ToString());
    //     int cardCount = hand.Count;
    //     string[] curOptions = new string[cardCount];
    //     for (int i = 0; i < cardCount; i++)
    //     {
    //         curOptions[i] = handOptions[i];
    //     }
    //     return sel(curOptions);
    // }
}