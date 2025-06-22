namespace Clearline.MediaFlow.Probe;

using System.Text.Json;

internal static class FFprobeJsonDeserializer
{
    public static T? Deserialize<[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)] T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, FFprobeJsonSerializerOptions.Instance);
    }
}
