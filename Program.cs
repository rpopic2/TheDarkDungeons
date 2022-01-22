public class Program
{
    public static Player player = Player.instance;
    public static int turn { get; private set; }
    public static Program instance = default!;
    private readonly CmdTuple basic = new CmdTuple();
    private readonly CmdTuple stanceShift = new CmdTuple();
    private readonly CmdTuple exile = new CmdTuple();
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static void Main()
    {
        instance = new Program();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dark Dungeon " + Rules.version);
        if (!Rules.SkipIntro) Intro();
        IO.pr("Your adventure begins...");
        InitActions();
        Map.NewMap();
        NewTurn();
        MainLoop();
    }
    private void InitActions()
    {
        basic.Add("Use Card(W)", () => player.UseCard());
        basic.Add("(R)est", () => player.Rest());
        basic.Add("E(x)ile", () => player.Exile());
        basic.Add("(S)tats", () => player.ShowStats());

        stanceShift.Add("(S)tanceshift", () =>
        {
            IO.seln(out int index, out bool cancel);
            if (!cancel) player.Hand.StanceShift(index);
            else IO.del();
        });

        exile.Add("Card(W)", () => player.Exile());
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
        player = new Player(name, className, 3, 5, 0, 2, 2, 2);
    }
    //-------------------------
    public void ElaspeTurn()
    {
        Map current = Map.Current;
        Monster monster = current.monster;
        monster.DoTurn();

        bool playerFirst = player.Lun >= monster?.Lun;
        Moveable? p1 = playerFirst ? player : monster;
        Moveable? p2 = playerFirst ? monster : player;

        if (p1?.TurnStance.stance == FightStance.Move && p2?.TurnStance.stance == FightStance.Move) IO.del(2);

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
    private void MainLoop()
    {
        do
        {
            IO.pr(Map.Current);
            ConsoleKey key = IO.rk().Key;
            IO.del();
            switch (key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.H:
                    player.Move(1);
                    break;
                case ConsoleKey.DownArrow:
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

        } while (player.IsAlive);
    }
    private void Menu()
    {
        do
        {
            IO.prfo(basic.Names.ToArray());
            ConsoleKey key = IO.rk().Key;
            IO.del();
            switch (key)
            {
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    return;
                default:
                    DefaultSwitch(key);
                    break;
            }

        } while (player.IsAlive);
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
}