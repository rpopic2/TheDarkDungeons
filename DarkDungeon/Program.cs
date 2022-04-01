global using Entities;
global using Items;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static readonly string[] stats = new string[] { "(1) Sol", "(2) Lun", "(3) Con" };
    public static readonly string[] actions = new string[] { "Cards(W)", "(E)quipments", "(R)est", "(S)tats", "E(x)ile" };
    private static Player player { get => Player.instance; }
    public static void Main()
    {
        instance = new Program();
        Game.NewTurn();
        do
        {
            instance.MainLoop();
            if (player.CurStance.stance != Stance.None) Game.ElaspeTurn();
        } while (player.IsAlive);
        IO.pr(player);
        IO.pr(player.Hand);
        IO.pr(player.Inven);
        IO.pr($"{player.Name}은 여기에 잠들었다...");
        IO.rk();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dungeons of the Mine " + Rules.version);
        Intro();
        Console.Clear();
        IO.pr($"{player.Name}은 광산으로 들어갔다...");
    }

    private void Intro()
    {
        IO.rk("Press any key to start...");

        IO.pr("캐릭터의 이름은?...");
        string name = Console.ReadLine() ?? "Michael";
        IO.pr($"{name}의 직업은?...");
        IO.seln(classes, out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel) index = 0;
        ClassName className = (ClassName)index;
        Player._instance = new Player(name, className);
    }
    //-------------------------

    private void MainLoop()
    {
        ConsoleKeyInfo info = IO.rk(Map.Current);
        ConsoleKey key = info.Key;
        switch (key)
        {
            case ConsoleKey.RightArrow:
            case ConsoleKey.L:
                player.Move(1);
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                player.Move(-1);
                break;
            case ConsoleKey.Escape:
                ConsoleKey key2 = IO.rk(actions).Key;
                DefaultSwitch(key2);
                break;
            default:
                if (IO.chkn(info.KeyChar, player.Inven.Cap, out int index))
                {
                    player.UseInven(index);
                    break;
                }
                else DefaultSwitch(key);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Q:
                TokenType? selResult = player.SelectToken();
                if (selResult is TokenType token) player.UseToken(token);
                break;
            case ConsoleKey.E:
                player.UseInven();
                break;
            case ConsoleKey.U:
                IO.pr(player.tokens);
                break;
            case ConsoleKey.OemPeriod:
                player.Rest();
                break;
            case ConsoleKey.Oem2:
                player.ShowStats();
                break;
            case ConsoleKey.X:
                player.Exile();
                break;
        }
    }
}