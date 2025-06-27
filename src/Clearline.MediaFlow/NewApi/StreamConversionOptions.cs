namespace Clearline.MediaFlow.NewApi;

public abstract class StreamConversionOptions<TStream>(TStream stream)
    where TStream : IStream
{
    private readonly ConversionArguments _arguments = [];
    private bool _isCopyStream;

    internal TStream Stream { get; } = stream;

    internal FilterCollection Filters { get; } = [];

    internal void AddPreInputArgument(string argument)
    {
        if (_isCopyStream)
        {
            throw new InvalidOperationException("Cannot add pre-input arguments when stream is set to copy.");
        }

        _arguments.AddPreInput(argument);
    }

    internal void AddPreInputArgument<T>(string name, T value)
    {
        if (_isCopyStream)
        {
            throw new InvalidOperationException("Cannot add pre-input arguments when stream is set to copy.");
        }

        _arguments.AddPreInput(name, value);
    }

    internal void AddPostInputArgument(string name)
    {
        if (_isCopyStream)
        {
            throw new InvalidOperationException("Cannot add post-input arguments when stream is set to copy.");
        }

        _arguments.AddPostInput(name);
    }

    internal void AddArgument(ConversionArgument argument)
    {
        if (_isCopyStream)
        {
            throw new InvalidOperationException("Cannot add arguments when stream is set to copy.");
        }

        _arguments.Add(argument);
    }

    internal void AddPostInputArgument<T>(string name, T value)
    {
        if (_isCopyStream)
        {
            throw new InvalidOperationException("Cannot add post-input arguments when stream is set to copy.");
        }

        _arguments.AddPostInput(name, value);
    }

    internal void CopyStream(char streamType)
    {
        if (_arguments.Any())
        {
            throw new InvalidOperationException("Cannot copy stream when arguments are already set.");
        }

        if (_isCopyStream)
        {
            throw new InvalidOperationException("Stream is already set to copy.");
        }

        _isCopyStream = true;
        _arguments.AddPostInput($"c:{streamType}", "copy");
    }

    internal void SetLanguage(char streamType, string? lang)
    {
        var language = !string.IsNullOrEmpty(lang) ? lang : Stream.Language;

        if (string.IsNullOrEmpty(language))
        {
            return;
        }

        if (language.Length > 3)
        {
            throw new ArgumentException("Language code must be 3 characters.", nameof(lang));
        }

        AddPostInputArgument($"metadata:{streamType}:s:{Stream.Index}", $"language={language}");
    }
}
