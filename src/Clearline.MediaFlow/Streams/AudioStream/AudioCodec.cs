namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct AudioCodec(string? Name)
{
    public static implicit operator AudioCodec(CodecName codec)
    {
        return new AudioCodec(codec.Name);
    }

    public static implicit operator AudioCodec(string? name)
    {
        return new AudioCodec(name);
    }

    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}
