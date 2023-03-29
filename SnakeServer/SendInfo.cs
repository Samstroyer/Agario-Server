using System.Text.Json.Serialization;
using System.Net;

public enum MessageType
{
    Position = 1,
    OtherPlayers = 2,
    Food = 3,
    GetFood = 4,
    Battle = 5,
    Close = 1001
}

public class SendInfo
{
    [JsonPropertyName("type"), JsonInclude]
    public MessageType MessageType { get; set; }

    [JsonPropertyName("content"), JsonInclude]
    public string Content { get; set; }

    [JsonPropertyName("id"), JsonInclude]
    public string ID { get; set; } = "Server";
}
