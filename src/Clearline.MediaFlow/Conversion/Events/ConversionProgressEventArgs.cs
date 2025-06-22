namespace Clearline.MediaFlow.Events;

/// <summary>
///     Conversion information
/// </summary>
[PublicAPI]
public readonly record struct ConversionProgressEventArgs
{
    internal ConversionProgressEventArgs(long processId, TimeSpan duration, TimeSpan totalLength)
    {
        ProcessId = processId;
        Duration = duration;
        TotalLength = totalLength;
    }

    /// <summary>
    ///    Gets the ID of the conversion process.
    /// </summary>
    public long ProcessId { get; }

    /// <summary>
    ///     Current processing time
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///     Gets the total length of the media being processed.
    /// </summary>
    public TimeSpan TotalLength { get; }

    /// <summary>
    ///    Gets the percentage of the conversion that has been completed.
    /// </summary>
    public int Percent => (int)(Math.Round(Duration.TotalSeconds / TotalLength.TotalSeconds, 2) * 100);
}
