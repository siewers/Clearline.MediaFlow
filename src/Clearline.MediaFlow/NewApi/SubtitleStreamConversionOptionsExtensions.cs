namespace Clearline.MediaFlow.NewApi;

public static class SubtitleStreamConversionOptionsExtensions
{
    public static SubtitleStreamConversionOptions WithCodec(this SubtitleStreamConversionOptions options, SubtitleCodec codec)
    {
        options.AddPostInputArgument("c:s", codec);
        return options;
    }

    public static SubtitleStreamConversionOptions WithLanguage(this SubtitleStreamConversionOptions options, string language)
    {
        options.SetLanguage(streamType: 's', language);
        return options;
    }
}
