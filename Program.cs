public class Program
{
    public const string version = "0.1";
    public const float vulMultiplier = 1.3f;
    public static Player player = Player.instance;
    public static Program? main;
    private bool skip = true;
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
    private CmdTuple basic = new CmdTuple();
    private CmdTuple stanceShift = new CmdTuple();
    private Monster monster;
    public static void Main()
    {
        main = new Program();
    }
    private Program()
    {
        Console.Clear();
        IO.pr("The Dark Dungeon " + version);
        if (!skip) Intro();
        IO.pr("Your adventure begins...\n");
        InitActions();
        monster = new Monster("Bat", ClassName.Warrior, 1, 3, 1, 2, 5, 2); //Test!
        player.target = monster;
        do
        {
            IO.Prompt(basic, out bool cancel);
            if (!cancel) IO.pr("");
        } while (player.Hp.point.Cur > 0);
    }
    private void InitActions()
    {
        basic.Add("(R)est", () => Rest());
        basic.Add("Use Card(W)", () => UseCard());
        basic.Add("(S)tats", () => ShowStats());

        stanceShift.Add("(S)tanceshift", () =>
        {
            IO.SelectPlayerCard(out int x, out bool cancel);
            if (!cancel) player.Hand.StanceShift(x);
        });
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
    private void Rest()
    {
        IO.pr("Resting a turn.");
        player.Pickup(player.Draw());
        player.rest = true;
        bool cancel = false;
        do
        {
            IO.Prompt(stanceShift, out cancel);
        } while (!cancel);
        OnPlayerAction();
    }
    private void UseCard()
    {
        IO.SelectPlayerCard(out int x, out bool cancel);
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
        player.UseCard(x);
        OnPlayerAction();
    }
    private void OnPlayerAction()
    {
        monster.DoTurn();
        if (player.lun >= monster.lun)
        {
            player.DoBattleAction();
            monster.DoBattleAction();
        }else
        {
            IO.pr("monster first");
            monster.DoBattleAction();
            player.DoBattleAction();
        }

    }

    private void ShowStats()
    {
        IO.pr(player.Stats);
        IO.rkc();
        IO.del(4);
    }
}