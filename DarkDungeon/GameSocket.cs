using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameSocket : IIO
{
    private Socket? s_listener;
    private Socket? s_client;
    ~GameSocket() 
    {
        s_listener?.Close();
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
        Console.WriteLine("Connection created!");
    }

    public void pr(string value, bool newline = true)
    {
        if (newline) value += "\n";
        s_client?.Send(Encoding.UTF8.GetBytes(value));
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
        Console.WriteLine($"Received: {buffer[0]}");
        return new((char)buffer[0], (ConsoleKey)buffer[0], false, false, false);
    }
    public string rl()
    {
        return "Unity";
    }
}
