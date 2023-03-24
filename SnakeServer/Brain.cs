using WebSocketSharp.Server;
using System.Net;

public class Brain
{
    public static bool listLock = false;
    public static Dictionary<string, SnakeProperties> playerDict = new();

    WebSocketServer wssv;

    public static List<Food> foodPoints = new();

    public Brain()
    {
        // string ip = Console.ReadLine();
        // Console.WriteLine("Attempting to open on: ws://" + ip + ":3000");
        // wssv = new("ws://" + ip + ":3000");

        wssv = new("ws://10.151.173.27:3000");
        Console.WriteLine("Opening on " + wssv.Address);
        wssv.AddWebSocketService<Trafficker>("/snake");

        for (int i = 0; i < 50; i++)
        {
            foodPoints.Add(new());
        }

        wssv.Start();




    }

    public void Start()
    {
        Console.ReadKey();

        wssv.Stop();
    }
}
