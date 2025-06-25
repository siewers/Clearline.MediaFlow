namespace Clearline.MediaFlow;

public readonly record struct Codec(string? Name)
{
    public static implicit operator Codec(string? name)
    {
        return new Codec(name);
    }
}
