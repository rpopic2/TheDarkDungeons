public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static readonly string[] stats = new string[] { "(1) Sol", "(2) Lun", "(3) Con" };
    public static readonly string[] actions = new string[] { "Cards(W)", "(E)quipments", "(R)est", "(S)tats", "E(x)ile" };
    public static Player player = Player.instance;
    public static int turn { get; private set; }
    public static void Main()
    {
        instance = new Program();
        do
        {
            instance.MainLoop();
            if (player.CurStance.stance != Stance.None) instance.ElaspeTurn();
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
        if (!Rules.SkipIntro) Intro();
        Console.Clear();
        IO.pr("Your adventure begins...");
        Map.NewMap();
        NewTurn();
        player.StartItem();
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
        player = Player.instance = new Player(name, className, 3, 5, 1, 2, 2, 2);
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
                Card? card = player.PickCard();
                player.UseCard(card);
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