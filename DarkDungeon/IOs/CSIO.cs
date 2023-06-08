public class CSIO : IIO
{
    private static Player s_player => Player.instance;
    public static bool IsInteractive = true;
    private const string EMPHASIS = "=> ";
    static readonly string DELSTRING = " ";

    static CSIO() {
        int delstringLength = Console.WindowWidth - 1;
        DELSTRING = new String(' ', (int)MathF.Max(0, delstringLength));
    }

    public void pr(string value, bool newline) {
        _pr(value, newline);
    }

    private static void _pr(string value, bool newline) {
        if (newline) Console.WriteLine(value);
        else Console.Write(value);
    }

    public static void pr(object value, __ flag = 0, string title = "", bool newline = true) {
        if (!IsInteractive)
            return;
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        string stringValue = title;
        stringValue += value.ToString() ?? string.Empty;

        if (flag.HasFlag(__.emphasis))
            stringValue = EMPHASIS + stringValue;
        if (flag.HasFlag(__.bottom)) {
            Console.CursorLeft = 0;
            Console.CursorTop += Console.WindowHeight - 1;
        }

        if (stringValue.Contains("^")) {
            pr_Color(stringValue, flag);
            if (!flag.HasFlag(__.bottom)) {
                _pr(string.Empty, newline);
            }
            else {
                Console.SetCursorPosition(x, y);
                return;
            }
        } else if (flag.HasFlag(__.bottom)) {
            _pr(stringValue, newline);
            Console.SetCursorPosition(x, y);
        } else {
            _pr(stringValue, newline);
        }
    }

    private static void pr_Color(string value, __ flags) {
        string[] splits = value.Split('^', StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in splits) {
            if (item.StartsWith("b")) Console.ForegroundColor = ConsoleColor.Blue;
            else if (item.StartsWith("g")) Console.ForegroundColor = ConsoleColor.Green;
            else if (item.StartsWith("r")) Console.ForegroundColor = ConsoleColor.Red;
            else if (item.StartsWith("/")) Console.ResetColor();
            else if (!item.StartsWith('c')) {
                _pr(item, false);
                continue;
            }
            _pr(item.Substring(1), false);
        }
        Console.ResetColor();
    }

    public void pr_map() {
        pr(Map.Current.ToString(), true);
    }

    public ConsoleKeyInfo rk() {
        if (IsInteractive) return Console.ReadKey(true);
        else return new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);
    }

    public void pr_depth_lv() {
        pr($"깊이 : {Map.Depth}\t레벨 : {s_player.Level} ({s_player.exp})", __.bottom | __.newline);
    }

    public void pr_hp_energy() {
        string myHpEnergy = $"Hp : {s_player.GetHp()}    기력 : {s_player.Energy}";
        pr($"{myHpEnergy}", __.bottom | __.newline);
    }

    public void pr_monster_hp_energy(Creature? frontCreature) {
        string enemyHpEnergy = frontCreature is null ? string.Empty : $"Hp : {frontCreature.GetHp()}    기력 : {frontCreature.Energy} (상대)";
        pr($"{enemyHpEnergy}", __.bottom | __.newline);
    }

    public void pr_inventory() {
        pr(s_player.Inven, __.bottom);
    }

    public string rl() {
        if (IsInteractive) return Console.ReadLine() ?? "null";
        else return "NonInteractive";
    }

    public void clr() {
        Console.Clear();
    }

    public void del() {
        if (!IsInteractive)
            return;
        //if (flags.HasFlag(__.bottom)) {
            //s_del_bottom();
            //return;
        //}
        if (Console.CursorTop == 0)
            return;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        pr(DELSTRING);
        Console.SetCursorPosition(0, Console.CursorTop - 1);

    }

    public void s_del_bottom() {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        Console.CursorTop = x + Console.WindowHeight - 1;
        pr(DELSTRING, false);
        Console.SetCursorPosition(x, y);
    }

}

