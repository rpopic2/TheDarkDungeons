public class CSIO : IIO
{
    private static Player s_player => Player.instance;
    public static bool IsInteractive = true;
    private const string EMPHASIS = "=> ";

    public void pr(string value, bool newline) {
        _pr(value, newline);
    }
    private static void _pr(string value, bool newline)
    {
        if (newline) Console.WriteLine(value);
        else Console.Write(value);
    }
    public static void pr(object value, __ flag = 0, string title = "")
    {
        if (!IsInteractive) return;
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        string stringValue = title;
        stringValue += value.ToString() ?? string.Empty;
        if (flag.HasFlag(__.emphasis)) stringValue = EMPHASIS + stringValue;
        if (flag.HasFlag(__.newline)) stringValue += "\n";
        if (flag.HasFlag(__.bottom)) Console.CursorTop = x + Console.WindowHeight - 1;
        if (stringValue.Contains("^"))
        {
            pr_Color(stringValue, flag);
            if (!flag.HasFlag(__.bottom)) pr(string.Empty);
            else Console.SetCursorPosition(x, y);
            return;
        }
        else if (flag.HasFlag(__.bottom))
        {
            _pr(stringValue, false);
            Console.SetCursorPosition(x, y);
        }
        else pr(stringValue);
    }
    private static void pr_Color(string value, __ flags)
    {
        string[] splits = value.Split('^', StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in splits)
        {
            if (item.StartsWith("b")) Console.ForegroundColor = ConsoleColor.Blue;
            else if (item.StartsWith("g")) Console.ForegroundColor = ConsoleColor.Green;
            else if (item.StartsWith("r")) Console.ForegroundColor = ConsoleColor.Red;
            else if (item.StartsWith("/")) Console.ResetColor();
            else if (!item.StartsWith('c'))
            {
                _pr(item, false);
                continue;
            }
            _pr(item.Substring(1), false);
        }
        Console.ResetColor();
    }

    public void pr_map()
    {
        pr(Map.Current.ToString(), false);
    }
    public ConsoleKeyInfo rk()
    {
        if (IsInteractive) return Console.ReadKey(true);
        else return new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);
    }
    public void pr_depth_lv()
    {
        pr($"깊이 : {Map.Depth}\t레벨 : {s_player.Level} ({s_player.exp})", __.bottom | __.newline);
    }
    public void pr_hp_energy()
    {
        string myHpEnergy = $"Hp : {s_player.GetHp()}    기력 : {s_player.Energy}";
        pr($"{myHpEnergy}", __.bottom | __.newline);
    }

    public void pr_monster_hp_energy(Creature? frontCreature) {
        string enemyHpEnergy = frontCreature is null ? string.Empty : $"Hp : {frontCreature.GetHp()}    기력 : {frontCreature.Energy} (상대)";
        pr($"{enemyHpEnergy}", __.bottom | __.newline);
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
