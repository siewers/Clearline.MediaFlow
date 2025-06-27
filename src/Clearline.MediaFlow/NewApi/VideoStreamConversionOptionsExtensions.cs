namespace Clearline.MediaFlow.NewApi;

public static class VideoStreamConversionOptionsExtensions
{
    public static VideoStreamConversionOptions WithInputFormat(this VideoStreamConversionOptions options, Format inputFormat)
    {
        options.AddPreInputArgument("f", inputFormat);
        return options;
    }

    public static VideoStreamConversionOptions WithCustomArgument(this VideoStreamConversionOptions options, string argument, ArgumentPosition position = ArgumentPosition.PostInput)
    {
        options.AddArgument(ConversionArgument.Create(argument, position));
        return options;
    }

    public static VideoStreamConversionOptions WithCodec(this VideoStreamConversionOptions options, VideoCodec codec)
    {
        options.AddPostInputArgument("c:v", codec);
        return options;
    }

    public static VideoStreamConversionOptions WithBitrate(this VideoStreamConversionOptions options, long minBitrate, long? maxBitrate = null, long? bufferSize = null)
    {
        options.AddPostInputArgument("b:v", minBitrate);

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

    public static VideoStreamConversionOptions WithLanguage(this VideoStreamConversionOptions options, string language)
    {
        options.SetLanguage(streamType: 'v', language);
        return options;
    }

    public static VideoStreamConversionOptions UseNativeInputRead(this VideoStreamConversionOptions options, bool readInputAtNativeFrameRate = true)
    {
        if (readInputAtNativeFrameRate)
        {
            options.AddPreInputArgument("re");
        }

        return options;
    }

    public static VideoStreamConversionOptions WithStreamLoop(this VideoStreamConversionOptions options, int loopCount)
    {
        options.AddPreInputArgument("stream_loop", loopCount);
        return options;
    }

    public static VideoStreamConversionOptions WithSeek(this VideoStreamConversionOptions options, TimeSpan seek)
    {
        if (seek > options.Stream.Duration)
        {
            throw new ArgumentException("Seek can not be greater than video duration. Seek: " + seek + " Duration: " + options.Stream.Duration);
        }

        options.AddPreInputArgument("ss", seek);
        return options;
    }

    public static VideoStreamConversionOptions WithSpeed(this VideoStreamConversionOptions options, double multiplier)
    {
        if (multiplier is < 0.5 or > 2.0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Value has to be greater than 0.5 and less than 2.0.");
        }

        var videoMultiplier = multiplier >= 1 ? 1 - (multiplier - 1) / 2 : 1 + (multiplier - 1) * -2;

        options.Filters.Add(new Filter("setpts", $"{videoMultiplier.ToFFmpegFormat()}*PTS "));
        return options;
    }

    public static VideoStreamConversionOptions WithAspectRatio(this VideoStreamConversionOptions options, string aspectRatio)
    {
        options.AddPostInputArgument("aspect", aspectRatio);
        return options;
    }

    public static VideoStreamConversionOptions WithPixelFormat(this VideoStreamConversionOptions options, PixelFormat pixelFormat)
    {
        options.AddPostInputArgument("pix_fmt", pixelFormat);
        return options;
    }

    public static VideoStreamConversionOptions WithFramerate(this VideoStreamConversionOptions options, double framerate)
    {
        options.AddPostInputArgument("r", framerate.ToFFmpegFormat(decimalPlaces: 3));
        return options;
    }

    public static VideoStreamConversionOptions WithFramerate(this VideoStreamConversionOptions options, int framerate)
    {
        return options.WithFramerate((double)framerate);
    }

    public static VideoStreamConversionOptions WithFramerate(this VideoStreamConversionOptions options, TimeSpan framerate)
    {
        if (framerate.TotalSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(framerate), "Framerate must be greater than zero.");
        }

        var fps = 1 / framerate.TotalSeconds;
        return options.WithFramerate(fps);
    }

    public static VideoStreamConversionOptions WithResolution(this VideoStreamConversionOptions options, int width, int height)
    {
        options.AddPostInputArgument("s", $"{width}x{height}");
        return options;
    }

    public static VideoStreamConversionOptions WithPadding(this VideoStreamConversionOptions options, int width, int height)
    {
        var parameter = $"\"scale={width}:{height}:force_original_aspect_ratio=decrease,pad={width}:{height}:-1:-1:color=black\"";
        options.AddPostInputArgument("vf", parameter);
        return options;
    }

    // TODO: Does this even work? AI slop?
    public static VideoStreamConversionOptions WithRotate(this VideoStreamConversionOptions options, int degrees)
    {
        if (degrees % 90 != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(degrees), "Rotation must be a multiple of 90 degrees.");
        }

        var rotation = degrees / 90;
        options.AddPostInputArgument("vf", $"\"transpose={rotation}\"");
        return options;
    }

    public static VideoStreamConversionOptions WithRotate(this VideoStreamConversionOptions options, RotateDegrees rotateDegrees)
    {
        var rotate = rotateDegrees == RotateDegrees.Invert
            ? "\"transpose=2,transpose=2\" "
            : $"\"transpose={(int)rotateDegrees}\" ";

        options.AddPostInputArgument("vf", rotate);
        return options;
    }

    public static VideoStreamConversionOptions WithLoop(this VideoStreamConversionOptions options, int count, int delay = 0)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Loop count must be zero or greater.");
        }

        options.AddPostInputArgument("loop", count);

        if (delay > 0)
        {
            options.AddPostInputArgument("final_delay", delay / 100);
        }

        return options;
    }

    public static VideoStreamConversionOptions Reverse(this VideoStreamConversionOptions options)
    {
        options.AddPostInputArgument("vf", "reverse");
        return options;
    }

    public static VideoStreamConversionOptions WithBitstreamFilter(this VideoStreamConversionOptions options, BitstreamFilter filter)
    {
        return options.WithBitstreamFilter(filter.ToStringFast(useMetadataAttributes: true));
    }

    public static VideoStreamConversionOptions WithBitstreamFilter(this VideoStreamConversionOptions options, string filter)
    {
        options.AddPostInputArgument("bsf:v", filter);
        return options;
    }
}
