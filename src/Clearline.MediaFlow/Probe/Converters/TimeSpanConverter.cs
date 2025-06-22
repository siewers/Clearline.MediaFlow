namespace Clearline.MediaFlow.Probe.Converters;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ReadValue(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public static TimeSpan ReadValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => TimeSpan.FromSeconds(reader.GetDouble()),
            JsonTokenType.String => Parse(reader.GetString()),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
        };
    }

    private static TimeSpan Parse(string? duration)
    {
        if (duration is null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(duration);
        }

        if (double.TryParse(duration, NumberFormatInfo.InvariantInfo, out var seconds))
        {
            return TimeSpan.FromSeconds(seconds);
        }

        if (duration.Length > 16)
        {
            // Example FFmpeg duration: 01:29:43.253000000
            // Trim to the max timespan length (FFmpeg milliseconds component is 9 digits)
            duration = duration[..16];
        }

        if (TimeSpan.TryParse(duration, NumberFormatInfo.InvariantInfo, out var timeSpan))
        {
            return timeSpan;
        }

        throw new JsonException($"Invalid TimeSpan format: '{duration}'");
    }
}
