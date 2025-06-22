namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when a FFmpeg process cannot find suitable output format.
/// </summary>
[PublicAPI]
public sealed class FFmpegNoSuitableOutputFormatFoundException : ConversionException
{
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when a FFmpeg process cannot find suitable output format.
    /// </summary>
    /// <param name="message">FFmpeg error output</param>
    /// <param name="arguments">FFmpeg input parameters</param>
    internal FFmpegNoSuitableOutputFormatFoundException(string message, string arguments)
        : base(message, arguments)
    {
    }

    internal static FFmpegNoSuitableOutputFormatFoundException Create(string errorMessage, string inputParameters)
    {
        return new FFmpegNoSuitableOutputFormatFoundException(errorMessage, inputParameters);
    }
}
