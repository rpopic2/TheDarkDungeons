global using Entities;
public class Program
{
    public static Program instance = default!;
    public static readonly string[] stats = new string[] { "^r(q) 힘/체력^/", "^g(w) 정밀/민첩^/", "^b(e) 마력/지능^/" };
    private static Player s_player { get => Player.instance; }
    private static int s_spawnrate = 10;
    public static int Turn { get; private set; }
    public static void Main()
    {
        instance = new Program();
        do
        {
            ElaspeTurn();
        } while (s_player.IsAlive);
        IO.pr(s_player.ToString(), __.color);
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
        IO.rk($"{s_player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
        IO.Redraw();
        IO.pr("?을 눌러 도움말 표시.");
    }
    private static void ElaspeTurn()
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
        Turn++;
        IO.Redraw();
    }
    private void Intro()
    {
        string[] classes = new string[] { "^r(q) 검사^/", "^g(w) 암살자^/", "^b(e) 마법사^/" };
        IO.rk("Press any key to start...", __.color);

        IO.pr("캐릭터의 이름은?...");
        string name = Console.ReadLine() ?? "Michael";
        IO.del(2);
        IO.pr($"{name}의 직업은?...");
        int index = 0;
        IO.sel(classes, __.color, out index, out bool cancel, out _, out _);
        IO.pr(classes[index], __.color);
        Player player = Player._instance = new Player(name);
        Map.NewMap();

        switch (index)
        {
            case 0:
                player.Inven.Add(Fightable.sword);
                player.Inven.Add(Fightable.shield);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                break;
            case 1:
                player.Inven.Add(Fightable.dagger);
                player.Inven.Add(Fightable.bow);
                player.Inven.Add(Fightable.arrow);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Defence);
                break;
            case 2:
                player.Inven.Add(Fightable.staff);
                player.Inven.Add(Fightable.magicBook);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Charge);
                break;
            default:
                break;
        }
        player.Inven.Add(Fightable.torch);
        player.SelectPickupStat(3);
    }
}