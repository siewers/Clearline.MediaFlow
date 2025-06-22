namespace Clearline.MediaFlow;

public interface IStream<out TStream> : IStream
    where TStream : IStream<TStream>
{
    /// <summary>
    ///     Set stream to copy with original codec
    /// </summary>
    TStream CopyStream();

    /// <summary>
    ///     Set codec for stream
    /// </summary>
    TStream SetCodec(CodecName codec);
}
