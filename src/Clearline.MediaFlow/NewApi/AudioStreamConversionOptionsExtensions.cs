namespace Clearline.MediaFlow.NewApi;

public static class AudioStreamConversionOptionsExtensions
{
    public static AudioStreamConversionOptions WithCodec(this AudioStreamConversionOptions options, AudioCodec codec)
    {
        options.AddPostInputArgument("c:a", codec);
        return options;
    }

    public static AudioStreamConversionOptions WithInputFormat(this AudioStreamConversionOptions options, Format inputFormat)
    {
        options.AddPreInputArgument("f", inputFormat);
        return options;
    }

    public static AudioStreamConversionOptions WithBitrate(this AudioStreamConversionOptions options, long minBitrate, long? maxBitrate = null, long? bufferSize = null)
    {
        options.AddPostInputArgument($"b:a:{options.Stream.Index}", minBitrate);

        if (maxBitrate.HasValue)
        {
            options.AddPostInputArgument("maxrate", maxBitrate.Value);
        }

        if (bufferSize.HasValue)
        {
            options.AddPostInputArgument("bufsize", bufferSize.Value);
        }

        return options;
    }

    public static AudioStreamConversionOptions WithLanguage(this AudioStreamConversionOptions options, string language)
    {
        options.SetLanguage(streamType: 'a', language);
        return options;
    }

    public static AudioStreamConversionOptions WithSampleRate(this AudioStreamConversionOptions options, int sampleRate)
    {
        options.AddPostInputArgument($"ar:{options.Stream.Index}", sampleRate);
        return options;
    }

    public static AudioStreamConversionOptions WithSeek(this AudioStreamConversionOptions options, TimeSpan seek)
    {
        if (seek > options.Stream.Duration)
        {
            throw new ArgumentException("Seek can not be greater than audio duration. Seek: " + seek + " Duration: " + options.Stream.Duration);
        }

        options.AddPreInputArgument("ss", seek);
        return options;
    }

    public static AudioStreamConversionOptions WithNativeInputRead(this AudioStreamConversionOptions options, bool readInputAtNativeFrameRate = true)
    {
        if (readInputAtNativeFrameRate)
        {
            options.AddPreInputArgument("re");
        }

        return options;
    }

    public static AudioStreamConversionOptions WithStreamLoop(this AudioStreamConversionOptions options, int loopCount)
    {
        options.AddPreInputArgument("stream_loop", loopCount);
        return options;
    }
}
