public class Program
{
    public static Program? main;
    private bool skip = true;
    public static Charactor player = new Charactor("Michael", ClassName.Assassin);
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
    private CmdTuple basic = new CmdTuple();
    private CmdTuple stanceShift = new CmdTuple();
    private bool stanceShiftFlag = false;
    public static void Main()
    {
        main = new Program();
    }
    private Program()
    {
        Console.Clear();
        if (!skip) Intro();
        player.exp.Gain(1);
        IO.del();
        IO.pr(player.Stats);
        IO.pr("\nYour adventure begins...");
        InitActions();

        do
        {
            BasicPrompt();
        } while (player.Hp.Cur > 0);
    }
    private void InitActions()
    {
        basic.Add("(R)est", () => Rest());
        basic.Add("Use Card(W)", () => UseCard());

        stanceShift.Add("(C)ontinue", () => stanceShiftFlag = false);
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
        IO.selsa(classes, out int selection);
        ClassName className = (ClassName)selection;
        player = new Charactor(name, className);
    }
    private static void Prompt(CmdTuple cmd)
    {
        IO.prfo(cmd.Names.ToArray());
        IO.selcmd(cmd);
    }
    private void BasicPrompt()
    {
        Prompt(basic);
    }
    private void PromptCards(Action<Card> action)
    {
        player.Hand.prh();
        IO.selc(out Card card, out int index);
        action(card);
        // IO.selcr(action);
    }
    private void Rest()
    {
        IO.pr("Resting a turn.");
        player.Hand.Pickup(player.Draw());
        IO.del();
        IO.del();
        stanceShiftFlag = true;
        do
        {
            Prompt(stanceShift);
        } while (stanceShiftFlag);
    }
    private void UseCard()
    {
        IO.prh(Hand.Player);
        IO.selc(out Card card, out int index);
        IO.del();
        Hand.Player.Delete(card);
    }
    private void StanceShift(Card card)
    {
        card.StanceShift();
        IO.del();
    }
}