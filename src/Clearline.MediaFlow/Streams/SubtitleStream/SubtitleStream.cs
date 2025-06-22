namespace Clearline.MediaFlow;

using Probe.Models;

[PublicAPI]
internal sealed class SubtitleStream : StreamBase<ISubtitleStream>, ISubtitleStream
{
    internal SubtitleStream(StreamModelBase streamModel, FormatModel formatModel)
        : base(streamModel, formatModel)
    {
    }

    public new SubtitleCodec Codec => base.Codec;

    public override ISubtitleStream SetLanguage(string? lang)
    {
        SetLanguage(streamType: 's', lang);
        return this;
    }

    public override ISubtitleStream CopyStream()
    {
        return SetCodec(SubtitleCodec.copy);
    }

    public override ISubtitleStream SetCodec(CodecName name)
    {
        return SetCodec(name);
    }

    public ISubtitleStream SetCodec(SubtitleCodec codec)
    {
        Arguments.AddPostInput("c:s", codec);
        return this;
    }

    /// <inheritdoc />
    public ISubtitleStream UseNativeInputRead(bool readInputAtNativeFrameRate)
    {
        Arguments.AddPreInput("re");
        return this;
    }

    /// <inheritdoc />
    public ISubtitleStream SetStreamLoop(int loopCount)
    {
        Arguments.AddPreInput("stream_loop", loopCount);
        return this;
    }

    public override IEnumerable<FilterConfiguration> GetFilters()
    {
        yield break;
    }
}
