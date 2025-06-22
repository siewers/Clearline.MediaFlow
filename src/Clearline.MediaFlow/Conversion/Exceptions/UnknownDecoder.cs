namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when a FFmpeg cannot find specified hardware accelerator.
/// </summary>
[PublicAPI]
public sealed class UnknownDecoderException : ConversionException
{
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when a FFmpeg cannot find a codec to decode the file.
    /// </summary>
    /// <param name="message">FFmpeg error output</param>
    /// <param name="arguments">FFmpeg input parameters</param>
    private UnknownDecoderException(string message, string arguments)
        : base(message, arguments)
    {
    }

    internal static UnknownDecoderException Create(string errorMessage, string inputParameters)
    {
        return new UnknownDecoderException(errorMessage, inputParameters);
    }
}
