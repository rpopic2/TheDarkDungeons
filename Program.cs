﻿public class Program
{
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
        if (!skip) Intro();
        player = new Player("Michael", ClassName.Assassin, 3, 5, 1, 2, 2, 2);
        IO.pr("\nYour adventure begins...\n");
        InitActions();
        monster = new Monster("Bat", ClassName.Warrior, 3, 3, 1, 2, 1, 2); //Test!
        player.curTarget = monster;
        do
        {
            IO.Prompt(basic, out bool cancel);
            if(!cancel) EventListener.OnTurnEnd();
        } while (player.Hp.Cur > 0);
    }
    private void InitActions()
    {
        basic.Add("(R)est", () => Rest());
        basic.Add("Use Card(W)", () => UseCard());
        basic.Add("(S)tats", () => ShowStats());

        stanceShift.Add("(S)tanceshift", () => PromptCards((card) => StanceShift(card)));
    }

    private void Intro()
    {
        IO.pr("The Dark Dungeon ver 0.1\nPress any key to start...");
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
    private void PromptCards(Action<Card> action)
    {
        IO.SelectPlayerCard(out Card? card);
        if (card is null) return;
        action(card);
    }
    private void StanceShift(Card card)
    {
        card.StanceShift();
    }
    //-------------------------
    private void Rest()
    {
        IO.pr("Resting a turn.");
        Player.hand.Pickup(player.Draw());
        bool cancel = false;
        do
        {
            IO.Prompt(stanceShift, out cancel);
        } while (!cancel);
    }
    private void UseCard()
    {
        IO.SelectPlayerCard(out Card? card);
        if (card is null)
        {
            IO.del();
            return;
        }
        player.UseCard(card);

        monster.Attack(monster.Draw(), player);
    }
    
    private void ShowStats()
    {
        IO.pr(player.Stats);
        IO.rkc();
        IO.del(4);
    }
}