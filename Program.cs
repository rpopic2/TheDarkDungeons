public class Program
{
    public static Player player = Player.instance;
    public static int turn { get; private set; }
    public static Program instance = default!;
    private bool skip = true;
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
    public Monster? monster;
    public static void Main()
    {
        instance = new Program();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dark Dungeon " + Rules.version);
        if (!skip) Intro();
        IO.pr("Your adventure begins...");
        InitActions();
        NewTurn();
        Map.NewMap();
        BasicPrompt();
    }
    private CmdTuple basic = new CmdTuple();
    private CmdTuple stanceShift = new CmdTuple();
    private CmdTuple exile = new CmdTuple();
    private void InitActions()
    {
        basic.Add("Use Card(W)", () => UseCard());
        basic.Add("(R)est", () => Rest());
        basic.Add("(S)tats", () => ShowStats());
        basic.Add("Mo(v)e", () => Move());

        stanceShift.Add("(S)tanceshift", () =>
        {
            IO.SelectCardIndex(out int x, out bool cancel);
            if (!cancel) player.Hand.StanceShift(x);
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
        monster?.DoTurn();

        Moveable? p1 = player;
        Moveable? p2 = monster;
        if (player.Lun < monster?.Lun)
        {
            p2 = player;
            p1 = monster;
        }
        p1?.TryBattle();
        p2?.TryBattle();
        p1?.UpdateTarget();
        p2?.UpdateTarget();
        NewTurn();
    }
    public void NewTurn()
    {
        turn++;
        IO.pr("\nTurn : " + turn);
    }
    private void BasicPrompt()
    {
        do
        {
            IO.Prompt(basic, out bool cancel);
            if (cancel) IO.Prompt(exile, out bool cancel2);
        } while (player.Hp.point.Cur > 0);
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
        player.UseCard(x, out bool star);
        if (!star) ElaspeTurn();
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
    private void Move()
    {
        IO.pr(Map.Current);
        do
        {
            KeyArrow arw = IO.rarw();
            switch (arw)
            {
                case KeyArrow.UpArrow:
                case KeyArrow.RightArrow:
                    //Map.Current.Move(1);
                    IO.del();
                    player.Move(1);
                    IO.pr(Map.Current);
                    break;
                case KeyArrow.DownArrow:
                case KeyArrow.LeftArrow:
                    //IO.del();
                    IO.del();
                    player.Move(-1);
                    IO.pr(Map.Current);
                    break;
                case KeyArrow.Cancel:
                    IO.del();
                    return;
            }
        } while (true);

    }
    private void ShowStats()
    {
        IO.pr(player.Stats);
        IO.rkc();
        IO.del(4);
    }
}