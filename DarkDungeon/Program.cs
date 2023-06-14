global using IO = DarkDungeon.IO;
public static class Program
{
    public const string VERSION = "0.6.4-150608";

    static Player s_player => Player.Me;

    static async Task Main(string[] args) {
        var isServer = args.Length > 0 && args[0] == "server";
        var result = await IO.InitIO(isServer);
        if (result == Result.Failure)
            return;

        Map.New();
        Player.Me = PlayerCreator.New();

        IO.clr();
        IO.pr($"{s_player.Name}은 횃불에 의지한 채 숲속으로 걸어 들어갔다.");
        if (IO.ServerSocket is not null)
            IO.Redraw();
        IO.ServerSocket?.pr("<game_intro>");
        IO.rk(false);
        IO.Redraw();
        IO.ServerSocket?.pr("<game_start>");

        IO.pr("?을 눌러 도움말 표시.");

        MainLoop();

        IO.ServerSocket?.pr_player_death();
        IO.pr(s_player.ToString());
        IO.pr($"{s_player.Name}은 여기에 잠들었다...");
        IO.rk();
    }

    static void MainLoop() {
        do {
            Map.Current.OnTurnElapse();
            IO.Redraw();
            IO.PrintTurnHistory();
            IO.OnTurnEnd();
        } while (s_player.IsAlive);
    }

    enum IOType {
        Console,
        Server,
    }
}

public enum Result {
    Success,
    Failure,
}

