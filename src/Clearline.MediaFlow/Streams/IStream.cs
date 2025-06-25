namespace Clearline.MediaFlow;

using System.Text;

/// <summary>
///     Base stream class
/// </summary>
[PublicAPI]
public interface IStream
{
    /// <summary>
    ///     Gets the path of the stream
    /// </summary>
    string Path { get; }

    /// <summary>
    ///    Gets the index of the stream
    /// </summary>
    int Index { get; }

    /// <summary>
    ///     Gets the stream codec
    /// </summary>
    Codec Codec { get; }

    /// <summary>
    ///     Gets the language of stream
    /// </summary>
    string? Language { get; }

    /// <summary>
    ///     Gets the duration of the stream
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    ///     Gets the bitrate of the stream in bits per second
    /// </summary>
    long Bitrate { get; }

    /// <summary>
    ///     Gets the title of the stream.
    /// </summary>
    string? Title { get; }

    /// <summary>
    ///     Get a value indicating whether the stream is default or not.
    /// </summary>
    bool? IsDefault { get; }

    /// <summary>
    ///     Gets a value indicating whether the stream is forced or not.
    /// </summary>
    bool? IsForced { get; }

    internal void AppendArguments(StringBuilder builder, ArgumentPosition forPosition);

    internal IEnumerable<IFilterConfiguration> GetFilters();

    /// <summary>
    ///     Get stream input
    /// </summary>
    /// <returns>Input path</returns>
    IEnumerable<MediaLocation> GetSource();
}
