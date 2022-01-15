﻿public class Program
{
    public const string version = "0.1";
    public const float vulMultiplier = 1.3f;
    public static Player player = Player.instance;
    public static Program? instance;
    private bool skip = true;
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
    public Monster monster;
    //public Map map;
    public Map currentMap;

    public static void Main()
    {
        instance = new Program();
    }
    public Program()
    {
        Console.Clear();

        IO.pr("The Dark Dungeon " + version);
        if (!skip) Intro();
        IO.pr("Your adventure begins...\n");
        InitActions();
        monster = new Monster("Bat", ClassName.Warrior, 1, 3, 1, 2, 5, 2); //Test!
        currentMap = new Map(6, this);
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
    private void BasicPrompt()
    {
        do
        {
            IO.Prompt(basic, out bool cancel);
            if (!cancel) IO.pr("");
            else IO.Prompt(exile, out bool cancel2);
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
        OnPlayerAction();
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
        if (!star) OnPlayerAction();
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
    }
    private void Move()
    {
        IO.pr(currentMap);
        do
        {
            KeyArrow arw = IO.rarw();
            switch (arw)
            {
                case KeyArrow.UpArrow:
                case KeyArrow.RightArrow:
                    IO.del();
                    currentMap.MoveUp();
                    IO.pr(currentMap);
                    break;
                case KeyArrow.DownArrow:
                case KeyArrow.LeftArrow:
                    IO.del();
                    currentMap.MoveDown();
                    IO.pr(currentMap);
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
    //
    private void OnPlayerAction()
    {
        monster.DoTurn();
        if (player.Lun >= monster.Lun)
        {
            player.DoBattleAction();
            monster.DoBattleAction();
        }
        else
        {
            monster.DoBattleAction();
            player.DoBattleAction();
        }
    }
}