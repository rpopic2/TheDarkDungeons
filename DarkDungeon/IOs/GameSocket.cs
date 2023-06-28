using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameSocket : IIO {
    private static Player s_player => Player.Me;
    private const string RK = "<rk>";
    private const string CLR = "<clr>";
    private Socket? s_client;

    ~GameSocket() {
        s_client?.Close();
        Console.WriteLine("Closing connection.");
    }

    public async static Task<GameSocket> New() {
        Console.WriteLine("Creating connection...");
        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        socket.Bind(ipEndPoint);
        socket.Listen();

        var gs = new GameSocket();

        gs.s_client = await socket.AcceptAsync();
        gs.s_client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        Console.WriteLine("Connection created!");
        return gs;
    }

    public void pr(string value, bool newline = true) {
        if (value == string.Empty)
            newline = true;
        if (newline)
            value += "\n";
        Console.WriteLine($"Sending: {value.Length}: {value}");
        SafeSend(value);
    }

    void SafeSend(string value) {
        var msglen = Encoding.UTF8.GetByteCount(value);
        s_client?.Send(BitConverter.GetBytes(msglen));
        s_client?.Send(Encoding.UTF8.GetBytes(value));
    }

    public void pr_intro(string value) {
        pr($"<game_intro>{value}");
    }

    public void pr_new_map(int len) {
        pr($"<new_map>{len}");
    }

    public void pr_map() {
        var facing = (int)Player.Me.Pos.facing;
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
        pr($"<mname>{frontCreature.Name}<mchar>{frontCreature.ToChar()}");
    }

    public void pr_inventory() {
        pr($"<items>{s_player.Inven.ToNetString()}");
    }

    public void pr_skill(IBehaviour[] skills) {
        var stringified = string.Join(',', skills.Select(s => s.Name));
        pr($"<skills>{stringified}");
    }

    public void pr_underfoot(char was) {
        pr($"<underfoot>{was}");
    }

    public void pr_corpse(int pos, Corpse corpse) {
        pr($"<corpse>{pos},{corpse.was}");
    }

    public void pr_loot(Inventory droplist) {
        pr($"<loot>{droplist.ToNetString()}");
    }

    public void pr_lvup(string value) {
        pr($"<lvup>{value}");
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
        pr("<rl>");
        var prefix = SafeRead(PREFIX_SIZE);
        var msglen = BitConverter.ToInt32(prefix);

        var buffer = SafeRead(msglen);
        var str = Encoding.UTF8.GetString(buffer);
        Console.WriteLine("[Client][rl] " + str);
        return str;
    }

    const int PREFIX_SIZE = sizeof(int);
    const int BUF_SIZE = 4096;
    byte[] _readBuffer = new byte[BUF_SIZE];

    Span<byte> SafeRead(int bytes_expected) {
        var bytes_recieved = 0;
        do {
            bytes_recieved += s_client?.Receive(_readBuffer, bytes_expected, SocketFlags.None) ?? 0;
            if (bytes_recieved == 0) {
                throw new Exception("Connection closed");
            }
        } while (bytes_recieved < bytes_expected);
        return _readBuffer.AsSpan(0, bytes_expected);
    }

    public void del() {
        pr("<del>");
    }

    public void clr() {
        Console.WriteLine("Clearing screen...");
        pr(CLR, false);
    }

    /// <summary>
    /// Send the player's current behaviour information.
    /// position, stance, item name
    /// </summary>
    public void pr_current_behaviour(Position pos, CurrentAction action) {
        pr($"<cur_behav>{pos.x},{action.CurrentBehav?.Stance},{action.CurrentItem?.Name},{action.CurrentBehav?.Name}");
    }

    public void pr_player_death() {
        pr("<pdeath>");
    }

    // status effects
    List<string> _statusEffectBuffer = new();

    public void pr_status_effect(string effect, bool active, int pos) {
        var no = active ? string.Empty : "no";
        _statusEffectBuffer.Add($"<s_{no}{effect}>{pos}");
    }

    public void flush_status_effects() {
        foreach (var s in _statusEffectBuffer) {
            pr(s);
        }
        _statusEffectBuffer.Clear();
    }
}
