global using Entities;
public class Program
{
    public const string VERSION = "0.6.170422";
    public static Program instance = default!;
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
        IO.pr(s_player.ToString());
        IO.pr($"{s_player.Name}은 여기에 잠들었다...");
        IO.rk();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dungeon of the Mine " + VERSION);
        Intro();
        Console.Clear();
        IO.rk($"{s_player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
        IO.Redraw();
        IO.pr("\n?을 눌러 도움말 표시.");
    }
    private static void ElaspeTurn()
    {
        var fights = Map.Current.Fightables;
        fights.ForEach(f =>
        {
            if (f.Stance.IsStun) f.Stance.ProcessStun();
            else f.SelectAction();
        });

        var firsts = from f in fights where f.Stance.CurrentBehav?.Stance == StanceName.Charge select f;
        var lasts = fights.Except(firsts);
        foreach (Fightable item in firsts) item.InvokeBehaviour();
        foreach (Fightable item in lasts) item.InvokeBehaviour();
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
        IO.rk("Press any key to start...");

        string name;
        do
        {
            IO.pr("캐릭터의 이름은?...");
            name = Console.ReadLine() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);

        IO.pr($"{name}의 직업은?...");
        int classIndex = 0;
        string[] classes = new string[] { "^r(q) 검사^/", "^g(w) 암살자^/", "^b(e) 마법사^/" };
        IO.sel(classes, 0, out classIndex, out bool cancel, out _, out _);
        if (classIndex != -1) IO.pr(classes[classIndex]);

        Player player = Player._instance = new Player(name);
        Map.NewMap();

        switch (classIndex)
        {
            case 0:
                player.Inven.Add(Fightable.torch);
                player.Inven.Add(Fightable.sword);
                player.Inven.Add(Fightable.shield);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                break;
            case 1:
                player.Inven.Add(Fightable.torch);
                player.Inven.Add(Fightable.dagger);
                player.Inven.Add(Fightable.bow);
                player.Inven.Add(Fightable.arrow);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Defence);
                break;
            case 2:
                player.Inven.Add(Fightable.torch);
                player.Inven.Add(Fightable.staff);
                player.Inven.Add(Fightable.magicBook);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Offence);
                player.PickupToken(TokenType.Charge);
                break;
            default:
                player.SelectStartItem();
                player.PickupToken(3);
                break;
        }
        player.SelectPickupStat(3);
    }
}