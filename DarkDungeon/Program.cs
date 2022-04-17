global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "^r(q) 검사^/", "^g(w) 암살자^/", "^b(e) 마법사^/" };
    public static readonly string[] stats = new string[] { "^r(q) 힘/체력^/", "^g(w) 정밀/민첩^/", "^b(e) 마력/지능^/" };
    public readonly Position MOVELEFT = new(1, Facing.Left);
    public readonly Position MOVERIGHT = new(1, Facing.Right);
    private static Player s_player { get => Player.instance; }
    private static int s_spawnrate = 10;
    public static int Turn { get; private set; }
    public static void Main()
    {
        instance = new Program();
        IO.rk($"{s_player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
        NewTurn();
        do
        {
            ElaspeTurn();
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
        IO.rk("Press any key to start...", __.color);

        IO.pr("캐릭터의 이름은?...");
        string name = Console.ReadLine() ?? "Michael";
        IO.del(2);
        IO.pr($"{name}의 직업은?...");
        int index = 0;
        IO.sel(classes, __.color, out index, out bool cancel, out _, out _);
        IO.pr(classes[index]);
        Player player = Player._instance = new Player(name);
        Map.NewMap();

        switch (index)
        {
            case 0:
                player.Inven.Add(Fightable.sword);
                player.Inven.Add(Fightable.shield);
                break;
            case 1:
                player.Inven.Add(Fightable.dagger);
                player.Inven.Add(Fightable.bow);
                player.Inven.Add(Fightable.arrow);
                break;
            case 2:
                player.Inven.Add(Fightable.staff);
                player.Inven.Add(Fightable.magicBook);
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
    public void MainLoop()
    {
        ConsoleKeyInfo info = IO.rk();
        ConsoleKey key = info.Key;
        switch (key)
        {
            case ConsoleKey.RightArrow:
            case ConsoleKey.L:
                if (info.Modifiers == ConsoleModifiers.Control) IO.Redraw();
                else if (s_player.CanMove(MOVERIGHT)) s_player.SelectBasicBehaviour(0, MOVERIGHT.x, (int)MOVERIGHT.facing);
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                if (s_player.CanMove(MOVELEFT)) s_player.SelectBasicBehaviour(0, MOVELEFT.x, (int)MOVELEFT.facing);
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
            case 'g':
                s_player.SelectBehaviour(Fightable.bareHand);
                break;
            case 'i':
                IO.DrawInventory();
                break;
            case '.': //Rest
            case 'n':
                s_player.SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                break;
            case '/':
            case 'm':
                s_player.ShowStats();
                break;
            case ' ': //상호작용
                if (s_player.UnderFoot is not null) s_player.SelectBasicBehaviour(2, 0, 0);
                break;
        }
    }
    internal static void ElaspeTurn()
    {
        var fights = Map.Current.Fightables;
        fights.ForEach(f =>
        {
            if (f.Stance.IsStun) f.Stance.ProcessStun();
            else f.DoTurn();
        });

        var firsts = from f in fights where f.Stance.CurrentBehav?.Stance == StanceName.Charge select f;
        var lasts = fights.Except(firsts);
        firsts.ToList().ForEach(m => m.InvokeBehaviour());
        lasts.ToList().ForEach(m => m.InvokeBehaviour());

        fights.ForEach(m =>
        {
            m.passives.Invoke((Fightable)m); //passives
        });
        fights.ForEach(m =>
        {
            m.OnTurnEnd(); //update target and reset stance
        });

        Map.Current.RemoveAndCreateCorpse();
        if (Map.Current.SpawnMobs && Turn % s_spawnrate == 0) Map.Current.Spawn();
        NewTurn();
    }
    public static void NewTurn()
    {
        Turn++;
        IO.Redraw();
    }
}