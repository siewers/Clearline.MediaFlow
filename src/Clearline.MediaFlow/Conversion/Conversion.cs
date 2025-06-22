namespace Clearline.MediaFlow;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Events;

/// <inheritdoc />
[PublicAPI]
public partial class Conversion : IConversion
{
    private readonly ConversionArguments _arguments = [];
    private readonly Lock _builderLock = new();
    private readonly Dictionary<MediaLocation, int> _inputFileMap = [];
    private readonly List<IStream> _streams = [];
    private Func<string, string>? _buildInputFileName;
    private Func<string, MediaLocation?>? _buildOutputFileName;
    private FFmpegWrapper? _ffmpeg;
    private bool _hasInputBuilder;
    private MediaLocation? _output;
    private ProcessPriorityClass? _priority;

    /// <inheritdoc />
    public event ConversionProgressEventHandler? OnProgress;

    /// <inheritdoc />
    public event DataReceivedEventHandler? OnDataReceived;

    /// <inheritdoc />
    public event VideoDataEventHandler? OnVideoDataReceived;

    /// <inheritdoc />
    public MediaLocation? OutputFilePath { get; private set; }

    /// <inheritdoc />
    public PipeDescriptor? OutputPipeDescriptor { get; private set; }

    /// <inheritdoc />
    public IEnumerable<IStream> Streams => _streams;

    /// <inheritdoc />
    public Task<IConversionResult> Start(CancellationToken cancellationToken = default)
    {
        var parameters = Build();
        return Start(parameters, cancellationToken);
    }

    /// <inheritdoc />
    public string Build()
    {
        lock (_builderLock)
        {
            var builder = new StringBuilder();

            AppendParameters(builder, ArgumentPosition.PreInput);
            AppendStreams(builder, ArgumentPosition.PreInput);

            _buildOutputFileName ??= _ => _output;

            if (_buildInputFileName is not null)
            {
                _hasInputBuilder = true;
                var inputFileName = _buildInputFileName("_%03d");
                builder.Append(inputFileName);
            }

            AppendInputs(builder);
            AppendStreams(builder, ArgumentPosition.PostInput);
            AppendFilters(builder);
            AddStreamMappings(builder);
            AppendParameters(builder, ArgumentPosition.PostInput);

            AppendOutputFilePath(builder);

            return builder.ToString().Trim();
        }
    }

    /// <inheritdoc />
    public Task<IConversionResult> Start(string arguments)
    {
        return Start(arguments, CancellationToken.None);
    }

    /// <inheritdoc />
    public async Task<IConversionResult> Start(string arguments, CancellationToken cancellationToken)
    {
        if (_ffmpeg is not null)
        {
            throw new InvalidOperationException("Conversion has already been started.");
        }

        _ffmpeg = new FFmpegWrapper();

        try
        {
            _ffmpeg.OnProgress += OnProgress;
            _ffmpeg.OnDataReceived += OnDataReceived;
            _ffmpeg.OnVideoDataReceived += OnVideoDataReceived;
            var startDateTime = DateTime.UtcNow;
            var startTime = Stopwatch.GetTimestamp();
            await _ffmpeg.RunProcess(arguments, _priority, cancellationToken);
            var endTime = Stopwatch.GetTimestamp();

            return new ConversionResult
                   {
                       StartTime = startDateTime.Add(TimeSpan.FromTicks(startTime)),
                       EndTime = startDateTime.Add(TimeSpan.FromTicks(endTime)),
                       Duration = Stopwatch.GetElapsedTime(startTime, endTime),
                       Arguments = arguments,
                       OutputLog = string.Join(Environment.NewLine, _ffmpeg.OutputLog),
                   };
        }
        finally
        {
            _ffmpeg.OnProgress -= OnProgress;
            _ffmpeg.OnDataReceived -= OnDataReceived;
            _ffmpeg.OnVideoDataReceived -= OnVideoDataReceived;
            _ffmpeg = null;
        }
    }

