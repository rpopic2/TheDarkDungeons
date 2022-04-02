global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) Warrior", "(2) Assassin", "(3) Mage" };
    public static readonly string[] stats = new string[] { "(1) Sol", "(2) Lun", "(3) Con" };
    public static readonly string[] actions = new string[] { "Cards(W)", "(E)quipments", "(R)est", "(S)tats", "E(x)ile" };
    private static Player player { get => Player.instance; }
    public static void Main()
    {
        instance = new Program();
        Game.NewTurn();
        do
        {
            instance.MainLoop();
            if (player.CurStance.stance != Stance.None) Game.ElaspeTurn();
        } while (player.IsAlive);
        IO.pr(player);
        IO.pr($"{player.Name}은 여기에 잠들었다...");
        IO.rk();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dungeons of the Mine " + Rules.version);
        Intro();
        Console.Clear();
        IO.rk($"{player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
    }

    private void Intro()
    {
        IO.rk("Press any key to start...");

        IO.pr("캐릭터의 이름은?...");
        string name = Console.ReadLine() ?? "Michael";
        IO.pr($"{name}의 직업은?...");
        IO.seln(classes, out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel) index = 0;
        ClassName className = (ClassName)index;
        Player._instance = new Player(name, className);
    }
    //-------------------------

    private void MainLoop()
    {
        ConsoleKeyInfo info = IO.rk();
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
            default:
                DefaultSwitch(info);
                break;
        }
    }
    public const string ItemKeys1 = "werty";
    private void DefaultSwitch(ConsoleKeyInfo key)
    {
        int i = ItemKeys1.IndexOf(key.KeyChar);
        if (i != -1 && player.Inven[i] is Item item)
        {
            IO.seln(item.skills, out int index, out bool cancel, out _);
            if (cancel) return;
            player.SelectSkillAndUse(item, index);
        }
        switch (key.KeyChar)
        {
            case 'q':
                IO.seln(Item.bardHand.skills, out int index, out bool cancel, out _);
                if (cancel) return;
                player.SelectSkillAndUse(Item.bardHand, index);
                break;
            case 'u':
                IO.rk(player.tokens);
                break;
            case '.':
                player.Rest();
                break;
            case '/':
                player.ShowStats();
                break;
            case 'x':
                player.Exile();
                break;
        }
    }
}