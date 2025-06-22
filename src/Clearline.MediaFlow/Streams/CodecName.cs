namespace Clearline.MediaFlow;

public readonly record struct CodecName(string? Name) // : IEquatable<AudioCodecName>, IEquatable<VideoCodecName>, IEquatable<SubtitleCodecName>
{
    // public bool Equals(AudioCodecName other)
    // {
    //     return Name == other.Name;
    // }
    //
    // public bool Equals(VideoCodecName other)
    // {
    //     return Name == other.Name;
    // }
    //
    // public bool Equals(SubtitleCodecName other)
    // {
    //     return Name == other.Name;
    // }
    //
    // public override string ToString()
    // {
    //     return Name ?? string.Empty;
    // }
    //
    // public static implicit operator CodecName(AudioCodecName codec)
    // {
    //     return new CodecName(codec.Name);
    // }
    //
    // public static implicit operator CodecName(VideoCodecName codec)
    // {
    //     return new CodecName(codec.Name);
    // }
    //
    // public static implicit operator CodecName(SubtitleCodecName codec)
    // {
    //     return new CodecName(codec.Name);
    // }

    public static implicit operator CodecName(string? name)
    {
        return new CodecName(name);
    }
}
