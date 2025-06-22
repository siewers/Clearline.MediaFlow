namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when a FFmpeg process return error.
/// </summary>
[PublicAPI]
public class ConversionException : Exception
{
    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when an FFmpeg process return error.
    /// </summary>
    /// <param name="message">The FFmpeg error message</param>
    /// <param name="arguments">The FFmpeg input parameters</param>
    /// <param name="innerException">The inner exception</param>
    protected internal ConversionException(string message, Exception innerException, string arguments)
        : base(message, innerException)
    {
        Arguments = arguments;
    }

    /// <inheritdoc />
    /// <summary>
    ///     The exception that is thrown when an FFmpeg process return error.
    /// </summary>
    /// <param name="message">FFmpeg error output</param>
    /// <param name="arguments">FFmpeg input parameters</param>
    internal ConversionException(string message, string arguments)
        : base(message)
    {
        Arguments = arguments;
    }

    /// <summary>
    ///     Gets the FFmpeg process arguments
    /// </summary>
    public string Arguments { get; }

    public override string Message => $"""
                                       {base.Message}

                                       FFmpeg arguments: {Arguments.Trim()}
                                       """;
}
