namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when a FFmpeg cannot find specified hardware accelerator.
/// </summary>
[PublicAPI]
public sealed class HardwareAcceleratorNotFoundException : ConversionException
{
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when a FFmpeg cannot find specified hardware accelerator.
    /// </summary>
    /// <param name="message">FFmpeg error output</param>
    /// <param name="arguments">FFmpeg input parameters</param>
    internal HardwareAcceleratorNotFoundException(string message, string arguments)
        : base(message, arguments)
    {
    }

    internal static HardwareAcceleratorNotFoundException Create(string errorMessage, string inputParameters)
    {
        return new HardwareAcceleratorNotFoundException(errorMessage, inputParameters);
    }
}
