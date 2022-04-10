﻿global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] classes = new string[] { "(q) 검사", "(w) 암살자", "(e) 마법사" };
    public static readonly string[] stats = new string[] { "(q) 힘/체력", "(w) 정밀/민첩", "(e) 마력/지능" };
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
        IO.del();
        IO.pr($"{name}의 직업은?...");
        int index = 0;
        IO.seli(classes, out index, out bool cancel, out _, out _);
        IO.pr(classes[index]);
        Player player = Player._instance = new Player(name);
        Map.NewMap();

        switch (index)
        {
            case 0:
                player.PickupItem(Inventoriable.sword);
                break;
            case 1:
                player.PickupItem(Inventoriable.dagger);
                break;
            case 2:
                player.PickupItem(Inventoriable.staff);
                break;
            default:
                break;
        }
        IO.pr("남은 능력치 포인트 : 1");
        player.SelectPickupStat();
        IO.del();
        IO.pr("얻을 토큰 종류를 선택해 주십시오. (3)");
        player.SelectPickupToken();
        player.SelectPickupToken();
        player.SelectPickupToken();
        //player.PickupItem(Item.torch);
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
                player.SelectBasicBehaviour(0, 1, 1);
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                player.SelectBasicBehaviour(0, -1, 1);
                break;
            default:
                DefaultSwitch(info);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKeyInfo key)
    {
        bool found = IO.chki(key.KeyChar, player.Inven.Count, out int i);
        if (found && player.Inven[i] is Item item)
        {
            IO.seli(item.skills, out int index, out bool cancel, out _, out _);
            if (cancel) return;
            player.SelectBehaviour(item, index);
        }
        switch (key.KeyChar)
        {
            case 'y':
                IO.seli(Inventoriable.bareHand.skills, out int index, out bool cancel, out _, out _);
                if (cancel) return;
                player.SelectBehaviour(Inventoriable.bareHand, index);
                break;
            case ' ':
                player.InteractUnderFoot();
                break;
            case '.':
                IO.pr("토큰을 획득하였습니다.");
                IO.seli(Tokens.TokenPromptNames, out int tokenType, out bool cancelRest, out _, out _);
                IO.del();
                if (cancelRest) return;
                int discard = -1;
                if (player.tokens.IsFull)
                {
                    IO.pr("손패가 꽉 찼습니다. 버릴 토큰을 고르십시오. " + Tokens.ToString((TokenType)tokenType));
                    IO.seli_t(out discard, out bool cancel2, out _);
                    IO.del();
                    if (cancel2) return;
                }
                player.SelectBasicBehaviour(1, tokenType, discard);
                break;
            case '/':
                player.ShowStats();
                break;
        }
    }
}