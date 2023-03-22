using WebSocketSharp.Server;
using System.Text.Json;
using WebSocketSharp;

public class Trafficker : WebSocketBehavior
{
    protected override void OnOpen()
    {
        Brain.playerDict
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        SendInfo packet = JsonSerializer.Deserialize<SendInfo>(e.Data);

        switch (packet.MessageType)
        {
            case "ateFood":
                // Fix new food and remove old
                break;
            case "updatePosition":
                // Update from the ID
                break;
            default:
                Console.WriteLine("Received unknown message type: " + packet.MessageType);
                Console.WriteLine("Content: " + packet.Content);
                break;
        }
    }
}
