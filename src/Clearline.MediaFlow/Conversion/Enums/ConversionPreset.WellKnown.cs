namespace Clearline.MediaFlow;

/// <summary>
///     Represents a conversion preset for media processing.
/// </summary>
public readonly partial record struct ConversionPreset
{
    /// <summary>
    ///     Very slow preset, used for high-quality conversions.
    /// </summary>
    public static readonly ConversionPreset VerySlow = new("veryslow");

    /// <summary>
    ///     Slower preset, used for high-quality conversions.
    /// </summary>
    public static readonly ConversionPreset Slower = new("slower");

    /// <summary>
    ///     Slow preset, used for high-quality conversions.
    /// </summary>
    public static readonly ConversionPreset Slow = new("slow");

    /// <summary>
    ///     Medium preset, used for a balance between speed and quality.
    /// </summary>
    public static readonly ConversionPreset Medium = new("medium");

    /// <summary>
    ///     Fast preset, used for faster conversions with some quality loss.
    /// </summary>
    public static readonly ConversionPreset Fast = new("fast");

    /// <summary>
    ///     Faster preset, used for faster conversions with more quality loss.
    /// </summary>
    public static readonly ConversionPreset Faster = new("faster");

    /// <summary>
    ///     Very fast preset, used for very fast conversions with significant quality loss.
    /// </summary>
    public static readonly ConversionPreset VeryFast = new("veryfast");

    /// <summary>
    ///     Superfast preset, used for extremely fast conversions with substantial quality loss.
    /// </summary>
    public static readonly ConversionPreset Superfast = new("superfast");

    /// <summary>
    ///     Ultra-fast preset, used for the fastest conversions with maximum quality loss.
    /// </summary>
    public static readonly ConversionPreset UltraFast = new("ultrafast");
}
