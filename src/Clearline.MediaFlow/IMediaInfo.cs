namespace Clearline.MediaFlow;

/// <summary>
///     Information about media file
/// </summary>
[PublicAPI]
public interface IMediaInfo
{
    /// <summary>
    ///     Gets the source media location
    /// </summary>
    MediaLocation Location { get; }

    /// <summary>
    ///     Gets the time of when the media was created
    /// </summary>
    DateTime? CreationTime { get; }

    /// <summary>
    ///     Gets the size of file in bytes
    /// </summary>
    long Size { get; }

    /// <summary>
    ///     Gets the duration of media
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    ///     Gets all streams in the media file
    /// </summary>
    IEnumerable<IStream> Streams { get; }

    /// <summary>
    ///     Gets video streams in the media file
    /// </summary>
    IEnumerable<IVideoStream> VideoStreams { get; }

    /// <summary>
    ///     Gets audio streams in the media file
    /// </summary>
    IEnumerable<IAudioStream> AudioStreams { get; }

    /// <summary>
    ///     Gets subtitle streams in the media file
    /// </summary>
    IEnumerable<ISubtitleStream> SubtitleStreams { get; }
}
