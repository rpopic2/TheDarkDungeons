global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(q) 검사", "(w) 암살자", "(e) 마법사" };
    public static readonly string[] stats = new string[] { "(q) 힘/체력", "(w) 정밀/민첩", "(e) 마력/지능" };
    private static Player s_player { get => Player.instance; }
    public static void Main()
    {
        instance = new Program();
        Game.NewTurn();
        do
        {
            instance.MainLoop();
            if (s_player.Stance.Stance != StanceName.None) Game.ElaspeTurn();
        } while (s_player.IsAlive);
        IO.pr(s_player);
        IO.pr($"{s_player.Name}은 여기에 잠들었다...");
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
        IO.del();
        IO.pr($"{name}의 직업은?...");
        int index = 0;
        IO.sel(classes, 0, out index, out bool cancel, out _, out _);
        IO.pr(classes[index]);
        Player player = Player._instance = new Player(name);
        Map.NewMap();

        switch (index)
        {
            case 0:
                player.Inven.Add(Fightable.sword);
                break;
            case 1:
                player.Inven.Add(Fightable.dagger);
                break;
            case 2:
                player.Inven.Add(Fightable.staff);
                break;
            default:
                break;
        }
        player.Inven.Add(Fightable.torch);
        IO.pr("남은 능력치 포인트 : 1");
        player.SelectPickupStat();
        IO.del();
        player.PickupToken(3);
        IO.del();
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
                if (info.Modifiers == ConsoleModifiers.Control)
                {
                    IO.DrawScreen();
                    return;
                }
                s_player.SelectBasicBehaviour(0, 1, (int)Facing.Right);
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                s_player.SelectBasicBehaviour(0, 1, (int)Facing.Left);
                break;
            default:
                DefaultSwitch(info);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKeyInfo key)
    {
        bool found = IO.chk(key.KeyChar, s_player.Inven.Count, out int i);
        if (found && s_player.Inven[i] is Item item)
        {
            s_player.SelectBehaviour(item);
        }
        switch (key.KeyChar)
        {
            case 'y':
                s_player.SelectBehaviour(Fightable.bareHand);
                break;
            case ' ':
                s_player.InteractUnderFoot();
                break;
            case '.':
                s_player.SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                break;
            case '/':
                s_player.ShowStats();
                break;
        }
    }
}