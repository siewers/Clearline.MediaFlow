namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct ConversionPreset(string Name)
{
    /// <summary>
    ///     Implicit conversion from string to ConversionPreset.
    /// </summary>
    public static implicit operator ConversionPreset(string name)
    {
        return new ConversionPreset(name);
    }

    public override string ToString()
    {
        return Name;
    }
}
