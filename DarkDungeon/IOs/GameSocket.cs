using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameSocket : IIO {
    private static Player s_player => Player.instance;
    private const string RK = "<rk>";
    private const string CLR = "<clr>";
    private Socket? s_client;

    ~GameSocket() {
        s_client?.Close();
        Console.WriteLine("Closing connection.");
    }

    public async Task New() {
        Console.WriteLine("Creating connection...");
        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        socket.Bind(ipEndPoint);
        socket.Listen();

        s_client = await socket.AcceptAsync();
        s_client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        Console.WriteLine("Connection created!");
    }

    public void pr(string value, bool newline = true) {
        if (value == string.Empty)
            newline = true;
        if (newline)
            value += "\n";
        var msglen = Encoding.UTF8.GetByteCount(value);
        Console.WriteLine($"Sending: {value.Length}: {value}");
        s_client?.Send(BitConverter.GetBytes(msglen));
        s_client?.Send(Encoding.UTF8.GetBytes(value));
    }

    public void pr_map() {
        var facing = (int)Player.instance.Pos.facing;
        pr($"<map>{facing}{Map.Current.Rendered}", false);
    }

    public void pr_depth_lv() {
        pr($"<depth>{Map.Depth}<level>{s_player.Level}<exp>{s_player.exp}");
    }

    public void pr_hp_energy() {
        pr($"<hp>{s_player.GetHp()}<energy>{s_player.Energy}<sight>{s_player.Stat.Sight}");
    }

    public void pr_monster_hp_energy(Creature? frontCreature) {
        if (frontCreature == null) {
            pr("<nom>");
            return;
        }
        pr($"<mpos>{frontCreature.Pos.x}<mhp>{frontCreature.GetHp()}<menergy>{frontCreature.Energy}");
    }

    public void pr_inventory() {
        pr($"<items>{s_player.Inven.ToNetString()}");
    }

    public void pr_skill(IBehaviour[] skills) {
        var stringified = string.Join(',', skills.Select(s => s.Name));
        pr($"<skills>{stringified}");
    }

    public void pr_corpse(int pos, Corpse corpse) {
        pr($"<corpse>{pos},{corpse.was}");
    }

    public void pr_loot(Inventory droplist) {
        pr($"<loot>{droplist.ToNetString()}");
    }

    public ConsoleKeyInfo rk() {
        pr(RK, false);
        var buffer = new byte[1];
        Console.WriteLine("Waiting for input...");
        var bytes_read = s_client?.Receive(buffer, 1, SocketFlags.None);
        if (bytes_read == 0) {
            Console.WriteLine("Connection closed by client.");
            Environment.Exit(1);
        }
        Console.WriteLine($"Received: {Encoding.UTF8.GetString(buffer, 0, 1)}");
        return new((char)buffer[0], (ConsoleKey)buffer[0], false, false, false);
    }

    public string rl() {
        return "Unity";
    }

    public void clr() {
        Console.WriteLine("Clearing screen...");
        pr(CLR, false);
    }

    public void pr_current_behaviour(Position pos, CurrentAction action) {
        pr($"<stance>{action.CurrentBehav?.Stance},{pos.x}");
    }
}
