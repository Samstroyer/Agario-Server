using System.Text.Json.Serialization;

public class Food
{
    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    [JsonIgnore]
    Random r = new();

    public Food()
    {
        X = r.Next(-1000, 1000);
        Y = r.Next(-1000, 1000);
    }
}
