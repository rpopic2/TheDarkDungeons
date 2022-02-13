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
        IO.pr("Your adventure ends here...");
        IO.rk();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dark Dungeon " + Rules.version);
        Intro();
        Console.Clear();
        IO.pr("Your adventure begins...");
    }

    private void Intro()
    {
        IO.rk("Press any key to start...");

        IO.pr("Choose charactor`s name...");
        string name = Console.ReadLine() ?? "Michael";
        IO.pr("Choose your class...");
        IO.seln(classes, out int index, out bool cancel, out ConsoleModifiers mod, classes.Count());
        if (cancel) index = 0;
        ClassName className = (ClassName)index;
        Player._instance = new Player(name, className, cap: 3, maxHp: 3, sol: 2, lun: 2, con: 2);
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
            case ConsoleKey.Q:
            case ConsoleKey.Escape:
                ConsoleKey key2 = IO.rk(actions).Key;
                DefaultSwitch(key2);
                break;
            default:
                if (IO.chkn(info.KeyChar, player.Hand.Cap, out int index))
                {
                    player.UseCard(index);
                    break;
                }
                DefaultSwitch(key);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                Card? card = player.SelectCard();
                if (card is Card card2) player.UseCard(card2);
                break;
            case ConsoleKey.E:
                player.UseInven();
                break;
            case ConsoleKey.R:
                player.Rest();
                break;
            case ConsoleKey.S:
                player.ShowStats();
                break;
            case ConsoleKey.X:
                player.Exile();
                break;
        }
    }
}