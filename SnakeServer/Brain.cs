using WebSocketSharp.Server;
using WebSocketSharp;
using System.Net;

public class Brain
{
    public static Dictionary<string, SnakeProperties> playerDict = new();

    WebSocketServer wssv;

    public Brain()
    {
        string ip = Console.ReadLine();
        Console.WriteLine("Attempting to open on: ws://" + ip + ":3000");

        wssv = new("ws://" + ip + ":3000");
        wssv.AddWebSocketService<Trafficker>("");

        wssv.Start();
    }

    public void Start()
    {
        Console.ReadKey();

        wssv.Stop();
    }
}
