using System.Text.Json.Serialization;

namespace ClassLibrary.Database.Models;

public class Joke
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("setup")]
    public required string Setup { get; set; }

    [JsonPropertyName("punchline")]
    public required string Punchline { get; set; }
}
