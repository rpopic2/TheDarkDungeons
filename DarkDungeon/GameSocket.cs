using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameSocket : IIO
{
    private const string EOM = "<EOM>";
    private Socket? s_client;
    ~GameSocket() 
    {
        s_client?.Close();
        Console.WriteLine("Closing connection.");
    }
    public async Task New()
    {
        Console.WriteLine("Creating connection...");
        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        socket.Bind(ipEndPoint);
        socket.Listen();
        s_client = await socket.AcceptAsync();
        s_client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        Console.WriteLine("Connection created!");
    }

    public void pr(string value, bool newline = true)
    {
        if (newline) value += "\n";
        if (value != "<EOT>") value += EOM;
        Console.WriteLine($"Sending: {value}");
        s_client?.Send(Encoding.UTF8.GetBytes(value));
    }
    public void eot()
    {
        s_client?.Send(Encoding.UTF8.GetBytes("<EOT>"));
    }

    public ConsoleKeyInfo rk()
    {
        var buffer = new byte[1];
        Console.WriteLine("Waiting for input...");
        var bytes_read = s_client?.Receive(buffer);
        if (bytes_read == 0)
        {
            Console.WriteLine("Connection closed by client.");
            Environment.Exit(1);
        }
        Console.WriteLine($"Received: {Encoding.UTF8.GetString(buffer, 0, 1)}");
        return new((char)buffer[0], (ConsoleKey)buffer[0], false, false, false);
    }
    public string rl()
    {
        return "Unity";
    }
    public void clr()
    {
        Console.WriteLine("Clearing screen...");
        s_client?.Send(Encoding.UTF8.GetBytes(@"\x1b[2J"));
    }
}
