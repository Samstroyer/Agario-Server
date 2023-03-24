using WebSocketSharp.Server;
using System.Text.Json;
using WebSocketSharp;
using System.Timers;

public class Trafficker : WebSocketBehavior
{
    System.Timers.Timer positionSender = new(250)
    {
        AutoReset = true,
        Enabled = true,
    };
    bool firstConnection = true;

    protected override void OnOpen()
    {
        if (firstConnection) positionSender.Elapsed += BroadcastPositions;

        string foodJson = JsonSerializer.Serialize<List<Food>>(Brain.foodPoints);

        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = MessageType.GetFood,
            Content = foodJson
        });

        Send(packet);

        Console.WriteLine("Connected a client!");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        SendInfo packet = JsonSerializer.Deserialize<SendInfo>(e.Data);

        string content = packet.Content;

        switch (packet.MessageType)
        {
            case MessageType.Food:
                Chef(content);
                break;

            case MessageType.Position:
                string pcID = packet.ID;

                while (Brain.listLock) ;
                Brain.listLock = true;
                if (Brain.playerDict.ContainsKey(pcID))
                {
                    Brain.playerDict[pcID] = JsonSerializer.Deserialize<PlayerProperties>(content);
                }
                else
                {
                    Brain.playerDict.Add(pcID, JsonSerializer.Deserialize<PlayerProperties>(content));
                }
                Brain.listLock = false;
                Console.WriteLine("Player {0} at X:{1}, Y:{2}, Size:{3}", pcID, Brain.playerDict[pcID].X, Brain.playerDict[pcID].Y, Brain.playerDict[pcID].Size);
                break;

            case MessageType.Close:
                string removeID = packet.ID;

                while (Brain.listLock) ;
                Brain.listLock = true;
                if (Brain.playerDict.ContainsKey(removeID))
                {
                    Brain.playerDict.Remove(removeID);
                    Console.WriteLine("Removed {0} from player dictionary!", removeID);
                }
                else Console.WriteLine("Failed to find {0} to remove!", removeID);
                Brain.listLock = false;
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
            MessageType = MessageType.Food,
            Content = newFoodJson
        });

        Sessions.Broadcast(foodUpdate);
        Console.WriteLine("Updated foodpoints: " + foodUpdate);
    }

    public void BroadcastPositions(Object sender, ElapsedEventArgs e)
    {
        while (Brain.listLock) ;
        Brain.listLock = true;

        string otherPlayers = JsonSerializer.Serialize<Dictionary<string, PlayerProperties>>(Brain.playerDict);
        string playerUpdate = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = MessageType.OtherPlayers,
            Content = otherPlayers
        });

        Console.WriteLine("Amount of players:{0}", Brain.playerDict.Count);

        Sessions.Broadcast(playerUpdate);
        Console.WriteLine(playerUpdate);

        Brain.listLock = false;
    }
}
