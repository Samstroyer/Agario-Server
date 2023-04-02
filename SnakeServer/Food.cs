using System.Text.Json.Serialization;

public class Food
{
    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    [JsonIgnore]
    Random r = new();

    public Food(int min, int max)
    {
        X = r.Next(min, max);
        Y = r.Next(min, max);
    }
}
