public class Program
{
    public static Player player = Player.instance;
    public static int turn { get; private set; }
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static readonly string[] actions = new string[] { "Use card(W)", "(R)est", "(S)tats", "E(x)ile" };
    public static void Main()
    {
        instance = new Program();
        do
        {
            instance.MainLoop();
        } while (player.IsAlive);
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dark Dungeon " + Rules.version);
        if (!Rules.SkipIntro) Intro();
        IO.pr("Your adventure begins...");
        Map.NewMap();
        NewTurn();
    }

    private void Intro()
    {
        IO.pr("Press any key to start...");
        Console.ReadKey(true);

        IO.pr("Choose charactor`s name...");
        string name = Console.ReadLine() ?? "";
        IO.pr("Choose your class...");
        IO.prfo(classes);
        IO.seln(out int index, out bool cancel, classes.Count());
        if (cancel) index = 0;
        ClassName className = (ClassName)index;
        Player.instance = new Player(name, className, 3, 5, 0, 2, 2, 2);
        player = Player.instance;
    }
    //-------------------------
    private void MainLoop()
    {
        IO.pr(Map.Current);
        ConsoleKey key = IO.rk().Key;
        IO.del();
        switch (key)
        {
            case ConsoleKey.RightArrow:
            case ConsoleKey.H:
                player.Move(1);
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.L:
                player.Move(-1);
                break;
            case ConsoleKey.Q:
            case ConsoleKey.Escape:
                Menu();
                break;
            default:
                DefaultSwitch(key);
                break;
        }
        if (player.TurnStance.stance != Stance.None) ElaspeTurn();
    }
    private void Menu()
    {
        IO.prfo(actions);
        ConsoleKey key = IO.rk().Key;
        IO.del();
        DefaultSwitch(key);
    }
    private void DefaultSwitch(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                player.UseCard();
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
    public void ElaspeTurn()
    {
        Monster monster = Map.Current.monster;
        monster.DoTurn();
        if (!player.DidPrint && !monster.DidPrint)
        {
            IO.del(2);
        }
        bool playerFirst = player.Lun >= monster?.Lun;
        Moveable? p1 = playerFirst ? player : monster;
        Moveable? p2 = playerFirst ? monster : player;



        p1?.TryAttack();
        p2?.TryAttack();
        p1?.OnTurnEnd();
        p2?.OnTurnEnd();

        NewTurn();
    }
    public void NewTurn()
    {
        turn++;
        IO.pr($"\nTurn : {turn}\tDungeon Level : {Map.level}");
    }
}