    /// <inheritdoc />
    public IConversion AddParameter(string parameter, ArgumentPosition argumentPosition = ArgumentPosition.PostInput)
    {
        _arguments.Add(ConversionArgument.Create(parameter, argumentPosition));
        return this;
    }

    /// <inheritdoc />
    public IConversion AddStream<T>(T? stream) where T : IStream
    {
        if (stream is not null)
        {
            _streams.Add(stream);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion AddStreams(IEnumerable<IStream?> streams)
    {
        foreach (var stream in streams)
        {
            AddStream(stream);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetHashFormat(Hash hashFormat = Hash.SHA256)
    {
        SetOutputFormat(Format.hash);
        var format = hashFormat.ToStringFast(useMetadataAttributes: true);
        return SetHashFormat(format);
    }

    /// <inheritdoc />
    public IConversion SetHashFormat(string hashFormat)
    {
        _arguments.AddPostInput("hash", hashFormat);
        return this;
    }

    /// <inheritdoc />
    public IConversion SetPreset(ConversionPreset preset)
    {
        _arguments.AddPostInput("preset", preset);
        return this;
    }

    /// <inheritdoc />
    public IConversion SetSeek(TimeSpan? seek)
    {
        if (seek.HasValue)
        {
            _arguments.AddPostInput("ss", seek.Value);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetInputTime(TimeSpan? time)
    {
        if (time.HasValue)
        {
            _arguments.AddPreInput("t", time.Value);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetOutputTime(TimeSpan? time)
    {
        if (time.HasValue)
        {
            _arguments.AddPostInput("t", time.Value);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion UseMultiThread(bool multiThread)
    {
        var threads = multiThread ? Environment.ProcessorCount : 1;
        _arguments.AddPostInput("threads", Math.Min(threads, val2: 16));
        return this;
    }

    /// <inheritdoc />
    public IConversion UseMultiThread(int threadsCount)
    {
        _arguments.AddPostInput("threads", threadsCount);
        return this;
    }

    /// <inheritdoc />
    public IConversion SetOutput(MediaLocation mediaLocation)
    {
        OutputFilePath = mediaLocation;
        _output = mediaLocation;
        return this;
    }

    /// <inheritdoc />
    public IConversion PipeOutput(PipeDescriptor descriptor = PipeDescriptor.stdout)
    {
        SetOutput($"pipe:{descriptor.ToStringFast()}");
        OutputPipeDescriptor = descriptor;
        return this;
    }

    /// <inheritdoc />
    public IConversion SetVideoBitrate(long bitrate)
    {
        _arguments.AddPostInput("b:v", bitrate);
        _arguments.AddPostInput("minrate", bitrate);
        _arguments.AddPostInput("maxrate", bitrate);
        _arguments.AddPostInput("bufsize", bitrate);

        if (HasH264Stream())
        {
            _arguments.AddPostInput("x264opts", "nal-hrd=cbr:force-cfr=1");
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetAudioBitrate(long bitrate)
    {
        _arguments.AddPostInput("b:a", bitrate);
        return this;
    }

    /// <inheritdoc />
    public IConversion UseShortest(bool useShortest)
    {
        if (useShortest)
        {
            _arguments.AddPostInput("shortest", useShortest);
        }
        else
        {
            _arguments.Remove("shortest");
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetPriority(ProcessPriorityClass? priority)
    {
        _priority = priority;
        return this;
    }

    /// <inheritdoc />
    public IConversion ExtractEveryNthFrame(int frameNo, Func<string, MediaLocation> buildOutputFileName)
    {
        _buildOutputFileName = buildOutputFileName;
        _arguments.AddPostInput("vf", $"select='not(mod(n\\,{frameNo}))'");
        OutputFilePath = buildOutputFileName("").Escape();
        SetVideoSyncMethod(VideoSyncMethod.vfr);

        return this;
    }

    /// <inheritdoc />
    public IConversion ExtractNthFrame(int frameNo, Func<string, MediaLocation> buildOutputFileName)
    {
        _buildOutputFileName = buildOutputFileName;
        _arguments.AddPostInput("vf", $"select='eq(n\\,{frameNo})'");
        OutputFilePath = buildOutputFileName("").Escape();
        SetVideoSyncMethod(VideoSyncMethod.passthrough);

        return this;
    }

    /// <inheritdoc />
    public IConversion BuildVideoFromImages(int startNumber, Func<string, string> buildInputFileName)
    {
        _buildInputFileName = buildInputFileName;
        _arguments.AddPreInput("start_number", startNumber);
        return this;
    }

    /// <inheritdoc />
    public IConversion BuildVideoFromImages(IEnumerable<MediaLocation> imageFiles)
    {
        var builder = new InputBuilder();
        _buildInputFileName = builder.PrepareInputFiles(imageFiles, out _);

        return this;
    }

    /// <inheritdoc />
    public IConversion SetInputFramerate(double framerate)
    {
        _arguments.AddPreInput("framerate", framerate.ToFFmpegFormat(3));
        _arguments.AddPreInput("r", framerate.ToFFmpegFormat(3));
        return this;
    }

    /// <inheritdoc />
    public IConversion SetFramerate(double framerate)
    {
        _arguments.AddPostInput("framerate", framerate.ToFFmpegFormat(3));
        _arguments.AddPostInput("r", framerate.ToFFmpegFormat(3));
        return this;
    }

    /// <inheritdoc />
    public IConversion UseHardwareAcceleration(HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
    {
        _arguments.AddPreInput("hwaccel", hardwareAccelerator);
        _arguments.AddPreInput("c:v", decoder);
        _arguments.AddPostInput("c:v", encoder);

        if (device != 0)
        {
            _arguments.AddPreInput("hwaccel_device", device);
        }

        UseMultiThread(false);
        return this;
    }

    /// <inheritdoc />
    public IConversion SetOverwriteOutput(bool overwrite)
    {
        if (overwrite)
        {
            _arguments.AddPostInput("y");
            _arguments.Remove("n");
        }
        else
        {
            _arguments.AddPostInput("n");
            _arguments.Remove("y");
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetInputFormat(Format inputFormat)
    {
        var format = inputFormat.ToStringFast(useMetadataAttributes: true);
        return SetInputFormat(format);
    }

    /// <inheritdoc />
    public IConversion SetInputFormat(string? format)
    {
        if (format is not null)
        {
            _arguments.AddPreInput("f", format);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetOutputFormat(Format outputFormat)
    {
        var format = outputFormat.ToStringFast(useMetadataAttributes: true);
        return SetOutputFormat(format);
    }

    /// <inheritdoc />
    public IConversion SetOutputFormat(string? format)
    {
        if (format is not null)
        {
            _arguments.AddPostInput("f", format);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetPixelFormat(PixelFormat pixelFormat)
    {
        var format = pixelFormat.ToStringFast(useMetadataAttributes: true);
        return SetPixelFormat(format);
    }

    /// <inheritdoc />
    public IConversion SetPixelFormat(string? pixelFormat)
    {
        if (pixelFormat is not null)
        {
            _arguments.AddPostInput("pix_fmt", pixelFormat);
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion SetVideoSyncMethod(VideoSyncMethod method)
    {
        if (method == VideoSyncMethod.auto)
        {
            _arguments.AddPostInput("fps_mode", value: -1);
        }
        else
        {
            _arguments.AddPostInput("fps_mode", method.ToStringFast());
        }

        return this;
    }

    /// <inheritdoc />
    public IConversion AddDesktopStream(string? videoSize = null, double framerate = 30, int xOffset = 0, int yOffset = 0)
    {
        var (path, format) = GetPathAndFormat();
        var index = _streams.Count != 0 ? _streams.Max(x => x.Index) + 1 : 0;

        var stream = new VideoStream(path, index);

        stream.SetInputFormat(format);

        stream.Arguments.AddPreInput("framerate", framerate.ToFFmpegFormat(4));
        stream.Arguments.AddPreInput("offset_x", xOffset);
        stream.Arguments.AddPreInput("offset_y", yOffset);

        if (videoSize is not null)
        {
            stream.Arguments.AddPreInput("video_size", videoSize);
        }

        AddStream(stream);

        return this;

        (string Path, Format Format) GetPathAndFormat()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return (Path: "desktop", Format: Format.gdigrab);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return (Path: "1:1", Format: Format.avfoundation);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return (Path: ":0.0+0,0", Format: Format.x11grab);
            }

            throw new PlatformNotSupportedException();
        }
    }

    /// <inheritdoc />
    public IConversion AddDesktopStream(VideoSize videoSize, double framerate = 30, int xOffset = 0, int yOffset = 0)
    {
        return AddDesktopStream(videoSize.ToStringFast(useMetadataAttributes: true), framerate, xOffset, yOffset);
    }

    private void AppendStreams(StringBuilder builder, ArgumentPosition forPosition)
    {
        foreach (var stream in _streams)
        {
            builder.Append(' ');
            stream.AppendArguments(builder, forPosition);
        }
    }

    private void AppendFilters(StringBuilder builder)
    {
        var configurations = new List<IFilterConfiguration>();

        configurations.AddRange(_streams.SelectMany(stream => stream.GetFilters()));

        var filterGroups = configurations.GroupBy(configuration => configuration.FilterType);

        foreach (var filterGroup in filterGroups)
        {
            builder.Append($"{filterGroup.Key} \"");

            foreach (var configuration in configurations.Where(x => x.FilterType == filterGroup.Key))
            {
                var values = new List<string>();

                foreach (var filter in configuration.Filters)
                {
                    var map = $"[{configuration.StreamNumber}]";
                    var value = string.IsNullOrEmpty(filter.Value) ? $"{filter.Key} " : $"{filter.Key}={filter.Value}";
                    values.Add($"{map} {value} ");
                }

                builder.Append(string.Join(";", values));
            }

            builder.Append("\" ");
        }
    }

    /// <summary>
    ///     Create map for included streams, including the InputBuilder if required
    /// </summary>
    /// <returns>Map argument</returns>
    private void AddStreamMappings(StringBuilder builder)
    {
        foreach (var stream in _streams)
        {
            if (_hasInputBuilder) // If we have an input builder we always want to map the first video stream as it will be created by our input builder
            {
                builder.Append(" -map 0:0");
            }

            foreach (var source in stream.GetSource())
            {
                if (_hasInputBuilder)
                {
                    // If we have an input builder we need to add one to the input file index to account for the input created by our input builder.
                    builder.Append($" -map {_inputFileMap[source] + 1}:{stream.Index}");
                }
                else
                {
                    builder.Append($" -map {_inputFileMap[source]}:{stream.Index}");
                }
            }
        }
    }

    private void AppendParameters(StringBuilder builder, ArgumentPosition forPosition)
    {
        builder.Append(' ')
               .Append(string.Join(separator: ' ', _arguments.Get(forPosition).Select(x => x.Value)));
    }

    private void AppendOutputFilePath(StringBuilder builder)
    {
        var outputFilename = _buildOutputFileName?.Invoke("_%03d");

        if (outputFilename is null)
        {
            return;
        }

        var directoryName = Path.GetDirectoryName(outputFilename);

        if (directoryName is not null)
        {
            Directory.CreateDirectory(directoryName);
        }

        builder.Append(' ')
               .Append(outputFilename.Escape());
    }

    /// <summary>
    ///     Create input string for all streams
    /// </summary>
    /// <returns>Input argument</returns>
    private void AppendInputs(StringBuilder builder)
    {
        var index = 0;

        foreach (var source in _streams.SelectMany(x => x.GetSource()).Distinct())
        {
            _inputFileMap[source] = index++;
            builder.Append($" -i {source.Escape()}");
        }
    }

    private bool HasH264Stream()
    {
        return _streams.Any(stream => stream is IVideoStream videoStream && (videoStream.Codec.Equals(VideoCodec.Libx264) || videoStream.Codec.Equals(VideoCodec.h264)));
    }

    internal static IConversion Create()
    {
        return new Conversion().SetOverwriteOutput(false);
    }
}
