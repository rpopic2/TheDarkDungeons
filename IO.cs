public static class IO
{
    public static void pr(object x)
    {
        Console.WriteLine(x);
    }
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
        pr(printResult);
        char key = Console.ReadKey().KeyChar;
        int indexOf = (Array.IndexOf<char>(keys, Char.ToLower(key)));
        if (indexOf != -1)
        {
            return indexOf;
        }
        return sel(options);
    }
}