namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct HardwareAccelerator(string Name)
{
    /// <summary>
    ///     Implicitly converts a <see cref="HardwareAccelerator" /> to a string.
    /// </summary>
    /// <param name="accelerator">The hardware accelerator name.</param>
    public static implicit operator string(HardwareAccelerator accelerator)
    {
        return accelerator.Name;
    }

    /// <summary>
    ///     Implicitly converts a string to a <see cref="HardwareAccelerator" />.
    /// </summary>
    /// <param name="name">The name of the hardware accelerator.</param>
    public static implicit operator HardwareAccelerator(string name)
    {
        return new HardwareAccelerator(name);
    }

    public override string ToString()
    {
        return Name;
    }
}
