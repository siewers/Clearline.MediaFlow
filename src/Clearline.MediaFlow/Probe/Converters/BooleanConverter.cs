namespace Clearline.MediaFlow.Probe.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class BooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ReadValue(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public static bool ReadValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => reader.GetInt32() == 1,
            JsonTokenType.String => bool.Parse(reader.GetString()!),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
        };
    }
}
