namespace Clearline.MediaFlow.Probe.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class NullableTimeSpanConverter : JsonConverter<TimeSpan?>
{
    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return TimeSpanConverter.ReadValue(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}
