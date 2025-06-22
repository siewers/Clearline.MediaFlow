namespace Clearline.MediaFlow.Probe;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Converters;

internal static class FFprobeJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Instance = new()
                                                            {
                                                                PropertyNameCaseInsensitive = false,
                                                                AllowOutOfOrderMetadataProperties = true,
                                                                Converters =
                                                                {
                                                                    new BooleanConverter(),
                                                                    new NullableBooleanConverter(),
                                                                    new TimeSpanConverter(),
                                                                    new NullableTimeSpanConverter(),
                                                                    new StreamDispositionModelConverter(),
                                                                    new FormatModelConverter(),
                                                                    new TagsModelConverter(),
                                                                },
                                                                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                                                                                   {
                                                                                       Modifiers = { SetNumberHandlingModifier },
                                                                                   },
                                                            };

    private static void SetNumberHandlingModifier(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Type == typeof(int) ||
            jsonTypeInfo.Type == typeof(int?) ||
            jsonTypeInfo.Type == typeof(long) ||
            jsonTypeInfo.Type == typeof(long?) ||
            jsonTypeInfo.Type == typeof(double) ||
            jsonTypeInfo.Type == typeof(double?))
        {
            jsonTypeInfo.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        }
    }
}
