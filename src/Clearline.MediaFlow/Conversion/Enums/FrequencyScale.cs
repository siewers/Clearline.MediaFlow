// ReSharper disable InconsistentNaming

namespace Clearline.MediaFlow;

[PublicAPI]
public readonly partial record struct FrequencyScale(string Name)
{
    /// <summary>
    ///     Implicitly converts a <see cref="FrequencyScale" /> to a string.
    /// </summary>
    /// <param name="scale">The frequency scale name.</param>
    public static implicit operator string(FrequencyScale scale)
    {
        return scale.Name;
    }

    /// <summary>
    ///     Implicitly converts a string to a <see cref="FrequencyScale" />.
    /// </summary>
    /// <param name="name">The name of the frequency scale.</param>
    public static implicit operator FrequencyScale(string name)
    {
        return new FrequencyScale(name);
    }

    public override string ToString()
    {
        return Name;
    }
}
