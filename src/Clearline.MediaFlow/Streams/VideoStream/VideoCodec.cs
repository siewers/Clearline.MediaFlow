namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct VideoCodec(string? Name)
{
    public static implicit operator VideoCodec(CodecName codec)
    {
        return new VideoCodec(codec.Name);
    }

    public static implicit operator VideoCodec(string name)
    {
        return new VideoCodec(name);
    }

    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}
