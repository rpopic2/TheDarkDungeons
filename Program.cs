public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static readonly string[] stats = new string[] { "(1) Sol", "(2) Lun", "(3) Con" };
    public static readonly string[] actions = new string[] { "Cards(W)", "(E)quipments", "(R)est", "(S)tats", "E(x)ile" };
    private static Player player { get => Player.instance; }
    public static int turn { get; private set; }
    public static event EventHandler? OnTurnEnd;
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
        Intro();
        Console.Clear();
        IO.pr("Your adventure begins...");
        Map.NewMap();
        player.StartItem();
        NewTurn();
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
        Player._instance = new Player(name, className, 3, 5, 1, 2, 2, 2);
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
    public void ElaspeTurn()
    {
        if (Map.Current.monster is Monster monster)
        {
            monster.DoTurn();
            //bool playerFirst = Fightable.IsFirst(player, monster);
            Fightable? p1 = player;
            Fightable? p2 = monster;

            p1?.TryAttack();
            p2?.TryAttack();
        }
        OnTurnEnd?.Invoke(this, EventArgs.Empty);
        if (IO.printCount == 3) IO.del(2);
        if (turn % 5 == 0) Map.Current.Spawn();

        NewTurn();
    }
    public void NewTurn()
    {
        IO.printCount = 0;
        turn++;
        IO.pr($"\nTurn : {turn}\tDungeon Level : {Map.level}");
    }
}