using System.Text.Json.Serialization;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("pointsArr"), JsonInclude]
    public (int x, int y)[] PreviousPointsArray { get; set; }
}
