using System.Text.Json.Serialization;

public class PlayerProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("size"), JsonInclude]
    public int Size { get; set; }
}
