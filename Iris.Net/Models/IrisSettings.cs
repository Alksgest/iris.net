using System.Text.Json.Serialization;

namespace Iris.Net.Models;

public class IrisSettings
{
    [JsonPropertyName("mainFile")]
    public string? MainFile { get; set; }
}