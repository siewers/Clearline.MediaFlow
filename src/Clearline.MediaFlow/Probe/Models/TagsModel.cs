namespace Clearline.MediaFlow.Probe.Models;

using System.Text.Json;

internal sealed class TagsModel() : Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
{
    public TagsModel(JsonElement tags)
        : this()
    {
        foreach (var entry in tags.EnumerateObject())
        {
            switch (entry.Name.ToLowerInvariant())
            {
                case "language":
                    Language = entry.Value.GetString();
                    break;
                case "stream_count":
                    StreamCount = entry.Value.GetInt32();
                    break;
                case "title":
                    Title = entry.Value.GetString();
                    break;
                case "creation_time":
                    CreationTime = entry.Value.GetDateTimeOffset();
                    break;
                case "rotate":
                    Rotation = entry.Value.GetInt32();
                    break;
                case "bps":
                    Bitrate = long.Parse(entry.Value.GetString()!);
                    break;
                case "duration":
                    Duration = entry.GetFFprobeTimeSpan();
                    break;
                default:
                    Add(entry.Name, entry.Value.GetString());
                    break;
            }
        }
    }

    public string? Language { get; }

    public int? StreamCount { get; }

    public string? Title { get; }

    public DateTimeOffset? CreationTime { get; }

    public int? Rotation { get; }

    public long? Bitrate { get; }

    public TimeSpan? Duration { get; }
}
