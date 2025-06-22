namespace Clearline.MediaFlow.Probe.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class NullableBooleanConverter : JsonConverter<bool?>
{
    public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return BooleanConverter.ReadValue(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}
