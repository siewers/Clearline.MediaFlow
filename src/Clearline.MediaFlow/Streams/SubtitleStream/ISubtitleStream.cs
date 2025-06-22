namespace Clearline.MediaFlow;

/// <summary>
///     Subtitle stream
/// </summary>
[PublicAPI]
public interface ISubtitleStream : IStream<ISubtitleStream>
{
    new SubtitleCodec Codec { get; }

    /// <summary>
    ///     Set subtitle language
    /// </summary>
    /// <param name="language">Language</param>
    /// <returns>ISubtitleStream</returns>
    ISubtitleStream SetLanguage(string? language);

    /// <summary>
    ///     Set subtitle codec
    /// </summary>
    /// <param name="codec">Subtitle codec</param>
    /// <returns>ISubtitleStream</returns>
    ISubtitleStream SetCodec(SubtitleCodec codec);

    /// <summary>
    ///     "-re" parameter. Read input at native frame rate. Mainly used to simulate a grab device, or live input stream (e.g.
    ///     when reading from a file). Should not be used with actual grab devices or live input streams (where it can cause
    ///     packet loss). By default, ffmpeg attempts to read the input(s) as fast as possible. This option will slow down the
    ///     reading of the input(s) to the native frame rate of the input(s). It is useful for real-time output (e.g.
    ///     livestreaming).
    /// </summary>
    /// <param name="readInputAtNativeFrameRate">Read input at native frame rate. False set parameter to default value.</param>
    /// <returns>IConversion object</returns>
    ISubtitleStream UseNativeInputRead(bool readInputAtNativeFrameRate);

    /// <summary>
    ///     "-stream_loop" parameter. Set number of times input stream shall be looped.
    /// </summary>
    /// <param name="loopCount">Loop 0 means no loop, loop -1 means infinite loop.</param>
    /// <returns>IConversion object</returns>
    ISubtitleStream SetStreamLoop(int loopCount);
}
