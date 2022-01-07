public class Program
{
    public static Program? main;
    private bool skip = true;
    public static Player player = new Player("Michael", ClassName.Assassin, 3, 5, 0, 2, 2, 2);
    public static readonly string[] classes = new string[] { "(W)arrior", "(A)ssassin", "(M)age" };
    private CmdTuple basic = new CmdTuple();
    private CmdTuple stanceShift = new CmdTuple();
    private bool stanceShiftFlag = false;
    private Monster monster;

    public static void Main()
    {
        main = new Program();
    }
    private Program()
    {
        Console.Clear();
        if (!skip) Intro();
        player = new Player("Michael", ClassName.Assassin, 3, 5, 0, 2, 2, 2);
        Player.exp.Gain(1);
        IO.del();
        IO.pr(player.Stats);
        IO.pr("\nYour adventure begins...");
        InitActions();
        monster = new Monster("Bat", ClassName.Warrior, 3, 3, 1, 2, 1, 2); //Test!
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
        player = new Player(name, className, 3, 5, 0, 2, 2, 2);
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
        Player.hand.prh();
        IO.selc(out Card card, out int index);
        action(card);
    }    
    private void StanceShift(Card card)
    {
        card.StanceShift();
        IO.del();
    }
    //-------------------------
    private void Rest()
    {
        IO.pr("Resting a turn.");
        Player.hand.Pickup(player.Draw());
        stanceShiftFlag = true;
        do
        {
            Prompt(stanceShift);
        } while (stanceShiftFlag);
    }
    private void UseCard()
    {
        IO.prh(Player.hand);
        IO.selc(out Card card, out int index);
        IO.del();
        Player.hand.Delete(card);
        monster.TakeDamage(card.sol);
        player.TakeDamage(monster.Draw().sol);
    }
}