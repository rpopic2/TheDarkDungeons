global using IO = DarkDungeon.IO;
public class Program
{
    const string VERSION = "0.6.3-150323";
    static Player s_player => Player.instance;

    static async Task Main(string[] args) {
        if (args.Length > 0 && args[0] == "server") {
            var gameSocket = new GameSocket();
            await gameSocket.New();
            IO.IIO = gameSocket;
        }
        else IO.IIO = new CSIO();
        Console.CancelKeyPress += (_, _) => { Environment.Exit(1); };

        IO.pr("The Dungeon of the Mine " + Program.VERSION);

        IO.CheckInteractive();
        if (!IO.IsInteractive)
            return;

        Map.NewMap();
        PlayerCreation.CreatePlayerPrompt();

        IO.clr();
        IO.pr($"{s_player.Name}은 횃불에 의지한 채 숲속으로 걸어 들어갔다.");
        IO.rk();
        IO.Redraw();
        IO.pr("\n?을 눌러 도움말 표시.");

        // main loop
        do {
            Map.Current.OnTurnElapse();
        } while (s_player.IsAlive);

        if (IO.IIO is GameSocket gs) {
            gs.pr_player_death();
        }
        IO.pr(s_player.ToString());
        IO.pr($"{s_player.Name}은 여기에 잠들었다...");
        IO.rk();
    }


}
