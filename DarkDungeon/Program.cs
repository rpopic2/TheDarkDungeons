global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(1) 검사", "(2) 암살자", "(3) 마법사" };
    public static readonly string[] stats = new string[] { "(1) 힘/체력", "(2) 정밀/민첩", "(3) 마력/지능" };
    private static Player player { get => Player.instance; }
    public static void Main()
    {
        instance = new Program();
        Game.NewTurn();
        do
        {
            instance.MainLoop();
            if (player.Stance.Stance != StanceName.None) Game.ElaspeTurn();
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
    }

    private void Intro()
    {
        IO.rk("Press any key to start...");

        IO.pr("캐릭터의 이름은?...");
        string name = Console.ReadLine() ?? "Michael";
        IO.pr($"{name}의 직업은?...");
        IO.seln(classes, out int index, out bool cancel, out ConsoleModifiers mod);
        if (cancel) index = 0;
        Player._instance = new Player(name);
        switch (index)
        {
            case 0:
                Player._instance.PickupItem(Item.sword);
                break;
            case 1:
                break;
            case 2:
                Player._instance.PickupItem(Item.staff);
                break;
            default:
                break;
        }
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
    private void DefaultSwitch(ConsoleKeyInfo key)
    {
        bool found = IO.chki(key.KeyChar, out int i);
        if (found && player.Inven[i] is Item item)
        {
            IO.seln(item.skills, out int index, out bool cancel, out _);
            if (cancel) return;
            player.SelectSkill(item, index);
        }
        switch (key.KeyChar)
        {
            case 'q':
                IO.seln(Item.bareHand.skills, out int index, out bool cancel, out _);
                if (cancel) return;
                player.SelectSkill(Item.bareHand, index);
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
        }
    }
}