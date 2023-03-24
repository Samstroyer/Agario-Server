using System.Text.Json.Serialization;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    [JsonPropertyName("size"), JsonInclude]
    public int Size { get; set; }
}
