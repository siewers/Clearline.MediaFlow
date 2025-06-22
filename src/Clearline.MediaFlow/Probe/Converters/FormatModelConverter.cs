namespace Clearline.MediaFlow.Probe.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Models;

internal sealed class FormatModelConverter : JsonConverter<FormatModel>
{
    public override FormatModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new FormatModel(JsonElement.ParseValue(ref reader));
    }

    public override void Write(Utf8JsonWriter writer, FormatModel value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}
