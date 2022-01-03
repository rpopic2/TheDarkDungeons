public class Program
{
    public static Program? main;
    bool skip = true;
    public static Charactor player = new Charactor("Michael", ClassName.Assassin);
    private CmdTuple basic = new CmdTuple();
    private CmdTuple stanceShift = new CmdTuple();

    public static void Main()
    {
        main = new Program();
    }
    private Program()
    {
        if (!skip) Intro();
        player.exp.Gain(1);
        IO.pr(player.Stats);
        IO.pr("\nYour adventure begins...\n");
        InitActions();
        Prompt(basic);
    }
    private void InitActions()
    {
        basic.Add("(R)est", () => Rest());
        basic.Add("(T)est", () => Test());

        stanceShift.Add("(C)ontinue", () => Prompt(basic));
        stanceShift.Add("(S)tanceshift", () =>
        {
            PromptCards((card) => card.StanceShift());
            Prompt(stanceShift);
        });
    }
    private void Intro()
    {
        if (skip) Prompt(basic);
        IO.pr("The Dark Dungeon ver 0.1\nPress any key to start...");
        Console.ReadKey();

        Console.Clear();


        IO.pr("Choose charactor`s name...");
        string name = Console.ReadLine() ?? "";

        IO.pr("Choose your class...");
        ClassName className = (ClassName)IO.sel(new string[] { "(W)arrior", "(A)ssassin", "(M)age" });

        player = new Charactor(name, className);
    }
    public static void Prompt(CmdTuple action)
    {
        IO.prfo(action.Names);
        IO.selr(action);
    }
    public void PromptCards(Action<Card> action)
    {
        player.Hand.prh();
        IO.selcard(action);
        IO.pr(player.Hand);
    }
    void Rest()
    {
        IO.pr("Resting a turn.");
        player.Hand.Pickup(player.Draw());
        Prompt(stanceShift);
    }
    void Test()
    {
        IO.pr("Test!");
        Prompt(basic);
    }
}