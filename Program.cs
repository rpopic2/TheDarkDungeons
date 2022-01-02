public class Program
{
    public static Program? main;
    bool skip = true;
    public Charactor player;

    public static void Main()
    {
        main = new Program();
    }
    private Program()
    {
        if (!skip) Intro();
        player = new Charactor("Michael", ClassName.Assassin);
        player.exp.Gain(1);
        IO.pr(player.Stats);
        IO.pr("\nYour adventure begins...\n");
        InitCommands();
        PromptAction();
    }
    private void Intro()
    {
        if (skip) PromptAction();
        IO.pr("The Dark Dungeon ver 0.1\nPress any key to start...");
        Console.ReadKey();

        Console.Clear();


        IO.pr("Choose charactor`s name...");
        string name = Console.ReadLine() ?? "";

        IO.pr("Choose your class...");
        ClassName className = (ClassName)IO.sel(new string[] { "(W)arrior", "(A)ssassin", "(M)age" });

        player = new Charactor(name, className);



    }
    string[] actions;
    Dictionary<char, Action> commands = new Dictionary<char, Action>();
    void InitCommands()
    {
        actions = new string[2] { "(R)est", "(T)est" };
        List<char> keys = IO.ParseKeys(actions);
        commands.Add('r', () => Rest());
        commands.Add('t', () => Test());
    }
    void PromptAction()
    {
        //sela(new string[] { "(R)est" }, new Action[] { () => Rest() });
        IO.prfo(actions);
        ReadKey();
    }
    void ReadKey()
    {
        char key = IO.rkc();
        if(commands.ContainsKey(key)) commands[key]();
        else ReadKey();
    }

    void Rest()
    {
        IO.pr("Resting a turn.");
        player.Hand.Pickup(player.Draw());
        StanceShift();
    }
    void Test()
    {
        IO.pr("Test!");
        PromptAction();
    }

    void StanceShift()
    {
        int result = sela(new string[] { "(C)ontinue", "(S)tanceshift" }, new Action[] { () => {PromptAction(); return;}, ()=>{player.Hand.StanceShift();
    StanceShift();} });

    }

    ///<summary>select to actions
    ///selas(new string[] {}, new Action[] {});</summary>
    int sela(string[] options, Action[] actions)
    {
        int result = IO.sel(options);
        actions[result]();
        PromptAction();
        return result;
    }
}