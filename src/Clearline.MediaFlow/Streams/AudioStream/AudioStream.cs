namespace Clearline.MediaFlow;

using Probe.Models;

[PublicAPI]
internal sealed class AudioStream : StreamBase<IAudioStream>, IAudioStream
{
    internal AudioStream(AudioStreamModel streamModel, FormatModel formatModel)
        : base(streamModel, formatModel)
    {
        Channels = streamModel.Channels;
        ChannelLayout = streamModel.ChannelLayout;
        SampleRate = streamModel.SampleRate;
    }

    /// <inheritdoc />
    public int Channels { get; }

    /// <inheritdoc />
    public string? ChannelLayout { get; }

    public new AudioCodec Codec => base.Codec;

    /// <inheritdoc />
    public int SampleRate { get; }

    /// <inheritdoc />
    public IAudioStream Reverse()
    {
        Arguments.AddPostInput("af", "areverse");
        return this;
    }

    /// <inheritdoc />
    public IAudioStream Split(TimeSpan startTime, TimeSpan duration)
    {
        Arguments.AddPostInput("ss", startTime);
        Arguments.AddPostInput("t", duration);
        return this;
    }

    /// <inheritdoc />
    public override IAudioStream CopyStream()
    {
        return SetCodec(AudioCodec.Copy);
    }

    /// <inheritdoc />
    public IAudioStream SetChannels(int channels)
    {
        Arguments.AddPostInput($"ac:{Index}", channels);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetBitstreamFilter(BitstreamFilter filter)
    {
        return SetBitstreamFilter(filter.ToStringFast(useMetadataAttributes: true));
    }

    /// <inheritdoc />
    public IAudioStream SetBitstreamFilter(string filter)
    {
        Arguments.AddPostInput("bsf:a", filter);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetBitrate(long bitRate)
    {
        Arguments.AddPostInput($"b:a:{Index}", bitRate);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize)
    {
        Arguments.AddPostInput($"b:a:{Index}", minBitrate);
        Arguments.AddPostInput("maxrate", maxBitrate);
        Arguments.AddPostInput("bufsize", bufferSize);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetSampleRate(int sampleRate)
    {
        Arguments.AddPostInput($"ar:{Index}", sampleRate);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream ChangeSpeed(double multiplier)
    {
        Filters.Add(GetAudioSpeedFilter(multiplier));
        return this;
    }

    public IAudioStream SetCodec(AudioCodec codec)
    {
        Arguments.AddPostInput("c:a", codec);
        return this;
    }

    public override IAudioStream SetCodec(Codec codec)
    {
        return SetCodec(codec);
    }

    /// <inheritdoc />
    public IAudioStream SetSeek(TimeSpan seek)
    {
        Arguments.AddPreInput("ss", seek);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetInputFormat(string inputFormat)
    {
        Arguments.AddPreInput("f", inputFormat);
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetInputFormat(Format inputFormat)
    {
        return SetInputFormat(inputFormat.ToStringFast(useMetadataAttributes: true));
    }

    /// <inheritdoc />
    public IAudioStream UseNativeInputRead(bool readInputAtNativeFrameRate)
    {
        Arguments.AddPreInput("re");
        return this;
    }

    /// <inheritdoc />
    public IAudioStream SetStreamLoop(int loopCount)
    {
        Arguments.AddPreInput("stream_loop", loopCount);
        return this;
    }

    public override IEnumerable<IFilterConfiguration> GetFilters()
    {
        if (Filters.Count == 0)
        {
            yield break;
        }

        yield return new FilterConfiguration(Index, "-filter:a", Filters);
    }

    public override IAudioStream SetLanguage(string? lang)
    {
        SetLanguage(streamType: 'a', lang);
        return this;
    }

    private static Filter GetAudioSpeedFilter(double multiplier)
    {
        if (multiplier is < 0.5 or > 2.0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Value has to be greater than 0.5 and less than 2.0.");
        }

        return new Filter("atempo", $"{multiplier.ToFFmpegFormat()}");
    }
}
