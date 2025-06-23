﻿namespace Clearline.MediaFlow.Exceptions;

/// <inheritdoc />
/// <summary>
///     The exception that is thrown when input does not exists.
/// </summary>
[PublicAPI]
public sealed class InvalidInputException : FileNotFoundException
{
    /// <summary>
    ///     The exception that is thrown when input does not exists.
    /// </summary>
    /// <param name="msg"></param>
    public InvalidInputException(string msg)
        : base(msg)
    {
    }
}
