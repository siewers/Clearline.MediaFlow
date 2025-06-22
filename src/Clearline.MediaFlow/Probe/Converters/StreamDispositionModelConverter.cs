namespace Clearline.MediaFlow.Probe.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Models;

internal sealed class StreamDispositionModelConverter : JsonConverter<StreamDispositionModel>
{
    public override StreamDispositionModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDisposition = JsonElement.ParseValue(ref reader);
        return new StreamDispositionModel(jsonDisposition);
    }

    public override void Write(Utf8JsonWriter writer, StreamDispositionModel value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}
