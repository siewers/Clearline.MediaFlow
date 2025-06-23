namespace Clearline.MediaFlow.Probe.Models;

using System.Text.Json.Serialization;

internal sealed class VideoStreamModel : StreamModelBase
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("pix_fmt")]
    public required string PixelFormat { get; set; }

    [JsonPropertyName("coded_height")]
    public int CodedHeight { get; set; }

    [JsonPropertyName("coded_width")]
    public int CodedWidth { get; set; }
}
