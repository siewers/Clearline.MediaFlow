namespace Clearline.MediaFlow.Probe.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class StreamDispositionModel : Dictionary<string, bool>
{
    public StreamDispositionModel(JsonElement jsonDisposition)
        : base(StringComparer.OrdinalIgnoreCase)
    {
        foreach (var entry in jsonDisposition.EnumerateObject())
        {
            switch (entry.Name.ToLowerInvariant())
            {
                case "default":
                    IsDefault = entry.GetFFprobeBoolean();
                    break;
                case "forced":
                    IsForced = entry.GetFFprobeBoolean();
                    break;
                default:
                    Add(entry.Name, entry.GetFFprobeBoolean());
                    break;
            }
        }
    }

    [JsonPropertyName("default")]
    public bool? IsDefault { get; }

    [JsonPropertyName("forced")]
    public bool? IsForced { get; }
}
