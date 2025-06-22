namespace Clearline.MediaFlow.Exceptions;

public sealed class GenericConversionException : ConversionException
{
    internal GenericConversionException(string message, string arguments)
        : base(message, arguments)
    {
    }

    public static GenericConversionException Create(string message, string arguments)
    {
        return new GenericConversionException(message, arguments);
    }
}
