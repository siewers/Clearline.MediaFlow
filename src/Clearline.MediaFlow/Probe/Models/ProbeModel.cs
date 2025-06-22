namespace Clearline.MediaFlow.Probe.Models;

using System.Text.Json.Serialization;

internal class ProbeModel(FormatModel format, IReadOnlyCollection<StreamModelBase> streams)
{
    [JsonPropertyName("format")]
    public FormatModel Format { get; } = format;

    [JsonPropertyName("streams")]
    public IReadOnlyCollection<StreamModelBase> Streams { get; } = streams;
}
