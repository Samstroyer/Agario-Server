using WebSocketSharp.Server;
using System.Text.Json;
using WebSocketSharp;

public class Trafficker : WebSocketBehavior
{
    protected override void OnOpen()
    {
        string foodJson = JsonSerializer.Serialize<List<Food>>(Brain.foodPoints);

        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = "FoodUpdate",
            Content = foodJson
        });

        Send(packet);

        Console.WriteLine("Connected a client!");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        SendInfo packet = JsonSerializer.Deserialize<SendInfo>(e.Data);

        string type = packet.MessageType;
        string content = packet.Content;

        Console.WriteLine(content);

        switch (type)
        {
            case "ateFood":
                // Fix new food and remove old
                break;
            case "updatePosition":
                string pcID = packet.ID;

                if (Brain.playerDict.ContainsKey(pcID))
                {
                    Brain.playerDict[pcID] = JsonSerializer.Deserialize<SnakeProperties>(content);
                }
                else
                {
                    Brain.playerDict.Add(pcID, JsonSerializer.Deserialize<SnakeProperties>(content));
                }
                break;
            default:
                Console.WriteLine("Received unknown message type: " + packet.MessageType);
                Console.WriteLine("Content: " + packet.Content);
                break;
        }
    }
}
