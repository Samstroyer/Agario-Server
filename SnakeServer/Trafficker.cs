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
            MessageType = "InitFood",
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

        switch (type)
        {
            case "ate":
                // Well, the food handler is called chef sometimes
                Chef(content);
                break;
            case "position":
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

    private void Chef(string content)
    {
        List<FoodUpdate> newFood = new();

        List<int> eatenIndexList = JsonSerializer.Deserialize<List<int>>(content);
        foreach (int i in eatenIndexList)
        {
            Brain.foodPoints[i] = new();

            FoodUpdate f = new()
            {
                Food = Brain.foodPoints[i],
                Index = i
            };
            newFood.Add(f);
        }

        string newFoodJson = JsonSerializer.Serialize<List<FoodUpdate>>(newFood);
        string foodUpdate = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = "FoodUpdate",
            Content = newFoodJson
        });

        Sessions.Broadcast(foodUpdate);
        Console.WriteLine("Updated foodpoints: " + foodUpdate);

        // Console.WriteLine("Updated foodpoints to clients!");
    }
}
