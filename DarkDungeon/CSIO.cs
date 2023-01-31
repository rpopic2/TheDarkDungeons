public class CSIO : IIO
{
    public static bool IsInteractive = true;
    public void pr(string value, bool newline)
    {
        if (newline) Console.WriteLine(value);
        else Console.Write(value);
    }
    public ConsoleKeyInfo rk()
    {
        if (IsInteractive) return Console.ReadKey(true);
        else return new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);
    }

    public string rl()
    {
        if (IsInteractive) return Console.ReadLine() ?? "null";
        else return "NonInteractive";
    }
    public void clr()
    {
        Console.Clear();
    }
}
