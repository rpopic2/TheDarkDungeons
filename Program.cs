﻿public class Program
{
    public static Player player = Player.instance;
    public static int turn { get; private set; }
    public static Program instance = default!;
    private readonly CmdTuple basic = new CmdTuple();
    private readonly CmdTuple stanceShift = new CmdTuple();
    private readonly CmdTuple exile = new CmdTuple();
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
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
        //BasicPrompt();
    }
    private void InitActions()
    {
        basic.Add("Use Card(W)", () => UseCard());
        basic.Add("(R)est", () => Rest());
        basic.Add("E(x)ile", () => IO.Prompt(exile, out bool cancel2));
        basic.Add("(S)tats", () => ShowStats());

        stanceShift.Add("(S)tanceshift", () =>
        {
            IO.SelectCardIndex(out int x, out bool cancel);
            if (!cancel) player.Hand.StanceShift(x);
            else IO.del();
        });

        exile.Add("Card(W)", () => ExileCard());
    }

    private void Intro()
    {
        IO.pr("Press any key to start...");
        Console.ReadKey();

        IO.pr("Choose charactor`s name...");
        string name = Console.ReadLine() ?? "";
        IO.pr("Choose your class...");
        IO.prfo(classes);
        IO.selsa(classes, out int selection, out bool cancel);
        if (cancel) selection = 0;
        ClassName className = (ClassName)selection;
        player = new Player(name, className, 3, 5, 0, 2, 2, 2);
    }
    //-------------------------
    public void ElaspeTurn()
    {
        Map current = Map.Current;
        Monster monster = current.monster;
        monster.DoTurn();
        if (current.NothingToPrint) IO.del(2);

        bool playerFirst = player.Lun >= monster?.Lun;
        Moveable? p1 = playerFirst ? player : monster;
        Moveable? p2 = playerFirst ? monster : player;

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
                UseCard();
                break;
            case ConsoleKey.R:
                Rest();
                break;
            case ConsoleKey.S:
                ShowStats();
                break;
            case ConsoleKey.X:
                ExileCard();
                break;
        }
    }
    private void Rest()
    {
        player.Rest();
        bool cancel = false;
        do
        {
            IO.Prompt(stanceShift, out cancel);
        } while (!cancel);
        ElaspeTurn();
    }
    private void UseCard()
    {
        IO.SelectCardIndex(out int x, out bool cancel);
        if (cancel)
        {
            IO.del();
            return;
        }
        Card? card = player.Hand[x];
        if (card is null)
        {
            IO.del();
            return;
        }
        player.UseCard(x, out bool elaspe);
        if (elaspe) ElaspeTurn();
    }
    private void ExileCard()
    {
        int index;
        Card card;
        do
        {
            IO.prh(player.Hand);
            IO.selc(out card, out index, out bool cancel);
            IO.del();
            if (cancel) return;
        } while (card.Stance == Stance.Star);
        player.Hand.Exile(index);
        ElaspeTurn();
    }
    private void ShowStats()
    {
        IO.pr(player);
        IO.rkc();
        IO.del(3);
    }
}