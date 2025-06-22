namespace Clearline.MediaFlow.Probe;

using System.Text.Json;

internal static class FFprobeJsonExtensions
{
    public static bool GetFFprobeBoolean(this JsonProperty property)
    {
        return property.Value.Deserialize<bool>(FFprobeJsonSerializerOptions.Instance);
    }

    public static TimeSpan GetFFprobeTimeSpan(this JsonProperty property)
    {
        return property.Value.Deserialize<TimeSpan>(FFprobeJsonSerializerOptions.Instance);
    }
}
