namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when a FFmpeg process cannot find suitable output format.
/// </summary>
[PublicAPI]
public sealed class InvalidBitstreamFilterException : ConversionException
{
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when a FFmpeg process cannot find suitable output format.
    /// </summary>
    /// <param name="message">FFmpeg error output</param>
    /// <param name="arguments">FFmpeg error output</param>
    internal InvalidBitstreamFilterException(string message, string arguments)
        : base(message, arguments)
    {
    }

    internal static InvalidBitstreamFilterException Create(string errorMessage, string inputParameters)
    {
        return new InvalidBitstreamFilterException(errorMessage, inputParameters);
    }
}
