namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct SubtitleCodec(string? Name)
{
    public static implicit operator SubtitleCodec(Codec codec)
    {
        return new SubtitleCodec(codec.Name);
    }

    public static implicit operator SubtitleCodec(string? name)
    {
        return new SubtitleCodec(name);
    }

    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}
